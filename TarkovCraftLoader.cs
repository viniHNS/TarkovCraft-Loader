using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;

namespace TarkovCraftLoader;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.vinihns.tarkovcraftloader";
    public override string Name { get; init; } = "TarkovCraft Loader";
    public override string Author { get; init; } = "ViniHNS";
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0"); 
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Contributors { get; init; }
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; } = "https://github.com/viniHNS/TarkovCraft-Loader";
    public override bool? IsBundleMod { get; init; } = false;
    public override string License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class Mod(ISptLogger<Mod> logger, DatabaseService databaseService) : IOnLoad
{
    public Task OnLoad()
    {
        logger.Info("[ViniHNS] TarkovCraft Loader initialized.");

        var hideout = databaseService.GetHideout();
        var recipes = hideout.Production.Recipes;

        if (recipes == null)
        {
            logger.Error("[ViniHNS] Recipes not found.");
            return Task.CompletedTask;
        }

        string assemblyPath = Assembly.GetExecutingAssembly().Location;
        string? modDirectory = Path.GetDirectoryName(assemblyPath);

        if (modDirectory == null)
        {
            logger.Error("[ViniHNS] Could not resolve mod directory.");
            return Task.CompletedTask;
        }

        string recipesFolder = Path.Combine(modDirectory, "recipes");

        if (!Directory.Exists(recipesFolder))
        {
            logger.Warning("[ViniHNS] 'recipes' folder not found. Skipping.");
            return Task.CompletedTask;
        }

        string[] parseFiles = Directory.GetFiles(recipesFolder, "*.json");
        int loadedCount = 0;

        var options = new JsonSerializerOptions();
        options.Converters.Add(new MongoIdConverter());

        foreach (string file in parseFiles)
        {
            try
            {
                string jsonContent = File.ReadAllText(file);

                var parsedList = JsonSerializer.Deserialize(jsonContent, recipes.GetType(), options);

                if (parsedList is IList listToLoad && recipes is IList destinationList)
                {
                    foreach (var item in listToLoad)
                    {
                        destinationList.Add(item);
                        loadedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"[ViniHNS] Failed parsing JSON file: {Path.GetFileName(file)} -> {ex.Message}");
            }
        }

        logger.Info($"[ViniHNS] Loaded {loadedCount} custom recipes.");

        return Task.CompletedTask;
    }
}

public class MongoIdConverter : System.Text.Json.Serialization.JsonConverter<SPTarkov.Server.Core.Models.Common.MongoId>
{
    public override SPTarkov.Server.Core.Models.Common.MongoId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? stringValue = reader.GetString();
        if (stringValue != null)
        {
            return new SPTarkov.Server.Core.Models.Common.MongoId(stringValue);
        }
        return default;
    }

    public override void Write(Utf8JsonWriter writer, SPTarkov.Server.Core.Models.Common.MongoId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}


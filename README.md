# TarkovCraft Loader

## What is this?

This is a mod for [SPT](https://www.sp-tarkov.com "The project's main goal is to provide a separate offline singleplayer experience with progression out-of-the-box for BSG's official client. You can now play Escape From Tarkov while you're waiting for their servers to get back online, while you're disconnected from the internet or if you need to take a break from the cheaters.") that allows you to easily load custom hideout production recipes from JSON files!

## What does this mod do?

You can add your own hideout production recipes by creating JSON files in the `recipes` folder.

- Automatically scanning a `recipes` folder for any `.json` files.
- Injecting the custom recipes directly into the game's database on server load.
- Seamlessly integrating with the existing Tarkov hideout system.

#### Example of a recipe file:

```json
[
    // Remove this comments and the example items to add your own recipes. JSON doesn't support comments.
    // You can add your own recipes here.
    // Item 1
  {
    "_id": "69b5875e49611de56bd2a916",
    "areaType": 2,
    "requirements": [
      {
        "areaType": 2,
        "requiredLevel": 1,
        "type": "Area"
      },
      {
        "templateId": "5447a9cd4bdc2dbd208b4567",
        "count": 1,
        "isFunctional": false,
        "isEncoded": false,
        "type": "Item"
      },
      {
        "templateId": "5448ba0b4bdc2d02308b456c",
        "type": "Tool"
      }
    ],
    "productionTime": 18000,
    "needFuelForAllProductionTime": false,
    "locked": false,
    "endProduct": "5448bd6b4bdc2dfc2f8b4569",
    "continuous": false,
    "count": 1,
    "productionLimitCount": 0,
    "isEncoded": false,
    "isCodeProduction": false
  },

  // Item 2
  {
    "_id": "69b587747d78ef83bb3d0155",
    "areaType": 10,
    "requirements": [
      {
        "areaType": 10,
        "requiredLevel": 2,
        "type": "Area"
      },
      {
        "templateId": "5448be9a4bdc2d02308b456c",
        "count": 4,
        "isFunctional": false,
        "isEncoded": false,
        "type": "Item"
      }
    ],
    "productionTime": 57000,
    "needFuelForAllProductionTime": false,
    "locked": false,
    "endProduct": "5448fee04bdc2dbc018b4567",
    "continuous": false,
    "count": 1,
    "productionLimitCount": 0,
    "isEncoded": false,
    "isCodeProduction": false
  }
  // You can add more recipes here.
]
```

You can use my website to generate recipes more easily: https://vinihns.github.io/TarkovCraft/

## Installation

1. Download the latest release `.zip` file.
2. Open the zip file.
3. Drag and drop the files directly into the root folder of your SPT installation.
4. The folders should merge automatically. If you get a prompt to overwrite files, say yes.

## Issues

- Recipes with invalid item IDs or malformed JSON structure will fail to load and will log errors in the server console.
- Ensure that you are using the correct `MongoId` format for IDs to avoid deserialization errors.
- **Hideout Performance**: Adding a large number of recipes to a single area (e.g., Lavatory, Medstation) can cause a temporary freeze or "stutter" when you first open that area in the hideout. This happens because the game client takes time to process and render all available craft options at once.

## License

This mod is licensed under the [MIT License](LICENSE).

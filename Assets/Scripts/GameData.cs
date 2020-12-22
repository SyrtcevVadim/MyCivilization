using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameData:MonoBehaviour 
{

    public static GameObject WarriorPrefab;
    public static GameObject WorkerPrefab;

    /// <summary>
    /// Генератор псевдо-случайных чисел.
    /// </summary>
    private static System.Random psevdoRandomNumberGenerator;

    /// <summary>
    /// Счетчик номера текущего хода
    /// </summary>
    public static uint currentTurnCounter = 1;

    /// <summary>
    /// Слой ландшафта игрового поля
    /// </summary>
    public static Tilemap terrainLayer;
    /// <summary>
    /// Слой городов игрового поля
    /// </summary>
    public static Tilemap cityLayer;
    /// <summary>
    /// Слой территории игрового поля
    /// </summary>
    public static Tilemap territoryLayer;
    /// <summary>
    /// Слой для отображения клеток возможных ходов юнита на игровом поле
    /// </summary>
    public static Tilemap movementLayer;
    /// <summary>
    /// 
    /// </summary>
    public static Tilemap selectTileLayer;

    /// <summary>
    /// Тайл города-столицы
    /// </summary>
    public static Tile capitalCityTile;
    /// <summary>
    /// Тайл города-провинции
    /// </summary>
    public static Tile provincialCityTile;
    /// <summary>
    /// Тайл юнита-человека
    /// </summary>
    public static Tile initialUnitTile;
    /// <summary>
    /// Тайл для представления территории игрока
    /// </summary>
    public static Tile territoryTile;
    /// <summary>
    /// Тайл для отображения ячеек, в которые юнит может пойти
    /// </summary>
    public static Tile tileForMovingIn;
    /// <summary>
    /// Тайл выбранного юнита-человека.
    /// </summary>
    public static Tile selectedUnitTile;

    public static Tile enemyUnitTile;
    public static Tile enemyCity;

    public static Tile selectedTile;

    /// <summary>
    /// Список имен тайлов, которые требуют 2 очка действия для передвижения в них
    /// </summary>
    public static string[] tilesRequired2AP = { "DeciduousForest1Glade",  "DeciduousForest2Glade", "SpruceForest1Glade", "SpruceForest2Glade",
"DeciduousForest1Dirt","DeciduousForest2Dirt","SpruceForest1Dirt","SpruceForest2Dirt", "SandstoneDirt","SandstoneGlade","Stone1Dirt","Stone1Glade","Stone2Dirt",
"Forest1Mars","Forest2Mars","Stone1Mars","Stone2Mars", "Stone3Mars","StoneCrystalMars","Cactus1Desert","Cactus2Desert","Cactus3Desert",
"Cactus4Desert","Stone1Desert","Stone2Desert","Stone3Desert"};
    /// <summary>
    /// Список имен тайлов, которые трубуют 1 очко действия для передвижения в них
    /// </summary>
    public static string[] tilesRequired1AP = { "Glade", "Dirt","Mars","CrystalsMars","Desert"};

    /// <summary>
    /// Список возможных имен для юнитов в игре.
    /// </summary>
    private static string[] possibleUnitNames = { "Vadim", "Katya", "Vova","Ksysha", "Sanya","Ilya","Yare-k","Chort"};
    
    /// <summary>
    /// Генерирует случайное имя юнита-человека.
    /// </summary>
    /// <returns>Возвращает сгенерированное имя.</returns>
    public static string GetRandomHumanName()
    {
        return possibleUnitNames[psevdoRandomNumberGenerator.Next(0, possibleUnitNames.Length)];
    }

    private void Awake()
    {
        psevdoRandomNumberGenerator = new System.Random();
        // Получаем все слои игрового поля
        terrainLayer = GameObject.Find("TerrainLayer").GetComponent<Tilemap>();     // Получаем слой ландшафта
        cityLayer = GameObject.Find("CityLayer").GetComponent<Tilemap>();           // Получаем слой городов
        territoryLayer = GameObject.Find("TerritoryLayer").GetComponent<Tilemap>(); // Получаем слой территории
        movementLayer = GameObject.Find("MovementLayer").GetComponent<Tilemap>();   // Получаем слой возможных передвижений юнита
        selectTileLayer = GameObject.Find("SelectTileLayer").GetComponent<Tilemap>();
        // Получаем все возможные тайлы
        capitalCityTile = Resources.Load<Tile>(@"Palettes\CityLayerPalette\CapitalCity");       // Получаем тайл города-столицы
        provincialCityTile = Resources.Load<Tile>(@"Palettes\CityLayerPalette\ProvincialCity"); // Получаем тайл города-провинции
        initialUnitTile = Resources.Load<Tile>(@"Palettes\UnitLayerPalette\InitialUnit");                // Получаем тайл юнита-человека
        selectedUnitTile = Resources.Load<Tile>(@"Palettes\UnitLayerPalette\SelectedUnit");

        enemyUnitTile = Resources.Load<Tile>(@"Palettes\UnitLayerPalette\EnemyUnit");
        enemyCity = Resources.Load<Tile>(@"Palettes\CityLayerPalette\EnemyCity");

        territoryTile = Resources.Load<Tile>(@"Palettes\TerritoryLayerPalette\TerritoryTile");  // Получаем тайл территории
        tileForMovingIn = Resources.Load<Tile>(@"Palettes\MovementLayerPalette\TileForMovingIn");   // Получаем тайл для отображения клеток для возможного перемещения
        selectedTile = Resources.Load<Tile>(@"Palettes\SelectTilePalette\SelectedTile");

        WarriorPrefab = Resources.Load<GameObject>(@"UnitPrefabs\Warrior");
        WorkerPrefab = Resources.Load<GameObject>(@"UnitPrefabs\Redneck");

    }
}

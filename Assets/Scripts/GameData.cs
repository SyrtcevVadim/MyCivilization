﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameData:MonoBehaviour 
{
    /// <summary>
    /// Счетчик номера текущего хода
    /// </summary>
    public static uint currentTurnCounter = 1;
    /// <summary>
    /// Общий золотой запас державы
    /// </summary>
    public static int totalGoldReserve = 0;
    /// <summary>
    /// Прирост золота в ход
    /// </summary>
    public static int totalGoldGrowth = 0;
    /// <summary>
    /// Прирост науки в ход
    /// </summary>
    public static int totalScienceGrowth = 0;

    /// <summary>
    /// Слой ландшафта игрового поля
    /// </summary>
    public static Tilemap baseLayer;
    /// <summary>
    /// Слой городов игрового поля
    /// </summary>
    public static Tilemap townLayer;
    /// <summary>
    /// Слой юнитов игрового поля
    /// </summary>
    public static Tilemap unitLayer;
    /// <summary>
    /// Слой территории игрового поля
    /// </summary>
    public static Tilemap territoryLayer;
    /// <summary>
    /// Слой для отображения клеток возможных ходов юнита на игровом поле
    /// </summary>
    public static Tilemap movementLayer;

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
    public static Tile unitHumanTile;
    /// <summary>
    /// Тайл для представления территории игрока
    /// </summary>
    public static Tile territoryTile;
    /// <summary>
    /// Тайл для отображения ячеек, в которые юнит может пойти
    /// </summary>
    public static Tile tileForMovingIn;

    public static string[] UnitPossibleNames = { "Chort", "Vova", "Martin", "Alex", "Vitya"};
    
    private void Awake()
    {
        // Получаем все слои игрового поля
        baseLayer = GameObject.Find("BaseLayer").GetComponent<Tilemap>();           // Получаем слой ландшафта
        townLayer = GameObject.Find("TownLayer").GetComponent<Tilemap>();           // Получаем слой городов
        unitLayer = GameObject.Find("UnitLayer").GetComponent<Tilemap>();           // Получаем слой юнитов
        territoryLayer = GameObject.Find("TerritoryLayer").GetComponent<Tilemap>(); // Получаем слой территории
        movementLayer = GameObject.Find("MovementLayer").GetComponent<Tilemap>();   // Получаем слой возможных передвижений юнита

        // Получаем все возможные тайлы
        capitalCityTile = Resources.Load<Tile>(@"Palettes\TownLayerPalette\CapitalCity");       // Получаем тайл города-столицы
        provincialCityTile = Resources.Load<Tile>(@"Palettes\TownLayerPalette\ProvincialCity"); // Получаем тайл города-провинции
        unitHumanTile = Resources.Load<Tile>(@"Palettes\UnitLayerPalette\Unit");                // Получаем тайл юнита-человека
        territoryTile = Resources.Load<Tile>(@"Palettes\TerritoryLayerPalette\TerritoryTile");  // Получаем тайл территории
        tileForMovingIn = Resources.Load<Tile>(@"Palettes\MovementLayerPalette\TileForMovingIn");   // Получаем тайл для отображения клеток для возможного перемещения


    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Player
{
    /// <summary>
    /// Список юнитов.
    /// </summary>
    public static List<Unit> listOfUnits;
    /// <summary>
    /// Список городов.
    /// </summary>
    public static List<City> listOfCities;

    /// <summary>
    /// Выбранный юнит.
    /// </summary>
    public static Unit selectedUnit;
    /// <summary>
    /// Выбранный город.
    /// </summary>
    public static City selectedCity;

    /// <summary>
    /// Хранит значения накопленных ресурсов игрока.
    /// </summary>
    public static Data data;

    /// <summary>
    /// Инициализирует игрока.
    /// </summary>
    public static void Init()
    {
        // Выделение памяти на список юнитов.
        listOfUnits = new List<Unit>();
        // Выделение памяти на список городов.
        listOfCities = new List<City>();

        // Создаем объект, содержащий информацию о количестве ресурсов игрока.
        data = new Data();

        // Изначально никакой город и никакой юнит не выбраны.
        selectedUnit = null;
        selectedCity = null;
    }

    /// <summary>
    /// Создает юнит игрока в указанном координатами месте игрового поля.
    /// </summary>
    /// <param name="coordinates">Координаты местоположения юнита</param>
    public static void CreateUnit(Vector3Int coordinates)
    {
        // Создает объект нового юнита
        Unit createdUnit = new Unit(coordinates);
        // Добавляет новый юнит в список юнитов игрока.
        listOfUnits.Add(createdUnit);
        // Отображает юнит на игровом поле.
        createdUnit.DisplayUnitOnPlayField();
    }

    /// <summary>
    /// Создает юнит игрока в указанном координатами месте игрового поля. Начальное количество
    /// очков действия юнита равняется startAP.
    /// </summary>
    /// <param name="coordinates">Координаты, в которых появится юнит.</param>
    /// <param name="startAP">Стартовое количество очков действия юнита</param>
    public static void CreateUnit(Vector3Int coordinates, int startAP)
    {
        Unit createdUnit = new Unit(coordinates, startAP);
        listOfUnits.Add(createdUnit);
        createdUnit.DisplayUnitOnPlayField();
    }

    /// <summary>
    /// Создает город игрока в указанном координатами месте игрового поля.
    /// </summary>
    /// <param name="coordinates">Координаты местоположения города</param>
    /// <param name="name">Название города</param>
    /// <param name="isCapital">Является ли город столицей</param>
    public static void CreateCity(Vector3Int coordinates,string name,bool isCapital = false)
    {
        City createdCity = new City(name, coordinates, isCapital);
        listOfCities.Add(createdCity);
        createdCity.DisplayCityOnPlayField();
    }

    /// <summary>
    /// Обновляет значения ресурсов игрока.
    /// </summary>
    public static void UpdatePlayerData()
    {
        data.goldGrowthPerTurn = 0;
        data.scienceGrowthPerTurn = 0;
        // Пересчитываем значения приростов золота и науки в ход
        foreach(City city in listOfCities)
        {
            data.goldGrowthPerTurn += city.goldGrowth;
            data.scienceGrowthPerTurn += city.scienceGrowth;
        }
        data.goldReserve += data.goldGrowthPerTurn;
    }
    
    private static void ChangeUnitTile(Unit unit,Tile newUnitTile)
    {
        // Меняем тайл юнита на новый
        unit.SetUnitTile(newUnitTile);
        GameData.unitLayer.SetTile(unit.GetCoordinates(), null);
        // Отрисовываем новый тайл юнита на карте
        GameData.unitLayer.SetTile(unit.GetCoordinates(), unit.GetTile());
    }

    /// <summary>
    /// Отмечает юнит выбранным.
    /// </summary>
    /// <param name="unit">Выбранный юнит.</param>
    public static void SelectUnit(Unit unit)
    {
        // Проверяем, не выбирается ли данный юнит подряд дважды
        if(selectedUnit == unit)
        {
            // В таком случае, снимаем выделение с юнита
            UnselectUnit();
        }
        else 
        {
            if(selectedUnit != null)
            {
                // Если перед этим выбирался другой юнит, снимаем с него выделение.
                UnselectUnit();
            }
            // Обозначаем данный юнит как выбранный
            selectedUnit = unit;

            ChangeUnitTile(selectedUnit, GameData.selectedUnitTile);

            // Выводим информацию о выбранном юните в панель информации о юните
            UnitInfoPanelLogic.UpdateUnitInfo(selectedUnit);
            // Если у юнита есть неистраченные очки действия, отрисовываем сетку перемещений
            if(selectedUnit.HasAP())
            {
                selectedUnit.SetTilesForMoving();
                selectedUnit.ShowTilesForMoving();
            }
        }
        
    }

    /// <summary>
    /// Снимает выделение с выбранного юнита.
    /// </summary>
    public static void UnselectUnit()
    {
        selectedUnit.HideTilesForMoving();
        // Снимаем с юнита выделение
        ChangeUnitTile(selectedUnit, GameData.initialUnitTile);

        // Скрываем панель информации о юните
        UnitInfoPanelLogic.Close();
        selectedUnit = null;
    }

    /// <summary>
    /// Отмечает город выбранным.
    /// </summary>
    /// <param name="city">Выбранный город.</param>
    public static void SelectCity(City city)
    {
        selectedCity = city;
        // Выводим информацию о городе.
        CityInfoPanelLogic.UpdateCityInfo(selectedCity);
    }
    
    /// <summary>
    /// Отладочная ф-ия для создания трех тестовых городов и трех юнитов.
    /// </summary>
    public static void SetTestTownAndUnitKit()
    {
        // Создаем столицу и два города провинции
        CreateCity(new Vector3Int(1, 2, 0), "Moscow", true);
        CreateCity(new Vector3Int(-4, 2, 0), "Orel");
        CreateCity(new Vector3Int(-1, -3, 0), "Kursk");

        // Создаем трех юнитов
        CreateUnit(new Vector3Int(2, 3, 0));
        CreateUnit(new Vector3Int(-2, 2, 0));
        CreateUnit(new Vector3Int(0, -2, 0));
        UpdatePlayerData();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    /// <summary>
    /// Список юнитов.
    /// </summary>
    public static List<Human> listOfUnits;
    /// <summary>
    /// Список городов.
    /// </summary>
    public static List<City> listOfCities;

    /// <summary>
    /// Выбранный юнит.
    /// </summary>
    public static Human selectedUnit;
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
        listOfUnits = new List<Human>();
        // Выделение памяти на список городов.
        listOfCities = new List<City>();

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
        Human createdUnit = new Human(coordinates);
        // Добавляет новый юнит в список юнитов игрока.
        listOfUnits.Add(createdUnit);
        // Отображает юнит на игровом поле.
        DisplayUnitOnPlayField(createdUnit);
    }

    /// <summary>
    /// Создает юнит игрока в указанном координатами месте игрового поля. У юнита будет
    /// количество очков действия, равное значению startAP
    /// </summary>
    /// <param name="coordinates">Координаты, в которых появится юнит.</param>
    /// <param name="startAP">Стартовое количество очков действия для юнита</param>
    public static void CreateUnit(Vector3Int coordinates, int startAP)
    {
        Human createdUnit = new Human(coordinates, startAP);
        listOfUnits.Add(createdUnit);
        DisplayUnitOnPlayField(createdUnit);
    }

    /// <summary>
    /// Отрисовывает юнит на игровом поле.
    /// </summary>
    /// <param name="unit">Юнит, который отрисуется на игровом поле.</param>
    private static void DisplayUnitOnPlayField(Human unit)
    {
        GameData.unitLayer.SetTile(unit.Coordinates, unit.unitTile);
    }

    /// <summary>
    /// Стирает юнит с игрового поля.
    /// </summary>
    /// <param name="unit">Юнит, который будет стёрт с игрового поля.</param>
    private static void RemoveUnitFromPlayField(Human unit)
    {
        GameData.unitLayer.SetTile(unit.Coordinates, null);
    }


    /// <summary>
    /// Создает город игрока в указанном координатами месте игрового поля.
    /// </summary>
    /// <param name="coordinates">Координаты местоположения города</param>
    /// <param name="name">Название города</param>
    /// <param name="isCapital">Является ли город столицей</param>
    public static void CreateCity(Vector3Int coordinates,string name,bool isCapital = false)
    {
        City newCity = new City(name, coordinates, isCapital);
        listOfCities.Add(newCity);
        DisplayCityOnPlayField(newCity);
    }

    /// <summary>
    /// Отображает город на игровом поле.
    /// </summary>
    /// <param name="city">Город, который будет отображен на игровом поле.</param>
    private static void DisplayCityOnPlayField(City city)
    {
        GameData.cityLayer.SetTile(city.Coordinates, city.cityTile);
        city.SetTerritory();
        city.ShowTerritory();
    }

    /// <summary>
    /// Стирает город с игрового поля.
    /// </summary>
    /// <param name="city">Город, который будет стерт с игрового поля.</param>
    private static void RemoveCityFromPlayField(City city)
    {
        GameData.cityLayer.SetTile(city.Coordinates, null);
    }

    /// <summary>
    /// Обновляет значения ресурсов игрока.
    /// </summary>
    public static void UpdatePlayerData()
    {
        data.goldGrowthPerTurn = 0;
        data.scienceGrowthPerTurn = 0;
        // Пересчитываем значения приростов золота/науки в ход
        foreach(City city in listOfCities)
        {
            data.goldGrowthPerTurn += city.goldGrowth;
            data.scienceGrowthPerTurn += city.scienceGrowth;
        }
        data.goldReserve += data.goldGrowthPerTurn;
    }
    
    /// <summary>
    /// Отмечает юнит выбранным.
    /// </summary>
    /// <param name="unit">Выбранный юнит.</param>
    public static void SelectUnit(Human unit)
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
            //Меняем цвет выбранного юнита на градиентный желтый
            selectedUnit.unitTile = GameData.selectedUnitTile;
            GameData.unitLayer.SetTile(selectedUnit.Coordinates, selectedUnit.unitTile);

            UnitInfoPanelLogic.UpdateUnitInfo(selectedUnit);
            if(selectedUnit.ActionPoints > 0)
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
        // Снимаем с юнита выделение, возвращая ему его основной цвет
        selectedUnit.unitTile = GameData.initialUnitTile;
        GameData.unitLayer.SetTile(selectedUnit.Coordinates, selectedUnit.unitTile);

        selectedUnit = null;
        UnitInfoPanelLogic.unitInfoPanel.SetActive(false);
    }

    /// <summary>
    /// Отмечает город выбранным.
    /// </summary>
    /// <param name="city">Выбранный город.</param>
    public static void SelectCity(City city)
    {
        selectedCity = city;
        // Выводим информацию о городе.
        CityInfoPanelLogic.UpdateCityInfo(city);
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

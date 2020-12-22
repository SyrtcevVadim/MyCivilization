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
    /// Создает юнита-рабочего.
    /// </summary>
    /// <param name="coordinates">Координаты, в которых юнит появится.</param>
    public static void CreateWorker(Vector3Int coordinates)
    {
        Worker createdUnit = new Worker(coordinates);
        listOfUnits.Add(createdUnit);
        createdUnit.DisplayUnitOnPlayField();
    }

    /// <summary>
    /// Создает юнита-рабочего
    /// </summary>
    /// <param name="coordinates">Координаты, в которых юнит появится.</param>
    /// <param name="startAP">Начальное число очков действия юнита</param>
    public static void CreateWorker(Vector3Int coordinates, int startAP)
    {
        // Создает объект нового юнита
        Worker createdUnit = new Worker(coordinates, startAP);
        // Добавляет новый юнит в список юнитов игрока.
        listOfUnits.Add(createdUnit);
        // Отображает юнит на игровом поле.
        createdUnit.DisplayUnitOnPlayField();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="coordinates"></param>
    public static void CreateWarrior(Vector3Int coordinates)
    {
        // Создает объект нового юнита
        Warrior createdUnit = new Warrior(coordinates);
        // Добавляет новый юнит в список юнитов игрока.
        listOfUnits.Add(createdUnit);
        // Отображает юнит на игровом поле.
        createdUnit.DisplayUnitOnPlayField();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="startAP"></param>
    public static void CreateWarrior(Vector3Int coordinates, int startAP)
    {
        // Создает объект нового юнита
        Warrior createdUnit = new Warrior(coordinates, startAP);
        // Добавляет новый юнит в список юнитов игрока.
        listOfUnits.Add(createdUnit);
        // Отображает юнит на игровом поле.
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
        // Пересчитываем значения приростов золота в ход
        foreach(City city in listOfCities)
        {
            data.goldGrowthPerTurn += city.goldGrowth;
        }
        // Наращиваем деньги только после первого хода игрока
        if(GameData.currentTurnCounter > 1)
        {
            data.goldReserve += data.goldGrowthPerTurn;
        }
    }
    
    /// <summary>
    /// Изменяет тайл юнита.
    /// </summary>
    /// <param name="unit">Юнит, тайл которого будет изменен.</param>
    /// <param name="newUnitTile">Новый тайл юнита.</param>
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
    public static void SetTestCityAndUnitKit()
    {
        // Создаем столицу и два города провинции
        CreateCity(new Vector3Int(1, 2, 0), "Moscow", true);
        CreateCity(new Vector3Int(-4, 2, 0), "Orel");
        CreateCity(new Vector3Int(-1, -3, 0), "Kursk");

        // Создаем трех юнитов
        CreateWorker(new Vector3Int(2, 3, 0));
        CreateWarrior(new Vector3Int(-2, 2, 0));
        CreateWorker(new Vector3Int(0, -2, 0));
        UpdatePlayerData();
    }
}

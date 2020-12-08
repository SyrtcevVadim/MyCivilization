using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComputer
{
    /// <summary>
    /// Список юнитов компьютерного противника.
    /// </summary>
    List<Unit> listOfUnits;

    /// <summary>
    /// Список городов компьютерного противника.
    /// </summary>
    List<City> listOfCities;

    /// <summary>
    /// Создает город компьютера-противника
    /// </summary>
    /// <param name="name">Название города.</param>
    /// <param name="coordinates">Координаты позиции города на карте</param>
    public void CreateCity(string name, Vector3Int coordinates)
    {
        City createdCity = new City(name, coordinates);
        createdCity.cityTile = GameData.enemyCity;
        listOfCities.Add(createdCity);
        createdCity.DisplayCityOnPlayField();
    }

    /// <summary>
    /// Создает юнит-варвар.
    /// </summary>
    /// <param name="coordinates">Координаты позиции, в которой юнит-варвар будет создан.</param>
    public void CreateBarbarian(Vector3Int coordinates)
    {
        Barbarian createdBarbarian = new Barbarian(coordinates);    // Создаем объект класса Barbarian
        listOfUnits.Add(createdBarbarian);                          // Добавляем созданный юнит в список юнитов
        createdBarbarian.DisplayUnitOnPlayField();                  // Отображаем созданный юнит на карте
    }

    /// <summary>
    /// Создает юнит-варвар.
    /// </summary>
    /// <param name="coordinates">Координаты позиции, в которой юнит-варвар будет создан.</param>
    /// <param name="startAP">Начальное количество очков действия юнита-варвара.</param>
    public void CreateBarbarian(Vector3Int coordinates, int startAP)
    {
        Barbarian createdBarbarian = new Barbarian(coordinates, startAP);
        listOfUnits.Add(createdBarbarian);
        createdBarbarian.DisplayUnitOnPlayField();
    }

    /// <summary>
    /// Конструктор объекта компьютер-противник.
    /// </summary>
    public EnemyComputer()
    {
        listOfUnits = new List<Unit>();     // Выделяем память под список юнитов
        listOfCities = new List<City>();    // Выделяем память под список городов
        Data data = new Data();             // Создаем объект, хранящий список ресурсов противника-компьютера
    }

    /// <summary>
    /// Создает компьютеру-противнику тестовый набор городов и юнитов.
    /// </summary>
    public void SetTestCityAndUnitKit()
    {
        CreateCity("AbrhBrh", new Vector3Int(-4, -4, 0));
        CreateBarbarian(new Vector3Int(-5, -4, 0));
    }

}

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

    public void CreateCity(string name, Vector3Int coordinates)
    {
        City createdCity = new City(name, coordinates);
        createdCity.cityTile = GameData.enemyCity;
        listOfCities.Add(createdCity);
        createdCity.DisplayCityOnPlayField();
    }

    public void CreateBarbarian(Vector3Int coordinates)
    {
        Barbarian createdBarbarian = new Barbarian(coordinates);
        listOfUnits.Add(createdBarbarian);
        createdBarbarian.DisplayUnitOnPlayField();
    }


    /// <summary>
    /// Конструктор объекта компьютер-противник.
    /// </summary>
    public EnemyComputer()
    {
        listOfUnits = new List<Unit>();
        listOfCities = new List<City>();

        Data data = new Data();
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

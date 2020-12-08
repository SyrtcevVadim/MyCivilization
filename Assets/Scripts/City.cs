using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class City
{

    // TODO сделать поля закрытыми, но пока пойдет
    /// <summary>
    /// Название города
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Координаты города
    /// </summary>
    private Vector3Int coordinates;

    /// <summary>
    /// Тайл, представляющий город на карте
    /// </summary>
    public Tile cityTile;

    /// <summary>
    /// Показывает, является ли текущий город столицей.
    /// </summary>
    bool isCapital;

    /// <summary>
    /// Прирост продукции каждый ход.
    /// </summary>
    public double productionGrowth;

    /// <summary>
    /// Значение произведенной продукции в городе.
    /// </summary>
    public double totalProductionValue;

    /// <summary>
    /// Верхняя граница накопления продукции в городе.
    /// </summary>
    public double maxPossibleProductionValue;

    /// <summary>
    /// Прирост золота каждый ход
    /// </summary>
    public int goldGrowth;

    /// <summary>
    /// Прирост науки каждый ход
    /// </summary>
    public int scienceGrowth;

    /// <summary>
    /// Текущее население города
    /// </summary>
    public int populationCounter;


    /// <summary>
    ///  Конструктор класса Город
    /// </summary>
    /// <param name="name">Имя города</param>
    /// <param name="coordinates">Координаты города в сетке</param>
    /// <param name="cityTile">Тайл, представляющий город на карте</param>
    /// <param name="isCapital">Является ли город городом-столицей</param>
    public City(string name, Vector3Int coordinates, bool isCapital = false)
    {
        // Если город является столицей, в нем увеличены все характеристики производства
        if (isCapital)
        {
            productionGrowth = 2.3;                            // Прирост продукции
            goldGrowth = 3;                                    // Прирост золота
            scienceGrowth = 1;                                 // Прирост науки
            populationCounter = 3;                             // Текущее население города
            maxPossibleProductionValue = 50;                  // Объем складов для производимой продукции в городе.
            cityTile = GameData.capitalCityTile;          // Устанавливаем тайл-столицы для города столицы
        }
        else
        {
            productionGrowth = 1;                              // Прирост продукции
            goldGrowth = 1;                                    // Прирост золота
            scienceGrowth = 0;                                 // Прирост науки
            populationCounter = 1;                             // Текущее население города
            maxPossibleProductionValue = 25;                  // Объем складов для производимой продукции в городе
            cityTile = GameData.provincialCityTile;       // Устанавливаем тайл-провинции для города-провинции
        }
        this.Name = name;               // Устанавливаем имя города
        this.coordinates = coordinates; // Сохраняем координаты города на карте
        
        this.isCapital = isCapital;     // Сохраняем информацию о том, является ли этот город столичным
    }

    /// <summary>
    /// Производит продукцию в городе. Наращивает счетчик накопленной продукции.
    /// </summary>
    public void GenerateProduction()
    {
        if(totalProductionValue + productionGrowth <= maxPossibleProductionValue)
        {
            totalProductionValue += productionGrowth;
        }
        else
        {
            totalProductionValue = maxPossibleProductionValue;
        }
    }

    /// <summary>
    /// Заплатить очки продукции города.
    /// </summary>
    /// <param name="spentProduction">Продукция, которую следует заплатить.</param>
    public void PayProductionCost(int spentProduction)
    {
        totalProductionValue -= spentProduction;
    }

    /// <summary>
    /// Отображает город на игровом поле.
    /// </summary>
    /// <param name="city">Город, который будет отображен на игровом поле.</param>
    public void DisplayCityOnPlayField()
    {
        GameData.cityLayer.SetTile(coordinates, cityTile);
    }

    /// <summary>
    /// Стирает город с игрового поля.
    /// </summary>
    /// <param name="city">Город, который будет стерт с игрового поля.</param>
    public void RemoveCityFromPlayField()
    {
        GameData.cityLayer.SetTile(coordinates, null);
    }
    /// <summary>
    /// Получает координаты города на карте.
    /// </summary>
    /// <returns>Координаты города на карте.</returns>
    public Vector3Int GetCoordinates()
    {
        return coordinates;
    }
}

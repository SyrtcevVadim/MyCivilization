using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class City
{
    /// <summary>
    /// Название города
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Координаты города
    /// </summary>
    private Vector3Int coordinates;

    /// <summary>
    /// Координаты города
    /// </summary>
    public Vector3Int Coordinates
    {
        get
        {
            return coordinates;
        }
        set
        {
            coordinates = value;
        }
    }

    /// <summary>
    /// Тайл, представляющий город на карте
    /// </summary>
    public Tile cityTile;

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
            maxPossibleProductionValue = 400;                  // Объем складов для производимой продукции в городе.
            this.cityTile = GameData.capitalCityTile;          // Устанавливаем тайл-столицы для города столицы
        }
        else
        {
            productionGrowth = 1;                              // Прирост продукции
            goldGrowth = 1;                                    // Прирост золота
            scienceGrowth = 0;                                 // Прирост науки
            populationCounter = 1;                             // Текущее население города
            maxPossibleProductionValue = 100;                  // Объем складов для производимой продукции в городе
            this.cityTile = GameData.provincialCityTile;       // Устанавливаем тайл-провинции для города-провинции
        }
        this.Name = name;               // Устанавливаем имя города
        this.coordinates = coordinates; // Сохраняем координаты города на карте
        
        this.isCapital = isCapital;     // Сохраняем информацию о том, является ли этот город столичным
        territory = new List<Vector3Int>();
    }


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
    /// Список координат тайлов, которые принадлежать городу
    /// </summary>
    public List<Vector3Int> territory;

    /// <summary>
    /// Просчитывает примыкающую к городу территорию.
    /// </summary>
    public void SetTerritory()
    {
        int x = Coordinates.x;
        int y = Coordinates.y;
        Vector3Int self, a, b, c, d, e, f;
        self = new Vector3Int(x, y, 0);

        if (System.Math.Abs(y) % 2 == 0)
        {
            a = new Vector3Int(x - 1, y, 0);
            b = new Vector3Int(x + 1, y, 0);
            c = new Vector3Int(x, y + 1, 0);
            d = new Vector3Int(x, y - 1, 0);
            e = new Vector3Int(x - 1, y + 1, 0);
            f = new Vector3Int(x - 1, y - 1, 0);
        }
        else
        {
            a = new Vector3Int(x - 1, y, 0);
            b = new Vector3Int(x + 1, y, 0);
            c = new Vector3Int(x, y + 1, 0);
            d = new Vector3Int(x, y - 1, 0);
            e = new Vector3Int(x + 1, y + 1, 0);
            f = new Vector3Int(x + 1, y - 1, 0);
        }
        Vector3Int[] arr = { self, a, b, c, d, e, f };
        foreach (Vector3Int coord in arr)
        {
            if (PlayFieldLogic.IsTileExists(coord))
            {
                territory.Add(coord);
            }
        }
    }
    /// <summary>
    /// Отображаем территорию города на игровом поле
    /// </summary>
    public void ShowTerritory()
    {
        foreach(Vector3Int coord in territory)
        {
            GameData.territoryLayer.SetTile(coord, GameData.territoryTile);
        }    
    }

    
}

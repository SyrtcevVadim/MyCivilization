using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Human
{

    /// <summary>
    /// Очки действия юнита
    /// </summary>
    private int actionPoints;

    /// <summary>
    /// Очки действия юнита
    /// </summary>
    public int ActionPoint
    {
        get
        {
            return actionPoints;
        }
        set
        {
            actionPoints = value;
        }
    }

    /// <summary>
    /// Координаты ячейки юнита на игровом поле.ы
    /// </summary>
    private Vector3Int coordinates;
    /// <summary>
    /// Координаты ячейки юнита на игровом поле.
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
    /// Тайл, представляющий юнита на игровом поле.
    /// </summary>
    public  Tile unitTile;

    // Характеристики юнита
    /// <summary>
    /// Имя юнита
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Жизненная сила юнита
    /// </summary>
    public uint toughness;
    /// <summary>
    /// Ударная мощь юнита
    /// </summary>
    public uint strength;
    /// <summary>
    /// Бронированность юнита
    /// </summary>
    public uint armor;
    /// <summary>
    /// Стоимость юнита в очках продукции.
    /// </summary>
    public static double costInProductionPoints;

    /// <summary>
    /// Конструктор класса Человек
    /// </summary>
    /// <param name="name">Имя человека</param>
    /// <param name="coordinates">Координаты позиции, в которой человек находится</param>
    /// <param name="unitTile">Tile, которым представлен спрайт человека</param>
    public Human(Vector3Int coordinates)
    {
        possibleTileCoordinatesForMoving = new List<Vector3Int>();          // У каждого человека есть соседние клетки для перемещения
        Name = GameData.GetRandomHumanName();                               // Получаем случайное имя для юнита-человека
        Coordinates = coordinates;                                          // Позиционируем его на карте
        unitTile = GameData.initialUnitTile;
        actionPoints = 2;                                                   // У человека изначально 2 очка действия
        toughness = 100;                                                    // 100 очков жизней
        strength = 20;                                                      // 20 очков силы
        armor = 3;                                                          // и 3 очка защиты
        costInProductionPoints = 10;                                        // Юнит-человек стоит 10 единиц продукции
    }

    /// <summary>
    /// Список всех ячеек, в которые юнит может совершить перемещение.
    /// </summary>
    public List<Vector3Int> possibleTileCoordinatesForMoving;

    /// <summary>
    /// Записывает в possibleTileCoordinatesForMovement все возможные ячейки для передвижения. 
    /// Использует метод IsTileExists для проверки существования игровой ячейки
    /// </summary>
    public void SetTilesForMoving()
    {
        possibleTileCoordinatesForMoving.Clear();
        int x = Coordinates.x;     // Псевдоним для unit.Coordinates.x
        int y = Coordinates.y;     // Псевдоним для unit.Coordinates.y
        Vector3Int a, b, c, d, e, f;
        // В зависимости от четности номера строки координаты клеток соседей считаются по-разному
        if (System.Math.Abs(y) % 2 == 0)
        {
            // Координаты 6 смежных ячеек
            a = new Vector3Int(x - 1, y, 0);
            b = new Vector3Int(x + 1, y, 0);
            c = new Vector3Int(x, y + 1, 0);
            d = new Vector3Int(x, y - 1, 0);
            e = new Vector3Int(x - 1, y + 1, 0);
            f = new Vector3Int(x - 1, y - 1, 0);
        }
        else
        {
            // Координаты 6 смежных ячеек
            a = new Vector3Int(x - 1, y, 0);
            b = new Vector3Int(x + 1, y, 0);
            c = new Vector3Int(x, y + 1, 0);
            d = new Vector3Int(x, y - 1, 0);
            e = new Vector3Int(x + 1, y + 1, 0);
            f = new Vector3Int(x + 1, y - 1, 0);
        }
        Vector3Int[] arr = {Coordinates, a, b, c, d, e, f };
        foreach (Vector3Int coord in arr)
        {
            if (PlayFieldLogic.IsTileExists(coord))
            {
                possibleTileCoordinatesForMoving.Add(coord);
            }
        }
    }

    /// <summary>
    /// Отображает ячейки, в которые юнит может совершить перемещение
    /// </summary>
    public void ShowTilesForMoving()
    {
        foreach(Vector3Int coord in possibleTileCoordinatesForMoving)
        {
            GameData.movementLayer.SetTile(coord, GameData.tileForMovingIn);
        }
    }

    /// <summary>
    /// Скрывает ячейки, в которые юнит может совершить перемещение
    /// </summary>
    public void HideTilesForMoving()
    {
        foreach(Vector3Int coord in possibleTileCoordinatesForMoving)
        {
            GameData.movementLayer.SetTile(coord, null);
        }
    }

    /// <summary>
    /// Проверяет, возможно ли совершить перемещение юнита в ячейку с координатами destination.
    /// </summary>
    /// <param name="destination">Координаты ячейки для перемещения.</param>
    /// <returns>Если перемещение возможно, true. Иначе - false.</returns>
    public bool IsMovingPossible(Vector3Int destination)
    {
        if(actionPoints > 0)
        {
            foreach (Vector3Int coord in possibleTileCoordinatesForMoving)
            {
                if (destination == coord)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Перемещает юнит в ячейку с координатами destination.
    /// </summary>
    /// <param name="destination">Координаты ячейки, в которую юнит перемещается.</param>
    public void Move(Vector3Int destination)
    {
        
        GameData.unitLayer.SetTile(Coordinates, null);      // Удаляем тайл юнита со старой позиции
        GameData.unitLayer.SetTile(destination, unitTile);  // Отрисовываем тайл юнита на новой позиции
        Coordinates = destination;                          // Сохраняем новые координаты юнита
        actionPoints -= 1;                                  // Уменьшаем количество очков действий на 1
    }
    
    
    
    /// <summary>
    /// Восстанавливает юниту очки действий.
    /// </summary>
    public void RestoreActionPoints()
    {
        actionPoints = 2;
    }
}

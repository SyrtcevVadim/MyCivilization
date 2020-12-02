using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class baseLayerTile
{
    public Vector3Int coordinates;
    public int requiresAP;
    public baseLayerTile(Vector3Int coord, int requiresAP)
    {
        this.coordinates = coord;
        this.requiresAP = requiresAP;
    }
}

public class Human
{

    /// <summary>
    /// Очки действия юнита
    /// </summary>
    private int actionPoints;

    /// <summary>
    /// Очки действия юнита
    /// </summary>
    public int ActionPoints
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
        movingTilesGrid = new List<baseLayerTile>();          // У каждого человека есть соседние клетки для перемещения
        movingTilesCoordinates = new List<Vector3Int>();
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
    public List<baseLayerTile> movingTilesGrid;
    /// <summary>
    /// Координаты всех ячеек, в которые юнит может совершить перемещение.
    /// </summary>
    public List<Vector3Int> movingTilesCoordinates;

    void AddNeighbourTilesToMovingGrid(Vector3Int coordinates, int remainingAP, int requireAP)
    {
        if (remainingAP >= 0)
        {
            int x = coordinates.x, y = coordinates.y;
            Vector3Int a, b, c, d, e, f;
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
            Vector3Int[] tiles = { a, b, c, d, e, f };
            // Добавляем текущую ячейку в сетку, если ее там еще нет
            if (!movingTilesCoordinates.Contains(coordinates))
            {
                //Debug.Log("require AP: " + requireAP + "| tile: " + coordinates);
                movingTilesGrid.Add(new baseLayerTile(coordinates, requireAP));
                movingTilesCoordinates.Add(coordinates);
            }
            // Если текущая ячейка уже существует, но в нее можно добраться коротким путем.
            else if(movingTilesGrid.Find(par => par.coordinates == coordinates).requiresAP > requireAP)
            {
                movingTilesGrid.Find(par => par.coordinates == coordinates).requiresAP = requireAP;
                //Debug.Log("EXCHANGING: require AP: " + requireAP + "| tile: " + coordinates);
            }
            foreach (Vector3Int item in tiles)
            {
                if(PlayFieldLogic.GetTileAPRequirments(item) == -1)
                {
                    // Непроходимая ячейка.
                }
                else
                {
                    int newRemainngAP = remainingAP - PlayFieldLogic.GetTileAPRequirments(item);
                    if (newRemainngAP >= 0)
                    {
                        AddNeighbourTilesToMovingGrid(item, newRemainngAP, requireAP+PlayFieldLogic.GetTileAPRequirments(item));
                    }
                }
            }
        }
    }


    /// <summary>
    /// Записывает в possibleTileCoordinatesForMovement все возможные ячейки для передвижения. 
    /// Использует метод IsTileExists для проверки существования игровой ячейки
    /// </summary>
    public void SetTilesForMoving()
    {
        movingTilesGrid.Clear();
        movingTilesCoordinates.Clear();
        AddNeighbourTilesToMovingGrid(Coordinates, actionPoints, 0);
        
    }

    /// <summary>
    /// Отображает ячейки, в которые юнит может совершить перемещение
    /// </summary>
    public void ShowTilesForMoving()
    {
        foreach(baseLayerTile tile in movingTilesGrid)
        {
            GameData.movementLayer.SetTile(tile.coordinates, GameData.tileForMovingIn);
        }
    }

    /// <summary>
    /// Скрывает ячейки, в которые юнит может совершить перемещение
    /// </summary>
    public void HideTilesForMoving()
    {
        foreach(Vector3Int coord in movingTilesCoordinates)
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
        //baseLayerTile destinationTile = movingTilesGrid.Find(x => x.coordinates == destination);
        //if((actionPoints - destinationTile.requiresAP > 0) && (destination != coordinates))
        //{
        //    foreach (baseLayerTile tile in movingTilesGrid)
        //    {
        //        if (destination == tile.coordinates)
        //        {
        //            return true;
        //        }
        //    }
        //}
        if(destination !=coordinates)
        {
            foreach(Vector3Int coord in movingTilesCoordinates)
            {
                if(destination == coord)
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
    public void Move(Vector3Int coordinates)
    {
        
        GameData.unitLayer.SetTile(Coordinates, null);                      // Удаляем тайл юнита со старой позиции
        GameData.unitLayer.SetTile(coordinates, unitTile);  // Отрисовываем тайл юнита на новой позиции
        Coordinates = coordinates;                          // Сохраняем новые координаты юнита
        actionPoints -= movingTilesGrid.Find(x => x.coordinates == coordinates).requiresAP ;// Уменьшаем количество очков действий на 1
    }
    
    
    
    /// <summary>
    /// Восстанавливает юниту очки действий.
    /// </summary>
    public void RestoreActionPoints()
    {
        actionPoints = 2;
    }
}

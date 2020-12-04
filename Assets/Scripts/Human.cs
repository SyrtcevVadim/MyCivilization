using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/// <summary>
/// Класс ячейки ландшафта на игровом поле.
/// </summary>
public class baseLayerTile
{
    /// <summary>
    /// Координаты ячейки на игровом поле.
    /// </summary>
    public Vector3Int coordinates;
    /// <summary>
    /// Количество очков действия, требуемых для перемещения из текущей ячейки в эту.
    /// </summary>
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
        movingGrid = new List<baseLayerTile>();          // У каждого человека есть соседние клетки для перемещения

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
    /// Список ячеек, в которые юнит может совершить перемещение.
    /// </summary>
    public List<baseLayerTile> movingGrid;

    private void AddNeighbourTilesToMovingGrid(Vector3Int coordinates, int remainingAP, int requireAP)
    {
        if (remainingAP >= 0)
        {

            Vector3Int[] tiles = PlayFieldLogic.GetListOfNeighbourTiles(coordinates);

            // Добавляем текущую ячейку в сетку, если ее там еще нет
            if (movingGrid.Find(obj => obj.coordinates == coordinates) == null)
            {
                //Debug.Log("require AP: " + requireAP + "| tile: " + coordinates);
                movingGrid.Add(new baseLayerTile(coordinates, requireAP));
                //movingTilesCoordinates.Add(coordinates);
            }
            // Если текущая ячейка уже существует, но в нее можно добраться коротким путем.
            else if(movingGrid.Find(obj => obj.coordinates == coordinates).requiresAP > requireAP)
            {
                // Устанавливаем более низкую цену за передвижение, если это возможно
                movingGrid.Find(obj => obj.coordinates == coordinates).requiresAP = requireAP;
                //Debug.Log("EXCHANGING: require AP: " + requireAP + "| tile: " + coordinates);
            }
            // Обрабатываем массив соседних ячеек этой же функцией
            foreach (Vector3Int item in tiles)
            {
                // Если ячейка не является непроходимой
                if(PlayFieldLogic.GetTileAPRequirments(item) != -1)
                {
                    int newRemainngAP = remainingAP - PlayFieldLogic.GetTileAPRequirments(item);
                    if (newRemainngAP >= 0)
                    {
                        // Просчитываем цену передвижения в соседние ячейки для данной ячейке
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
        movingGrid.Clear();
        AddNeighbourTilesToMovingGrid(Coordinates, actionPoints, 0);
        // Запрещаем юнитам ходить в ячейки, в которых уже стоят союзные юниты.
        foreach(Human unit in Player.listOfUnits)
        {
            if(movingGrid.Find(obj => obj.coordinates == unit.coordinates) !=null && unit.coordinates != coordinates)
            {
                movingGrid.Remove(movingGrid.Find(x => x.coordinates == unit.coordinates));
            }
        }
        
    }

    /// <summary>
    /// Отображает ячейки, в которые юнит может совершить перемещение
    /// </summary>
    public void ShowTilesForMoving()
    {
        foreach(baseLayerTile tile in movingGrid)
        {
            GameData.movementLayer.SetTile(tile.coordinates, GameData.tileForMovingIn);
        }
    }

    /// <summary>
    /// Скрывает ячейки, в которые юнит может совершить перемещение
    /// </summary>
    public void HideTilesForMoving()
    {
        foreach(baseLayerTile tile in movingGrid)
        {
            GameData.movementLayer.SetTile(tile.coordinates, null);
        }
    }

    /// <summary>
    /// Проверяет, возможно ли совершить перемещение юнита в ячейку с координатами destination.
    /// </summary>
    /// <param name="destination">Координаты ячейки для перемещения.</param>
    /// <returns>Если перемещение возможно, true. Иначе - false.</returns>
    public bool IsMovingPossible(Vector3Int destination)
    {
        // Запрещаем юниту ходить в клетку с самим собой
        if(destination !=coordinates)
        {
            // Если пользователь кликнул в одну из ячеек сетки перемещений, разрешаем ему ход
            foreach(baseLayerTile tile in movingGrid)
            {
                if(destination == tile.coordinates)
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
        actionPoints -= movingGrid.Find(x => x.coordinates == coordinates).requiresAP ;// Уменьшаем количество очков действий на 1
    }
    
    
    
    /// <summary>
    /// Восстанавливает юниту очки действий.
    /// </summary>
    public void RestoreActionPoints()
    {
        actionPoints = 2;
    }
}

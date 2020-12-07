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
/// <summary>
/// Базовый класс для всех юнитов в игре.
/// </summary>
public class Unit
{
    
    /// <summary>
    /// Координаты ячейки юнита на игровом поле.
    /// </summary>
    protected Vector3Int coordinates;

    /// <summary>
    /// Тайл, представляющий юнита на игровом поле.
    /// </summary>
    protected  Tile unitTile;

    // TODO переделать под индексатор. Опасно: выход за границы массива при получении максимального уровня!!
    /// <summary>
    /// Массив, содержащий необходимое количество опыта для получения каждого уровня. Номер уровня совпадает с индексом в данном массиве.
    /// </summary>
    protected static int[] experienceRequiredForLevel = new int[] { 0, 5, 10, 15 };

    // Характеристики юнита
    /// <summary>
    /// Имя юнита
    /// </summary>
    protected string Name { get; set; }
    /// <summary>
    /// Специализация юнита
    /// </summary>
    protected string Specialization { get; set; }

    /// <summary>
    /// Очки действия юнита
    /// </summary>
    protected int currentAP;
    /// <summary>
    /// Максимально вомзожное количество очков действий юнита.
    /// </summary>
    protected int maxAP;


    /// <summary>
    /// Уровень юнита
    /// </summary>
    protected int currentLevel;
    /// <summary>
    /// Количество очков опыта юнита.
    /// </summary>
    protected int collectedExperience;

    /// <summary>
    /// Максимально возможное количество очков здоровью у юнита.
    /// </summary>
    protected int maxHP;
    /// <summary>
    /// Текущее количество очков здоровья юнита.
    /// </summary>
    protected int currentHP;


    /// <summary>
    /// Ударная мощь юнита
    /// </summary>
    protected int strength;
    /// <summary>
    /// Бронированность юнита
    /// </summary>
    protected int armor;


    /// <summary>
    /// Стоимость юнита в очках продукции.
    /// </summary>
    public static int costInProductionPoints = 10;


    /// <summary>
    /// Конструктор класса юнит.
    /// </summary>
    /// <param name="coordinates">Координаты юнита на карте.</param>
    public Unit(Vector3Int coordinates)
    {
        movingGrid = new List<baseLayerTile>(); // Выделяем память под сетку перемещения

        Name = GameData.GetRandomHumanName();   // Получаем случайное имя для юнита
        this.coordinates = coordinates;         // Позиционируем его на карте
        unitTile = GameData.initialUnitTile;
        maxAP = 2;
        currentAP = maxAP;                      // У человека изначально 2 очка действия
        maxHP = 100;                            // 100 очков жизней
        currentHP = maxHP;                      // Изначально у юнита максимальное количество очков здоровья
        collectedExperience = 0;                // При создании у юнита нет очков опыта
        currentLevel = 0;                                                   
        strength = 20;                          // 20 очков силы
        armor = 3;                              // и 3 очка защиты
    }

    /// <summary>
    /// Конструктор класса юнит.
    /// </summary>
    /// <param name="coordinates">Координаты юнита на карте.</param>
    /// <param name="startAP">Начальное число очков действия юнита.</param>
    public Unit(Vector3Int coordinates, int startAP)
    {
        movingGrid = new List<baseLayerTile>();
        Name = GameData.GetRandomHumanName();
        this.coordinates = coordinates;
        unitTile = GameData.initialUnitTile;
        currentAP = startAP;
        maxAP = 2;
        maxHP = 100;
        currentHP = maxHP;
        collectedExperience = 0;
        currentLevel = 0;
        strength = 20;
        armor = 3;
    }

    /// <summary>
    /// Список ячеек, в которые юнит может совершить перемещение.
    /// </summary>
    public List<baseLayerTile> movingGrid;

    /// <summary>
    /// Просчитывает ячейки, в которые юнит может совершить перемещение.
    /// </summary>
    /// <param name="coordinates">Координаты текущей ячейки.</param>
    /// <param name="remainingAP">Доступные очки действия.</param>
    /// <param name="requireAP">Очки действия, необходимые для перемещения в текущую ячейку.</param>
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
        AddNeighbourTilesToMovingGrid(coordinates, currentAP, 0);
        // Запрещаем юнитам ходить в ячейки, в которых уже стоят союзные юниты.
        foreach(Unit unit in Player.listOfUnits)
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
    public void Move(Vector3Int newCoordinates)
    {
        
        GameData.unitLayer.SetTile(coordinates, null);                      // Удаляем тайл юнита со старой позиции
        GameData.unitLayer.SetTile(newCoordinates, unitTile);  // Отрисовываем тайл юнита на новой позиции
        coordinates = newCoordinates;                          // Сохраняем новые координаты юнита
        currentAP -= movingGrid.Find(x => x.coordinates == coordinates).requiresAP ;// Уменьшаем количество очков действий на 1
    }
    
    /// <summary>
    /// Восстанавливает юниту очки действий.
    /// </summary>
    public void RestoreUnitAP()
    {
        currentAP = maxAP;
    }

    /// <summary>
    /// Отображает юнит на игровом поле.
    /// </summary>
    /// <param name="unit">Юнит, который отрисуется на игровом поле.</param>
    public void DisplayUnitOnPlayField()
    {
        GameData.unitLayer.SetTile(coordinates, unitTile);
    }

    /// <summary>
    /// Убирает юнит с игрового поля.
    /// </summary>
    /// <param name="unit">Юнит, который будет стёрт с игрового поля.</param>
    public void RemoveUnitFromPlayField()
    {
        GameData.unitLayer.SetTile(coordinates, null);
    }

    /// <returns>Координаты юнита</returns>
    public Vector3Int GetCoordinates()
    {
        return coordinates;
    }

    /// <returns>Имя юнита.</returns>
    public string GetName()
    {
        return Name;
    }


    /// <returns>Специализация юнита.</returns>
    public string GetSpecialization()
    {
        return Specialization;
    }

    /// <returns>Текущее значение очков жизней.</returns>
    public int GetCurrentHP()
    {
        return currentHP;
    }

    /// <returns>Максимально возможное значение очков жизней.</returns>
    public int GetMaxHP()
    {
        return maxHP;
    }

    /// <returns>Текущее значение очков действия.</returns>
    public int GetCurrentAP()
    {
        return currentAP;
    }

    /// <returns>Накопленные юнитом очки опыта.</returns>
    public int GetCollectedExperience()
    {
        return collectedExperience;
    }

    /// <returns>Количесвто очков, необходимых для перехода на следующий уровень. </returns>
    public int GetExperienceRequiredForNextLevel()
    {
        return experienceRequiredForLevel[currentLevel+1];
    }

    /// <returns>Значение наносимого юнитом урона при атаке.</returns>
    public int GetStrength()
    {
        return strength;
    }

    /// <returns>Очки брони юнита.</returns>
    public int GetArmor()
    {
        return armor;
    }

    /// <summary>
    /// Проверяет, остались ли у юнита очки действий.
    /// </summary>
    /// <returns>true, если у юнита остались очки действия. Иначе - false.</returns>
    public bool HasAP()
    {
        if (currentAP > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <returns>Тайл юнита.</returns>
    public Tile GetTile()
    {
        return unitTile;
    }

    /// <summary>
    /// Устанавливает тайл юнита.
    /// </summary>
    /// <param name="newTile">Тайл, которым будет заменен тайл юнита.</param>
    public void SetUnitTile(Tile newTile)
    {
        unitTile = newTile;
    }
}

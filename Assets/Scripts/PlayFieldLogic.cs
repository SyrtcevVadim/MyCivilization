using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayFieldLogic : MonoBehaviour
{
    const int LEFT_BUTTON_CLICK = 0;
    const int RIGHT_BUTTON_CLICK = 1;

    /// <summary>
    /// Координаты ячейки карты, на которую пользователь навел курсором мыши.
    /// </summary>
    private static Vector3Int selectedTileCoordinates;

    /// <summary>
    /// Список вражеских юнитов.
    /// </summary>
    List<EnemyComputer> listOfEnemies;

    private void Awake()
    {
        // Создаем текущего игрока.
        listOfEnemies = new List<EnemyComputer>();
        listOfEnemies.Add(new EnemyComputer());
        Player.Init();
    }
    private void Start()
    {
        Player.SetTestCityAndUnitKit();
        foreach(EnemyComputer enemy in listOfEnemies)
        {
            enemy.SetTestCityAndUnitKit();
        }
    }

    /// <summary>
    /// Перемещает камеру к точке.
    /// </summary>
    /// <param name="pointCoordinates">Координаты точки, к которой перемещается камера.</param>
    private void MoveCameraToPoint(Vector3Int pointCoordinates)
    {
        Vector3 cameraMovement = pointCoordinates - Camera.main.transform.position;
        // После вычисления перемещения информация о координате z теряется
        // Координата z должна быть равна -10, чтобы камера могла видеть все объекты
        cameraMovement.z = -10;
        Camera.main.transform.Translate(cameraMovement);
    }

    /// <summary>
    /// Проверяем, кликнул ли пользователь по своему городу. Если кликнул, обрабатываем нажатие.
    /// </summary>
    /// <param name="coordinates">Координаты ячейки, в которую пользователь кликнул ЛКМ.</param>
    private void ProcessClickOnPlayerCity(Vector3Int coordinates)
    {
        // Пробегаемся по списку городов и проверяем нажатия пользователем по одному из городов.
        foreach (City city in Player.listOfCities)
        {
            // Если пользователь нажал на какой-либо из своих городов
            if (city.GetCoordinates() == coordinates)
            {
                // Если данный город уже выбран, ничего не делаем
                if(Player.selectedCity != city)
                {
                    // Отмечаем данный город как выбранный
                    Player.SelectCity(city);
                    // Перемещаем к выбранному городу камеру.
                    MoveCameraToPoint(coordinates);
                }
                
            }
        }
    }


    /// <summary>
    /// Проверяем, кликнул ли пользователь по своему юниту. Если кликнул, обрабатываем нажатие.
    /// </summary>
    /// <param name="coordinates"></param>
    private void ProcessClickOnPlayerUnit(Vector3Int coordinates)
    {
        foreach (Unit unit in Player.listOfUnits)
        {
            // Если пользователь щелкнул ЛКМ по юниту, то он его выбирает. 
            if (unit.GetCoordinates() == coordinates)
            {
                Player.SelectUnit(unit);
            }
        }
    }


    /// <summary>
    /// Проверяет, существует ли по данным координатам клетка игрового поля
    /// </summary>
    /// <param name="tileCoordinates">Координаты клетки для проверки</param>
    /// <returns>Если клетка существует, true. Иначе - false</returns>
    public static bool IsTileExists(Vector3Int tileCoordinates)
    {
        if (GameData.terrainLayer.GetTile(tileCoordinates) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Считает количество очков действия для того, чтобы переместить юнит в данную ячейку.
    /// </summary>
    /// <param name="coordinates">Координаты ячейки</param>
    /// <returns>Количество очков действия, которые требуются для перемещения в данную ячейку</returns>
    public static int GetTileAPRequirments(Vector3Int coordinates)
    {
        Tile tile = (Tile)GameData.terrainLayer.GetTile(coordinates);
        if (tile == null)
        {
            return -1;
        }
        // Проверяем, не требует ли данная ячейка 1 ОД
        foreach (string item1AP in GameData.tilesRequired1AP)
        {
            if(item1AP == tile.name)
            {
                return 1;
            }
        }
        foreach(string item2AP in GameData.tilesRequired2AP)
        {
            if(item2AP == tile.name)
            {
                return 2;
            }
        }
        
        return 3;
    }

    /// <summary>
    /// Просчитываем координаты соседних клеток для текущей клетки с координатами coordinates.
    /// </summary>
    /// <param name="coordinates">Координаты текущей ячейки</param>
    /// <returns>Массив из координат соседних для текущей клеток.</returns>
    public static Vector3Int[] GetListOfNeighbourTiles(Vector3Int coordinates)
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
        return new Vector3Int[] { a, b, c, d, e, f };
    }

    /// <summary>
    /// Получает позицию курсора в мировых координатах.
    /// </summary>
    /// <returns>Возвращает позицию курсора в мировых координатах.</returns>
    private static Vector3 GetMouseWorldCoordinates()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// Получает координаты в координаты ячейки карты.
    /// </summary>
    /// <param name="coordinates">Координаты ячейки, соответствующей мировым координатам.</param>
    /// <returns></returns>
    private static Vector3Int GetTileCoordinates(Vector3 coordinates)
    {
        return GameData.terrainLayer.WorldToCell(coordinates);
    }

    /// <summary>
    /// Выделяет ячейку с координатами coordinates.
    /// </summary>
    /// <param name="coordinates">Координаты выделяемой ячейки.</param>
    private static void SelectTile(Vector3Int coordinates)
    {
        if(selectedTileCoordinates == null)
        {
            selectedTileCoordinates = coordinates;
            GameData.selectTileLayer.SetTile(selectedTileCoordinates, GameData.selectedTile);
        }
        else
        {
            GameData.selectTileLayer.SetTile(selectedTileCoordinates, null);
            selectedTileCoordinates = coordinates;
            GameData.selectTileLayer.SetTile(selectedTileCoordinates, GameData.selectedTile);
        }
    }

    private void Update()
    {
        
        Vector3 mouseWorldCoordinates = GetMouseWorldCoordinates();         // Получаем положение курсора мыши в мировых координатах
        Vector3Int tileCoordinates = GetTileCoordinates(mouseWorldCoordinates); // Конвертируем позицию этой точки в координаты ячейки, в которой она содержится

        // При наведении курсора на ячейку поля, на нее накладывается выделение в виде шестиугольной рамки.
        if (IsTileExists(tileCoordinates))
        {
            SelectTile(tileCoordinates);
        }

        // По нажатию клавиши Esc открывается внутриигровое меню
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuPanelLogic.IsMenuPanelActive = true;
            MenuPanelLogic.menuBackground.SetActive(true);
        }
        // Все действия игроком на игровом поле обрабатываются, если внутриигровое меню неактивно.
        if (!MenuPanelLogic.IsMenuActive())
        {
            StatusBarLogic.UpdateStatusBar();
            // Обрабатываем нажатия пользователем левой кнопкой мыши по городам, юнитам и ландшафту
            // С помощью левой кнопки мыши пользователь может выбрать юнита для передвижения
            if (Input.GetMouseButtonDown(LEFT_BUTTON_CLICK))
            {
                // Проверяем, кликнул ли пользователь ЛКМ по одному из своих городов:
                ProcessClickOnPlayerCity(tileCoordinates);
                // Проверяем, кликнул ли пользователь ЛКМ по одному из своих юнитов:
                ProcessClickOnPlayerUnit(tileCoordinates);
            }

            // С помощью правой кнопки мыши юнитов можно будет передвигать
            if (Input.GetMouseButtonDown(RIGHT_BUTTON_CLICK))
            {
                if (Player.selectedUnit != null && Player.selectedUnit.IsMovingPossible(tileCoordinates))
                {
                    // Если юнит выбран и движение в выбранную клетку возможно, производим перемещение юнита
                    Player.selectedUnit.Move(tileCoordinates);     // Производим перемещение юнита в указанную клетку
                    UnitInfoPanelLogic.UpdateUnitInfo(Player.selectedUnit);   // Обновляем информацию о юните в UnitInfoPanel
                    Player.selectedUnit.HideTilesForMoving();  // После движения убираем сетку возможных тайлов для передвижения
                    Player.selectedUnit.SetTilesForMoving();   // Просчитываем новую сетку возможных передвижений
                    if (Player.selectedUnit.HasAP())
                    {
                        Player.selectedUnit.ShowTilesForMoving();  // Вновь отрисовываем сетку возможных тайлов для перемещения
                    }
                }
            }
        }
    }
}

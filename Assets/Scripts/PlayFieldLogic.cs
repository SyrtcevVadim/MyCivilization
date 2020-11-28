using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayFieldLogic : MonoBehaviour
{
    /// <summary>
    /// Список городов государства игрока
    /// </summary>
    public static List<City> listOfPlayerCities;
    /// <summary>
    /// Список юнитов годударства игрока
    /// </summary>
    public static List<Human> listOfPlayerUnits;
    /// <summary>
    /// Список вражеских юнитов 
    /// </summary>
    public static List<Human> listOfEnemyUnits;
    /// <summary>
    /// Список городов противника.
    /// </summary>
    public static List<Human> listOfEnemyCities;
    
    /// <summary>
    /// Выбранный игроком юнит.
    /// </summary>
    public static Human selectedUnit;
    /// <summary>
    /// Выбранный игроком город.
    /// </summary>
    public static City selectedCity;

    /// <summary>
    /// Создает город игрока на карте.
    /// </summary>
    /// <param name="coordinates">Координаты города.</param>
    /// <param name="name">Имя города.</param>
    /// <param name="isCapital">Флаг столицы.</param>
    private static void CreateCity(string name, Vector3Int coordinates, bool isCapital=false)
    {
        City newCity = new City(name, coordinates, isCapital);      // Создаем город
        listOfPlayerCities.Add(newCity);                            // Добавляем его в список городов
        GameData.townLayer.SetTile(coordinates, newCity.cityTile);  // Отображаем данный город на карте
        newCity.SetTerritory();                                     // Просчитываем территорию города
        newCity.ShowTerritory();                                    // Отображаем территорию города
    }

    /// <summary>
    /// Создает юнит игрока на карте.
    /// </summary>
    /// <param name="coordinates">Координаты юнита.</param>
    public static void CreateUnit(Vector3Int coordinates)
    {
        Human newHuman = new Human(coordinates);                    // Создаем юнит
        listOfPlayerUnits.Add(newHuman);                            // Добавляем его в список юнитов игрока
        GameData.unitLayer.SetTile(coordinates, newHuman.unitTile); // Отрисовываем юнит на карте
    }

    private void Awake()
    {
        listOfPlayerCities = new List<City>();  // Выделяем память для хранения списка городов
        listOfPlayerUnits = new List<Human>();        // Выделяем память для хранения списка дружественных юнитов
        listOfEnemyUnits = new List<Human>();      // Выделяем память для хранения списка вражескию юнитов
    }
    private void Start()
    {
        // Создаем город-столицу
        CreateCity("Moscow", new Vector3Int(1, 2, 0), true);
        // Создадим еще два города-провинции
        CreateCity("Orel", new Vector3Int(-2, 2, 0));
        CreateCity("Kursk", new Vector3Int(-1, -3, 0));

        // Рядом с каждым городом поставим юнита-человека
        CreateUnit(new Vector3Int(2, 2, 0));
        CreateUnit(new Vector3Int(-3, 2, 0));
        CreateUnit(new Vector3Int(0, -3, 0));
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
    /// Отмечает город, как выбранный. У выбранного города выводится вся информация в правой части экрана.
    /// </summary>
    /// <param name="city">Город, который теперь считается выбранным.</param>
    private void SelectCity(City city)
    {
        selectedCity = city;
        CityInfoPanelLogic.UpdateCityInfo(selectedCity);
    }

    /// <summary>
    /// Проверяем, кликнул ли пользователь по своему городу. Если кликнул, обрабатываем нажатие.
    /// </summary>
    /// <param name="coordinates">Координаты ячейки, в которую пользователь кликнул ЛКМ.</param>
    private void ProcessClickOnPlayerCity(Vector3Int coordinates)
    {
        // Пробегаемся по списку городов и проверяем наличие нажатия пользователем по одному из городов.
        foreach (City city in listOfPlayerCities)
        {
            // Если пользователь нажал на какой-либо из своих городов
            if (city.Coordinates == coordinates)
            {
                // Отмечаем данный город как выбранный
                SelectCity(city);
                // Перемещаем к выбранному городу камеру.
                MoveCameraToPoint(coordinates);
                
            }
        }
    }

    /// <summary>
    /// Отмечает юнит как выбранный.
    /// </summary>
    /// <param name="unit">Выбранный юнит.</param>
    private void SelectUnit(Human unit)
    {
        // Если игрок уже выбирал какой-то юнит
        if(selectedUnit != null)
        {
            // Скрываем сетку возможных перемещений для предыдущего выбранного юнита
            selectedUnit.HideTilesForMoving();
        }
        selectedUnit = unit;    // Отмечаем, что юнит выбран
        // О выбранном юните в левом нижнем углу экрана выводится информация
        UnitInfoPanelLogic.UpdateUnitInfo(selectedUnit);
        // Если у выбранного юнита есть очки перемещения, надо отобразить сетку возможных перемещений.
        if(selectedUnit.ActionPoint > 0)
        {
            // Просчитываем сетку возможных передвижений
            selectedUnit.SetTilesForMoving();
            // Отображаем данную сетку на игровом поле
            selectedUnit.ShowTilesForMoving();
        }
        
    }

    /// <summary>
    /// Проверяем, кликнул ли пользователь по своему юниту. Если кликнул, обрабатываем нажатие.
    /// </summary>
    /// <param name="coordinates"></param>
    private void ProcessClickOnPlayerUnit(Vector3Int coordinates)
    {
        foreach (Human human in listOfPlayerUnits)
        {
            // Если пользователь щелкнул ЛКМ по юниту, то он его выбирает. 
            if (human.Coordinates == coordinates)
            {
                SelectUnit(human);
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
        if (GameData.baseLayer.GetTile(tileCoordinates) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {
        // Все действия игроком на игровом поле обрабатываются, если внутриигровое меню неактивно.
        if (!MenuPanelLogic.IsMenuActive())
        {
            StatusBarLogic.UpdateStatusBar();
            // Обрабатываем нажатия пользователем левой кнопкой мыши по городам, юнитам и ландшафту
            // С помощью левой кнопки мыши пользователь может выбрать юнита для передвижения
            if (Input.GetMouseButtonDown(0))
            {
                // Получаем координаты точки в пространстве, в которую пользователь кликнул
                Vector3 clickedWorldCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Конвертируем позицию этой точки в координаты ячейки, в которой она содержится
                Vector3Int clickedTileCoordinates = GameData.baseLayer.WorldToCell(clickedWorldCoordinates);

                // Проверяем, кликнул ли пользователь ЛКМ по одному из своих городов:
                ProcessClickOnPlayerCity(clickedTileCoordinates);
                // Проверяем, кликнул ли пользователь ЛКМ по одному из своих юнитов:
                ProcessClickOnPlayerUnit(clickedTileCoordinates);
            }

            // С помощью правой кнопки мыши юнитов можно будет передвигать
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 clickedWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int destination = GameData.baseLayer.WorldToCell(clickedWorldPosition);
                if (selectedUnit!= null && selectedUnit.IsMovingPossible(destination))
                {
                    // Если юнит выбран и движение в выбранную клетку возможно, производим перемещение юнита
                    selectedUnit.Move(destination);     // Производим перемещение юнита в указанную клетку
                    UnitInfoPanelLogic.UpdateUnitInfo(selectedUnit);   // Обновляем информацию о юните в UnitInfoPanel
                    selectedUnit.HideTilesForMoving();  // После движения убираем сетку возможных тайлов для передвижения
                    selectedUnit.SetTilesForMoving();   // Просчитываем новую сетку возможных передвижений
                    if (selectedUnit.ActionPoint > 0)
                    {
                        selectedUnit.ShowTilesForMoving();  // Вновь отрисовываем сетку возможных тайлов для перемещения
                    }
                }
            }
        }
    }
}

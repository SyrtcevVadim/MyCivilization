using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class UILogic : MonoBehaviour
{
    /// <summary>
    /// Кнопка окончания текущего хода.
    /// </summary>
    private static Button endTurnButton;
    /// <summary>
    /// Кнопка вызова внутриигрового меню настроек.
    /// </summary>
    private static Button openMenuButton;
    
    
    private void Awake()
    {
        endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        openMenuButton = GameObject.Find("OpenMenuButton").GetComponent<Button>();
    }

    /// <summary>
    /// Обновляет информацию об общем приросте науки по всем городам игрока.
    /// </summary>
    /// <param name="listOfPlayerCities">Список городов игрока.</param>
    public static void UpdateTotalScienceGrowth(List<City> listOfPlayerCities)
    {
        int totalScienceGrowth = 0;
        foreach(City city in listOfPlayerCities)
        {
            totalScienceGrowth += city.scienceGrowth;
        }
        GameData.totalScienceGrowth = totalScienceGrowth;
    }

    /// <summary>
    /// Обновляет информацию об общем приросте золота по всем городам игрока.
    /// </summary>
    /// <param name="listOfPlayerCities">Список городов игрока.</param>
    public static void UpdateTotalGoldGrowth(List<City> listOfPlayerCities)
    {
        int totalGoldGrowth = 0;
        foreach(City city in listOfPlayerCities)
        {
            totalGoldGrowth += city.goldGrowth;
        }
        GameData.totalGoldGrowth = totalGoldGrowth;
    }

    /// <summary>
    /// Наращиваем общий запас золота на значение общего прироста золота в ход.
    /// </summary>
    private void UpdateTotalGoldReserve()
    {
        GameData.totalGoldReserve += GameData.totalGoldGrowth;
    }

    /// <summary>
    /// Обновляет очки действия у всех юнитов игрока. У каждого юнита устанавливается 2 очка действия.
    /// </summary>
    /// <param name="listOfPlayerUnits">Список юнитов игрока.</param>
    private void RestorePlayerUnitActionPoints(List<Human> listOfPlayerUnits)
    {
        foreach(Human unit in listOfPlayerUnits)
        {
            unit.RestoreActionPoints();
        }
    }

    /// <summary>
    /// Наращивает счетчик текущего хода на 1.
    /// </summary>
    private void IncreaseCurrentTurnCounter()
    {
        GameData.currentTurnCounter++;
    }

    /// <summary>
    /// Вызывается при нажатии пользователем на кнопку окончания хода endTurnButton. Обрабатывает процесс окончания хода
    /// </summary>
    public void OnEndTurnButtonClick()
    {

        UpdateTotalScienceGrowth(Player.listOfCities);
        UpdateTotalGoldGrowth(Player.listOfCities);

        // После окончания хода у всех юнитов восстанавливаются очки действий(action points)
        RestorePlayerUnitActionPoints(Player.listOfUnits);

        // Наращивается счетчик текущего хода
        IncreaseCurrentTurnCounter();

        // Если какой-то юнит оставлся активным на момент окончания хода, обновим о нем информацию.
        if(UnitInfoPanelLogic.unitInfoPanel.activeSelf)
        {
            UnitInfoPanelLogic.UpdateUnitInfo(Player.selectedUnit);
            Player.selectedUnit.SetTilesForMoving();
            Player.selectedUnit.ShowTilesForMoving();
        }

        // Увеличиваем счетчик запасов золота
        UpdateTotalGoldReserve();
        StatusBarLogic.UpdateStatusBar();
        foreach(City city in Player.listOfCities)
        {
            city.totalProductionValue += city.productionGrowth;
        }
        if(Player.selectedCity != null)
        {
            CityInfoPanelLogic.UpdateCityInfo(Player.selectedCity);
        }
        
        
    }
    /// <summary>
    /// Вызывается при нажатии пользователем на кнопку вызова игрового меню openMenuButton.
    /// </summary>
    public void OnOpenMenuButtonClick()
    {

    }

   
}

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
    /// Обновляет очки действия у всех юнитов игрока. У каждого юнита устанавливается 2 очка действия.
    /// </summary>
    /// <param name="listOfPlayerUnits">Список юнитов игрока.</param>
    private void RestorePlayerUnitActionPoints(List<Unit> listOfPlayerUnits)
    {
        foreach(Unit unit in listOfPlayerUnits)
        {
            unit.RestoreUnitAP();
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
        // Наращивается счетчик текущего хода
        IncreaseCurrentTurnCounter();
        Player.UpdatePlayerData();
        // После окончания хода у всех юнитов восстанавливаются очки действий(action points)
        RestorePlayerUnitActionPoints(Player.listOfUnits);
        
        // Если какой-то юнит оставлся активным на момент окончания хода, обновим о нем информацию.
        if(UnitInfoPanelLogic.unitInfoPanel.activeSelf)
        {
            UnitInfoPanelLogic.UpdateUnitInfo(Player.selectedUnit);
            Player.selectedUnit.SetTilesForMoving();
            Player.selectedUnit.ShowTilesForMoving();
        }
        
        StatusBarLogic.UpdateStatusBar();
        foreach(City city in Player.listOfCities)
        {
            city.GenerateProduction();
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

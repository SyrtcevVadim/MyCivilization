using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityInfoPanelLogic : MonoBehaviour
{
    /// <summary>
    /// Панель для отображения всей информации о городе
    /// </summary>
    public static GameObject cityInfoPanel;

    /// <summary>
    /// Метка для отображения имени города
    /// </summary>
    static Text cityNameLabel;

    /// <summary>
    /// Метка для отображения популяции города
    /// </summary>
    static Text populationCounterLabel;

    /// <summary>
    /// Метка для отображения прироста производства в ход
    /// </summary>
    static Text productionGrowthLabel;

    /// <summary>
    /// Метка для отображения прироста золота в ход
    /// </summary>
    static Text goldGrowthLabel;

    /// <summary>
    /// Метка для отображения прироста науки в ход
    /// </summary>
    static Text scienceGrowthLabel;
    /// <summary>
    /// TODO
    /// </summary>
    static GameObject notEnoughProductionForPurchase;

    /// <summary>
    /// Метка для обозначения текущей накопленной продукции в городе
    /// </summary>
    static Text totalProductionValueLabel;

    private void Awake()
    {
        cityNameLabel = GameObject.Find("CityNameLabel").GetComponent<Text>();
        populationCounterLabel = GameObject.Find("PopulationCounterLabel").GetComponent<Text>();
        productionGrowthLabel = GameObject.Find("ProductionGrowthLabel").GetComponent<Text>();
        goldGrowthLabel = GameObject.Find("GoldGrowthLabel").GetComponent<Text>();
        scienceGrowthLabel = GameObject.Find("CityScienceGrowthLabel").GetComponent<Text>();
        totalProductionValueLabel = GameObject.Find("TotalProductionValueLabel").GetComponent<Text>();
        notEnoughProductionForPurchase = GameObject.Find("NotEnoughProductionForPurchase");
        notEnoughProductionForPurchase.SetActive(false);
        cityInfoPanel = GameObject.Find("CityInfoPanel");
        cityInfoPanel.SetActive(false);
    }

    /// <summary>
    /// Обновляет панель информации о городе.
    /// </summary>
    /// <param name="city">Город, информация о котором обновляется.</param>
    public static void UpdateCityInfo(City city)
    {
        cityNameLabel.text = string.Format("City name:{0}", city.Name);
        populationCounterLabel.text = string.Format("Population:{0}", city.populationCounter);
        productionGrowthLabel.text = string.Format("Production:+{0}", city.productionGrowth);
        goldGrowthLabel.text = string.Format("Gold:+{0}", city.goldGrowth);
        scienceGrowthLabel.text = string.Format("Science:+{0}", city.scienceGrowth);
        totalProductionValueLabel.text = string.Format("Total production: {0}|Max:{1}", city.totalProductionValue, city.maxPossibleProductionValue);
        notEnoughProductionForPurchase.SetActive(false);
        cityInfoPanel.SetActive(true);
    }
    
    /// <summary>
    /// Вызывается при нажатии пользователем на кнопку создания юнита.
    /// </summary>
    public void OnCreateButtonClick()
    {
        // Если игроку не хватает ресурсов на создание юнита:
        if (PlayFieldLogic.selectedCity.totalProductionValue < Human.costInProductionPoints)
        {
            notEnoughProductionForPurchase.SetActive(true);
        }
        else
        {
            notEnoughProductionForPurchase.SetActive(false);
            bool isEmpty = true;
            // не позволяем заказать новый юнит ,если на клетке города стоит какой-то юнит
            foreach (Human unit in PlayFieldLogic.listOfPlayerUnits)
            {
                if (PlayFieldLogic.selectedCity.Coordinates == unit.Coordinates)
                {
                    isEmpty = false;
                }
            }
            if (isEmpty)
            {
                // Создаем новый юнит
                //GameData.unitLayer.SetTile(PlayFieldLogic.selectedCity.Coordinates, GameData.unitHumanTile);
                //Human newUnit = new Human("NewOne", PlayFieldLogic.selectedCity.Coordinates, GameData.unitHumanTile);
                //PlayFieldLogic.listOfUnits.Add(newUnit);
                
                PlayFieldLogic.CreateUnit(PlayFieldLogic.selectedCity.Coordinates);
                PlayFieldLogic.selectedCity.totalProductionValue -= Human.costInProductionPoints;
            }
            else
            {
                Debug.Log("There\'s a unit in town");
            }
           

        }
    }


    public  void OnCloseCityInfoPanelClick()
    {
        PlayFieldLogic.selectedCity = null;
        cityInfoPanel.SetActive(false);
    }
}

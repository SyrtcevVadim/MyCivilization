using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPanelLogic : MonoBehaviour
{
    /// <summary>
    /// Панель, на которой отображается вся информация о юните.
    /// </summary>
    public static GameObject unitInfoPanel;
    /// <summary>
    /// Метка, отображающая имя юнита.
    /// </summary>
    static Text unitNameLabel;
    /// <summary>
    /// Метка, отображающая класс юнита.
    /// </summary>
    static Text unitClassLabel;
    /// <summary>
    /// Метка, отображающая количество очков действия юнита.
    /// </summary>
    static Text unitActionPointLabel;

    /// <summary>
    /// Метка, отображающая силу юнита.
    /// </summary>
    static Text unitStrengthLabel;
    /// <summary>
    /// Метка, отображающая количество очков здоровья юнита.
    /// </summary>
    static Text unitToughnessLabel;
    /// <summary>
    /// Метка, отображающая количество очков бронирования юнита.
    /// </summary>
    static Text unitArmorLabel;
    private void Awake()
    {
        unitNameLabel = GameObject.Find("SelectedUnitNameLabel").GetComponent<Text>();
        unitClassLabel = GameObject.Find("SelectedUnitClassLabel").GetComponent<Text>();
        unitActionPointLabel = GameObject.Find("ActionPointLabel").GetComponent<Text>();
        unitStrengthLabel = GameObject.Find("StrengthLabel").GetComponent<Text>();
        unitToughnessLabel = GameObject.Find("ToughnessLabel").GetComponent<Text>();
        unitArmorLabel = GameObject.Find("ArmorLabel").GetComponent<Text>();
        unitInfoPanel = GameObject.Find("UnitInfoPanel");
        unitInfoPanel.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    public static void UpdateUnitInfo(Human unit)
    {
        unitNameLabel.text = string.Format("Name: {0}", unit.Name);     
        unitClassLabel.text = string.Format("Class: {0}", "Human");        
        unitActionPointLabel.text = unit.ActionPoint.ToString();               
        unitStrengthLabel.text = unit.strength.ToString();                     
        unitToughnessLabel.text = unit.toughness.ToString();                   
        unitArmorLabel.text = unit.armor.ToString();                           
        unitInfoPanel.SetActive(true);                                             
    }

    /// <summary>
    /// Вызывается при нажатии на кнопку закрытия меню информации о выбранном юните.
    /// </summary>
    public void OnCloseUnitInfoButtonClick()
    {
        Player.UnselectUnit();
    }
}

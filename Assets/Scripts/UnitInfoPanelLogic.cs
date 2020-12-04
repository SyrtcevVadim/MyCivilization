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
    static Text maxPossibleHP;

    static Text collectedExperience;
    static Text experienceRequiredForNextLevel;

    static Text remainHP;
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
        maxPossibleHP = GameObject.Find("MaxPossibleHP").GetComponent<Text>();
        remainHP = GameObject.Find("RemainHP").GetComponent<Text>();
        collectedExperience = GameObject.Find("CollectedExperience").GetComponent<Text>();
        experienceRequiredForNextLevel = GameObject.Find("ExperienceRequiredForNextLevel").GetComponent<Text>();

        unitArmorLabel = GameObject.Find("ArmorLabel").GetComponent<Text>();
        unitInfoPanel = GameObject.Find("UnitInfoPanel");
        unitInfoPanel.SetActive(false);
    }
    /// <summary>
    /// Обновляет информацию в панели информации о выбранном юните
    /// </summary>
    /// <param name="unit">Юнит, информация которого отображается в панели</param>
    public static void UpdateUnitInfo(Human unit)
    {
        unitNameLabel.text = string.Format("Name: {0}", unit.Name);     
        unitClassLabel.text = string.Format("Class: {0}", "Human");        
        unitActionPointLabel.text = unit.ActionPoints.ToString();               
        unitStrengthLabel.text = unit.strength.ToString();                     
        maxPossibleHP.text = unit.maxHP.ToString();
        remainHP.text = unit.remainHP.ToString();
        collectedExperience.text = unit.collectedExperience.ToString();
        experienceRequiredForNextLevel.text = Human.requiredExperienceForLevel[unit.currentLevel + 1].ToString() ;
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

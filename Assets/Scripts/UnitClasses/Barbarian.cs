using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Barbarian : Unit
{
    /// <summary>
    /// Путь до файла с характеристиками юнита-варвара.
    /// </summary>
    string pathToCharacteristicsFile = @"UnitCharacteristics/Barbarian";

    /// <summary>
    /// Стоимость заказа юнита-варвара в городе в очках продукции.
    /// </summary>
    new public static int costInProductionPoints = 20;

    /// <summary>
    /// Устанавливает значения характеристик юнита-варвара.
    /// </summary>
    protected override void SetCharacteristics()
    {
        TextAsset characteristicsFile = Resources.Load<TextAsset>(pathToCharacteristicsFile);
        string str = characteristicsFile.text;
        string rawMaxHP = str.Substring(str.IndexOf("MaxHP:") + 6, str.IndexOf('\n', str.IndexOf("MaxHP:")) - (str.IndexOf("MaxHP:") + 6));
        string rawMaxAP = str.Substring(str.IndexOf("MaxAP:") + 6, str.IndexOf('\n', str.IndexOf("MaxAP:")) - (str.IndexOf("MaxAP:") + 6));
        string rawStrength = str.Substring(str.IndexOf("Strength:") + 9, str.IndexOf('\n', str.IndexOf("Strength:")) - (str.IndexOf("Strength:") + 9));
        string rawArmor = str.Substring(str.IndexOf("Armor:") + 6, str.IndexOf('\n', str.IndexOf("Armor:")) - (str.IndexOf("Armor:") + 6));

        Specialization = "Barbarian";
        maxHP = Convert.ToInt32(rawMaxHP);
        currentHP = maxHP;
        maxAP = Convert.ToInt32(rawMaxAP);
        currentAP = maxAP;
        armor = Convert.ToInt32(armor);
        strength = Convert.ToInt32(rawStrength);
    }

    /// <summary>
    /// Конструктор класса юнит-варвар.
    /// </summary>
    /// <param name="coordinates">Координаты позиции создаваемого юнита-варвара.</param>
    public Barbarian(Vector3Int coordinates):base(coordinates)
    {
        SetCharacteristics();
        unitTile = GameData.enemyUnitTile;
    }


    /// <summary>
    /// Конструктор класса юнит-варвар.
    /// </summary>
    /// <param name="coordiantes">Координаты позиции создаваемого юнита-варвара.</param>
    /// <param name="startAP">Начальное количество очков действия юнита-варвара.</param>
    public Barbarian(Vector3Int coordiantes, int startAP):base(coordiantes, startAP)
    {
        SetCharacteristics();
        currentAP = startAP;
        unitTile = GameData.enemyUnitTile;
    }
}

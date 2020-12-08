﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Класс, реализующий юнита-рабочего.
/// </summary>
public class Worker: Unit
{
    /// <summary>
    /// Стоимость заказа юнита-рабочего в городе в очках продукции.
    /// </summary>
    new public static int costInProductionPoints = 25;

    private string pathToCharacteristicsFile = @"UnitCharacteristics/Worker";

    /// <summary>
    /// Получает характеристики юнита.
    /// </summary>
    protected override void SetCharacteristics()
    {
        TextAsset characteristicsFile = Resources.Load<TextAsset>(pathToCharacteristicsFile);
        string str = characteristicsFile.text;
        string rawMaxHP = str.Substring(str.IndexOf("MaxHP:") + 6, str.IndexOf('\n', str.IndexOf("MaxHP:")) - (str.IndexOf("MaxHP:") + 6));
        string rawMaxAP = str.Substring(str.IndexOf("MaxAP:") + 6, str.IndexOf('\n', str.IndexOf("MaxAP:")) - (str.IndexOf("MaxAP:") + 6));
        string rawStrength = str.Substring(str.IndexOf("Strength:") + 9, str.IndexOf('\n', str.IndexOf("Strength:")) - (str.IndexOf("Strength:") + 9));
        string rawArmor = str.Substring(str.IndexOf("Armor:") + 6, str.IndexOf('\n', str.IndexOf("Armor:")) - (str.IndexOf("Armor:") + 6));
        Specialization = "Worker";
        maxHP = Convert.ToInt32(rawMaxHP);
        currentHP = maxHP;
        maxAP = Convert.ToInt32(rawMaxAP);
        currentAP = maxAP;
        armor = Convert.ToInt32(rawStrength);
        strength = Convert.ToInt32(rawStrength);


    }

    /// <summary>
    /// Конструктор объекта юнит-рабочий.
    /// </summary>
    /// <param name="coordinates">Координаты позиции юнита на карте.</param>
    public Worker(Vector3Int coordinates):base(coordinates)
    {
        // Устанавливаем характеристики юнита-рабочего
        // Неэффективно: при создании нового юнита происходит обращение в файл
        SetCharacteristics();
    }

    /// <summary>
    /// Конструктор объекта юнит-рабочий.
    /// </summary>
    /// <param name="coordinates">Координаты позиции юнита на карте.</param>
    /// <param name="startAP">Начальное число очков действия.</param>
    public Worker(Vector3Int coordinates, int startAP):base(coordinates, startAP)
    {
        Specialization = "Worker";
        maxHP = 10;
        currentHP = maxHP;
        maxAP = 3;
        currentAP = maxAP;
        armor = 0;
        strength = 0;
    }
}

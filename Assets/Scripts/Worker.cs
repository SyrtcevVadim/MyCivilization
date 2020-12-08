using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, реализующий юнита-рабочего.
/// </summary>
public class Worker: Unit
{
    /// <summary>
    /// Стоимость заказа юнита-рабочего в городе в очках продукции.
    /// </summary>
    new public static int costInProductionPoints = 25;

    /// <summary>
    /// Конструктор объекта юнит-рабочий.
    /// </summary>
    /// <param name="coordinates">Координаты позиции юнита на карте.</param>
    public Worker(Vector3Int coordinates):base(coordinates)
    {
        Specialization = "Worker";
        maxHP = 10;
        maxAP = 3;
        currentAP = maxAP;
        currentHP = maxHP;
        armor = 0;
        strength = 0;
        
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

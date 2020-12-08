using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit
{
    /// <summary>
    /// Стоимость заказа юнита-война в городе в очках продукции.
    /// </summary>
    new public static int costInProductionPoints = 40;

    /// <summary>
    /// Конструктор объекта юнит-воин.
    /// </summary>
    /// <param name="coordinates">Координаты позиции юнита на карте.</param>
    public Warrior(Vector3Int coordinates) : base(coordinates)
    {
        Specialization = "Warrior";
        maxHP = 100;
        maxAP = 2;
        currentAP = maxAP;
        currentHP = maxHP;
        armor = 3;
        strength = 30;
    }

    /// <summary>
    /// Конструктор объекта юнит-воин.
    /// </summary>
    /// <param name="coordinates">Координаты позиции юнита на карте.</param>
    /// <param name="startAP">Начальное число очков действия юнита.</param>
    public Warrior(Vector3Int coordinates, int startAP) : base(coordinates, startAP)
    {
        Specialization = "Warrior";
        maxHP = 100;
        currentHP = maxHP;
        maxAP = 2;
        currentAP = maxAP;
        armor = 3;
        strength = 30;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit
{
    new public static int costInProductionPoints = 40;

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

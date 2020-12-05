using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Worker: Unit
{
    public Worker(Vector3Int coordinates):base(coordinates)
    {
        Specialization = "Worker";
        maxHP = 10;
        currentHP = maxHP;
        armor = 0;
        strength = 0;
        
    }
    public Worker(Vector3Int coordinates, int startAP):base(coordinates, startAP)
    {

    }
}

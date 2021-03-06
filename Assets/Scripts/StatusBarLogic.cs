﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// Панель состояния игровых ресурсов игрока. Отображается в левом верхнем углу экрана
/// </summary>
public class StatusBarLogic : MonoBehaviour
{
    /// <summary>
    /// Метка для отображения номера текущего хода
    /// </summary>
    public static Text currentTurnLabel;
    /// <summary>
    /// метка для отображения общего золотого запаса игрока и прироста золота каждый ход
    /// </summary>
    public static Text goldReserveLabel;
    private void Awake()
    {
        // Получаем все метки из панели состояния
        currentTurnLabel = GameObject.Find("CurrentTurnLabel").GetComponent<Text>();
        goldReserveLabel = GameObject.Find("GoldReserveLabel").GetComponent<Text>();
    }
    private void Start()
    {
        // Устанавливаем начальное значение меток
        UpdateStatusBar();
    }

    /// <summary>
    /// Обновляет информацию на панели StatusBar.
    /// </summary>
    public static void UpdateStatusBar()
    {
        goldReserveLabel.text = string.Format("Gold:{0}|{1}{2}", Player.data.goldReserve, (Player.data.totalGoldGrowthPerTurn >= 0) ? "+" : "-", Player.data.totalGoldGrowthPerTurn);
        currentTurnLabel.text = string.Format("Turn:{0}", GameData.currentTurnCounter);
    }
    
}

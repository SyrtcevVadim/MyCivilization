using System;
using System.Collections.Generic;
public class Data
{
    /// <summary>
    /// Прирост золота в ход.
    /// </summary>
    private int goldGrowthPerTurn;

    /// <summary>
    /// Накопленное золото на текущий момент.
    /// </summary>
    private int goldReserve;

    /// <summary>
    /// Прирост науки в ход.
    /// </summary>
    private int scienceGrowthPerTurn;

    /// <summary>
    /// Создает объект Data.
    /// </summary>
    public Data()
    {
        // Изначально у игрока все параметры прироста и накопленных ресурсов нулевые
        goldGrowthPerTurn = 0;
        goldReserve = 0;
        scienceGrowthPerTurn = 0;
        
    }
}

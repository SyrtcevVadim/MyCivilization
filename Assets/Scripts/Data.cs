using System;
using System.Collections.Generic;
public class Data
{
    /// <summary>
    /// Прирост золота в ход.
    /// </summary>
    public int goldGrowthPerTurn;

    /// <summary>
    /// Накопленное золото на текущий момент.
    /// </summary>
    public int goldReserve;


    /// <summary>
    /// Создает объект Data.
    /// </summary>
    public Data()
    {
        // Изначально у игрока все параметры прироста и накопленных ресурсов нулевые
        goldGrowthPerTurn = 0;
        goldReserve = 0;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public static class Calculation
{
    private static Dictionary<int, string> reduce = new()
    {
        {1000, "K"},
        {1000000, "M"},
        {1000000000, "B"}
    };
    
    public static string GetReduceMoney(this float money)
    {
        string reduceMoney = money.ToString("#.##");

        foreach (var keyValue in reduce)
        {
            var reduceNumber = money / keyValue.Key;
            if (reduceNumber >= 1)
            {
                reduceMoney = $"{reduceNumber:#.##}{keyValue.Value}";
            }
        }

        return reduceMoney;
    }
}

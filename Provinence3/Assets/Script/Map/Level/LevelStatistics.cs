using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LevelStatistics
{
    public static bool GetReward(int monstersKilled, int questCompleted, Dictionary<CraftItemType, int> collectedCrafts,bool bossKilled)
    {
        var sumCrafts = collectedCrafts.Values.Sum();
        int total = monstersKilled + questCompleted*17 + (int)(sumCrafts*2.3f);
        total += bossKilled ? 10 : 0;
        return total > 100;
    } 
}


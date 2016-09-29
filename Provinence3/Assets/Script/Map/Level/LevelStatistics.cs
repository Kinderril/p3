using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LevelStatistics
{
    public List<PlayerItem> GetRewards(int monstersKilled, int questCompleted, Dictionary<CraftItemType, int> collectedCrafts)
    {
        List<PlayerItem> items = new List<PlayerItem>();
        int awardsCoef = 0;
        if (monstersKilled > 60)
        {
            awardsCoef += 3;
        }
        else if (monstersKilled > 35)
        {
            awardsCoef += 1;
        }

        if (questCompleted > 2)
        {
            awardsCoef += 2;
        }
        var sumCrafts = collectedCrafts.Values.Sum();
        if (sumCrafts > 20)
        {
            awardsCoef += 2;
        }


        return items;
    } 
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class OpenLevels
{
    private List<int> opendLevels = new List<int>();
    private readonly Dictionary<int, List<int>> listOfOpendBornPositions = new Dictionary<int, List<int>>();
    private const char DELEM = '$';
    private const string KEY_OPENED_LEVELS = "KEY_OPENED_LEVELS";
    private const string KEY_OPENED_POS = "KEY_OPENED_POS";
    public OpenLevels()
    {
        var isFirst = PlayerPrefs.GetString(KEY_OPENED_LEVELS, "").Length < 2;
        if (isFirst)
        {
            opendLevels.Add(1);
            opendLevels.Add(2);
            foreach (var opendLevel in opendLevels)
            {
                listOfOpendBornPositions.Add(opendLevel, new List<int>() {1});
            }
            Save();
        }
        else
        {
            Load();
        }
    }
    
    public void Save()
    {
        string openedLevels = "";
        foreach (var pos in listOfOpendBornPositions)
        {
            openedLevels += pos.Key.ToString() + DELEM;
            string openedPos = "";
            foreach (var index in pos.Value)
            {
                openedPos += index.ToString() + DELEM;
            }
            PlayerPrefs.SetString(KEY_OPENED_POS + pos.Key, openedPos);
        }
        PlayerPrefs.SetString(KEY_OPENED_LEVELS,openedLevels);
    }

    private void Load()
    {
        opendLevels = new List<int>();
        var openedLevels = PlayerPrefs.GetString(KEY_OPENED_LEVELS,"").Split(DELEM);
        foreach (var lvl in openedLevels)
        {
            if (lvl.Length != 0)
            {
                var index = Convert.ToInt32(lvl);
                opendLevels.Add(index);
                var positions = PlayerPrefs.GetString(KEY_OPENED_POS + index,"").Split(DELEM);
                listOfOpendBornPositions.Add(index, new List<int>());
                
                foreach (var pos in positions)
                {
                    if (pos.Length != 0)
                    {
                        listOfOpendBornPositions[index].Add(Convert.ToInt32(pos));
                    }
                }
            }
        }
    }

    public List<int> GetAllOpenedMissions()
    {
        return opendLevels;
    } 

    public List<int> GetAllBornPositions(int mission)
    {
        if (listOfOpendBornPositions.ContainsKey(mission))
            return listOfOpendBornPositions[mission];
        return new List<int>();
    }

    public bool IsPositionOpen(int misson, int index)
    {
        return listOfOpendBornPositions[misson].Contains(index);
    }

    public void OpenPosition(int missionIndex, int id)
    {
        if (listOfOpendBornPositions.ContainsKey(missionIndex))
        {
            listOfOpendBornPositions[missionIndex].Add(id);
        }
        Save();
    }
}


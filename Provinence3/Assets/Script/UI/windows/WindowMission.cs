﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowMission : BaseWindow
{
    private int currentSelectedRespawnPoint;
    private int currentSelectedMission = 1;
    public List<RespawnPointToggle> MissionsToggles;
    private List<RespawnPointToggle> RespawnToggles;

    public Transform Layout;
    public RespawnPointToggle PrefabRespawnPointToggle;
    public DifficultyChooser DifficultyChooser;
    private int curDiffChoosed;

    public override void Init()
    {
        base.Init();
        Dictionary<int, int> stubList = new Dictionary<int, int>();
        var openedMissions = MainController.Instance.PlayerData.OpenLevels.GetAllOpenedMissions();
        foreach (var t in MissionsToggles)
        {
            Debug.Log("Selected: " + t.transform.name);
            t.Toggle.onValueChanged.RemoveAllListeners();
            int a = t.ID;
            stubList.Add(a, t.ID);
            t.Toggle.onValueChanged.AddListener(arg0 =>
            {
                if (arg0)
                {
                    MissionSelected(stubList[a]);
                }
            });
            List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(t.ID);
       
            t.Toggle.isOn = t.ID == 1;
            t.Toggle.interactable = opensRespawnPoints.Count > 0;
        }
        DifficultyChooser.Init(OnDifChanges);
        MissionSelected(1);
    }

    private void OnDifChanges(int obj)
    {
        curDiffChoosed = obj;
    }

    private void MissionSelected(int mission)
    {
        var count = DataBaseController.Instance.DataStructs.GetRespawnPointsCountByMission(mission);
        List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(mission);
        RespawnToggles = new List<RespawnPointToggle>();
        var toggleGroup = Layout.GetComponent<ToggleGroup>();
        Utils.ClearTransform(Layout);
        for (int i = 1; i < count; i++)
        {
            var rpToggle = DataBaseController.GetItem<RespawnPointToggle>(PrefabRespawnPointToggle, Vector3.zero);
            RespawnToggles.Add(rpToggle);
            rpToggle.transform.SetParent(Layout);
            rpToggle.Toggle.group = toggleGroup;
            rpToggle.ID = i;
            rpToggle.text.text = "Respawn Stage " + i;
            rpToggle.Toggle.isOn = i == 1;
            Debug.Log("mission:" + mission + " Check for: " + i + "  " + opensRespawnPoints.Contains(i));
            rpToggle.Toggle.interactable = opensRespawnPoints.Contains(i);
        }
        currentSelectedRespawnPoint = mission;
        foreach (var respawnToggle in RespawnToggles)
        {
            respawnToggle.Toggle.onValueChanged.RemoveAllListeners();
            respawnToggle.Toggle.onValueChanged.AddListener(arg0 =>
            {
                if (arg0)
                {
                    currentSelectedRespawnPoint = respawnToggle.ID;
                }
            });
            if (respawnToggle.ID == 1)
            {
                respawnToggle.Toggle.isOn = true;
            }
        }

    }

    public void OnPlayClick()
    {
        int GetCurrentBornPosIndex = currentSelectedRespawnPoint;
        MainController.Instance.StartLevel(GetCurrentBornPosIndex,curDiffChoosed, currentSelectedMission);
    }

}


using System;
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
    private const string key_last_level_chosed = "key_last_level_chosed";
    

    public override void Init()
    {
        base.Init();
        Dictionary<int, int> stubList = new Dictionary<int, int>();
        var lastLevel = PlayerPrefs.GetInt(key_last_level_chosed, 1);
        var openedMissions = MainController.Instance.PlayerData.OpenLevels.GetAllOpenedMissions();
        foreach (var pointToggle in MissionsToggles)
        {
            Debug.Log("Selected: " + pointToggle.transform.name);
            pointToggle.text.text = pointToggle.ID.ToString();
            pointToggle.Toggle.onValueChanged.RemoveAllListeners();
            int a = pointToggle.ID;
            stubList.Add(a, pointToggle.ID);
            pointToggle.Toggle.onValueChanged.AddListener(arg0 =>
            {
                if (arg0)
                {
                    MissionSelected(stubList[a]);
                }
            });
            List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(pointToggle.ID);
       
            pointToggle.Toggle.isOn = pointToggle.ID == lastLevel;
            pointToggle.Toggle.interactable = opensRespawnPoints.Count > 0;
        }
        DifficultyChooser.Init(OnDifChanges);
        MissionSelected(lastLevel);
    }

    private void OnDifChanges(int obj)
    {
        curDiffChoosed = obj;
    }

    private void MissionSelected(int mission)
    {
        currentSelectedMission = mission;
        PlayerPrefs.SetInt(key_last_level_chosed, mission);
        var count = DataBaseController.Instance.DataStructs.GetRespawnPointsCountByMission(mission) + 1;
        List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(mission);
        RespawnToggles = new List<RespawnPointToggle>();
        var toggleGroup = Layout.GetComponent<ToggleGroup>();
        Utils.ClearTransform(Layout);
        var names = DataBaseController.Instance.RespawnPositionsNames[mission];
        for (int i = 1; i < count; i++)
        {
            var rpToggle = DataBaseController.GetItem<RespawnPointToggle>(PrefabRespawnPointToggle, Vector3.zero);
            RespawnToggles.Add(rpToggle);
            rpToggle.transform.SetParent(Layout);
            rpToggle.Toggle.group = toggleGroup;
            rpToggle.ID = i;
            rpToggle.text.text = names[i];
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
        MainController.Instance.StartLevel(currentSelectedRespawnPoint, curDiffChoosed, currentSelectedMission);
    }

}


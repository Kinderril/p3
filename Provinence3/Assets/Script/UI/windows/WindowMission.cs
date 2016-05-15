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
    public Button StartMisson;
    public Button BuyMission;
    public Text CostMissionField;

    public RespawnPointToggle PrefabRespawnPointToggle;
    public DifficultyChooser DifficultyChooser;
    private int curDiffChoosed;
    private const string key_last_level_chosed = "key_last_level_chosed";
    

    public override void Init()
    {
        base.Init();
        Dictionary<int, int> stubList = new Dictionary<int, int>();
        var lastLevel = PlayerPrefs.GetInt(key_last_level_chosed, 1);
//        var openedMissions = MainController.Instance.PlayerData.OpenLevels.GetAllOpenedMissions();
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
                    var id = stubList[a];
                    List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(id);

                    MissionSelected(stubList[a], opensRespawnPoints.Count > 0);
                }
            });
            
            pointToggle.Toggle.isOn = pointToggle.ID == lastLevel;
//            pointToggle.Toggle.interactable = opensRespawnPoints.Count > 0;
        }
        DifficultyChooser.Init(OnDifChanges);
        MissionSelected(lastLevel,true);
    }

    private void OnDifChanges(int obj)
    {
        curDiffChoosed = obj;
    }

    private void MissionSelected(int mission,bool canactivate)
    {
        currentSelectedMission = mission;
        PlayerPrefs.SetInt(key_last_level_chosed, mission);
        var count = DataBaseController.Instance.DataStructs.GetRespawnPointsCountByMission(mission) + 1;
        List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(mission);
        RespawnToggles = new List<RespawnPointToggle>();
        var toggleGroup = Layout.GetComponent<ToggleGroup>();
        Utils.ClearTransform(Layout);
        if (canactivate)
        {
            var names = DataBaseController.Instance.RespawnPositionsNames[mission];
            for (int i = 1; i < count; i++)
            {
                var rpToggle = DataBaseController.GetItem<RespawnPointToggle>(PrefabRespawnPointToggle, Vector3.zero);
                RespawnToggles.Add(rpToggle);
                rpToggle.transform.SetParent(Layout, false);
                rpToggle.Toggle.group = toggleGroup;
                rpToggle.ID = i;
                rpToggle.text.text = names[i];
                Debug.Log("mission:" + mission + " Check for: rpToggle.ID:" + rpToggle.ID + "  " +
                          opensRespawnPoints.Contains(i));
                rpToggle.Toggle.interactable = opensRespawnPoints.Contains(i);

                rpToggle.Toggle.onValueChanged.RemoveAllListeners();
                rpToggle.Toggle.onValueChanged.AddListener(arg0 =>
                {
                    if (arg0)
                    {
                        currentSelectedRespawnPoint = rpToggle.ID;
                        Debug.Log("currentSelectedRespawnPoint selected:" + currentSelectedRespawnPoint);
                    }
                });
                rpToggle.Toggle.isOn = rpToggle.ID == 1;
                if (rpToggle.Toggle.isOn)
                {
                    currentSelectedRespawnPoint = rpToggle.ID;
                }
            }
        }
        StartMisson.gameObject.SetActive(canactivate);
        BuyMission.gameObject.SetActive(!canactivate);
        if (!canactivate)
        {
            CostMissionField.text = "10 crystals";
        }
        Debug.Log("currentSelectedRespawnPoint:" + currentSelectedRespawnPoint);

    }

    public void OnOpenLvlClick()
    {
        var toPoen = currentSelectedMission;
    }

    public void OnPlayClick()
    {
        MainController.Instance.StartLevel(currentSelectedRespawnPoint, curDiffChoosed, currentSelectedMission);
    }

}


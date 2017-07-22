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
    public Button DebugStartMisson;
//    public Text CostMissionField;

    public OpenLevelWindow OpenLevelWindow;
    public RespawnPointToggle PrefabRespawnPointToggle;
    public DifficultyChooser DifficultyChooser;
    private int curDiffChoosed;
    private const string key_last_level_chosed = "key_last_level_chosed";
    

    public override void Init()
    {
        base.Init();
        InitLevels();
    }

    private void InitLevels()
    {

        DebugStartMisson.gameObject.SetActive(false);
#if DEBUG
        DebugStartMisson.gameObject.SetActive(true);
#endif
        OpenLevelWindow.gameObject.SetActive(false);
        var isTutorComplete = MainController.Instance.PlayerData.IsTutorialComplete;
        Dictionary<int, int> stubList = new Dictionary<int, int>();
        var lastLevel = PlayerPrefs.GetInt(key_last_level_chosed, 0);
        if (lastLevel == 0)
        {
            if (isTutorComplete)
            {
                lastLevel = 1;
            }
        }
        var isOpen = MainController.Instance.PlayerData.OpenLevels.IsLevelOpen(lastLevel);
        if (!isOpen)
        {
            if (isTutorComplete)
            {
                lastLevel = 1;
            }
            else
            {
                lastLevel = 0;
            }
        }
        //        var openedMissions = MainController.Instance.PlayerData.OpenLevels.GetAllOpenedMissions();
        int maxLvl = 0;
        RespawnPointToggle lastToggledPoint = null;
        foreach (var pointToggle in MissionsToggles)
        {
            Debug.Log("Selected: " + pointToggle.transform.name);
            pointToggle.text.text = pointToggle.ID.ToString();
            pointToggle.Toggle.onValueChanged.RemoveAllListeners();
            int mission = pointToggle.ID;
            stubList.Add(mission, pointToggle.ID);

            pointToggle.Toggle.onValueChanged.AddListener(arg0 =>
            {
                if (arg0)
                {
                    var id = stubList[mission];
                    List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(id);
                    MissionSelected(stubList[mission], opensRespawnPoints.Count > 0);
                }
            });
            if (mission > maxLvl)
            {
                List<int> opensRespawnPoints = MainController.Instance.PlayerData.OpenLevels.GetAllBornPositions(mission);
                if (opensRespawnPoints.Count > 0)
                    maxLvl = mission;
            }
            if (pointToggle.ID == lastLevel)
            {
                lastToggledPoint = pointToggle;
            }
        }
        foreach (var pointToggle in MissionsToggles)
        {
            if (pointToggle.ID == 0)
            {
                pointToggle.Toggle.interactable = !isTutorComplete;
            }
            else
            {
                pointToggle.Toggle.interactable = isTutorComplete && pointToggle.ID <= maxLvl + 1;
            }

        }
        if (lastToggledPoint != null && lastToggledPoint.Toggle.interactable && lastToggledPoint.ID <= maxLvl)
        {
            lastToggledPoint.Toggle.isOn = true;
        }
        else
        {
            var allOpend = MissionsToggles.Where(x => x.Toggle.interactable && x.ID <= maxLvl);
            var rnd = allOpend.ToList().RandomElement();
            rnd.Toggle.isOn = true;
        }

        DifficultyChooser.Init(OnDifChanges);
        MissionSelected(lastLevel, true);
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
//        if (!canactivate)
//        {
//            CostMissionField.text = "10 crystals";
//        }
        Debug.Log("currentSelectedRespawnPoint:" + currentSelectedRespawnPoint);

    }

    public void OnOpenLvlClick()
    {
        var index = currentSelectedMission;
        if (MainController.Instance.PlayerData.OpenLevels.CanOpenLevel(index))
        {
            OpenLevelWindow.Init(index,LevelOpened,OpenLevelClose);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "You need a higher level to open this location.");
        }
    }

    private void OpenLevelClose()
    {
        
    }

    private void LevelOpened(int obj)
    {
        InitLevels();
    }

    public void OnPlayClick()
    {
        MainController.Instance.StartLevel(currentSelectedRespawnPoint, curDiffChoosed, currentSelectedMission);
    }

    public void OnPlayDebugClick()
    {
        MainController.Instance.StartLevel(0, curDiffChoosed, 99);

    }
}


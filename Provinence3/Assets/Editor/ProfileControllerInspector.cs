using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProfileControllerInspector : EditorWindow
{

    private static string basePath = "";

    [MenuItem("Construct/ProfileControllerInspector")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ProfileControllerInspector window = (ProfileControllerInspector)EditorWindow.GetWindow(typeof(ProfileControllerInspector));
        window.Show();
    }
    public void OnGUI()
    {
        if (!Application.isPlaying)
        {
            if (GUILayout.Button("ADD 1 point"))
            {
                var AllocatedPoints = PlayerPrefs.GetInt(PlayerData.ALLOCATED, 0);
                AllocatedPoints++;
                PlayerPrefs.SetInt(PlayerData.ALLOCATED, AllocatedPoints);
            }
            if (GUILayout.Button("GIVE 99000 money"))
            {
                var str = PlayerData.INVENTORY + ItemId.money.ToString();
                var cur = PlayerPrefs.GetInt(str, 0);
                cur += 99000;
                PlayerPrefs.SetInt(str, cur);
            }
            if (GUILayout.Button("GIVE 10 crystal"))
            {
                var str = PlayerData.INVENTORY + ItemId.crystal.ToString();
                var cur = PlayerPrefs.GetInt(str, 0);
                cur += 10;
                PlayerPrefs.SetInt(str, cur);
            }
            if (GUILayout.Button("Reset IDs"))
            {
                CheckID();
            }
            if (GUILayout.Button("SetFont"))
            {
                SetFont();
            }
            if (GUILayout.Button("CLEAR ALL"))
            {
                PlayerPrefs.DeleteAll();
            }
            EditorGUILayout.LabelField(">>>");
            Repaint();
        }
        else
        {

            if (GUILayout.Button("GIVE ALL ITEMS ALL"))
            {
                foreach(CraftItemType v in Enum.GetValues(typeof(CraftItemType)))
                {
                    MainController.Instance.PlayerData.AddItem(new ExecCraftItem(v, 10));
                }

            }
            if (GUILayout.Button("Open all level"))
            {
                MainController.Instance.PlayerData.OpenLevels.DebugOpenAllLevels();

            }
            if (GUILayout.Button("Tutor complete"))
            {
                MainController.Instance.PlayerData.TutorEnd();
            }
        }
    }

    private void SetFont()
    {
        var allPrefabs = GetAllPrefabs();
        Font font = (Font)Resources.Load("Font/MyUnderwood");
        int index = 0;
        foreach (string prefab in allPrefabs)
        {
            UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
            GameObject go;
            try
            {
                go = (GameObject)o;
                var text = go.GetComponent<Text>();
                if (text != null)
                {
                    index++;
                    text.font = font;
                }
                foreach (Transform v in go.transform)
                {
                    var text2 = v.GetComponent<Text>();
                    if (text2 != null)
                    {
                        index++;
                        text2.font = font;
                    }
                }

            }
            catch
            {
                Debug.Log("For some reason, prefab " + prefab + " won't cast to GameObject");
            }
        }
        Debug.Log("Font changed at: " + index + " prefabs");
    }
    


    private void CheckID()
    {
   
           int bulletIndex = 100;
        var allPrefabs = GetAllPrefabs();
        foreach (string prefab in allPrefabs)
        {
            UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
            GameObject go;
            try
            {
                go = (GameObject)o;
                var bullet = go.GetComponent<Bullet>();
                if (bullet != null)
                {
                    bullet.ID = bulletIndex;
                    bulletIndex++;
                    EditorUtility.SetDirty(go);
                }
                var control = go.GetComponent<BaseControl>();
                if (control != null)
                {
                    control.Cache();
                }

            }
            catch
            {
                Debug.Log("For some reason, prefab " + prefab + " won't cast to GameObject");

            }
        }
        Debug.Log("All bullets ID setted last id:" + bulletIndex);
    }
    public static string[] GetAllPrefabs()
    {
        List<string> prefabsDontCheck = new List<string>()
        {
            "UIFakeStoreCanvas","AccountMenu","DebugCanvas","DebugHUD"
        };

        string[] temp = AssetDatabase.GetAllAssetPaths();
        List<string> result = new List<string>();
        foreach (string s in temp)
        {
            if (basePath.Length > 0 && !s.Contains(basePath))
            {
                continue;
            }

            bool dontCheck = prefabsDontCheck.Any(p => s.Contains(p));
            if (dontCheck)
                continue;
            if (s.Contains(".prefab"))
                result.Add(s);
        }
        return result.ToArray();
    }
}


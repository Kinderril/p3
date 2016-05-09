using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

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
            if (GUILayout.Button("GIVE 1000 money"))
            {
                var str = PlayerData.INVENTORY + ItemId.money.ToString();
                var cur = PlayerPrefs.GetInt(str, 0);
                cur += 1000;
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
        }
    }


    private void CheckID()
    {
        int bulletIndex = 1;
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


public class ShaderGlobalValues : EditorWindow
{
    private float fogLevel;
    private float fogDiff;

    [MenuItem("Construct/ShaderGlobalValues")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ShaderGlobalValues));
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        fogLevel = EditorGUILayout.FloatField("fogLevel", fogLevel);
        fogDiff = EditorGUILayout.FloatField("fogDiff", fogDiff);
        if (GUILayout.Button("Process"))
        {
            Shader.SetGlobalFloat(LevelObject.FOG_START_LEVEL, fogLevel);
            Shader.SetGlobalFloat(LevelObject.FOG_DIFF, fogDiff);
            Shader.SetGlobalFloat(LevelObject.FOG_START_LEVEL_TERRAIN, fogLevel - 2);
            Shader.SetGlobalFloat(LevelObject.FOG_DIFF_TERRAIN, fogDiff * 3);
        }

    }

}


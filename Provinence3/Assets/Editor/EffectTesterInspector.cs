using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EffectTester))]
public class EffectTesterInspector : Editor
{
    private EffectTester myTarget;
    public override void OnInspectorGUI()
    {
        myTarget = (EffectTester)target;
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Test"))
            {
                TestDo();
            }
            if (GUILayout.Button("Stop"))
            {
                myTarget.Stop();
            }
        }
    }

    private void TestDo()
    {
        myTarget.TestDo();
    }
}
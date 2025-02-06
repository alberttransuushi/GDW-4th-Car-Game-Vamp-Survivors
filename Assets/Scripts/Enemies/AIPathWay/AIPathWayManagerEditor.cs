using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(AIPathWayManager))]
public class AIPathWayManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AIPathWayManager aIPathWayManager = (AIPathWayManager)target;

        if (GUILayout.Button("Update"))
        {
            aIPathWayManager.UpdateCheckPoints();
        }
    }
}

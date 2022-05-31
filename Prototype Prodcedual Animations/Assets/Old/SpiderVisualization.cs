using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Targetpoint))]
public class SpiderVisualization : Editor
{
    private void OnSceneGUI()
    {
        //Targetpoint targetpoint = (Targetpoint)target;
        //Handles.color = Color.cyan;
        //Handles.DrawWireArc(targetpoint.transform.position + targetpoint.offset, Vector3.up, Vector3.forward, 360, 0.02f);
    }
}

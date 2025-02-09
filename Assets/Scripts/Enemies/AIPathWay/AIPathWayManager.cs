using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIPathWayManager : MonoBehaviour
{
    public List<CheckPointManager> checkPoints;
    // Start is called before the first frame update
    private void Start()
    {
        UpdateCheckPoints();
    }

    public void UpdateCheckPoints()
    {
        checkPoints = new List<CheckPointManager>();
        foreach(Transform checkPoint in transform) {
            checkPoints.Add(checkPoint.gameObject.GetComponent<CheckPointManager>());
        }

        for (int i = 0; i < checkPoints.Count; i++)
        {
            if (i == 0) {
                checkPoints[i].prevCheckPoint = checkPoints[checkPoints.Count - 1];
                checkPoints[i].nextCheckPoint = checkPoints[i+1];
            } else if(i == checkPoints.Count - 1) {
                checkPoints[i].prevCheckPoint = checkPoints[i - 1];
                checkPoints[i].nextCheckPoint = checkPoints[0];
            } else
            {
                checkPoints[i].prevCheckPoint = checkPoints[i - 1];
                checkPoints[i].nextCheckPoint = checkPoints[i + 1];
            }
        }
    }
}

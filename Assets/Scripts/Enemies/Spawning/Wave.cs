using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy Wave", menuName = "Wave")]
public class Wave : ScriptableObject, ISerializationCallbackReceiver
{

    

    public List<WaveEnemyData> enemiesInWaveInitialize;

    [System.NonSerialized] public List<WaveEnemyData> enemiesInWaveRuntime;


    

    public void OnAfterDeserialize()
    {
        enemiesInWaveRuntime = enemiesInWaveInitialize;
    }

    public void OnBeforeSerialize() { }

}


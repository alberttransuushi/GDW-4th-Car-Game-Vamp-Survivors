using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventList : MonoBehaviour
{
  [SerializeField] List<GameObject> eventObjects;
  public GameObject GetRandomEventObject() {
    return eventObjects[Random.Range(0, eventObjects.Count)];
  }
}

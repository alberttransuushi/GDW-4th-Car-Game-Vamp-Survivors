using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEventManager : MonoBehaviour {
  [SerializeField] List<GameObject> eventTriggers;
  // Start is called before the first frame update
  void Start() {
    for (int i = 0; i < transform.childCount; i++) {
      eventTriggers.Add(transform.GetChild(i).gameObject);
    }
  }
  public GameObject GetRandomEventTrigger() {
    int rand = Random.Range(0, eventTriggers.Count);
    return eventTriggers[rand];
  }
}

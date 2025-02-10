using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour {
  [SerializeField] EventList eventList;

  //event trigger spawn points
  [SerializeField] List<GameObject> subEventManagers;
  int eventStartRef;

  //event trigger types
  bool makeNewEvent;
  bool raceEvent;

  [SerializeField] int eventTriggerTimeDuration;
  [SerializeField] List<int> eventTimes;
  [SerializeField] List<int> eventTimesDupe;

  [SerializeField] TMP_Text eventText;
  float alpha = 1;
  [SerializeField] float textDelayFade;
  float timer;
  private void Start() {
    for (int i = 0; i < transform.childCount; i++) {
      subEventManagers.Add(transform.GetChild(i).gameObject.GetComponentInChildren<SubEventManager>().gameObject);
    }
    eventTimesDupe = new List<int>(eventTimes);
  }
  private void Update() {
    textDelayFade -= Time.deltaTime;
    timer += Time.deltaTime;
    if (timer > eventTimesDupe[0]) {// event spawn
      eventTimesDupe.RemoveAt(0);
      GameObject randEventTrigger = GetRandomEventTrigger();
      randEventTrigger.SetActive(true);
      randEventTrigger.GetComponent<EventTrigger>().SetTimer(45);
      eventText.text = "Event Spawned";
      alpha = 1;
      textDelayFade = 2;
      eventText.color = new Color(1, 1, 1, alpha);
      makeNewEvent = true;
    }
    if (textDelayFade <= 0) {
      if (alpha > 0) {
        alpha -= Time.deltaTime * 0.5f;
      }
      eventText.color = new Color(1, 1, 1, alpha);
    }
  }
  GameObject GetRandomEventTrigger() {
    int rand = Random.Range(0, subEventManagers.Count);
    return subEventManagers[rand].GetComponent<SubEventManager>().GetRandomEventTrigger();
  }
  public void EventTriggerReached() {
    if (makeNewEvent) {
      eventText.text = "Event Created";
      alpha = 1;
      textDelayFade = 2;
      eventText.color = new Color(1, 1, 1, alpha);
      raceEvent = true;
      makeNewEvent = false;
      int rand = Random.Range(50, subEventManagers.Count);
      rand += eventStartRef;
      if (eventStartRef > subEventManagers.Count) {
        eventStartRef -= subEventManagers.Count;
      }
      GameObject randEventTrigger = subEventManagers[rand].GetComponent<SubEventManager>().GetRandomEventTrigger();
      randEventTrigger.SetActive(true);
      randEventTrigger.GetComponent<EventTrigger>().SetTimer(45);
    } else if (raceEvent) {
      eventText.text = "Event Completed";
      alpha = 1;
      textDelayFade = 2;
      eventText.color = new Color(1, 1, 1, alpha);
      raceEvent = false;
    }
  }
  public void EventTriggerFaded() {
    makeNewEvent = false;
    raceEvent = false;
  }
}

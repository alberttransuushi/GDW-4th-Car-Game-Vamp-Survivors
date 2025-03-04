using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour {
  [SerializeField] GameObject expOrb;
  [SerializeField] EventList eventList;

  //event trigger spawn points
  [SerializeField] List<GameObject> subEventManagers;
  int eventStartRef;

  //event trigger types
  bool makeNewEvent;
  bool raceEvent;

  //event variable tracking
  public static int enemiesKilled;
  [SerializeField] int enemiesToBeKilled;
  bool enemiesKilledEvent;
  public static int timesHit;
  [SerializeField] float timesAvoidGettingHit;
  bool timesHitEvent;
  float eventTimer;
  float eventTimeLimit;

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
    if (Input.GetKeyDown(KeyCode.K)) {
      MakeRandomEvent();
    }
    textDelayFade -= Time.deltaTime;
    timer += Time.deltaTime;
    eventTimer += Time.deltaTime;
    if (eventTimesDupe.Count > 0){
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
    }
    if (timesHitEvent) {
      if (eventTimer > 30) {
        eventText.text = "Event Completed";
        alpha = 1;
        textDelayFade = 2;
        eventText.color = new Color(1, 1, 1, alpha);
        timesHitEvent = false;
      }
      if (timesHit >= timesAvoidGettingHit) {
        eventText.text = "Event Failed";
        alpha = 1;
        textDelayFade = 2;
        eventText.color = new Color(1, 1, 1, alpha);
        timesHitEvent = false;
      }
    } else if (enemiesKilledEvent) {
      if (eventTimer > 30) {
        eventText.text = "Event Failed";
        alpha = 1;
        textDelayFade = 2;
        eventText.color = new Color(1, 1, 1, alpha);
        enemiesKilledEvent = false;
      }
      if (enemiesKilled >= enemiesToBeKilled) {
        eventText.text = "Event Complete";
        alpha = 1;
        textDelayFade = 2;
        eventText.color = new Color(1, 1, 1, alpha);
        enemiesKilledEvent = false;
      }
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
      MakeRandomEvent();
    } else if (raceEvent) {
      eventText.text = "Event Completed";
      alpha = 1;
      textDelayFade = 2;
      eventText.color = new Color(1, 1, 1, alpha);
      raceEvent = false;
    }
  }
  void MakeRandomEvent() {
    alpha = 1;
    textDelayFade = 2;
    eventText.color = new Color(1, 1, 1, alpha);
    //random event creation
    int rand = Random.Range(0, 3);
    Debug.Log(rand);
    if (rand == 0) {
      eventText.text = "Event Created: reach the destination";
      raceEvent = true;
      makeNewEvent = false;
      rand = Random.Range(50, subEventManagers.Count);
      rand += eventStartRef;
      if (eventStartRef > subEventManagers.Count) {
        eventStartRef -= subEventManagers.Count;
      }
      GameObject randEventTrigger = subEventManagers[rand].GetComponent<SubEventManager>().GetRandomEventTrigger();
      randEventTrigger.SetActive(true);
      randEventTrigger.GetComponent<EventTrigger>().SetTimer(rand);
    } else if (rand == 1) {
      eventText.text = "Event Created: DONT GET HIT";
      timesHitEvent = true;
      timesHit = 0;
      eventTimer = 0;
      eventTimeLimit = 30;
    } else if (rand == 2) {
      eventText.text = "Event Created: KILL ENEMIES";
      enemiesKilledEvent = true;
      enemiesKilled = 0;
      eventTimer = 0;
      eventTimeLimit = 30;
    }
    
  }
  public void EventTriggerFaded() {
    makeNewEvent = false;
    raceEvent = false;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour {
  [SerializeField] GameObject expOrb;
  [SerializeField] EventList eventList;

  //event trigger spawn points
  [SerializeField] List<GameObject> subEventManagers;
  [SerializeField] List<Hoop> hoops;
  [SerializeField] int hoopStartReferenceOffset = 15;
  [SerializeField] int gapsBetweenHoops = 2;
  [SerializeField] float hoopLRAdjustments = 0.2f;
  [SerializeField] int hoopAmount = 25;
  int eventStartRef;

  //event trigger types
  bool makeNewEvent;
  bool raceEvent;

  //event variable tracking
  //kill enemies
  public static int enemiesKilled;
  [SerializeField] int enemiesToBeKilled;
  bool enemiesKilledEvent;
  //dont get hit
  public static int timesHit;
  public static float timeSinceLastHit;
  [SerializeField] float timeSinceLastHitWinCon;
  bool timesHitEvent;
  //hoops
  int hoopCount;
  bool hoopsEvent;
  int hoopReference;

  //event compelted
  bool eventCompleted;

  float eventTimer;
  float eventTimeLimit;

  [SerializeField] int eventTriggerTimeDuration;
  [SerializeField] List<int> eventTimes;
  [SerializeField] List<int> eventTimesDupe;

  [SerializeField] TMP_Text eventText;
  [SerializeField] Color eventTextColor;
  float alpha = 0;
  [SerializeField] float textDelayFade;
  float timer;
  private void Start() {
    for (int i = 0; i < transform.childCount; i++) {
      subEventManagers.Add(transform.GetChild(i).gameObject.GetComponentInChildren<SubEventManager>().gameObject);
      hoops.Add(transform.GetChild(i).gameObject.GetComponentInChildren<Hoop>());
      hoops[i].gameObject.SetActive(false);
    }
    
    
    eventTimesDupe = new List<int>(eventTimes);
  }
  private void Update() {
    if (Input.GetKeyDown(KeyCode.K)) {
      MakeRandomEvent();
    }
    
    //timer updates
    textDelayFade -= Time.deltaTime;
    timer += Time.deltaTime;
    eventTimer += Time.deltaTime;
    timeSinceLastHit += Time.deltaTime;

    if (eventTimesDupe.Count > 0){
      if (timer > eventTimesDupe[0]) {// event spawn
        eventTimesDupe.RemoveAt(0);
        GameObject randEventTrigger = GetRandomEventTrigger();
        randEventTrigger.SetActive(true);
        randEventTrigger.GetComponent<EventTrigger>().SetTimer(45);
        eventText.text = "Event Trigger Spawned";
        UpdateEventText();
        makeNewEvent = true;
      }
    }
    if (timesHitEvent) {
      if (timeSinceLastHit >= timeSinceLastHitWinCon) {
        eventText.text = "Event Completed";
        eventCompleted = true;
        UpdateEventText();
        timesHitEvent = false;
      }
      if (eventTimer > eventTimeLimit) {
        eventText.text = "Event Failed";
        UpdateEventText();
        timesHitEvent = false;
      }
    } else if (enemiesKilledEvent) {
      if (eventTimer > eventTimeLimit) {
        eventText.text = "Event Failed";
        UpdateEventText();
        enemiesKilledEvent = false;
      }
      if (enemiesKilled >= enemiesToBeKilled) {
        eventText.text = "Event Complete";
        eventCompleted = true;
        UpdateEventText();
        enemiesKilledEvent = false;
      }
    } else if (hoopsEvent) {
      if (eventTimer > eventTimeLimit) {
        eventText.text = "Event Failed";
        UpdateEventText();
        hoopsEvent = false;
      }
      if (hoopCount >= hoopAmount) {
        eventText.text = "Event Complete";
        eventCompleted = true;
        UpdateEventText();
        enemiesKilledEvent = false;
      }
    }
    if (textDelayFade <= 0) {
      if (alpha > 0) {
        alpha -= Time.deltaTime * 0.5f;
      }
      eventText.color = new Color(eventTextColor.r, eventTextColor.g, eventTextColor.b, alpha);
    }
    if (eventCompleted) {
      GameObject playerCar = GameObject.FindGameObjectWithTag("Player");
      float expDrop = playerCar.GetComponent<PlayerExp>().GetNextLevelRequirement();
      //GameObject orb = Instantiate(expOrb, playerCar.transform.position, playerCar.transform.rotation);
      ///orb.GetComponent<ExpOrb>().SetExpValue(expDrop);
      playerCar.GetComponent<PlayerExp>().GetExp(expDrop);
    }
  }
  void UpdateEventText() {
    alpha = 1;
    textDelayFade = 2;
    eventText.color = new Color(eventTextColor.r, eventTextColor.g, eventTextColor.b, alpha);
    
  }
  GameObject GetRandomEventTrigger() {
    int rand = Random.Range(0, subEventManagers.Count);
    return subEventManagers[rand].GetComponent<SubEventManager>().GetRandomEventTrigger();
  }
  public void EventTriggerReached(GameObject trigger) {
    hoopReference = subEventManagers.FindInstanceID(trigger);
    if (makeNewEvent) {
      eventText.text = "Event Created";
      UpdateEventText();
      MakeRandomEvent();
    } else if (raceEvent) {
      eventText.text = "Event Completed";
      UpdateEventText();
      raceEvent = false;
    }
  }
  void MakeRandomEvent() {
    alpha = 1;
    textDelayFade = 2;
    eventText.color = new Color(1, 1, 1, alpha);
    //random event creation
    //int rand = Random.Range(0, 4);
    int rand = 3;

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
      UpdateEventText();
      timesHitEvent = true;
      timesHit = 0;
      eventTimer = 0;
      eventTimeLimit = 30;
    } else if (rand == 2) {
      eventText.text = "Event Created: KILL ENEMIES";
      UpdateEventText();
      enemiesKilledEvent = true;
      enemiesKilled = 0;
      eventTimer = 0;
      eventTimeLimit = 30;
    } else if (rand == 3) {//hoops
      eventText.text = "Event Created: GET THROUGH ALL THE HOOPS";
      UpdateEventText();
      SquenceHoops(hoopReference + hoopStartReferenceOffset, gapsBetweenHoops, hoopLRAdjustments, hoopAmount);
      hoopsEvent = true;
      hoopCount = 0;
      eventTimer = 0;
      eventTimeLimit = 30;
    }
    
  }
  public void EventTriggerFaded() {
    makeNewEvent = false;
    raceEvent = false;
  }

  void SquenceHoops(int start, int gap, float lrVariation, int length) {
    int refHoop = start;
    float hoopLR = Random.Range(0.3f, 0.7f);
    for (int i = 0; i < length; i ++) {
      hoopLR += lrVariation * Random.Range(-1f, 1f);
      if (hoopLR < 0) {
        hoopLR = 0;
      } else if (hoopLR > 1) {
        hoopLR = 1;
      }
      hoops[refHoop].gameObject.SetActive(true);
      hoops[refHoop].AdjustLeftRight(hoopLR);
      refHoop += gap;
    }
  }
  public void AddHoopToCount() {
    hoopCount += 1;
  }
}

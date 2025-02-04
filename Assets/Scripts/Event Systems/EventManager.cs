using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour {
  [SerializeField] EventList eventList;
  [SerializeField] List<GameObject> eventLocations;

  [SerializeField] int eventTriggerTimeDuration;
  [SerializeField] List<int> eventTimes;
  [SerializeField] List<int> eventTimesDupe;

  [SerializeField] TMP_Text eventText;
  float alpha = 1;
  [SerializeField] float textDelayFade;
  float timer;
  private void Start() {
    for (int i = 0; i < transform.childCount; i++) {
      eventLocations.Add(transform.GetChild(i).gameObject);
    }
    eventTimesDupe = new List<int>(eventTimes);
  }
  private void Update() {
    textDelayFade -= Time.deltaTime;
    timer += Time.deltaTime;
    if (timer > eventTimesDupe[0]) {
      eventTimesDupe.RemoveAt(0);
      int rand = Random.Range(0, eventLocations.Count);
      eventLocations[rand].gameObject.SetActive(true);
      eventLocations[rand].GetComponent<EventTrigger>().SetTimer(45);
      eventText.text = "Event Spawned";
      alpha = 1;
      textDelayFade = 2;
      eventText.color = new Color(1, 1, 1, alpha);
    }
    if (textDelayFade <= 0) {
      if (alpha > 0) {
        alpha -= Time.deltaTime * 0.5f;
      }
      eventText.color = new Color(1, 1, 1, alpha);
    }
  }
  public void CreateEvent() {
    eventText.text = "Event Created";
    alpha = 1;
    textDelayFade = 2;
    eventText.color = new Color(1, 1, 1, alpha);
  }
}

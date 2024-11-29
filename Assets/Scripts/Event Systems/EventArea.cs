using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventArea : MonoBehaviour
{
  public void CreateEvent() {
    GameObject eventObj = Instantiate(GetComponentInParent<EventList>().GetRandomEventObject());
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{

    private ParticleSystem splosion;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Splosion");
        splosion = GetComponent<ParticleSystem>();
        splosion.Play();
        StartCoroutine(pop());

    }

    IEnumerator pop()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this);
    }

   
}

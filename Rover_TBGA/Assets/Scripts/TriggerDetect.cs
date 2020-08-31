using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetect : MonoBehaviour
{
    public GameObject obj;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            obj.GetComponent<Robo>().DetectPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            obj.GetComponent<Robo>().ExitDecetPlayer();
        }
    }
}

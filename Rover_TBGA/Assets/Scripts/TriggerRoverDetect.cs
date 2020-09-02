using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoverDetect : MonoBehaviour
{
    public Rover rover;
    public Material matGridOff;
    public Material matGridOn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Grid"))
        {
            other.GetComponent<MeshRenderer>().material = matGridOn;
        }

        if(other.CompareTag("Robo"))
        {
            other.GetComponent<Robo>().DetectPlayer();
            rover.SetCanSHoot(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grid"))
        {
            other.GetComponent<MeshRenderer>().material = matGridOff;
        }

        if (other.CompareTag("Robo"))
        {
            other.GetComponent<Robo>().ExitDecetPlayer();
            rover.SetCanSHoot(false);
        }
    }
}

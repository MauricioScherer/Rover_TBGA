﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoverDetect : MonoBehaviour
{
    public Material matGridOff;
    public Material matGridOn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Grid"))
        {
            other.GetComponent<MeshRenderer>().material = matGridOn;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grid"))
        {
            other.GetComponent<MeshRenderer>().material = matGridOff;
        }
    }
}

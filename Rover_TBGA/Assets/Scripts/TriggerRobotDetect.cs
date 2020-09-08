using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRobotDetect : MonoBehaviour
{
    public Robo robot;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collision"))
        {
            robot.DetectColissionCave();
        }
    }
}

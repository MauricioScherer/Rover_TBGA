using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRobot : MonoBehaviour
{
    private Transform target;

    public Robo robot;
    public GameObject line;

    void Update()
    {
        if(target != null)
        {
            transform.LookAt(target, Vector3.up);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                if(hit.collider.gameObject.name != "Body" && !robot.GetPauseShoot() && hit.collider.gameObject.tag != "Shield")
                {
                    robot.SetPauseShoot(true);
                }
                else if(robot.GetPauseShoot() && hit.collider.gameObject.name == "Body" || robot.GetPauseShoot() && hit.collider.gameObject.tag == "Shield")
                {
                    robot.SetPauseShoot(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rover"))
        {
            target = other.transform;
            line.SetActive(true);
            robot.DetectPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rover"))
        {
            target = null;
            line.SetActive(false);
            robot.ExitDecetPlayer();
        }
    }
}

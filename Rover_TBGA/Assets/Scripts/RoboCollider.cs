using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboCollider : MonoBehaviour
{
    public Robo robot;

    public void Damage()
    {
        robot.SetDamage();
    }
}

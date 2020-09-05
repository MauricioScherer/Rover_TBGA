﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverCollision : MonoBehaviour
{
    public Rover rover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            rover.RechargeAmmo();
            other.GetComponent<Item>().GetItem();
        }
        else if (other.CompareTag("Fuel"))
        {
            rover.RechargeFuel();
            other.GetComponent<Item>().GetItem();
        }
    }

    public void Damage()
    {
        rover.SetDamage();
    }
}
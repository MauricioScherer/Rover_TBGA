using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public GameObject soldierObj;
    public GameObject feedbackGetObj;

    public void GetSoldier()
    {
        GetComponent<BoxCollider>().enabled = false;
        soldierObj.SetActive(false);
        feedbackGetObj.SetActive(true);        
        Destroy(gameObject, 3.0f);
    }
}

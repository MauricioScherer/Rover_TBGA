using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private bool _timer;
    private float _count;

    public GameObject itemIdle;
    public GameObject itemGet;
    public Image time;


    private void Update()
    {
        if(_timer)
        {
            _count += 0.025f * Time.deltaTime;

            time.fillAmount = _count;

            if (_count >= 1)
            {
                GetComponent<BoxCollider>().enabled = true;
                itemIdle.SetActive(true);
                itemGet.SetActive(false);
                _timer = false;
                _count = 0;
                time.fillAmount = _count;
            }
        }
    }

    public void GetItem()
    {
        GetComponent<BoxCollider>().enabled = false;
        itemIdle.SetActive(false);
        itemGet.SetActive(true);
        _timer = true;
    }
}

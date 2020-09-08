using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private bool _timer;
    private float _count;
    private int _countSeg;

    public GameObject itemIdle;
    public GameObject itemGet;
    public TextMesh counter;
    public int counterMax;

    private void Start()
    {
        _countSeg = counterMax;
        counter.text = "";
    }


    private void Update()
    {
        if(_timer)
        {
            _count += Time.deltaTime;

            if(_count >= 1)
            {
                _countSeg--;
                _count = 0;
                counter.text = _countSeg.ToString();
            }

            if (_countSeg <= 0)
            {
                GetComponent<BoxCollider>().enabled = true;
                itemIdle.SetActive(true);
                itemGet.SetActive(false);
                _timer = false;
                _count = 0;
                _countSeg = counterMax;
                counter.text = "";
            }
        }
    }

    public void GetItem()
    {
        GetComponent<BoxCollider>().enabled = false;
        itemIdle.SetActive(false);
        itemGet.SetActive(true);
        _timer = true;
        counter.text = counterMax.ToString();
    }
}

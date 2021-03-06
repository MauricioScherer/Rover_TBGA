﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private bool _life;
    private float _counter;

    public float speed;
    public float timeLife;
    public GameObject shootCollider;

    // Update is called once per frame
    void Update()
    {
        if(_life)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            _counter += Time.deltaTime;

            if(_counter >= timeLife)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void StartBullet()
    {
        _life = true;
        _counter = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Robo"))
        {
            other.GetComponent<RoboCollider>().Damage();
            GameObject effectShoot = Instantiate(shootCollider, transform.position, transform.rotation);
            Destroy(effectShoot, 2.0f);
            gameObject.SetActive(false);
        }
        else if(other.CompareTag("Collision"))
        {
            GameObject effectShoot = Instantiate(shootCollider, transform.position, transform.rotation);
            Destroy(effectShoot, 2.0f);
            gameObject.SetActive(false);
        }
    }
}

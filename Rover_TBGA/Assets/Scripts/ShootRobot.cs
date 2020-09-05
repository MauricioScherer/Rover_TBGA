using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRobot : MonoBehaviour
{
    private float _counter;

    public float speed;
    public float timeLife;
    public GameObject shootCollider;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        _counter += Time.deltaTime;

        if (_counter >= timeLife)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Rover"))
        {
            other.GetComponent<RoverCollision>().Damage();
            GameObject effectShoot = Instantiate(shootCollider, transform.position, transform.rotation);
            Destroy(effectShoot, 2.0f);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Shield"))
        {
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

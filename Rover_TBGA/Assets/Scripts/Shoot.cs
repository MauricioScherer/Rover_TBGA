using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Camera cam;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        if(screenPos.y > Screen.height || screenPos.y < 0)
        {
            gameObject.SetActive(false);
        }
        if (screenPos.x > Screen.width || screenPos.x < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}

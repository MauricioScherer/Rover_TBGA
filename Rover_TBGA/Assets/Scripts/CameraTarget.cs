using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float speedMove;
    public Transform targetMove;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetMove.position, speedMove * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetMove.rotation, speedMove * Time.deltaTime);
    }
}

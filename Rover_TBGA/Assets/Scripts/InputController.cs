using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public string inputSteerAxis = "Horizontal";
    //public string inputThrottleAxis = "Vertical";
    //public string inputThrottleAxisRe = "Fire2";

    public float ThrottelInput { get; private set; }
    public float SteerInput { get; private set; }

    private void Start()
    {

    }

    private void Update()
    {
        SteerInput = Input.GetAxis(inputSteerAxis);
        //if(Input.GetButtonDown("Fire1"))
        //    ThrottelInput = Input.GetAxis(inputThrottleAxis);
        //else
        //    ThrottelInput = Input.GetAxis(inputThrottleAxisRe);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo : MonoBehaviour
{
    public PetriNet robo;
    // Start is called before the first frame update
    void Start()
    {
        robo = new PetriNet("Assets/RedesPetri/Robo.pflow");
    }

    // Update is called once per frame
    void Update()
    {
        robo.ExecCycle();
        
        
    }
}

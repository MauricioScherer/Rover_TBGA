using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public PetriNet rover;

    // Start is called before the first frame update
    void Start()
    {
        rover = new PetriNet("Assets/RedesPetri/Rover.pflow");    //instancia RdP do Lander e constroi sua rede...
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rover.GetPlaceByLabel("Combustivel").Tokens);
        rover.ExecCycle();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {            
            rover.GetPlaceByLabel("Deslocar").Tokens = 1;
        }
    }
}

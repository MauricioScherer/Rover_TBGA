using RdPengine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class Rover : MonoBehaviour
{
    private PetriNet _rover;
    
    //status Rover
    private Place _fuel;
    private Place _ammo;
    private Place _life;
    [Header("Status")]
    public Text fuel;
    public Text ammo;
    public Text life;

    void Start()
    {
        _rover = new PetriNet("Assets/RedesPetri/Rover.pflow");

        //Define status conform PetriNet
        _fuel = _rover.GetPlaceByLabel("Fuel");
        _ammo = _rover.GetPlaceByLabel("Ammo");
        _life = _rover.GetPlaceByLabel("Life");

        //update values for status
        _fuel.AddCallback(RefreshTextos, "refreshFuel", Tokens.InOrOut);
        _ammo.AddCallback(RefreshTextos, "refreshAmmo", Tokens.InOrOut);
        _life.AddCallback(RefreshTextos, "refreshLife", Tokens.InOrOut);

        RefreshTextos();
    }

    void Update()
    {
        //movimentação temporaria
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }
    }

    public void RefreshTextos()
    {
        life.text = "Vida: " + _life.Tokens.ToString();
        fuel.text = "Combustivel: " + _fuel.Tokens.ToString();
        ammo.text = "Munição: " + _ammo.Tokens.ToString();
    }
}

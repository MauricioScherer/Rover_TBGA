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
    public GameObject roverPng;
    public Text fuel;
    public Text ammo;
    public Text life;

    [Header("Pool Shoot")]
    public PoolShoot pool;
    public Transform spawnShoot;

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
            roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 180.0f);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, -90.0f);
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
        }

        //tiro temporario
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_ammo.Tokens > 0)
            {
                _rover.GetPlaceByLabel("#Shoot").Tokens = 1;
                GameObject _shoot = pool.ActiveShoot();

                if(_shoot != null)
                {
                    _shoot.transform.position = spawnShoot.transform.position;
                    _shoot.transform.rotation = spawnShoot.transform.rotation;
                    _shoot.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Sem munição");
            }
        }
    }

    public void RefreshTextos()
    {
        life.text = "Vida: " + _life.Tokens.ToString();
        fuel.text = "Combustivel: " + _fuel.Tokens.ToString();
        ammo.text = "Munição: " + _ammo.Tokens.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ammo"))
        {
            _rover.GetPlaceByLabel("#RechargeAmmo").Tokens = 1;
            Debug.Log("Recarregou Municao");
        }
        else if(other.CompareTag("Fuel"))
        {
            _rover.GetPlaceByLabel("#RechargeFuel").Tokens = 1;
            Debug.Log("Recarregou Gasolina");
        }
        else if (other.CompareTag("Parede"))
        {
            _rover.GetPlaceByLabel("#Collision").Tokens = 1;
            Debug.Log("Colidiu");
        }
    }
}

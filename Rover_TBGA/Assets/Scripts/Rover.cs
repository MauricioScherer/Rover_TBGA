using RdPengine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    private float _countFuel;

    private bool _canShoot;

    [Header("Physics")]
    public Transform centerOfMass;
    public float motorTorque;
    public float maxSteer;
    public float Steer { get; set; }
    public float Throttle { get; set; }

    private Rigidbody _rigidbody;
    private Wheel[] _wheels;

    [Header("Status")]
    public Text fuel;
    public Text ammo;
    public Text life;

    [Header("Pool Shoot")]
    public GameObject mira;
    public PoolShoot pool;
    public Transform spawnShoot;

    [Header("Camera")]
    public Transform targetCamera;
    public Vector3 posCameraMove;
    public Vector3 posCameraCombat;

    void Start()
    {
        _wheels = GetComponentsInChildren<Wheel>();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;

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

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            mira.SetActive(!mira.activeSelf);
        }

        if(mira.activeSelf)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        Steer = GameManager.Instance.InputController.SteerInput;

        Throttle = Input.GetAxis("Vertical");

        if(Input.GetAxis("Vertical") != 0)
        {
            _countFuel += Time.deltaTime;
        }

        if(_countFuel >= 1)
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;
            _countFuel = 0;
        }

        foreach (var wheel in _wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }

    public void RefreshTextos()
    {
        life.text = "Vida: " + _life.Tokens.ToString();
        fuel.text = "Combustivel: " + _fuel.Tokens.ToString();
        ammo.text = "Munição: " + _ammo.Tokens.ToString();
    }

    public void Shoot()
    {
        if (_ammo.Tokens > 0)
        {
            _rover.GetPlaceByLabel("#Shoot").Tokens = 1;
            GameObject _shoot = pool.ActiveShoot();

            if (_shoot != null)
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

    public void Shield()
    {
        Debug.Log("Ativar escudos");
    }

    public void RechargeAmmo()
    {
        _rover.GetPlaceByLabel("#RechargeAmmo").Tokens = 1;
    }

    public void RechargeFuel()
    {
        _rover.GetPlaceByLabel("#RechargeFuel").Tokens = 1;
    }

    public void SetCanSHoot(bool p_status)
    {
        _canShoot = p_status;
    }
}

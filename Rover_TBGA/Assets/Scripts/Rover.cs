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
    private Place _sdRescue;

    private float _countFuel;
    private int _capacity;

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
    public Text soldier;

    [Header("Pool Shoot")]
    public GameObject mira;
    public PoolShoot pool;
    public Transform spawnShoot;
    public float timeReload;
    public Image reloadCanShoot;
    public Text ammoMensage;

    [Header("Shield")]
    public GameObject shield;
    public float timeShield;

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

        // ajustando a multiplicidade do arco posterior ao Place "SoldierRescue" conforme o número de soldados da cena
        _rover.GetConnection(17, 18).Multiplicity = GameManager.Instance.GetSoldierInScene(); 
       

        //Define status conform PetriNet
        _fuel = _rover.GetPlaceByLabel("Fuel");
        _ammo = _rover.GetPlaceByLabel("Ammo");
        _life = _rover.GetPlaceByLabel("Life");
        _sdRescue = _rover.GetPlaceByLabel("SoldierRescue");

        //update values for status
        _fuel.AddCallback(RefreshTextos, "refreshFuel", Tokens.InOrOut);
        _ammo.AddCallback(RefreshTextos, "refreshAmmo", Tokens.InOrOut);
        _life.AddCallback(RefreshTextos, "refreshLife", Tokens.InOrOut);
       _sdRescue.AddCallback(RefreshTextos, "refreshSoldier", Tokens.InOrOut); 
   
        RefreshTextos();

        ammoMensage.text = "Arma desativada";
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            mira.SetActive(!mira.activeSelf);
            reloadCanShoot.enabled = mira.activeSelf;

            if (!mira.activeSelf)
            {
                reloadCanShoot.fillAmount = 0;
                ammoMensage.text = "Arma desativada";
            }
            else
            {
                reloadCanShoot.color = Color.yellow;
                ammoMensage.text = "Carregando arma";
            }
        }

        if(mira.activeSelf)
        {
            if(reloadCanShoot.fillAmount < 1)
                reloadCanShoot.fillAmount += timeReload * Time.deltaTime;
            else
            {
                ammoMensage.text = "Arma carregada";
                reloadCanShoot.color = Color.green;
            }

            if (Input.GetMouseButtonDown(0) && reloadCanShoot.fillAmount == 1)
            {
                Shoot();
                reloadCanShoot.fillAmount = 0;
            }
        }

        if(Input.GetKeyDown("e") && _rover.GetPlaceByLabel("TimerShield").Tokens == 0)
        {
            if(_rover.GetPlaceByLabel("Fuel").Tokens > 5)
            {
                _rover.GetPlaceByLabel("#Shield").Tokens = 1;
                shield.SetActive(true);
                Invoke("DeactiveShield", timeShield);
            }
            else
            {
                Debug.Log("Sem combustivel para ativar o escudo");
            }
        }

        Steer = GameManager.Instance.InputController.SteerInput;

        if(_rover.GetPlaceByLabel("Fuel").Tokens > 0)
        {
            Throttle = Input.GetAxis("Vertical");

            if (Input.GetAxis("Vertical") != 0)
            {
                _countFuel += Time.deltaTime;
            }

            if (_countFuel >= 1)
            {
                _rover.GetPlaceByLabel("#Move").Tokens = 1;
                _countFuel = 0;
            }
        }
        else
        {
            Throttle = 0;
            Debug.Log("Sem combustivel");
        }

        foreach (var wheel in _wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;
            wheel.Torque = Throttle * motorTorque;
        }
    }

    public void RefreshTextos()
    {
        life.text = _life.Tokens.ToString();
        fuel.text = _fuel.Tokens.ToString();
        ammo.text = _ammo.Tokens.ToString();
        soldier.text = _sdRescue.Tokens.ToString() + " / " + GameManager.Instance.GetSoldierInScene();

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
                _shoot.GetComponent<Shoot>().StartBullet();
            }
        }
        else
        {
            Debug.Log("Sem munição");
        }
    }

    public void DeactiveShield()
    {
        _rover.GetPlaceByLabel("#OffShield").Tokens = 1;
        shield.SetActive(false);
        Debug.Log("Escudos desativados");
    }

    public void RechargeAmmo()
    {
        _rover.GetPlaceByLabel("#RechargeAmmo").Tokens = 1;
    }

    public void RechargeFuel()
    {
        _rover.GetPlaceByLabel("#RechargeFuel").Tokens = 1;
    }

    public void RescueSoldier()
    {
        _rover.GetPlaceByLabel("#Rescue").Tokens = 1;
    }

    public void PortalActive()
    {
        _rover.GetPlaceByLabel("#Portal").Tokens = 1;
    }

    public void SetCanSHoot(bool p_status)
    {
        _canShoot = p_status;
    }

    public void SetDamage()
    {
        _rover.GetPlaceByLabel("#Damage").Tokens = 1;
    }

    public int GetSoldiersInRover()
    {
        return _sdRescue.Tokens;
    }
}

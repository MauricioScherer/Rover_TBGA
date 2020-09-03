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

    private bool _canShoot;

    [Header("Status")]
    public GameObject roverPng;
    public Text fuel;
    public Text ammo;
    public Text life;

    [Header("Pool Shoot")]
    public PoolShoot pool;
    public Transform spawnShoot;

    [Header("UI Rover")]
    public GameObject[] btnsMove;
    public GameObject btnShoot;
    public GameObject btnShield;
    public GameObject btnRotRight;
    public GameObject btnRotLeft;

    [Header("Camera")]
    public Transform targetCamera;
    public Vector3 posCameraMove;
    public Vector3 posCameraCombat;

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

    private void FixedUpdate()
    {
        if(_canShoot && btnsMove[0].activeSelf)
        {
            for (int i = 0; i < btnsMove.Length; i++)
                btnsMove[i].SetActive(false);
        }
    }

    public void RefreshTextos()
    {
        life.text = "Vida: " + _life.Tokens.ToString();
        fuel.text = "Combustivel: " + _fuel.Tokens.ToString();
        ammo.text = "Munição: " + _ammo.Tokens.ToString();
    }

    #region MOVE
    public void Move(GameObject p_btn)
    {
        if(!_canShoot)
        {
            _rover.GetPlaceByLabel("#Move").Tokens = 1;

            ResetBtns();

            if (p_btn.name == "Up")
            {
                roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.0f);
            }
            else if (p_btn.name == "Down")
            {
                roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 180.0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.0f);
            }
            else if (p_btn.name == "Right")
            {
                roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, -90.0f);
                transform.position = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
            }
            else if (p_btn.name == "Left")
            {
                roverPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
                transform.position = new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z);
            }

            Invoke("ResetBtns", 0.5f);
        }
    }

    private void ResetBtns()
    {
        for (int i = 0; i < btnsMove.Length; i++)
            btnsMove[i].SetActive(!btnsMove[i].activeSelf);
    }
    #endregion

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

    public void RotationRight()
    {
        var rot = roverPng.transform.rotation;
        rot.z += 10.0f;
        roverPng.transform.rotation = rot;
    }

    public void RotationLeft()
    {
        var rot = roverPng.transform.rotation;
        rot.z -= 10.0f;
        roverPng.transform.rotation = rot;
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
        //else if (other.CompareTag("Parede"))
        //{
        //    _rover.GetPlaceByLabel("#Collision").Tokens = 1;
        //    Debug.Log("Colidiu");
        //}
    }

    public void SetCanSHoot(bool p_status)
    {
        _canShoot = p_status;

        if(p_status)//esta dentro da visão do robô
        {
            targetCamera.localPosition = posCameraCombat;
        }
        else
        {
            targetCamera.localPosition = posCameraMove;
        }

        for (int i = 0; i < btnsMove.Length; i++)
            btnsMove[i].SetActive(!_canShoot);

        btnShoot.SetActive(_canShoot);
        btnShield.SetActive(_canShoot);
        btnRotRight.SetActive(_canShoot);
        btnRotLeft.SetActive(_canShoot);
    }
}

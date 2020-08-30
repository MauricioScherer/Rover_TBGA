using RdPengine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robo : MonoBehaviour
{
    private PetriNet _robot;
    private int directionChoice;
    private float count;
    private bool isIdle;

    //status Rover
    private Place _life;

    [Header("Status")]
    public GameObject robotPng;
    public Text life;
    public float timer;

    [Header("Pool Shoot")]
    public PoolShoot pool;
    public Transform spawnShoot;

    // Start is called before the first frame update
    void Start()
    {

        _robot = new PetriNet("Assets/RedesPetri/Robo.pflow");

        //Define status conform PetriNet
        _life = _robot.GetPlaceByLabel("Life");

        //update values for status
        _life.AddCallback(RefreshTextos, "refreshLife", Tokens.InOrOut);

        RefreshTextos();

        count = timer;
        isIdle = true;
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        if (count < 0)
        {
            count = timer;
            isIdle = !isIdle;
            if (isIdle == true) _robot.GetPlaceByLabel("Idle").Tokens = 1;
            if (_robot.GetPlaceByLabel("#Idle").IsEmpty() == true) RandomDirection();
        }

        // como fazer a vida aparecer acima do personagem??
        //life.transform.position = new Vector3(robotPng.transform.position.x, robotPng.transform.position.y, robotPng.transform.position.z);

    }

    public void RefreshTextos()
    {
        life.text = "Vida: " + _life.Tokens.ToString();
    }

    public void RandomDirection()
    {
        directionChoice = Random.Range(1, 4);

        switch (directionChoice)
        {
            case 1:
                _robot.GetPlaceByLabel("#Up").Tokens = 1;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 180.0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f); //up
                break;
            case 2:
                _robot.GetPlaceByLabel("#Down").Tokens = 1;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f); //down
                break;
            case 3:
                _robot.GetPlaceByLabel("#Right").Tokens = 1;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
                transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z); //right
                break;
            case 4:
                _robot.GetPlaceByLabel("#Left").Tokens = 1;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, -90.0f);
                transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z); //left
                break;
        }
    }
}

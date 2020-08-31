using RdPengine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Robo : MonoBehaviour
{
    private PetriNet _robot;
    private int directionChoice;
    private float count;
    private bool isMoving;
    private Vector3 target;

    //status Rover
    private Place _life;

    [Header("Status")]
    public GameObject robotPng;
    public Image lifeBar;
    public float speed;
    public int timeIsMoving;

    [Header("Shoot")]
    public GameObject Shoot;
    public Transform spawnShoot;

    // Start is called before the first frame update
    void Start()
    {

        _robot = new PetriNet("Assets/RedesPetri/Robo2.pflow");

        //Define status conform PetriNet
        _life = _robot.GetPlaceByLabel("Life");
        //update values for status
        _life.AddCallback(RefreshTextos, "refreshLife", Tokens.InOrOut);

        RefreshTextos();

        RandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.Translate(target * speed * Time.deltaTime);

            count++;
            if(count >= timeIsMoving)
            {
                count = 0;
                isMoving = false;
                RandomDirection();
            }
        }
        else
        {
            //atirando
        }
        //count -= Time.deltaTime;
        //if (count < 0)
        //{
        //    count = timer;
        //    isIdle = !isIdle;
        //    if (isIdle == true) _robot.GetPlaceByLabel("#Idle").Tokens = 1;
        //    if (_robot.GetPlaceByLabel("#Idle").IsEmpty() == true) RandomDirection();
        //}

        // como fazer a vida aparecer acima do personagem??
        //life.transform.position = new Vector3(robotPng.transform.position.x, robotPng.transform.position.y, robotPng.transform.position.z);

    }

    public void RefreshTextos()
    {
        float value = _life.Tokens;
        value = value / 3;
        lifeBar.fillAmount = value;

        if (value <= 0)
            Destroy(gameObject);
    }

    public void RandomDirection()
    {
        directionChoice = Random.Range(1, 5);

        switch (directionChoice)
        {
            case 1:
                _robot.GetPlaceByLabel("#Up").Tokens = 1;
                target = Vector3.up;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 180.0f);
                break;
            case 2:
                _robot.GetPlaceByLabel("#Down").Tokens = 1;
                target = Vector3.down;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                break;
            case 3:
                _robot.GetPlaceByLabel("#Right").Tokens = 1;
                target = Vector3.right;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
                break;
            case 4:
                _robot.GetPlaceByLabel("#Left").Tokens = 1;
                target = Vector3.left;
                robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, -90.0f);
                break;
        }

        timeIsMoving = Random.Range(30, 120);

        isMoving = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shoot"))
        {
            _robot.GetPlaceByLabel("#Damage").Tokens = 1;
        }
    }

    public void DetectPlayer()
    {
        _robot.GetPlaceByLabel("#Proximity").Tokens = 1;
        isMoving = false;
    }

    public void ExitDecetPlayer()
    {
        _robot.GetPlaceByLabel("#Reset").Tokens = 1;
        isMoving = true;
    }
}

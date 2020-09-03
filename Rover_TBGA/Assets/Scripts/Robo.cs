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
    private bool isMoving = true;
    private Vector3 target;
    private GameObject _currentPlayer;

    //status Rover
    private Place _life;

    [Header("Status")]
    public GameObject robotPng;
    public Image lifeBar;
    public float speed;
    public float timeSleepMin;
    public float timeSleepMax;

    [Header("Shoot")]
    public GameObject Shoot;
    public Transform spawnShoot;

    public GameObject areaView;

    // Start is called before the first frame update
    void Start()
    {

        _robot = new PetriNet("Assets/RedesPetri/Robo2.pflow");

        //Define status conform PetriNet
        _life = _robot.GetPlaceByLabel("Life");
        //update values for status
        _life.AddCallback(RefreshTextos, "refreshLife", Tokens.InOrOut);

        _currentPlayer = GameObject.FindGameObjectWithTag("Player");

        RefreshTextos();

        RandomDirection();
    }

    public void RefreshTextos()
    {
        float value = _life.Tokens;
        value = value / 3;
        lifeBar.fillAmount = value;

        if (value <= 0)
        {
            _currentPlayer.GetComponent<Rover>().SetCanSHoot(false);
            Destroy(gameObject, 0.1f);
        }
    }

    public void RandomDirection()
    {
        if(isMoving)
        {
            directionChoice = Random.Range(1, 5);

            switch (directionChoice)
            {
                case 1:
                    _robot.GetPlaceByLabel("#Up").Tokens = 1;
                    //target = Vector3.up;
                    robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 180.0f);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.0f);
                    break;
                case 2:
                    _robot.GetPlaceByLabel("#Down").Tokens = 1;
                    //target = Vector3.down;
                    robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.0f);
                    break;
                case 3:
                    _robot.GetPlaceByLabel("#Right").Tokens = 1;
                    //target = Vector3.right;
                    robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
                    transform.position = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
                    break;
                case 4:
                    _robot.GetPlaceByLabel("#Left").Tokens = 1;
                    //target = Vector3.left;
                    robotPng.transform.eulerAngles = new Vector3(90.0f, 0.0f, -90.0f);
                    transform.position = new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z);
                    break;
            }

            float time0 = timeSleepMin;
            float time1 = timeSleepMax;
            float time = Random.Range(time0, time1);
            Invoke("RandomDirection", time);
        }
        else
        {
            //aqui é atirando no player
        }

        //timeIsMoving = Random.Range(30, 120);

        //isMoving = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shoot"))
        {
            _robot.GetPlaceByLabel("#Damage").Tokens = 1;
            other.gameObject.SetActive(false);
        }
    }

    public void DetectPlayer()
    {
        _robot.GetPlaceByLabel("#Proximity").Tokens = 1;
        areaView.SetActive(false);
        isMoving = false;
    }

    public void ExitDecetPlayer()
    {
        _robot.GetPlaceByLabel("#Reset").Tokens = 1;
        isMoving = true;
    }
}

using RdPengine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    private bool pauseShoot;

    //status Rover
    private Place _life;
    private Place _ammo;

    [Header("Status")]
    public Image lifeBar;
    public float speed;
    public float timeSleepMin;
    public float timeSleepMax;

    public Text ammoText;

    [Header("Shoot")]
    private float _reloadShoot;
    public GameObject Shoot;
    public Transform spawnShoot;
    public Image reloadBar;
    public float timeReload;
    public float timeFullReload;
    public GameObject reloadText;

    // Start is called before the first frame update
    void Start()
    {

        _robot = new PetriNet("Assets/RedesPetri/Robo2.pflow");

        //Define status conform PetriNet
        _life = _robot.GetPlaceByLabel("Life");
        //update values for status
        _life.AddCallback(RefreshTextos, "refreshLife", Tokens.InOrOut);

        //Define status conform PetriNet
        _ammo = _robot.GetPlaceByLabel("Ammo");
        //update values for status
        _ammo.AddCallback(RefreshTextos, "refreshAmmo", Tokens.InOrOut);

        _currentPlayer = GameObject.FindGameObjectWithTag("Player");

        RefreshTextos();

        //RandomDirection();
    }

    public void RefreshTextos()
    {
        ammoText.text = "Munição " + _ammo.Tokens.ToString();
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
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.0f);
                    break;
                case 2:
                    _robot.GetPlaceByLabel("#Down").Tokens = 1;
                    //target = Vector3.down;
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.0f);
                    break;
                case 3:
                    _robot.GetPlaceByLabel("#Right").Tokens = 1;
                    //target = Vector3.right;
                    transform.position = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
                    break;
                case 4:
                    _robot.GetPlaceByLabel("#Left").Tokens = 1;
                    //target = Vector3.left;
                    transform.position = new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z);
                    break;
            }

            float time0 = timeSleepMin;
            float time1 = timeSleepMax;
            float time = Random.Range(time0, time1);
            Invoke("RandomDirection", time);
        }

        //timeIsMoving = Random.Range(30, 120);

        //isMoving = true;
    }

    private void Update()
    {
        if(!isMoving)
        {
            if(_ammo.Tokens > 0)
            {
                if(!pauseShoot)
                {
                    if (reloadBar.color != Color.yellow)
                    {
                        reloadBar.color = Color.yellow;
                        reloadText.SetActive(false);
                    }

                    _reloadShoot += Time.deltaTime;
                    reloadBar.fillAmount = _reloadShoot / timeReload;

                    if (_reloadShoot >= timeReload)
                    {
                        _robot.GetPlaceByLabel("#shoot").Tokens = 1;
                        Instantiate(Shoot, spawnShoot.position, spawnShoot.rotation);
                        _reloadShoot = 0;
                    }
                }
            }
            else
            {
                if(reloadBar.color != Color.red)
                {
                    reloadBar.color = Color.red;
                    reloadText.SetActive(true);
                }

                _reloadShoot += Time.deltaTime;
                reloadBar.fillAmount = _reloadShoot / timeFullReload;

                if(_reloadShoot >= timeFullReload)
                {
                    _robot.GetPlaceByLabel("#reloadAmmo").Tokens = 1;
                    _reloadShoot = 0;
                }
            }
        }
    }

    public void SetDamage()
    {
        _robot.GetPlaceByLabel("#Damage").Tokens = 1;
    }

    public void DetectPlayer()
    {
        _robot.GetPlaceByLabel("#Proximity").Tokens = 1;
        isMoving = false;
    }

    public bool GetProximity()
    {
        return _robot.GetPlaceByLabel("#Proximity").Tokens == 1 ? true : false;
    }

    public void ExitDecetPlayer()
    {
        _robot.GetPlaceByLabel("#Reset").Tokens = 1;
        isMoving = true;
        pauseShoot = false;
    }

    public bool GetPauseShoot()
    {
        return pauseShoot;
    }

    public void SetPauseShoot(bool p_status)
    {
        pauseShoot = p_status;
    }
}

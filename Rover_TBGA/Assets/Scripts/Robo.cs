using RdPengine;
using UnityEngine;
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
    private Place _fuel;

    [Header("Status")]
    public Image lifeBar;
    public float speed;
    private float walkingTime;
    private float downTime;
    private float walkingSeg;
    private bool stopped;
    private float reloadFuel;
    public int timeReloadFuel;
    public Image fuelBar;

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

        //Define status conform PetriNet
        _fuel = _robot.GetPlaceByLabel("Fuel");
        //update values for status
        _fuel.AddCallback(RefreshTextos, "refreshFuel", Tokens.InOrOut);

        _currentPlayer = GameObject.FindGameObjectWithTag("Player");

        RefreshTextos();

        directionChoice = Random.Range(1, 5);

        RandomDirection();
    }

    public void RefreshTextos()
    {
        ammoText.text = "Munição " + _ammo.Tokens.ToString();
        float value = _life.Tokens;
        value = value / 3;
        lifeBar.fillAmount = value;

        float value2 = _fuel.Tokens;
        fuelBar.fillAmount = value2 / 100;

        if (value <= 0)
        {
            _currentPlayer.GetComponent<Rover>().SetCanSHoot(false);
            Destroy(gameObject, 0.1f);
        }
    }    

    public void RandomDirection()
    {
        int newSort;

        do
            newSort = Random.Range(1, 5);
        while (newSort == directionChoice);

        directionChoice = newSort;

        switch (directionChoice)
        {
            case 1:
                _robot.GetPlaceByLabel("#Up").Tokens = 1;
                target = Vector3.forward;
                break;
            case 2:
                _robot.GetPlaceByLabel("#Down").Tokens = 1;
                target = -Vector3.forward;
                break;
            case 3:
                _robot.GetPlaceByLabel("#Right").Tokens = 1;
                target = Vector3.right;
                break;
            case 4:
                _robot.GetPlaceByLabel("#Left").Tokens = 1;
                target = Vector3.left;
                break;
        }

        walkingTime = Random.Range(4, 10);
        downTime = Random.Range(1, 3);
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
                if (reloadBar.color != Color.red)
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
        else //esta se movendo
        {
            if(_fuel.Tokens > 0)
            {
                walkingSeg += Time.deltaTime;

                if (!stopped)
                {
                    if (walkingSeg < walkingTime)
                        transform.Translate(target * speed * Time.deltaTime);
                    else
                    {
                        stopped = true;
                        walkingSeg = 0;
                    }
                }
                else
                {
                    if (walkingSeg >= downTime)
                    {
                        RandomDirection();
                        stopped = false;
                        walkingSeg = 0;
                    }
                }
            }
            else
            {
                reloadFuel += Time.deltaTime;

                if(reloadFuel >= timeReloadFuel)
                {
                    reloadFuel = 0;
                    _robot.GetPlaceByLabel("#ReloadFuel").Tokens = 1;
                }
            }
        }
    }

    public void DetectColissionCave()
    {
        stopped = true;
        walkingSeg = 0;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public InputController InputController { get; private set; }
    public CanvasManager CanvasManager { get; private set; }

    [Header("objetos instânciaveis")]
    public GameObject Rover;
    public GameObject Robot;
    //public GameObject poolShoot;
    public GameObject fuel;
    public GameObject ammo;
    public GameObject soldier;
    public GameObject portal;

    private int soldiersInScene = 0;

    private void Awake()
    {
        Instance = this;
        InputController = GetComponentInChildren<InputController>();
        CanvasManager = GetComponentInChildren<CanvasManager>();

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartNumberSoldier()
    {
        soldiersInScene = GameObject.FindGameObjectsWithTag("Soldier").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(0);
    }

    public void SetSoldierInScene(int p_value)
    {
        soldiersInScene = p_value;
    }

    public int GetSoldierInScene()
    {
        return soldiersInScene;
    }
}

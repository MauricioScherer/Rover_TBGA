﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public InputController InputController { get; private set; }
    public CanvasManager CanvasManager { get; private set; }

    [Header("objetos instânciaveis")]
    public GameObject roverPlayer;
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

        //if(SceneManager.GetActiveScene().name != "StartMenu")
        //{
        //    ActiveMouse(false);
        //}
        //else
        //{
        //    ActiveMouse(true);
        //}
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

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(2);
        else
            Reiniciar();
    }

    public void SetSoldierInScene(int p_value)
    {
        soldiersInScene = p_value;
    }

    public int GetSoldierInScene()
    {
        return soldiersInScene;
    }

    public void ActiveMouse(bool active)
    {
        if(active)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = active;
    }
}

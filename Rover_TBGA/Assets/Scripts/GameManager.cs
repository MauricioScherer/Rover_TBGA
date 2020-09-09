﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public InputController InputController { get; private set; }
    public CanvasManager CanvasManager { get; private set; }

    public int soldiersInScene = 0;

    private void Awake()
    {
        Instance = this;
        InputController = GetComponentInChildren<InputController>();
        CanvasManager = GetComponentInChildren<CanvasManager>();

        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
}

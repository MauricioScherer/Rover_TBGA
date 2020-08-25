using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Rover : MonoBehaviour
{
    #region VAR Status
    private int life;
    private int ammunition;
    private int fuel;
    #endregion

    public PetriNet rover;

    void Start()
    {
        rover = new PetriNet("Assets/RedesPetri/Rover.pflow");    //instancia RdP do Lander e constroi sua rede...

        SetLife(rover.GetPlaceByLabel("Saude").Tokens);
        SetAmmunition(rover.GetPlaceByLabel("Municao").Tokens);
        SetFuel(rover.GetPlaceByLabel("Combustivel").Tokens);
    }

    void Update()
    {
        Debug.Log(rover.GetPlaceByLabel("Combustivel").Tokens);
        rover.ExecCycle();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move();
        }
    }

    #region Status
    //life
    public void SetLife(int p_life)
    {
        this.life = p_life;
    }
    public int GetLife()
    {
        return this.life;
    }
    //ammunition
    public void SetAmmunition(int p_ammunition)
    {
        this.ammunition = p_ammunition;
    }
    public int GetAmmunition()
    {
        return this.ammunition;
    }
    //fuel
    public void SetFuel(int p_fuel)
    {
        this.fuel = p_fuel;
    }
    public int GetFuel()
    {
        return this.fuel;
    }
    #endregion

    #region Shoot
    private void Shoot()
    {
        if (ammunition > 0)
        {
            ActiveToken("Atirar");
            SetAmmunition(rover.GetPlaceByLabel("Municao").Tokens);
        }
        else
        {
            Debug.Log("Sem munição");
        }
    }
    private void ReloadAmmo()
    {
        if (ammunition < 30)
        {
            ActiveToken("RecarregarMunicao");
            SetAmmunition(rover.GetPlaceByLabel("Municao").Tokens);
        }
        else
        {
            Debug.Log("Munição cheia");
        }
    }
    #endregion

    #region Move
    private void Move()
    {
        if (fuel > 0)
        {
            ActiveToken("Deslocar");
            SetFuel(rover.GetPlaceByLabel("Combustivel").Tokens);
        }
        else
        {
            Debug.Log("Sem combustivel");
        }
    }
    #endregion

    #region Rescue
    private void Rescue()
    {
        if (fuel > 1)
        {
            ActiveToken("Resgatar");
            SetFuel(rover.GetPlaceByLabel("Combustivel").Tokens);
        }
        else
        {
            Debug.Log("Precisa no mínimo de 2 unidades de combustivel para resgatar");
        }
    }
    #endregion

    #region PetriNetRover
    private void ActiveToken(string p_nameLabel)
    {
        rover.GetPlaceByLabel(p_nameLabel).Tokens = 1;
    }
    #endregion
}

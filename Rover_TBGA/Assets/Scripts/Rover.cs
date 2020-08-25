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
            Deslocar();
        }
    }

    #region SetStatus
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

    #region Events
    private void Deslocar()
    {
        if(fuel > 0)
        {
            rover.GetPlaceByLabel("Deslocar").Tokens = 1;
            SetFuel(rover.GetPlaceByLabel("Combustivel").Tokens);
        }
        else
        {
            Debug.Log("Sem combustivel");
        }
    }
    private void Resgatar()
    {
        if (fuel > 1)
        {
            rover.GetPlaceByLabel("Resgatar").Tokens = 1;
            SetFuel(rover.GetPlaceByLabel("Combustivel").Tokens);
        }
        else
        {
            Debug.Log("Precisa no mínimo de 2 unidades de combustivel para resgatar");
        }
    }
    #endregion
}

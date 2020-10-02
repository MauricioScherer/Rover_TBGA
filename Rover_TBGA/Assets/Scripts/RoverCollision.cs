using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverCollision : MonoBehaviour
{
    public Rover rover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            rover.RechargeAmmo();
            other.GetComponent<Item>().GetItem();
            GameManager.Instance.CanvasManager.SetMensage("Mais munição!");
        }
        else if (other.CompareTag("Fuel"))
        {
            rover.RechargeFuel();
            other.GetComponent<Item>().GetItem();
            GameManager.Instance.CanvasManager.SetMensage("Rover recarregado!");
        }
        else if (other.CompareTag("Soldier"))
        {
            rover.RescueSoldier();
            other.GetComponent<Soldier>().GetSoldier();
            GameManager.Instance.CanvasManager.SetMensage("Soldado regatado!");
        }
        else if (other.CompareTag("Portal"))
        {
            if (rover.GetSoldiersInRover() == GameManager.Instance.GetSoldierInScene())
            {
                rover.PortalActive();
                rover.Throttle = 0;
                GameManager.Instance.CanvasManager.CallMissionPainel(true);
                GameManager.Instance.CanvasManager.SetMensage("Resgate Concluído");
            }
            else
            {
                int sdFaltantes = GameManager.Instance.GetSoldierInScene() - rover.GetSoldiersInRover();

                GameManager.Instance.CanvasManager.SetMensage("Faltam " + "<b>" + "<color='red'>" + sdFaltantes + "</color>" +"</b>" + " soldados a serem regatados!");
            }
        }

    }

    public void Damage()
    {
        rover.SetDamage();
    }
}

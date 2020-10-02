using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Text mensage;
    public float timeMensage;

    [Header("Status Rover")]
    public Text fuel;
    public Text ammo;
    public Text life;
    public Text soldier;
    public Image reloadAmmo;
    public Text ammoMensage;

    [Header("Painel")]
    public GameObject blackScreen;
    public Image info;
    public Sprite[] sprite;

    public void SetMensage(string p_mensage)
    {
        mensage.text = p_mensage;
        mensage.gameObject.SetActive(true);
        Invoke("DesativeMensage", timeMensage);
    }

    public void DesativeMensage()
    {
        mensage.gameObject.SetActive(false);
    }

    public void SetFuel(string p_value)
    {
        fuel.text = p_value;
    }
    public void SetAmmo(string p_value)
    {
        ammo.text = p_value;
    }
    public void SetLife(string p_value)
    {
        life.text = p_value;
    }
    public void SetSoldier(string p_value)
    {
        soldier.text = p_value;
    }
    public void SetReloadAmmo(float p_value)
    {
        reloadAmmo.fillAmount = p_value;
    }
    public void SetColorReloadAmmo(Color p_color)
    {
        reloadAmmo.color = p_color;
    }
    public void SetAmmoMensage(string p_value)
    {
        ammoMensage.text = p_value;
    }
    public void CallMissionPainel(bool win)
    { 
        blackScreen.SetActive(true);
        if(win)
        {
            info.sprite = sprite[0];
        }
        else
        {
            info.sprite = sprite[1];
        }
        GameManager.Instance.ActiveMouse(true);
    }
}

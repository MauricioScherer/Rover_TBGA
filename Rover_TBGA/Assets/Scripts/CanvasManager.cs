using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Text mensage;
    public float timeMensage;

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
}

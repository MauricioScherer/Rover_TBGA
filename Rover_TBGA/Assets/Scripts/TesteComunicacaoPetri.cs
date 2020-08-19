using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class TesteComunicacaoPetri : MonoBehaviour
{
    public PetriNet petriTeste;
    public Text qtPlace1;
    public Text qtPlace2;
    public Text qtPlaceFinal;
    public Image transitionX;
    public Image transitionY;
    public Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        petriTeste = new PetriNet("Assets/RedesPetri/RedePetriTeste.pflow");    //instancia RdP do Lander e constroi sua rede...
    }

    // Update is called once per frame
    void Update()
    {
        petriTeste.ExecCycle();

        qtPlace1.text = petriTeste.GetPlaceByLabel("Place_1").Tokens.ToString();
        qtPlace2.text = petriTeste.GetPlaceByLabel("Place_2").Tokens.ToString();
        qtPlaceFinal.text = petriTeste.GetPlaceByLabel("Place_Final").Tokens.ToString();

        if (petriTeste.GetPlaceByLabel("Place_1").Tokens > 0)
            transitionX.color = colors[0];
        else
            transitionX.color = colors[1];

        if (petriTeste.GetPlaceByLabel("Place_2").Tokens > 1)
            transitionY.color = colors[0];
        else
            transitionY.color = colors[1];
    }

    public void SetPlace1(GameObject p_obj)
    {
        if(p_obj.name == "Place_1")
            petriTeste.GetPlaceByLabel("Place_1").Tokens += 1;
        else if (p_obj.name == "Place_2")
            petriTeste.GetPlaceByLabel("Place_2").Tokens += 1;
    }
}

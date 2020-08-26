using System;
using System.Collections;
using RdPengine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PetriNet player;
    public float speed;
   
    public Text countText;
    public Text winText;
    public Text energyText;
        
    private Rigidbody2D rb2d;

    private GameObject boss, boss2, portal;
    
    private Place pontos;
    private Place energy;    
    
    void Start()
    {
        player = new PetriNet("Assets/player.pflow");
        rb2d = GetComponent<Rigidbody2D>();
        pontos = player.GetPlaceByLabel("Pontos");
        energy = player.GetPlaceByLabel("Energy");   
        
        pontos.AddCallback(RefreshTextos,"refreshPontos",Tokens.In);
        energy.AddCallback(RefreshTextos,"refreshEnergy", Tokens.InOrOut);

        player.GetPlaceByLabel("Win!").AddCallback(Message,"a", Tokens.In);
        player.GetPlaceByLabel("GameOver").AddCallback(Message,"b", Tokens.In);

        winText.text = "";
            
        RefreshTextos();
        
        (portal = GameObject.Find("Portal")).SetActive(false);
        (boss = GameObject.Find("BossEnemy")).SetActive(false);
        (boss2 = GameObject.Find("BossEnemy2")).SetActive(false);

        StartCoroutine("EnergyDecrement");
    }
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * speed);
    }
    public void Message()
    {
        winText.text = (player.GetPlaceByLabel("Win!").Tokens == 1) ? "Venceu!" : "Game over!" ;
        GameObject.Find("Player").SetActive(false);
    }   
    public void RefreshTextos()
     {
         countText.text = "Pontos:" + pontos.Tokens.ToString();
         energyText.text = "Energy:" + energy.Tokens.ToString();
     }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(("Enemy")))
            player.GetPlaceByLabel("#CollisionWithEnemy").Tokens = 1;
        else
            if (other.gameObject.CompareTag(("Boss")))
                player.GetPlaceByLabel("#CollisionWithBoss").Tokens = 1;
 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            player.GetPlaceByLabel("#@Pickup").Tokens = 1;
            other.gameObject.SetActive(false);
            switch (pontos.Tokens)
            {
                case 3: boss.SetActive(true);
                    break;
                case 6:  boss2.SetActive(true);
                    break;
                case 10: portal.SetActive(true);
                    break;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Energy"))
            {
                other.gameObject.SetActive(false);
                player.GetPlaceByLabel("#@Cherries").Tokens = 1;
            }
            else
            {
                if (other.gameObject.CompareTag("Portal"))
                {
                    other.gameObject.SetActive(false);
                    player.GetPlaceByLabel("#@Portal").Tokens = 1;
                }  
            }
        }
    }
    IEnumerator EnergyDecrement()
    {
        while (energy.Tokens > 0)
        {
            yield return new WaitForSeconds(2);
            player.GetPlaceByLabel("#TimeElapsed").Tokens = 1;
        }
    }
}

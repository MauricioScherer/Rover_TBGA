using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOfLife : MonoBehaviour
{
    public int state = 0; // Estados da celula: 0 - off | 1 - on.
    public int[] position;
    SpriteRenderer sprRend;
    public float age = 0; //Idade da celula.

    private int gridSizeX, gridSizeY;


    private void Start()
    {
        gridSizeX = GameObject.FindGameObjectWithTag("GameOfLife").GetComponent<GameOfLife>().X;
        gridSizeY = GameObject.FindGameObjectWithTag("GameOfLife").GetComponent<GameOfLife>().Y;
    }

    public void CreateCell(int[] pos, Sprite newSpr)
    {
        state = 0;
        position = pos;

        this.transform.position = new Vector3(-100 / 2 + position[0] + 0.5f, 3f, -100 / 2 + position[1] + 0.5f);
        this.transform.Rotate(90f, 0f, 0f);
        this.gameObject.AddComponent<BoxCollider>(); //Adicionando box collider para usar o mouse raycast
        this.gameObject.AddComponent<SpriteRenderer>();
        sprRend = this.gameObject.GetComponent<SpriteRenderer>();
        SetSprite(newSpr);
    }

    public void SetState(int newStatus)
    {
        state = newStatus;
        ChangeColour();
    }

    void SetSprite(Sprite newSpr)
    {
        sprRend.sprite = newSpr;
        sprRend.color = new Color(255, 255, 255, 0);
    }

    public void SwitchState()
    {
        state = state == 0 ? 1 : 0;
        SetState(state);
    }

    public void ChangeColour()
    {
        if (state == 1)
        {
            age += 0.01f;
            if (age > 1) age = 1;
            sprRend.color = new Color(255, 255, 255, age);
        }
        else
        {
            age -= 0.003f;
            if (age < 0) age = 0;
            sprRend.color = new Color(255, 255, 255, age);
        }   
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public MapGenerator_meu mapGenerator;
    [Range(0.00001f, 3f)] public float updateRate  = 0.1f;
    [Range(50, 500)] public int ciclesRate = 500;

    public bool randomizeAtStart = false;
    public bool start = true;
    public bool smooth = false;

    [Space]
    public Sprite tile;
    [Space]

    [SerializeField] private int cicles = 0;
    private GameObject cell;

    public int X
    {
        get => gridSizeX;
        set => gridSizeX = value;
    }

    public int Y
    {
        get => gridSizeY;
        set => gridSizeY = value;
    }

    private int gridSizeX, gridSizeY;
    private CellOfLife[,] cells;
    private int[,] states;


    private void Start()
    {
        gridSizeX = mapGenerator.width;
        gridSizeY = mapGenerator.height;

        cells = new CellOfLife[gridSizeX, gridSizeY];
        states = new int[gridSizeX, gridSizeY];
    
        CreateGrid(gridSizeX, gridSizeY);

        if(randomizeAtStart)
        {
            RandomizeGrid();
        }

        //Se verdadeiro inicia imendiatamente e atualiza a cada 0.1 segundos
        if (start)
        {
            InvokeRepeating("UpdateStates", 0.1f, updateRate);
        }
    }

    private void FixedUpdate()
    {
        if (start)
        {
            if (cicles > ciclesRate)
            {
                RandomizeGrid();
                cicles = 0;
            }
        }
    }

    void CreateGrid(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Nome da posição e passando o component CellOfLife para 'cells' na posição [x,y].
                cell = new GameObject("x: " + x + " y: " + y); 
                cell.AddComponent<CellOfLife>().CreateCell(new int[] { x, y }, tile);
                cells[x, y] = cell.GetComponent<CellOfLife>();
                cell.transform.SetParent(gameObject.transform);
            }
        }
    }

    public void RandomizeGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //0 para off e 1 para on
                cells[x, y].SetState(Random.Range(0, 2));
            }
        }
    }

    public void UpdateStates()
    {
       //Loop para pegar o grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                int state = cells[x, y].state;  //Pegando o state inicial da celula
                int result = state; //Guardando o state inicial da celula

                int count = GetLivingNeighbours(x, y);  //Guardando o quantidade de vizinhos vivos da celula

                //Aplicando as regras do GAME OF LIFE e atualizando 'result'

                if (state == 1 && count < 2) result = 0;                    //Morre por ser solitária
                
                if (state == 1 && (count == 2 || count == 3)) result = 1;   //Se mantém viva
                
                if (state == 1 && count > 3) result = 0;                    //Morre por superpopulação
                
                if (state == 0 && count == 3) result = 1;                   //Nasce


                //Smooth inspirado em 'sistemas de carverna'
                if (smooth)
                {
                    if (count > 4) result = 1;
                    else if (count < 4) result = 0;
                }

                //Criando um array de resultados para aplicar no proximo clico das células
                states[x, y] = result; 
            }
        }

        //Aplicando os resultados nas células
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                cells[x, y].SetState(states[x, y]);
            }
        }

        cicles++;
    }

    public int GetLivingNeighbours(int x, int y)
    {
        int count = 0;

        //Lengenda:
        //[x , y] = eixo x e y da célula
        //[i , j] = eixo x e y do vizinho

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //Pegando as celulas vizinhas passando em 'cells' os valores de [col,row]
                int col = (x + i + gridSizeX) % gridSizeX;
                int row = (y + j + gridSizeY) % gridSizeY;

                count += cells[col, row].state; // Contando as células vivas
                //Se state é 1 é feito a soma, se 0 nada acontece. 
            }
        }

        //Retirando a célula atual da contagem
        count -= cells[x, y].state;

        return count;
    }

    public void Nebline()
    {
        InvokeRepeating("UpdateStates", 0.1f, updateRate);
        start = true;
    }

    public void Dissipate()
    {
        CancelInvoke();
        start = false;

        //Loop para pegar o grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                cells[x, y].state = 0;
            }
        }
    }
}

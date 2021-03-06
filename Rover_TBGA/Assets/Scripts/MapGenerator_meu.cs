﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Dijkstra;

public class MapGenerator_meu : MonoBehaviour
{
    public int width;
    public int height;
    private int posX, posY;
    private bool flood;
    public int numCicle = 0;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public GameObject parede;
    public GameObject teto;
    //private int counterTeste = 0;
    //private List<GameObject> objTemp = new List<GameObject>();

    //qt numero ciclos para instanciar objeto
    private int cicloRover;
    private int cicloPortal;

    [SerializeField]
    private int qtSoldier;
    private int[] cicloSoldados;

    [SerializeField]
    private int qtRobot;
    private int[] cicloRobot;

    [SerializeField]
    private int qtFuel;
    private int[] cicloFuel;

    [SerializeField]
    private int qtAmmo;
    private int[] cicloAmmo;

    int[,] map;

    int[,] mapFloodFill;
    int[,] mapStatus;
    Vector3[,] mapPositions;

    List<Vector3> position = new List<Vector3>();

    List<Vector3> soldierCount = new List<Vector3>();

    [SerializeField]
    private GameObject tile0;
    //[SerializeField]
    //private GameObject tile1;

    private Transform roverPosition;
    private bool stayLoop;

    private void Start()
    {
        roverPosition = GameManager.Instance.roverPlayer.GetComponent<Transform>();
        cicloSoldados = new int[qtSoldier];
        cicloRobot = new int[qtRobot];
        cicloFuel = new int[qtFuel];
        cicloAmmo = new int[qtAmmo];
        GenerateMap();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown("t"))
        {
            DistanciaMaisCurta();
        }
    }

    //private void FixedUpdate()
    //{
    //    if (Input.GetKey("t"))
    //    {
    //        if (counterTeste < numCicle)
    //        {
    //            //objTemp[counterTeste].GetComponent<MeshRenderer>().material = testeProgressao;
    //            counterTeste++;
    //        }
    //    }
    //}

    void GenerateMap()
    {
        map = new int[width, height];
        mapStatus = new int[width, height];
        mapPositions = new Vector3[width, height];

        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
        DrawMap();

        posX = 0;
        posY = 0;

        do
        {
            if (posX < width - 1)
            {
                posX++;
            }
            else
            {
                posX = 0;
                if (posY < height - 1)
                    posY++;
                else
                    posY = 0;
            }

            if (map[posX, posY] == 0)
            {
                FillMeu(posX, posY);
            }
        }
        while (!flood);
    }

    void RandomFillMap()
    {
        if(seed != "")
        {
            if (useRandomSeed)
            {
                DateTime currentTime = System.DateTime.Now;
                seed = currentTime.Second.ToString() + Time.deltaTime.ToString();
            }
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurrondingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;
            }
        }
    }

    int GetSurrondingWallCount(int p_gridX, int p_gridY)
    {
        int wallCount = 0;
        for (int neighBourX = p_gridX - 1; neighBourX <= p_gridX + 1; neighBourX++)
        {
            for (int neighBourY = p_gridY - 1; neighBourY <= p_gridY + 1; neighBourY++)
            {
                if (neighBourX >= 0 && neighBourX < width && neighBourY >= 0 && neighBourY < height)
                {
                    if (neighBourX != p_gridX || neighBourY != p_gridY)
                    {
                        wallCount += map[neighBourX, neighBourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    private void DrawMap()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, 0.0f, -height / 2 + y + 0.5f);
                    Quaternion rot = new Quaternion(0, 0, 0, 1);

                    mapPositions[x, y] = new Vector3(pos.x + 100, 0.1f, pos.z + 100);

                    if (map[x, y] == 1)
                    {

                        GameObject newBloco;

                        if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
                        {
                            if (map[x + 1, y] == 1 && map[x - 1, y] == 1 && map[x, y + 1] == 1 && map[x, y - 1] == 1)
                            {
                                newBloco = Instantiate(teto, pos, rot);
                            }
                            else
                            {
                                newBloco = Instantiate(parede, pos, rot);
                            }
                        }
                        else
                        {
                            newBloco = Instantiate(parede, pos, rot);
                        }
                        newBloco.transform.parent = transform;
                    }
                }
            }
        }
    }

    public void FillMeu(int p_x, int p_y)
    {
        mapFloodFill = map;

        List<int> ptsx = new List<int>();
        ptsx.Add(p_x);
        List<int> ptsy = new List<int>();
        ptsy.Add(p_y);

        while(ptsx.Count > 0)
        {
            numCicle++;
            if (mapFloodFill[ptsx[0], ptsy[0]] == 0)
            {
                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                position.Add(pos);
                ptsx.Add(ptsx[0]);
                ptsy.Add(ptsy[0]);

                mapFloodFill[ptsx[0], ptsy[0]] = 2;
            }
            if (mapFloodFill[ptsx[0] - 1, ptsy[0]] == 0)
            {
                ptsx.Add(ptsx[0] - 1);
                ptsy.Add(ptsy[0]);

                Vector3 pos = new Vector3(-width / 2 + (ptsx[0] - 1) + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                position.Add(pos);

                mapFloodFill[ptsx[0] - 1, ptsy[0]] = 2;
            }
            if (mapFloodFill[ptsx[0], ptsy[0] - 1] == 0)
            {
                ptsx.Add(ptsx[0]);
                ptsy.Add(ptsy[0] - 1);

                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + (ptsy[0] - 1) + 0.5f);
                position.Add(pos);

                mapFloodFill[ptsx[0], ptsy[0] - 1] = 2;
            }
            if (mapFloodFill[ptsx[0] + 1, ptsy[0]] == 0)
            {
                ptsx.Add(ptsx[0] + 1);
                ptsy.Add(ptsy[0]);

                Vector3 pos = new Vector3(-width / 2 + (ptsx[0] + 1) + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                position.Add(pos);

                mapFloodFill[ptsx[0] + 1, ptsy[0]] = 2;
            }
            if (mapFloodFill[ptsx[0], ptsy[0] + 1] == 0)
            {
                ptsx.Add(ptsx[0]);
                ptsy.Add(ptsy[0] + 1);

                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + (ptsy[0] + 1) + 0.5f);
                position.Add(pos);

                mapFloodFill[ptsx[0], ptsy[0] + 1] = 2;
            }
            ptsx.RemoveAt(0);
            ptsy.RemoveAt(0);
        }

        //numero conforme o que esta na posição
        // 1 -> chao normal
        // 2 -> chao alagado    

        //inicia a distribuição dos objetos de cena
        if(numCicle >= 2000)
        {
            mapStatus = mapFloodFill;

            for(int gridX = 0; gridX < width; gridX++)
            {
                for (int gridY = 0; gridY < height; gridY++)
                {
                    if(mapStatus[gridX, gridY] == 2) //se for considerado uma área possível
                    {
                        //coloca um custo no chao entre 1 e 2
                        mapStatus[gridX, gridY] = UnityEngine.Random.Range(1, 3);
                    }
                    else //paredes e áreas inalcansáveis
                    {
                        mapStatus[gridX, gridY] = 10;
                    }
                }              
            }

            flood = true;
            Quaternion rot = new Quaternion(0, 0, 0, 1);
            cicloPortal = numCicle - 100;
            Instantiate(GameManager.Instance.portal, position[cicloPortal], rot);

            //posição de onde está o portal na grid
            //statusPosition[cicloPortal] = 3;

            //Definição ciclo do Rover
            cicloRover = numCicle / 2;

            int intervalo;

            //definição ciclo dos soldados
            intervalo = numCicle / qtSoldier;
            for (int i = 0; i < cicloSoldados.Length; i++)
            {
                int minValue = intervalo * i;
                int maxValue = i == 0 ? intervalo : minValue * 2;
                maxValue = maxValue > numCicle ? numCicle : maxValue;
                cicloSoldados[i] = UnityEngine.Random.Range(minValue, maxValue);
            }

            //definição ciclo dos robos
            intervalo = numCicle / qtRobot;
            for (int i = 0; i < cicloRobot.Length; i++)
            {
                int minValue = intervalo * i;
                int maxValue = i == 0 ? intervalo : minValue * 2;
                maxValue = maxValue > numCicle ? numCicle : maxValue;
                cicloRobot[i] = UnityEngine.Random.Range(minValue, maxValue);
            }

            //definição ciclo combustivel
            intervalo = numCicle / qtFuel;
            for (int i = 0; i < cicloFuel.Length; i++)
            {
                int minValue = intervalo * i;
                int maxValue = i == 0 ? intervalo : minValue * 2;
                maxValue = maxValue > numCicle ? numCicle : maxValue;
                cicloFuel[i] = UnityEngine.Random.Range(minValue, maxValue);
            }

            //definição ciclo munição
            intervalo = numCicle / qtAmmo;
            for (int i = 0; i < cicloAmmo.Length; i++)
            {
                int minValue = intervalo * i;
                int maxValue = i == 0 ? intervalo : minValue * 2;
                maxValue = maxValue > numCicle ? numCicle : maxValue;
                cicloAmmo[i] = UnityEngine.Random.Range(minValue, maxValue);
            }


            for (int i = 0; i < position.Count; i++)
            {
                if(i == cicloRover)
                {
                    GameManager.Instance.roverPlayer.GetComponent<Transform>().position = position[i];
                    GameManager.Instance.roverPlayer.SetActive(true);

                    print("Posição inicial Rover: " + position[i].x + " / " + position[i].z);
                }
                
                for(int s = 0; s < cicloSoldados.Length; s++)
                {
                    if(cicloSoldados[s] == i)
                    {
                        Instantiate(GameManager.Instance.soldier, position[i], rot);
                        //atualiza a lista que nessa posição tem um soldado
                        //statusPosition[i] = 3;
                        soldierCount.Add(position[i]);
                    }
                }

                for (int r = 0; r < cicloRobot.Length; r++)
                {
                    if (cicloRobot[r] == i)
                    {
                        Instantiate(GameManager.Instance.Robot, position[i], rot);
                    }
                }

                for (int f = 0; f < cicloFuel.Length; f++)
                {
                    if (cicloFuel[f] == i)
                    {
                        Instantiate(GameManager.Instance.fuel, position[i], rot);
                    }
                }

                for (int a = 0; a < cicloAmmo.Length; a++)
                {
                    if (cicloAmmo[a] == i)
                    {
                        Instantiate(GameManager.Instance.ammo, position[i], rot);
                    }
                }

                GameManager.Instance.StartNumberSoldier();
            }

            GetPositionRover();
        }
        else
        {
            //foreach (GameObject p_obj in objTemp)
            //    Destroy(p_obj);
            //objTemp.Clear();
            mapFloodFill = null;
            position.Clear();
            numCicle = 0;
        }

        if(numCicle >= 10000)
        {
            ptsx.Clear();
            ptsy.Clear();
        }
    }

    private void GetPositionRover()
    {
        MainClass.AddDraph(mapPositions, mapStatus, width);
    }

    private void DistanciaMaisCurta()
    {
        Vector3 _rover = roverPosition.position;
        _rover.x += 100;
        _rover.z += 100;
        Vector3 _soldirProximity = soldierCount[0];
        _soldirProximity.x += 100;
        _soldirProximity.z += 100;

        List<Vector3> caminho = new List<Vector3>();
        caminho = MainClass.GetPath(_rover, _soldirProximity);

        for(int i = 0; i < caminho.Count; i++)
        {
            print(caminho[i]);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject player;
    public Material testeProgressao;
    private int counterTeste = 0;
    private List<GameObject> objTemp = new List<GameObject>();

    int[,] map;

    private void Start()
    {
        GenerateMap();
    }

    private void FixedUpdate()
    {
        if(Input.GetKey("t"))
        {
            if(counterTeste < numCicle)
            {
                objTemp[counterTeste].GetComponent<MeshRenderer>().material = testeProgressao;
                counterTeste++;
            }
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
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
        if (useRandomSeed)
        {
            DateTime currentTime = System.DateTime.Now;
            seed = currentTime.Second.ToString() + Time.deltaTime.ToString();
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
        List<int> ptsx = new List<int>();
        ptsx.Add(p_x);
        List<int> ptsy = new List<int>();
        ptsy.Add(p_y);

        bool instancePlayer = false;

        while(ptsx.Count > 0)
        {
            numCicle++;
            if (map[ptsx[0], ptsy[0]] == 0)
            {
                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                Quaternion rot = new Quaternion(0, 0, 0, 1);
                GameObject temp = Instantiate(player, pos, rot);
                objTemp.Add(temp);
            }
            if (map[ptsx[0] - 1, ptsy[0]] == 0)
            {
                ptsx.Add(ptsx[0] - 1);
                ptsy.Add(ptsy[0]);

                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                Quaternion rot = new Quaternion(0, 0, 0, 1);
                GameObject temp = Instantiate(player, pos, rot);
                objTemp.Add(temp);

                map[ptsx[0] - 1, ptsy[0]] = 1;
            }
            if (map[ptsx[0], ptsy[0] - 1] == 0)
            {
                ptsx.Add(ptsx[0]);
                ptsy.Add(ptsy[0] - 1);

                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                Quaternion rot = new Quaternion(0, 0, 0, 1);
                GameObject temp = Instantiate(player, pos, rot);
                objTemp.Add(temp);

                map[ptsx[0], ptsy[0] - 1] = 1;
            }
            if (map[ptsx[0] + 1, ptsy[0]] == 0)
            {
                ptsx.Add(ptsx[0] + 1);
                ptsy.Add(ptsy[0]);

                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                Quaternion rot = new Quaternion(0, 0, 0, 1);
                GameObject temp = Instantiate(player, pos, rot);
                objTemp.Add(temp);

                map[ptsx[0] + 1, ptsy[0]] = 1;
            }
            if (map[ptsx[0], ptsy[0] + 1] == 0)
            {
                ptsx.Add(ptsx[0]);
                ptsy.Add(ptsy[0] + 1);

                Vector3 pos = new Vector3(-width / 2 + ptsx[0] + 0.5f, 0.0f, -height / 2 + ptsy[0] + 0.5f);
                Quaternion rot = new Quaternion(0, 0, 0, 1);
                GameObject temp = Instantiate(player, pos, rot);
                objTemp.Add(temp);

                map[ptsx[0], ptsy[0] + 1] = 1;
            }
            ptsx.RemoveAt(0);
            ptsy.RemoveAt(0);
        }

        if(numCicle >= 2000)
        {
            flood = true;
        }
        else
        {
            foreach (GameObject p_obj in objTemp)            
                Destroy(p_obj);
            
            objTemp.Clear();
            numCicle = 0;
        }
    }
}

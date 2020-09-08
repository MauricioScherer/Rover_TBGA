using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator_meu : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public GameObject parede;
    public GameObject teto;

    int[,] map;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            GenerateMap();
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
    }

    void RandomFillMap()
    {
        if(useRandomSeed)
        {
            DateTime currentTime = System.DateTime.Now;
            seed = currentTime.Second.ToString() + Time.deltaTime.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
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
        for(int neighBourX = p_gridX - 1; neighBourX <= p_gridX + 1; neighBourX++)
        {
            for (int neighBourY = p_gridY - 1; neighBourY <= p_gridY + 1; neighBourY++)
            {
                if(neighBourX >= 0 && neighBourX < width && neighBourY >= 0 && neighBourY < height)
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
                        newBloco = Instantiate(parede, pos, rot);
                        newBloco.transform.parent = transform;
                    }
                    //else
                    //    Instantiate(chao, pos, rot);
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if(map != null)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                Vector3 pos = new Vector3(-width / 2 + x + 0.5f, 0.0f, -height / 2 + y + 0.5f);
    //                Gizmos.DrawCube(pos, Vector3.one);
    //            }
    //        }
    //    }
    //}
}

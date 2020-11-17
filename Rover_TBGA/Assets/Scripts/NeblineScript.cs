using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeblineScript : MonoBehaviour
{
    public Generator generator;
    public GameOfLife gameOfLife;

    private void Start()
    {
        StartCoroutine(Event());
    }

    private IEnumerator Event()
    {
        gameOfLife.Nebline();
        Debug.Log("ATIVO");

        float time = generator.Uniform(10, 50);
        Debug.Log("TIME: " + time);

        yield return new WaitForSeconds(time);

        gameOfLife.Dissipate();

        time = generator.Exponential(30);
        Debug.Log("TIME: " + time);

        yield return new WaitForSeconds(time);
        
        ContinueRoutine();
    }

    private void ContinueRoutine()
    {
        StartCoroutine(Event());
    }
}

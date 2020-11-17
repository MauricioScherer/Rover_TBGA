using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeblineScript : MonoBehaviour
{
    public Generator generator;
    public GameOfLife gameOfLife;
    public int numberOfEvents;
    private int count = 0;

    private void Start()
    {
        StartCoroutine(Event());
    }

    private IEnumerator Event()
    {
        do
        {
            Debug.Log("Event: " + count + " / " + numberOfEvents);

            gameOfLife.Nebline();
            Debug.Log("NEBLINE ACTIVED");

            float time = generator.Normal(60, 1);
            Debug.Log("Time: " + time);

            yield return new WaitForSeconds(time);

            gameOfLife.Dissipate();
            Debug.Log("DISSIPATE..");

            yield return new WaitForSeconds(time);

            count++;

        } while (count < numberOfEvents);
    }
}

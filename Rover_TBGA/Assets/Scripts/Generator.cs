using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private float modulus = Mathf.Pow(2, 31) - 1;
    private float multiplier = 48271;
    private float increment = 0;
    [SerializeField] private float seed;

    private float Random()
    {
        seed = (multiplier * seed + increment) % modulus;

        float normalized = seed / (modulus - 1);

        return normalized;
        //return normalized * -1;
    }

    public void SetSeed(float value) => seed = value;

    public float Uniform(float min, float max)
    {
        float u = Random();
        float delta = max - min;

        return min + (u * delta);
    }

    public float Exponential(float average)
    {
        float u = Random();

        return -average * Mathf.Log(1 - u);
    }

    public float Normal(float average, float deviation)
    {
        float u1, u2, v1, v2, w;
        float normal;

        do
        {
            do
            {
                u1 = Random();
                u2 = Random();

                v1 = 2 * u1 - 1;
                v2 = 2 * u2 - 1;

                w = Mathf.Pow(v1, 2) + Mathf.Pow(v2, 2);

            } while (w >= 1);

            float y, x1, x2;

            y = Mathf.Sqrt((-2 * Mathf.Log(w) / w));

            x1 = v1 * y;
            x2 = v2 * y;

            normal = average + deviation * x1;

        } while (normal < 0);

        return normal;
    }
}

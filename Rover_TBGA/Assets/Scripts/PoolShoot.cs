using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolShoot : MonoBehaviour
{
    private GameObject _shootCurrent;
    public GameObject[] shoots;

    public GameObject ActiveShoot()
    {
        _shootCurrent = null;

        for (int i = 0; i < shoots.Length; i++)
        {
            if (!shoots[i].activeSelf && _shootCurrent == null)
            {
                _shootCurrent = shoots[i];
            }
        }

        return _shootCurrent;
    }
}

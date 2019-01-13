using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CubeSpawer spawer;

    void Awake()
    {
        spawer = FindObjectOfType<CubeSpawer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(spawer.currentCube != null)
            {
                spawer.currentCube.Stop();
            }

            spawer.SpawnCube();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CubeSpawer[] spawers;
    // Start is called before the first frame update
    void Awake()
    {
        spawers = FindObjectsOfType<CubeSpawer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(MovingCube.CurrentCube != null)
            {
                MovingCube.CurrentCube.Stop();
            }

            int index = Random.Range(0, spawers.Length);
            Debug.Log("index = " + index);

            spawers[index].SpawnCube();
        }
    }
}

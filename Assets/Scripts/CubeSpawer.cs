using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CubeData
{
    public GameObject spawnPoint;
    public MoveDirection moveDirection;
}

public class CubeSpawer : MonoBehaviour
{
    public GameObject cubePrefab;

    public MovingCube currentCube;
    public MovingCube lastCube;

    public CubeData[] cubeSpawnData;

    void Awake()
    {
        lastCube = GameObject.Find("Start").GetComponent<MovingCube>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentCube != null)
            {
                currentCube.Stop();
            }

            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        GameObject cube = Instantiate(cubePrefab);

        currentCube = cube.GetComponent<MovingCube>();

        int index = Random.Range(0, cubeSpawnData.Length);
        Debug.Log("index = " + index);

        if (lastCube != null && lastCube.gameObject != GameObject.Find("Start"))
        {
            float x = cubeSpawnData[index].moveDirection == MoveDirection.X ? cubeSpawnData[index].spawnPoint.transform.position.x : lastCube.transform.position.x;
            float z = cubeSpawnData[index].moveDirection == MoveDirection.Z ? cubeSpawnData[index].spawnPoint.transform.position.z : lastCube.transform.position.z;

            cube.transform.position = new Vector3(x, lastCube.transform.position.y + cube.transform.localScale.y, z);
        }
        else
        {
            cube.transform.position = cubeSpawnData[index].spawnPoint.transform.position;
        }

        cube.GetComponent<MovingCube>().MoveDirection = cubeSpawnData[index].moveDirection;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int i = 0; i < cubeSpawnData.Length; i++)
        {
            Gizmos.DrawWireCube(cubeSpawnData[i].spawnPoint.transform.position, cubePrefab.transform.localScale);
        }        
    }
}

public enum MoveDirection
{
    X,
    Z
}

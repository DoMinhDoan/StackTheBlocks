using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public MoveDirection MoveDirection { get; set; }

    private CubeSpawer spawer;

    void Awake()
    {
        spawer = FindObjectOfType<CubeSpawer>();
    }

    void Start()
    {
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(spawer.lastCube.transform.localScale.x, transform.localScale.y, spawer.lastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(MoveDirection == MoveDirection.Z)
        {
            transform.position += Vector3.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        }
             
    }

    internal void Stop()
    {
        moveSpeed = 0.0f;
        float hangover, lastCubeScale;
        if (MoveDirection == MoveDirection.Z)
        {
            hangover = transform.position.z - spawer.lastCube.transform.position.z;
            lastCubeScale = spawer.lastCube.transform.localScale.z;

            float direction = hangover > 0 ? 1.0f : -1.0f;
            SplitCubeOnZ(hangover, direction);
        }
        else
        {
            hangover = transform.position.x - spawer.lastCube.transform.position.x;
            lastCubeScale = spawer.lastCube.transform.localScale.x;

            float direction = hangover > 0 ? 1.0f : -1.0f;
            SplitCubeOnX(hangover, direction);
        }

        
        if (Mathf.Abs(hangover) > lastCubeScale)    // == die
        {
            spawer.lastCube = GameObject.Find("Start").GetComponent<MovingCube>();
            spawer.currentCube = null;
            SceneManager.LoadScene(0);

            return;
        }
        spawer.lastCube = this;
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = spawer.lastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = spawer.lastCube.transform.position.z + (hangover / 2);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2 * direction);
        float fallingBlockZPosition = cubeEdge + (fallingBlockSize / 2 * direction);

        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newZSize = spawer.lastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newZSize;

        float newZPosition = spawer.lastCube.transform.position.x + (hangover / 2);

        transform.localScale = new Vector3(newZSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newZPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newZSize / 2 * direction);
        float fallingBlockXPosition = cubeEdge + (fallingBlockSize / 2 * direction);

        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z);
        }
            

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        Destroy(cube.gameObject, 1.0f);
    }
}

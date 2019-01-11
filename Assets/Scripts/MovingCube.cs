using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }

    public MoveDirection MoveDirection { get; set; }

    void OnEnable()
    {
        if(LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        }
        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
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
        if (MoveDirection == MoveDirection.Z)
        {
            moveSpeed = 0.0f;

            float hangover = transform.position.z - LastCube.transform.position.z;
            if (Mathf.Abs(hangover) > LastCube.transform.localScale.z)
            {
                LastCube = null;
                CurrentCube = null;
                SceneManager.LoadScene(0);
            }

            float direction = hangover > 0 ? 1.0f : -1.0f;

            SplitCubeOnZ(hangover, direction);
        }
        else
        {
            moveSpeed = 0.0f;

            float hangover = transform.position.x - LastCube.transform.position.x;
            if (Mathf.Abs(hangover) > LastCube.transform.localScale.x)
            {
                LastCube = null;
                CurrentCube = null;
                SceneManager.LoadScene(0);
            }

            float direction = hangover > 0 ? 1.0f : -1.0f;

            SplitCubeOnX(hangover, direction);
        }

        LastCube = this;
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2 * direction);
        float fallingBlockZPosition = cubeEdge + (fallingBlockSize / 2 * direction);

        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newZSize;

        float newZPosition = LastCube.transform.position.x + (hangover / 2);

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

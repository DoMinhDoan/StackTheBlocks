using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawer : MonoBehaviour
{
    public GameObject cubePrefab;
    public MoveDirection moveDirection;

    public void SpawnCube()
    {
        GameObject cube = Instantiate(cubePrefab);
        if(MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Start"))
        {
            cube.transform.position = new Vector3(transform.position.x, MovingCube.LastCube.transform.position.y + cube.transform.localScale.y, transform.position.z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.GetComponent<MovingCube>().MoveDirection = moveDirection;
        
    }

    private void OnDrawGizmos()
    {
        
    }
}

public enum MoveDirection
{
    X,
    Z
}

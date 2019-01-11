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
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;

            cube.transform.position = new Vector3(x, MovingCube.LastCube.transform.position.y + cube.transform.localScale.y, z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.GetComponent<MovingCube>().MoveDirection = moveDirection;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}

public enum MoveDirection
{
    X,
    Z
}

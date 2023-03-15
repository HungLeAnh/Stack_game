using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float stopPosition = -3f;
    [SerializeField] private float startPosition = 3f;
    [SerializeField] private bool moveForward = true;


    public static MovingCube CurentCube { get; set; }
    public static MovingCube LastCube { get; set; }
    public MoveDirection MoveDirection { get;  set; }

    private void OnEnable()
    {
        if (LastCube == null)
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        CurentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
        transform.localScale = new Vector3(LastCube.transform.localScale.x,transform.localScale.y,LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {
        moveSpeed = 0;
        float hangover = GetHangover();
        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;
        if (Mathf.Abs(hangover) >= max)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            LastCube = null;
            CurentCube = null;
            //Gamover
            GameManager.instance.isGameOver = true;

        }
        else if (Mathf.Abs(hangover) < 0.05)
        {
            SnapToLastCubePosition();
            LastCube = this;
        }
        else
        {
            float direction = hangover > 0 ? 1f : -1f;
            if (MoveDirection == MoveDirection.Z)
                SplitCubeOnZ(hangover, direction);
            else
                SplitCubeOnX(hangover, direction);
            LastCube = this;

        }
    }

    private void SnapToLastCubePosition()
    {
        CurentCube.transform.position = new Vector3(LastCube.transform.position.x, CurentCube.transform.position.y, LastCube.transform.position.z);
    }

    private float GetHangover()
    {
        if(MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;

    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = LastCube.transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.position.x + (hangover / 2f);
        transform.localScale = new Vector3(Mathf.Abs(newXSize), transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f) * direction;
        float fallinngBlockXPosition = cubeEdge + (fallingBlockSize / 2f) * direction;

        SpawnDropCube(fallinngBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = LastCube.transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Abs(newZSize));
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f)*direction;
        float fallinngBlockZPosition = cubeEdge + (fallingBlockSize / 2f)*direction;

        SpawnDropCube(fallinngBlockZPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallinngBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if(MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Abs(fallingBlockSize));
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallinngBlockZPosition);
        }
        else
        {
            cube.transform.localScale = new Vector3(Mathf.Abs(fallingBlockSize), transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallinngBlockZPosition, transform.position.y, transform.position.z);
        }
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(cube, 2f);
    }

    public void DestroyCube(float time)
    {
        Destroy(gameObject,time);
    }

    private void Update()
    {
        if(MoveDirection == MoveDirection.Z)
        {
            if (moveForward)
            {
                transform.position += transform.forward * Time.deltaTime * moveSpeed;
                if (transform.position.z >= startPosition)
                {
                    moveForward = false;
                }
            }
            else
            {
                transform.position -= transform.forward * Time.deltaTime * moveSpeed;
                if (transform.position.z <= stopPosition  )
                {
                    moveForward = true;
                }
            }

        }
        else
        {
            if (moveForward)
            {
                transform.position += transform.right * Time.deltaTime * moveSpeed;
                if (transform.position.x >= startPosition)
                {
                    moveForward = false;
                }
            }
            else
            {
                transform.position -= transform.right * Time.deltaTime * moveSpeed;
                if (transform.position.x <= stopPosition)
                {
                    moveForward = true;
                }
            }
        }

    }
}

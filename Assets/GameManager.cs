using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Camera mainCamera;

    public static event Action OnSpawingCube = delegate() { };
    public static event Action OnPlay = delegate { };
    public static event Action OnGameOver = delegate { };
    public static event Action<Color, Color> OnRunning = delegate(Color currentColor, Color nextColor) { };

    public static GameObject cubeStack;
    
    private SpawningCube[] spawners;
    private int spawnerIndex;
    private SpawningCube currentSpawner;
    
    private bool isRunning = false;
    private bool gameStart = false;
    private bool gameOver = false;

    private Color currentColor;
    private Color nextColor;

    [SerializeField]
    private float zoomSpeed = 10;
    [SerializeField]
    private Vector3 cameraStartPosition = new Vector3(-3,4,-3);
    [SerializeField]
    private float orthographicSize = 3;

    [SerializeField]
    private float explosionForce = 20;
    [SerializeField]
    private float destroyTime = 0;

    public bool isGameOver { get => gameOver; set => gameOver = value; }

    private void Awake()
    {
        if(instance != null)
        {
            instance = null;
        }
        instance = this;

        spawners = FindObjectsOfType<SpawningCube>();
        cubeStack = GameObject.Find("Stack");
        gameStart = true;
        currentColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f)); 
        nextColor = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f));
        GameManager.OnRunning(currentColor, nextColor);

    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (gameStart)
            {
                isRunning = true;
                gameStart = false;
                return;
            }

            if (isRunning)
            {

                if (MovingCube.CurentCube != null)
                {
                    MovingCube.CurentCube.Stop();
                    if (isGameOver == true)
                    {
                        OnGameOver();
                        isRunning = false;
                        CanvasManager.GetInstance().SwitchCanvas(CanvasType.EndScreen);
                        float gameOverCameraPos = mainCamera.transform.position.y * 2 / 3;
                        GameOverCameraMove(gameOverCameraPos);
                        return;
                    }

                }
                spawnerIndex = spawnerIndex == 0 ? 1 : 0;
                currentSpawner = spawners[spawnerIndex];
                currentSpawner.SpawingCube();
                OnSpawingCube();
                MoveCamera();
                GameManager.OnRunning(currentColor, nextColor);
            }

            if (!isRunning && gameOver)
            {
                //CanvasManager.GetInstance().SwitchCanvas(CanvasType.GameUI);
                for (int i = 0; i < cubeStack.transform.childCount; i++)
                {
                    Transform cube = cubeStack.transform.GetChild(i);
                    Rigidbody rigCube = cube.GetComponent<Rigidbody>();
                    rigCube.isKinematic = false;
                    rigCube.AddForce(explosionForce * cube.transform.up);
                    Destroy(cube.gameObject, destroyTime);
                }
                CanvasManager.GetInstance().SwitchCanvas(CanvasType.GameUI);
                //Reset camera setting
                mainCamera.transform.position = cameraStartPosition;
                mainCamera.orthographicSize = orthographicSize;
                //Restart game
                OnPlay();
                spawnerIndex = spawnerIndex == 0 ? 1 : 0;
                currentSpawner = spawners[spawnerIndex];
                currentSpawner.SpawingCube();

                isRunning = true;
                isGameOver = false;
                return;
            }
        }
        if (isRunning && CanvasManager.GetInstance().GetCurrentCanvasType() != CanvasType.GameUI && gameStart == false)
        {
            CanvasManager.GetInstance().SwitchCanvas(CanvasType.GameUI);
            OnPlay();
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = spawners[spawnerIndex];
            currentSpawner.SpawingCube();
            return;
        }

    }
    private void MoveCamera()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
            mainCamera.transform.position.y + currentSpawner.transform.localScale.y/2,
            mainCamera.transform.position.z);
    }
    private void GameOverCameraMove(float newYPos)
    {
        if(newYPos>= mainCamera.orthographicSize)
        {
            mainCamera.orthographicSize = mainCamera.transform.position.y/2;
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, newYPos, mainCamera.transform.position.z);
        }
        else
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, 4f, mainCamera.transform.position.z);
        }
    }
}

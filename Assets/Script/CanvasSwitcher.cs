using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField]
    private CanvasType desiredCanvasType;

    CanvasManager canvasManager;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        canvasManager = CanvasManager.GetInstance();
    }

    private void OnButtonClicked()
    {
        canvasManager.SwitchCanvas(desiredCanvasType);
    }
}

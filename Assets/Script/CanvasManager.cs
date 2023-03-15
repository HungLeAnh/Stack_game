using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public enum CanvasType
{
    Menu,
    GameUI,
    Setting,
    About,
    More,
    EndScreen
}

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField]
    List<CanvasController> canvasControllersList;
    CanvasController lastActiveCanvas;
    protected override void Awake()
    {
        base.Awake();
        canvasControllersList.ForEach(x => x.gameObject.SetActive(false));
        CanvasController menuController = canvasControllersList.Find(x => x.CanvasType == CanvasType.Menu);
        if(menuController != null)
        {
            menuController.gameObject.SetActive(true);
            lastActiveCanvas = menuController;
        }
        else
        {
            Debug.Log("Missing GameUIController");
        }
    }
    public void SwitchCanvas(CanvasType Type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }
        CanvasController newCanvas = canvasControllersList.Find(x => x.CanvasType == Type);
        if(newCanvas!= null)
        {
            newCanvas.gameObject.SetActive(true);
            lastActiveCanvas = newCanvas;
        }
    }

    public CanvasType GetCurrentCanvasType()
    {
        return lastActiveCanvas.CanvasType;

    }
}

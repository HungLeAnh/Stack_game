using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private CanvasType canvasType;
    
    public CanvasType CanvasType { get => canvasType; set => canvasType = value; }
}

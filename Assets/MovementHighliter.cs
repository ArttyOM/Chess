using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Canvas))]
public class MovementHighliter : MonoBehaviour//, IPointerUpHandler, IPointerDownHandler
{
    public Vector2Int coords;
    public int sortingLayer = 1;

    private Canvas _canvas;
    private Transform _parentTransform;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.overrideSorting = true;
        _canvas.sortingLayerID = sortingLayer;
    
        _parentTransform = this.transform.parent;

        //Debug.Log("координаты родителя:"+_parentTransform.GetComponent<CellCoords>().coords);
        coords = _parentTransform.GetComponent<CellCoords>().coords;
    }
    //OnPointerDown is also required to receive OnPointerUp callbacks

    //public void OnPointerUp()
    //{
    //    Debug.Log("PoinerUP");
    //    //Messenger<Vector2Int>.Broadcast("PoinerUP",coords);
    //}
}

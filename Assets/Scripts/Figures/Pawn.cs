using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;

public class Pawn : MonoBehaviour, IFigure
{
    public RectTransform FigureRectTransform {get; set; }
    private FigureColor _pawnColor;
    public FigureColor FigureColor
    {
        get { return _pawnColor; }
        set { _pawnColor = value; }
    }

    public void Awake()
    {
        FigureRectTransform = this.gameObject.GetComponent<RectTransform>();
        
    }

    public void OnAttacked()
    {
        Destroy(this.gameObject);
    }
    public void Attack()
    {

    }
    public void Move()
    {

    }
}

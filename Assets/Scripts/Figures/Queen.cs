using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Queen : AbstractFigure
{

    public RectTransform FigureRectTransform { get; set; }
    private FigureColor _figColor;
    public FigureColor FigureColor
    {
        get { return _figColor; }
        set { _figColor = value; }
    }

    public void Awake()
    {
        FigureRectTransform = this.gameObject.GetComponent<RectTransform>();

    }

    public override void OnAttacked()
    {
        Destroy(this.gameObject);
    }
    public override void Attack()
    {

    }
    public override void Move()
    {

    }
    public override List<Vector2Int> PossibilityToMove()
    {
        return null;
    }
    public override List<Vector2Int> PossibilityToAttack()
    {
        return null;
    }
}

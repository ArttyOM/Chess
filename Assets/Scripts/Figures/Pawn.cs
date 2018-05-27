using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pawn : AbstractFigure
{
    public RectTransform FigureRectTransform {get; set; }
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

    private bool isNotMovedYet = true;//свой первый ход пешка имеет право сделать на 2 клетки
    public override List<Vector2Int> PossibilityToMove()
    {
        //считаем на основе своей текущей позиции
        //можно не учитывать белую пешку на 8й строке - она превратится в другую фигуру
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        if (FigureColor == FigureColor.white)
        {
            possibleMoves.Add(new Vector2Int(coords.x, coords.y + 1));//x- буквы, у - цифры
            if (isNotMovedYet) possibleMoves.Add(new Vector2Int(coords.x, coords.y + 2));//x- буквы, у - цифры
        }
        else if (FigureColor == FigureColor.black)//чисто технически есть еще состояние FigureColor.notСonfigured
        {

            possibleMoves.Add(new Vector2Int(coords.x, coords.y -1 ));//x- буквы, у - цифры
            if (isNotMovedYet) possibleMoves.Add(new Vector2Int(coords.x, coords.y -2));//x- буквы, у - цифры
        }
        //Debug.Log("Coords of this " + ((BoardManager.LatVector)coords.x).ToString() + (coords.y+1).ToString());

        return possibleMoves;
    }
    public override List<Vector2Int> PossibilityToAttack()
    {
        return null;
    }


    /// <summary>
    ///   Пешка может ходить на одну клетку вперед 
    ///   Либо на 2, если это её первый ход
    /// </summary>
    public override void Move()
    {
        //родительская ячейка хранится BoardManager.cells[coords.x, coords.y]
        //следовательно
    }
}

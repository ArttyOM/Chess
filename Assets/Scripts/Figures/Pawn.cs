using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pawn : AbstractFigure
{
    
    //TODO спецправило атаки на другие пешки
    //TODO спецправило превращения в другие фигуры
    private bool isNotMovedYet = true;//свой первый ход пешка имеет право сделать на 2 клетки
    private static Pawn _specialAttackTarget = null;//в один ход может существовать только одна возможность использовать спецатаку
    //пешки ходят только вперед относительно своего игрока
    public override List<Vector2Int> PossibilityToMove()
    {
        //Debug.Log(FigureColor.ToString() + " pawn");
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

    private GameObject _specialHighliter;
    public override void HighlightSpecialAttack()
    {
       // Debug.Log("0 control point");
        if (_specialAttackTarget == null) return;
        //Debug.Log("1 control point");
        Vector2Int specialAttackCoords = _specialAttackTarget.coords;
        if (this.coords.y != _specialAttackTarget.coords.y) return;//потенциально атакующая пешка должна быть на той же линии, что и жертва
        //Debug.Log("2 control point");
        if (this.FigureColor == _specialAttackTarget.FigureColor) return; //своя фигура не может быть целью для спецатаки
        //Debug.Log("3 control point");

        if ((this.coords.x + 1 != _specialAttackTarget.coords.x) && (this.coords.x - 1 != _specialAttackTarget.coords.x)) return;
        //Debug.Log("4 control point");
        //атакующая фигура должна быть справа или слева от жертвы

        if (_specialAttackTarget.FigureColor == FigureColor.white)
            specialAttackCoords.y--;
        else if (_specialAttackTarget.FigureColor == FigureColor.black)
            specialAttackCoords.y++;


        _specialHighliter = Instantiate(
            GlobalFields.MyLazyBD.attackHighliter, 
            BoardManager.cells[specialAttackCoords.y, specialAttackCoords.x].transform);
        RectTransform rectTransform = _specialHighliter.GetComponent<RectTransform>();
        StartCoroutine(SetSize(rectTransform));
        Debug.Log("5 control point");
    }

    /// <summary>
    /// пешки рубят только по вперед-вправо и вперед-влево относительно своего игрока
    /// </summary>
    /// <returns></returns>
    public override List<Vector2Int> PossibilityToAttack()
    {
        List<Vector2Int> possibleAttacks = new List<Vector2Int>();
        if (FigureColor == FigureColor.white)
        {
            if (coords.x + 1 <= 7) possibleAttacks.Add(new Vector2Int(coords.x+1, coords.y + 1));//x- буквы, у - цифры
            if (coords.x - 1 >= 0) possibleAttacks.Add(new Vector2Int(coords.x-1, coords.y + 1));
        }
        else if (FigureColor == FigureColor.black)//чисто технически есть еще состояние FigureColor.notСonfigured
        {
            if (coords.x + 1 <= 7) possibleAttacks.Add(new Vector2Int(coords.x+1, coords.y - 1));//x- буквы, у - цифры
            if (coords.x - 1 >= 0) possibleAttacks.Add(new Vector2Int(coords.x-1, coords.y - 1));//x- буквы, у - цифры
        }



        return possibleAttacks;
    }

    public override void Move(Vector2Int coords)
    {
        if (isNotMovedYet) isNotMovedYet = false;
        Debug.Log("Figure moved to " + coords);
        transform.SetParent(BoardManager.cells[coords.y, coords.x].transform, false);
        this.coords = coords;
    }//любую фигура умеет перемещаться

    public override void Move(GameObject hitObject)
    {
        if (hitObject == this.gameObject) return;//если кликнули сами на себя - ничего не происходит

        if ((hitObject.transform.childCount == 0) || (hitObject.GetComponentInChildren<AbstractFigure>() != null)) return;
        //прервать функцию при попытке ходьбы на другую фигуру - для атаки есть метод Attack

        _specialAttackTarget = null;

        //при ходьбе на 2 клетки эта пешка становится целью для спецатаки
        if ((this.coords.y + 2 == hitObject.GetComponent<CellCoords>().coords.y) ||//для белых
            (this.coords.y - 2 == hitObject.GetComponent<CellCoords>().coords.y))//для черных
        {
            _specialAttackTarget = this;
            Debug.Log("Специальная атака доступна против ");
        }
        //но спецатака доступна только пешкам противника и только на 1 полуход

        transform.SetParent(hitObject.transform, false);
        this.coords = hitObject.GetComponent<CellCoords>().coords;//TODO null check

        if (_specialAttackTarget != null) Debug.Log(_specialAttackTarget.coords);
        else Debug.Log("Специальная атака не доступна");

        if (isNotMovedYet) isNotMovedYet = false;

        PlayerController.EndTurn();
    }

    public override void Attack(GameObject hitObject)
    {
        if (_specialHighliter != null && 
            hitObject == _specialHighliter.transform.parent.gameObject) //||(hitObject == _specialHighliter))
        {
            _specialAttackTarget.OnAttacked();
            //Debug.Log("Таргет в прицеле");
        }

        if (hitObject == this.gameObject) return;//если кликнули сами на себя - ничего не происходит

        if (hitObject.transform.childCount <= 1) return;//1 потомок у недоступных для атаки фигур и у клеток, доступных для Move

        if (hitObject.GetComponentInChildren<AbstractFigure>() == null) return;//прервать, если в потомках нет фигур

        _specialAttackTarget = null;

        hitObject.GetComponentInChildren<AbstractFigure>().OnAttacked();//вызовем у атакованной фигуры соответствующий метод

        transform.SetParent(hitObject.transform, false);
        this.coords = hitObject.GetComponent<CellCoords>().coords;//TODO null check

        PlayerController.EndTurn();
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Destroy(_specialHighliter);
    }
}

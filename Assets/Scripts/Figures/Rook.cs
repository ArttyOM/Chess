using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Rook : AbstractFigure
{
    //TODO спецправило рокировки
    public override List<Vector2Int> PossibilityToMove()
    {
        List<Vector2Int> possibleMoves = PossibilityToAttack();
        //+возможность рокировки
        return possibleMoves;
    }
    public override List<Vector2Int> PossibilityToAttack()
    {
        List<Vector2Int> possibleAttacks = new List<Vector2Int>();

        int tx, ty;
        for (int i = 1; i < 7; i++)//вправо 
        {
            tx = coords.x + i;
            ty = coords.y;
            if (tx <= 7)
            {
                AbstractFigure checkedFigure = BoardManager.cells[ty, tx].GetComponentInChildren<AbstractFigure>();
                if ((checkedFigure != null) && (this.FigureColor == checkedFigure.FigureColor)) break;
                //если в очередной ячейке содержится фигура и она своего цвета, прервать ветку 

                possibleAttacks.Add(new Vector2Int(tx, ty));

                if ((checkedFigure != null) && (this.FigureColor != checkedFigure.FigureColor)) break;
                //если фигура вражеская, её можно атаковать, но дальше по этой ветке не пройти
            }
            else break;//прервать цикл, когда мы вышли за пределы хотя бы одной из координат
        }

        for (int i = 1; i < 7; i++)//влево 
        {
            tx = coords.x - i;
            ty = coords.y;
            if (tx >= 0)
            {
                AbstractFigure checkedFigure = BoardManager.cells[ty, tx].GetComponentInChildren<AbstractFigure>();
                if ((checkedFigure != null) && (this.FigureColor == checkedFigure.FigureColor)) break;
                //если в очередной ячейке содержится фигура и она своего цвета, прервать ветку 

                possibleAttacks.Add(new Vector2Int(tx, ty));

                if ((checkedFigure != null) && (this.FigureColor != checkedFigure.FigureColor)) break;
                //если фигура вражеская, её можно атаковать, но дальше по этой ветке не пройти
            }
            else break;//прервать цикл, когда мы вышли за пределы хотя бы одной из координат
        }

        for (int i = 1; i < 7; i++)//вниз
        {
            tx = coords.x ;
            ty = coords.y - i;
            if (ty >= 0)
            {
                AbstractFigure checkedFigure = BoardManager.cells[ty, tx].GetComponentInChildren<AbstractFigure>();
                if ((checkedFigure != null) && (this.FigureColor == checkedFigure.FigureColor)) break;
                //если в очередной ячейке содержится фигура и она своего цвета, прервать ветку 

                possibleAttacks.Add(new Vector2Int(tx, ty));

                if ((checkedFigure != null) && (this.FigureColor != checkedFigure.FigureColor)) break;
                //если фигура вражеская, её можно атаковать, но дальше по этой ветке не пройти
            }
            else break;//прервать цикл, когда мы вышли за пределы хотя бы одной из координат
        }

        for (int i = 1; i < 7; i++)//вверх
        {
            tx = coords.x ;
            ty = coords.y + i;
            if (ty <= 7)
            {
                AbstractFigure checkedFigure = BoardManager.cells[ty, tx].GetComponentInChildren<AbstractFigure>();
                if ((checkedFigure != null) && (this.FigureColor == checkedFigure.FigureColor)) break;
                //если в очередной ячейке содержится фигура и она своего цвета, прервать ветку 

                possibleAttacks.Add(new Vector2Int(tx, ty));

                if ((checkedFigure != null) && (this.FigureColor != checkedFigure.FigureColor)) break;
                //если фигура вражеская, её можно атаковать, но дальше по этой ветке не пройти
            }
            else break;//прервать цикл, когда мы вышли за пределы хотя бы одной из координат
        }

        return possibleAttacks;
    }
}

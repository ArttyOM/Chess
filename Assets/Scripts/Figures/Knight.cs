using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Knight : AbstractFigure
{

    public override List<Vector2Int> PossibilityToMove()
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();
        if ((coords.x + 2 <= 7)&&(coords.y+1<=7)) possibleMoves.Add(new Vector2Int(coords.x + 2, coords.y + 1));//x- буквы, у - цифры
        if ((coords.x + 2 <= 7) && (coords.y -1  >= 0)) possibleMoves.Add(new Vector2Int(coords.x + 2, coords.y - 1));
        if ((coords.x + 1 <= 7) && (coords.y + 2 <= 7)) possibleMoves.Add(new Vector2Int(coords.x + 1, coords.y + 2));
        if ((coords.x + 1 <= 7) && (coords.y - 2 >= 0)) possibleMoves.Add(new Vector2Int(coords.x + 1, coords.y - 2));

        if ((coords.x - 2 >= 0) && (coords.y + 1 <= 7)) possibleMoves.Add(new Vector2Int(coords.x - 2, coords.y + 1));//x- буквы, у - цифры
        if ((coords.x - 2 >= 0) && (coords.y - 1 >= 0)) possibleMoves.Add(new Vector2Int(coords.x - 2, coords.y - 1));
        if ((coords.x - 1 >= 0) && (coords.y + 2 <= 7)) possibleMoves.Add(new Vector2Int(coords.x - 1, coords.y + 2));
        if ((coords.x - 1 >= 0) && (coords.y - 2 >= 0)) possibleMoves.Add(new Vector2Int(coords.x - 1, coords.y - 2));

        return possibleMoves;
    }
    public override List<Vector2Int> PossibilityToAttack()
    {
        return PossibilityToMove(); //конь движется также, как и атакует
    }
}

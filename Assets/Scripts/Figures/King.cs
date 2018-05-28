using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class King : AbstractFigure
{
    public override void OnAttacked()
    {
        Debug.Log("The king cant be destroyed!");
    }
    //public override void Attack(GameObject hitObject)
    //{

    //}
    //public override void Move(Vector2Int coords)
    //{

    //}
    public override List<Vector2Int> PossibilityToMove()
    {
        List<Vector2Int> possibleMoves = PossibilityToAttack();
        //+возможность рокировки
        return possibleMoves;
    }
    public override List<Vector2Int> PossibilityToAttack()
    {
        List<Vector2Int> possibleAttacks = new List<Vector2Int>();

        for (int i = -1; i<=1; i++)
            for (int j=-1;j<=1;j++)
            {
                if ((i==0)&&(j==0)) continue;//собственная позиция
                if ((coords.x+j<=7)&&(coords.x+j>=0)&&(coords.y+i<=7)&&(coords.y+i>=0))
                    possibleAttacks.Add(new Vector2Int(coords.x + j, coords.y+i));
            }
        
        return possibleAttacks;
    }
}

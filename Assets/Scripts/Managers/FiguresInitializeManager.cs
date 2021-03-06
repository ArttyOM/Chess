﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
/// <summary>
/// Начальная расстановка фигур
/// </summary>
public class FiguresInitializeManager : MonoBehaviour , IManager{
    public BoardManager BoardMgr { get; set; }

    //можно было через ScriptableObject сделать
    //префабы белых
    [SerializeField]
    public GameObject whiteKing, whiteQueen, whiteBishop, whiteKnight, whiteRook, whitePawn;
    //префабы черных
    [SerializeField]
    public GameObject blackKing, blackQueen, blackBishop, blackKnight, blackRook, blackPawn;


    public ManagerStatus status { get; set; }
    public void Startup()
    {
        Debug.Log("FiguresInitialPositionManager starting...");
        //запустили менеджер, но нужно дождаться, пока BoardManager создаст сетку
        status = ManagerStatus.Initializing;
        StartCoroutine(WaitForBoardManagerStarted());

    }

    IEnumerator WaitForBoardManagerStarted()
    {
       // Debug.Log(BoardMgr.status);
        while (BoardMgr.status != ManagerStatus.Started) 
        {
            yield return null;
        }
        //как только менеджер доски создал ячейки, можно приступать к расстановке фигур
        status = ManagerStatus.Started;
        Debug.Log("FigureInitializeManager started!");

        SetFigures();


    }

    private void SetFigures()
    {
        //GameObject tempObj;
        //RectTransform ObjRectTransform;

        GameObject[] whiteFiguresLine = { whiteRook, whiteKnight, whiteBishop, whiteQueen,
            whiteKing, whiteBishop, whiteKnight, whiteRook };
        GameObject[] blackFiguresLine = { blackRook, blackKnight, blackBishop, blackQueen,
           blackKing, blackBishop, blackKnight, blackRook };


        for (int i = 0; i < 8; i++)
        {
            CreateAndSetFigure(whitePawn, new Vector2Int(i, 1), FigureColor.white); //2я строка
            CreateAndSetFigure(whiteFiguresLine[i], new Vector2Int(i, 0), FigureColor.white); //1я строка

            CreateAndSetFigure(blackPawn, new Vector2Int(i, 6), FigureColor.black);
            CreateAndSetFigure(blackFiguresLine[i], new Vector2Int(i, 7), FigureColor.black);
            //tempObj = Instantiate(whitePawn, BoardManager.cells[1, i].transform);
            //ObjRectTransform = tempObj.GetComponent<RectTransform>();
            //StartCoroutine(SetSize(ObjRectTransform));

            //parentRectTransform = BoardManager.cells[1, i].GetComponent<RectTransform>();
            //Debug.Log(BoardManager.cells[1, i].GetComponent<RectTransform>().rect);
            //Debug.Log(parentRectTransform.rect.width);

        }
        
        //for (int i =0; i<8; i++)
        //{
        //    CreateAndSetFigure(blackPawn, new Vector2Int(6, i));
        //    //tempObj = Instantiate(blackPawn, BoardManager.cells[6, i].transform);
        //    //ObjRectTransform = tempObj.GetComponent<RectTransform>();
        //    //StartCoroutine(SetSize(ObjRectTransform));
        //}
    }

    /// <summary>
    /// создает указанную фигуру и устанавливает в выбранную позицию
    /// </summary>
    /// <param name="figurePrefab">ссылка на шаблон фигуры</param>
    /// <param name="coords">позиция на доске, куда нужно устанавливать фигуру</param>
    public void CreateAndSetFigure(GameObject figurePrefab, Vector2Int coords, FigureColors.FigureColor owner)
    {
        GameObject tempObj;
        RectTransform ObjRectTransform;

        // B3 соответствует cells[2, B]
        tempObj = Instantiate(figurePrefab, BoardManager.cells[coords.y, coords.x].transform);
 
        tempObj.GetComponent<AbstractFigure>().coords = coords;//не забываем проставить координаты новой фигуры
        tempObj.GetComponent<AbstractFigure>().FigureColor = owner;
        ObjRectTransform = tempObj.GetComponent<RectTransform>();
        StartCoroutine(SetSize(ObjRectTransform));
        //GridLayout-у требуется время, чтобы получить корректный rect
    }

    IEnumerator SetSize(RectTransform rectTransform)
    {
        while (rectTransform != null)
        {
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
            //yield return null;
            yield return new WaitUntil(() => parentRectTransform.rect.width != 0);
            //TODO: убрать хардкод
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.7f * parentRectTransform.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.7f * parentRectTransform.rect.height);

            yield return new WaitForSeconds(GlobalFields.MyLazyBD.viewUpdatePeriod);
            //т.к. BoardManager адаптирует игровое поле под размер окна, размер фигур также должен адаптироваться
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiguresInitializeManager : MonoBehaviour , IManager{
    public BoardManager BoardMgr { get; set; }

    //можно было через ScriptableObject сделать
    //префабы белых
    [SerializeField]
    private GameObject whiteKing, whiteQueen, whiteBishop, whiteKnight, whiteRook, whitePawn;
    //префабы черных
    [SerializeField]
    private GameObject blackKing, blackQueen, blackBishop, blackKnight, blackRook, blackPawn;


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
        status = ManagerStatus.Started;
        Debug.Log("FigureInitializeManager started!");

        SetFigures();



        //как только менеджер доски создал ячейки, можно приступать к расстановке фигур

    }

    private void SetFigures()
    {
        GameObject tempObj;
        RectTransform ObjRectTransform;
        
        for (int i = 0; i < 8; i++)
        {
            tempObj = Instantiate(whitePawn, BoardManager.cells[1, i].transform);
            ObjRectTransform = tempObj.GetComponent<RectTransform>();
            //parentRectTransform = BoardManager.cells[1, i].GetComponent<RectTransform>();
            //Debug.Log(BoardManager.cells[1, i].GetComponent<RectTransform>().rect);
            //Debug.Log(parentRectTransform.rect.width);

            StartCoroutine(SetSize(ObjRectTransform));
            //GridLayout-у требуется время, чтобы получить корректный rect
        }
        for (int i =0; i<8; i++)
        {
            tempObj = Instantiate(blackPawn, BoardManager.cells[6, i].transform);
            ObjRectTransform = tempObj.GetComponent<RectTransform>();
            StartCoroutine(SetSize(ObjRectTransform));
        }
    }

    IEnumerator SetSize(RectTransform rectTransform)
    {
        RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        //yield return null;
        yield return new WaitUntil(() => parentRectTransform.rect.width != 0);
        //TODO: убрать хардкод
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.7f*parentRectTransform.rect.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.7f*parentRectTransform.rect.height);
    }
}

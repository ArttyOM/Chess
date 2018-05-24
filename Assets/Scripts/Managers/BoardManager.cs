using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class BoardManager : MonoBehaviour, IManager {
    enum LatVector {A,B,C,D,E,F,G,H}
    [SerializeField]
    private GameObject _whiteCell, _blackCell;
    public GameObject WhiteCell
    {
        get { return _whiteCell; }
        set
        {
            if (value != null) _whiteCell = value;
            else Debug.Log("Значение не было изменено");
        }
    }
    public GameObject BlackCell
    {
        get {return _blackCell;}
        set
        {
            if (value != null) _blackCell = value;
            else Debug.Log("Значение не было изменено");
        }
    }

    [SerializeField]
    private Transform _boardTransform;
    public Transform BoardTransform
    {
        get { return _boardTransform; }
        set {
            if (value != null)
            {
                if (value.GetComponent<GridLayout>() != null)
                    _boardTransform = value;
                else Debug.Log("Значение не было изменено: у объекта не найден компонент GridLayout");
            }
            else Debug.Log("Значение не было изменено");
        }
    }

    RectTransform boardRectTransform;

    void Awake ()
    {
        //Debug.Log(Screen.height);
        //шахматное поле - всегда квадратное

        UpdateBoardSize(Screen.height);

        //генерим поле
        //А1 (0,0) - черное поле, A2(1,0), B1(0,1) - белые поля
        //Т.е. сумма индексов четная - поле четное, в противном случае белое
        //GameObject tempObj;
        //for (int i = 0; i < 8; i++)
        //{
        //    for (LatVector j = 0; (int)j < 8; j++)
        //    {
        //        if ((i + (int)j) % 2 == 0) tempObj = Instantiate(_blackCell, _boardTransform);
        //        else tempObj = Instantiate(_whiteCell, _boardTransform);
        //        tempObj.name = (j).ToString() + (i + 1).ToString();
        //    }
        //}

    }

    void UpdateBoardSize(float size)
    {
        Vector2 boardSize = new Vector2(size, size);
        boardRectTransform = _boardTransform.GetComponent<RectTransform>();
        boardRectTransform.sizeDelta = boardSize;
        boardRectTransform.position = new Vector2(size / 2, size / 2);
    }

}

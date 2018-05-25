using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private RectTransform _boardRectTransform;
    private RectTransform _boardRectTransformPivot;

    public static GameObject[,] cells = new GameObject[8, 8];


    public ManagerStatus status { get; set; } //в идеале весь класс как синглтон оформить - доска в игре всегда одна
    public void Startup ()
    {
        status = ManagerStatus.Initializing;
        Debug.Log("BoardManager starting...");
        //Debug.Log(Screen.height);
        //шахматное поле - всегда квадратное

        _boardRectTransform= _boardTransform.GetComponent<RectTransform>();//TODO:null-check
        _boardRectTransformPivot = _boardTransform.parent.GetComponent<RectTransform>();

        

        //генерим поле
        //А1(0, 0) - черное поле, A2(1, 0), B1(0, 1) - белые поля
        //Т.е.сумма индексов четная - поле четное, в противном случае белое
        GameObject tempObj;
        for (int i = 0; i < 8; i++)
        {
            for (LatVector j = 0; (int)j < 8; j++)
            {
                if ((i + (int)j) % 2 == 0) tempObj = Instantiate(_blackCell, _boardTransform);
                else tempObj = Instantiate(_whiteCell, _boardTransform);
                tempObj.name = (j).ToString() + (i + 1).ToString();
                cells[i, (int)j] = tempObj;
            } 
        }

        OnBoardSizeUpdate();
        OnCellSizeUpdate();
        status = ManagerStatus.Started;
        Debug.Log("BoardManager started");
        StartCoroutine(UpdateBoardSize());
        //теперь доска готова к расстановке фигур


    }

    /// <summary>
    /// Обновляет размер поля, используя якоря (для адаптивной подстройки под разрешение экрана
    /// TODO: ограничить размеры
    /// </summary>
    /// <param name="size">сторона квадрата</param>
    IEnumerator UpdateBoardSize()
    {
        do
        {
            OnBoardSizeUpdate();
            OnCellSizeUpdate();
            //yield return null;
            yield return new WaitForSeconds(1f);
            //вызов SetBoardSize раз в секунду
            //для наглядности
        }
        while (true);
    }

    /// <summary>
    /// TODO убрать хардкод, додумать алгоритм определения spacing с учетом пустых пикселей в исходном спрайте
    /// </summary>
    void OnCellSizeUpdate()
    {
 
        GridLayoutGroup tempGridLayout = _boardRectTransform.GetComponent<GridLayoutGroup>();
        //размер ячейки - 1/9 от родительского объекта:
        //чуть меньше минимума, чтобы был spacing, 
        //1 /89 : вычисленный размер spacing-a
        //TODO вынести хардкод
        tempGridLayout.cellSize = new Vector2((_boardRectTransform.rect.width) / 8, (_boardRectTransform.rect.height) / 8);
        tempGridLayout.padding.left = (int)(10);
        tempGridLayout.padding.bottom = (int)(10);

        tempGridLayout.spacing = new Vector2(-3f, -3f);

        //Debug.Log("yo "+BoardManager.cells[1, 1].GetComponent<RectTransform>().rect);
    }


    /// <summary>
    /// приводим поле к квадратной форме
    /// используем родительский RectTransform,
    /// строим квадрат по его наименьшей стороне
    /// </summary>
    void OnBoardSizeUpdate()
    {
        float min = Mathf.Min(
    _boardRectTransformPivot.rect.height,
    _boardRectTransformPivot.rect.width
    );

        _boardRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, min);
        _boardRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, min);

        
        //    float w = Screen.width, h = Screen.height;
        //    Vector2 max = new Vector2();// величина относительно разрешения экрана 
        //    Vector2 min = _boardRectTransform.anchorMin;
        //    //Vector2 min;
        //    //if (boardRectTransform.anchorMin == null) min = new Vector2(0,0);
        //    //else min = new Vector2(boardRectTransform.anchorMin.x, boardRectTransform.anchorMin.y);// величина относительно разрешения экрана
        //    //будем устанавливать размер от левого нижнего угла, поэтому min не меняется
        //    //max.x*w - min.x*w = size 
        //    //max.y*h - min.y*h = size
        //    max.x = min.x + size / w;
        //    max.y = min.x + size / h;
        //    _boardRectTransform.anchorMax = max;

        //Debug.Log(new Vector2(_boardRectTransform.anchorMax.x*w, _boardRectTransform.anchorMax.y * h));
        //Debug.Log(new Vector2(w,h));

        //Теперь нужно проконтроллировать случай, когда объект доски является потомком относительно pivot-а
        //достаточно гарантировать, что у pivot-a max.x-min.x = max.y-min.y
        //if (_boardRectTransform.SetSizeWithCurrentAnchors)


    }
}

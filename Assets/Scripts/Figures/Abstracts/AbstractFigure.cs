using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public abstract class AbstractFigure : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected static bool isPawnUpgrading=false;
    //Transform FigureTransform { get; set; }
    public FigureColor FigureColor { get; set; }//у любой фигуры есть цвет
    public Vector2Int coords; // у любой фигуры есть координаты

    public abstract List<Vector2Int> PossibilityToMove();// результат проверки возможности ходьбы
    public abstract List<Vector2Int> PossibilityToAttack(); //результат проверки возможности атаки

    public virtual void OnAttacked()//все фигуры кроме короля могут быть атакованы
    {
        Debug.Log(gameObject.name + " was been pwned");
        Destroy(this.gameObject);
    }
    public virtual void Move(Vector2Int coords) //у пешек, короля и ладьи Move дополнительно помечает 1й ход фигуры
    {
        Debug.Log("Figure moved to " + coords);
        transform.SetParent(BoardManager.cells[coords.y, coords.x].transform, false);
        this.coords = coords;
    }
    public virtual void Move(GameObject hitObject)
    {
        if (hitObject == this.gameObject) return;//если кликнули сами на себя - ничего не происходит
        if (hitObject.transform.childCount == 0) return;
        //прервать функцию при поытке ходьбы на неподсвеченное поле TODO:учесть случай, когда в настройках можно отключать подстветку ходов
        if (hitObject.GetComponentInChildren<AbstractFigure>()!=null) return; 
        //прервать функцию при попытке ходьбы на другую фигуру - для атаки есть метод Attack
       
        transform.SetParent(hitObject.transform, false);
        this.coords = hitObject.GetComponent<CellCoords>().coords;//TODO null check

        //если движение состоялось, передать ход
        PlayerController.EndTurn();
    }

    public virtual void Attack(GameObject hitObject)
    {
        if (hitObject == this.gameObject) return;//если кликнули сами на себя - ничего не происходит

        if (hitObject.transform.childCount <= 1) return;//1 потомок у недоступных для атаки фигур и у клеток, доступных для Move

        if (hitObject.GetComponentInChildren<AbstractFigure>() == null) return;//прервать, если в потомках нет фигур

        hitObject.GetComponentInChildren<AbstractFigure>().OnAttacked();//вызовем у атакованной фигуры соответствующий метод

        transform.SetParent(hitObject.transform, false);
        this.coords = hitObject.GetComponent<CellCoords>().coords;//TODO null check

        //если атака состоялась, передать ход
        PlayerController.EndTurn();

    }//любая фигура умеет атаковать (но не для всех Move=Attack)

    public virtual void HighlightSpecialAttack()
    {

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            Attack(result.gameObject);
            Move(result.gameObject);
            // Debug.Log("Hit " + result.gameObject.name);
        }

        //if (possibilityToMove == null) Debug.Log("PossibilityToMove == null");
        //else Debug.Log(possibilityToMove[0]);

        //Move(possibilityToMove[0]);//TODO написать выбор

        Color thisImageColor = _thisImage.color;
        thisImageColor.a = 1f; //возвращаем прозрачность к нормальному состоянию
        _thisImage.color = thisImageColor;


        foreach (GameObject movementHighliter in movementHighliters)
        {
            Destroy(movementHighliter);
        }
        movementHighliters.Clear();

        foreach (GameObject attackHighliter in attackHighliters)
        {
            Destroy(attackHighliter);
        }
        movementHighliters.Clear();


        if (m_DraggingIcon != null)
            Destroy(m_DraggingIcon);
    }

    //любую фигура умеет перемещаться

    //любую фигуру можно выделать для перемещения
    public bool dragOnSurfaces = true;

    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    private Image _thisImage;

    void Awake()
    {
        _thisImage = this.GetComponent<Image>();
        
    }

    private List<Vector2Int> possibilityToMove = new List<Vector2Int>();//возможное перемещение фигуры без учета других фигур
    private List<Vector2Int> possibilityToAttack = new List<Vector2Int>();//возможна ли атака этой фигуры без учета других фигур
    private List<GameObject> movementHighliters = new List<GameObject>();//подсветка движений
    private List<GameObject> attackHighliters = new List<GameObject>();//подсветка возможных атак

    //инициация игроком движения фигуры
    public void OnBeginDrag(PointerEventData eventData)
    {

        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        //может ли игрок этого цвета перемещать фигуру?
        if (this.FigureColor != PlayerController.Turn) return;

        if (AbstractFigure.isPawnUpgrading) return;//в процессе выбора апгрейда пешки другой игрок не может ходить 

        //TODOнаходится ли король под шахом и может ли эта фигура его устранить?

        //TODOприведет ли перемещение фигуры к угрозе для короля?

        possibilityToMove.Clear();
        //Debug.Log(coords.x + " " + coords.y);
        //проверим, может ли фигура перемещаться или атаковать
        possibilityToMove= PossibilityToMove();
        

        foreach (Vector2Int t in possibilityToMove)
        {
            if (BoardManager.cells[t.y, t.x].transform.childCount != 0) continue;//двигаться можно только в случае, если поле не занято
             
            GameObject tObj = Instantiate(GlobalFields.MyLazyBD.movementHighliter, BoardManager.cells[t.y, t.x].transform);
            RectTransform rectTransform = tObj.GetComponent<RectTransform>();
            movementHighliters.Add(tObj);
            StartCoroutine(SetSize(rectTransform));
            //Debug.Log("Figure can move to " + t);
        }

        possibilityToAttack.Clear();
        possibilityToAttack = PossibilityToAttack();

        HighlightSpecialAttack();//создаем подсветку для спецатаки
        foreach (Vector2Int t1 in possibilityToAttack)
        {

            if (BoardManager.cells[t1.y, t1.x].transform.childCount == 0) continue;
            
            AbstractFigure checkedFigure = BoardManager.cells[t1.y, t1.x].GetComponentInChildren<AbstractFigure>();
            if (checkedFigure == null) continue;
            else if (checkedFigure.FigureColor == this.FigureColor) continue;
            //атаковать можно только в случае, если поле занято фигурой противника

            GameObject tObj = Instantiate(GlobalFields.MyLazyBD.attackHighliter, BoardManager.cells[t1.y, t1.x].transform);
            RectTransform rectTransform = tObj.GetComponent<RectTransform>();
            StartCoroutine(SetSize(rectTransform));

            attackHighliters.Add(tObj);
            //Debug.Log("Figure can move to " + t);
        }

        //убедимся, что наша фигура может либо двигаться, либо атаковать
        if ((movementHighliters.Count == 0)&&(attackHighliters.Count==0)) return;

        //склонируем объект, чтобы позволить его перенести и в случае чего моментально откатить
        m_DraggingIcon = Instantiate(this.gameObject);//new GameObject("icon");
        
        m_DraggingIcon.GetComponent<Image>().raycastTarget = false;
        Color thisImageColor = _thisImage.color;
        thisImageColor.a = GlobalFields.MyLazyBD.draggedFigureAlpha;
        _thisImage.color = thisImageColor; 

        m_DraggingIcon.transform.SetParent(canvas.transform, false);
        m_DraggingIcon.transform.SetAsLastSibling();

        //var image = m_DraggingIcon.AddComponent<Image>();
        //image.sprite = GetComponent<Image>().sprite;
        //image.SetNativeSize();

        if (dragOnSurfaces)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = canvas.transform as RectTransform;

        SetDraggedPosition(eventData);

    }

    public void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcon != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    void Start()
    {
        
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponentInParent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponentInParent<EventSystem>();
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    protected IEnumerator SetSize(RectTransform rectTransform)
    {
        while (rectTransform != null) 
        {
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
            //yield return null;
            //yield return new WaitUntil(() => parentRectTransform.rect.width != 0);
            //TODO: убрать хардкод
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  parentRectTransform.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentRectTransform.rect.height);



            yield return null;
            //yield return new WaitForSeconds(GlobalFields.MyLazyBD.viewUpdatePeriod);
        }
    }

}

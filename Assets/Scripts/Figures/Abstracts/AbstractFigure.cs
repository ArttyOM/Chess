using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public abstract class AbstractFigure : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    RectTransform FigureRectTransform { get; set; }
    FigureColor FigureColor { get; set; }//у любой фигуры есть цвет
    public Vector2Int coords; // у любой фигуры есть координаты
    public abstract void Move();//любую фигура умеет перемещаться
    public abstract void Attack();//любая фигура умеет атаковать (но не для всех Move=Attack)
    public abstract void OnAttacked();//все фигуры кроме короля могут быть атакованы
    public abstract List<Vector2Int> PossibilityToMove();// результат проверки возможности ходьбы
    public abstract List<Vector2Int> PossibilityToAttack(); //результат проверки возможности атаки
    //любую фигуру можно выделать для перемещения
    public bool dragOnSurfaces = true;

    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    private Image _thisImage;
    public void OnBeginDrag(PointerEventData eventData)
    {

        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;

        //Debug.Log(coords.x + " " + coords.y);
        //проверим, может ли фигура перемещаться или атаковать
        List<Vector2Int> possibilityToMove= PossibilityToMove();
        if (possibilityToMove == null) return;

        //отобразим на доске возможные перемещения

        //склонируем объект, чтобы позволить его перенести и в случае чего моментально откатить
        m_DraggingIcon = Instantiate(this.gameObject);//new GameObject("icon");
        _thisImage = this.GetComponent<Image>();
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

    public void OnEndDrag(PointerEventData eventData)
    {
        Color thisImageColor = _thisImage.color;
        thisImageColor.a = 1f; //возвращаем прозрачность к нормальному состоянию
        _thisImage.color = thisImageColor;

        //this.transform.SetParent();

        if (m_DraggingIcon != null)
            Destroy(m_DraggingIcon);
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

}

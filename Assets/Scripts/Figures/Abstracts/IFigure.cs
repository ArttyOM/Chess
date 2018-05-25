using FigureColors;
using UnityEngine;
public interface IFigure
{
    RectTransform FigureRectTransform { get; set; }
    FigureColor FigureColor { get; set; }//у любой фигуры есть цвет
    void Move();//любую фигура умеет перемещаться
    void Attack();//любая фигура умеет атаковать (но не для всех Move=Attack)
    void OnAttacked();//все фигуры кроме короля могут быть атакованы
    //
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MyLazyDB", menuName = "Create DB")]
[System.Serializable]
public class LazyBD : ScriptableObject
{

    [SerializeField]
    public GameObject movementHighliter;
    [SerializeField]
    public GameObject attackHighliter;

    [SerializeField]
    public GameObject whiteUpgrader;//набор кнопок для апгейда белой пешки
    [SerializeField]
    public GameObject blackUpgrader;//набор кнопок для апгрейда черной пешки

    [SerializeField]
    public float viewUpdatePeriod = 1f;//период изменения размеров поля
    [SerializeField]
    public float draggedFigureAlpha = 0.3f;


}



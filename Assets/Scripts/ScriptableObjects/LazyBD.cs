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
    public float viewUpdatePeriod = 1f;
    [SerializeField]
    public float draggedFigureAlpha = 0.3f;


}



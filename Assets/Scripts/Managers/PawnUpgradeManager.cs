using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;
public class PawnUpgradeManager : MonoBehaviour, IManager
{
    [SerializeField]
    private GameObject whiteUpgrader;
    [SerializeField]
    private GameObject blackUpgrader;

    private FiguresInitializeManager _figuresInitializeManager;


    private Animator _whiteAnimator, _blackAnimator;
    public ManagerStatus status { get; set;}
    public void Startup()
    {
        _whiteAnimator = whiteUpgrader.GetComponent<Animator>();
        _blackAnimator = blackUpgrader.GetComponent<Animator>();
        status = ManagerStatus.Started;

        
    }


    public void Awake()
    {
        _figuresInitializeManager = GetComponent<FiguresInitializeManager>();
        Messenger.AddListener("PawnUpgradeRequest", OnPawnUpgradeRequest);
    }
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Messenger.Broadcast("PawnUpgradeRequest");
    //    }
    //}
    private void OnDestroy()
    {
        Messenger.RemoveListener("PawnUpgradeRequest", OnPawnUpgradeRequest);
    }
    /// <summary>
    /// если поступил запрос на апгрейд пешки
    /// показываем игроку панель с фигурами соответствующего игроку цвета
    /// </summary>
    public void OnPawnUpgradeRequest()
    {

        if (PlayerController.Turn == FigureColor.white)
        {
            Debug.Log("сообщение получено, выкатываем белую панель");
            _whiteAnimator.SetBool("isPanelEnabled", true);
        }
        if (PlayerController.Turn == FigureColor.black)
        {
            Debug.Log("сообщение получено, выкатываем черную панель");
            _blackAnimator.SetBool("isPanelEnabled", true);
        }
    }

    public void OnFigureSelect(string key)
    {
        _whiteAnimator.SetBool("isPanelEnabled", false);
        _blackAnimator.SetBool("isPanelEnabled", false);
        Debug.Log("selected figure is " + key);

        GameObject tObj = new GameObject();
        switch (key)
        {
            case "WHITE_QUEEN": tObj = _figuresInitializeManager.whiteQueen; break;
            case "WHITE_ROOK": tObj = _figuresInitializeManager.whiteRook; break;
            case "WHITE_BISHOP": tObj = _figuresInitializeManager.whiteBishop; break;
            case "WHITE_KNIGHT": tObj = _figuresInitializeManager.whiteKnight; break;

            case "BLACK_QUEEN": tObj = _figuresInitializeManager.blackQueen; break;
            case "BLACK_ROOK": tObj = _figuresInitializeManager.blackRook; break;
            case "BLACK_BISHOP": tObj = _figuresInitializeManager.blackBishop; break;
            case "BLACK_KNIGHT": tObj = _figuresInitializeManager.blackKnight; break;
            default:break;
        }
        
        Messenger<GameObject>.Broadcast("PawnUpgradeResponse", tObj);
        //отправляем фигуру, которую нужно создать, и _figureInitializeManager, для предоставления доступа к методу setsize
    }
    //        if (whiteUpgrader == null)
    //        {
    //            whiteUpgrader = Instantiate(GlobalFields.MyLazyBD.whiteUpgrader, BoardManager.canvas.transform);
    //    //_whiteAnimator =// 
    //    whiteUpgader.ge
    //}
}

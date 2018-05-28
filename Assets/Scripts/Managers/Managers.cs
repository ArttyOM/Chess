using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FigureColors;

/// <summary>
/// Класс, запускающий остальные менеджеры
/// Также создает 2 экземпляра контроллера игроков 
/// (помещает внутрь собственной иерархии)
/// </summary>
[RequireComponent(typeof (BoardManager))]
[RequireComponent(typeof(FiguresInitializeManager))]
[RequireComponent(typeof(PawnUpgradeManager))]
public class Managers : MonoBehaviour {

	public static BoardManager BoardManagerObj { get; private set; }
    public static FiguresInitializeManager FiguresInitializeManagerObj { get; set; }
    
    public static PlayerController WhitePlayer { get; private set; }
    public static PlayerController BlackPlayer { get; private set; }

    public static PawnUpgradeManager UpgradeManger { get; private set; }

    private List<IManager> _startSequence;

	void Awake(){
        
        BoardManagerObj = GetComponent<BoardManager>();
        FiguresInitializeManagerObj = GetComponent<FiguresInitializeManager>();
        FiguresInitializeManagerObj.BoardMgr = BoardManagerObj;

        GameObject whitePlayerObj = new GameObject("WhitePlayer");
        whitePlayerObj.transform.SetParent(this.transform);
        WhitePlayer = PlayerController.CreateComponent(whitePlayerObj, FigureColor.white);

        GameObject blackPlayerObj = new GameObject("BlackPlayer");
        blackPlayerObj.transform.SetParent(this.transform);
        BlackPlayer = PlayerController.CreateComponent(blackPlayerObj, FigureColor.black);
        //создали в иерархии менеджеров 2 объекта с контроллерами игроков

        UpgradeManger = GetComponent<PawnUpgradeManager>();

        _startSequence = new List<IManager>();
		_startSequence.Add(BoardManagerObj);
        _startSequence.Add(FiguresInitializeManagerObj);
        _startSequence.Add(WhitePlayer);
        _startSequence.Add(BlackPlayer);
        _startSequence.Add(UpgradeManger);
		StartCoroutine(StartupManagers());
	}

	private IEnumerator StartupManagers(){
		foreach (IManager manager in _startSequence) {
			manager.Startup ();
		}
		yield return null; //остановка на один кадр

		int numModules = _startSequence.Count;
		int numReady = 0;

		while (numReady < numModules) {

			int lastReady = numReady;
			numReady = 0;

			foreach (IManager manager in _startSequence) {
				if (manager.status == ManagerStatus.Started) {
					numReady++;
				}
			}

			if (numReady > lastReady)
				Debug.Log ("Progress: " + numReady + "/" + numModules);
			yield return null; //остановка на один кадр перед следующей проверкой
		}
		Debug.Log ("All managers started up");
	}
}

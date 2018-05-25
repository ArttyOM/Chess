using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoardManager))]
[RequireComponent(typeof(FiguresInitializeManager))]

public class Managers : MonoBehaviour {

	public static BoardManager BoardManagerObj { get; private set; }
    public static FiguresInitializeManager FiguresInitializeManagerObj { get; set; } 

	private List<IManager> _startSequence;

	void Awake(){
        BoardManagerObj = GetComponent<BoardManager>();
        FiguresInitializeManagerObj = GetComponent<FiguresInitializeManager>();
        FiguresInitializeManagerObj.BoardMgr = BoardManagerObj;

		_startSequence = new List<IManager>();
		_startSequence.Add(BoardManagerObj);
        _startSequence.Add(FiguresInitializeManagerObj);
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

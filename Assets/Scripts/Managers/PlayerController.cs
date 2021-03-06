﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FigureColors;

[RequireComponent(typeof(Image))]
public class PlayerController : MonoBehaviour, IManager
{
    private static int _countOfPlayerControllers = 0;
    /// <summary>
    /// используется вместо стандартного AddComponent,
    /// чтобы при прикреплении указывать параметры
    /// </summary>
    /// <param name="where">GameObject, к которому подцепляем компонент</param>
    /// <param name="playerColor">Цвет игрока</param>
    /// <returns></returns>
    public static PlayerController CreateComponent(GameObject where, FigureColor playerColor)
    {
        //слушателя есть смысл добавлять только для первого контроллера игрока
        //if (_countOfPlayerControllers==0) Messenger.AddListener("PawnUpgradeRequest", OnPawnUpgradeRequest);

        PlayerController playerController = where.AddComponent<PlayerController>();
        playerController._playerColor = playerColor;

        _countOfPlayerControllers++;
        return playerController;

    }



    public static FigureColor Turn { get; private set; } //чей сейчас ход
    public static void EndTurn()
    {
        switch (Turn)
        {
            case FigureColor.white: Turn = FigureColor.black; break;
            case FigureColor.black: Turn = FigureColor.white; break;
            default: break;
        }

        Debug.Log("Сейчас ход " + Turn);

    }

    private Animator _whiteAnimator = new Animator(), _blacanimator = new Animator();
    private FigureColor _playerColor = FigureColor.notСonfigured;
    public FigureColor PlayerColor
    {
        get { return _playerColor; }
    }

    public ManagerStatus status { get; set; }
    public void Startup()
    {
        Turn = FigureColor.white;//по умолчанию первыми ходят белые
        Debug.Log(PlayerColor.ToString() + " player controller starting...");

        status = ManagerStatus.Started;
    }

}

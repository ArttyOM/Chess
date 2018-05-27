using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FigureColors;

[RequireComponent(typeof(Image))]
public class PlayerController : MonoBehaviour, IManager
{

    /// <summary>
    /// используется вместо стандартного AddComponent,
    /// чтобы при прикреплении указывать параметры
    /// </summary>
    /// <param name="where">GameObject, к которому подцепляем компонент</param>
    /// <param name="playerColor">Цвет игрока</param>
    /// <returns></returns>
    public static PlayerController CreateComponent(GameObject where, FigureColor playerColor)
    {
        PlayerController playerController = where.AddComponent<PlayerController>();
        playerController._playerColor = playerColor;
        return playerController;
    }

    public static FigureColor turn { get; private set; } //чей сейчас ход


    private FigureColor _playerColor = FigureColor.notСonfigured;
    public FigureColor PlayerColor
    {
      get { return _playerColor; }
    }
    
    public ManagerStatus status { get; set; }
    public void Startup()
    {
        turn = FigureColor.white;//по умолчанию первыми ходят белые
        Debug.Log(PlayerColor.ToString()+" player controller starting...");
        
        status = ManagerStatus.Started;
    }
    private void EndTurn()
    {
        switch (turn)
        {
            case FigureColor.white: turn = FigureColor.black; break;
            case FigureColor.black: turn = FigureColor.white; break;
            default: break;
        }
      
    }




}
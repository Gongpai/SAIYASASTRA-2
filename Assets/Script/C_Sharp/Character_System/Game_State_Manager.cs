using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_State_Manager
{

    private static Game_State_Manager _instance;

    public static Game_State_Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Game_State_Manager();
            }
            return _instance;
        }
    }

    public GameState currentGameState { get; private set; }

    public delegate void GameStateChangeHandler(GameState gamestate);
    public event GameStateChangeHandler OnGameStateChange;

    private Game_State_Manager()
    {

    }

    public void Setstate(GameState gamestate)
    {
        if (gamestate == currentGameState)
            return;

        currentGameState = gamestate;
        OnGameStateChange?.Invoke(gamestate);
    }
}

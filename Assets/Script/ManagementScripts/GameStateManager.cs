public enum GameStates { 
    Gameplay,
    Paused
}

public class GameStateManager
{
    private static GameStateManager instance;
    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameStateManager();

            return instance;
        }
    }

    public GameStates currentGameState { get; private set; }    

    public delegate void GameStateChangeHandler(GameStates newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    public void ChangeState(GameStates changedState)
    {
        if(changedState == currentGameState)
        {
            return;
        }
        
        currentGameState = changedState;
        OnGameStateChanged(currentGameState);
    }
}

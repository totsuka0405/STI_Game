using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance { get; private set; }

    public enum GameState { MainMenu, InGame, Pause, End }
    private GameState currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"Game State changed to: {currentState}");
        // 状態遷移に応じて他のManagerに通知
        switch (currentState)
        {
            case GameState.MainMenu:
                // UI初期化など
                break;
            case GameState.InGame:
                // CycleManager開始
                break;
            case GameState.Pause:
                // ポーズ処理
                break;
            case GameState.End:
                // エンディング処理
                break;
        }
    }
}

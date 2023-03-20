using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private LevelDescriptor[] levelDescriptors;
    [SerializeField] private Game game;
    [SerializeField] private GameObject gameOverScreen;
    

    private UpgradesLogic upgradesLogic;

    private void Start() {
        StartGame();
    }

    public void StartGame() {
        upgradesLogic = new UpgradesLogic();
        game.StartGame(levelDescriptors, upgradesLogic);
        
        game.OnGameFinished -= OnGameFinished;
        game.OnGameFinished += OnGameFinished;
    }

    private void OnGameFinished() {
        gameOverScreen.SetActive(true);
    }
}
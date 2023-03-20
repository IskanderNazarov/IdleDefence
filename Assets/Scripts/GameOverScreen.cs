using UnityEngine;

public class GameOverScreen : MonoBehaviour {

    public void Show() {
        gameObject.SetActive(true);
        //todo show some statistics
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
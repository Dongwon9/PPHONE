using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {
    public GameObject continueButton;

    private void Awake() {
        if (!GameSaveManager.Instance.FileExists) {
            continueButton.SetActive(false);
        }
    }

    public void StartNewGame() {
        GameSaveManager.Instance.NewGame();
        SceneManager.LoadScene("Scene_Stage");
    }

    public void LoadGame() {
        GameSaveManager.Instance.LoadGame();
        SceneManager.LoadScene("Scene_Stage");
    }
}
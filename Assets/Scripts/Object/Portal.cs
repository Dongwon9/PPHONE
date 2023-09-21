using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameSaveManager.Instance.SaveData.stageCount += 1;
            GameSaveManager.Instance.SaveGame();
            SceneManager.LoadScene("Scene_Stage");
        }
    }
}
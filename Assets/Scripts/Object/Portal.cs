using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {
    private new BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private void OnEnable() {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider.enabled = false;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
    }
    private void Update() {
        if(Mathf.Abs((transform.position - Player.Position).magnitude) >= 2) {
            collider.enabled =true;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GameSaveManager.Instance.SaveData.stageCount += 1;
            Player.Instance.HealHP((int)((Player.Instance.MaxHP - Player.Instance.HP) * 0.75f));
            GameSaveManager.Instance.SaveGame();
            SceneManager.LoadScene("Scene_Stage");
        }
    }
}
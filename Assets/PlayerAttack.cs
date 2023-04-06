using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    //TurnAction으로 충돌을 활성화한 후 3프레임 후 사라진다.
    private int frameCount = 0, life = 3;
    public int damage;
    void Update() {
        frameCount += 1;
        if (frameCount >= life) {
            Destroy(gameObject);
            frameCount = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.SendMessage("TakeDamage", damage);
        }
    }
}

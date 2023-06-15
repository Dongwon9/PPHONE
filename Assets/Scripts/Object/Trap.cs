using UnityEngine;

public class Trap : MonoBehaviour {
    [SerializeField] private int damage;
    /// <summary>이 함정은 일회용인가?</summary>
    [SerializeField] private bool oneUse;

    private void OnCollisionEnter2D(Collision2D collision) {
        Player damageTarget;
        bool success = collision.gameObject.TryGetComponent<Player>(out damageTarget);
        if (success) {
            damageTarget.TakeDamage(damage);
            if (oneUse) {
                Destroy(gameObject);
            }
        }
    }
}
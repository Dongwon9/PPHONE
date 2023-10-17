using UnityEngine;

public class Trap : MonoBehaviour {
    /// <summary>이 함정은 일회용인가?</summary>
    [SerializeField] private bool oneUse;
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            return;
        }
        collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        if (oneUse) {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class Trap : MonoBehaviour {
    [SerializeField] private int damage;
    /// <summary>이 함정은 일회용인가?</summary>
    [SerializeField] private bool oneUse;

    private void OnCollisionEnter2D(Collision2D collision) {
        //이것이 바로 인터페이스의 활용법!
        TurnActor.IDamagable damageTarget;
        bool success = collision.gameObject.TryGetComponent<TurnActor.IDamagable>(out damageTarget);
        if (success) {
            damageTarget.TakeDamage(damage);
            if (oneUse) {
                Destroy(gameObject);
            }
        }
    }
}
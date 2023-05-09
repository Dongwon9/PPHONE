using UnityEngine;

public class Trap : MonoBehaviour {
    [SerializeField] private int damage;
    /// <summary>�� ������ ��ȸ���ΰ�?</summary>
    [SerializeField] private bool oneUse;

    private void OnCollisionEnter2D(Collision2D collision) {
        //�̰��� �ٷ� �������̽��� Ȱ���!
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
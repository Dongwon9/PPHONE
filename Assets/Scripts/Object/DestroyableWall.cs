using UnityEngine;

public class DestroyableWall : MonoBehaviour, IDamagable {
    private int HP;
    public void TakeDamage(int damage) {
        HP -= 1;
        if (HP <= 0) {
            GameManager.Instance.WalkableGrid.SetWalkable(transform.position, true);
            Destroy(gameObject);
        }
    }
}
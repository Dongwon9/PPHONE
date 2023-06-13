using UnityEngine;

public class DestroyableWall : MonoBehaviour, TurnActor.IDamagable {
    private int HP;
    public void TakeDamage(int damage) {
        HP -= 1;
        if (HP <= 0) {
            GameManager.WalkableGrid.SetGridObject(transform.position, true);
            Destroy(gameObject);
        }
    }
}
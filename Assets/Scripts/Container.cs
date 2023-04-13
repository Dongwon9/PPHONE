using UnityEngine;

public abstract class Container : MonoBehaviour {
    protected Armor containingArmor;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent<Player>(out var player)) {
            player.ArmorEquip(containingArmor);
            Destroy(gameObject);
        }
    }
}
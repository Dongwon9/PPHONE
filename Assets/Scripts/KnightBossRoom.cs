using UnityEngine;

public class KnightBossRoom : MonoBehaviour {
    private Knight boss;
    private void OnEnable() {
        boss = GetComponentInParent<Knight>();
        transform.SetParent(null, true);
    }

    private void Update() {
        if (boss == null) {
            Destroy(gameObject);
        }
    }
}
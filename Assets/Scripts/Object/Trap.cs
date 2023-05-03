using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trap : MonoBehaviour {
    [SerializeField] private int damage;
    [SerializeField] private bool oneUse;

    // Start is called before the first frame update

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision) {
        bool success = collision.gameObject.TryGetComponent<TurnActor.IDamagable>(out var damageTarget);
        Debug.Log(damageTarget.ToString());
        if (success) {
            damageTarget.TakeDamage(damage);
            if (oneUse) {
                Destroy(gameObject);
            }
        }
    }
}
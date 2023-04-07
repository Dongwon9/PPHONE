using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour {
    public Attack AttackPrefab;
    public static IObjectPool<Attack> AttackPool;
    private void Awake() {
        AttackPool = new ObjectPool<Attack>(
        () => {
            Attack attack = Instantiate(AttackPrefab, transform).GetComponent<Attack>();
            attack.SetManagedPool(AttackPool);
            return attack;
        },
        (attack) => attack.gameObject.SetActive(true),
        (attack) => attack.gameObject.SetActive(false),
        (attack) => Destroy(attack.gameObject),
        maxSize: 100
        );
    }
}

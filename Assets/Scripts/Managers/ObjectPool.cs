using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour {
    public static IObjectPool<RedSquare> AttackPool;
    public RedSquare AttackPrefab;

    private void Awake() {
        AttackPool = new ObjectPool<RedSquare>(
        () => {
            RedSquare attack = Instantiate(AttackPrefab, transform).GetComponent<RedSquare>();
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
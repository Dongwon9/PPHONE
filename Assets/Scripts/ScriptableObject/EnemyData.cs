using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject {
    [SerializeField]
    private int maxHP;
    public int MaxHP { get { return maxHP; } }
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }
}
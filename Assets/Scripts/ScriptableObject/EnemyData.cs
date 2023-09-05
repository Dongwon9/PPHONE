using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject {
    [SerializeField]
    private int maxHP;
    public int MaxHP { get; }
    [SerializeField]
    private int damage;
    public int Damage { get; }
}
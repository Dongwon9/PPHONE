using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject {

    public enum GoldDropType { nogold, normal, boss }

    [SerializeField]
    private int maxHP;
    public int MaxHP { get { return maxHP; } }
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }
    [SerializeField]
    private GoldDropType dropType;
    public GoldDropType DropType { get { return dropType; } }
    public int GetGoldAmount() {
        switch (dropType) {
            case GoldDropType.normal:
                return 10;

            case GoldDropType.boss:
                return 50;

            default:
                return 0;
        }
    }
}
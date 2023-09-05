using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "새 소모아이템")]
public class Consumable : ScriptableObject {
    public new string name;
    public string description;
    public Sprite icon;
    public int HPRecovery;
}
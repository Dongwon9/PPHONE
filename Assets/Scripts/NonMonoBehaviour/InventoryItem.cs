using UnityEngine;

public class InventoryItem : ScriptableObject {
    [SerializeField] protected Sprite icon;
    [SerializeField] private new string name;
    [SerializeField] private string description;
}
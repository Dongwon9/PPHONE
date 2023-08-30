using UnityEngine;

public class Inventory : MonoBehaviour {
    public const int MAX_SLOTS = 5;
    public static Inventory Instance;
    [SerializeField]
    private Consumable[] Items = new Consumable[MAX_SLOTS];
    private bool IndexInRange(int index) {
        if (index >= 0 && index < Items.Length) {
            return true;
        } else {
            Debug.LogWarning(index + "는 인벤토리 범위를 벗어납니다.");
            return false;
        }
    }

    private void Awake() {
        Instance = this;
    }

    public Consumable GetItem(int index) {
        if (!IndexInRange(index)) {
            return null;
        }

        return Items[index];
    }

    public void AddItem(Consumable item) {
        int slot;
        for (slot = 0; slot < MAX_SLOTS; slot++) {
            if (Items[slot] == null) {
                break;
            }
            if (slot == MAX_SLOTS - 1) {
                return;
            }
        }

        Items[slot] = item;
    }

    public void UseItem(int index) {
        if (!IndexInRange(index)) {
            return;
        }
        Debug.Log(Items[index].name + "을 사용했다!");
        RemoveItem(index);
    }

    public void RemoveItem(int index) {
        if (!IndexInRange(index)) {
            return;
        }

        Items[index] = null;
    }
}
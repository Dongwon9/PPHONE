using UnityEngine;

public class Inventory : MonoBehaviour {
    public const int MAX_SLOTS = 5;
    public static Inventory Instance;
    [SerializeField]
    private Consumable[] Items = new Consumable[MAX_SLOTS];
    public Consumable[] Content { get { return Items; } }
    [SerializeField] private int gold;
    public int Gold { get { return gold; } private set { gold = value; } }
    private bool IndexInRange(int index) {
        if (index >= 0 && index < Items.Length) {
            return true;
        } else {
            Debug.LogWarning(index + "는 인벤토리 범위를 벗어납니다.");
            return false;
        }
    }

    public void ModifyGold(int amount) {
        Gold += amount;
        if (Gold < 0) {
            Gold = 0;
        }
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Items = GameSaveManager.Instance.SaveData.inventory;
    }

    public Consumable GetItem(int index) {
        if (!IndexInRange(index)) {
            return null;
        }

        return Items[index];
    }

    /// <returns>아이템이 성공적으로 추가되었는가?(bool)</returns>
    public bool AddItem(Consumable item) {
        int slot;
        for (slot = 0; slot < MAX_SLOTS; slot++) {
            if (Items[slot] == null) {
                //빈 아이템 슬롯을 찾았다.
                break;
            }
            if (slot == MAX_SLOTS - 1) {
                //인벤토리가 꽉 찼다.
                return false;
            }
        }

        Items[slot] = item;
        return true;
    }

    public void UseItem(int index) {
        if (!IndexInRange(index)) {
            return;
        }
        switch (Items[index].name) {
            case "회복포션":
                Player.Instance.HealHP(Items[index].HPRecovery);
                break;

            case "강화포션":
                Player.Instance.ActivateDoubleDamage(11);
                break;

            default:
                Debug.LogError(Items[index].name + "이라는 아이템은 없습니다.");
                RemoveItem(index);
                return;
        }

        //아이템을 사용하는 것도 1턴을 소모한다.
        Player.Instance.TakeInput(null);
        RemoveItem(index);
    }

    public void RemoveItem(int index) {
        if (!IndexInRange(index)) {
            return;
        }
        Items[index] = null;
    }
}
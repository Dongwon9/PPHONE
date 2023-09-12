﻿using UnityEngine;

public class Inventory : MonoBehaviour {
    public const int MAX_SLOTS = 5;
    public static Inventory Instance;
    [SerializeField]
    private Consumable[] Items = new Consumable[MAX_SLOTS];
    public Consumable[] Content { get { return Items; } }
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

    private void Start() {
        Items = GameSaveManager.Instance.SaveData.inventory;
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
        Player.Instance.HealHP(Items[index].HPRecovery);
        //아이템을 사용하는 것도 1턴을 소모한다.
        Player.Instance.TakeInput(null);
        RemoveItem(index);
    }

    public void RemoveItem(int index) {
        if (!IndexInRange(index)) {
            return;
        }
        Items[index] = null;
        InventoryUI.instance.UpdateItemSlots();
    }
}
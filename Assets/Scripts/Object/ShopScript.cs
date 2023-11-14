using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {
    [SerializeField]
    private Consumable[] shopList = new Consumable[5];
    [SerializeField]
    private Button[] buttons;
    public List<Consumable> allConsumables = new();
    private void Start() {
        for (int i = 0; i < shopList.Length; i++) {
            shopList[i] = allConsumables[Random.Range(0, allConsumables.Count)];
        }
    }

    private void Update() {
        for (int i = 0; i < buttons.Length; i++) {
            if (null != buttons[i]) {
                buttons[i].image.sprite = shopList[i].icon;
            }
        }
    }

    public Consumable GetShopItem(int index) {
        if (index >= shopList.Length) {
            Debug.LogError(index + " 는 상점 목록의 최대 인덱스인 " + shopList.Length + "를 초과합니다.");
            return null;
        }
        return shopList[index];
    }

    public void BuyItem(int index) {
        if (Inventory.Instance.Gold < 100) {
            return;
        }
        if (!Inventory.Instance.AddItem(shopList[index])) {
            return;
        }
        Inventory.Instance.ModifyGold(-100);
        Destroy(buttons[index].gameObject);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
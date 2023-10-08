using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {
    [SerializeField]
    private Consumable[] shopList = new Consumable[5];
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    public List<Consumable> allConsumables = new();
    private void Start() {
        for (int i = 0; i < shopList.Length; i++) {
            shopList[i] = allConsumables[Random.Range(0, allConsumables.Count)];
        }
        gameObject.SetActive(false);
    }

    private void Update() {
        for (int i = 0; i < buttons.Length; i++) {
            if (null != buttons[i]) {
                buttons[i].image.sprite = shopList[i].icon;
            }
        }
    }

    public void BuyItem(int index) {
        if (Inventory.Instance.Gold < 100) {
            return;
        }
        if (Inventory.Instance.AddItem(shopList[index])) {
            Inventory.Instance.ModifyGold(-100);
            Destroy(buttons[index].gameObject);
        } else {
            return;
        }
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
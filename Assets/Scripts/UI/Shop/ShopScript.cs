using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {
    [SerializeField]
    private Consumable[] shopList = new Consumable[5];
    [SerializeField]
    private Weapon[] weaponList = new Weapon[2];
    [SerializeField]
    private Button[] PotionButtons;
    [SerializeField]
    private Button[] WeaponButtons;
    public List<Consumable> allConsumables = new();
    private void Start() {
        for (int i = 0; i < shopList.Length; i++) {
            shopList[i] = allConsumables[Random.Range(0, allConsumables.Count)];
        }
    }

    private void Update() {
        for (int i = 0; i < PotionButtons.Length; i++) {
            if (null != PotionButtons[i]) {
                PotionButtons[i].image.sprite = shopList[i].icon;
            }
        }
        for (int i = 0; i < weaponList.Length; i++) {
            if (WeaponButtons[i]) {
                WeaponButtons[i].image.sprite = weaponList[i].icon;
            }
        }
    }

    public string GetShopItem(int index) {
        if (index < shopList.Length) {
            return shopList[index].name;
        }
        if (index < shopList.Length + weaponList.Length) {
            return weaponList[index - shopList.Length].name;
        }
        Debug.LogError("인덱스가 맞지 않습니다.");
        return null;
    }

    public void BuyItem(int index) {
        if (Inventory.Instance.Gold < 100) {
            return;
        }
        if (!Inventory.Instance.AddItem(shopList[index])) {
            return;
        }
        Inventory.Instance.ModifyGold(-100);
        Destroy(PotionButtons[index].gameObject);
    }

    public void BuyWeapon(int index) {
        if (Inventory.Instance.Gold < 250) {
            return;
        }
        Player.Instance.equippedWeapon = weaponList[index];
        Destroy(WeaponButtons[index].gameObject);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
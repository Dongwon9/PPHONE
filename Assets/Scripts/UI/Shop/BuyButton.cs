using TMPro;
using UnityEngine;

public class BuyButton : MonoBehaviour {
    public TextMeshProUGUI itemName;
    public ShopScript shopScript;
    public int itemIndex;
    private void Update() {
        itemName.SetText(shopScript.GetShopItem(itemIndex));
    }
}
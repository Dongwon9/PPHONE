using TMPro;
using UnityEngine;

public class ItemToolTip : MonoBehaviour {
    private int chosenSlot = -1;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    public void ChooseSlot(int slot) {
        if (chosenSlot == slot) { return; }
        chosenSlot = slot;
        itemName.SetText(Inventory.Instance.GetItem(slot).name);
        itemDescription.SetText(Inventory.Instance.GetItem(slot).description);
    }
    public void TextReset() {
        itemName.SetText("");
        itemDescription.SetText("");
        chosenSlot = -1;
        gameObject.SetActive(false);
    }
    public void TrashItem() {
        Inventory.Instance.RemoveItem(chosenSlot);
        InventoryUI.instance.UpdateItemSlots();
        gameObject.SetActive(false);
    }

    public void UseItem() {
        Inventory.Instance.UseItem(chosenSlot);
        InventoryUI.instance.UpdateItemSlots();
        gameObject.SetActive(false);
    }
}
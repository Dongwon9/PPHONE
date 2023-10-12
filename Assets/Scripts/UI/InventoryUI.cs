using UnityEngine;

public class InventoryUI : MonoBehaviour {
    [SerializeField] private ItemSlot[] slots = new ItemSlot[5];
    [SerializeField] private ItemToolTip tooltip;
    public const int MAX_SLOTS = 5;
    public static InventoryUI instance;
    private void Awake() {
        instance = this;
        tooltip.gameObject.SetActive(false);
    }

    public void Update() {
        for (int i = 0; i < MAX_SLOTS; i++) {
            if (Inventory.Instance.GetItem(i) != null) {
                slots[i].gameObject.SetActive(true);
                slots[i].slotButton.image.sprite = Inventory.Instance.GetItem(i).icon;
            } else {
                slots[i].slotButton.image.sprite = null;
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChooseSlot(int index) {
        tooltip.gameObject.SetActive(true);
        tooltip.ChooseSlot(index);
    }
}
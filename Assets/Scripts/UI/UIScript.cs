using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
    [SerializeField] private Slider HPBar, ShieldBar;
    [SerializeField] private TextMeshProUGUI HPText, ShieldText, GoldText;
    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] private ShopScript shop;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameClear;
    public static bool GameClear = false;
    private void Start() {
        GameClear = false;
    }
    private void Update() {
        HPBar.maxValue = Player.Instance.MaxHP;
        HPBar.value = Player.Instance.HP;
        ShieldBar.maxValue = Player.Instance.MaxShield;
        ShieldBar.value = Player.Instance.Shield;
        HPText.SetText(
            Player.Instance.HP.ToString() + "/" + Player.Instance.MaxHP.ToString());
        ShieldText.SetText(
            Player.Instance.Shield.ToString() + "/" + Player.Instance.MaxShield.ToString());
        GoldText.SetText(Inventory.Instance.Gold.ToString() + " Gold");
        if (!Player.Instance.GameOver && !GameClear) {
            buttonManager.gameObject.SetActive(!shop.gameObject.activeSelf);
            gameOver.SetActive(false);
            gameClear.SetActive(false);
        } else {
            buttonManager.gameObject.SetActive(false);
            shop.gameObject.SetActive(false);
            if (Player.Instance.GameOver) {
                gameOver.SetActive(true);
            }else { // if(gameClear)
                gameClear.SetActive(true);
            }
        }

    }
}
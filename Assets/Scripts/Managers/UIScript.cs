using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
    [SerializeField] private Slider HPBar, ShieldBar;
    [SerializeField] private TextMeshProUGUI HPText, ShieldText;
    private void Update() {
        HPBar.maxValue = GameManager.PlayerReference.MaxHP;
        HPBar.value = GameManager.PlayerReference.HP;
        ShieldBar.maxValue = GameManager.PlayerReference.MaxShield;
        ShieldBar.value = GameManager.PlayerReference.Shield;
        HPText.SetText(
            GameManager.PlayerReference.HP.ToString() + "/" + GameManager.PlayerReference.MaxHP.ToString());
        ShieldText.SetText(
            GameManager.PlayerReference.Shield.ToString() + "/" + GameManager.PlayerReference.MaxShield.ToString());
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
    [SerializeField] private Slider HPBar, ShieldBar;
    [SerializeField] private TextMeshProUGUI HPText, ShieldText;
    private void Update() {
        HPBar.maxValue = Player.Instance.MaxHP;
        HPBar.value = Player.Instance.HP;
        ShieldBar.maxValue = Player.Instance.MaxShield;
        ShieldBar.value = Player.Instance.Shield;
        HPText.SetText(
            Player.Instance.HP.ToString() + "/" + Player.Instance.MaxHP.ToString());
        ShieldText.SetText(
            Player.Instance.Shield.ToString() + "/" + Player.Instance.MaxShield.ToString());
    }
}
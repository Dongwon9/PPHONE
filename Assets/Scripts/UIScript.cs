using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour {
    public Player player;
    public TextMeshProUGUI HPText;

    private void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        HPText.text = "HP : " + player.HP + "/" + player.MaxHP + "\nShield : " + player.Shield + "" + "/" + player.MaxShield;
    }

    // Update is called once per frame
    private void Update() {
        HPText.text = "HP : " + player.HP + "/" + player.MaxHP + "\nShield : " + player.Shield + "" + "/" + player.MaxShield;
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour {
    public Player player;
    public TextMeshProUGUI HPText;
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        HPText.text = "HP : " + player.HP;
    }

    // Update is called once per frame
    void Update() {
        HPText.text = "HP : " + player.HP;
    }
}

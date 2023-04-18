using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    private int counter = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) {
            return;
        }
        counter += 1;
        UIManager.Instance.SetText(counter + "번째로 만나는군, "+collision.name);
        UIManager.Instance.SetUIActive(true);
    }
}

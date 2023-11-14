using UnityEngine;

public class ShopAreaScript : MonoBehaviour {
    public GameObject Shop;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Shop.SetActive(true);
        }
    }
}
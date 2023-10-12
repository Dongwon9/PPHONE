using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemScript : MonoBehaviour
{
    [SerializeField] private List<Consumable> items;
    private int randomPick = -1;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player")) {
            return;
        }
        if(randomPick == -1) {
            randomPick = Random.Range(0, items.Count+2);
        }
        if(randomPick == items.Count) {
            //대미지
            Player.Instance.TakeDamage(10);
            Destroy(gameObject);
        } else if(randomPick == items.Count + 1) {
            //골드
            Inventory.Instance.ModifyGold(120);
            Destroy(gameObject);
        }else {
            //아이템
            if (Inventory.Instance.AddItem(items[randomPick])) {
                Destroy(gameObject);
            }
        }
    }
}

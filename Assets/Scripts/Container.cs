using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : TurnActor {
    public Armor containingArmor;
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Player>().ArmorEquip(containingArmor);
            Destroy(gameObject);
        }
    }
}

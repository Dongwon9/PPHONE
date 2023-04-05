using System.Collections;
using UnityEngine;

public class BasicArmor : Armor {
    public override void OnEquip(Player player) {
        base.OnEquip(player);
        player.maxShield += 20;
    }
    public override void OnUnequip() {
        equippedPlayer.maxShield -= 20;
        base.OnUnequip();
    }
}

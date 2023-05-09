public class BasicArmorObject : Container {

    private class BasicArmor : Armor {
        public override void OnEquip(Player player) {
            base.OnEquip(player);
            player.AddMaxShield(20);
        }

        public override void OnUnequip() {
            equippedPlayer.AddMaxShield(-20);
            base.OnUnequip();
        }
    }

    private void OnEnable() {
        containingArmor = new BasicArmor();
    }
}
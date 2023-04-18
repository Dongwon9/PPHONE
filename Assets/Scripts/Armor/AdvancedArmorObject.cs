public class AdvancedArmorObject : Container {

    private class AdvancedArmor : Armor {

        public override void OnEquip(Player player) {
            base.OnEquip(player);
            player.AddMaxShield(40);
        }

        public override void OnUnequip() {
            equippedPlayer.AddMaxShield(-40);
            base.OnUnequip();
        }
    }

    private void OnEnable() {
        containingArmor = new AdvancedArmor();
    }
}
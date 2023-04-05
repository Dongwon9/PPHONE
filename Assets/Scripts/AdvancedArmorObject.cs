public class AdvancedArmorObject : Container {
    class AdvancedArmor : Armor {
        public override void OnEquip(Player player) {
            base.OnEquip(player);
            player.maxShield += 40;
        }
        public override void OnUnequip() {
            equippedPlayer.maxShield -= 40;
            base.OnUnequip();
        }
    }
    private void OnEnable() {
        containingArmor = new AdvancedArmor();
    }
}
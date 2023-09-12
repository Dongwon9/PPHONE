public class SaveData {
    public int HP;
    public int maxHP;
    public int shield;
    public int maxShield;
    public Consumable[] inventory = new Consumable[0];
    public int seed;
    public int stageCount;
    public SaveData() {
    }

    public void FillData() {
        maxHP = Player.Instance.MaxHP;
        HP = Player.Instance.HP;
        maxShield = Player.Instance.MaxShield;
        shield = Player.Instance.Shield;
        seed = GameManager.Instance.Seed;
        inventory = Inventory.Instance.Content;
    }
}
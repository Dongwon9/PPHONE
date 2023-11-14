public abstract class Armor {
    protected Player equippedPlayer;

    /// <summary>장착시 효과(최대SP증가 포함)</summary>
    public virtual void OnEquip(Player player) {
        equippedPlayer = player;
    }

    /// <summary>플레이어 피격시 효과</summary>
    public virtual void OnHit() { }

    /// <summary>매 턴마다 효과</summary>
    public virtual void OnTurnUpdate() { }

    /// <summary>장착 해제시 효과(최대 SP감소 포함)</summary>
    public virtual void OnUnequip() {
        equippedPlayer = null;
    }
}
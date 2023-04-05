using UnityEngine;
public abstract class Armor {
    protected Player equippedPlayer;
    //장착시 효과(최대SP증가 포함)
    public virtual void OnEquip(Player player) {
        equippedPlayer = player;
    }
    //장착 해제시 효과(최대 SP감소 포함)
    public virtual void OnUnequip() {
        equippedPlayer = null;
    }
    //플레이어 피격시 효과
    public virtual void OnHit() { }
    //매 턴마다 효과
    public virtual void OnTurnUpdate() { }
}
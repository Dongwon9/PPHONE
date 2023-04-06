using System;
using UnityEngine;
public class Player : TurnActor {
    public static event Action OnTurnUpdate;
    public Armor equippedArmor = null;
    public PlayerAttack playerAttack;
    public int HP, Shield, maxHP, maxShield;
    private int moveCount = 0;
    private Direction facing = Direction.Right;
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            nextAction = () => Move(Direction.Left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            nextAction = () => Move(Direction.Right);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            nextAction = () => Move(Direction.Down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            nextAction = () => Move(Direction.Up);
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            nextAction = () => {
                PlayerAttack atk = Instantiate(playerAttack, transform.position + Vector3.right, Quaternion.identity);
                atk.damage = 100;
            };
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            nextAction = () => { return; };
        }
        if (nextAction != null) {
            OnTurnUpdate();
        }
    }
    new void Move(Direction dir) {
        base.Move(dir);
        facing = dir;
    }
    protected override void TurnUpdate() {
        base.TurnUpdate();
        moveCount += 1;
        if (moveCount == 3) {
            moveCount = 0;
            HP -= 1;
        }
        if (equippedArmor != null) {
            equippedArmor.OnTurnUpdate();
        }
    }
    private void TakeDamage(int damage, bool piercing = false) {
        HP -= damage;
        Debug.Log(ToString() + "Takes damage!");
        equippedArmor.OnHit();
    }
    public void ArmorEquip(Armor armor) {
        if (equippedArmor != null) {
            equippedArmor.OnUnequip();
            //그 자리에서 움직이면, EquippedArmor(였던 것)를 가진 Container가
            //마지막에 있던 자리에 생성된다.
        }
        equippedArmor = armor;
        equippedArmor.OnEquip(this);
    }
}

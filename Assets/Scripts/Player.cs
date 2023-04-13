using System;
using UnityEngine;

/// <summary>
/// Player클래스가 턴의 흐름을 제어한다.
/// </summary>
public class Player : TurnActor {

    public static event Action OnTurnUpdate;

    public Armor equippedArmor = null;
    public int HP, Shield, maxHP, maxShield;
    private int moveCount = 0;
    private Direction facing = Direction.Right;
    private Animator animator;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            nextAction = () => Move(Direction.Left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            nextAction = () => Move(Direction.Right);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            nextAction = () => Move(Direction.Down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            nextAction = () => Move(Direction.Up);
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            nextAction = () => PlayerAttack(1);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            nextAction = () => { };
        }

        if (nextAction != null && timeCounter < 0) {
            OnTurnUpdate();
        }
    }

    private new void Move(Direction dir) {
        base.Move(dir);
        facing = dir;
    }

    private void PlayerAttack(int damage) {
        Vector3 direction = Vector3.zero;
        switch (facing) {
            case Direction.Left:
                direction = Vector3.left;
                break;

            case Direction.Right:
                direction = Vector3.right;
                break;

            case Direction.Up:
                direction = Vector3.up;
                break;

            case Direction.Down:
                direction = Vector3.down;
                break;
        }
        AttackPreTurn(transform.position + direction, damage);
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

    public void TakeDamage(int damage, Action onHitEffect = null) {
        if (Shield < damage) {
            HP -= damage - Shield;
            Shield = 0;
        }
        equippedArmor?.OnHit();
        animator.SetTrigger("isHit");
        onHitEffect?.Invoke();
    }

    public void ArmorEquip(Armor armor) {
        if (equippedArmor != null) {
            equippedArmor.OnUnequip();
            //TODO:그 자리에서 움직이면, EquippedArmor(였던 것)를 가진 Container가
            //마지막에 있던 자리에 생성된다.
        }
        equippedArmor = armor;
        equippedArmor.OnEquip(this);
    }

    protected override void DecideNextAction() {
    }
}
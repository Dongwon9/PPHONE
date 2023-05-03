using System;
using UnityEngine;

/// <summary>
/// Player클래스가 턴의 흐름을 제어한다.
/// </summary>
public class Player : TurnActor, TurnActor.IDamagable {
    public Armor equippedArmor = null;
    private Animator animator;
    private Direction facing = Direction.Right;
    [SerializeField] private int hp;
    private int moveCount = 0;

    public static event Action OnTurnUpdate;

    public int HP { get { return hp; } private set { hp = value; } }
    public int MaxHP { get; private set; }
    public int MaxShield { get; private set; }
    public int Shield { get; private set; }

    public void AddMaxHP(int value) {
        MaxHP += value;
    }

    /// <summary>
    /// 최대 쉴드 감소는 여기에 음수를 넣어서 표현한다.("음수를 더한다")
    /// </summary>
    public void AddMaxShield(int value) {
        MaxShield += value;
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

    public void TakeDamage(int damage) {
        if (Shield < damage) {
            HP -= damage - Shield;
            Shield = 0;
        } else {
            Shield -= damage;
        }
        equippedArmor?.OnHit();
        animator.SetTrigger("isHit");
    }

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        TurnReady = true;
    }

    protected override void DecideNextAction() {
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

    private new void Move(Direction dir) {
        base.Move(dir);
        facing = dir;
        animator.SetTrigger("isWalk");
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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            nextAction = () => {
                Move(Direction.Left);
            };
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            nextAction = () => {
                Move(Direction.Right);
            };
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            nextAction = () => Move(Direction.Down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            nextAction = () => Move(Direction.Up);
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            nextAction = () => PlayerAttack(1);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            nextAction = () => { };
        }

        if (nextAction != null && TurnReady) {
            if (UIManager.Instance.UIActive) {
                UIManager.Instance.SetUIActive(false);
                nextAction = null;
            } else {
                OnTurnUpdate();
            }
        }
    }
}
using System;
using UnityEngine;

/// <summary>
/// PlayerŬ������ ���� �帧�� �����Ѵ�.
/// </summary>
public class Player : TurnActor {
    public int HP { get; private set; }
    public int MaxHP { get; private set; }
    public int MaxShield { get; private set; }
    public int Shield { get; private set; }
    public Armor equippedArmor = null;
    private Animator animator;
    private Direction facing = Direction.Right;
    private int moveCount = 0;

    public static event Action OnTurnUpdate;

    public void AddMaxHP(int value) {
        MaxHP += value;
    }

    /// <summary>
    /// �ִ� ���� ���Ҵ� ���⿡ ������ �־ ǥ���Ѵ�.("������ ���Ѵ�")
    /// </summary>
    public void AddMaxShield(int value) {
        MaxShield += value;
    }

    public void ArmorEquip(Armor armor) {
        if (equippedArmor != null) {
            equippedArmor.OnUnequip();
            //TODO:�� �ڸ����� �����̸�, EquippedArmor(���� ��)�� ���� Container��
            //�������� �ִ� �ڸ��� �����ȴ�.
        }
        equippedArmor = armor;
        equippedArmor.OnEquip(this);
    }

    public void TakeDamage(int damage, Action onHitEffect = null) {
        if (Shield < damage) {
            HP -= damage - Shield;
            Shield = 0;
        } else {
            Shield -= damage;
        }
        equippedArmor?.OnHit();
        animator.SetTrigger("isHit");
        onHitEffect?.Invoke();
    }

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
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
}
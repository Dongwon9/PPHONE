using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerŬ������ ���� �帧�� �����Ѵ�.
/// </summary>
public class Player : MovingTurnActor, TurnActor.IDamagable {
    public static Player Instance;
    public Armor equippedArmor = null;
    private readonly List<PartComponents> playerPartComponents = new();
    private Animator animator;
    private int defaultAttackDamage = 1;
    private Direction facing = Direction.Right;
    [SerializeField]
    private int hp = 100, maxHP = 100, shield = 20, maxShield = 20;
    private int moveCount = 0;
    [SerializeField]
    private List<GameObject> playerParts;
    public int HP => hp;
    public int MaxHP => maxHP;
    public int MaxShield => maxShield;
    public int Shield => shield;

    /// <summary>
    /// ��� TurnActor���� �� �̺�Ʈ�� TurnUpdate()�� ������Ű��,
    /// �÷��̾� �ൿ�� �������� �� �̺�Ʈ�� ȣ���� �� �̺�Ʈ�� ������
    /// ������ TurnActor���� TurnUpdate()�� ����ȴ�.
    /// </summary>
    public static event Action OnTurnUpdate;

    /// <summary>
    /// �÷��̾��� ������ �ʿ��� ������Ʈ�� ��� Ŭ����
    /// GetComponent�� �ִ��� ���� ������ �������.
    /// </summary>
    private class PartComponents {
        public Animator animator;
        public SpriteRenderer sprite;
        public Transform transform;
        public PartComponents(Animator animator, SpriteRenderer sprite, Transform transform) {
            this.animator = animator != null ? animator : throw new ArgumentNullException(nameof(animator));
            this.sprite = sprite != null ? sprite : throw new ArgumentNullException(nameof(sprite));
            this.transform = transform;
        }
    }

    public void AddMaxHP(int value) {
        maxHP += value;
    }

    /// <summary>
    /// �ִ� ���� ���Ҵ� ���⿡ ������ �־ ǥ���Ѵ�.("������ ���Ѵ�")
    /// </summary>
    public void AddMaxShield(int value) {
        maxShield += value;
    }

    public void ArmorEquip(Armor armor) {
        if (equippedArmor != null) {
            equippedArmor.OnUnequip();
        }
        equippedArmor = armor;
        equippedArmor.OnEquip(this);
    }

    public void TakeDamage(int damage) {
        if (shield < damage) {
            hp -= damage - shield;
            shield = 0;
        } else {
            shield -= damage;
        }
        equippedArmor?.OnHit();
        animator.SetTrigger("isHit");
    }

    /// <summary>
    /// �Է��� ���⿡�� �޴´�.
    /// </summary>
    public void TakeInput(Direction? inputDir) {
        if (inputDir == null) {
            nextAction = () => { };
            return;
        }
        if (ButtonManager.Instance.AttackMode) {
            nextAction = () => PlayerAttack((Direction)inputDir, defaultAttackDamage);
        } else {
            nextAction = () => Move((Direction)inputDir);
        }
    }

    protected override void Awake() {
        base.Awake();
        Instance = this;
        animator = GetComponent<Animator>();
        foreach (GameObject obj in playerParts) {
            playerPartComponents.Add(
                new PartComponents(obj.GetComponent<Animator>(), obj.GetComponent<SpriteRenderer>(), obj.transform));
        }
        TurnReady = true;
    }

    protected override void DecideNextAction() {
    }

    protected override void FlipSprite(bool toRight) {
        //���� �ڽ��� �����´�
        base.FlipSprite(toRight);
        //��������� �������°�?
        float sign = toRight ? 1.0f : -1.0f;
        //�ڽ��� ��� ������ �÷��̾�� ���� �������� �����´�
        foreach (var part in playerPartComponents) {
            part.sprite.flipX = !toRight;
            part.transform.localPosition = new Vector3(MathF.Abs(part.transform.localPosition.x) * sign, part.transform.localPosition.y);
        }
    }

    protected override void TurnUpdate() {
        base.TurnUpdate();
        moveCount += 1;
        if (moveCount == 3) {
            moveCount = 0;
            hp -= 1;
        }
        equippedArmor?.OnTurnUpdate();
    }

    private new void Move(Direction dir) {
        base.Move(dir);
        facing = dir;
        animator.SetTrigger("isWalk");
    }

    private void PlayerAttack(Direction dir, int damage) {
        Vector3 direction = Vector3.zero;
        switch (dir) {
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

        foreach (var obj in playerPartComponents) {
            obj.animator.SetTrigger("Attack");
        }
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
            nextAction = () => PlayerAttack(facing, 1);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            nextAction = () => { };
        }

        if (nextAction != null && TurnReady) {
            //if (UIManager.Instance.UIActive) {
            //    UIManager.Instance.SetUIActive(false);
            //    nextAction = null;
            //} else {
            OnTurnUpdate();
            //}
        }
    }
}
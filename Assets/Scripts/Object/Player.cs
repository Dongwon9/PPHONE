using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player클래스가 턴의 흐름을 제어한다.
/// </summary>
public class Player : TurnActor, TurnActor.IDamagable {
    public Armor equippedArmor = null;
    private readonly List<PartComponents> playerPartComponents = new();
    private Animator animator;
    private Direction facing = Direction.Right;
    [SerializeField]
    private int hp = 100, maxHp = 100, shield = 20, maxShield = 20;
    private int moveCount = 0;
    [SerializeField]
    private List<GameObject> playerParts;
    public int HP { get { return hp; } private set { hp = value; } }
    public int MaxHP { get { return maxHp; } private set { maxHp = value; } }
    public int MaxShield { get { return maxShield; } private set { maxShield = value; } }
    public int Shield { get { return shield; } private set { shield = value; } }

    /// <summary>
    /// 모든 TurnActor들이 이 이벤트에 TurnUpdate()를 구독시키고,
    /// 플레이어 행동이 정해지면 이 이벤트를 호출해 이 이벤트에 구독된
    /// 수많은 TurnActor들의 TurnUpdate()가 실행된다.
    /// </summary>
    public static event Action OnTurnUpdate;

    private class PartComponents {
        public Animator animator;
        public SpriteRenderer sprite;
        public Transform transform;
        public PartComponents(Animator animator, SpriteRenderer sprite, Transform transform) {
            this.animator = animator ?? throw new ArgumentNullException(nameof(animator));
            this.sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
            this.transform = transform;
        }
    }

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
        foreach (GameObject obj in playerParts) {
            playerPartComponents.Add(
                new PartComponents(obj.GetComponent<Animator>(), obj.GetComponent<SpriteRenderer>(), obj.transform));
        }
        TurnReady = true;
    }

    protected override void DecideNextAction() {
    }

    protected override void FlipSprite(bool toRight) {
        base.FlipSprite(toRight);
        float sign;
        if (toRight) {
            sign = 1.0f;
        } else {
            sign = -1.0f;
        }

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
            nextAction = () => PlayerAttack(1);
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
        //메인카메라가 플레이어의 자식이 아니기 때문에
        //이렇게 따라오게 한다.
    }
}
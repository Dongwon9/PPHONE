using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Player클래스가 턴의 흐름을 제어한다.</summary>
public class Player : MovingTurnActor, IDamagable {
    private const float healthDrainDelay = 2f;
    private readonly List<PartComponents> playerPartComponents = new();
    private Animator animator;
    [SerializeField]
    private int hp, maxHP, shield, maxShield;
    private int moveCount = 0;
    [SerializeField]
    private List<GameObject> playerParts;
    private int doubleDamageTurnLeft = 0;
    private Action nextAction;
    private int ShieldHealCoolTime = 0;
    private float healthDrainTimer = 0;
    public static Player Instance;
    public Armor equippedArmor = null;
    public Weapon equippedWeapon;
    public static Vector3 Position => Instance.transform.position;
    public int HP => hp;
    public int MaxHP => maxHP;
    public int MaxShield => maxShield;
    public int Shield => shield;
    public bool GameOver => hp <= 0;
    /// <summary>
    /// 모든 TurnActor들이 이 이벤트에 TurnUpdate()를 구독시키고,<br></br>
    /// 플레이어 행동이 정해지면 이 이벤트를 호출해 이 이벤트에 구독된
    /// 수많은 TurnActor들의 TurnUpdate()가 실행된다.
    /// </summary>
    public static event Action OnTurnUpdate;

    /// <summary>
    /// 플레이어의 파츠의 필요한 컴포넌트만 담는 클래스<br></br>
    /// GetComponent를 최대한 적게 쓰려고 만들었다.
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

    private void Start() {
        SaveData data = GameSaveManager.Instance.SaveData;
        hp = data.HP;
        maxHP = data.maxHP;
        shield = data.shield;
        maxShield = data.maxShield;
        healthDrainTimer = healthDrainDelay;
    }

    private void Update() {
        if (nextAction != null && !TurnProcessing) {
            //무조건 플레이어가 먼저 행동한다.
            TurnUpdate();
            StartCoroutine(Timer());
            //그 후에 다른 모든 TurnActor들이 행동한다.
            Invoke(nameof(ProcessTurn), MovingTurnActor.movingTime * 1f);
        }
        if (GameOver) {
            Camera mainCamera = Camera.main;
            mainCamera.transform.SetParent(null, true);
            gameObject.SetActive(false);
        }
        healthDrainTimer -= Time.deltaTime;
        //2초가 지나면 체력 감소
        if (healthDrainTimer < 0) {
            hp -= 1;
            healthDrainTimer += healthDrainDelay;
        }
    }

    private IEnumerator Timer() {
        TurnProcessing = true;
        float turnDelay = movingTime * 1f;
        while (turnDelay > 0) {
            yield return new WaitForSeconds(0.1f);
            turnDelay -= 0.1f;
        }
        TurnProcessing = false;
        turnDelay = 0;
    }

    private new void Move(Direction dir) {
        base.Move(dir);
        animator.SetTrigger("isWalk");
    }

    private int GetFinalDamage() {
        return doubleDamageTurnLeft > 0 ? equippedWeapon.damage * 2 : equippedWeapon.damage;
    }

    private void PlayerAttack(Direction dir) {
        foreach (Vector2 offset in equippedWeapon.GetAttackSquare(dir)) {
            Attack(transform.position + (Vector3)offset, GetFinalDamage(), Target.Any);
            CreateSlash(transform.position + (Vector3)offset);
        }
        foreach (PartComponents obj in playerPartComponents) {
            obj.animator.SetTrigger("Attack");
        }
    }

    private void ProcessTurn() {
        OnTurnUpdate?.Invoke();
    }

    protected override void Awake() {
        base.Awake();
        Instance = this;
        animator = GetComponent<Animator>();
        foreach (GameObject obj in playerParts) {
            playerPartComponents.Add(
                new PartComponents(obj.GetComponent<Animator>(), obj.GetComponent<SpriteRenderer>(), obj.transform));
        }
        StartsFacingRight = true;
    }

    //player는 모든 TurnActor보다 먼저 턴이 진행되기 때문에,
    //OnTurnUpdate에 메소드를 구독하지 않는다.
    protected override void OnDisable() {
    }

    protected override void OnEnable() {
    }

    protected override void TurnUpdate() {
        nextAction();
        nextAction = null;
        healthDrainTimer = healthDrainDelay;
        moveCount += 1;

        if (moveCount == 3) {
            moveCount = 0;
            hp -= 1;
        }
        if (doubleDamageTurnLeft > 0) {
            doubleDamageTurnLeft -= 1;
        }
        if (ShieldHealCoolTime == 0) {
            shield += 5;
            if (shield > maxShield) {
                shield = maxShield;
            }
        } else {
            ShieldHealCoolTime -= 1;
        }
        equippedArmor?.OnTurnUpdate();
    }

    /// <summary>자신 스프라이트를 좌우로 뒤집는다.</summary>
    /// <param name="toRight">true면 오른쪽, false면 왼쪽을 보게 된다.</param>
    protected override void FlipSprite(bool toRight) {
        //먼저 자신을 뒤집는다
        base.FlipSprite(toRight);
        //자신의 모든 파츠를 플레이어와 같은 방향으로 뒤집는다
        float sign = !toRight ? 1.0f : -1.0f;
        foreach (PartComponents part in playerPartComponents) {
            part.sprite.flipX = spriteRenderer.flipX;
            part.transform.localPosition = new Vector3(MathF.Abs(part.transform.localPosition.x) * sign, part.transform.localPosition.y);
        }
    }

    public void ActivateDoubleDamage(int turnCount) {
        doubleDamageTurnLeft += turnCount;
    }

    public void AddMaxHP(int value) {
        maxHP += value;
    }

    /// <summary>
    /// 최대 쉴드 감소는 여기에 음수를 넣어서 표현한다.("음수를 더한다")
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

    public void HealHP(int value) {
        if (value < 0) {
            Debug.LogWarning("체력회복량이 음수입니다.");
            return;
        }
        if (hp + value > maxHP) {
            hp = maxHP;
        } else {
            hp += value;
        }
        DamageNumberManager.instance.DisplayDamageNumber(value, transform.position + Vector3.up, true);
    }

    public void HideOrShowArm(int showArm) {
        foreach (PartComponents part in playerPartComponents) {
            part.sprite.enabled = (showArm == 1);
        }
    }

    public void TakeDamage(int damage) {
        if (shield < damage) {
            hp -= damage - shield;
            shield = 0;
        } else {
            shield -= damage;
        }
        ShieldHealCoolTime = 7;
        equippedArmor?.OnHit();
        animator.SetTrigger("isHit");
        DamageNumberManager.instance.DisplayDamageNumber(damage, transform.position + Vector3.up);
    }

    /// <summary>입력은 여기에서 받는다. </summary>
    public void TakeInput(Direction? inputDir) {
        if (inputDir == null) {
            nextAction = () => { };
            return;
        }
        if (ButtonManager.Instance.AttackMode) {
            nextAction = () => PlayerAttack((Direction)inputDir);
        } else {
            nextAction = () => Move((Direction)inputDir);
        }
    }
}
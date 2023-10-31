using UnityEngine;

/// <summary>모든 적들이 상속할 abstract 클래스</summary>
public abstract class Enemy : MovingTurnActor, IDamagable {
    protected Animator animator;
    [SerializeField] protected int HP;
    [SerializeField] protected EnemyData enemydata;
    //[SerializeField] private SpriteRenderer NextActonSprite;
    protected override void Awake() {
        base.Awake();
        StartsFacingRight = false;
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        HP = (int)(enemydata.MaxHP * GameManager.Instance.GetStageHPModifier());
    }

    /// <summary>
    /// 스턴당했을 때 내부 변수 등을 조절하고 싶으면 사용한다.
    /// </summary>
    protected virtual void TurnUpdateOnStun() {
    }

    public virtual void TakeDamage(int damage) {
        HP -= damage;
        animator.SetTrigger("isHit");
        if (HP <= 0) {
            Destroy(gameObject);
        }
        DamageNumberManager.instance.DisplayDamageNumber(damage, transform.position + Vector3.up);
    }

    private void Update() {
        if (HP <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        Inventory.Instance.ModifyGold(enemydata.GetGoldAmount());
    }
}
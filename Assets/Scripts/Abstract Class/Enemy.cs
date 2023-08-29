using System.Collections.Generic;
using UnityEngine;

/// <summary>모든 적들이 상속할 abstract 클래스</summary>
public abstract class Enemy : MovingTurnActor, TurnActor.IDamagable {
    private Animator animator;
    [SerializeField] private int HP;
    [SerializeField] private int attackDamage;
    private List<Effect> StatusList = new List<Effect>();

    protected Stats GetFinalStats() {
        Stats finalStats = new Stats();
        finalStats.damage += attackDamage;
        foreach (var status in StatusList) {
            finalStats.damage += status.attack;
            finalStats.defence += status.defense;
            finalStats.healthPerTurn += status.healthPerTurn;
            finalStats.isStunned = finalStats.isStunned || status.stunned;
        }
        return finalStats;
    }

    //[SerializeField] private SpriteRenderer NextActonSprite;
    protected override void Awake() {
        base.Awake();
        StartsFacingRight = false;
        animator = GetComponent<Animator>();
    }

    protected override void TurnUpdate() {
        if (GetFinalStats().isStunned) {
            TurnUpdateOnStun();
        }
        HP += GetFinalStats().healthPerTurn;
        if (HP <= 0) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 스턴당했을 때 내부 변수 등을 조절하고 싶으면 사용한다.
    /// </summary>
    protected virtual void TurnUpdateOnStun() {
    }

    public virtual void TakeDamage(int damage) {
        if (damage - GetFinalStats().defence <= 0) {
            return;
        }
        HP -= damage - GetFinalStats().defence;
        animator.SetTrigger("isHit");
        if (HP <= 0) {
            Destroy(gameObject);
        }
    }
}
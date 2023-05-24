using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {
    /// <summary>
    /// 한 턴의 실행이 끝나는데 걸리는 시간.<br></br>
    /// 플레이어는 최소 이 시간을 기다린 후에 행동할 수 있다.
    /// </summary>

    protected new Collider2D collider;
    /// <summary>
    /// 모든 TurnActor들이 사용하는 다음턴 action
    /// </summary>
    protected Action nextAction;
    protected SpriteRenderer spriteRenderer;
    protected bool StartsFacingRight;

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    /// <summary>
    /// position에 공격 경고를 띄운다. 이 함수를 반복적으로 사용해 적의 공격을 구현한다.
    /// </summary>
    public void AttackPreTurn(Vector3 position, int damage, Action onHitEffect = null, bool instant = false) {
        Attack attack = ObjectPool.AttackPool.Get();
        attack.tag = gameObject.tag;
        attack.transform.position = position;
        attack.damage = damage;
        attack.onHitEffect = onHitEffect;
        attack.instant = instant;
    }

    protected virtual void Awake() {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// TurnActor들이 다음 행동을 정할 때 사용하는 함수
    /// </summary>
    protected abstract void DecideNextAction();

    //스프라이트를 X축으로 뒤집거나 뒤집지 않는다
    protected virtual void FlipSprite(bool toRight) {
        spriteRenderer.flipX = StartsFacingRight ? !toRight : toRight;
    }

    /// <summary>
    /// Private인 timeCounter를 대신해 클래스들이 참조할 수 있는 속성<br></br>
    /// 다른 클래스들은 자신 턴이 준비 됐는지만 알 수 있다.
    /// </summary>
    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        DecideNextAction();
    }

    /// <summary>
    /// 현재 턴의 행동을 실행하는 코드
    /// </summary>
    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "의 nextAction이 null입니다");
            DecideNextAction();
        }
        nextAction();
        DecideNextAction();
        // DecideNextAction();
    }

    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }
}
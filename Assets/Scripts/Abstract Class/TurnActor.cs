using System;
using UnityEngine;

/// <summary>
/// 턴마다 어떤 행동을 하는 모든 오브젝트는 TurnActor를 상속한다.
/// </summary>
public abstract class TurnActor : MonoBehaviour {
    /// <summary>
    /// 모든 TurnActor들이 사용하는 다음턴 action
    /// </summary>
    protected Action nextAction;

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }

    /// <summary>
    /// TurnActor들이 다음 행동을 정할 때 사용하는 함수
    /// </summary>
    protected virtual void DecideNextAction() {
        nextAction = () => { };
    }

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
}
using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {
    /// <summary>
    /// �� ���� ������ �����µ� �ɸ��� �ð�.<br></br>
    /// �÷��̾�� �ּ� �� �ð��� ��ٸ� �Ŀ� �ൿ�� �� �ִ�.
    /// </summary>

    protected new Collider2D collider;
    /// <summary>
    /// ��� TurnActor���� ����ϴ� ������ action
    /// </summary>
    protected Action nextAction;
    protected SpriteRenderer spriteRenderer;
    protected bool StartsFacingRight;

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    /// <summary>
    /// position�� ���� ��� ����. �� �Լ��� �ݺ������� ����� ���� ������ �����Ѵ�.
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
    /// TurnActor���� ���� �ൿ�� ���� �� ����ϴ� �Լ�
    /// </summary>
    protected abstract void DecideNextAction();

    //��������Ʈ�� X������ �����ų� ������ �ʴ´�
    protected virtual void FlipSprite(bool toRight) {
        spriteRenderer.flipX = StartsFacingRight ? !toRight : toRight;
    }

    /// <summary>
    /// Private�� timeCounter�� ����� Ŭ�������� ������ �� �ִ� �Ӽ�<br></br>
    /// �ٸ� Ŭ�������� �ڽ� ���� �غ� �ƴ����� �� �� �ִ�.
    /// </summary>
    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        DecideNextAction();
    }

    /// <summary>
    /// ���� ���� �ൿ�� �����ϴ� �ڵ�
    /// </summary>
    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "�� nextAction�� null�Դϴ�");
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
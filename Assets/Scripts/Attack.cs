using System;
using UnityEngine;
using UnityEngine.Pool;

public class Attack : TurnActor {

    //TurnAction���� �浹�� Ȱ��ȭ�� �� 3������ �� �������.
    private float timeCount = 0f;

    private const float lifeTime = 0.2f;

    public int damage;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private IObjectPool<Attack> managedPool;
    public Action onHitEffect;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable() {
        base.OnEnable();
        spriteRenderer.enabled = true;
        if (gameObject.CompareTag("Enemy")) {
            boxCollider.enabled = false;
            nextAction += () => {
                boxCollider.enabled = true;
                spriteRenderer.enabled = false;
            };
        } else {
            boxCollider.enabled = true;
        }
    }

    protected override void DecideNextAction() {
        nextAction += () => { };
    }

    /// <summary>
    /// ������ƮǮ���� Ȱ���ϴ� �ڵ�
    /// </summary>
    /// <param name="pool"></param>
    public void SetManagedPool(IObjectPool<Attack> pool) {
        managedPool = pool;
    }

    private void Update() {
        if (!boxCollider.enabled) {
            if (gameObject.CompareTag("Player")) {
                boxCollider.enabled = true;
            } else {
                return;
            }
        }
        timeCount += Time.deltaTime;
        if (timeCount >= lifeTime) {
            managedPool.Release(this);
            timeCount = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Player>()?.TakeDamage(damage, onHitEffect);
        } else if (gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
}
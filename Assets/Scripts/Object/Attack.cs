using System;
using UnityEngine;
using UnityEngine.Pool;

public class Attack : TurnActor {
    public int damage;
    public bool instant;
    public Action onHitEffect;
    private const float lifeTime = 2 * movingTime;
    private IObjectPool<Attack> managedPool;
    private float timeCount = 0f;
    public void SetManagedPool(IObjectPool<Attack> pool) {
        managedPool = pool;
    }

    protected override void Awake() {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void DecideNextAction() {
        nextAction += () => { };
    }

    protected override void OnEnable() {
        base.OnEnable();
        spriteRenderer.enabled = true;
        if (gameObject.CompareTag("Enemy")) {
            objectCollider.enabled = false;
            nextAction += () => {
                objectCollider.enabled = true;
                spriteRenderer.enabled = false;
            };
        } else {
            objectCollider.enabled = true;
        }
        if (instant) {
            objectCollider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Player>()?.TakeDamage(damage);
        } else if (gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }

    private void Update() {
        if (!objectCollider.enabled) {
            if (gameObject.CompareTag("Player") || instant) {
                objectCollider.enabled = true;
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
}
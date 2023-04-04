using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Attack : TurnActor {
    //TurnAction으로 충돌을 활성화한 후 3프레임 후 사라진다.
    private int frameCount = 0, life = 3, damage = 1;
    private BoxCollider2D boxCollider;
    private IObjectPool<Attack> managedPool;
    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    protected override void OnEnable() {
        base.OnEnable();
        boxCollider.enabled = false;
        nextAction = () => boxCollider.enabled = true;
    }
    public void SetManagedPool(IObjectPool<Attack> pool) {
        managedPool = pool;
    }
    void Update() {
        if (!boxCollider.enabled) return;
        frameCount += 1;
        if (frameCount >= life) {
            managedPool.Release(this);
            frameCount = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.SendMessage("TakeDamage", damage);
        }
    }
}

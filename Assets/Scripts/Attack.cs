using System;
using UnityEngine;
using UnityEngine.Pool;

public class Attack : TurnActor {
    //TurnAction으로 충돌을 활성화한 후 3프레임 후 사라진다.
    private int frameCount = 0, life = 3;
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
        if (gameObject.CompareTag("Enemy")) {
            boxCollider.enabled = false;
            nextAction += () => boxCollider.enabled = true;
        } else {
            boxCollider.enabled = true;
            spriteRenderer.color = Color.blue;            
        }
    }
    protected override void DecideNextAction() {
        nextAction += () => { };
    }
    /// <summary>
    /// 오브젝트풀링에 활용하는 코드
    /// </summary>
    /// <param name="pool"></param>
    public void SetManagedPool(IObjectPool<Attack> pool) {
        managedPool = pool;
    }
    void Update() {
        if (!boxCollider.enabled) {
            if (gameObject.CompareTag("Player")) {
                boxCollider.enabled = true;
            } else {
                return;
            }
        }
        frameCount += 1;
        if (frameCount >= life) {
            managedPool.Release(this);
            frameCount = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Player>()?.TakeDamage(damage,onHitEffect);
        } else if (gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
}

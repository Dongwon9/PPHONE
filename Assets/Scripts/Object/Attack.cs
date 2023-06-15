﻿using System;
using UnityEngine;
using UnityEngine.Pool;

public class Attack : TurnActor {
    private const float lifeTime = 2 * MovingTurnActor.movingTime;
    private new Collider2D collider;
    private IObjectPool<Attack> managedPool;
    private SpriteRenderer spriteRenderer;
    private float timeCount = 0f;
    public int damage;
    public bool instant;
    public Action onHitEffect;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<Player>()?.TakeDamage(damage);
        } else if (gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage);
        }
    }

    private void Update() {
        if (!collider.enabled) {
            if (gameObject.CompareTag("Player") || instant) {
                collider.enabled = true;
            } else {
                return;
            }
        }
        timeCount += Time.deltaTime;
        if (timeCount >= lifeTime) {
            managedPool.Release(this);
            timeCount = 0f;
        }
        if (instant) {
            collider.enabled = true;
        }
    }

    protected override void DecideNextAction() {
        nextAction += () => { };
    }

    protected override void OnEnable() {
        base.OnEnable();
        spriteRenderer.enabled = true;
        if (gameObject.CompareTag("Enemy")) {
            collider.enabled = false;
            nextAction += () => {
                collider.enabled = true;
                spriteRenderer.enabled = false;
            };
        } else {
            collider.enabled = true;
        }
        if (instant) {
            collider.enabled = true;
        }
    }

    /// <summary>
    /// 공격이 실행되기 전에 제거하기 위한 public 메서드
    /// </summary>
    public void RemoveAttack() {
        managedPool.Release(this);
    }

    public void SetManagedPool(IObjectPool<Attack> pool) {
        managedPool = pool;
    }
}
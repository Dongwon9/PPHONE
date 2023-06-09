﻿using System;
using UnityEngine;

/// <summary>모든 적들이 상속할 abstract 클래스</summary>
public abstract class Enemy : MovingTurnActor, TurnActor.IDamagable {
    private Animator animator;
    [SerializeField] private int HP;
    //[SerializeField] private SpriteRenderer NextActonSprite;

    protected class EnemyAction {
        public Action PreTurnAction;
        public Action TurnAction;

        public EnemyAction(Action preTurnAction, Action turnAction) {
            PreTurnAction = preTurnAction;
            TurnAction = turnAction;
        }
    }

    protected override void Awake() {
        base.Awake();
        StartsFacingRight = false;
        animator = GetComponent<Animator>();
    }

    protected EnemyAction MoveAction(Direction dir) {
        return new EnemyAction(() => MovePreTurn(dir), () => Move(dir));
    }

    protected void MovePreTurn(Direction dir) {
        //NextActonSprite.enabled = true;
        Vector3 offset = Vector3.zero;
        switch (dir) {
            case TurnActor.Direction.Left:
                offset = Vector3.left;
                break;

            case TurnActor.Direction.Right:
                offset = Vector3.right;
                break;

            case TurnActor.Direction.Up:
                offset = Vector3.up;
                break;

            case TurnActor.Direction.Down:
                offset = Vector3.down;
                break;
        }
        //NextActonSprite.transform.SetPositionAndRotation(transform.position + offset, Quaternion.Euler(0, 0, -90 * (int)dir));
    }

    public virtual void TakeDamage(int damage) {
        HP -= damage;
        animator.SetTrigger("isHit");
        if (HP <= 0) {
            nextAction = null;
            Destroy(gameObject);
        }
    }
}
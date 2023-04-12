using System;
using UnityEngine;
using UnityEngine.Pool;

public enum Direction { Left, Up, Right, Down };
public abstract class Enemy : TurnActor {
    protected class EnemyAction {
        public Action PreTurnAction;
        public Action TurnAction;
        public EnemyAction(Action preTurnAction, Action turnAction) {
            PreTurnAction = preTurnAction;
            TurnAction = turnAction;
        }
    }
    public SpriteRenderer NextActonSprite;
    public int HP;
    private Animator animator;
    protected EnemyAction MoveAction(Direction dir) {
        return new EnemyAction(() => MovePreTurn(dir), () => Move(dir));
    }
    

    protected void MovePreTurn(Direction dir) {
        NextActonSprite.enabled = true;
        Vector3 offset = Vector3.zero;
        switch (dir) {
            case Direction.Left:
                offset = Vector3.left;
                break;
            case Direction.Right:
                offset = Vector3.right;
                break;
            case Direction.Up:
                offset = Vector3.up;
                break;
            case Direction.Down:
                offset = Vector3.down;
                break;
        }
        NextActonSprite.transform.position = transform.position + offset;
        NextActonSprite.transform.rotation = Quaternion.Euler(0, 0, -90 * (int)dir);
    }
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    
    public virtual void TakeDamage(int damage) {
        HP -= damage;
        animator.SetTrigger("isHit");
        if(HP <= 0) {
            Destroy(gameObject);
        }
    }
    
    
}

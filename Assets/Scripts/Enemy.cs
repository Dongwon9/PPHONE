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
    
    public Attack AttackPrefab;
    public static IObjectPool<Attack> AttackPool;
    public SpriteRenderer NextActonSprite;
    public int HP;
    protected EnemyAction MoveAction(Direction dir) {
        return new EnemyAction(() => MovePreTurn(dir), () => Move(dir));
    }
    //이 함수를 반복적으로 사용해 여러 곳에 공격하는 것을 구현한다.
    protected void AttackPreTurn(int offsetX, int offsetY, int damage) {
        var attack = AttackPool.Get();
        attack.transform.position = transform.position + new Vector3(offsetX, offsetY, 0);
        attack.damage = damage;
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
        AttackPool = new ObjectPool<Attack>(
            () => {
                Attack attack = Instantiate(AttackPrefab).GetComponent<Attack>();
                attack.SetManagedPool(AttackPool);
                return attack;
            },
        (attack) => attack.gameObject.SetActive(true),
        (attack) => attack.gameObject.SetActive(false),
        (attack) => Destroy(attack.gameObject),
        maxSize: 100
        );
    }
}

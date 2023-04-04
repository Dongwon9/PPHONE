using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;

public enum Direction { Left, Up, Right, Down };
public class Enemy : TurnActor {
    protected class EnemyAction {
        public Action PreTurnAction;
        public Action TurnAction;
        public EnemyAction(Action preTurnAction, Action turnAction) {
            PreTurnAction = preTurnAction;
            TurnAction = turnAction;
        }
    }
    private List<EnemyAction> sampleAI;
    private int counter = 0;
    public Attack AttackPrefab;
    private IObjectPool<Attack> AttackPool;
    public SpriteRenderer NextActonSprite;
    public int HP;
    protected EnemyAction MoveAction(Direction dir) {
        return new EnemyAction(() => MovePreTurn(dir), () => Move(dir));
    }
    //이 함수를 반복적으로 사용해 여러 곳에 공격하는 것을 구현한다.
    protected void AttackPreTurn(int offsetX, int offsetY) {
        var attack = AttackPool.Get();
        attack.transform.position = transform.position + new Vector3(offsetX, offsetY, 0);
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
        gravityAffected = true;
    }
    protected void OnEnable() {
        base.OnEnable();
        sampleAI = new List<EnemyAction> {
        MoveAction(Direction.Left),
        MoveAction(Direction.Left),
        MoveAction(Direction.Right),
        MoveAction(Direction.Right),
        new EnemyAction(()=>AttackPreTurn(-1,0),()=>{return; }),
        MoveAction(Direction.Up),
        MoveAction(Direction.Up),
        MoveAction(Direction.Down),
        new EnemyAction(LaserPreTurn,()=>{return; })
        };
        nextAction = sampleAI[0].TurnAction;
        sampleAI[0].PreTurnAction();
    }
    void LaserPreTurn() {
        for (int i = 1; i <= 10; i++) {
            AttackPreTurn(-i, 0);
        }
    }
    private void Update() {
        if (nextAction != null)
            return;
        nextAction = sampleAI[counter].TurnAction;
        sampleAI[counter].PreTurnAction();
    }
    protected override void TurnUpdate() {
        base.TurnUpdate();
        counter = (counter + 1) % sampleAI.Count;
    }
}

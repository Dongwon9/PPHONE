using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {
    private readonly EnemyAction Nothing = new(() => { }, () => { });

    private int counter = 0;

    private List<EnemyAction> sampleAI;

    public event Action PreTurnActions;

    private class WaveAttack {
        private const int attackDuration = 5;
        private static List<WaveAttack> attackList = new List<WaveAttack>();
        private static BasicEnemy attackUser = null;
        private Direction facing;
        private int turnCounter = 0, damage;

        private WaveAttack() {
        }

        private void Execute() {
            int dirSign = 1;
            if (facing == Direction.Left) {
                dirSign = -1;
            }
            for (int i = -1; i <= 1; i++) {
                attackUser.AttackPreTurn(attackUser.transform.position + new Vector3(turnCounter * dirSign, i), damage,
                    () => {
                    });
            }
            turnCounter += 1;
            if (turnCounter > attackDuration) {
                turnCounter = 0;
            }
        }

        public static void Activate(Direction facing, int damage) {
            WaveAttack usingAttack = null;
            foreach (var attack in attackList) {
                if (attack.turnCounter == 0) {
                    usingAttack = attack;
                    break;
                }
            }
            if (usingAttack == null) {
                usingAttack = new WaveAttack();
                attackList.Add(usingAttack);
            }
            usingAttack.turnCounter = 1;
            usingAttack.damage = damage;
            usingAttack.facing = facing;
        }

        public static void ExecuteAll() {
            foreach (var attack in attackList) {
                if (attack.turnCounter > 0) {
                    attack.Execute();
                }
            }
        }

        public static void SetAttackUser(BasicEnemy enemy) {
            attackUser = enemy;
        }
    }

    private void LaserPreTurn() {
        for (int i = 1; i <= 10; i++) {
            //AttackPreTurn(-i, 0, 2);
            AttackPreTurn(transform.position + new Vector3(-i, 0), 2);
        }
    }

    protected override void DecideNextAction() {
        EnemyAction decidedAction = Nothing;
        nextAction = () => { };
        if (counter < 4) {
            if (counter % 2 == 0) {
                WaveAttack.Activate(Direction.Left, 5);
            } else {
                WaveAttack.Activate(Direction.Right, 5);
            }
        }
        counter = (counter + 1) % 8;
        PreTurnActions += decidedAction.PreTurnAction;
        nextAction = decidedAction.TurnAction;
        PreTurnActions();
        WaveAttack.ExecuteAll();
        PreTurnActions = () => { };
    }

    protected override void OnEnable() {
        WaveAttack.SetAttackUser(this);
        base.OnEnable();
        sampleAI = new List<EnemyAction> {
            MoveAction(Direction.Right),
            MoveAction(Direction.Left),
            MoveAction(Direction.Up),
            MoveAction(Direction.Down)
        };
    }

    public void AddNextAction(Action action) {
        nextAction += action;
    }
}
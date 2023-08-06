using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {
    private int counter = 0;

    private List<Action> sampleAI;

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
            if (facing == TurnActor.Direction.Left) {
                dirSign = -1;
            }
            for (int i = -1; i <= 1; i++) {
                attackUser.AttackWarning(attackUser.transform.position + new Vector3(turnCounter * dirSign, i));
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
            //AttackWarning(-i, 0, 2);
            AttackWarning(transform.position + new Vector3(-i, 0));
        }
    }

    protected override void DecideNextAction() {
        nextAction = () => { };
        if (counter < 4) {
            if (counter % 2 == 0) {
                WaveAttack.Activate(TurnActor.Direction.Left, 5);
            } else {
                WaveAttack.Activate(TurnActor.Direction.Right, 5);
            }
        }
        counter = (counter + 1) % 8;
        PreTurnActions();
        WaveAttack.ExecuteAll();
        PreTurnActions = () => { };
    }

    protected override void OnEnable() {
        WaveAttack.SetAttackUser(this);
        base.OnEnable();
        sampleAI = new List<Action> {
        };
    }

    public void AddNextAction(Action action) {
        nextAction += action;
    }
}
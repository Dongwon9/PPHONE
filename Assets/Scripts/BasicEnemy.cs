using System.Collections.Generic;
using UnityEngine;
public class BasicEnemy : Enemy {
    private List<EnemyAction> sampleAI;
    private int counter = 0;
    private WaveAttack waveAttack;
    void LaserPreTurn() {
        for (int i = 1; i <= 10; i++) {
            //AttackPreTurn(-i, 0, 2);
            AttackPreTurn(transform.position + new Vector3(-i, 0), 2);
        }
    }

    readonly EnemyAction Nothing = new(() => { }, () => { });
    protected override void OnEnable() {
        base.OnEnable();
        waveAttack = GetComponent<WaveAttack>();
        sampleAI = new List<EnemyAction> {
            new EnemyAction(()=>{ },()=>waveAttack.ActivatePreTurn(3)),
            Nothing,
            Nothing,
            Nothing,
            Nothing
        };
        nextAction = sampleAI[0].TurnAction;
        sampleAI[0].PreTurnAction();
    }
    protected override void DecideNextAction() {
        counter = (counter + 1) % 5;
        nextAction = sampleAI[counter].TurnAction;
        sampleAI[counter].PreTurnAction();
    }

}

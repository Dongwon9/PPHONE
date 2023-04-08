using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WaveAttack : TurnActor {
    private Vector3 attackOrigin;
    private int counter = 0;
    private int damage;
    public void ActivatePreTurn(int damage) {
        counter = 1;
        attackOrigin = transform.position;
        this.damage = damage;
    }

    void Update() {
        if (nextAction != null) {
            return;
        }
        if (counter > 0) {
            for (int i = -1; i <= 1; i++) {
                AttackPreTurn(attackOrigin + new Vector3(-counter, i), damage);
            }
            counter++;
            if (counter > 5) {
                counter = 0;
            }
        }
        nextAction = () => { };
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Laser : TurnActor {
    public GameObject laserDeco;
    [SerializeField]
    private int turnTillLaser = 4;//레이저가 몇 턴에 한번씩 발사되는가?
    [SerializeField]
    private int startingTurnCount;
    private int turnCount = 0;
    protected override void OnEnable() {
        base.OnEnable();
        turnCount = startingTurnCount % turnTillLaser;
    }

    protected override void TurnUpdate() {
        turnCount += 1;
        if(turnCount >= turnTillLaser) {
            LaserAttack(transform.position, Vector2.right, 10, Target.Any, true);
            laserDeco.SetActive(true);
            Invoke(nameof(TurnOffLaser), MovingTurnActor.movingTime);
            turnCount = 0;
        }
    }
    private void TurnOffLaser() {
        laserDeco.SetActive(false);
    }
}
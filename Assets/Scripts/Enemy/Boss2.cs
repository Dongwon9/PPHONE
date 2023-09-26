using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Boss2 : Enemy, TurnActor.IDamagable {
    private int turnsSinceTeleport = 0; // 순간 이동을 위한 턴 카운터
    private Direction[] directions = { Direction.Right, Direction.Up, Direction.Left, Direction.Down };
    private int[] directionCoolTime = { 0, 0, 0, 0 };
    int counter = 0;
    private enum AIMode { Moving, Attacking };
    private AIMode mode = AIMode.Moving;
    private Vector3 PosToAttack;
    // 게임 루프나 턴 관리 로직에서 호출되는 메서드로, 보스의 움직임을 처리한다
    protected override void TurnUpdate() {
        //순간이동 한 후, 공격
        if(mode == AIMode.Attacking) {
            Attack(PosToAttack, enemydata.Damage, Target.Player);
            mode = AIMode.Moving;
            return;
        }
        turnsSinceTeleport += 1;
        // 5턴이 지났으면 순간 이동 실행
        if (turnsSinceTeleport >= 5) {
            //플레이어 옆으로 순간이동
            List<Vector3> CanTeleportTo = new();
            foreach (var dir in directions) {
                Vector3 v = Player.Position + DirectionToVector(dir);
                if (CheckForObjectAtPosition(new Target[] { Target.Wall },v) && 
                      (v != transform.position)) {
                    CanTeleportTo.Add(v);
                }
            }
            transform.position = CanTeleportTo[Random.Range(0,CanTeleportTo.Count)];
            turnsSinceTeleport = 0; // 턴 카운터 초기화
            for (int i = 0; i < 4; i++) {
                directionCoolTime[i] = 0; //이동방향 쿨타임도 초기화
            }
            PosToAttack = Player.Position;
            mode = AIMode.Attacking;
            return;
        }
        counter += 1;
        if (counter < 2)
            return;
        counter = 0;
        // 보스의 이동 로직 구현(무작위 방향으로 이동
        List<Direction> canMoveTo = new();
        for (int i = 0; i < 4; i++) {
            if (CanMove(directions[i]) && (directionCoolTime[i] == 0)) {
                canMoveTo.Add(directions[i]);
            }
        }
        Direction pickedDir = canMoveTo[Random.Range(0, canMoveTo.Count)];
        Move(pickedDir);
        for (int i = 0; i < 4; i++) {
            if (directionCoolTime[i] > 0) {
                directionCoolTime[i]--;
            }
        }
        //보스는 한 방향으로 움직이면, 2턴동안 그 반대방향으로 움직이지 않는다.
        switch (pickedDir) {
            case Direction.Left:
                if (directionCoolTime[0] == 0) {
                    directionCoolTime[0] = 2;
                }
                break;

            case Direction.Right:
                if (directionCoolTime[2] == 0) {
                    directionCoolTime[2] = 2;
                }
                break;

            case Direction.Up:
                if (directionCoolTime[3] == 0)
                    directionCoolTime[3] = 2;
                break;

            case Direction.Down:
                if (directionCoolTime[1] == 0)
                    directionCoolTime[1] = 2;
                break;
        }              
        
    }
}
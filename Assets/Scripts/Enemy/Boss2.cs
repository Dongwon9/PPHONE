using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy, IDamagable {

    private enum AIMode { Moving, Attacking };

    private readonly Direction[] directions = { Direction.Right, Direction.Up, Direction.Left, Direction.Down };
    private readonly int[] directionCoolTime = { 0, 0, 0, 0 };
    private int turnsSinceTeleport = 0; // 순간 이동을 위한 턴 카운터
    private int counter = 0;
    private AIMode mode = AIMode.Moving;
    private Vector3 PosToAttack;
    protected override void TurnUpdate() {
        //순간이동 한 후, 공격
        if (mode == AIMode.Attacking) {
            Attack(PosToAttack, enemydata.Damage, Target.Player);
            CreateSlash(PosToAttack);
            animator.SetTrigger("attack02");
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
                if (IsObjectAtPosition(Target.Wall, v) &&
                      (v != transform.position)) {
                    CanTeleportTo.Add(v);
                }
            }
            transform.position = CanTeleportTo[Random.Range(0, CanTeleportTo.Count)];
            turnsSinceTeleport = 0; // 턴 카운터 초기화
            for (int i = 0; i < 4; i++) {
                directionCoolTime[i] = 0; //이동방향 쿨타임도 초기화
            }
            PosToAttack = Player.Position;
            AttackWarning(PosToAttack);
            mode = AIMode.Attacking;
            return;
        }
        counter += 1;
        if (counter < 2) {
            return;
        }

        counter = 0;
        // 보스의 이동 로직 구현(무작위 방향으로 이동)
        List<Direction> canMoveTo = new();
        for (int i = 0; i < 4; i++) {
            if (CanMove(directions[i]) && (directionCoolTime[i] == 0)) {
                canMoveTo.Add(directions[i]);
            }
        }
        Direction pickedDir = canMoveTo[Random.Range(0, canMoveTo.Count)];
        Move(pickedDir);
        animator.SetTrigger("walk");
        for (int i = 0; i < 4; i++) {
            if (directionCoolTime[i] > 0) {
                directionCoolTime[i]--;
            }
        }
        //보스는 한 방향으로 움직이면, 2턴동안 그 반대방향으로 움직이지 않는다.
        switch (pickedDir) {
            case Direction.Left:
                if (directionCoolTime[(int)Direction.Right] == 0) {
                    directionCoolTime[(int)Direction.Right] = 2;
                }
                break;

            case Direction.Right:
                if (directionCoolTime[(int)Direction.Left] == 0) {
                    directionCoolTime[(int)Direction.Left] = 2;
                }
                break;

            case Direction.Up:
                if (directionCoolTime[(int)Direction.Down] == 0) {
                    directionCoolTime[(int)Direction.Down] = 2;
                }
                break;

            case Direction.Down:
                if (directionCoolTime[(int)Direction.Up] == 0) {
                    directionCoolTime[(int)Direction.Up] = 2;
                }
                break;
        }
    }
}
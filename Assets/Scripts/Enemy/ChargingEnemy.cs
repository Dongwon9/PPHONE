using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : Enemy, TurnActor.IDamagable {

    private enum AIMode { Finding, Charging, Stun }

    private readonly Vector2[] directionOrder = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };
    private readonly List<Direction> dirorder = new List<Direction> { Direction.Up, Direction.Down, Direction.Right, Direction.Left };
    private Direction ChargeDirection;
    private AIMode mode = AIMode.Finding;
    private Path[] allPathToPlayer = new Path[4];
    private int turnCounter;
    private Vector3? DecideWhereToGo() {
        //플레이어의 4방향 중, 어느 방향으로 일직선상에 갈지 정한다.
        Vector3[] targetPos = new Vector3[4];
        int i = 0;
        for (i = 0; i < 4; i++) {
        }
        targetPos[0] = Round(Physics2D.Raycast(Player.Position, Vector2.up, 100, LayerMask.GetMask("Wall")).point) - Vector2.up;
        targetPos[1] = Round(Physics2D.Raycast(Player.Position, Vector2.down, 100, LayerMask.GetMask("Wall")).point) - Vector2.down;
        targetPos[2] = Round(Physics2D.Raycast(Player.Position, Vector2.right, 100, LayerMask.GetMask("Wall")).point) - Vector2.right;
        targetPos[3] = Round(Physics2D.Raycast(Player.Position, Vector2.left, 100, LayerMask.GetMask("Wall")).point) - Vector2.left;

        for (i = 0; i <= 1; i++) {
            //만약 이 좌표가 자신보다 '뒤'에 있으면
            if ((transform.position.y < Player.Position.y && targetPos[i].y < transform.position.y) ||
                 (transform.position.y > Player.Position.y && targetPos[i].y > transform.position.y)) {
                //자신의 좌표에 맞게 맞춘다
                targetPos[i] = new Vector3(targetPos[i].x, transform.position.y);
            }
            allPathToPlayer[i].FindPath(transform.position, targetPos[i]);
        }
        for (; i <= 3; i++) {
            if ((transform.position.x < Player.Position.x && targetPos[i].x < transform.position.x) ||
                (transform.position.x > Player.Position.x && targetPos[i].x > transform.position.x)) {
                targetPos[i] = new Vector3(transform.position.x, targetPos[i].y);
            }
            allPathToPlayer[i].FindPath(transform.position, targetPos[i]);
        }
        //4개의 지점중 가장 경로가 짧은 곳으로 간다.
        Vector3? returnPos = null;
        int length = 999999999;
        foreach (var path in allPathToPlayer) {
            if (path.PathExists && path.PathLength < length) {
                length = path.PathLength;
                returnPos = path.GetNextPos();
            }
        }
        return returnPos;
    }

    protected override void OnEnable() {
        base.OnEnable();
        for (int i = 0; i < allPathToPlayer.Length; i++) {
            allPathToPlayer[i] = new Path();
        }
    }

    protected override void TurnUpdate() {
        if (mode == AIMode.Finding) {
            for (int i = 0; i < 4; i++) {
                //플레이어와 벽에 막히는 레이캐스트를 한다.
                var hit = Physics2D.Raycast(transform.position, directionOrder[i], 100, LayerMask.GetMask("Wall") | LayerMask.GetMask("Player"));
                //레이캐스트를 맞은 것이 플레이어면, 돌진 모드로 전환한다.
                if (hit.collider.CompareTag("Player")) {
                    mode = AIMode.Charging;
                    ChargeDirection = dirorder[i];
                    break;
                }
            }
        }
        switch (mode) {
            case AIMode.Finding:
                if (turnCounter % 2 == 1) {
                    return;
                }

                Vector3? dest = DecideWhereToGo();
                if (dest != null) {
                    Move((Vector3)dest);
                }
                break;

            case AIMode.Charging:
                //가려는 칸에 벽이 있으면, 현 위치에서 멈추고 기절한다.
                if (CheckForObjectAtPosition(new Target[] { Target.Wall }, transform.position + DirectionToVector(ChargeDirection))) {
                    mode = AIMode.Stun;
                    turnCounter = 3;
                    return;
                }
                //이동 전, 플레이어가 자신에게 부딪히면, 플레이어에게 대미지를 주고 자신은 기절한다.
                if (CheckForObjectAtPosition(new Target[] { Target.Player }, transform.position)) {
                    Attack(transform.position, enemydata.Damage, Target.Player);
                    mode = AIMode.Stun;
                    turnCounter = 3;
                    return;
                }

                Move(ChargeDirection);
                //이동 후에 한번 더 플레이어가 자신에게 부딪혔는지 확인한다.
                if (CheckForObjectAtPosition(new Target[] { Target.Player }, transform.position + DirectionToVector(ChargeDirection))) {
                    Attack(transform.position + DirectionToVector(ChargeDirection), enemydata.Damage, Target.Player);
                    mode = AIMode.Stun;
                    turnCounter = 3;
                }
                break;

            case AIMode.Stun:
                turnCounter -= 1;
                if (turnCounter <= 0) {
                    turnCounter = 0;
                    mode = AIMode.Finding;
                }
                break;
        }
    }
}
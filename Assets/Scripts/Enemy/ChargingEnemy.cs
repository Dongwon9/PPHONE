using UnityEngine;

/// <summary>
/// 플레이어가 일직선상에 있으면 플레이어 방향으로 돌진해오는 적
/// </summary>
public class ChargingEnemy : Enemy, IDamagable {

    private enum AIMode { Finding, Charging, Stun }

    private readonly Vector3[] directionOrder = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
    private readonly Path path = new();
    private Vector3 ChargeDirection;
    private AIMode mode = AIMode.Finding;
    private int turnCounter;

    protected override void TurnUpdate() {
        if (mode == AIMode.Finding) {
            for (int i = 0; i < 4; i++) {
                //플레이어와 벽에 막히는 레이캐스트를 한다.
                var hit = Physics2D.Raycast(transform.position, directionOrder[i], 100,
                    LayerMask.GetMask("Wall") | LayerMask.GetMask("Player"));
                //레이캐스트를 맞은 것이 플레이어면, 돌진 모드로 전환한다.
                if (hit.collider.CompareTag("Player")) {
                    mode = AIMode.Charging;
                    ChargeDirection = directionOrder[i];
                    break;
                }
            }
        }

        switch (mode) {
            case AIMode.Finding:
                turnCounter++;
                if (turnCounter % 2 == 1) {
                    return;
                }

                path.FindPath(transform.position, Player.Position);
                if (path.PathExists) {
                    Move(path.GetNextPos());
                }

                break;

            case AIMode.Charging:
                //이동 전, 플레이어가 자신에게 부딪히면, 플레이어에게 대미지를 주고 자신은 기절한다.
                if (IsObjectAtPosition(Target.Player, transform.position)) {
                    Attack(transform.position, enemydata.Damage, Target.Player);
                    mode = AIMode.Stun;
                    turnCounter = 3;
                    return;
                }
                //가려는 칸에 벽이 있으면, 현 위치에서 멈추고 기절한다.
                if (IsObjectAtPosition(Target.Wall, transform.position + ChargeDirection)) {
                    mode = AIMode.Stun;
                    turnCounter = 3;
                    return;
                }

                //움직인다.
                Move(transform.position + ChargeDirection);

                //이동 후에 한번 더 플레이어가 자신에게 부딪혔는지 확인한다.
                if (IsObjectAtPosition(Target.Player, transform.position + ChargeDirection)) {
                    Attack(transform.position + ChargeDirection, enemydata.Damage, Target.Player);
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
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy {

    private enum AIMode { finding, attacking };

    private readonly Path pathFinding = new();
    private List<Vector3> bossTraces = new List<Vector3>();// 보스의 흔적 위치 리스트
    [SerializeField] private GameObject PurpleSquare;
    private List<GameObject> PurpleSquareList = new List<GameObject>();
    private int counter = 0;
    private AIMode mode = AIMode.finding;
    private Vector3 attackLocation; // 공격할 좌표
    private void OnDestroy() {
        foreach (GameObject go in PurpleSquareList) {
            Destroy(go);
        }
    }

    protected override void Awake() {
        base.Awake();
    }

    protected override void TurnUpdate() {
        counter += 1;
        if (counter == 2) {
            if (!bossTraces.Contains(transform.position)) {
                bossTraces.Add(transform.position);
                PurpleSquareList.Add(Instantiate(PurpleSquare, transform.position, Quaternion.identity));
            }
            counter = 0;
        }
        if (mode == AIMode.finding) {
            pathFinding.FindPath(transform.position, Player.Position);
            if (!pathFinding.PathExists) {
                return;
            }
            //필요한 이동 길이가 1칸보다 많으면 움직인다.
            if (pathFinding.PathLength > 1) {
                Move(pathFinding.GetNextPos());
            } else {
                //필요한 이동 길이가 1칸이 되면, 그 턴은 아무것도 하지 않고,
                //다음턴에 공격한다.
                AttackWarning(pathFinding.GetNextPos());
                attackLocation = pathFinding.GetNextPos();
                mode = AIMode.attacking;
            }
        } else {
            Attack(attackLocation, enemydata.Damage, Target.Player);
            CreateSlash(attackLocation);
            mode = AIMode.finding;
        }

        // 플레이어의 위치를 확인하고, 보스의 흔적 위치와 일치하는지 확인한다.
        foreach (Vector3 tracePosition in bossTraces) {
            Attack(tracePosition, 1, Target.Player);
        }
    }
}
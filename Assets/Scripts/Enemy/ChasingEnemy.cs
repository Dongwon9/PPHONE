using UnityEngine;

/// <summary>플레이어를 쫓아와서 거리가 1칸이 되면 공격하는 적</summary>
public class ChasingEnemy : Enemy {

    private enum AIMode { finding, attacking };

    private AIMode mode = AIMode.finding;
    private Vector3 attackLocation; // 공격할 좌표
    private Path pathfinding;
    protected override void TurnUpdate() {
        if (mode == AIMode.finding) {
            pathfinding.FindPath(transform.position, Player.Position);
            if (!pathfinding.PathExists) {
                return;
            }
            //필요한 이동 길이가 1칸보다 많으면 움직인다.
            if (pathfinding.PathLength > 1) {
                Move(pathfinding.GetNextPos());
            } else {
                //필요한 이동 길이가 1칸이 되면, 그 턴은 아무것도 하지 않고,
                //다음턴에 공격한다.
                AttackWarning(pathfinding.GetNextPos());
                attackLocation = pathfinding.GetNextPos();
                mode = AIMode.attacking;
            }
        } else {
            Attack(attackLocation, enemydata.Damage, Target.Player);
            mode = AIMode.finding;
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new Path();
        pathfinding.FindPath(transform.position, Player.Position);
    }
}
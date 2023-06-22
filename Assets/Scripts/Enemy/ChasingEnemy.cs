using System.Collections.Generic;
using UnityEngine;

/// <summary>플레이어를 쫓아와서 거리가 1칸이 되면 공격하는 적</summary>
public class ChasingEnemy : Enemy {
    private AStarPathfinding pathfinding;
    [SerializeField] private List<Vector3> pathToPlayer;

    private void ExecuteMove() {
        Move(pathToPlayer[1]);
    }

    protected override void DecideNextAction() {
        nextAction = () => {
            pathToPlayer = pathfinding?.FindPath(transform.position, Player.Instance.transform.position);
            if (pathToPlayer == null) {
                return;
            }
            //필요한 이동 길이가 2칸보다 많으면 움직인다.
            if (pathToPlayer.Count > 2) {
                Invoke(nameof(ExecuteMove), movingTime / 2);
            }
            //필요한 이동 길이가 2칸이면 공격한다.
            if (pathToPlayer.Count == 2) {
                AttackWarning(Player.Instance.transform.position);
                nextAction = () => Attack(Player.Instance.transform.position, 5, Target.Player);
            }
        };
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new AStarPathfinding(31, 31);
        pathToPlayer = pathfinding?.FindPath(transform.position, Player.Instance.transform.position);
    }
}
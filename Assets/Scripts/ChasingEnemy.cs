using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어를 쫓아와서 거리가 1칸이 되면 공격하는 적
/// </summary>
public class ChasingEnemy : Enemy {
    private AStarPathfinding pathfinding;
    [SerializeField] private List<Vector3> pathToPlayer;

    protected override void DecideNextAction() {
        nextAction = () => {
            if (pathToPlayer != null && pathToPlayer.Count > 2) {
                transform.position = pathToPlayer[1];
            }
            pathToPlayer = pathfinding.FindPath(transform.position, GameManager.playerReference.transform.position);
        };
        if (pathToPlayer.Count == 2) {
            AttackPreTurn(GameManager.playerReference.transform.position, 5);
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new AStarPathfinding(63, 63);
        pathToPlayer = pathfinding.FindPath(transform.position, GameManager.playerReference.transform.position);
    }

    protected override void TurnUpdate() {
        base.TurnUpdate();
    }

    // Update is called once per frame
    private void Update() {
    }
}
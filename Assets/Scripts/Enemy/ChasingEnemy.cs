using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �Ѿƿͼ� �Ÿ��� 1ĭ�� �Ǹ� �����ϴ� ��
/// </summary>
public class ChasingEnemy : Enemy {
    private AStarPathfinding pathfinding;
    [SerializeField] private List<Vector3> pathToPlayer;

    protected override void DecideNextAction() {
        pathToPlayer = pathfinding?.FindPath(transform.position, GameManager.PlayerReference.transform.position);
        nextAction = () => {
            if (pathToPlayer != null && pathToPlayer.Count > 2) {
                Move(pathToPlayer[1]);
            }
        };
        if (pathToPlayer != null && pathToPlayer.Count == 2) {
            AttackPreTurn(GameManager.PlayerReference.transform.position, 5);
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new AStarPathfinding(31, 31);
        pathToPlayer = pathfinding?.FindPath(transform.position, GameManager.PlayerReference.transform.position);
    }
}
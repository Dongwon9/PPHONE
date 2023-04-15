using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �Ѿƿͼ� �Ÿ��� 1ĭ�� �Ǹ� �����ϴ� ��
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
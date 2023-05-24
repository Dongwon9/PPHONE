using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �Ѿƿͼ� �Ÿ��� 1ĭ�� �Ǹ� �����ϴ� ��
/// </summary>
public class ChasingEnemy : Enemy {
    private AStarPathfinding pathfinding;
    [SerializeField] private List<Vector3> pathToPlayer;

    protected override void DecideNextAction() {
        pathToPlayer = pathfinding?.FindPath(transform.position, Player.Instance.transform.position);
        nextAction = () => {
            //�ʿ��� �̵� ���̰� 2ĭ���� ������ �����δ�.
            if (pathToPlayer != null && pathToPlayer.Count > 2) {
                Invoke("ExecuteMove", movingTime / 2);
            }
        };
        //�ʿ��� �̵� ���̰� 2ĭ�̸� �����Ѵ�.
        if (pathToPlayer != null && pathToPlayer.Count == 2) {
            AttackPreTurn(Player.Instance.transform.position, 5);
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new AStarPathfinding(31, 31);
        pathToPlayer = pathfinding?.FindPath(transform.position, Player.Instance.transform.position);
    }

    private void ExecuteMove() {
        Move(pathToPlayer[1]);
    }
}
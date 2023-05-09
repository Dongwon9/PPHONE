using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy {
    [SerializeField] private int damage = 10;
    private List<Vector3> ExplosionPosList;
    [SerializeField] private int ExplosionRadius;
    private AStarPathfinding pathfinding;
    [SerializeField] private List<Vector3> pathToPlayer;
    [SerializeField] private GameObject RedSquare;
    private int turnsTillExplosion = -1;
    protected override void DecideNextAction() {
        if (turnsTillExplosion != -1) {
            //자폭 로직을 개시,3턴 후에 폭발한다.
            nextAction = () => {
                turnsTillExplosion -= 1;
                if (turnsTillExplosion == 0) {
                    foreach (Vector3 pos in ExplosionPosList) {
                        AttackPreTurn(transform.position + pos, damage, instant: true);
                    }
                    Destroy(gameObject);
                }
            };
        } else {
            //ChasingEnemy와 같은 위치추적 로직
            pathToPlayer = pathfinding?.FindPath(transform.position, GameManager.PlayerReference.transform.position);
            nextAction = () => {
                if (pathToPlayer != null && pathToPlayer.Count > 2) {
                    Move(pathToPlayer[1]);
                }
            };
            if (pathToPlayer != null && pathToPlayer.Count == 2) {
                //거리 2 이내까지 오면, 자폭로직으로 전환한다.
                nextAction = () => { };
                turnsTillExplosion = 2;
                foreach (Vector3 pos in ExplosionPosList) {
                    Instantiate(RedSquare, transform.position + pos, Quaternion.identity, transform);
                }
            }
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new AStarPathfinding(31, 31);
        pathToPlayer = pathfinding?.FindPath(transform.position, GameManager.PlayerReference.transform.position);
        ExplosionPosList = new();
        for (int x = -ExplosionRadius; x <= ExplosionRadius; x++) {
            for (int y = -(ExplosionRadius - Mathf.Abs(x)); y <= (ExplosionRadius - Mathf.Abs(x)); y++) {
                ExplosionPosList.Add(new Vector3(x, y));
            }
        }
    }
}
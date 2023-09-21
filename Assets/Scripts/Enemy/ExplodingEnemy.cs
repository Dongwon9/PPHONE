﻿using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy {
    /// <summary>
    /// 자폭모드 전환 후, 자폭할 때까지 걸리는 시간
    /// </summary>
    [SerializeField] private int explosionTime;
    private List<Vector3> ExplosionPosList;
    [SerializeField] private int ExplosionRadius;
    [SerializeField] private GameObject ExplosionSprite;
    private Path pathfinding;
    [SerializeField] private GameObject RedSquare;
    private int turnsTillExplosion = -1;

    protected override void TurnUpdate() {
        if (turnsTillExplosion != -1) {
            //자폭 로직을 개시,3턴 후에 폭발한다.
            turnsTillExplosion -= 1;
            if (turnsTillExplosion <= 0) {
                Explode();
            }
        } else {
            //ChasingEnemy와 같은 위치추적 로직
            pathfinding.FindPath(transform.position, Player.Position);
            if (pathfinding.PathExists && pathfinding.PathLength > 2) {
                Move(pathfinding.GetNextPos());
            }
            if (pathfinding.PathExists && pathfinding.PathLength <= 2) {
                //거리 2 이내까지 오면, 자폭로직으로 전환한다.
                turnsTillExplosion = explosionTime - 1;
                foreach (Vector3 pos in ExplosionPosList) {
                    Instantiate(RedSquare, transform.position + pos, Quaternion.identity, transform);
                }
            }
        }

        void Explode() {
            foreach (Vector3 pos in ExplosionPosList) {
                Attack(transform.position + pos, enemydata.Damage, Target.Player);
            }
            ExplosionSprite.transform.SetParent(null, true);
            ExplosionSprite.SetActive(true);
            Destroy(gameObject);
        }
    }

    protected override void OnEnable() {
        base.OnEnable();
        pathfinding = new Path();
        pathfinding.FindPath(transform.position, Player.Position);
        ExplosionSprite.SetActive(false);
        ExplosionPosList = new();
        for (int x = -ExplosionRadius; x <= ExplosionRadius; x++) {
            for (int y = -(ExplosionRadius - Mathf.Abs(x)); y <= (ExplosionRadius - Mathf.Abs(x)); y++) {
                ExplosionPosList.Add(new Vector3(x, y));
            }
        }
    }
}
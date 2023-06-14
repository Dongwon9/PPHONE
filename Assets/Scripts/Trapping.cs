using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Trapping : MonoBehaviour {
    public GameObject obstaclePrefab; // 장애물 프리팹
    public GameObject enemyPrefab; // 적 프리팹

    private int obstacleWidth = 4; // 장애물의 너비
    private int obstacleHeight = 3; // 장애물의 높이

    private void Start() {
        GenerateObstacleAndEnemy();
    }

    private void GenerateObstacleAndEnemy() {
        // 맵 중앙 좌표
        int centerX = (int)transform.position.x;
        int centerY = (int)transform.position.y;

        // 장애물 생성
        int obstacleStartX = centerX - obstacleWidth / 2;
        int obstacleEndX = obstacleStartX + obstacleWidth;
        int obstacleStartY = centerY - obstacleHeight / 2;
        int obstacleEndY = obstacleStartY + obstacleHeight;

        for (int x = obstacleStartX; x < obstacleEndX; x++) {
            for (int y = obstacleStartY; y < obstacleEndY; y++) {
                Instantiate(obstaclePrefab, transform.position + new Vector3(x, y, 0), Quaternion.identity, transform);
            }
        }

        // 적 생성
        Instantiate(enemyPrefab, transform.position + new Vector3(centerX, centerY, 0), Quaternion.identity, transform);
    }
}
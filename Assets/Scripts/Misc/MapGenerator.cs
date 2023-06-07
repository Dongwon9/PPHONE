using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {
    public GameObject enemyPrefab;
    public GameObject obstaclePrefab; // 장애물 프리팹
                                      // 적 프리팹

    private int obstacleHeight = 5;
    private int obstacleWidth = 5; // 장애물의 너비
                                   // 장애물의 높이

    private void Awake() {
        GenerateObstacleAndEnemy();
    }

    private void GenerateObstacleAndEnemy() {
        // 맵 중앙 좌표
        //Random.Range(int a,int b) :a 보다 크거나 같고 b보다 작은 정수를 무작위로 생성한다.
        int centerX = Random.Range(-1, 2) * 10;
        int centerY = Random.Range(-1, 2) * 10;

        // 장애물 생성
        int obstacleStartX = centerX - obstacleWidth / 2;
        int obstacleEndX = obstacleStartX + obstacleWidth;
        int obstacleStartY = centerY - obstacleHeight / 2;
        int obstacleEndY = obstacleStartY + obstacleHeight;

        for (int x = obstacleStartX; x < obstacleEndX; x++) {
            for (int y = obstacleStartY; y < obstacleEndY; y++) {
                if (x == obstacleStartX || y == obstacleStartY ||
                    x == obstacleEndX - 1 || y == obstacleEndY - 1) {
                    Instantiate(obstaclePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                }
            }
        }
        GameManager.Instance.UpdateWalkableGrid();
        // 적 생성
        Instantiate(enemyPrefab, new Vector3(centerX + 0.5f, centerY + 0.5f, 0), Quaternion.identity);
    }
}
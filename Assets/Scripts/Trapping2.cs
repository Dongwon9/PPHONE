using UnityEngine;

public class Trapping2 : MonoBehaviour {
    public GameObject obstaclePrefab; // 장애물 프리팹
    public GameObject enemyPrefab; // 적 프리팹

    private int obstacleWidth = 3; // 장애물의 너비
    private int obstacleHeight = 3; // 장애물의 높이

    private void Start() {
        GenerateObstacleAndEnemy();
    }

    private void GenerateObstacleAndEnemy() {
        // 맵 크기
        int mapWidth = 9;
        int mapHeight = 9;

        // 장애물 생성
        PlaceObstacleInCorner(0, 0);
        PlaceObstacleInCorner(mapWidth - obstacleWidth, 0);
        PlaceObstacleInCorner(0, mapHeight - obstacleHeight);
        PlaceObstacleInCorner(mapWidth - obstacleWidth, mapHeight - obstacleHeight);
    }

    //네 개의 모서리쪽에 장애물을 배치하는 방식
    //총 9x9칸 안에서 귀퉁이(4곳)에 3x3 크기의 장애물이 만들어진 후
    //각각의 장애물 안에 적이 1개씩 배치되는 방식
    private void PlaceObstacleInCorner(int startX, int startY) {
        for (int x = startX; x < startX + obstacleWidth; x++) {
            for (int y = startY; y < startY + obstacleHeight; y++) {
                Instantiate(obstaclePrefab, transform.position + new Vector3(x, y, 0), Quaternion.identity, transform);
            }
        }

        // 적 생성
        Instantiate(enemyPrefab, transform.position + new Vector3(startX + 1, startY + 1, 0), Quaternion.identity, transform);
    }
}
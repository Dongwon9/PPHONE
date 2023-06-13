using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject obstaclePrefab; // 장애물 프리팹을 연결할 변수
    public GameObject enemyPrefab; // 적 프리팹을 연결할 변수

    private const int mapSize = 9; // 맵의 크기
    private const int obstacleCount = 10; // 장애물 개수
    private const int obstacleSize = 7; // 장애물 크기

    private Vector2Int[] obstaclePositions; // 장애물 위치 배열

    private void Start() {
        GenerateObstacles();
        GenerateEnemies();
    }

    private void GenerateObstacles() {
        obstaclePositions = GetObstaclePositions();

        foreach (Vector2Int position in obstaclePositions) {
            // 장애물 생성
            Instantiate(obstaclePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
    }

    private Vector2Int[] GetObstaclePositions() {
        Vector2Int[] positions = new Vector2Int[obstacleCount];

        // 81칸 맵의 정중앙에 7x7 크기의 영역을 배치하기 위한 시작 위치 계산
        int startX = (mapSize - obstacleSize) / 2;
        int startY = (mapSize - obstacleSize) / 2;

        for (int i = 0; i < obstacleCount; i++) {
            positions[i] = new Vector2Int(startX, startY);
            startX++;
        }

        return positions;
    }

    private void GenerateEnemies() {
        int enemyCount = Random.Range(1, 4);

        for (int i = 0; i < enemyCount; i++) {
            Vector2Int position = GetRandomPosition();
            Instantiate(enemyPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
    }

    private Vector2Int GetRandomPosition() {
        int x = Random.Range(0, mapSize);
        int y = Random.Range(0, mapSize);
        return new Vector2Int(x, y);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class Bush : TurnActor {
    public GameObject obstaclePrefab; // 장애물 프리팹을 연결할 변수
    public GameObject enemyPrefab; // 적 프리팹을 연결할 변수
    public GameObject bushPrefab; // bush 장애물 프리팹을 연결할 변수

    private const int mapSize = 9; // 맵의 크기
    private const int obstacleCount = 10; // 장애물 개수
    private const int obstacleSize = 7; // 장애물 크기
    private const int bushCount = 4; // bush 장애물 개수
    private int turnCount = 0;
    private List<Vector2Int> bushPositions;
    private Vector2Int[] obstaclePositions; // 장애물 위치 배열

    protected override void DecideNextAction() {
        if (turnCount < 3) {
            nextAction = () => {
                turnCount++;
                if (turnCount >= 3) {
                    GenerateEnemies();
                    Destroy(gameObject);
                }
            };
        } else {
            nextAction = () => { };
        }
    }

    private void Start() {
        bushPositions = new List<Vector2Int>();
        GenerateObstacles();
        GenerateBushObstacles();
    }

    private void GenerateObstacles() {
        obstaclePositions = GetObstaclePositions();

        foreach (Vector2Int position in obstaclePositions) {
            // 장애물 생성
            Instantiate(obstaclePrefab, transform.position + new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
        GameManager.Instance.UpdateWalkableGrid();
    }

    private void GenerateBushObstacles() {
        Vector2Int[] bushPositions = GetBushPositions();

        foreach (Vector2Int position in bushPositions) {
            // bush 장애물 생성
            Instantiate(bushPrefab, transform.position + new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
    }

    // bush 장애물의 위치를 가져온 후, 각 위치에 bushPrefab을 인스턴스화하여 bush 장애물을 생성
    // 적이 들어있는 bush 장애물에는 enemyPrefab도 함께 생성됨
    private Vector2Int[] GetObstaclePositions() {
        Vector2Int[] positions = new Vector2Int[obstacleCount];

        // 81칸 맵의 정중앙에 7x7 크기의 영역을 배치하기 위한 시작 위치 계산
        //int startX = (mapSize - obstacleSize) / 2;
        //int startY = (mapSize - obstacleSize) / 2;

        for (int i = 0; i < obstacleCount; i++) {
            positions[i] = new Vector2Int(Random.Range(0, mapSize), Random.Range(0, mapSize));
        }

        return positions;
    }

    private Vector2Int[] GetBushPositions() {
        Vector2Int[] positions = new Vector2Int[bushCount];

        for (int i = 0; i < bushCount; i++) {
            // 랜덤한 위치를 생성하여 bush 장애물의 위치로 지정
            positions[i] = GetRandomPosition();
            bushPositions.Add(positions[i]);
        }

        return positions;
    }

    private void GenerateEnemies() {
        int enemyCount = Random.Range(1, 4);

        for (int i = 0; i < enemyCount; i++) {
            Vector2Int position = bushPositions[Random.Range(0, bushPositions.Count)];
            Instantiate(enemyPrefab, transform.position + new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
    }

    private Vector2Int GetRandomPosition() {
        int x = Random.Range(0, mapSize);
        int y = Random.Range(0, mapSize);
        return new Vector2Int(x, y);
    }
}
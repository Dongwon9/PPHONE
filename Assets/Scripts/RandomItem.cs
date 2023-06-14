using UnityEngine;

public class RandomItem : MonoBehaviour {
    public GameObject obstaclePrefab;  // 장애물 프리팹
    public GameObject itemPrefab;      // 아이템 프리팹

    private const int obstacleSizeX = 5;  // 장애물 가로 크기
    private const int obstacleSizeY = 3;  // 장애물 세로 크기
    private const int itemPosX = 4;       // 아이템 가로 위치
    private const int itemPosY = 4;       // 아이템 세로 위치

    private void Start() {
        GenerateMap();
    }

    private void GenerateMap() {
        // 중간에 가로 5칸, 세로 3칸의 장애물 생성
        int obstacleStartX = (9 - obstacleSizeX) / 2;
        int obstacleStartY = (9 - obstacleSizeY) / 2;
        for (int x = obstacleStartX; x < obstacleStartX + obstacleSizeX; x++) {
            for (int y = obstacleStartY; y < obstacleStartY + obstacleSizeY; y++) {
                // 5x3칸의 장애물을 2등분하는 벽 생성
                Instantiate(obstaclePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
            }
        }

        // 각각의 방에 아이템 생성
        Instantiate(itemPrefab, new Vector3(itemPosX - 1, itemPosY, 0), Quaternion.identity, transform);
        Instantiate(itemPrefab, new Vector3(itemPosX + 1, itemPosY, 0), Quaternion.identity, transform);
        GameManager.Instance.UpdateWalkableGrid();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {
    public GameObject enemyPrefab;
    public GameObject obstaclePrefab; // ��ֹ� ������
                                      // �� ������

    private int obstacleHeight = 5;
    private int obstacleWidth = 5; // ��ֹ��� �ʺ�
                                   // ��ֹ��� ����

    private void Awake() {
        GenerateObstacleAndEnemy();
    }

    private void GenerateObstacleAndEnemy() {
        // �� �߾� ��ǥ
        //Random.Range(int a,int b) :a ���� ũ�ų� ���� b���� ���� ������ �������� �����Ѵ�.
        int centerX = Random.Range(-1, 2) * 10;
        int centerY = Random.Range(-1, 2) * 10;

        // ��ֹ� ����
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
        // �� ����
        Instantiate(enemyPrefab, new Vector3(centerX + 0.5f, centerY + 0.5f, 0), Quaternion.identity);
    }
}
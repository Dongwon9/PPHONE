using System.Collections.Generic;
using UnityEngine;

public class Thorns : TurnActor {
    public GameObject obstaclePrefab;  // 장애물 프리팹
    public int damageAmount = 3;  // 장애물과의 충돌 시 감소되는 체력

    private List<Transform> obstacleList = new List<Transform>();  // 현재 맵에 존재하는 모든 장애물 리스트
    private int[] obstacleColumns = new int[] { 2, 5, 8 };  // 기준 칼럼 배열

    private int turnCount = 0;  // 캐릭터의 움직임 턴 수
    private Vector3 characterPosition;  // 캐릭터의 위치

    protected override void TurnUpdate() {
        ThornsAction();
    }

    private void ThornsAction() {
        // 캐릭터의 움직임 턴 수 증가
        turnCount++;

        // 3턴마다 장애물 생성, 4턴마다 장애물 제거
        if (turnCount % 4 == 0) {
            // 기준 칼럼마다 장애물 생성
            for (int i = 0; i < obstacleColumns.Length; i++) {
                // 해당 칼럼에 이미 장애물이 생성되어 있다면 생성하지 않음
                if (obstacleList.Exists(o => o.position.x == obstacleColumns[i])) {
                    continue;
                }

                // 해당 칼럼에 장애물 생성
                Vector3 spawnPosition = transform.position + new Vector3(obstacleColumns[i], 9f, 0f);
                GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, transform);
                obstacleList.Add(obstacle.transform);
            }
        } else if (turnCount % 4 == 3) {
            // 모든 장애물 제거
            for (int i = 0; i < obstacleList.Count; i++) {
                Destroy(obstacleList[i].gameObject);
            }
            obstacleList.Clear();
        }

        // 캐릭터와 장애물의 충돌 체크
        CheckCollision();
    }

    private void CheckCollision() {
        // 캐릭터 위치와 장애물 위치 비교
        for (int i = 0; i < obstacleList.Count; i++) {
            Transform obstacle = obstacleList[i];
            if (obstacle.position == characterPosition) {
                // 캐릭터와 장애물의 위치가 같을 때(캐릭터와 장애물의 좌표값이 정확히 일치할때) 체력 감소
                DecreaseHealth();
            }
        }
    }

    private void DecreaseHealth() {
        // 체력 감소
        // Debug.Log를 사용하여 체력 감소를 확인
        Debug.Log("Health decreased by " + damageAmount);
    }
}
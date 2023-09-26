using System.Collections.Generic;
using UnityEngine;

/// <summary> 주어진 프리셋들을 각 방에 무작위로 배치하는 스크립트 </summary>
public class MapGenerator : MonoBehaviour {
    [SerializeField] private GameObject BossRoom;
    [SerializeField] private GameObject obstacleSecuring;
    private void Awake() {
        List<GameObject> RoomPresets = new();
        for (int i = 0; i < transform.childCount; i++) {
            RoomPresets.Add(transform.GetChild(i).gameObject);
        }
        //중앙을 제외한 각 방
        List<(int x, int y)> coordList = new() {
            (-1,-1),(-1,0),(-1,1),
            (0,-1),         (0,1),
            (1,-1), (1,0),  (1,1)
        };
        for(int i = 0; i < Random.Range(0, 1);i++) {
        GameObject obstacle = Instantiate(obstacleSecuring, transform);
        obstacle.SetActive(false);
         RoomPresets.Add(obstacle);
         }
        //프리셋이 7개보다 많으면, 무작위로 제거해서 7개로 만든다
        //(8번째는 보스방이니까)
        while (RoomPresets.Count > 7) {
            int removeTarget = Random.Range(0, RoomPresets.Count);
            GameObject obj = RoomPresets[removeTarget];
            RoomPresets.RemoveAt(removeTarget);
            Destroy(obj);
        }
        //프리셋 배치
        foreach (GameObject obj in RoomPresets) {
            int i = Random.Range(0, coordList.Count);
            (int x, int y) = coordList[i];
            coordList.RemoveAt(i);
            obj.transform.position = new Vector3(x * 10, y * 10);
            obj.SetActive(true);
            if(coordList.Count == 1) {
                break;
            } 
        }
        BossRoom.transform.position = new Vector3(coordList[0].x * 10, coordList[0].y * 10);
        BossRoom.SetActive(true);
        GameManager.Instance.UpdateWalkableGrid();
    }
}
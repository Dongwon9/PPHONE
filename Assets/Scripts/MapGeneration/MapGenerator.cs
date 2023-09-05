using System.Collections.Generic;
using UnityEngine;

/// <summary> 주어진 프리셋들을 각 방에 무작위로 배치하는 스크립트 </summary>
public class MapGenerator : MonoBehaviour {
    private void Awake() {
        List<GameObject> RoomPresets = new();
        for (int i = 0; i < transform.childCount; i++) {
            RoomPresets.Add(transform.GetChild(i).gameObject);
        }
        //중앙을 제외한 각 방
        List<(int x, int y)> coordList = new() {
            (-1,-1),(-1,0),(-1,1),
            (0,-1),        (0,1),
            (1,-1),(1,0),(1,1)
        };
        //프리셋이 8개보다 많으면, 무작위로 제거해서 8개로 만든다
        while (RoomPresets.Count > 8) {
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
            obj.transform.Translate(new Vector3(x * 10, y * 10));
            obj.SetActive(true);
        }
        GameManager.Instance.UpdateWalkableGrid();
    }
}
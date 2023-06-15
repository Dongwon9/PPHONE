using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private List<GameObject> RoomPresets;
    private void Awake() {
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
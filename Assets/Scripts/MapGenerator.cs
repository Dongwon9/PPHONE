using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private List<GameObject> RoomPresets;
    private List<int[]> Appeared;
    // Start is called before the first frame update
    private void Awake() {
        Appeared = new List<int[]>();
        //for (int i = 0; i < transform.childCount; i++) {
        //    RoomPresets.Add(transform.GetChild(i).gameObject);
        //}
        //while (RoomPresets.Count > 8) {
        //    int removeTarget = Random.Range(0, RoomPresets.Count);
        //    Destroy(RoomPresets[removeTarget]);
        //    RoomPresets.RemoveAt(removeTarget);
        //}
        foreach (GameObject obj in RoomPresets) {
            Vector3 offset;
            while (true) {
                int x = Random.Range(-1, 2);
                int y = Random.Range(-1, 2);
                int[] xy = new int[] { x, y };
                if (!Appeared.Contains(xy) && !(x == 0 && y == 0)) {
                    Appeared.Add(xy);
                    offset = new Vector3(x * 10, y * 10);
                    break;
                }
            }
            obj.transform.Translate(offset);
            obj.SetActive(true);
            GameManager.Instance.UpdateWalkableGrid();
        }

        // Update is called once per frame
    }
}
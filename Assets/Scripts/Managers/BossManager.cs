using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {
    [SerializeField] private List<GameObject> BossArray = new();
    private List<int> BossOrder = new();
    [SerializeField] private GameObject finalBoss;
    public static BossManager Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        if (BossOrder.Count < BossArray.Count) {
            BossOrder.Clear();
            for (int i = 0; i < BossArray.Count; i++) {
                BossOrder.Add(Random.Range(0, BossArray.Count));
            }
        }
    }

    public GameObject GetBoss() {
        if (GameManager.Instance.stageNumber == 3) {
            return finalBoss;
        }
        return BossArray[BossOrder[GameManager.Instance.stageNumber - 1]];
    }
}
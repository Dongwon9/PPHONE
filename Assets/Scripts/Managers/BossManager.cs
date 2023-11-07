using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {
    [SerializeField] private List<GameObject> BossArray = new();
    private bool bossShuffled = false;
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
        if (!bossShuffled) {
            //Fisher–Yates 셔플알고리즘:
            //보스의 순서를 무작위로 섞는다.
            for (int i = 0; i < BossArray.Count - 1; i++) {
                int j = Random.Range(i, BossArray.Count);
                (BossArray[j], BossArray[i]) = (BossArray[i], BossArray[j]);
            }
            bossShuffled = true;
        }
    }

    public GameObject GetBoss() {
        if (GameManager.Instance.StageNumber == GameManager.FinalStageNumber) {
            return finalBoss;
        }
        return BossArray[GameManager.Instance.StageNumber - 1];
    }
}
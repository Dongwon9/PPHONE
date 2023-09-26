using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppearing : MonoBehaviour {
    [SerializeField] private List<GameObject> minions;
    [SerializeField] private GameObject boss;
    private void OnEnable() {
        boss.SetActive(false);
    }
    private void Update() {
        int aliveCount = 0;
        foreach(GameObject go in minions) {
            if (go != null) {
                aliveCount += 1;
            }
        }
        if(boss == null) {
            Destroy(gameObject); 
            return;
        }
        if(aliveCount == 0) {
            boss.SetActive(true);
        }
    }
}


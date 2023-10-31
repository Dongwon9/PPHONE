using UnityEngine;

public class SplitingBossManager : MonoBehaviour {
    public int enemyLeft = 1;
    private void Update() {
        if (enemyLeft == 0) {
            Destroy(gameObject);
        }
    }
}
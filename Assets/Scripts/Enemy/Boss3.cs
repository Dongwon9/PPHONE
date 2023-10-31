using UnityEngine;

public class Boss3 : Boss1 {
    public int size = 3;
    public SplitingBossManager splitingBossManager;

    public void ScaleSize() {
        HP = (int)(HP * Mathf.Pow(0.5f, 3 - size));
        transform.localScale *= Mathf.Pow(2.0f / 3.0f, 3 - size);
    }

    public override void TakeDamage(int damage) {
        HP -= damage;
        if (HP <= 0) {
            splitingBossManager.enemyLeft -= 1;
            if (size > 1) {
                Boss3 minion = Instantiate(gameObject, transform.position + Vector3.right, Quaternion.identity).GetComponent<Boss3>();
                minion.size = size - 1;
                splitingBossManager.enemyLeft += 1;
                minion.ScaleSize();
                minion.gameObject.SetActive(true);
                minion = Instantiate(gameObject, transform.position + Vector3.left, Quaternion.identity).GetComponent<Boss3>();
                minion.size = size - 1;
                splitingBossManager.enemyLeft += 1;
                minion.ScaleSize();
                minion.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
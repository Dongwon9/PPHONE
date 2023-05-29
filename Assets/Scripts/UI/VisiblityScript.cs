using UnityEngine;
public class VisiblityScript : MonoBehaviour {
    private SpriteMask[] spriteMasks;
    void Awake() {
        spriteMasks = GetComponentsInChildren<SpriteMask>();
    }

    // Update is called once per frame
    void Update() {
        foreach (var spriteMask in spriteMasks) {
            Vector2 distAndDir = spriteMask.transform.position - transform.position;
            //벽 뒤에 1칸까지는 보이는 칸이니, 대상 칸의 2칸 앞에 벽이 있어야 그 칸이 보이지 않는다.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, distAndDir, Mathf.Max(distAndDir.magnitude - 2f, 0), LayerMask.GetMask("Wall"));
            spriteMask.enabled = (hit.collider == null);
        }
    }
}

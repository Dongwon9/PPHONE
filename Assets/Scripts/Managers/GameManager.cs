using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public Grid<bool> WalkableGrid { get; private set; }
    public static GameObject playerReference;
    public Image UIWindow;

    ////그리드에 넣을 오브젝트 : 플레이어, 적, 벽
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        playerReference = GameObject.FindGameObjectWithTag("Player");
        WalkableGrid = new Grid<bool>(31, 31, (grid, x, y) => true);
        RaycastHit2D hit2D;
        for (int x = -16; x <= 16; x++) {
            for (int y = -16; y <= 16; y++) {
                hit2D = Physics2D.Raycast(new Vector2(x + 0.5f, y + 0.5f), Vector2.zero, 0.0f, LayerMask.GetMask("Wall"));
                Debug.Log(x + "," + y + ": " + hit2D.collider);
                WalkableGrid.SetGridObject(x, y, !hit2D);
            }
        }
    }
}
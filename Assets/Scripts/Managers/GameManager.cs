using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static Grid<bool> WalkableGrid;
    public Image UIWindow;
    private static GameObject playerReference = null;
    [SerializeField] private bool WalkableGridDebugDisplay;
    public static GameManager Instance { get; private set; }

    public static GameObject PlayerReference {
        get {
            if (playerReference == null) {
                playerReference = GameObject.FindGameObjectWithTag("Player");
            }
            if (playerReference == null) {
                Debug.LogError("플레이어를 찾을 수 없습니다!");
            }
            return playerReference;
        }
        set { playerReference = value; }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        playerReference = GameObject.FindGameObjectWithTag("Player");
        WalkableGrid = new Grid<bool>(31, 31, (grid, x, y) => true, WalkableGridDebugDisplay);
        RaycastHit2D hit2D;
        for (int x = -16; x <= 16; x++) {
            for (int y = -16; y <= 16; y++) {
                hit2D = Physics2D.Raycast(new Vector2(x + 0.5f, y + 0.5f), Vector2.down, 0.1f, LayerMask.GetMask("Wall"));
                //Debug.Log(x + "," + y + ": " + hit2D.collider);
                WalkableGrid.SetGridObject(x + 15, y + 15, !hit2D);
            }
        }
    }
}
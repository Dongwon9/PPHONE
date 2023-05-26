using UnityEngine;

public enum Direction { Left, Up, Right, Down };

//디자인패턴: 싱글톤
public class GameManager : MonoBehaviour {
    //맵의 걸을 수 있는 칸과 없는 칸을 저장하는 격자
    public static Grid<bool> WalkableGrid;
    //public Image UIWindow;
    //플레이어 스크립트에 대한 참조는 이걸로 하면 된다.
    private static Player playerReference = null;
    [SerializeField] private bool WalkableGridDebugDisplay;
    public static GameManager Instance { get; private set; }

    public static Player PlayerReference {
        get {
            if (playerReference == null) {
                playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        playerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        WalkableGrid = new Grid<bool>(31, 31, (grid, x, y) => true, WalkableGridDebugDisplay);
        RaycastHit2D hit2D;
        //(-16,-16)에서(16,16)까지의 맵의 모든 칸에 하나싹 레이캐스트를 해서
        //거기에 벽이 있는지를 판단한다.
        for (int x = -16; x <= 16; x++) {
            for (int y = -16; y <= 16; y++) {
                hit2D = Physics2D.Raycast(new Vector2(x + 0.5f, y + 0.5f), Vector2.down, 0.1f, LayerMask.GetMask("Wall"));
                WalkableGrid.SetGridObject(x + 15, y + 15, !hit2D);
            }
        }
    }
}
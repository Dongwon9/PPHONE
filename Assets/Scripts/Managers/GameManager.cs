using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private bool WalkableGridDebugDisplay;
    public GameObject redSquare;
    public int stageCount;
    public static GameManager Instance { get; private set; }
    /// <summary>맵의 걸을 수 있는 칸과 없는 칸을 저장하는 격자</summary>
    public Grid WalkableGrid { get; private set; }
    private void Awake() {
        Instance = this;
        WalkableGrid = new Grid(-15, -15, 15, 15);
        UpdateWalkableGrid();
        if (WalkableGridDebugDisplay) {
            DebugShowWalkableGrid();
        }
    }

    public float GetStageHPModifier() {
        switch (stageCount) {
            case 1:
                return 1.0f;

            case 2:
                return 1.5f;

            case 3:
                return 2.0f;

            default:
                return 1.0f;
        }
    }

    private void DebugShowWalkableGrid() {
        for (int x = -15; x <= 15; x++) {
            for (int y = -15; y <= 15; y++) {
                if (!WalkableGrid.GetNode(x, y).isWalkable) {
                    Instantiate(redSquare, new Vector3(x, y), Quaternion.identity, this.transform);
                }
            }
        }
    }

    /// <summary>walkableGrid 전체를 업데이트하는데 사용한다.</summary>
    public void UpdateWalkableGrid() {
        //(-16,-16)에서(16,16)까지의 맵의 모든 칸에 하나싹 레이캐스트를 해서
        //거기에 벽이 있는지를 판단한다.
        RaycastHit2D hit2D;
        for (int x = -15; x <= 15; x++) {
            for (int y = -15; y <= 15; y++) {
                hit2D = Physics2D.Raycast(new Vector2(x, y), Vector2.down, 0.1f, LayerMask.GetMask("Wall"));
                WalkableGrid.SetWalkable(x, y, !hit2D);
            }
        }
    }
}
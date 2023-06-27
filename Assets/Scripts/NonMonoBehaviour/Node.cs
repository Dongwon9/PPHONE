public class Node {
    /// <summary>이 노드의 좌표</summary>
    public readonly int x, y;
    /// <summary>출발노드에서 여기까지의 거리</summary>
    public int gCost = 999999999;
    ///<summary>여기부터 도착노드까지의 예상 거리</summary>
    public int hCost = 999999999;
    /// <summary>이 노드는 걸을 수 있는 노드인가?</summary>
    public bool isWalkable = true;
    /// <summary>이 노드로 경로탐색해 온 노드 </summary>
    public Node PrevNode;
    /// <summary>
    /// GCost+HCost<br></br>
    /// 이게 최소인 경로를 찾는다.
    /// </summary>
    public int FCost => gCost + hCost;
    public Node(int x, int y) {
        this.x = x;
        this.y = y;
    }
}
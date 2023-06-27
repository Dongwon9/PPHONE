using System.Collections.Generic;
using UnityEngine;

public class Grid {
    private readonly Node[,] GridNode;
    public int LeftBottomX { get; private set; }
    public int LeftBottomY { get; private set; }
    public int RightTopX { get; private set; }
    public int RightTopY { get; private set; }
    public int Width => RightTopX - LeftBottomX;
    public int Height => RightTopY - LeftBottomY;
    public Grid(int leftBottomX, int leftBottomY, int rightTopX, int rightTopY) {
        GridNode = new Node[rightTopX - leftBottomX + 1, rightTopY - leftBottomY + 1];
        for (int i = leftBottomX; i <= rightTopX; i++) {
            for (int j = leftBottomY; j <= rightTopY; j++) {
                GridNode[i - leftBottomX, j - leftBottomY] = new Node(i, j);
            }
        }
        LeftBottomX = leftBottomX;
        LeftBottomY = leftBottomY;
        RightTopX = rightTopX;
        RightTopY = rightTopY;
    }

    public Node GetNode(int x, int y) {
        return GridNode[x - LeftBottomX, y - LeftBottomY];
    }

    public List<Node> GetAllNode() {
        List<Node> nodes = new();
        for (int i = LeftBottomX; i <= RightTopX; i++) {
            for (int j = LeftBottomY; j <= RightTopY; j++) {
                nodes.Add(GetNode(i, j));
            }
        }
        return nodes;
    }

    public void SetWalkable(int x, int y, bool walkable) {
        GetNode(x, y).isWalkable = walkable;
    }

    public void SetWalkable(Vector3 pos, bool walkable) {
        SetWalkable(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), walkable);
    }

    /// <summary>
    /// 맵에 있는 모든 노드의 HCost를 계산한다.
    /// </summary>
    /// <param name="end"></param>
    public void SetHCostFull(Node end) {
        foreach (Node node in GridNode) {
            node.hCost = Mathf.Abs(node.x - end.x) + Mathf.Abs(node.y - end.y);
        }
    }
}
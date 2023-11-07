using System.Collections.Generic;
using UnityEngine;

public class Path {
    //경로 전체
    private List<Node> FullPath = new();
    public (int x, int y)[] NeighborOffset = { (0, 1), (1, 0), (-1, 0), (0, -1) };
    /// <summary>경로가 존재하는가?</summary>
    public bool PathExists => FullPath.Count >= 1;
    /// <summary>경로의 길이</summary>
    public int PathLength => FullPath.Count - 1;
    public Vector3 GetNextPos() {
        Node next;
        if (FullPath.Count < 2) {
            next = FullPath[0];
        } else {
            next = FullPath[1];
        }
        return new Vector3(next.x, next.y);
    }

    public List<Vector3> GetFullPathVector() {
        if (!PathExists) {
            return null;
        }
        List<Vector3> res = new();
        foreach (Node node in FullPath) {
            res.Add(new Vector3(node.x, node.y));
        }
        return res;
    }

    /// <summary>
    /// 경로 탐색 알고리즘: A* 알고리즘
    /// </summary>
    public void FindPath(int startx, int starty, int endx, int endy) {
        Grid nodeGrid = GameManager.Instance.WalkableGrid;
        //나중에 탐색할 노드
        List<Node> OpenList = new();
        //탐색을 완료한 노드
        List<Node> ClosedList = new();
        //초기화
        foreach (Node node in nodeGrid.GetAllNode()) {
            node.PrevNode = null;
            node.gCost = 0;
        }
        Node startNode = nodeGrid.GetNode(startx, starty);
        Node endNode = nodeGrid.GetNode(endx, endy);
        nodeGrid.SetHCostFull(endNode);
        OpenList.Add(startNode);
        startNode.gCost = 0;
        //탐색 시작
        while (OpenList.Count > 0) {
            //OpenList에서 Fcost가 가장 낮은 노드를 찾는다.
            Node currentNode = FindLowestFCostNode();

            //도착 노드를 찾았다.
            if (currentNode == nodeGrid.GetNode(endx, endy)) {
                FullPath = GetFullPath();
                return;
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);
            //이웃 노드들을 탐색한다.
            foreach (Node node in getNeighbor(currentNode)) {
                //이웃 노드를 이미 탐색했다.
                if (ClosedList.Contains(node)) {
                    continue;
                }
                //이웃 노드가 걸을 수 있는 노드가 아니다.
                if (!node.isWalkable) {
                    ClosedList.Add(currentNode);
                    continue;
                }
                if (!OpenList.Contains(node)) {
                    //이웃 노드가 OpenList에 없으면 OpenList에 넣는다.
                    OpenList.Add(node);
                    node.PrevNode = currentNode;
                    node.gCost = CalculateGCost(node);
                } else if (node.gCost > currentNode.gCost + 10) {
                    //이웃 노드가 OpenList에 있으면, 더 낮은 gCost를 찾으면
                    //gCost를 업데이트하고, 이웃 노드의 이전 노드를 현재 노드로 한다.
                    node.gCost = currentNode.gCost + 10;
                    node.PrevNode = currentNode;
                }
            }
        }
        //경로가 없다.
        FullPath.Clear();
        return;

        //로컬 함수들
        int CalculateGCost(Node node) {
            return node.PrevNode.gCost + 1;
        }
        Node FindLowestFCostNode() {
            Node ReturnNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++) {
                if (OpenList[i].FCost < ReturnNode.FCost) {
                    ReturnNode = OpenList[i];
                }
            }
            return ReturnNode;
        }
        List<Node> getNeighbor(Node node) {
            List<Node> neighborList = new();
            foreach (var (x, y) in NeighborOffset) {
                if (node.x + x <= nodeGrid.RightTopX &&
                    node.x + x >= nodeGrid.LeftBottomX &&
                    node.y + y <= nodeGrid.RightTopY &&
                    node.y + y >= nodeGrid.LeftBottomY) {
                    neighborList.Add(nodeGrid.GetNode(node.x + x, node.y + y));
                }
            }
            //한쪽 방향만을 우선하는 것을 막기 위해 이웃 노드 리스트를 섞는다.
            for (int i = 0; i < neighborList.Count - 1; i++) {
                int j = Random.Range(i, neighborList.Count);
                (neighborList[j], neighborList[i]) = (neighborList[i], neighborList[j]);
            }
            return neighborList;
        }
        List<Node> GetFullPath() {
            List<Node> path = new();
            Node node = endNode;
            while (node != null) {
                path.Add(node);
                node = node.PrevNode;
                if (path.Count > 50) {
                    Debug.LogError("!!!");
                }
            }
            path.Reverse();
            return path;
        }
    }

    /// <summary>
    /// 경로 탐색 알고리즘: A* 알고리즘
    /// </summary>
    public void FindPath(Vector3 startpos, Vector3 endpos) {
        FindPath(Mathf.RoundToInt(startpos.x), Mathf.RoundToInt(startpos.y),
            Mathf.RoundToInt(endpos.x), Mathf.RoundToInt(endpos.y));
    }
}
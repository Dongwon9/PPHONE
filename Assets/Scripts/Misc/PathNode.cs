/*
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

public class PathNode {
    private Grid<PathNode> grid;
    public PathNode cameFromNode;
    public int fCost;
    public int gCost;
    public int hCost;
    public bool isWalkable;
    public int x;
    public int y;
    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void DetermineIsWalkable() {
        SetIsWalkable(GameManager.WalkableGrid.GetGridObject(x, y));
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return x + "," + y;
    }
}
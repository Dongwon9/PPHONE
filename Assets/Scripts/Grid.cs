using System;
using UnityEngine;

public class Grid<TGridObject> {

    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private const float cellSize = 1f;

    private TGridObject[,] gridArray;

    private int height;

    private Vector3 originPosition;

    private int width;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public Grid(int width, int height, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        this.width = width;
        this.height = height;

        gridArray = new TGridObject[width, height];
        originPosition = new Vector3(-31, -31);
        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public float GetCellSize() {
        return cellSize;
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        } else {
            Debug.Log("Invalid coordinate in GetGridObject!");
            return default;
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }

    public int GetHeight() {
        return height;
    }

    public Vector3 GetOrigin() {
        return originPosition;
    }

    public int GetWidth() {
        return width;
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void TriggerGridObjectChanged(int x, int y) {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }
}
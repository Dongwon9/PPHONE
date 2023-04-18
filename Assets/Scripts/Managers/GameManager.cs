using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public Grid<GameObject> GameMapGrid { get; private set; }
    public static GameObject playerReference;
    public Image UIWindow;

    ////�׸��忡 ���� ������Ʈ : �÷��̾�, ��, ��
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        playerReference = GameObject.FindGameObjectWithTag("Player");
        GameMapGrid = new Grid<GameObject>(63, 63, (grid, x, y) => null);

        GameMapGrid.SetGridObject(playerReference.transform.position, playerReference);

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            GameMapGrid.SetGridObject(enemy.transform.position, enemy);
        }
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall")) {
            GameMapGrid.SetGridObject(wall.transform.position, wall);
        }
    }
}
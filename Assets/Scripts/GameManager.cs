using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameObject playerReference;
    private Grid<GameObject> GameMapGrid;

    ////�׸��忡 ���� ������Ʈ : �÷��̾�, ��, ��
    private void Awake() {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        //GameMapGrid = new Grid<GameObject>(63, 63);
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //GameMapGrid.SetGridObject(player.transform.position, player);
        //foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
        //    GameMapGrid.SetGridObject(enemy.transform.position, enemy);
        //}
        //foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall")) {
        //    GameMapGrid.SetGridObject(wall.transform.position, wall);
        //}
    }
}
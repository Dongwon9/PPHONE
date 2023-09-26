using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy
{
    private List<Vector3> bossTraces = new List<Vector3>();// 보스의 흔적 위치 리스트
    [SerializeField] private GameObject PurpleSquare;
    private List<GameObject> PurpleSquareList = new List<GameObject>(); 
    private readonly Path pathFinding = new();
    private int counter = 0;
    protected override void Awake() {
        base.Awake();
    }
    protected override void TurnUpdate() {
        counter += 1;
        if (counter == 2) {
            if (!bossTraces.Contains(transform.position)) {
                bossTraces.Add(transform.position);
                PurpleSquareList.Add(Instantiate(PurpleSquare, transform.position, Quaternion.identity));
            }
            counter = 0;
        }
        pathFinding.FindPath(transform.position, Player.Position);
        Move(pathFinding.GetNextPos());

        // 플레이어의 위치를 확인하고, 보스의 흔적 위치와 일치하는지 확인한다.
        foreach (Vector3 tracePosition in bossTraces){
            Attack(tracePosition, 1, Target.Player); 
        }
    }
    private void OnDestroy() {
        foreach(GameObject go in PurpleSquareList) {
            Destroy(go);
        }
    }
}
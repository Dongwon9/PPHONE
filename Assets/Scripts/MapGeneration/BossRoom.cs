using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour {
    [SerializeField] private GameObject BossRoomWalls;
    private bool bossActive = false;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject NextStagePortal;
    private void OnEnable() {
        BossRoomWalls.SetActive(false);
        boss.SetActive(false);
        NextStagePortal.SetActive(false);
    }
    void Update() {
        //플레이어가 방의 중앙에서 3칸 이내의 거리에 오면
        if(!bossActive && 
            Mathf.Abs( (Player.Position - transform.position).magnitude)<=5.0f) {        
            bossActive = true;
            //보스 소환, 방 문 닫힘
            BossRoomWalls.SetActive(true);           
            boss.SetActive(true);
            
        }
        //보스가 죽었으면
        if(boss == null) {
            //방 문 다시열림, 포탈 소환
            Destroy(BossRoomWalls);
            NextStagePortal.SetActive(true);
        }
    }
}

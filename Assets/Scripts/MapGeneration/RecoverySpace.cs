﻿using UnityEngine;

public class RecoverySpace : TurnActor {
    public int recoveryAmount = 7; // 체력 회복량

    private bool IsCharacterInRecoverySpace() {
        // 맵 중앙의 6x6 칸인지 확인
        //Vector3 characterPosition = Player.Position;
        //int recoverySpaceSize = 6; // 공간 크기

        //int startX = (int)transform.position.x + Mathf.RoundToInt(characterPosition.x) - recoverySpaceSize / 2;
        //int startY = (int)transform.position.y + Mathf.RoundToInt(characterPosition.y) - recoverySpaceSize / 2;

        //int characterX = Mathf.RoundToInt(characterPosition.x);
        //int characterY = Mathf.RoundToInt(characterPosition.y);

        //return (characterX >= startX && characterX < startX + recoverySpaceSize &&
        //        characterY >= startY && characterY < startY + recoverySpaceSize);
        return Mathf.Abs((transform.position - Player.Position).magnitude) <= 4.0f;
    }

    protected override void DecideNextAction() {
        nextAction = CharacterMoved;
    }

    public void CharacterMoved() {
        if (IsCharacterInRecoverySpace()) {
            Player.Instance.HealHP(recoveryAmount);
            Debug.Log("Healed!");
        }
    }
}
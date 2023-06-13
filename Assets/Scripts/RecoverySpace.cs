using UnityEngine;

public class RecoverySpace : MonoBehaviour {
    public int recoveryAmount = 7; // 체력 회복량

    private Character character;

    public void CharacterMoved() {
        if (IsCharacterInRecoverySpace()) {
            character.RecoverHealth(recoveryAmount);
        }
    }

    private void Start() {
        character = FindObjectOfType<Character>();
    }

    private bool IsCharacterInRecoverySpace() {
        // 맵 중앙의 6x6 칸인지 확인
        Vector3 characterPosition = character.transform.position;
        int recoverySpaceSize = 6; // 공간 크기

        int startX = Mathf.RoundToInt(characterPosition.x) - recoverySpaceSize / 2;
        int startY = Mathf.RoundToInt(characterPosition.y) - recoverySpaceSize / 2;

        int characterX = Mathf.RoundToInt(characterPosition.x);
        int characterY = Mathf.RoundToInt(characterPosition.y);

        return (characterX >= startX && characterX < startX + recoverySpaceSize &&
                characterY >= startY && characterY < startY + recoverySpaceSize);
    }
}

public class Character : MonoBehaviour {
    public int maxHealth = 100; // 최대 체력
    private int currentHealth; // 현재 체력

    private RecoverySpace recoverySpace;

    public void Move() {
        // 캐릭터 이동 로직

        // 캐릭터가 이동할 때마다 체력 회복 체크
        recoverySpace.CharacterMoved();
    }

    public void RecoverHealth(int amount) {
        // 체력 회복
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        Debug.Log("체력 회복 중, 현재 체력: " + currentHealth);
    }

    private void Start() {
        currentHealth = maxHealth;
        recoverySpace = FindObjectOfType<RecoverySpace>();
    }
}
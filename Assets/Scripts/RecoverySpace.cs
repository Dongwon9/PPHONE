using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RecoverySpace : MonoBehaviour
{
    public int recoveryAmount = 10; // ü�� ȸ����
    public float recoveryInterval = 2f; // ü�� ȸ�� ����

    private Character character;
    private float timer;

    private void Start()
    {
        character = FindObjectOfType<Character>();
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= recoveryInterval)//2�ʰ� ����Ҷ����� ü���� ȸ��
        {
            timer = 0f;
            character.RecoverHealth(recoveryAmount);
        }
    }
}

public class Character : MonoBehaviour
{
    public int maxHealth = 100; // �ִ� ü��
    private int currentHealth; // ���� ü��

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void RecoverHealth(int amount)
    {
        // ü�� ȸ��
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("ü�� ȸ����, ���� ü��: " + currentHealth);
    }
}
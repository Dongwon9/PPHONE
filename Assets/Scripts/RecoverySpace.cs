using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RecoverySpace : MonoBehaviour
{
    public int recoveryAmount = 10; // 체력 회복량
    public float recoveryInterval = 2f; // 체력 회복 간격

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

        if (timer >= recoveryInterval)//2초가 경과할때마다 체력을 회복
        {
            timer = 0f;
            character.RecoverHealth(recoveryAmount);
        }
    }
}

public class Character : MonoBehaviour
{
    public int maxHealth = 100; // 최대 체력
    private int currentHealth; // 현재 체력

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void RecoverHealth(int amount)
    {
        // 체력 회복
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("체력 회복중, 현재 체력: " + currentHealth);
    }
}
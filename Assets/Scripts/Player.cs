using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Player : TurnActor {
    public static event Action OnTurnUpdate;
    public int HP;
    private int moveCount = 0;
    private void Awake() {
        gravityAffected = true;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            nextAction = () => Move(Direction.Left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            nextAction = () => Move(Direction.Right);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            nextAction = () => Move(Direction.Down);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            nextAction = () => Move(Direction.Up);
        } else if (Input.GetKeyDown(KeyCode.Z)) {
            //АјАн
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            nextAction = () => { return; };
        }
        if (nextAction != null) {
            OnTurnUpdate();
        }
    }
    protected override void TurnUpdate() {
        base.TurnUpdate();
        moveCount += 1;
        if (moveCount == 3) {
            moveCount = 0;
            HP -= 1;
        }
    }

    private void TakeDamage(int damage) {
        HP -= damage;
        Debug.Log(ToString() + "Takes damage!");
    }
}

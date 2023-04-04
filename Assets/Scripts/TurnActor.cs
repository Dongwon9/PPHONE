using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditorInternal;
using UnityEngine;

public class TurnActor : MonoBehaviour {
    protected Action nextAction;
    protected bool gravityAffected = false;
    // Start is called before the first frame update
    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        ApplyGravity();
    }
    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }

    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.LogError(ToString() + "'s nextAction was null!");
        } else {
            nextAction();
        }
        nextAction = null;
        ApplyGravity();
    }

    protected void ApplyGravity() {
        if (!gravityAffected) return;
        LayerMask layer = LayerMask.GetMask("Wall") | LayerMask.GetMask("Platform");
        //������ �� : �� ��ġ���� �ּ� 1ĭ �Ʒ��� �ִ� �� �Ǵ� ����
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down, Vector2.down, Mathf.Infinity, layer);
        if (hit.collider == null) {  // �Ʒ��� ������ ���� ����...
            Destroy(gameObject);
        } else {
            transform.position = hit.collider.transform.position + Vector3.up;
        }
    }
    protected void Move(Direction dir) {
        Vector3 direction = Vector3.zero;
        switch (dir) {
            case Direction.Left:
                direction = Vector3.left; 
                break;
            case Direction.Right:
                direction = Vector3.right; 
                break;
            case Direction.Up:
                direction = Vector3.up;
                break;
            case Direction.Down:
                direction = Vector3.down;
                break;
        }
        //���� �����Ϸ��� �������� 1ĭ ������ ���� ���� �ִ°�?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) {
            transform.Translate(direction);
        }
    }
}

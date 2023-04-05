using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {
    protected Action nextAction;
    // Start is called before the first frame update
    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
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
    //�ش� �������� ������ �� �ִ����� �˰� ���� �� ����Ѵ�.
    protected bool MoveCheck(Direction dir) {
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
        return hit.collider != null;
    }
}

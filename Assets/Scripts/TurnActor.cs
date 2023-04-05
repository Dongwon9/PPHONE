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
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는가?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) {
            transform.Translate(direction);
        }
    }
    //해당 방향으로 움직일 수 있는지만 알고 싶을 때 사용한다.
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
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는가?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }
}

using System;
using System.Collections;
using UnityEngine;

public abstract class MovingTurnActor : TurnActor {
    public const float movingTime = 0.1f;
    /// <summary>
    /// dir 방향으로 1칸 이동한다.
    /// </summary>
    protected float timeCounter = -1f;
    private Vector3 moveDir = Vector3.zero;

    public bool TurnReady {
        get {
            return timeCounter == -1;
        }
        set {
            if (value == true) {
                timeCounter = -1;
            } else {
                timeCounter = 0;
            }
        }
    }
    protected void Move(Direction dir) {
        Vector3 direction = Vector3.zero;
        bool? moveToRight = null;
        switch (dir) {
            case Direction.Left:
                direction = Vector3.left;
                moveToRight = false;
                break;

            case Direction.Right:
                direction = Vector3.right;
                moveToRight = true;
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
            moveDir = direction;
            timeCounter = 0;
        }
        //스프라이트 x축으로 반전
        if (moveToRight != null) {
            FlipSprite((bool)moveToRight);
        }
    }

    protected void Move(Vector3 dest) {
        moveDir = dest - transform.position;
        if (moveDir.x > 0) {
            FlipSprite(true);
        } else if (moveDir.x < 0) {
            FlipSprite(false);
        }
        timeCounter = 0;
    }

    /// <summary>
    /// 해당 방향으로 움직일 수 있는지만 알고 싶을 때 사용한다.
    /// </summary>
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
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는지 여부를 반환한다.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }

    protected override void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "의 nextAction이 null입니다");
            DecideNextAction();
        }
        nextAction();
        TurnReady = false;
        nextAction = null;
    }

    /// <summary>
    /// 오브젝트의 좌표를 (정수)+0.5로 교정한다.
    /// </summary>
    private void AdjustPosition() {
        transform.position = new Vector3(
            MathF.Floor(transform.position.x) + 0.5f,
            MathF.Floor(transform.position.y) + 0.5f,
            MathF.Floor(transform.position.y) * 0.1f);
    }

    private void FixedUpdate() {
        //캐릭터가 일정 시간에 걸쳐 움직이게 하는 코드
        if (!TurnReady) {
            if (moveDir.normalized == Vector3.left) {
                spriteRenderer.flipX = true;
            } else if (moveDir.normalized == Vector3.right) {
                spriteRenderer.flipX = false;
            }
            transform.Translate(moveDir * Time.fixedDeltaTime / movingTime);
            timeCounter += Time.deltaTime;
            collider.enabled = false;
            if (timeCounter >= movingTime) {
                TurnReady = true;
                moveDir = Vector3.zero;

                AdjustPosition();
                collider.enabled = true;
                DecideNextAction();
            }
        }
    }
}
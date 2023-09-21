using System;
using UnityEngine;

/// <summary>TurnActor중 움직일 수 있는 오브젝트들<br></br>
///(플레이어, 적 등)</summary>
public abstract class MovingTurnActor : TurnActor {
    private Vector3 moveDir = Vector3.zero;
    protected new Collider2D collider;
    protected SpriteRenderer spriteRenderer;
    /// <summary>이 오브젝트의 스프라이트는 시작할 때 오른쪽을 보는가?</summary>
    protected bool StartsFacingRight;
    /// <summary>현재 턴에 지나간 시간</summary>
    protected float timeCounter = 0f;
    protected bool TurnProcessing = false;
    /// <summary>
    /// 한 턴의 실행이 끝나는데 걸리는 시간.<br></br>
    /// 플레이어는 최소 이 시간을 기다린 후에 행동할 수 있다.
    /// </summary>
    public const float movingTime = 0.1f;
    /// <summary>오브젝트의 좌표를 정수로 교정한다.</summary>
    private void AdjustPosition() {
        transform.position = new Vector3(
            MathF.Round(transform.position.x),
            MathF.Round(transform.position.y),
            MathF.Round(transform.position.y) * 0.1f);
    }


    private void FixedUpdate() {
        //캐릭터가 일정 시간에 걸쳐 움직이게 하는 코드
        if (TurnProcessing) {
            collider.enabled = false;
            if (moveDir.normalized == Vector3.left) {
                FlipSprite(false);
            } else if (moveDir.normalized == Vector3.right) {
                FlipSprite(true);
            }
            //왼쪽도 오른쪽도 아니면, 스프라이트를 그대로 둔다.
            transform.Translate(moveDir * Time.fixedDeltaTime / movingTime);
            timeCounter += Time.fixedDeltaTime;

            if (timeCounter >= movingTime) {
                TurnProcessing = false;
                moveDir = Vector3.zero;
                AdjustPosition();
                collider.enabled = true;
            }
        }
    }

    protected virtual void Awake() {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>스프라이트를 X축으로 뒤집거나 뒤집지 않는다.</summary>
    /// <param name="toRight">스프라이트가 오른쪽을 볼까?</param>
    protected virtual void FlipSprite(bool toRight) {
        spriteRenderer.flipX = StartsFacingRight ? !toRight : toRight;
    }

    /// <summary>dir 방향으로 1칸 이동한다.</summary>
    protected void Move(Direction dir) {
        Vector3 direction = DirectionToVector(dir);
        bool? moveToRight = null;
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는가?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) {
            //벽이 없다면 움직인다.
            moveDir = direction;
            TurnProcessing = true;
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
        TurnProcessing = true;
        timeCounter = 0;
    }

    /// <summary>해당 방향으로 움직일 수 있는지만 알고 싶을 때 사용한다.</summary>
    protected bool MoveCheck(Direction dir) {
        Vector3 direction = TurnActor.DirectionToVector(dir);
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는지 여부를 반환한다.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }
}
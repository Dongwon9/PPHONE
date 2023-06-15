using System;
using UnityEngine;

/// <summary>
/// TurnActor중 움직일 수 있는 오브젝트들<br></br>
///(플레이어, 적 등)
/// </summary>
public abstract class MovingTurnActor : TurnActor {
    private Vector3 moveDir = Vector3.zero;
    protected new Collider2D collider;
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    ///이 오브젝트의 스프라이트는 시작할 때 오른쪽을 보는가?
    /// </summary>
    protected bool StartsFacingRight;
    protected float timeCounter = -1f;
    /// <summary>
    /// 한 턴의 실행이 끝나는데 걸리는 시간.<br></br>
    /// 플레이어는 최소 이 시간을 기다린 후에 행동할 수 있다.
    /// </summary>
    public const float movingTime = 0.1f;
    /// <summary>
    /// Private인 timeCounter를 대신해 클래스들이 참조할 수 있는 속성<br></br>
    /// 다른 클래스들은 자신 턴이 준비 됐는지만 알 수 있다.
    /// </summary>
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

    protected virtual void Awake() {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 스프라이트를 X축으로 뒤집거나 뒤집지 않는다
    /// </summary>
    protected virtual void FlipSprite(bool toRight) {
        spriteRenderer.flipX = StartsFacingRight ? !toRight : toRight;
    }

    /// <summary>
    /// dir 방향으로 1칸 이동한다.
    /// </summary>
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
}
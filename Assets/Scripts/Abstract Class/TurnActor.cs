using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {
    public const float movingTime = 0.1f;
    /// <summary>
    /// 모든 TurnActor들이 사용하는 다음턴 action
    /// </summary>
    protected Action nextAction;
    /// <summary>
    /// 이름을 collider 라고 하려 했더니 같은 이름의 deprecated 필드가 있댄다...
    /// </summary>
    protected Collider2D objectCollider;
    protected SpriteRenderer spriteRenderer;
    private Vector3 moveDir = Vector3.zero;
    private float timeCounter = -1f;
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

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    /// <summary>
    /// position에 공격 경고를 띄운다. 이 함수를 반복적으로 사용해 적의 공격을 구현한다.
    /// </summary>
    public void AttackPreTurn(Vector3 position, int damage, Action onHitEffect = null, bool instant = false) {
        Attack attack = ObjectPool.AttackPool.Get();
        attack.tag = gameObject.tag;
        attack.transform.position = position;
        attack.damage = damage;
        attack.onHitEffect = onHitEffect;
        attack.instant = instant;
    }

    protected virtual void Awake() {
        objectCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// TurnActor들이 다음 행동을 정할 때 사용하는 함수
    /// </summary>
    protected abstract void DecideNextAction();

    /// <summary>
    /// dir 방향으로 1칸 이동한다.
    /// </summary>
    protected void Move(Direction dir) {
        Vector3 direction = Vector3.zero;
        bool? spriteFlip = null;
        switch (dir) {
            case Direction.Left:
                direction = Vector3.left;
                spriteFlip = true;
                break;

            case Direction.Right:
                direction = Vector3.right;
                spriteFlip = false;
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
        if (spriteFlip != null) {
            spriteRenderer.flipX = (bool)spriteFlip;
        }
    }

    protected void Move(Vector3 dest) {
        moveDir = dest - transform.position;
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

    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        AdjustPosition();
        DecideNextAction();
    }

    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "의 nextAction이 null입니다");
            DecideNextAction();
        }
        nextAction();
        TurnReady = false;
        nextAction = null;
        // DecideNextAction();
    }

    /// <summary>
    /// 오브젝트의 좌표를 (정수)+0.5로 교정한다.
    /// </summary>
    private void AdjustPosition() {
        transform.position = new Vector3(MathF.Floor(transform.position.x) + 0.5f, MathF.Floor(transform.position.y) + 0.5f);
    }

    private void FixedUpdate() {
        //캐릭터가 일정 시간에 걸쳐 움직이게 하는 코드
        if (timeCounter >= 0) {
            if (moveDir.normalized == Vector3.left) {
                spriteRenderer.flipX = true;
            } else if (moveDir.normalized == Vector3.right) {
                spriteRenderer.flipX = false;
            }
            transform.Translate(moveDir * Time.fixedDeltaTime / movingTime);
            timeCounter += Time.deltaTime;
            objectCollider.enabled = false;
            if (timeCounter >= movingTime) {
                TurnReady = true;
                moveDir = Vector3.zero;

                AdjustPosition();
                objectCollider.enabled = true;
                DecideNextAction();
            }
        }
    }

    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }
}
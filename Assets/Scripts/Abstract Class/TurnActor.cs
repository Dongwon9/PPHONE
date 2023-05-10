using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {
    /// <summary>
    /// �� ���� ������ �����µ� �ɸ��� �ð�.<br></br>
    /// �÷��̾�� �ּ� �� �ð��� ��ٸ� �Ŀ� �ൿ�� �� �ִ�.
    /// </summary>
    public const float movingTime = 0.1f;
    protected new Collider2D collider;
    /// <summary>
    /// ��� TurnActor���� ����ϴ� ������ action
    /// </summary>
    protected Action nextAction;
    protected SpriteRenderer spriteRenderer;
    private Vector3 moveDir = Vector3.zero;
    private float timeCounter = -1f;
    /// <summary>
    /// Private�� timeCounter�� ����� Ŭ�������� ������ �� �ִ� �Ӽ�<br></br>
    /// �ٸ� Ŭ�������� �ڽ� ���� �غ� �ƴ����� �� �� �ִ�.
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

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    /// <summary>
    /// position�� ���� ��� ����. �� �Լ��� �ݺ������� ����� ���� ������ �����Ѵ�.
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
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// TurnActor���� ���� �ൿ�� ���� �� ����ϴ� �Լ�
    /// </summary>
    protected abstract void DecideNextAction();

    //��������Ʈ�� X������ �����ų� ������ �ʴ´�
    protected virtual void FlipSprite(bool toRight) {
        spriteRenderer.flipX = toRight;
    }

    /// <summary>
    /// dir �������� 1ĭ �̵��Ѵ�.
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
        //���� �����Ϸ��� �������� 1ĭ ������ ���� ���� �ִ°�?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) {
            moveDir = direction;
            timeCounter = 0;
        }
        //��������Ʈ x������ ����
        if (moveToRight != null) {
            FlipSprite((bool)moveToRight);
        }
    }

    protected void Move(Vector3 dest) {
        moveDir = dest - transform.position;
        timeCounter = 0;
    }

    /// <summary>
    /// �ش� �������� ������ �� �ִ����� �˰� ���� �� ����Ѵ�.
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
        //���� �����Ϸ��� �������� 1ĭ ������ ���� ���� �ִ��� ���θ� ��ȯ�Ѵ�.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }

    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        AdjustPosition();
        DecideNextAction();
    }

    /// <summary>
    /// ���� ���� �ൿ�� �����ϴ� �ڵ�
    /// </summary>
    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "�� nextAction�� null�Դϴ�");
            DecideNextAction();
        }
        nextAction();
        TurnReady = false;
        nextAction = null;
        // DecideNextAction();
    }

    /// <summary>
    /// ������Ʈ�� ��ǥ�� (����)+0.5�� �����Ѵ�.
    /// </summary>
    private void AdjustPosition() {
        transform.position = new Vector3(MathF.Floor(transform.position.x) + 0.5f, MathF.Floor(transform.position.y) + 0.5f);
    }

    private void FixedUpdate() {
        //ĳ���Ͱ� ���� �ð��� ���� �����̰� �ϴ� �ڵ�
        if (timeCounter >= 0) {
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

    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }
}
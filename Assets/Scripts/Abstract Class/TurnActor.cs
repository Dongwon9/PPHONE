using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {
    protected const float movingTime = 0.05f;

    /// <summary>
    /// ��� TurnActor���� ����ϴ� ������ action
    /// </summary>
    protected Action nextAction;

    /// <summary>
    /// �̸��� collider ��� �Ϸ� �ߴ��� ���� �̸��� deprecated �ʵ尡 �ִ���...
    /// </summary>
    protected Collider2D objectCollider;

    protected float timeCounter = -1f;
    private Vector3 moveDir = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// position�� ���� ����� ����. �� �Լ��� �ݺ������� ����� ���� ������ �����Ѵ�.
    /// </summary>
    public void AttackPreTurn(Vector3 position, int damage, Action onHitEffect = null) {
        var attack = ObjectPool.AttackPool.Get();
        attack.tag = gameObject.tag;
        attack.transform.position = position;
        attack.damage = damage;
        attack.onHitEffect = onHitEffect;
    }

    protected virtual void Awake() {
        objectCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// TurnActor���� ���� �ൿ�� ���� �� ����ϴ� �Լ�
    /// </summary>
    protected abstract void DecideNextAction();

    /// <summary>
    /// dir �������� 1ĭ �̵��Ѵ�.
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
        //���� �����Ϸ��� �������� 1ĭ ������ ���� ���� �ִ°�?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) {
            moveDir = direction;
            timeCounter = 0;
        }
        //��������Ʈ x������ ����
        if (spriteFlip != null) {
            spriteRenderer.flipX = (bool)spriteFlip;
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

    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "�� nextAction�� null�Դϴ�");
            DecideNextAction();
        }
        nextAction();

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
            objectCollider.enabled = false;
            if (timeCounter >= movingTime) {
                timeCounter = -1f;
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
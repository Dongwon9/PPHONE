using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {

    /// <summary>
    /// ��� TurnActor���� ����ϴ� ������ action
    /// </summary>
    protected Action nextAction;

    private Collider2D colldier;
    private Vector3 moveDir = Vector3.zero;
    protected float timeCounter = -1f;
    private const float movingTime = 0.1f;

    // Start is called before the first frame update
    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        DecideNextAction();
    }

    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }

    protected virtual void Awake() {
        colldier = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        if (timeCounter >= 0) {
            transform.Translate(moveDir * Time.fixedDeltaTime / movingTime);
            timeCounter += Time.deltaTime;
            colldier.enabled = false;
            if (timeCounter >= movingTime) {
                timeCounter = -1f;
                moveDir = Vector3.zero;
                //�������� ��ģ �Ŀ� ������Ʈ�� ��ǥ�� (����)+0.5�� �����Ѵ�.
                transform.position = new Vector3(MathF.Floor(transform.position.x) + 0.5f, MathF.Floor(transform.position.y) + 0.5f);
                colldier.enabled = true;
            }
        }
        Debug.Log(gameObject.ToString() + "frameCounter = " + timeCounter);
    }

    /// <summary>
    /// TurnActor���� ���� �ൿ�� ���� �� ����ϴ� �Լ�
    /// </summary>
    protected abstract void DecideNextAction();

    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "�� nextAction�� null�Դϴ�");
        } else {
            nextAction();
        }
        nextAction = null;
        DecideNextAction();
    }

    /// <summary>
    /// position�� ���� ��� ����. �� �Լ��� �ݺ������� ����� ���� ������ �����Ѵ�.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="damage"></param>
    public void AttackPreTurn(Vector3 position, int damage, Action onHitEffect = null) {
        var attack = ObjectPool.AttackPool.Get();
        attack.tag = gameObject.tag;
        attack.transform.position = position;
        attack.damage = damage;
        attack.onHitEffect = onHitEffect;
    }

    /// <summary>
    /// dir �������� 1ĭ �̵��Ѵ�.
    /// </summary>
    /// <param name="dir"></param>
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
            moveDir = direction;
            timeCounter = 0;
        }
    }

    /// <summary>
    /// �ش� �������� ������ �� �ִ����� �˰� ���� �� ����Ѵ�.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
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
}
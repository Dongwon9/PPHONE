using System;
using UnityEngine;

/// <summary>
/// 턴마다 어떤 행동을 하는 모든 오브젝트는 TurnActor를 상속한다.
/// </summary>
public abstract class TurnActor : MonoBehaviour {
    /// <summary>모든 TurnActor들이 사용하는 다음턴 action</summary>

    public enum Direction { Left, Up, Right, Down };

    public enum Target { Player, Enemy, Any, Wall }

    protected Action nextAction;

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    /// <summary> position 칸에 공격을 한다.</summary>
    /// <param name="target">공격할 대상(플레이어,적 또는 둘다)</param>
    protected void Attack(Vector3 position, int damage, Target target) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.down, 0.1f);
        foreach (RaycastHit2D hit in hits) {
            switch (target) {
                case Target.Player:
                    hit.collider.GetComponent<Player>()?.TakeDamage(damage);
                    break;

                case Target.Enemy:
                    hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
                    break;

                case Target.Any:
                    hit.collider.GetComponent<IDamagable>()?.TakeDamage(damage);
                    break;
            }
        }
    }

    /// <summary> TurnActor들이 다음 행동을 정할 때 사용하는 함수</summary>
    protected virtual void DecideNextAction() {
        nextAction = () => { };
    }

    protected virtual void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }

    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        DecideNextAction();
    }

    /// <summary>현재 턴의 행동을 실행하는 코드</summary>
    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "의 nextAction이 null입니다");
            DecideNextAction();
        }
        nextAction();
        DecideNextAction();
    }

    public Vector3 DirectionToVector(Direction dir) {
        switch (dir) {
            case Direction.Left:
                return Vector3.left;

            case Direction.Right:
                return Vector3.right;

            case Direction.Up:
                return Vector3.up;

            case Direction.Down:
                return Vector3.down;

            default:
                return Vector3.zero;
        }
    }

    public bool CheckForObjectAtPosition(Target[] objectType, Vector3 position) {
        if (objectType.Length == 0) {
            Debug.LogError("objectType 매개변수에 요소가 하나도 없습니다");
            return false;
        }
        string[] objects = new string[objectType.Length];
        for (int i = 0; i < objects.Length; i++) {
            switch (objectType[i]) {
                case Target.Enemy:
                    objects[i] = "Enemy";
                    break;

                case Target.Player:
                    objects[i] = "Player";
                    break;

                case Target.Wall:
                    objects[i] = "Wall";
                    break;
            }
        }
        return Physics2D.Raycast(position, Vector2.down, 0.1f, LayerMask.GetMask(objects));
    }

    public Vector2 Round(Vector2 orig) {
        return new Vector2(
         MathF.Round(orig.x),
         MathF.Round(orig.y));
    }

    /// <summary>
    /// position에 공격 경고를 띄운다. 이 함수를 반복적으로 사용해 적의 공격을 구현한다.
    /// </summary>
    public void AttackWarning(Vector3 position, bool instant = false) {
        RedSquare attack = ObjectPool.AttackPool.Get();
        attack.transform.position = position;
        attack.instant = instant;
    }
}
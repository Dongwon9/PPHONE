using System;
using UnityEngine;

/// <summary>
/// 버프와 디버프를 나타내는 클래스
/// </summary>
[CreateAssetMenu(fileName = "NewEffect", menuName = "새 상태이상")]
public class Effect : ScriptableObject {
    public Action<MovingTurnActor> specialFunction = (MovingTurnActor target) => { };
    public static readonly Effect[] effectList;
    public int duration;
    public Sprite Icon;
    public string Id;
    public bool isDebuff; //'디버프 제거' 등을 구현하기 위해 만든 필드
    public string Description;

    /// <summary>1턴마다 체력이 이 수만큼 증가/감소한다.</summary>
    public int healthPerTurn;
    public bool stunned;
    /// <summary>
    /// 양수면 공격력 증가, 음수면 공격력 감소
    /// </summary>
    public int attack;
    /// <summary>
    /// 공격력에 이 수를 곱한다.<br></br>
    /// 이 수가 1이 아니면 attack은 무시된다.
    /// </summary>
    public float attackMultiplier = 1;
    /// <summary>
    /// 양수면 받는 대미지 방어, 음수면 대미지 증가
    /// </summary>
    public int defense;
}
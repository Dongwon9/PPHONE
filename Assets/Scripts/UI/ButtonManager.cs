using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField]
    private Button UpButton, DownButton, LeftButton, RightButton, ModeButton, WaitButton;
    public static ButtonManager Instance;
    /// <summary>
    /// 버튼이 공격모드인가? 이동모드인가?
    /// </summary>
    public bool AttackMode { get; private set; }
    private void Awake() {
        Instance = this;
        AttackMode = false;
        UpButton.onClick.AddListener(() => Player.Instance.TakeInput(Direction.Up));
        DownButton.onClick.AddListener(() => Player.Instance.TakeInput(Direction.Down));
        LeftButton.onClick.AddListener(() => Player.Instance.TakeInput(Direction.Left));
        RightButton.onClick.AddListener(() => Player.Instance.TakeInput(Direction.Right));
        ModeButton.onClick.AddListener(() => AttackMode = !AttackMode);
        WaitButton.onClick.AddListener(() => Player.Instance.TakeInput(null));
    }
}
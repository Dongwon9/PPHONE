using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    public static ButtonManager Instance;
    [SerializeField]
    private Button UpButton, DownButton, LeftButton, RightButton, ModeButton, WaitButton;
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
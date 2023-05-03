using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    public GameObject UIWindow;
    [SerializeField] private TextMeshProUGUI text;
    public bool UIActive { get; private set; }

    private UIManager() {
    }

    public void SetText(string text) {
        this.text.SetText(text);
    }

    public void SetUIActive(bool active) {
        UIActive = active;
        UIWindow.SetActive(UIActive);
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SetUIActive(false);
        }
    }
}
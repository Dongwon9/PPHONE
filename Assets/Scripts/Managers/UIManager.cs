using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI text;
    public GameObject UIWindow;
    private UIManager() { }

    public bool UIActive { get; private set; }
    public void SetUIActive(bool active) {
        UIActive = active;
        UIWindow.SetActive(UIActive);
    }

    public void SetText(string text) {
        this.text.SetText(text);
    }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SetUIActive(false);
        }
    }
}

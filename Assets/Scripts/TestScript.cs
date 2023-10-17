using UnityEngine;

public class TestScript : MonoBehaviour {
    private void Awake() {
        print("Awake!");
    }

    private void Start() {
        print("Start!");
    }

    private void OnEnable() {
        print("onEnable!");
    }

    private void OnDestroy() {
        print("onDestroy!");
    }

    private void OnDisable() {
        print("onDisable!");
    }
}
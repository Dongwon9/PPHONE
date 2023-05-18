using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EachButtonScript : MonoBehaviour {
    private Image image;
    [SerializeField]
    private Sprite moveImage, attackImage;
    private void Awake() {
        image = GetComponent<Image>();
    }

    private void Update() {
        image.sprite = ButtonManager.Instance.AttackMode ? attackImage : moveImage;
    }
}
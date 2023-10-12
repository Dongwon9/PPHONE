using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShowScript : MonoBehaviour
{
    private readonly List<PartComponents> playerPartComponents = new();
    [SerializeField]
    private List<GameObject> playerParts;
    private class PartComponents {
        public Animator animator;
        public SpriteRenderer sprite;
        public Transform transform;
        public PartComponents(Animator animator, SpriteRenderer sprite, Transform transform) {
            this.animator = animator != null ? animator : throw new ArgumentNullException(nameof(animator));
            this.sprite = sprite != null ? sprite : throw new ArgumentNullException(nameof(sprite));
            this.transform = transform;
        }
    }
    private void Awake() {
        foreach (GameObject obj in playerParts) {
            playerPartComponents.Add(
                new PartComponents(obj.GetComponent<Animator>(), obj.GetComponent<SpriteRenderer>(), obj.transform));
        }
    }

public void HideOrShowArm(int showArm) {
        foreach (var part in playerPartComponents) {
            part.sprite.enabled = (showArm == 1);
        }
    }
}

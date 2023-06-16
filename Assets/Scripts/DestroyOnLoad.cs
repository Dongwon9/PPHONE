using System.Collections;
using UnityEngine;

/// <summary>
/// 프리팹에 있는 스프라이트마스크에 넣는다.<br></br>
/// 로드나 적 생성 즉시에 파괴된다.
/// </summary>
public class DestroyOnLoad : MonoBehaviour {
    private void OnEnable() {
        Destroy(gameObject);
    }

    private void Update() {
        Destroy(gameObject);
    }
}
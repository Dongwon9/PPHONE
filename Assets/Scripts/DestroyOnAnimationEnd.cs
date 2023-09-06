using UnityEngine;

/// <summary>
/// 애니메이션이 끝나면 오브젝트가 파괴되는 경우가 많아서,<br></br>
/// 각각의 오브젝트에 따로 부착할 수 있게 한개의 파일로 빼놨다.
/// </summary>
public class DestroyOnAnimationEnd : MonoBehaviour {
    /// <summary>
    /// 마지막 애니메이션 프레임에 애니메이션 트리거로 Destroy를 넣으면 이 함수를 호출한다.
    /// </summary>
    public void Destroy() {
        Destroy(gameObject);
    }
    public void DestroyParent() {
        Destroy(transform.parent.gameObject);
    }
}

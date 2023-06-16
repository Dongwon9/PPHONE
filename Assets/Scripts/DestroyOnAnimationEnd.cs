using UnityEngine;

/// <summary>
/// 애니메이션이 끝나면 오브젝트가 파괴되는 경우가 많아서,<br></br>
/// 각각의 오브젝트에 따로 부착할 수 있게 한개의 파일로 빼놨다.
/// </summary>
public class DestroyOnAnimationEnd : MonoBehaviour {
    public void Destroy() {
        Destroy(gameObject);
    }
}
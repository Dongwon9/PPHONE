using UnityEngine;

/// <summary>
/// �ִϸ��̼��� ������ ������Ʈ�� �ı��Ǵ� ��찡 ���Ƽ�,<br></br>
/// ������ ������Ʈ�� ���� ������ �� �ְ� �Ѱ��� ���Ϸ� ������.
/// </summary>
public class DestroyOnAnimationEnd : MonoBehaviour {
    public void Destroy() {
        Destroy(gameObject);
    }
}
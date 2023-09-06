using TMPro;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour {
    [SerializeField] private GameObject damageTextPrefab;
    public static DamageNumberManager instance;

    private void Awake() {
        instance = this;
    }

    public void DisplayDamageNumber(int number, Vector3 position, bool isHeal = false) {
        GameObject DamageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        TextMeshPro damageTMP = DamageText.transform.GetChild(0).GetComponent<TextMeshPro>();
        damageTMP.SetText(number.ToString());
        damageTMP.color = isHeal ? Color.green : Color.red;
    }
}
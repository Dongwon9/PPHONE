using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour {
    public GameObject objectPrefab; // 물체 프리팹
                                    // UI 구축
    public GameObject dialoguePanel;
    public Text questionText;
    public Button trueButton;
    public Button falseButton;
    private GameObject currentObject; // 현재 물체

    private int objectDistance = 1; // 물체와의 최대 접촉 거리

    private bool isObjectVisible = false; // 물체의 가시성 여부판단
    private int correctAnswer;

    private void Start() {
        // 게임 맵 생성
        CreateGameMap();
    }

    private void CreateGameMap() {
        // 맵 정중앙에 물체 생성
        currentObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
    }

    private void Update() {
        // 물체와의 접촉 여부 확인
        if (Vector3.Distance(transform.position, currentObject.transform.position) <= objectDistance) {
            if (!isObjectVisible) {
                isObjectVisible = true;
                // 대화창 열기
                OpenDialogue();
            }
        } else {
            if (isObjectVisible) {
                isObjectVisible = false;
                // 대화창 닫기
                CloseDialogue();
            }
        }
    }

    private void OpenDialogue() {
        // 랜덤 산술 문제 출제
        int number1 = Random.Range(1, 10);
        int number2 = Random.Range(1, 10);
        int result = number1 + number2;

        // 문제와 정답 설정
        string question = "문제: " + number1 + " + " + number2 + " = ?";
        correctAnswer = number1 + number2;

        // 대화창 열기
        dialoguePanel.SetActive(true);
        questionText.text = question;

        // O 버튼 클릭 시 정답 체크
        trueButton.onClick.AddListener(CheckAnswer);
        // X 버튼 클릭 시 오답 체크
        falseButton.onClick.AddListener(CheckAnswer);
    }

    private void CloseDialogue() {
        // 대화창 닫기
        dialoguePanel.SetActive(false);
    }

    private void CheckAnswer() {
        bool isCorrect = (correctAnswer == (trueButton == UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject ? 1 : 0));

        if (isCorrect) {
            Debug.Log("정답입니다!");
        } else {
            Debug.Log("오답입니다!");
        }
    }
}
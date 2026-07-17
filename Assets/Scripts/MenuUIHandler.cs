using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;
// [추가] 유니티 에디터에서만 작동하는 기능을 쓰기 위해 가져옵니다.
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField nameInputField; 
    public TextMeshProUGUI bestScoreText; 

    private void Start()
    {
        // 씬이 시작될 때 DataManager에 저장된 최고 점수가 있다면 화면에 표시
        if (DataManager.Instance != null && DataManager.Instance.highScore > 0)
        {
            bestScoreText.text = $"Best Score : {DataManager.Instance.highScorePlayerName} : {DataManager.Instance.highScore}";
        }
        else
        {
            bestScoreText.text = "Best Score : 0";
        }
    }

    public void StartGame()
    {
        // 사용자가 입력한 이름을 DataManager에 저장 (장면 간 영속성 핵심)
        DataManager.Instance.currentPlayerName = nameInputField.text;
        
        // 메인 게임 씬 로드 (Build Settings에 등록된 Main 씬 이름이나 인덱스 입력)
        SceneManager.LoadScene(1); 
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // 플레이 모드를 종료합니다.
        
        // 그게 아니라 진짜 빌드된 게임이라면?
        #else
        Application.Quit();
        #endif
    }
}
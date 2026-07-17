using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem; // MIGRATED: New Input System namespace

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
// [추가 1] 최고 점수를 표시할 UI 텍스트 변수 추가
    public Text BestScoreText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // MIGRATED: InputAction replaces Input.GetKeyDown(KeyCode.Space)
    private InputAction m_LaunchAction;

    // MIGRATED: bind the Space key as a button action
    void Awake()
    {
        m_LaunchAction = new InputAction("Launch", InputActionType.Button, "<Keyboard>/space");
    }

    // MIGRATED: enable the action while the component is active
    void OnEnable()
    {
        m_LaunchAction.Enable();
    }

    // MIGRATED: disable the action when the component is inactive
    void OnDisable()
    {
        m_LaunchAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

        // [추가 2] 게임 시작 시 현재 플레이어 이름과 최고 점수를 UI에 세팅
        if (DataManager.Instance != null)
        {
            // 현재 플레이어 이름 표시 (선택 사항: ScoreText 옆에 띄우기)
            ScoreText.text = $"{DataManager.Instance.currentPlayerName} Score : {m_Points}";
            
            // 최고 점수 표시
            BestScoreText.text = $"Best Score : {DataManager.Instance.highScorePlayerName} : {DataManager.Instance.highScore}";
        }
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

// [추가 3] 게임 오버 시 최고 점수인지 확인하고 저장 (세션 간 영속성)
        if (DataManager.Instance != null && m_Points > DataManager.Instance.highScore)
        {
            DataManager.Instance.highScore = m_Points;
            DataManager.Instance.highScorePlayerName = DataManager.Instance.currentPlayerName;
            
            // 파일에 기록
            DataManager.Instance.SaveHighScore(); 
            
            // 화면 텍스트 즉시 업데이트
            BestScoreText.text = $"Best Score : {DataManager.Instance.highScorePlayerName} : {DataManager.Instance.highScore}";
        }

    }

    // 메뉴로 돌아가는 버튼용 함수
    public void BackToMenu()
    {
        // 씬 매니저를 통해 메뉴 씬을 로드합니다. 
        // (Build Settings에서 메뉴 씬의 이름을 "Menu"로 했거나 인덱스가 0일 때 작동합니다)
        SceneManager.LoadScene("Menu"); // 메뉴 씬 이름이 다르다면 그 이름으로 바꿔주세요.
    }
}

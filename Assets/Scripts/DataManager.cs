using UnityEngine;
using System.IO;
using TMPro;

public class DataManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static DataManager Instance;

    // 1. 장면 간 영속성 (현재 플레이어 이름)
    public string currentPlayerName; 

    // 2. 세션 간 영속성 (최고 점수와 달성자 이름)
    public int highScore; 
    public string highScorePlayerName; 

    private void Awake()
    {
        // 싱글톤 패턴: 씬이 넘어가도 파괴되지 않도록 설정
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 게임 시작 시 기존 최고 점수 불러오기
        LoadHighScore();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScorePlayerName;
    }

    // 파일 포장(저장)
    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = highScore;
        data.highScorePlayerName = highScorePlayerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    // 파일 풀기(불러오기)
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScorePlayerName = data.highScorePlayerName;
        }
    }
}
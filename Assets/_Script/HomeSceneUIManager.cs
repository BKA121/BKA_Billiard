using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneUIManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
    public void OnPracticeButtonClicked()
    {
        MatchConfig localPvP = MatchConfig.CreatePvPMatch(102, 
                                                          1, "Player 1", PlayerType.Local, 
                                                          2, "Player 2", PlayerType.Local, 40f);
        GameManager.Instance.MatchConfigToLoad = localPvP;
        SceneManager.LoadScene("GameScene");
    }
}

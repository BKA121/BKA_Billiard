using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public void OnPracticeButtonClicked()
    {
        MatchConfig localPvP = MatchConfig.CreatePvPMatch(102, 
                                                          1, "Player 1", PlayerType.Local, 
                                                          2, "Player 2", PlayerType.Local, 40f);
        GameManager.Instance.MatchConfigToLoad = localPvP;
        SceneManager.LoadScene("GamePlay");
    }
}

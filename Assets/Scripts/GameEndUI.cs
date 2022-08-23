using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    public Text winnerText;
    public Button restartBtn;
    public Button quitBtn;

    private void Awake()
    {
        restartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
        });
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}

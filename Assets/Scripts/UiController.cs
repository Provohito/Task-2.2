using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    [SerializeField]
    GameObject diePanel;

    public void OpenDiePanel()
    {
        Time.timeScale = 0;
        diePanel.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}

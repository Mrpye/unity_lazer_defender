using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSecods = 1f;
   public void LoadStartMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadGame() {
        GameSession gameSession = FindObjectOfType<GameSession>();
        Destroy(gameSession);
        SceneManager.LoadScene("MainGame");
    }

    public void LoadGameOver() {
        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver() {
        yield return new WaitForSeconds(delayInSecods);
        SceneManager.LoadScene("GameOver");
    }
    public void QuitGame() {
        Application.Quit();
    }


}

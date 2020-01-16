using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour {

    // [SerializeField] private int pointsPerBlockedDestroyed = 83;

    [SerializeField] private int currentscore = 0;
    [SerializeField] private int lives = 5;
   
    private void Awake() {
        SetUpSingleton();
    }

    private void SetUpSingleton() {
        int gameSessionCount = FindObjectsOfType<GameSession>().Length;
        if (gameSessionCount > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }
    public int GetScore() {
        return currentscore;
    }
    public int AddScore(int points) {
        return currentscore += points; ;
    }

    public int GetLives() {
        return lives;
    }

    public void AddLife() {
         lives ++;
    }
    public void SetLifes(int lives) {
         this.lives= lives;
    }




    public bool LooseLife() {
        lives--;
        if (lives <= 0) {
            return false;
        } else {
            return true;
        }
    }
    public void ResetScore() {
        Destroy(gameObject);
    }

   
}
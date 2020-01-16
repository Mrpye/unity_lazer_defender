using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> powerUpPrefab;
    [SerializeField] public float moveSpeed = 2f;
    public void SpawnRandomPowerUP(float x,float y,int spawn_chance) {
        int powerup_index=Random.Range(0, powerUpPrefab.Count);
        int powerup_chance = Random.Range(0, 100- spawn_chance);
        if (powerup_chance == 0 || powerup_chance == 5) {
            GameObject power_up= Instantiate<GameObject>(powerUpPrefab[powerup_index], new Vector2(x, y), Quaternion.identity);
            power_up.GetComponent<PowerUp>().moveSpeed = moveSpeed;
        }
    }
    // Start is called before the first frame update
   
}

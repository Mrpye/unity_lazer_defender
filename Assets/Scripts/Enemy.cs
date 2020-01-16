using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] private float health = 100f;

    [SerializeField] private int scoreValue = 100;

    [Header("Enemy Shooting")]
    [SerializeField] private bool shoot_enabled = true;
    [SerializeField] private bool double_lasers = false;
    [SerializeField] private float double_laser_distance = 0.5f;
    
    [SerializeField] private GameObject PrefabLaser;
    private float shot_counter;
    [SerializeField] private float minTimeBetweenShots = 0.2f;
    [SerializeField] private float maxTimeBetweenShots = 3f;
    [SerializeField] private float projectile_speed = 10f;
    [SerializeField] private float durationOfExplosion = 1f;
    [SerializeField] private float minShootWithinRange = 0.3f;
    [SerializeField] public AudioClip laser;

    [Header("Enemy FX")]
    [SerializeField] private GameObject deathFX;


    [Header("Debris")]
    [SerializeField] private  List<GameObject> debri_item;
    [SerializeField] private int debri_item_count=3;
    [SerializeField] private int debri_move_speed_min = 1;
    [SerializeField] private int debri_move_speed_max = 5;
    [SerializeField] private int debri_rotate_speed_min = 1;
    [SerializeField] private int debri_rotate_speed_max = 5;
    [SerializeField] private bool debri_on_death =false;
    [SerializeField] private bool is_debri = false;
    [SerializeField] private float time_to_live = 6;

    [Header("PowerUp")]
    [SerializeField] private bool can_spawn_random_powerup=false;
    [SerializeField] private int random_powerup_chance = 100;

    private bool JustShotOverPlayer = false;
    private Player player;

    private void Start() {
        shot_counter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        player = FindObjectOfType<Player>();

        minShootWithinRange = Random.Range(0.1f, 1f);
    }

    // Update is called once per frame
    private void Update() {
        if (time_to_live <= 0 && is_debri==true) {
            Destroy(gameObject);
            return;
        } else {
            time_to_live -= Time.deltaTime;
        }
        if (player) {
            float px = player.transform.position.x;
            if (transform.position.x > px - minShootWithinRange && transform.position.x < px + minShootWithinRange && JustShotOverPlayer == false) {
                if (shoot_enabled == true) {
                    JustShotOverPlayer = true;
                    fire();
                }
            }
        }
        CountDownAndShoot();
    }

    private void CountDownAndShoot() {
        if (shoot_enabled == true) {
            shot_counter -= Time.deltaTime;
            if (shot_counter <= 0f) {
                fire();
                shot_counter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
                JustShotOverPlayer = false;
            }
        }
    }

    private void fire() {
        if (double_lasers == false) {
            GameObject laser = Instantiate(PrefabLaser, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectile_speed, -2);
        } else {
            Vector3 left_laser = new Vector3(transform.position.x - double_laser_distance, transform.position.y, transform.position.z);
            Vector3 right_laser = new Vector3(transform.position.x + double_laser_distance, transform.position.y, transform.position.z);

            GameObject laser1 = Instantiate(PrefabLaser, left_laser, Quaternion.identity) as GameObject;
            laser1.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectile_speed, -2);

            GameObject laser2 = Instantiate(PrefabLaser, right_laser, Quaternion.identity) as GameObject;
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectile_speed, -2);
        }
        if (laser != null) {
            AudioSource.PlayClipAtPoint(laser, this.gameObject.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        FindObjectOfType<GameSession>().AddScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        if (debri_on_death==true) {
            for (int i =0; i < debri_item_count; i++) {
                GameObject item = debri_item[Random.Range(0, debri_item.Count-1)];


                GameObject debri = Instantiate(item, transform.position, Quaternion.identity) as GameObject;
                debri.GetComponent<Rigidbody2D>().rotation = Random.Range(-debri_rotate_speed_min, debri_rotate_speed_max); 
                float move_speed= Random.Range(debri_move_speed_min, debri_move_speed_max); 
                debri.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-move_speed, move_speed), Random.Range(-move_speed, move_speed), -2);
                debri.GetComponent<Enemy>().is_debri = true;
            }
        }
       

        //Spawn Powerup
        if (can_spawn_random_powerup==true) {
            FindObjectOfType<PowerUpSpawner>().SpawnRandomPowerUP(transform.position.x, transform.position.y, random_powerup_chance);
        }
    }
}
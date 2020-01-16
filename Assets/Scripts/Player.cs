using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Player")]
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float padding = 0.1f;
    [SerializeField] private int maxHealth = 800;
    [SerializeField] private int health = 200;
    [SerializeField] private GameObject deathFX;
    [SerializeField] private float durationOfExplosion = 1f;

    [Header("Player Projectile")]
    [SerializeField] private GameObject PrefabLaser;

    [SerializeField] private bool douple_fire = false;
    [SerializeField] private int temp_invencibility_on_create = 10;
    [SerializeField] private float projectile_speed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] public AudioClip laserAudio;

    [Header("Player Shield")]
    [SerializeField] private List<GameObject> shield_animation;
    

    private HealthBar healthBar;
    private LivesBar livesBar;
    private GameSession gameSession;
    private float xMin;
    private float yMin;
    private float xMax;
    private float yMax;
    private float time_left_on_shield = 0f;
    private Coroutine fire_method;
    private float double_laser_distance = 0.5f;
    private bool isInvicable = false;
   
    public Coroutine active_shield;

    // Start is called before the first frame update
    private void Start() {
        SetupMoveBoundrys();

        douple_fire = false;
        gameSession = FindObjectOfType<GameSession>();
        healthBar = FindObjectOfType<HealthBar>();
        livesBar = FindObjectOfType<LivesBar>();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        livesBar.SetLives(gameSession.GetLives());
        StartCoroutine(MakePlayerInvincible());
    }

    // Update is called once per frame
    private void Update() {
        Move();
        Fire();
    }

    private void Fire() {
        if (Input.GetButtonDown("Fire1")) {
            fire_method = StartCoroutine(FireConinuous());
        }
        if (Input.GetButtonUp("Fire1")) {
            if (fire_method != null) {
                StopCoroutine(fire_method);
            }
        }
    }

    private IEnumerator ActivateShieldForXTime(float seconds) {
        StartCoroutine(ShowShield());
        do {
            yield return new WaitForSeconds(seconds-2);
            this.time_left_on_shield--;
        } while ((time_left_on_shield - 2) > 0);

        for (int i = 0; i < 10; i++) {
            for (int x = 0; x < shield_animation.Count; x++) {
                Renderer r = shield_animation[x].GetComponent<Renderer>();
                r.material.color = Color.black;
            }

            yield return new WaitForSeconds(0.05f);

            for (int x = 0; x < shield_animation.Count; x++) {
                Renderer r = shield_animation[x].GetComponent<Renderer>();
                r.material.color = Color.white;
            }

            yield return new WaitForSeconds(0.05f);
        }

        StartCoroutine(HideShield());
    }

    private IEnumerator ShowShield() {
        for (int i = 0; i < shield_animation.Count; i++) {
            shield_animation[i].SetActive(true);
            Renderer r = shield_animation[i].GetComponent<Renderer>();
            r.material.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
        isInvicable = true;
    }

    private IEnumerator HideShield() {
        for (int i = 0; i < shield_animation.Count; i++) {
            shield_animation[i].SetActive(false);
            yield return new WaitForSeconds(0.05f);
        }
        isInvicable = false;
    }

    private IEnumerator FireConinuous() {
        while (true) {
            if (douple_fire == true) {
                Vector3 left_laser = new Vector3(transform.position.x - double_laser_distance, transform.position.y, transform.position.z);
                Vector3 right_laser = new Vector3(transform.position.x + double_laser_distance, transform.position.y, transform.position.z);

                GameObject laser1 = Instantiate(PrefabLaser, left_laser, Quaternion.identity) as GameObject;
                laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectile_speed);

                GameObject laser2 = Instantiate(PrefabLaser, right_laser, Quaternion.identity) as GameObject;
                laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectile_speed);
            } else {
                GameObject laser = Instantiate(PrefabLaser, transform.position, Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectile_speed);
            }
            if (laserAudio != null) {
                AudioSource.PlayClipAtPoint(laserAudio, new Vector3(0,0,0));
            }
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private IEnumerator MakePlayerInvincible() {
        isInvicable = true;
        StartCoroutine(ShowShield());
        var r = GetComponent<SpriteRenderer>();
        for (int i = 0; i < temp_invencibility_on_create; i++) {
            r.material.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            r.material.color = Color.black;
            yield return new WaitForSeconds(0.05f);
        }
        r.material.color = Color.white;
        StartCoroutine(HideShield());
        isInvicable = false;
    }

    private void SetupMoveBoundrys() {
        Camera game_camera = Camera.main;
        xMin = game_camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = game_camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding; ;
        yMin = game_camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding; ;
        yMax = game_camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding; ;
    }

    private void Move() {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXpos, newYpos);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "PowerUp") {
            PowerUp power_up = other.gameObject.GetComponent<PowerUp>();
            if (power_up.power_up_type == "DOUBLE_FIRE") {
                douple_fire = true;
            } else if (power_up.power_up_type == "SHIELD") {
                if (isInvicable == false) {
                    active_shield = StartCoroutine(ActivateShieldForXTime(power_up.param_value));
                } else {
                    this.time_left_on_shield += power_up.param_value;
                    StopCoroutine(active_shield);
                    StartCoroutine(ActivateShieldForXTime(power_up.param_value));
                }
            } else if (power_up.power_up_type == "HEALTH") {
                health = Mathf.Clamp(health + (int)power_up.param_value, 0, maxHealth);
                healthBar.SetHealth(health);
            } else if (power_up.power_up_type == "LIFE") {
                var lives = Mathf.Clamp((int)gameSession.GetLives() + 1, 0, 5);
                gameSession.SetLifes((int)lives);
                livesBar.SetLives((int)lives);
            }
            AudioSource audio_source = power_up.GetComponent<AudioSource>();
            AudioClip audio = audio_source.clip;
            if (audio_source != null) {
                AudioSource.PlayClipAtPoint(audio, this.gameObject.transform.position);
            }

            Destroy(other.gameObject);
        } else {
            if (isInvicable == false) {
                if (other.CompareTag("Enemy")) {
                    health = 0;
                    Die();
                } else {
                    DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                    if (!damageDealer) { return; }
                    ProcessHit(damageDealer);
                }
            }
        }
    }

    private void ProcessHit(DamageDealer damageDealer) {
        if (isInvicable == false) {
            health -= damageDealer.GetDamage();
            damageDealer.Hit();
            healthBar.SetHealth(health);
            if (health <= 0) {
                Die();
            }
        }
    }

    //IEnumerator DelayGameOver() {
    //    yield return new WaitForSeconds(delayInSecods);
    //    SceneManager.LoadScene("GameOver");
    // }

    private void Die() {
        gameSession.LooseLife();
        livesBar.SetLives(gameSession.GetLives());
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        //Need to check if we used all our lives
        if (gameSession.GetLives() <= 0) {
            FindObjectOfType<Level>().LoadGameOver(); ;
        } else {
            FindObjectOfType<PlayerSpawner>().LoadPlayer();
        }
    }
}
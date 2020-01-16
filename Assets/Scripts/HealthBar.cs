using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField]  private GameObject bar;
    [SerializeField] private int maxHealth;
    [SerializeField] private float currentHealth;

    // Start is called before the first frame update
    private float normalizedMaxHealth;


    public void SetMaxHealth(int maxHealth) {
        this.maxHealth = maxHealth;
    }
    public void SetHealth(float health) {
        float normHealth = health/maxHealth;
        currentHealth = health;
        bar.transform.localScale = new Vector3(normHealth, 1f,1f);
    }
}
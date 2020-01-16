using System.Collections;
using UnityEngine;

public class ShieldHit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "EnemyLaser") {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            damageDealer.Hit();
            StartCoroutine(AnimateHit());
        }
    }

    private IEnumerator AnimateHit() {
        var r = GetComponent<SpriteRenderer>();
        r.material.color = Color.magenta;
        yield return new WaitForSeconds(0.05f);
        r.material.color = Color.white;
    }
}
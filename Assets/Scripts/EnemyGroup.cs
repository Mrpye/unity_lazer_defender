using UnityEngine;

public class EnemyGroup : MonoBehaviour {
    [SerializeField] private bool rotate = false;
    [SerializeField] private float rotate_speed = -1f;

    // Start is called before the first frame update
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        Enemy[] enemy = gameObject.GetComponentsInChildren<Enemy>();
        if (enemy.Length <= 0) {
            Destroy(gameObject);
        } else {
            if (rotate == true) {
                //gameObject.transform.Rotate(0, 0, rotate_speed, Space.Self);
                gameObject.transform.Rotate(0, 0, rotate_speed, Space.Self);

                foreach (Transform go in gameObject.transform) {
                    go.Rotate(0, 0, -rotate_speed, Space.Self);  
                }


            }
        }
    }
}
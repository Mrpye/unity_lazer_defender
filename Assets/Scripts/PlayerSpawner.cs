using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    [SerializeField] private GameObject player;

    public void LoadPlayer() {
        StartCoroutine(LoadPlayerDelayed());
    }
    // Start is called before the first frame update
    private IEnumerator LoadPlayerDelayed() {
        yield return new WaitForSeconds(2f);
        Instantiate<GameObject>(player,new Vector2(0f, -8.46f), Quaternion.identity);
    }
}
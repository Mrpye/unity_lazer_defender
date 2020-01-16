using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesBar : MonoBehaviour
{
    [SerializeField] List<GameObject> livesIcons;
    public void SetLives(int lives) {
        for(int i =0; i< livesIcons.Count; i++) {
            if (i< lives) {
                livesIcons[i].SetActive(true);
            } else {
                livesIcons[i].SetActive(false);
            }
        }
    }

}

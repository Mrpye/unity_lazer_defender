using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float minSpeedOfRotation = 1f;
    [SerializeField] float maxSpeedOfRotation = 10f;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Random.Range(minSpeedOfRotation, maxSpeedOfRotation));
    }
}

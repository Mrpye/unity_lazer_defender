using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wav Config")]
public class WaveConfig : ScriptableObject {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float spawnRandomFactor = 0.3f;
    [SerializeField] private int numberOfEnemies = 5;
    [SerializeField] public float MoveSpeed = 2f;

    [SerializeField] public bool HasSecondMoveSpeed = false;
    [SerializeField] public int SecondMoveSpeedAt = 0;
    [SerializeField] public float SecondmoveSpeed = 2f;

    [Header("Audio")]
    [SerializeField] public AudioClip intro_music;

    [SerializeField] public AudioClip music;
    [SerializeField] public bool force_intro = false;


    [Header("Looping")]
    [SerializeField] public bool loop = false;

    [SerializeField] public int start_loop = 0;

   

    [Header("Circling")]
    [SerializeField] public bool circling = false;
    [SerializeField] public float radius = 3;
    [SerializeField] public float radius_increase_factor = 0.01f;
    [SerializeField] public float max_radius = 10f;

    [Header("Sin")]
    [SerializeField] public bool sin = false;
    [SerializeField] private float amplitudeX = 10.0f;
    [SerializeField] private float amplitudeY = 5.0f;
    [SerializeField] private float omegaX = 5.0f;
    [SerializeField] private float omegaY = 1.0f;

    [Header("Circling/Sin shared")]
    [SerializeField] public int point_steps = 20;
    [SerializeField] public float angle_factor = 1f;



    [Header("Next Wave")]
    [SerializeField] public bool wait_until_x_left = false;

    [SerializeField] public int wait_until_x_ammount = 0;

    public GameObject GetEnemyPrefab() {
        return enemyPrefab;
    }

    public List<Vector3> GenerateCircle1(Vector3 t) {
        var waveWayPoints = new List<Vector3>();
        float steps = 2 * Mathf.PI / 20;

        for (float i = 0; i < (2 * Mathf.PI); i += steps) {
            var offset = new Vector3(Mathf.Sin(i), Mathf.Cos(i), t.z) * radius;
            Vector3 newTransform = new Vector3(t.x, t.y, t.z) + offset;
            waveWayPoints.Add(newTransform);
        }
        return waveWayPoints;
    }

    public List<Vector3> GenerateCircle(Vector3 t) {
        var waveWayPoints = new List<Vector3>();


        float angle = angle_factor * (Mathf.PI / 180);//* angle_factor
        float steps = (angle / point_steps);
        //float steps = (2 * Mathf.PI / point_steps);
        //float angle = (2 * Mathf.PI);//* angle_factor
        var new_radius = this.radius;
        for (float i = 0; i <= angle; i += steps) {
             new_radius +=   radius_increase_factor;
            new_radius = Mathf.Clamp(new_radius, 0, max_radius);

            var offset = new Vector3(Mathf.Sin(i), Mathf.Cos(i), t.z) * new_radius;
            Vector3 newTransform = new Vector3(t.x, t.y, t.z) + offset;
            waveWayPoints.Add(newTransform);
        }
        return waveWayPoints;
    }

    public List<Vector3> SinPoints(Vector3 t) {
        var waveWayPoints = new List<Vector3>();
        float angle = angle_factor * (Mathf.PI / 180);//* angle_factor
        float steps = 2 * Mathf.PI / point_steps;

        for (float i = 0; i < angle; i += steps) {
            float x = amplitudeX * Mathf.Cos(omegaX * i);
            float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * i));

            var offset = new Vector3(x, y, t.z);
            Vector3 newTransform = new Vector3(t.x, t.y, t.z) + offset;
            waveWayPoints.Add(newTransform);
        }
        return waveWayPoints;
    }

    public List<Vector3> GetWayPoints() {
        var waveWayPoints = new List<Vector3>();
        foreach (Transform child in pathPrefab.transform) {
            Vector3 wp = new Vector3(child.transform.position.x, child.transform.position.y);
            waveWayPoints.Add(wp);
        }
        Vector3 t = waveWayPoints[waveWayPoints.Count - 1];
        if (circling == true) {
            start_loop = waveWayPoints.Count + 1;
            waveWayPoints.AddRange(GenerateCircle(t));
        } else if (sin == true) {
            start_loop = waveWayPoints.Count + 1;
            waveWayPoints.AddRange(SinPoints(t));
        }

        return waveWayPoints;
    }

    public float GetTimeBetweenSpawns() {
        return timeBetweenSpawns;
    }

    public float GetSpawnRandomFactor() {
        return spawnRandomFactor;
    }

    public int GetNumberOfEnemies() {
        return numberOfEnemies;
    }

    public float GetMoveSpeed() {
        return MoveSpeed;
    }
}
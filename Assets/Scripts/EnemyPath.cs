using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour {
    private WaveConfig waveConfig;
    private List<Vector3> waypoints;

    private int waypointIndex = 0;
    private bool exit_pos = false;

    // Start is called before the first frame update
    private void Start() {
        if (waveConfig == null) { return; }
        waypoints = waveConfig.GetWayPoints();
        if (waypoints == null) { return; }
        transform.position = waypoints[waypointIndex];
    }

    public void SetWaveConfig(WaveConfig waveConfig) {
        this.waveConfig = waveConfig;
    }

    // Update is called once per frame
    private void Update() {
        Move();
    }

    private void Move() {
        float MoveSpeed = 0f;
        Vector3 targetPosition;
        if (waypoints == null) { return; }
        if (waypointIndex <= waypoints.Count - 1 || exit_pos == true) {

            if (exit_pos == true) {
                 targetPosition = waypoints[0];
            } else {
                 targetPosition = waypoints[waypointIndex];
            }

            

            if (waypointIndex >= waveConfig.SecondMoveSpeedAt && waveConfig.HasSecondMoveSpeed) {
                MoveSpeed = waveConfig.SecondmoveSpeed;
            } else {
                MoveSpeed = waveConfig.MoveSpeed;
            }

            var movementThisFrame = MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition) {
                if (waveConfig.loop == true) {
                    if (waypointIndex >= waypoints.Count - 1) {
                        waypointIndex = waveConfig.start_loop;
                    } else {
                        waypointIndex++;
                    }
                } else {
                    waypointIndex++;
                    if (exit_pos == true) { Destroy(gameObject); }
                    if (waypointIndex > waypoints.Count - 1 && (waveConfig.circling == true || waveConfig.sin == true)) {
                        exit_pos = true;
                    } 
                }
               
            }
            
        } else {
            Destroy(gameObject);
        }
    }
}
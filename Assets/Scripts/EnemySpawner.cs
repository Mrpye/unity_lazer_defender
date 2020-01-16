using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private List<WaveConfig> waveConfig;
    [SerializeField] private MusicPlayer music_player;
    [SerializeField] private int play_from;

    private int startingWave = 0;
    [SerializeField] private bool looping = false;

    // Start is called before the first frame update
    private IEnumerator Start() {
        startingWave = play_from;
        do {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves() {
        for (int i = startingWave; i < waveConfig.Count; i++) {
            var currentWave = waveConfig[i];
            yield return StartCoroutine(SpawnAllEnemiesInWav(currentWave));
            if (currentWave.wait_until_x_left) {
                var e = FindObjectsOfType<Enemy>();
                while (e.Length > currentWave.wait_until_x_ammount) {
                    yield return new WaitForSeconds(1);
                    e = FindObjectsOfType<Enemy>();
                }
            }
        }
    }

    private IEnumerator SpawnAllEnemiesInWav(WaveConfig waveConfig) {
        if (music_player != null) {
            AudioSource audio_source = music_player.GetComponent<AudioSource>();

            if (waveConfig.intro_music != null && waveConfig.music != null) {
                music_player.playMusic(waveConfig.intro_music, waveConfig.music, waveConfig.force_intro);
            } else if (waveConfig.intro_music == null && waveConfig.music != null) {
                music_player.playMusic(waveConfig.music);
            } else {
                music_player.playMusic();
            }
        }
        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++) {
            var newEnemy = Instantiate(
                   waveConfig.GetEnemyPrefab(),
                   waveConfig.GetWayPoints()[0],
                   Quaternion.identity);
            newEnemy.GetComponent<EnemyPath>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
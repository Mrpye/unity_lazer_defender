using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    [SerializeField] public AudioClip default_Intro;
    [SerializeField] public AudioClip default_music;

    private AudioClip current_intro;
    private AudioClip current_music;
    private bool currently_playng_intro = false;
    private AudioSource audio_source;
    private Coroutine cr;

    // Start is called before the first frame update
    private void Awake() {
        SetUpSingleton();
    }

    private void SetUpSingleton() {
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void playMusic() {
        if (default_Intro != null && default_music != null) {
            playMusic(default_Intro, default_music);
        } else if (default_Intro == null && default_music != null) {
            playMusic(default_music);
        }
    }

    public void playMusic(AudioClip music) {
        audio_source = gameObject.GetComponent<AudioSource>();
        if (current_music != null && current_music.name == music.name) { return; }
        audio_source.Stop();
        currently_playng_intro = false;
        audio_source.clip = music;
        current_music = music;
        current_intro = null;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void playMusic(AudioClip Intro, AudioClip music, bool force_intro = false) {
        audio_source = gameObject.GetComponent<AudioSource>();
        if (force_intro == false) {
            if (current_music != null && current_intro != null && currently_playng_intro == true && current_intro.name == Intro.name) { return; }
            if (current_music != null && current_intro != null && currently_playng_intro == false && current_music.name == music.name) { return; }
        }
        audio_source.Stop();
        currently_playng_intro = false;
        if (cr != null) { StopCoroutine(cr); }
        Coroutine ct = StartCoroutine(playMusicWithIntro(Intro, music));
    }

    private IEnumerator playMusicWithIntro(AudioClip Intro, AudioClip music) {
        audio_source.clip = Intro;
        current_intro = Intro;
        current_music = music;
        currently_playng_intro = true;
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(Intro.length - 1);
        currently_playng_intro = false;
        audio_source.clip = music;
        gameObject.GetComponent<AudioSource>().Play();
    }
}
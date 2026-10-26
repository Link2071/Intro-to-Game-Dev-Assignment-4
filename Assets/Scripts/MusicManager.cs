using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicStates
{
    start,
    normal,
    danger,
    recover,
}

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicClips;
    public static MusicManager instance; 
    private AudioSource _audioSource;
    private float _scaredTime;
    private float _timer;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (instance != null && instance != this) Destroy(this);
        instance = this;

        StartCoroutine(PlayMusicWithDelay(4, MusicStates.normal));
    }

    void Start()
    {
        if(_audioSource != null) _audioSource.PlayOneShot(musicClips[(int)MusicStates.start]);
    }

    void Update(){
        if (GhostManager.instance.globalGhostState == GhostStates.danger)
        {
            _timer -= Time.deltaTime;
            if (_timer >= 0)
            {
                PlayMusic(MusicStates.recover);
            }
        }
    }

    private IEnumerator PlayMusicWithDelay(float delay, MusicStates newMusic)
    {
        yield return new WaitForSeconds(delay);
        _audioSource.Stop();
        _audioSource.clip = musicClips[(int)newMusic];
        _audioSource.Play();
    }

    public void PlayMusic(MusicStates newState){
        if (musicClips[(int)newState] == _audioSource.clip)
        {
            if (_audioSource.clip == musicClips[(int)MusicStates.danger])
            {
                _timer = _scaredTime;
            }
            return;
        }

        _audioSource.Stop();
        switch (newState)
        {
            case MusicStates.normal:
                _audioSource.clip = musicClips[(int)MusicStates.normal];
                break;
            case MusicStates.danger:
                _audioSource.clip = musicClips[(int)MusicStates.danger];
                _timer = _scaredTime;
                break;
            case MusicStates.recover:
                _audioSource.clip = musicClips[(int)MusicStates.recover];
                break;
        }
        _audioSource.Play();
    }
}

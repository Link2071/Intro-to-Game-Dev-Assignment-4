using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip dangerMusic;
    [SerializeField] private AudioClip deathMusic;
    private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(menuMusic);
        Invoke("playLevelMusic", 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(_audioSource.isPlaying == false)
        {
            _audioSource.Play();
        }
    }

    void playLevelMusic(){
        _audioSource.Stop();
        _audioSource.Play();
    }
}

using System.Collections;
using UnityEngine;

public class PlayerSoundFx : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(playMovingSound());
    }

    IEnumerator playMovingSound()
    {
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(1);
        }
    }
}

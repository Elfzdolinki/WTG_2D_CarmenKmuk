using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] private AudioSource musicSource;

    [Header("--- Audio Clips ---")]
    public AudioClip background;

    // Metoda do odtwarzania muzyki w tle
    public void PlayBackgroundMusic()
    {
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true; // Odtwarzanie w pêtli
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("MusicSource or Background clip is not assigned!");
        }
    }

    // Metoda do zmiany g³oœnoœci
    public void SetVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp(volume, 0f, 1f); // Upewnij siê, ¿e wartoœæ jest miêdzy 0 a 1
        }
        else
        {
            Debug.LogWarning("MusicSource is not assigned!");
        }
    }
}
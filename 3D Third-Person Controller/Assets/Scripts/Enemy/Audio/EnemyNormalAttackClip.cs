using UnityEngine;

public class EnemyNormalAttackClip : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] private AudioClip[] audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], 0.5f);
    }
}

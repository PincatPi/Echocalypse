using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//TODO: 音频播放可以重构
public class PlayCombatBGM : MonoBehaviour
{
    private EnemyCombatController enemyCombatController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField][Range(0f, 1f)] private float volume;
    void Start()
    {
        enemyCombatController = GetComponent<EnemyCombatController>();
    }
    
    void Update()
    {
        //PlayBGM();
    }

    private void PlayBGM()
    {
        if (enemyCombatController.GetCurrentTarget() && audioSource.mute == true)
        {
            audioSource.Play();
            audioSource.volume = volume;
            audioSource.mute = false;
        }
    }
}

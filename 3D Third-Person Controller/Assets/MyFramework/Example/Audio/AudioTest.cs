using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    private AudioSource audioSource;
    
    private float v = 0f;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Play Audio Test"))
        {
            v = 0f;
            AudioManager.GetInstance().ChangeBackgroundMusicVolume(0f);
            AudioManager.GetInstance().PlayBackgroundMusic("Test/普通朋友", 0.1f);
        }
        
        if (GUI.Button(new Rect(0, 100, 100, 100), "Pause Audio Test"))
            AudioManager.GetInstance().PauseBackgroundMusic();
        if (GUI.Button(new Rect(0, 200, 100, 100), "Stop Audio Test"))
            AudioManager.GetInstance().StopBackgroundMusic();
        if (GUI.Button(new Rect(0, 300, 100, 100), "Play Sound Test"))
            AudioManager.GetInstance().PlaySound("Test/早上好", 1f, false, (audioSource) => 
            {
                this.audioSource = audioSource;
            });
        if (GUI.Button(new Rect(0, 400, 100, 100), "Stop Sound Test"))
            AudioManager.GetInstance().StopSound(audioSource);
        
        AudioManager.GetInstance().ChangeBackgroundMusicVolume(v);
        v += Time.deltaTime / 100;
    }
}

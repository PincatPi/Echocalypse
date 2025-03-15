using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音频管理模块
/// </summary>
public class AudioManager : SingletonPatternBase<AudioManager>
{
    //背景音乐
    private AudioSource backgroundMusic = null;
    
    //音效
    private GameObject soundObj = null;
    private List<AudioSource> soundsList = new List<AudioSource>();

    public AudioManager()
    {
        MonoManager.GetInstance().AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = soundsList.Count - 1; i >= 0; --i)
        {
            if (!soundsList[i].isPlaying)
            {
                //TODO: 此处可以使用缓存池来优化
                GameObject.Destroy(soundsList[i]);
                soundsList.RemoveAt(i);
            }
        }
    }

    
    #region 背景音乐
    
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="path">背景音乐资源路径</param>
    /// <param name="volume">背景音乐音量大小</param>
    public void PlayBackgroundMusic(string path, float volume)
    {
        if (backgroundMusic == null)
        {
            GameObject obj = new GameObject("BackgroundMusic");
            backgroundMusic = obj.AddComponent<AudioSource>();
        }
        //异步加载背景音乐后播放
        ResourcesLodingManager.GetInstance().LoadAsync<AudioClip>(path, (clip) =>
        {
            backgroundMusic.clip = clip;
            backgroundMusic.loop = true;
            backgroundMusic.volume = volume;
            backgroundMusic.Play();
        });
    }

    /// <summary>
    /// 改变背景音乐音量大小
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeBackgroundMusicVolume(float volume)
    {
        if(backgroundMusic == null)
            return;
        backgroundMusic.volume = volume;
    }
    
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBackgroundMusic()
    {
        if(backgroundMusic == null)
            return;
        backgroundMusic.Pause();
    }
    
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBackgroundMusic()
    {
        if(backgroundMusic == null)
            return;
        backgroundMusic.Stop();
    }

    /// <summary>
    /// 静音背景音乐
    /// </summary>
    public void MuteBackgroundMusic()
    {
        if(backgroundMusic == null)
            return;
        backgroundMusic.mute = true;
    }
    
    #endregion

    
    #region 音效
    //TODO: 音效的部分可以应用缓存池来进行优化
    //

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path">音效路径名</param>
    /// <param name="volume">音量大小</param>
    /// <param name="isLoop">该音效是否循环播放</param>
    /// <param name="callback">用于外部获取新增的audioSource的回调函数</param>
    public void PlaySound(string path, float volume, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if (!soundObj)
        {
            soundObj = new GameObject("Sounds");
        }
        
        ResourcesLodingManager.GetInstance().LoadAsync<AudioClip>(path, (clip) => 
        {
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.volume = volume;
            audioSource.Play();
            soundsList.Add(audioSource);
            if (callback != null)
            {
                callback.Invoke(audioSource);
            }
        });
    }

    public void PlaySound(AudioClip clip, float volume, bool isLoop, UnityAction<AudioSource> callback = null)
    {
        if (!soundObj)
        {
            soundObj = new GameObject("Sounds");
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.volume = volume;
            audioSource.Play();
            soundsList.Add(audioSource);
            if (callback != null)
            {
                callback.Invoke(audioSource);
            }
        }
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource audioSource)
    {
        if (soundsList.Contains(audioSource))
        {
            soundsList.Remove(audioSource);
            audioSource.Stop();
            GameObject.Destroy(audioSource);
        }
    }

    /// <summary>
    /// 改变该脚本管理的所有音效的音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSoundVolume(float volume)
    {
        for (int i = 0; i < soundsList.Count; i++)
        {
            soundsList[i].volume = volume;
        }
    }
    
    #endregion
}

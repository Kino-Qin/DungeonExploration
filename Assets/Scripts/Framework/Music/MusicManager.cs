using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���ֹ�����
/// </summary>
public class MusicManager : Singleton<MusicManager>
{
    private AudioSource _bgmSource;
    private float _bgmVolume;
    private float _soundVolume;
    private List<AudioSource> _soundSources = new List<AudioSource>();
    private List<AudioSource> _delayRemoveSources = new List<AudioSource>();
    private const string SOUND_OBJECT_PATH = "Music/Sound/SoundObject";
    private const string MUSIC_PATH = "Music/Music/";
    private const string SOUND_PATH = "Music/Sound/";

    public MusicManager()
    {
        MonoManager.Instance.AddUpdateListener(Update);
    }
    private void Update()
    {
        foreach (AudioSource item in _soundSources)
        {
            if (!item.isPlaying)
            {
                _delayRemoveSources.Add(item);
            }
        }
        foreach (AudioSource item in _delayRemoveSources)
        {
            _soundSources.Remove(item);
            PoolManager.Instance.Push(SOUND_OBJECT_PATH, item.gameObject);
        }
        _delayRemoveSources.Clear();
    }
    #region Music
    /// <summary>
    /// ���ű�������
    /// </summary>
    /// <param name="musicName">��������·��</param>
    public void PlayMusic(string musicName)
    {
        if (_bgmSource == null)
        {
            GameObject obj = new GameObject("BGM");
            _bgmSource = obj.AddComponent<AudioSource>();
        }
        ResourcesManager.Instance.LoadAsync<AudioClip>($"{MUSIC_PATH}{musicName}", (clip) =>
        {
            _bgmSource.clip = clip;
            _bgmSource.volume = _bgmVolume;
            _bgmSource.Play();
        });
    }
    /// <summary>
    /// ��ͣ�������֣��ٴβ���ͬһ�ױ�������ʱ��������ţ�
    /// </summary>
    public void PauseMusic()
    {
        if (_bgmSource == null)
        {
            return;
        }
        _bgmSource.Pause();
    }
    /// <summary>
    /// ֹͣ��������
    /// </summary>
    public void StopMusic()
    {
        if (_bgmSource == null)
        {
            return;
        }
        _bgmSource.Stop();
    }
    /// <summary>
    /// ���ñ������ִ�С
    /// </summary>
    /// <param name="value"></param>
    public void SetMusicVolume(float value)
    {
        _bgmVolume = value;
        if (_bgmSource != null)
        {
            _bgmSource.volume = value;
        }
    }
    #endregion

    #region Sound
    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="soundName">��Ч�ļ�·��</param>
    /// <param name="callback">�ص�����������Ϊ������Ч��AudioSource</param>
    public void PlaySound(string soundName, UnityAction<AudioSource> callback = null)
    {
        ResourcesManager.Instance.LoadAsync<AudioClip>($"{SOUND_PATH}{soundName}", (clip) =>
        {
            PoolManager.Instance.Pop(SOUND_OBJECT_PATH, (obj) =>
            {
                AudioSource audioSource = obj.GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.volume = _soundVolume;
                audioSource.Play();
                _soundSources.Add(audioSource);
                callback?.Invoke(audioSource);
            });
        });
    }
    /// <summary>
    /// ֹͣ��Ч
    /// </summary>
    /// <param name="audioSource">��Ҫֹͣ����Ч��AudioSource</param>
    public void StopSound(AudioSource audioSource)
    {
        if (_soundSources.Contains(audioSource))
        {
            audioSource.Stop();
            _soundSources.Remove(audioSource);
            PoolManager.Instance.Push(SOUND_OBJECT_PATH, audioSource.gameObject);
        }
    }
    /// <summary>
    /// ������Ч��С
    /// </summary>
    /// <param name="value"></param>
    public void SetSoundVolume(float value)
    {
        _soundVolume = value;
        foreach (AudioSource item in _soundSources)
        {
            item.volume = _soundVolume;
        }
    }
    #endregion

}

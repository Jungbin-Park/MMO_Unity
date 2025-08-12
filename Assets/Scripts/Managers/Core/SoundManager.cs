using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        audioClips.Clear();
    }

    // 경로를 받는 버전
    public void Play(string _path, Define.Sound _type = Define.Sound.Effect, float _pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(_path, _type);
        Play(audioClip, _type, _pitch);
    }

    // 오디오 클립을 받는 버전
    public void Play(AudioClip _audioClip, Define.Sound _type = Define.Sound.Effect, float _pitch = 1.0f)
    {
        if (_audioClip == null) return;

        if (_type == Define.Sound.Bgm)
        {
            AudioSource audioSource = audioSources[(int)Define.Sound.Bgm];

            // 이미 노래가 재생 중이라면 노래 종료 후 재생
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = _pitch;
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else if (_type == Define.Sound.Effect)
        {
            AudioSource audioSource = audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = _pitch;
            //audioSource.spatialBlend = 1.0f;
            //audioSource.rolloffMode = AudioRolloffMode.Linear;
            //audioSource.minDistance = 1.0f;
            //audioSource.maxDistance = 30.0f;
            audioSource.PlayOneShot(_audioClip);
        }
    }

    // 캐싱 역할의 오디오클립 반환 함수
    AudioClip GetOrAddAudioClip(string _path, Define.Sound _type = Define.Sound.Effect)
    {
        // BGM이 아닌 Effect 사운드의 경우 빈번하게 호출되므로 그 때마다 Load를 실행하면 부하가 올 수 있음
        if (_path.Contains("Sounds/") == false)
            _path = $"Sounds/{_path}";

        AudioClip audioClip = null;

        if (_type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(_path);
        }
        else if (_type == Define.Sound.Effect)
        {
            // audioClips 배열에 없는 경우에만 Load로 불러오고 있으면 배열에서 꺼내와서 반환
            if (audioClips.TryGetValue(_path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(_path);
                audioClips.Add(_path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {_path}");

        return audioClip;
    }
}

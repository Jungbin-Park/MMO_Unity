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

    // ��θ� �޴� ����
    public void Play(string _path, Define.Sound _type = Define.Sound.Effect, float _pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(_path, _type);
        Play(audioClip, _type, _pitch);
    }

    // ����� Ŭ���� �޴� ����
    public void Play(AudioClip _audioClip, Define.Sound _type = Define.Sound.Effect, float _pitch = 1.0f)
    {
        if (_audioClip == null) return;

        if (_type == Define.Sound.Bgm)
        {
            AudioSource audioSource = audioSources[(int)Define.Sound.Bgm];

            // �̹� �뷡�� ��� ���̶�� �뷡 ���� �� ���
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

    // ĳ�� ������ �����Ŭ�� ��ȯ �Լ�
    AudioClip GetOrAddAudioClip(string _path, Define.Sound _type = Define.Sound.Effect)
    {
        // BGM�� �ƴ� Effect ������ ��� ����ϰ� ȣ��ǹǷ� �� ������ Load�� �����ϸ� ���ϰ� �� �� ����
        if (_path.Contains("Sounds/") == false)
            _path = $"Sounds/{_path}";

        AudioClip audioClip = null;

        if (_type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(_path);
        }
        else if (_type == Define.Sound.Effect)
        {
            // audioClips �迭�� ���� ��쿡�� Load�� �ҷ����� ������ �迭���� �����ͼ� ��ȯ
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

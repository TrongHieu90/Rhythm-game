using System;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public Sound[] Sounds;
	void Awake()
	{
		foreach (var sound in Sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.lopp;
		}
	}

	private void Start()
	{
		Play("MainTheme");
	}

    public void Play(string name)
    {
	    var s = Array.Find(Sounds, sound => sound.name == name);
	    s?.source.Play();
    }

    public void Play(string name, float pitch)
    {
	    var s = Array.Find(Sounds, sound => sound.name == name);
	    if (s != null)
	    {
		    s.source.pitch = pitch;
		    s.source.Play();
		}
    }

    public void Stop(string name)
    {
	    var s = Array.Find(Sounds, sound => sound.name == name);
	    s?.source.Stop();
    }
}

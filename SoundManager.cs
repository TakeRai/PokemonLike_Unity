using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioSource bgmSource;
	public AudioSource seSource;

	public AudioClip startBgm;
	[SerializeField] List<AudioClip> battlebgms;



	public AudioClip StartBgm
    {
        get { return startBgm; }
    }

	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		seSource.clip = clip;

		//Play the clip.
		seSource.Play();
	}

	public void startBGMplay()
    {
		bgmSource.clip = startBgm;
		bgmSource.Play();
    }

	public void battleBGMplay()
    {
		int r = Random.Range(0, battlebgms.Count);

		bgmSource.clip = battlebgms[r];

		bgmSource.Play();
    }
}

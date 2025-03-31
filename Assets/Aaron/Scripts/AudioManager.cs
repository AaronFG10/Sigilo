using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] musicSource;
    private AudioSource[]sfxSource;
    
    public static AudioManager instance;

    private void Awake()
    {
    if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    else
        {
            Destroy(gameObject);
        }

    musicSource=new AudioSource[2];
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i]= gameObject.AddComponent<AudioSource>();
            musicSource[i].playOnAwake = false;
            musicSource[i].loop = true;
        }
        sfxSource=new AudioSource[4];
        for (int i = 0;i < sfxSource.Length; i++)
        {
            sfxSource[i]=gameObject.AddComponent<AudioSource>();
            sfxSource[i].playOnAwake=false;
        }
        
    }

    public void PlayMusic(AudioClip song, float volume)
    {
        for(int i = 0;i<musicSource.Length;i++)
        {
            if(musicSource[i].isPlaying)
            {
                musicSource[i].clip = song;
                musicSource[i].volume = volume;
                musicSource[i].Play();
            }
        }
        musicSource[0].clip = song;
        musicSource[0].volume = volume;
        musicSource[0].Play();
    }
    public void PlayMusicFde(AudioClip song, float volume, float fadeduration)
    {
        AudioSource init = null;
        AudioSource next = null;
        for (int i = 0; i < musicSource.Length; i++)
        {

            if (musicSource[i].isPlaying)
            {
                init = musicSource[i];
            }
            else
            {
                musicSource[i].volume = volume;
                next = musicSource[i];
            }

        }
        next.clip = song;
        next.Play();

        StartCoroutine(FadeMusic(init, next, fadeduration));
    }

    IEnumerator FadeMusic(AudioSource initial,AudioSource nextSource, float fadeDuration)
    {
        float t = 0;
        float volumenPorcentaje=t/fadeDuration;
        float initialVolume=initial.volume;
        float nextVolume=nextSource.volume;
        while(t<fadeDuration)
        {
            t += Time.deltaTime;
            initial.volume = initialVolume*(1-volumenPorcentaje);
            nextSource.volume = nextVolume*volumenPorcentaje;
            volumenPorcentaje=t/fadeDuration ;
            yield return null;
        }
        yield return null;

    }

    public void PlaySFX(AudioClip sfx, float volume)
    {
        for(int i = 0;i<sfxSource.Length;i++)
        {
            if(sfxSource[i].isPlaying==false)
            {
                sfxSource[i].clip = sfx;
                sfxSource[i].volume = volume;
                sfxSource[i].Play();
                return;
            }
          
        }
        
    }

    public void StopSong()
    {
        for(int i = 0;i < musicSource.Length;i++)
        {
            musicSource[i].Stop();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioClipData
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;
}

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    public AudioClipData backgroundMusic;
    public AudioClipData menuMusic;
    
    [Header("Player Sounds")]
    public AudioClipData jumpSound;
    public AudioClipData walkSound;
    public AudioClipData deathSound;
    public AudioClipData extraLifeSound;
    
    [Header("Game Sounds")]
    public AudioClipData barrelRollSound;
    public AudioClipData barrelBounceSound;
    public AudioClipData barrelDestroySound;
    public AudioClipData hammerPickupSound;
    public AudioClipData hammerSwingSound;
    public AudioClipData hammerHitSound;
    
    [Header("DK Sounds")]
    public AudioClipData dkRoarSound;
    public AudioClipData barrelThrowSound;
    
    [Header("UI Sounds")]
    public AudioClipData buttonClickSound;
    public AudioClipData gameOverSound;
    public AudioClipData levelCompleteSound;
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    
    [Header("Settings")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    // Singleton instance
    public static AudioManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            CreateDefaultAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayMenuMusic();
    }
    
    void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }
        
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;
        }
        
        if (ambientSource == null)
        {
            GameObject ambientObj = new GameObject("AmbientSource");
            ambientObj.transform.SetParent(transform);
            ambientSource = ambientObj.AddComponent<AudioSource>();
            ambientSource.loop = true;
            ambientSource.volume = sfxVolume * 0.5f;
        }
    }
    
    void CreateDefaultAudioClips()
    {
        // Create procedural audio clips if none are assigned
        if (jumpSound.clip == null)
        {
            jumpSound.clip = CreateJumpSound();
            jumpSound.volume = 0.7f;
            jumpSound.pitch = 1.2f;
        }
        
        if (walkSound.clip == null)
        {
            walkSound.clip = CreateWalkSound();
            walkSound.volume = 0.4f;
            walkSound.loop = true;
        }
        
        if (barrelRollSound.clip == null)
        {
            barrelRollSound.clip = CreateBarrelRollSound();
            barrelRollSound.volume = 0.3f;
            barrelRollSound.loop = true;
        }
        
        if (hammerHitSound.clip == null)
        {
            hammerHitSound.clip = CreateHammerHitSound();
            hammerHitSound.volume = 0.8f;
            hammerHitSound.pitch = 1.3f;
        }
        
        if (backgroundMusic.clip == null)
        {
            backgroundMusic.clip = CreateBackgroundMusic();
            backgroundMusic.volume = 0.6f;
            backgroundMusic.loop = true;
        }
        
        if (deathSound.clip == null)
        {
            deathSound.clip = CreateDeathSound();
            deathSound.volume = 0.8f;
            deathSound.pitch = 0.8f;
        }
        
        if (levelCompleteSound.clip == null)
        {
            levelCompleteSound.clip = CreateLevelCompleteSound();
            levelCompleteSound.volume = 0.9f;
        }
    }
    
    AudioClip CreateJumpSound()
    {
        return CreateToneClip("JumpSound", 440f, 0.2f, AudioType.Square);
    }
    
    AudioClip CreateWalkSound()
    {
        return CreateNoiseClip("WalkSound", 0.1f, NoiseType.Brown);
    }
    
    AudioClip CreateBarrelRollSound()
    {
        return CreateNoiseClip("BarrelRollSound", 0.5f, NoiseType.Pink);
    }
    
    AudioClip CreateHammerHitSound()
    {
        return CreateToneClip("HammerHitSound", 220f, 0.3f, AudioType.Sawtooth);
    }
    
    AudioClip CreateBackgroundMusic()
    {
        return CreateMelodyClip("BackgroundMusic", new float[] { 440f, 523f, 659f, 784f }, 2f);
    }
    
    AudioClip CreateDeathSound()
    {
        return CreateSweepClip("DeathSound", 880f, 220f, 1f);
    }
    
    AudioClip CreateLevelCompleteSound()
    {
        return CreateMelodyClip("LevelCompleteSound", new float[] { 523f, 659f, 784f, 1047f }, 0.8f);
    }
    
    enum AudioType { Sine, Square, Sawtooth, Triangle }
    enum NoiseType { White, Pink, Brown }
    
    AudioClip CreateToneClip(string name, float frequency, float duration, AudioType waveType)
    {
        int sampleRate = 44100;
        int samples = Mathf.RoundToInt(sampleRate * duration);
        float[] clipData = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float wave = 0f;
            
            switch (waveType)
            {
                case AudioType.Sine:
                    wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
                    break;
                case AudioType.Square:
                    wave = Mathf.Sign(Mathf.Sin(2f * Mathf.PI * frequency * t));
                    break;
                case AudioType.Sawtooth:
                    wave = 2f * (frequency * t - Mathf.Floor(frequency * t + 0.5f));
                    break;
                case AudioType.Triangle:
                    wave = 2f * Mathf.Abs(2f * (frequency * t - Mathf.Floor(frequency * t + 0.5f))) - 1f;
                    break;
            }
            
            // Apply envelope (fade in/out)
            float envelope = 1f;
            if (t < 0.05f) envelope = t / 0.05f;
            else if (t > duration - 0.05f) envelope = (duration - t) / 0.05f;
            
            clipData[i] = wave * envelope * 0.5f;
        }
        
        AudioClip clip = AudioClip.Create(name, samples, 1, sampleRate, false);
        clip.SetData(clipData, 0);
        return clip;
    }
    
    AudioClip CreateNoiseClip(string name, float duration, NoiseType noiseType)
    {
        int sampleRate = 44100;
        int samples = Mathf.RoundToInt(sampleRate * duration);
        float[] clipData = new float[samples];
        
        System.Random random = new System.Random();
        float lastSample = 0f;
        
        for (int i = 0; i < samples; i++)
        {
            float noise = 0f;
            
            switch (noiseType)
            {
                case NoiseType.White:
                    noise = (float)(random.NextDouble() * 2.0 - 1.0);
                    break;
                case NoiseType.Pink:
                    // Simple pink noise approximation
                    noise = (float)(random.NextDouble() * 2.0 - 1.0);
                    noise = (lastSample * 0.99f) + (noise * 0.01f);
                    lastSample = noise;
                    break;
                case NoiseType.Brown:
                    // Brown noise (Brownian)
                    noise = (float)(random.NextDouble() * 2.0 - 1.0) * 0.1f;
                    lastSample += noise;
                    if (lastSample > 1f || lastSample < -1f) lastSample *= 0.95f;
                    noise = lastSample;
                    break;
            }
            
            clipData[i] = noise * 0.3f;
        }
        
        AudioClip clip = AudioClip.Create(name, samples, 1, sampleRate, false);
        clip.SetData(clipData, 0);
        return clip;
    }
    
    AudioClip CreateMelodyClip(string name, float[] frequencies, float totalDuration)
    {
        int sampleRate = 44100;
        int samples = Mathf.RoundToInt(sampleRate * totalDuration);
        float[] clipData = new float[samples];
        
        float noteDuration = totalDuration / frequencies.Length;
        int samplesPerNote = Mathf.RoundToInt(sampleRate * noteDuration);
        
        for (int noteIndex = 0; noteIndex < frequencies.Length; noteIndex++)
        {
            float frequency = frequencies[noteIndex];
            int startSample = noteIndex * samplesPerNote;
            
            for (int i = 0; i < samplesPerNote && startSample + i < samples; i++)
            {
                float t = (float)i / sampleRate;
                float wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
                
                // Apply envelope
                float envelope = 1f;
                float noteT = (float)i / samplesPerNote;
                if (noteT < 0.1f) envelope = noteT / 0.1f;
                else if (noteT > 0.8f) envelope = (1f - noteT) / 0.2f;
                
                clipData[startSample + i] = wave * envelope * 0.4f;
            }
        }
        
        AudioClip clip = AudioClip.Create(name, samples, 1, sampleRate, false);
        clip.SetData(clipData, 0);
        return clip;
    }
    
    AudioClip CreateSweepClip(string name, float startFreq, float endFreq, float duration)
    {
        int sampleRate = 44100;
        int samples = Mathf.RoundToInt(sampleRate * duration);
        float[] clipData = new float[samples];
        
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / duration;
            float frequency = Mathf.Lerp(startFreq, endFreq, progress);
            
            float wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
            
            // Apply envelope
            float envelope = 1f - progress; // Fade out
            
            clipData[i] = wave * envelope * 0.5f;
        }
        
        AudioClip clip = AudioClip.Create(name, samples, 1, sampleRate, false);
        clip.SetData(clipData, 0);
        return clip;
    }
    
    // Public methods for playing sounds
    public void PlayJumpSound()
    {
        PlaySFX(jumpSound);
    }
    
    public void PlayWalkSound()
    {
        PlaySFXLooped(walkSound, ambientSource);
    }
    
    public void StopWalkSound()
    {
        if (ambientSource.clip == walkSound.clip)
        {
            ambientSource.Stop();
        }
    }
    
    public void PlayDeathSound()
    {
        PlaySFX(deathSound);
    }
    
    public void PlayExtraLifeSound()
    {
        PlaySFX(extraLifeSound);
    }
    
    public void PlayBarrelRollSound()
    {
        PlaySFXLooped(barrelRollSound, ambientSource);
    }
    
    public void PlayBarrelBounceSound()
    {
        PlaySFX(barrelBounceSound);
    }
    
    public void PlayBarrelDestroySound()
    {
        PlaySFX(barrelDestroySound);
    }
    
    public void PlayHammerPickupSound()
    {
        PlaySFX(hammerPickupSound);
    }
    
    public void PlayHammerSwingSound()
    {
        PlaySFX(hammerSwingSound);
    }
    
    public void PlayHammerHitSound()
    {
        PlaySFX(hammerHitSound);
    }
    
    public void PlayDKRoarSound()
    {
        PlaySFX(dkRoarSound);
    }
    
    public void PlayBarrelThrowSound()
    {
        PlaySFX(barrelThrowSound);
    }
    
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }
    
    public void PlayGameOverSound()
    {
        StopMusic();
        PlaySFX(gameOverSound);
    }
    
    public void PlayLevelCompleteSound()
    {
        PlaySFX(levelCompleteSound);
    }
    
    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic);
    }
    
    public void PlayMenuMusic()
    {
        if (menuMusic.clip != null)
        {
            PlayMusic(menuMusic);
        }
        else
        {
            PlayMusic(backgroundMusic);
        }
    }
    
    void PlaySFX(AudioClipData audioData)
    {
        if (audioData.clip != null && sfxSource != null)
        {
            sfxSource.pitch = audioData.pitch;
            sfxSource.PlayOneShot(audioData.clip, audioData.volume * sfxVolume * masterVolume);
        }
    }
    
    void PlaySFXLooped(AudioClipData audioData, AudioSource source)
    {
        if (audioData.clip != null && source != null)
        {
            source.clip = audioData.clip;
            source.volume = audioData.volume * sfxVolume * masterVolume;
            source.pitch = audioData.pitch;
            source.loop = audioData.loop;
            
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
    }
    
    void PlayMusic(AudioClipData audioData)
    {
        if (audioData.clip != null && musicSource != null)
        {
            if (musicSource.clip != audioData.clip)
            {
                musicSource.clip = audioData.clip;
                musicSource.volume = audioData.volume * musicVolume * masterVolume;
                musicSource.pitch = audioData.pitch;
                musicSource.loop = audioData.loop;
                musicSource.Play();
            }
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume * masterVolume;
        }
        if (ambientSource != null)
        {
            ambientSource.volume = sfxVolume * masterVolume * 0.5f;
        }
    }
    
    void UpdateAllVolumes()
    {
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }
    
    public void PauseAll()
    {
        if (musicSource != null) musicSource.Pause();
        if (sfxSource != null) sfxSource.Pause();
        if (ambientSource != null) ambientSource.Pause();
    }
    
    public void ResumeAll()
    {
        if (musicSource != null) musicSource.UnPause();
        if (sfxSource != null) sfxSource.UnPause();
        if (ambientSource != null) ambientSource.UnPause();
    }
}

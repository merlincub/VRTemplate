using System.Collections;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class AudioData
{
  public float globalVolume;
  public float musicVolume;
  public float ambienceVolume;
  public float soundEffectVolume;
}

public class AudioController : MonoBehaviour
{
  //[Range(0.0f, 1.0f)] public float globalVolume = 1.0f;
  //[Range(0.0f, 1.0f)] public float musicVolume = 1.0f;
  //[Range(0.0f, 1.0f)] public float ambienceVolume = 1.0f;
  //[Range(0.0f, 1.0f)] public float soundEffectVolume = 1.0f;    
  private bool globalSoundIsMuted = false;

  public AudioData audioData;

  static private AudioController theAudioController = null;
  static public AudioController Instance { get { return theAudioController; } }

  private void Awake()
  {
    if (theAudioController == null)
    {
      DontDestroyOnLoad(gameObject);
      theAudioController = this;
    }

    Load();
  }

  private void Start()
  {
    AudioManager audioManager = AudioManager.Instance;
    audioManager.volumeSoundFX = audioData.globalVolume;

    foreach (SoundGroup soundGroup in audioManager.soundGroupings)
    {
      if (soundGroup.name == "Music")
      {
        soundGroup.volumeOverride = audioData.musicVolume;
      }
      else if (soundGroup.name == "Ambience")
      {
        soundGroup.volumeOverride = audioData.ambienceVolume;
      }
      else if (soundGroup.name == "Sound effects")
      {
        soundGroup.volumeOverride = audioData.soundEffectVolume;
      }
    }
 
    SetGlobalVolume(0.7f);
    SetSoundEffectsVolume(0.4f);
    SetMusicVolume(0.5f);
    SetAmbienceVolume(0.6f);

    Save();
  }

  static public void MuteGlobalSoundToggle()
  {
    if (theAudioController == null)
    {
#if UNITY_EDITOR
      Debug.LogError("ERROR: audio controller not yet initialized or created!" + " Time: " + Time.time);
#endif
      return;
    }
    AudioManager audioManager = AudioManager.Instance;

    if (!theAudioController.globalSoundIsMuted)
    {
      audioManager.volumeSoundFX = 0.0f;
      theAudioController.globalSoundIsMuted = true;
    }
    else
    {
      audioManager.volumeSoundFX = theAudioController.audioData.globalVolume;
      theAudioController.globalSoundIsMuted = false;
    }
  }

  static public void SetGlobalVolume(float volume)
  {
    theAudioController.audioData.globalVolume = volume;
    AudioManager.Instance.volumeSoundFX = volume;
  }

  static public void SetMusicVolume(float volume)
  {
    AudioManager.Instance.soundGroupings[1].volumeOverride = volume;
    theAudioController.audioData.musicVolume = volume;
  }

  static public void SetAmbienceVolume(float volume)
  {
    AudioManager.Instance.soundGroupings[2].volumeOverride = volume;
    theAudioController.audioData.ambienceVolume = volume;
  }

  static public void SetSoundEffectsVolume(float volume)
  {
    AudioManager.Instance.soundGroupings[0].volumeOverride = volume;
    theAudioController.audioData.soundEffectVolume = volume;
  }

  public void Save()
  {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(Application.persistentDataPath + "/audioSettings.dat", FileMode.Open);
    bf.Serialize(file, audioData);
    file.Close();
  }

  public void Load()
  {
    if(!File.Exists(Application.persistentDataPath + "/audioSettings.dat"))
    {
      File.Create(Application.persistentDataPath + "/audioSettings.dat");
    }

    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(Application.persistentDataPath + "/audioSettings.dat", FileMode.Open);
    audioData = (AudioData)bf.Deserialize(file);
    file.Close();
  }
}
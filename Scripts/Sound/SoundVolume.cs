using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoundVolume : MonoBehaviour {

  public SoundFXRef sound;

  private void Awake()
  {
    GetComponent<Collider>().isTrigger = true; // Make sure the collider is a trigger.
  }

  //private void Start()
  //{
  //  sound.StopSound();
  //}

  private void OnTriggerEnter(Collider other)
  { 
    if (other.tag == "Player")
    {
      Debug.Log("SoundVolume OnTriggerEnter on " + gameObject.name);
      EnableSound(true);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.tag == "Player")
    {
      Debug.Log("SoundVolume OnTriggerExit on " + gameObject.name);
      EnableSound(false);
    }
  }

  public void EnableSound(bool enable)
  {
    try
    {
      if (enable)
      {
        sound.PlaySound();
        Debug.Log("Playing sound on " + gameObject.name);
      }
      else
      {
        sound.StopSound();
        Debug.Log("Stopping sound on " + gameObject.name);
      }
    }
    catch (Exception exception)
    { }
  }

  private void OnDisable()
  {
    try
    {
      sound.StopSound();
      Debug.Log("OnDisable stopping sound on " + gameObject.name);
    }
    catch (Exception exception)
    { }
  }
}

using UnityEngine;
using UnityEngine.Events;

public class Grabbable : OVRGrabbable
{
  public bool isImportant = false;

  [Header("Sound")]
  public SoundFXRef collisionSound;
  public SoundFXRef grabSound;

  [Range(0.0f, 1.0f)] public float volumeGain = 0.1f;
  public VibrationForce vibrationForce = VibrationForce.Medium;

  public void OnCollisionEnter(Collision collision)
  {
    Debug.Log("Grabbable OnCollisionEnter");
    
    // update the last known position
    // so we can restore it when object is lost
    
    if (collision.relativeVelocity.magnitude > 1)
    {
        float collisionVolume =
            collision.relativeVelocity.magnitude * volumeGain;
        
        collisionSound.PlaySoundAt(transform.position, 0.0f, collisionVolume);
    }
  }

  public void OnTriggerEnter( Collider col ) {
    Debug.Log("Grabbable OnTriggerEnter");
  }

  override public void GrabBegin(OVRGrabber hand, Collider grabPoint)
  {
    base.GrabBegin(hand, grabPoint);
    grabSound.PlaySoundAt(transform.position);
    //DoHaptics(vibrationForce);
  }

  //override public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
  //{
  //  base.GrabEnd(linearVelocity, angularVelocity);
  //  //Do somthing else
  //}
}
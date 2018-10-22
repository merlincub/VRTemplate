using UnityEngine;

public class PhysicsBounce : MonoBehaviour
{
  [Tooltip("Factor of the force that objects are repelled by.")]
  public float forceFactor = 1.0f;

  [Header("Sound")]
  [Tooltip("Sound played when the trap is triggered.")]
  public SoundFXRef impactSound;

  private void OnCollisionEnter(Collision collision)
  {
    var force = Vector3.Reflect(collision.relativeVelocity * forceFactor, collision.contacts[0].normal);
    collision.rigidbody.AddForce(force, ForceMode.Impulse);

    if (impactSound != null)
    {
      impactSound.PlaySoundAt(collision.transform.position, 0.0f, 1.0f);
    }
  }
}

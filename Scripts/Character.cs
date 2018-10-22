using UnityEngine;

public class Character : MonoBehaviour
{
  /// <summary>Is the character alive?</summary>
  public bool Alive { get; private set; }

  public float Mana
  {
    get { return mana; }
    set { mana = Mathf.Clamp(value, 0.0f, maxMana); }
  }

  public float ManaNormalized { get { return Mana / maxMana; } }

  [Tooltip("Current mana ratio. Updated by the script; only for debug viewing in the inspector.")]
  [Range(0.0f, 1.0f)]
  public float manaDebug;
  [Range(0.0f, 0.03f)]
  public float timeDebug;
  public float maxMana = 100;
  public SoundFXRef dieSoundFx;

  private float mana;

  private void Start()
  {
    Reset();
  }

  private void Update()
  {
    manaDebug = Mathf.Clamp01(Mana / maxMana);
    timeDebug = Time.timeScale;
  }

  public void Die()
  {
    if (Alive)
    {
      Alive = false;
      dieSoundFx.PlaySoundAt(transform.position);
    }
  }

  public void Reset()
  {
    Alive = true;
    mana = maxMana;
  }

    /// <summary>Adjusts the player's position so it's not too close to obstacles.</summary>
    public void RetractFromObstacles()
    {
        var eyePosition = GetComponentInChildren<OVRCameraRig>().centerEyeAnchor;
        var ray = new Ray(eyePosition.position, eyePosition.forward);
        var hitInfo = new RaycastHit();

        if (Physics.Raycast(ray, out hitInfo, 0.5f))
        {
            var correctedPosition = hitInfo.point - (hitInfo.point - eyePosition.position).normalized * 2.0f;
            transform.position = new Vector3(correctedPosition.x, transform.position.y, correctedPosition.z);
        }
    }
}

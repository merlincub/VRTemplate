using UnityEngine;

public class KinematicHover : MonoBehaviour
{
  [Header("Phase")]
  public float phase1;
  public float phase2;
  public float phase3;

  [Header("Oscillation distance")]
  public float distance1;
  public float distance2;
  public float distance3;

  [Header("Rotation")]
  public float rotation1;
  public float rotation2;
  public float rotation3;

  private Vector3 startPosition;

  private void Awake()
  {
    startPosition = transform.position;
  }

  // Updates the transform's Y coordinate by adding three different sines, each with their own configurable period and move distance.
  private void Update()
  {
    transform.position = startPosition + Vector3.up * (
        Mathf.Sin(Time.time * phase1) * distance1
      + Mathf.Sin(Time.time * phase2) * distance2
      + Mathf.Sin(Time.time * phase3) * distance3
      );

    transform.Rotate(
      Mathf.Sin(Time.time * phase1) * rotation1,
      Mathf.Sin(Time.time * phase2) * rotation2,
      Mathf.Sin(Time.time * phase3) * rotation3);
  }
}

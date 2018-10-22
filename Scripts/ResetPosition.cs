using UnityEngine;
using UnityEngine.Events;

public class ResetPosition : MonoBehaviour
{
  [Tooltip("Position that the object will be returned to when exceeding the maxOffset. It is initially set to the starting position of the object.")]
  public Vector3 safePosition;

  [Tooltip("Rotation that the object will be returned to when exceeding the maxOffset. It is initially set to the starting rotation of the object.")]
  public Vector3 safeRotation;

  [Tooltip("The max offset from the reset position which is allowed. Set a vector component to 'Nan' to disable checks on that axis.")]
  public Vector3 maxOffset = new Vector3(float.PositiveInfinity, 5.0f, float.PositiveInfinity);

  [Tooltip("Perform runtime checks? Disable if you just want to be able to reset the object position manually.")]
  public bool enableChecks = true;

  [Tooltip("This event is invoked when the object is returned to the safe position.")]
  public UnityEvent onReturned = new UnityEvent();

  private Rigidbody rigidBody;

  private void Awake()
  {
    rigidBody = gameObject.GetComponentInParent<Rigidbody>();
  }

  private void Start()
  {
    Set(transform);
  }

  private void Update()
  {
    if (enableChecks)
    {
      if (!float.IsInfinity(maxOffset.x) && Mathf.Abs(safePosition.x - transform.position.x) > maxOffset.x)
      {
        Return();
      }
      else if (!float.IsInfinity(maxOffset.y) && Mathf.Abs(safePosition.y - transform.position.y) > maxOffset.y)
      {
        Return();
      }
      else if (!float.IsInfinity(maxOffset.z) && Mathf.Abs(safePosition.z - transform.position.z) > maxOffset.z)
      {
        Return();
      }
    }
  }

  /// <summary>
  /// Sets the position and rotation the controlled transform will be returned to, using a transform.
  /// </summary>
  public void Set(Transform transform)
  {
    safePosition = transform.position;
    safeRotation = transform.eulerAngles;
  }

  /// <summary>
  /// Return the object to its current reset position and rotation.
  /// </summary>
  public void Return()
  {
    if (rigidBody != null)
    {
      rigidBody.velocity = rigidBody.angularVelocity = Vector3.zero;
    }

    transform.position = safePosition;
    transform.eulerAngles = safeRotation;

    onReturned.Invoke();
  }
}

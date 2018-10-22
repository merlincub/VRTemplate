using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Use this behavior to subscribe to trigger events originating from this object, in another behavior.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerVolume : MonoBehaviour
{
  public UnityEvent triggerEvent = new UnityEvent();
  public class ColliderEvent : UnityEvent<Collider> {}

  public ColliderEvent onTriggerEnter = new ColliderEvent();
  public ColliderEvent onTriggerExit = new ColliderEvent();

  private void Awake()
  {
    GetComponent<Collider>().isTrigger = true; // Make sure the collider is a trigger.
  }

  private void OnTriggerEnter(Collider other)
  {
    onTriggerEnter.Invoke(other);
    triggerEvent.Invoke();
  }

  private void OnTriggerExit(Collider other)
  {
    onTriggerEnter.Invoke(other);
  }
}

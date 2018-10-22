using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PhysicsHelper : MonoBehaviour
{
  public float delay;
  public float force;
  public Vector3 direction;
  public GameObject prefab;
  public float interval = 1;
  public int count = 10;

  private float timestamp;
  private float lastInstantiation;
  
  private void Start()
  {
    timestamp = Time.time;
  }
  
  private void Update()
  {
    if (Time.time - timestamp < delay)
    {
      return;
    }

    if (Time.time - lastInstantiation >= interval && count > 0)
    {
      var instance = Instantiate(prefab, null);
      instance.transform.position = transform.position + Vector3.up;
      var rb = instance.GetComponent<Rigidbody>();

      if (rb != null)
      {
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
      }

      lastInstantiation = Time.time;
      count--;
    }
  }

#if UNITY_EDITOR
  private void OnDrawGizmosSelected()
  {
    if (direction.sqrMagnitude != 0.0f)
    {
      Handles.color = Color.red;
      Handles.DrawLine(transform.position, transform.position + direction.normalized);
    }
  }
#endif
}

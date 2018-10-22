using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArrivalTransform : MonoBehaviour
{
  /// <summary>Shorthand for transform.position.</summary>
  public Vector3 Position { get { return transform.position; } }
  /// <summary>Shorthand for transform.forward.</summary>
  public Vector3 Direction { get { return transform.forward; } }
#if UNITY_EDITOR
  /// <summary>Position for connecting gizmo drawings.</summary>
  public Vector3 GizmoPosition { get { return transform.position + Vector3.up; } }
#endif

  //private Portal linkedPortal;

  private void Start()
  {
    //linkedPortal = transform.parent != null ? transform.parent.GetComponent<Portal>() : null;
  }

  /// <summary>Move the given transform to this destination.</summary>
  public void MoveTransform(Transform traveller)
  {
    traveller.position = transform.position;
    traveller.rotation = transform.rotation;

    //if (linkedPortal != null)
    //{
    //  linkedPortal.TriggerArrivalEffect(transform.position + Vector3.up);
    //}

    var player = traveller.GetComponent<Character>();

    if (player != null)
    {
      player.RetractFromObstacles();
    }
    else
    {
      traveller.position += Vector3.up * traveller.GetComponent<Collider>().bounds.extents.y * 2.0f; // To not get positioned in the ground.
    }
  }

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Handles.color = Color.green;
    Handles.CircleHandleCap(0, transform.position + Vector3.up * 0.01f, Quaternion.Euler(90.0f, 0.0f, 0.0f), 0.5f, EventType.Repaint);
    Handles.ArrowHandleCap(0, GizmoPosition, Quaternion.Euler(90.0f, 0.0f, 0.0f), 1.0f, EventType.Repaint);
    Handles.color = Color.blue;
    Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.5f, transform.rotation, 0.5f, EventType.Repaint);
  }
#endif
}

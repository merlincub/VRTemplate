using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
  [Tooltip("The maximum range where the UI will be shown.")]
  public float maxRange = 3.0f;
  [Tooltip("Description for this interactable that is shown in the preview UI when pointing at the object.")]
  [TextArea(3, 10)]
  public string previewInfo;
  [Tooltip("Invoked when the player presses the trigger while pointing at the object.")]
  public UnityEvent onInteractionEvent;

  /// <summary>Invoke the interaction event for this interactable.</summary>
  public void Invoke()
  {
    if (onInteractionEvent != null)
    {
      onInteractionEvent.Invoke();
    }
  }
}

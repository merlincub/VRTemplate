using UnityEngine;
using UnityEngine.UI;

public class UiPreviewInfo : MonoBehaviour
{
  [Tooltip("Reference to the text element that will show the preview info.")]
  public Text previewInfo;

  [Tooltip("Reference to the panel containing the preview text.")]
  public GameObject panel;

  /// <summary>Show the preview info UI.</summary>
  /// <param name="observer">The transform of the observer (player camera). The UI will be aligned to face this position.</param>
  /// <param name="hand">The transform of the interactor's hand. The UI will be positioned relative to this. to face this position.</param>
  /// <param name="interactable">The interactable object to show info for.</param>
  /// <param name="offset">Distance offset from the interactable's position.</param>
  public void Show(Transform observer, Transform hand, Interactable interactable)
  {
    if (observer == null || hand == null || interactable == null)
    {
      Hide();
      return;
    }

    transform.position = hand.position;
    transform.LookAt(interactable.transform.position - (observer.position - hand.position).normalized);
    previewInfo.text = interactable.previewInfo;
    panel.SetActive(true);
  }

  public void Hide()
  {
    panel.SetActive(false);
  }
}

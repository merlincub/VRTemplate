using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// InteractionHandler detects when the user points at interactable objects and displays information about them in the UI.
/// </summary>
public class InteractionHandler : MonoBehaviour
{
  public class TransformEvent : UnityEvent<Transform> { }

  [Tooltip("Reference to the transform (hand) that will be parent to the preview UI and origin of raycasts.")]
  public Transform handTransform;

  public bool debugPrint;

  /// <summary>The currently highlighted interactable if there is one.</summary>
  public Interactable Highlighted { get; private set; }

  [Tooltip("The type of controller to use.")]
  public OVRInput.Controller controller;
  [Tooltip("Max range for showing previews. In meters.")]
  public float previewRange = 10.0f;
  [Tooltip("Reference to the UI element which shows the preview text.")]
  public UiPreviewInfo previewPrefab;

  private Transform observerTransform;
  private UiPreviewInfo previewUI;
  private const float InteractionTriggerThreshold = 0.55f;
  private const float InteractionReleaseThreshold = 0.30f;

  private int interactablesMask;
  private float maxRangeSqr;
  private bool triggerPressed;

  private void Awake()
  {
    interactablesMask = LayerMask.GetMask("Pickable");
    maxRangeSqr = previewRange * previewRange;
    previewUI = Instantiate(previewPrefab);
  }

  private void Start()
  {
    observerTransform = GameHandler.instance.Player.GetComponentInChildren<OVRCameraRig>().centerEyeAnchor;
    previewUI.transform.SetParent(handTransform);
    previewUI.transform.localPosition = new Vector3(0.0f, 0.0f, 0.25f);
  }

  private void Update()
  {
    var ray = new Ray(handTransform.position, handTransform.forward);
    var hitInfo = new RaycastHit();

    if (Physics.Raycast(ray, out hitInfo, 10.0f))
    {
      if ((handTransform.position - hitInfo.transform.position).sqrMagnitude <= maxRangeSqr)
      {
        Highlighted = hitInfo.transform.GetComponent<Interactable>();
      }
      else
      {
        Highlighted = null;
      }
    }
    else
    {
      Highlighted = null;
    }

    previewUI.Show(observerTransform, handTransform, Highlighted);

    var newTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

    if (!triggerPressed && Highlighted != null && newTriggerValue >= InteractionTriggerThreshold)
    {
      triggerPressed = true;
      Highlighted.Invoke();
    }

    if (triggerPressed && newTriggerValue < InteractionReleaseThreshold)
    {
      triggerPressed = false;
    }
  }
}
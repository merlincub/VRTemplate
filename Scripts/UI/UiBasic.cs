using UnityEngine;
using UnityEngine.EventSystems;

public class UiBasic : MonoBehaviour {

  [Tooltip("Enable to make the panel center in front of the player when shown.")]
  public bool centerOnPlayer;

  [Tooltip("Reference to the ui panel")]
  public GameObject panel;

  GameObject canvas;
  protected OVRCameraRig cameraRig;

  // Mouse pointer
  private OVRMousePointer pointer;
  private Vector2 mousePos;

  // Input module
  private OVRInputModule inputModule;

  [Header("Sound")]
  [Tooltip("Sound played when ui is shown")]
  public SoundFXRef showSound;

  protected virtual void Init()
  {
    //panel = GameObject.Find("UIWinGame/Panel");
    if (!panel)
    {
      Debug.LogError("panel not found in UiWinGame");
    }

    canvas = GetComponent<Canvas>().gameObject;

    // Setup mouse pointer
    pointer = GetComponent<OVRMousePointer>();
    LockCursor();

    CentreMouse();

    GameObject playerObject = GameHandler.instance.PlayerController.gameObject;

    // We use OVRCameraRig to set rotations to cameras,
    // and to be influenced by rotation
    OVRCameraRig[] CameraRigs = playerObject.GetComponentsInChildren<OVRCameraRig>();

    if (CameraRigs.Length == 0)
      Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
    else if (CameraRigs.Length > 1)
      Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
    else
    {
      cameraRig = CameraRigs[0];
      if(CameraRigs[0].centerEyeAnchor)
      {
        GetComponent<Canvas>().worldCamera = CameraRigs[0].centerEyeAnchor.GetComponent<Camera>();
      }
      
    }

    inputModule = FindObjectOfType<OVRInputModule>();
  }

  // Update is called once per frame
  protected virtual void Update ()
  {
    OVRInput.Controller activeController = OVRInput.GetActiveController();
    Transform activeTransform = cameraRig.centerEyeAnchor;

    if ((activeController == OVRInput.Controller.LTouch) || (activeController == OVRInput.Controller.LTrackedRemote))
      activeTransform = cameraRig.leftHandAnchor;

    if ((activeController == OVRInput.Controller.RTouch) || (activeController == OVRInput.Controller.RTrackedRemote))
      activeTransform = cameraRig.rightHandAnchor;

    if (activeController == OVRInput.Controller.Touch)
      activeTransform = cameraRig.rightHandAnchor;

    OVRGazePointer.instance.rayTransform = activeTransform;
    inputModule.rayTransform = activeTransform;


    //Lock cursor to window on mouse press
    if (Input.GetMouseButtonDown(0))
    {
      LockCursor();
    }
#if !UNITY_ANDROID || UNITY_EDITOR
    UpdateMousePointer();
#endif

  }

  #region Mouse Control
  /// <summary>
  /// Position mouse pointer in centre of centre panel
  /// </summary>
  public void CentreMouse()
  {
    mousePos = new Vector2(0, 0);
    SetPointerPanel(panel);
  }

  /// <summary>
  /// Set the panel the mouse pointer is currently in
  /// </summary>
  void SetPointerPanel(GameObject panel)
  {
    pointer.pointerObject.transform.SetParent(panel.transform);
    pointer.pointerObject.transform.localRotation = Quaternion.identity;
  }
  
  /// <summary>
  /// Update mouse pointer based on mouse movement, moving between panels as necessary
  /// </summary>
  void UpdateMousePointer()
  {
    // Only allow mouse movement if gaze is focussed on this canvas
    if (inputModule.activeGraphicRaycaster != canvas.GetComponent<OVRRaycaster>())
      return;
    // Move the mouse according to move speed, and move to next panel if necessary
    mousePos += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * pointer.mouseMoveSpeed;
    float currentPanelWidth = panel.GetComponent<RectTransform>().rect.width;
    float currentPanelHeight = panel.GetComponent<RectTransform>().rect.height;
    if (mousePos.x < -currentPanelWidth / 2)
    {
      //if (currentPointerPanel.leftPanel != null)
      //{
      //  mousePos.x += currentPanelWidth / 2 + currentPointerPanel.leftPanel.panel.GetComponent<RectTransform>().rect.width / 2;
      //  SetPointerPanel(currentPointerPanel.leftPanel);
      //}
      //else
      //{
        mousePos.x = -currentPanelWidth / 2;
      //}
    }
    //else if (mousePos.x > currentPanelWidth / 2)
    //{
    //  if (currentPointerPanel.rightPanel != null)
    //  {
    //    mousePos.x -= currentPanelWidth / 2 + currentPointerPanel.rightPanel.panel.GetComponent<RectTransform>().rect.width / 2;
    //    SetPointerPanel(currentPointerPanel.rightPanel);
    //  }
    //  else
    //  {
    //    mousePos.x = currentPanelWidth / 2;
    //  }
    //}
    mousePos.y = Mathf.Clamp(mousePos.y, -currentPanelHeight / 2, currentPanelHeight / 2);

    // Position mouse pointer
    pointer.SetLocalPosition(mousePos);
  }

  void LockCursor()
  {
#if !UNITY_ANDROID || UNITY_EDITOR
    // Lock the cursor
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    Debug.Log("Lock cursor");
#endif
  }
 #endregion Mouse Control

  protected virtual void OnEnable()
  {
    Init();
  }

  public virtual void Hide()
  {
    panel.SetActive(false);
  }

  public bool IsShowing()
  {
    return panel.activeSelf;
  }

  public virtual void Show()
  {
    if(showSound != null)
    {
      showSound.PlaySoundAt(gameObject.transform.position, 0, .5f);
    }

    if (centerOnPlayer)
    {
      transform.position = GameHandler.instance.DisplayTransform.position;
      panel.transform.LookAt(panel.transform.position - (cameraRig.centerEyeAnchor.transform.position - panel.transform.position).normalized);
    }

    panel.SetActive(true);
  }
}

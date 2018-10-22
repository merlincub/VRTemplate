using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor; // For stopping play mode.
#endif

public class UiSettingsMenu : UiBasic
{
  public UiGameSettings gameSettingsPrefab;

  [Header("Input")]
  public OVRInput.Button toggleVisibilityOvrButton = OVRInput.Button.Start;
  public KeyCode toggleVisibilityKeyboardKey = KeyCode.Escape;

  [Header("UI element references")]
  public Button vrSettingsButton;
  public Button gameSettingsButton;
  public Button quitGameButton;
  public Button closeButton;

  private UiGameSettings gameSettingsPanel;

  void Start()
  {
    base.Init();

    // add listener to the buttons
    if (vrSettingsButton != null)
    {
      vrSettingsButton.onClick.AddListener(OnClickedVrSettings);
    }

    if (gameSettingsButton != null)
    {
      gameSettingsButton.onClick.AddListener(OnClickedGameSettings);
    }

    if (quitGameButton != null)
    {
      quitGameButton.onClick.AddListener(OnClickedQuitGame);
    }

    if (closeButton != null)
    {
      closeButton.onClick.AddListener(Hide);
    }
  }

  protected override void Update()
  {
    if (OVRInput.GetDown(toggleVisibilityOvrButton) || Input.GetKeyDown(toggleVisibilityKeyboardKey))
    {
      if (panel.gameObject.activeInHierarchy)
      {
        Hide();
      }
      else
      {
        Show();
      }
    }

    base.Update();
  }

  public override void Show()
  {
    GameHandler.instance.IsPaused = true;
    
    base.Show();
  }

  public override void Hide()
  {
    base.Hide();

    GameHandler.instance.ResumeIfAllowed();
  }

  private void OnClickedVrSettings()
  {
    Hide();
    OVRInspector.instance.Show();
  }

  private void OnClickedGameSettings()
  {
    Hide();

    if (gameSettingsPanel == null)
    {
      if (gameSettingsPrefab == null)
      {
        Debug.LogError("Missing game settings prefab. Can't instantiate game settings menu!");
        return;
      }

      gameSettingsPanel = Instantiate(gameSettingsPrefab);
    }

    gameSettingsPanel.Show();
  }

  private void OnClickedQuitGame()
  {
    // TODO: Ask - are you sure?
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}

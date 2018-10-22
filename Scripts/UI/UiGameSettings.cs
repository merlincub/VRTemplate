using UnityEngine;
using UnityEngine.UI;

public class UiGameSettings : UiBasic
{
  [Header("UI element references")]
  public Button closeButton;

  void Start()
  {
    base.Init();

    // add listeners to the buttons

    if (closeButton != null)
    {
      closeButton.onClick.AddListener(Hide);
    }
  }
}

using UnityEngine;
using UnityEngine.UI;

public class UiWinGame : UiBasic
{
  [Tooltip("Reference to the win button")]
  public Button winButton;

  [Tooltip("Reference to the win button")]
  public Button continueButton;

  protected override void Init()
  {
    base.Init();

    // set name of the winGame UI gameobject
    gameObject.name = "UIWinGame";

    //winButton = GameObject.Find("UIWinGame/Panel/WinButton").GetComponent<Button>();
    //continueButton = GameObject.Find("UIWinGame/Panel/ContinueButton").GetComponent<Button>();
    if (!winButton || !continueButton)
    {
      Debug.LogError("Buttons not found in UiWinGame");
    }

    // add listener to the buttons
    winButton.onClick.AddListener(() => { GameHandler.instance.ReturnToGameStartPoint(); });
    continueButton.onClick.AddListener(() => { GameHandler.instance.OnContinueGame(); });

    EventManager.StartListening("onPlayerDied", Hide);
    Hide();
  }

  public void OnDisable()
  {
    EventManager.StopListening("onPlayerDied", Hide);
  }

  protected override void OnEnable()
  { 
    Init();
  }

  public override void Show()
  {
    base.Show();
    gameObject.transform.position = GameHandler.instance.DisplayTransform.position;
  }
}

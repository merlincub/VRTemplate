using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UiLooseGame : UiBasic
{
  [Tooltip("Reference to the text element that will show the on death info.")]
  public Text gameOverInfo;

  [Tooltip("What will show the on time up info.")]
  public string onTimeUpInfo;

  [Tooltip("What will show the on death info.")]
  public string onDeathInfo;

  public Button returnToMainRoomButton;
  public Button restartLevelButton;

  protected override void Init()
  {
    base.Init();

    // set name of the winGame UI gameobject
    gameObject.name = "UILooseGame";

    GameObject playerObject = GameHandler.instance.PlayerController.gameObject;

    if (!returnToMainRoomButton)
    {
      Debug.LogError("returnToMainRoomButton not found in UiLooseGame");
    }
    returnToMainRoomButton.onClick.AddListener(() => { GameHandler.instance.ReturnToGameStartPoint(); });

    if (!restartLevelButton)
    {
      Debug.LogError("restartLevelButton not found in UiLooseGame");
    }
    restartLevelButton.onClick.AddListener(() => { GameHandler.instance.RestartLevel(); });
    Hide();
  }

  public void Show(string textToDisplay)
  {
    base.Show();
    gameObject.transform.position = GameHandler.instance.DisplayTransform.position;
    gameOverInfo.text = textToDisplay;
  }

  public void ShowOnDeath()
  {
    Show(onDeathInfo);
  }

  public void ShowOnTimeUp()
  {
    Show(onTimeUpInfo);
  }

  protected override void OnEnable()
  {
    Init();
  }

  protected override void Update()
  {
    base.Update();

    // For non-VR:
    if (Input.GetKeyDown(KeyCode.Space))
    {
      GameHandler.instance.RestartLevel();
    }
    else if (Input.GetKeyDown(KeyCode.Escape))
    {
      GameHandler.instance.ReturnToGameStartPoint();
    }
  }
}

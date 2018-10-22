using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
  private static GameHandler gameHandler;

  public static GameHandler instance
  {
    get
    {
      if (!gameHandler)
      {
        gameHandler = FindObjectOfType(typeof(GameHandler)) as GameHandler;

        if (!gameHandler)
        {
          Debug.LogError("There needs to be one active GameHandler script on a GameObject in your scene.");
        }
      }

      return gameHandler;
    }
  }

  /// <summary>Get or set the paused state. When the game is paused the time scale is set to zero and all time-dependent functionality stops.</summary>
  public bool IsPaused
  {
    get { return Time.timeScale == 0.0f; }
    set { Time.timeScale = value ? 0.0f : 1.0f; }
  }

  public bool GameIsWon { get; private set; }

  public bool GameIsRunning { get; private set; }

  /// <summary>Time elapsed since the current game started. In seconds.</summary>
  public float TimeElapsed { get; private set; }

  /// <summary>How much time remains before it runs out. In seconds.</summary>
  public float TimeRemaining { get { return Mathf.Max(totalTimeBeforeGameEnds - TimeElapsed, 0.0f); } }

  private UiLooseGame uiLooseGame;

  private UiWinGame winGameUI;

  private UIPlayerHUD uiPlayerHud;

  private UiSettingsMenu settingsMenu;

  [Tooltip("reference to the lobby room")]
  public GameObject lobbyLevel;

  [Tooltip("start Position of a new game")]
  public ArrivalTransform startPosition;

  //[Tooltip("Where the ui should be display.")]
  public Transform DisplayTransform { get; private set; }

  //[Tooltip("Reference to the player character.")]
  public Character Player { get; private set; }

  //[Tooltip("Reference to the player controller.")]
  public OVRPlayerController PlayerController { get; private set; }

  //[Tooltip("Reference to the locomotion controller.")]
  public LocomotionController LocomotionController { get; private set; }

  [Tooltip("The initial duration of a game. When it runs out the game ends. In seconds.")]
  public float totalTimeBeforeGameEnds = 60.0f;

  [Tooltip("Is this a tutorial? (This was stipulated in planning, but isn't used yet.)")]
  public bool isTutorial;

  [Header("Sound")]
  [Tooltip("Music played when the player wins the game")]
  public SoundFXRef winMusic;

  [Tooltip("Music played when the player looses the game")]
  public SoundFXRef gameOverMusic;

  [Header("Prefab references")]
  [Tooltip("Reference to the Loose Game UI element.")]
  public UiLooseGame uiLooseGamePrefab;

  [Tooltip("Reference to the Win Game UI element.")]
  public UiWinGame uiWinGamePrefab;

  [Tooltip("Reference to the settings menu prefab.")]
  public UiSettingsMenu uiSettingsPrefab;

  [Tooltip("Reference to the player HUD prefab.")]
  public UIPlayerHUD uiPlayerHudPrefab;

  private void Awake()
  {
    Init();
  }

  private void Init()
  {
    if (!startPosition)
    {
      Debug.LogError("start position needs to be set for game to start");
    }

    if (!uiLooseGamePrefab)
    {
      Debug.LogError("uiLooseGamePrefab in GameHandler is not set");
    }

    if (!uiWinGamePrefab)
    {
      Debug.LogError("uiWinGamePrefab in GameHandler is not set");
    }

    // Find and set the player character.
    var playerObject = GameObject.FindGameObjectWithTag("Player");
    Player = playerObject != null ? playerObject.GetComponent<Character>() : null;
    PlayerController = playerObject != null ? playerObject.GetComponent<OVRPlayerController>() : null;

    LocomotionController = GameObject.Find("PlayerController/LocomotionController").GetComponent<LocomotionController>();
    if (!LocomotionController)
    {
      Debug.LogError("LocomotionController cannot be found for GameHandler");
    }

    if (!lobbyLevel)
    {
      Debug.LogWarning("Main room reference cannot be found for GameHandler");
    }

    if (Player == null)
    {
      Debug.LogError("Player character not found!");
    }
    else if (PlayerController == null)
    {
      Debug.LogError("player controller not found!");
    }
    else if (LocomotionController == null)
    {
      Debug.LogError("locomotion controller not found!");
    }

    InitReferences();

    DontDestroyOnLoad(gameObject);
  }

  private void InitReferences()
  {
    if (!DisplayTransform)
    {
      GameObject uiPos = GameObject.Find("PlayerController/OVRCameraRig/TrackingSpace/CenterEyeAnchor/Ui Pos");
      if (!uiPos)
      {
        Debug.LogError("Ui Pos cannot be found on GameHandler");
      }
      DisplayTransform = uiPos.transform;
    }
  }

  private void OnEnable()
  {
    //EventManager.StartListening("onExitStairs", OnExitStairs);
    EventManager.StartListening("onExitStairsFound", OnExitStairsFound);
  }

  public void OnDisable()
  {
    //EventManager.StopListening("onExitStairs", OnExitStairs);
    EventManager.StopListening("onExitStairsFound", OnExitStairsFound);

    if (winGameUI)
    {
      Destroy(winGameUI.gameObject);
    }

    if (uiLooseGame)
    {
      Destroy(uiLooseGame.gameObject);
    }
  }

  private void Start()
  {
    RestartLevel();
  }

  private void Update()
  {
    if (GameIsRunning)
    {
      UpdateGame();
    }
  }

  public void StartGame()
  {
    Debug.Log("Game started!");
    GameIsRunning = true;
    TimeElapsed = 0.0f;

    Player.Reset();

    GameIsWon = false;

    if(lobbyLevel)
    {
      lobbyLevel.SetActive(false);
    }

    if (uiPlayerHud == null)
    {
      uiPlayerHud = Instantiate(uiPlayerHudPrefab);
    }

    if (settingsMenu == null)
    {
      settingsMenu = Instantiate(uiSettingsPrefab);
    }

    EventManager.TriggerEvent("gameStart");
  }

  public void RestartLevel()
  {
    StartGame();
  }

  private void UpdateGame()
  {
    if(!isTutorial)
    {
      TimeElapsed += Time.deltaTime;
    }

    if (!Player.Alive)
    {
      OnPlayerDied();
    }

    if (TimeElapsed >= totalTimeBeforeGameEnds)
    {
      OnTimeUp();
    }
  }

  public void SetGameWon()
  {
    GameIsRunning = false;
  }

  private void OnExitStairsFound()
  {
    if (!winGameUI)
    {
      winGameUI = Instantiate(uiWinGamePrefab);
    }

    if (!winGameUI.IsShowing())
    {
      winMusic.PlaySound();
      winGameUI.Show();

      //GameHandler.instance.SetGameWon();
      LocomotionController.gameObject.SetActive(false);
      PlayerController.EnableLinearMovement = false;
      PlayerController.EnableRotation = false;
    }
  }

  public void OnContinueGame()
  {
    // hide win game ui
    winGameUI.Hide();
    winMusic.StopSound();

    LocomotionController.gameObject.SetActive(true);
    PlayerController.EnableLinearMovement = true;
    PlayerController.EnableRotation = true;
  }

  /// <summary>
  /// Resume the game but only if allowed. It will not resume when the game is over.
  /// </summary>
  public void ResumeIfAllowed()
  {
    if (GameIsRunning)
    {
      IsPaused = false;
    }
  }

  private void OnPlayerDied()
  {
    Debug.Log("Player died!");
    IsPaused = true; // To prevent movement etc.
    GameIsRunning = false;
    GameIsWon = false;
    EventManager.TriggerEvent("onPlayerDied");

    // instantiate and show game over screen
    if (!uiLooseGame)
    {
      uiLooseGame = Instantiate(uiLooseGamePrefab);
    }

    gameOverMusic.PlaySound();
    uiLooseGame.ShowOnDeath();

    LocomotionController.gameObject.SetActive(false);
    PlayerController.EnableLinearMovement = false;
    PlayerController.EnableRotation = false;
  }

  private void OnTimeUp()
  {
    Debug.Log("Time up!");
    GameIsRunning = false;
    GameIsWon = false;
    IsPaused = true; // To prevent movement etc.
    EventManager.TriggerEvent("onTimeUp");

    // instantiate and show game over screen
    if (!uiLooseGame)
    {
      uiLooseGame = Instantiate(uiLooseGamePrefab);
    }

    if (!uiLooseGame.IsShowing())
    {
      gameOverMusic.PlaySound();
      uiLooseGame.ShowOnDeath();

      LocomotionController.gameObject.SetActive(false);
      PlayerController.EnableLinearMovement = false;
      PlayerController.EnableRotation = false;
    }
  }

  private void ReturnToPosition(ArrivalTransform destination)
  {
    if (winGameUI)
    {
      Destroy(winGameUI.gameObject);
    }

    if (uiLooseGame)
    {
      Destroy(uiLooseGame.gameObject);
    }

    gameOverMusic.StopSound();
    winMusic.StopSound();

    GameIsRunning = true;

    Player.Reset();

    if (lobbyLevel)
    {
      lobbyLevel.SetActive(true);
    }

    LocomotionController.gameObject.SetActive(true);
    PlayerController.EnableLinearMovement = true;
    PlayerController.EnableRotation = true;

    IsPaused = false;
    
     // TODO fix a normal teleport player function for player start
    //Portal.Teleport(PlayerController.transform, destination);
  }

  public void ReturnToGameStartPoint()
  {
    ReturnToPosition(startPosition);
  }
}
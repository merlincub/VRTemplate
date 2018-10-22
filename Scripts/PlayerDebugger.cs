using UnityEngine;

public class PlayerDebugger : MonoBehaviour
{
  public bool enableOffsetTranslation = true;

#if UNITY_EDITOR
  private void Awake()
  {
    if(enableOffsetTranslation)
    {
      // Set the starting height of the headset emulator.
      var manager = OVRManager.instance;
      var offset = manager.headPoseRelativeOffsetTranslation;
      var character = GameHandler.instance.Player.GetComponent<CharacterController>();
      offset.y = character != null ? character.height : 1.7f;
      manager.headPoseRelativeOffsetTranslation = offset;
    }
  }

  private void Update()
  {
    // Press 'P' while in Otherworld to toggle the return countdown.

    //if (Input.GetKeyDown(KeyCode.P))
    //{
    //  var transient = GameHandler.instance.Player.GetComponent<OtherworldTransient>();

    //  if (transient != null)
    //  {
    //    transient.TimerPaused = !transient.TimerPaused;
    //  }
    //}
  }
#endif
}

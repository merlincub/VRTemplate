using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHUD : MonoBehaviour
{
  public Slider manaSlider;
  public Vector3 offset;

  private Transform bodyTransform;
  private Character character;

  private void Start ()
  {
    character = GameHandler.instance.Player;
    bodyTransform = GameHandler.instance.PlayerController.transform;
  }

  private void Update ()
  {
    transform.position = bodyTransform.position + bodyTransform.TransformVector(offset);
    transform.eulerAngles = Vector3.Scale(bodyTransform.eulerAngles, Vector3.up);

    if (manaSlider != null)
    {
      manaSlider.value = character.ManaNormalized;
    }
  }
}

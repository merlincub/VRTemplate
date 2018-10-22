using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickSound : MonoBehaviour {

  public SoundFXRef clickSound;
  private Button Button { get { return GetComponent<Button>(); } }

  // Use this for initialization
  void Start ()
  {
    Button.onClick.AddListener(() => PlaySound());
	}
	
	void PlaySound ()
  {
    clickSound.PlaySoundAt(gameObject.transform.position);
  }
}

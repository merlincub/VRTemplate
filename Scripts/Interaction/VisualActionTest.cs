using UnityEngine;

public class VisualActionTest : MonoBehaviour
{
  private MeshRenderer meshRenderer;
  private Material material;

  private float last;

  private void Awake()
  {
    meshRenderer = GetComponent<MeshRenderer>();
    material = meshRenderer.material;
  }

  public void ChangeColor()
  {
    material.color = Random.ColorHSV(0.0f, 1.0f, 0.2f, 1.0f, 0.5f, 1.0f);
  }

  public void ApplyImpulse(Transform transform)
  {
    var rb = transform.GetComponent<Rigidbody>();
    rb.AddForce(Vector3.up * 2.0f, ForceMode.VelocityChange);
  }
}

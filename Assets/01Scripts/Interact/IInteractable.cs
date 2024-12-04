using UnityEngine;

public interface IInteractable
{
    public MeshRenderer MeshRenderer { get; }
    public Vector3 Position { get; }
    public void InteractWith(Player player);
    public void SetHighlight(bool isHighlight);
    public GameObject GameObject { get; }
}

using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string Prompt = "Interact";
    public abstract void Interact();
}

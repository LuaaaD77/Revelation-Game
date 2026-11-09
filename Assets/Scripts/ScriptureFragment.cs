using UnityEngine;

public class ScriptureFragment : Interactable
{
    public string Title;
    [TextArea] public string Verse;
    [TextArea] public string JournalText;
    public bool Collected { get; private set; }
    Renderer rend;
    float phase;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        Prompt = "Read Scripture";
    }

    void Update()
    {
        if (Collected) return;
        phase += Time.deltaTime;
        transform.Rotate(Vector3.up, 35f * Time.deltaTime, Space.World);
        transform.position += Vector3.up * Mathf.Sin(phase * 1.7f) * 0.0018f;
    }

    public override void Interact()
    {
        if (!Collected) GameManager.Instance?.RegisterFragment(this);
    }

    public void MarkCollected()
    {
        Collected = true;
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
    }
}

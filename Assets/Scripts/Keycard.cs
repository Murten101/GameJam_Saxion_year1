using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Keycard : Singleton<Keycard>
{
    internal bool IsCollected { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsCollected) return;
        IsCollected = true;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtils : Singleton<DebugUtils>
{
    private static readonly List<Message> _debugStrings = new();

#if UNITY_EDITOR
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(ClearStringAtEndOfFrame());
    }
#endif

    public static void DrawDebugText(object message)
    {
        AddString(message.ToString(), null);
    }
    public static void DrawDebugText(object message, Color? color)
    {
        AddString(message.ToString(), color);
    }

    public static void DrawDebugText(string title, object message, Color? color)
    {
        AddString($"{title} : {message}", color);
    }

    private static void AddString(string text, Color? color)
    {
        _debugStrings.Add(new Message(text, color ?? Color.white));
    }

    private struct Message
    {
        public Message(string text, Color color)
        {
            this.text = text;
            this.color = color;
        }

        public string text;
        public Color color;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        float y = 0;
        foreach (var item in _debugStrings)
        {
            GUI.color = item.color;
            GUI.Label(new Rect(5, 12.5f + y, Screen.width, Screen.height), item.text);
            y += 15f;
        }
    }

    private IEnumerator ClearStringAtEndOfFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            _debugStrings.Clear();
        }
    }

#endif
}

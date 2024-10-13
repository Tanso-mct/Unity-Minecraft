using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextButtonParts : MonoBehaviour
{
    public UnityEvent clickEvent = null;
    [SerializeField] private TextButtonEl textEl = null;

    [SerializeField] private GameObject hoverImageGroup = null;
    [SerializeField] private GameObject hoverText = null;

    public void HoverEvent()
    {
        textEl.ShowImages(false, textEl.initGroup.name);
        textEl.ShowText(false, textEl.initText.name);

        textEl.ShowImages(true, hoverImageGroup.name);
        textEl.ShowText(true, hoverText.name);
    }

    public void UnHoverEvent()
    {
        textEl.ShowImages(false, hoverImageGroup.name);
        textEl.ShowText(false, hoverText.name);

        textEl.ShowImages(true, textEl.initGroup.name);
        textEl.ShowText(true, textEl.initText.name);
    }

    public void ClickEvent()
    {
        clickEvent.Invoke();
    }
}

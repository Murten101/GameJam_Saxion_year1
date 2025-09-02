using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class SignOpacity : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0)
        {
            text.DOFade(1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0)
        {
            text.DOFade(0, 0.5f);
        }
    }
}

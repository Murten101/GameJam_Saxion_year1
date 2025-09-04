using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillOnTouch : MonoBehaviour
{
    [SerializeField]
    LayerMask playerMask;
    //[SerializeField] private List<Sprite> _deathSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & playerMask) != 0)
        {
            Debug.Log("Collision Enter");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

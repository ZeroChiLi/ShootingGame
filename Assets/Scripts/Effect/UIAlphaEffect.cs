using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlphaEffect : MonoBehaviour
{
    public CanvasGroup group;
    public float speed = 10;

    private void OnEnable()
    {
        group.alpha = 0;
    }

    private void Update()
    {
        group.alpha = Mathf.Lerp(group.alpha, 1, Time.deltaTime * speed);
    }
}

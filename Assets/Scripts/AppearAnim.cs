using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AppearAnim : MonoBehaviour
{
    Vector3 originalScale;

    void OnEnable()
    {
        originalScale = transform.localScale;
        AppaerAnim();
    }
    
    public void AppaerAnim()
    {
        transform.DOScale(originalScale * 1.2f, 0.5f).OnComplete(()=>
        {
            transform.DOScale(originalScale, 0.5f);
            gameObject.SetActive(false);
        });
    }
}

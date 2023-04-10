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
            
            StartCoroutine(GoLittleAndDisable());
        });
    }
    private IEnumerator GoLittleAndDisable()
    {
        yield return new WaitForSeconds(0.2f);
        transform.DOScale(originalScale, 0.5f);
        gameObject.SetActive(false);
    }
    public void OnDisable()
    {
        transform.DOKill();
    }
}

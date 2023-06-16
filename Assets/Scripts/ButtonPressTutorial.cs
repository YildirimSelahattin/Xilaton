using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressTutorial : MonoBehaviour
{
    Vector3 originalScale;
    Vector3 originalPos;

    void Start()
    {
        originalScale = transform.localScale;
        originalPos = transform.localPosition;
    }

    public void MoveFunc()
    {
        transform.DOLocalMoveY(-10, 1F).OnComplete(()=>{
            transform.DOScale(originalScale*0.8f,0.2f).OnComplete(()=> transform.DOScale(originalScale, 0.2f).OnComplete(() =>
            {
                transform.DOLocalMoveY(originalPos.y, 1F).OnComplete(() => MoveFunc());
            }));
        });
    }
}

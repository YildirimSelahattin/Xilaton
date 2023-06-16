using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandManager : MonoBehaviour
{
    public void TutorialMove(List<Vector3> moveList)
    {
        Vector3[] moveArray = moveList.ToArray();
        transform.DOPath(moveArray, 1f, pathType: PathType.CatmullRom).SetSpeedBased(true).OnComplete(() =>  TutorialMove(moveList)).SetEase(Ease.Linear);
    }
}

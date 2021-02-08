using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesController : MonoBehaviour
{
    public GameObject currentPage;
    public GameObject[] pages;
    public float transitionDuration = 0.7f;
    public LeanTweenType leanType = LeanTweenType.easeInOutExpo;

    [Header("Directions")]
    public Vector3 leftSide = new Vector3(-1280, 0);
    public Vector3 center = Vector3.zero;
    public Vector3 rightSide = new Vector3(1280, 0);

    bool isTweening = false;

    public void OpenPage(int pageInd)
    {
        if (!pages[pageInd].Equals(currentPage))
        {
            if (!isTweening)
            {
                isTweening = true;

                GameObject newPage = pages[pageInd];

                newPage.transform.localPosition = rightSide;
                newPage.SetActive(true);

                LeanTween.moveLocal(currentPage, leftSide, transitionDuration).setEase(leanType);
                LeanTween.moveLocal(newPage, center, transitionDuration)
                    .setEase(leanType)
                    .setOnComplete(() =>
                    {
                        isTweening = false;
                        currentPage.SetActive(false);
                        currentPage = newPage;
                    });
            }
        }
    }
}

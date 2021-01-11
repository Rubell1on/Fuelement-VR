using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesController : MonoBehaviour
{
    public GameObject currentPage;
    public GameObject[] pages;
    
    public void OpenPage(int pageInd)
    {
        currentPage.SetActive(false);
        currentPage = pages[pageInd];
        currentPage.SetActive(true);
    }
}

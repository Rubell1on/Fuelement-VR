using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchVidsController : MonoBehaviour
{
    public GameObject fuelementLogo;
    public GameObject mainMenu;
    public bool noVids = false;

    void Start()
    {
        if (!noVids)
        {
            LeanTween.addListener((int)FuelementVidController.FulementVidEventType.onFinish, e =>
            {
                fuelementLogo.SetActive(false);
                mainMenu.SetActive(true);
            });

            FuelementVidController fVC;

            if (fuelementLogo.TryGetComponent(out fVC))
            {
                fVC.Launch();
            }
        } else
        {
            fuelementLogo.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}

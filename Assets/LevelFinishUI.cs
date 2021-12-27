using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishUI : MonoBehaviour
{
    public CustomButton replay;
    public CustomButton next;
    public CustomButton mainMenu;

    public Window window;
    private LevelsController levelsController;

    private void OnEnable()
    {
        if (levelsController == null)
        {
            levelsController = LevelsController.GetInstance();
            if (levelsController == null) return;
        }

        replay.onClick.AddListener(Replay);

        if (levelsController.IsLast)
        {
            next.interactable = false;
        } 
        else
        {
            next.onClick.AddListener(LoadNextLevel);
        }

        mainMenu?.onClick.AddListener(BackToMainMenu);
    }

    private void OnDisable()
    {
        replay?.onClick.RemoveListener(Replay);
        next?.onClick.RemoveListener(LoadNextLevel);
        mainMenu?.onClick.RemoveListener(BackToMainMenu);
    }

    void BackToMainMenu()
    {
        LevelManager.GetInstance()?.LoadMainMenu();
    }

    void LoadNextLevel()
    {
        CustomLevel nextLevel = levelsController.SelectNext();
        CarData car = CarsController.GetInstance()?.CurrentCar;

        if (nextLevel == null || car == null) return;

        LevelManager.GetInstance()?.Load(nextLevel, car);
    }

    void Replay()
    {
        CustomLevel currentLevel = levelsController.CurrentLevel;
        CarData car = CarsController.GetInstance()?.CurrentCar;

        if (currentLevel == null || car == null) return;

        LevelManager.GetInstance()?.Load(currentLevel, car);
    }

    public void Show()
    {
        window?.Show();
    }
}

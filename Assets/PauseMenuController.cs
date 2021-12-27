using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PauseMenuController : MonoBehaviour
{
    public string escapeAxis = "Escape";
    public CustomButton @continue;
    public CustomButton settings;
    public CustomButton backToMainMenu;
    public Window window;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private bool executing = false;

    void Start()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
        @continue.onClick.AddListener(Hide);
        backToMainMenu.onClick.AddListener(BackToMainMenu);
    }

    private void OnDestroy()
    {
        @continue.onClick.RemoveListener(Hide);
        backToMainMenu.onClick.RemoveListener(BackToMainMenu);
    }

    void Show()
    {
        executing = true;
        canvas.enabled = true;
        window.Show(callback: () => executing = false);
    }

    void Hide()
    {
        executing = true;
        window.Hide(callback: () =>
        {
            canvas.enabled = false;
            executing = false;
        });
    }

    void BackToMainMenu()
    {
        LevelManager.GetInstance()?.LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxisRaw($"Keyboard_{escapeAxis}") != 0 || Input.GetAxisRaw(escapeAxis) != 0) && executing == false )
        {
            if (!window.gameObject.activeSelf)
            {
                Show();
            } else
            {
                Hide();
            }
        }
    }
}

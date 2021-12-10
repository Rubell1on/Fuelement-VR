using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindowsController : MonoBehaviour
{
    public bool hideAllCancels = false;
    public List<Window> windows;
    public int currentId = 0;
    public UnityEvent done;

    private Window currentWindow;

    // Start is called before the first frame update
    void Awake()
    {
        if (windows.Count > 0)
        {
            CustomButton back = windows[0].windowControls.back;
            if (back.gameObject.activeSelf)
            {
                back.gameObject.SetActive(false);
            }

            Window lastWindow = windows[windows.Count - 1];

            CustomButton next = lastWindow.windowControls.next;
            if (next.gameObject.activeSelf)
            {
                next.gameObject.SetActive(false);
            }

            lastWindow?.windowControls?.done.onClick.AddListener(() =>
            {
                done?.Invoke();
            });

            windows.ForEach(w => {
                if (w.gameObject.activeSelf)
                {
                    w.Hide(true);
                }

                WindowControls controls = w?.windowControls;
                SetMainActions(controls);

                if (hideAllCancels)
                {
                    HideCancel(controls);
                }
            });

            //if (windows.Count)
        }
    }

    void HideCancel(WindowControls controls)
    {
        CustomButton cancel = controls.cancel;
        if (cancel != null)
        {
            if (cancel.gameObject.activeSelf)
            {
                cancel.gameObject.SetActive(false);
            }
        }
    }

    void SetMainActions(WindowControls controls)
    {
        CustomButton next = controls.next;
        CustomButton back = controls?.back;

        if (next.gameObject.activeSelf)
        {
            next.onClick.AddListener(Next);
        }

        if (back.gameObject.activeSelf)
        {
            back.onClick.AddListener(Previous);
        }
    }

    [ContextMenu("Show first")]
    public void ShowFirst()
    {
        ShowAtId(0);
    }

    public void ShowAtId(int id)
    {
        if (windows.Count > 0 && id <= windows.Count - 1)
        {
            if (currentWindow != null)
            {
                currentWindow.Hide(true);
            }

            Window window = windows[id];
            if (window != null)
            {
                window.Show(currentWindow == null ? false : true);
                currentId = id;
                currentWindow = window;
            }
        }
    }

    [ContextMenu("Next")]
    public void Next()
    {
        int id = currentId + 1;
        if (id <= windows.Count - 1)
        {
            if (currentWindow != null)
            {
                currentWindow.Hide(true);
            }

            Window targetWindow = windows[id];
            if (targetWindow != null)
            {
                targetWindow.Show(currentWindow == null ? false : true);
            }

            currentId = id;
            currentWindow = targetWindow;
        }
    }

    [ContextMenu("Previous")]
    public void Previous()
    {
        int id = currentId - 1;
        if (id >= 0)
        {
            if (currentWindow != null)
            {
                currentWindow.Hide(true);
            }

            Window targetWindow = windows[id];
            if (targetWindow != null)
            {
                targetWindow.Show(currentWindow == null ? false : true);
            }

            currentId = id;
            currentWindow = targetWindow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

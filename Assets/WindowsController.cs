using System.Linq;
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
    public UnityEvent cancel;

    private Window currentWindow;

    // Start is called before the first frame update
    void Awake()
    {
        InitWindows();

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

            windows.ForEach(w => {
                if (w.gameObject.activeSelf || w.GetComponent<CanvasGroup>().alpha != 0)
                {
                    w.Hide(true);
                }

                WindowControls controls = w?.windowControls;
                SetMainActions(controls);

                if (hideAllCancels)
                {
                    HideCancel(controls);
                }
                else
                {
                    controls.cancel.onClick.AddListener(() => cancel?.Invoke());
                }

                HideDone(controls);
            });

            CustomButton doneButton = lastWindow?.windowControls.done;
            doneButton?.gameObject.SetActive(true);

            lastWindow?.windowControls?.done.onClick.AddListener(() =>
            {
                done?.Invoke();
                lastWindow.Hide();
                currentWindow = null;
            });
        }
    }

    private void OnDestroy()
    {
        List<WindowControls> controls = windows.Select(w => w.windowControls).ToList();
        RemoveListenersFromButtons(controls);
    }

    void RemoveListenersFromButtons(List<WindowControls> controls)
    {
        controls.ForEach(c =>
        {
            List<CustomButton> buttons = new List<CustomButton>() { c.back, c.next, c.done, c.cancel };
            buttons.Where(b => b != null).ToList().ForEach(RemoveListeners);

        });

        void RemoveListeners(CustomButton button)
        {
            if (button.onClick.GetPersistentEventCount() > 0)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

    void InitWindows()
    {
        int count = transform.childCount;
        if (count == 0)
        {
            return;
        }

        List<Window> windows = new List<Window>();

        for (int i = 0; i < count; i++)
        {
            Window window = transform.GetChild(i)?.GetComponent<Window>();
            windows.Add(window);
        }

        this.windows = windows;

        //windows = transform.GetComponentsInChildren<Window>().ToList();
    }

    void HideDone(WindowControls controls)
    {
        CustomButton done = controls.done;

        if (done != null) 
        {
            if (done.gameObject.activeSelf)
            {
                done.gameObject.SetActive(false);
            }
        }
    }

    void HideCancel(WindowControls controls)
    {
        CustomButton cancel = controls.cancel;
        if (cancel?.gameObject?.activeSelf == false)
        {
            return;
        }

        cancel.gameObject.SetActive(false);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Semaphore : MonoBehaviour
{
    public bool enableOnStart = false;
    public bool SettingState { get { return settingState; } }
    public SemaphoreSection.SectionType state = SemaphoreSection.SectionType.red;
    public List<SemaphoreSection> sections;
    public float sectionChangeDelay = 2;

    public UnityEvent sectionChangeFinished = new UnityEvent();

    public SemaphoreSection Section { get { return sections[(int)state]; } }

    private Coroutine stateSettting;
    private bool settingState = false;

    private void Start()
    {
        if (enableOnStart)
        {
            stateSettting = StartCoroutine(SetState(SemaphoreSection.SectionType.yellow));
        }
    }

    [ContextMenu("Set red")]
    public void SetRed()
    {
        stateSettting = StartCoroutine(SetState(SemaphoreSection.SectionType.red));
    }

    [ContextMenu("Set yellow")]
    public void SetYellow()
    {
        stateSettting = StartCoroutine(SetState(SemaphoreSection.SectionType.yellow));
    }

    [ContextMenu("Set green")]
    public void SetGreen()
    {
        stateSettting = StartCoroutine(SetState(SemaphoreSection.SectionType.green));
    }

    public void StopSetState()
    {
        StopCoroutine(stateSettting);
    }

    public IEnumerator SetState(SemaphoreSection.SectionType targetSection, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        while (state != targetSection)
        {
            settingState = true;
            switch (targetSection)
            {
                case SemaphoreSection.SectionType.red:

                    switch (state)
                    {
                        case SemaphoreSection.SectionType.red:
                            Section.EnableLight();
                            yield return null;

                            break;

                        case SemaphoreSection.SectionType.yellow:
                            SetSingleSectionState(SemaphoreSection.SectionType.red);
                            yield return null;

                            break;

                        case SemaphoreSection.SectionType.green:
                            yield return StartCoroutine(Section._Blink(3, 0.5f));
                            SetSingleSectionState(SemaphoreSection.SectionType.yellow);
                            yield return StartCoroutine(SetState(targetSection, sectionChangeDelay));

                            break;
                    }

                    break;

                case SemaphoreSection.SectionType.yellow:
                    switch (state)
                    {
                        case SemaphoreSection.SectionType.red:
                            SetSingleSectionState(SemaphoreSection.SectionType.yellow);
                            yield return null;

                            break;

                        case SemaphoreSection.SectionType.yellow:
                            Section.EnableLight();
                            yield return null;

                            break;

                        case SemaphoreSection.SectionType.green:
                            yield return StartCoroutine(Section._Blink(3, 0.5f));
                            SetSingleSectionState(SemaphoreSection.SectionType.yellow);
                            yield return StartCoroutine(SetState(targetSection, sectionChangeDelay));

                            break;
                    }

                    break;

                case SemaphoreSection.SectionType.green:
                    switch (state)
                    {
                        case SemaphoreSection.SectionType.red:
                            SetSingleSectionState(SemaphoreSection.SectionType.yellow);
                            yield return StartCoroutine(SetState(targetSection, sectionChangeDelay));

                            break;


                        case SemaphoreSection.SectionType.yellow:
                            SetSingleSectionState(SemaphoreSection.SectionType.green);
                            yield return null;

                            break;

                        case SemaphoreSection.SectionType.green:
                            Section.EnableLight();
                            yield return null;

                            break;
                    }

                    break;
            }
        }

        settingState = false;
        sectionChangeFinished?.Invoke();
    }

    void SetSingleSectionState(SemaphoreSection.SectionType targetSection)
    {
        Section.DisableLight();
        sections[(int)targetSection].EnableLight();
        state = targetSection;
    }
}

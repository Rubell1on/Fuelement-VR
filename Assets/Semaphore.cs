using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour
{
    public SemaphoreSection.SectionType currentSection = SemaphoreSection.SectionType.yellow;
    public List<SemaphoreSection> sections;
    public float sectionChangeDelay = 2;

    public SemaphoreSection Section { get { return sections[(int)currentSection]; } }

    private void Start()
    {
        StartCoroutine(ChangeSection(currentSection));
    }

    [ContextMenu("Set red")]
    public void SetRed()
    {
        StartCoroutine(ChangeSection(SemaphoreSection.SectionType.red));
    }

    [ContextMenu("Set yellow")]
    public void SetYellow()
    {
        StartCoroutine(ChangeSection(SemaphoreSection.SectionType.yellow));
    }

    [ContextMenu("Set green")]
    public void SetGreen()
    {
        StartCoroutine(ChangeSection(SemaphoreSection.SectionType.green));
    }

    public IEnumerator ChangeSection(SemaphoreSection.SectionType targetSection, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        switch (targetSection)
        {
            case SemaphoreSection.SectionType.red:

                switch (currentSection)
                {
                    case SemaphoreSection.SectionType.red:
                        Section.EnableLight();
                            
                        break;

                    case SemaphoreSection.SectionType.yellow:
                        ChangeSingeSection(SemaphoreSection.SectionType.red);
                        break;

                    case SemaphoreSection.SectionType.green:
                        Section.Blink(callback: () =>
                        {
                            ChangeSingeSection(SemaphoreSection.SectionType.yellow);
                            StartCoroutine(ChangeSection(targetSection, sectionChangeDelay));
                        });

                        yield break;
                }

                break;

            case SemaphoreSection.SectionType.yellow:
                switch (currentSection)
                {
                    case SemaphoreSection.SectionType.red:
                        ChangeSingeSection(SemaphoreSection.SectionType.yellow);
                        break;

                    case SemaphoreSection.SectionType.yellow:
                        Section.EnableLight();
                        break;

                    case SemaphoreSection.SectionType.green:
                        Section.Blink(callback: () =>
                        {
                            ChangeSingeSection(SemaphoreSection.SectionType.yellow);
                            StartCoroutine(ChangeSection(targetSection, sectionChangeDelay));
                        });

                        yield break;
                }

                break;

            case SemaphoreSection.SectionType.green:
                switch (currentSection)
                {
                    case SemaphoreSection.SectionType.red:
                        ChangeSingeSection(SemaphoreSection.SectionType.yellow);
                        StartCoroutine(ChangeSection(targetSection, sectionChangeDelay));

                        yield break;

                    case SemaphoreSection.SectionType.yellow:
                        ChangeSingeSection(SemaphoreSection.SectionType.green);
                        
                        break;

                    case SemaphoreSection.SectionType.green:
                        Section.EnableLight();

                        break;
                }

                break;
        }
    }

    void ChangeSingeSection(SemaphoreSection.SectionType targetSection)
    {
        Section.DisableLight();
        sections[(int)targetSection].EnableLight();
        currentSection = targetSection;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour
{
    public SemaphoreSection.SectionType currentSection = SemaphoreSection.SectionType.none;
    public List<SemaphoreSection> sections;

    public SemaphoreSection Section { get { return sections[(int)currentSection]; } }

    public void ChangeCurrentSection(SemaphoreSection.SectionType targetSection)
    {
        if (currentSection == targetSection)
        {
            return;
        }

        switch (targetSection)
        {
            case SemaphoreSection.SectionType.green:
                
                break;
        }

        void ChangeToYellow()
        {
            switch(currentSection)
            {
                case SemaphoreSection.SectionType.green:
                    Section.Blink(5, 0.5f, () =>
                    {
                        Section.SetDefault();
                        sections[(int)SemaphoreSection.SectionType.yellow].SetYellow();
                        currentSection = SemaphoreSection.SectionType.yellow;
                    });

                    break;

                case SemaphoreSection.SectionType.red:
                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.yellow].SetYellow();
                    currentSection = SemaphoreSection.SectionType.yellow;
                    break;
            }
        }

        IEnumerator ChangeToGreen()
        {
            switch (currentSection)
            {
                case SemaphoreSection.SectionType.yellow:
                    
                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.green].SetGreen();
                    currentSection = SemaphoreSection.SectionType.green;

                    break;

                case SemaphoreSection.SectionType.red:
                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.yellow].SetYellow();
                    currentSection = SemaphoreSection.SectionType.yellow;

                    yield return new WaitForSeconds(2);

                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.green].SetGreen();
                    currentSection = SemaphoreSection.SectionType.green;

                    break;
            }
        }

        IEnumerator ChangeToRed()
        {
            switch (currentSection)
            {
                case SemaphoreSection.SectionType.yellow:

                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.red].SetRed();
                    currentSection = SemaphoreSection.SectionType.red;

                    break;

                case SemaphoreSection.SectionType.green:
                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.yellow].SetYellow();
                    currentSection = SemaphoreSection.SectionType.yellow;

                    yield return new WaitForSeconds(2);

                    Section.SetDefault();
                    sections[(int)SemaphoreSection.SectionType.red].SetRed();
                    currentSection = SemaphoreSection.SectionType.red;

                    break;
            }
        }

        //IEnumerator OnBlinkFinished()
        //{
        //    Section.SetDefault();
        //    sections[(int)SemaphoreSection.SectionType.yellow].SetYellow();
        //    currentSection = SemaphoreSection.SectionType.yellow;
        //    yield return new WaitForSeconds(2);

        //    sections[(int)SemaphoreSection.SectionType.yellow].SetYellow();
        //    currentSection = SemaphoreSection.SectionType.yellow;
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverBehaviorStatistic : MonoBehaviour
{
    public GameObject template;
    public DriverBehaviorController behaviorController;
    public List<StatisticElement> statisticElements = new List<StatisticElement>();

    public void Start()
    {
        Fill();
    }

    public void Fill()
    {
        if (statisticElements.Count > 0)
        {
            statisticElements.ForEach(e => Destroy(e.gameObject));
        }

        foreach (KeyValuePair<string, DriverBehaviorController.DriverActivity> pair in behaviorController.driverActivities)
        {
            GameObject instance = Instantiate(template, transform);
            StatisticElement element = instance.GetComponent<StatisticElement>();

            DriverBehaviorController.DriverActivity activity = pair.Value;

            element.title.text = activity.behaviorName;
            element.errors.Value = activity.errorsCount;
            element.max.Value = activity.maxValue;
            element.min.Value = activity.minValue;
            element.avg.Value = activity.avgValue;

            statisticElements.Add(element);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectorCard : MonoBehaviour
{

    public Text carFullName;
    public RawImage producerLogo;
    public Image background;


    public void setCardData(CarData data)
    {
        if (carFullName != null) carFullName.text = data.carFullName;
       
        if (producerLogo != null) producerLogo.texture = data.producerLogo;
        if (background != null) background.color = data.background; 
    }
}
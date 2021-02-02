using UnityEngine;
using UnityEngine.UI;

public class CarCard : MonoBehaviour
{
    public Text carFullName;
    public Text releaseYear;
    public RawImage producerLogo;
    public GameObject carPlacement;
    public Image background;

    GameObject car;

    void Awake()
    {
        int count = carPlacement.transform.childCount;
        if (count > 0)
        {
            car = carPlacement.transform.GetChild(0).gameObject;

            _DisableCarScripts();
            _SetDefaultCarTransform();
        }
    }

    public void setCardData(CarData data)
    {
        if (carFullName != null) carFullName.text = data.carFullName;
        if (releaseYear != null) releaseYear.text = data.releaseYear.ToString();
        if (producerLogo != null) producerLogo.texture = data.producerLogo;
        if (carPlacement != null)
        {
            if (car != null)
            {
                Destroy(car);
            }
            car = Instantiate(data.car, carPlacement.transform);

            _DisableCarScripts();
            _SetDefaultCarTransform();
        }
        if (background != null) background.color = data.background; 
    }

    void _SetDefaultCarTransform()
    {
        Transform t = car.transform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
    }

    void _DisableCarScripts()
    {
        CarScriptsController scriptsController;
        if (car.TryGetComponent(out scriptsController))
        {
            scriptsController.EnableCardMode();
        }
    }
}
using TMPro;
using UnityEngine;

public class CityDataShower : MonoBehaviour
{
    private City city;
    [SerializeField] public TextMeshProUGUI carAmountText;
    [SerializeField] public TextMeshProUGUI lightAmountText;
    private void Start()
    {
        city = City.Instance;
    }
    void LateUpdate()
    {
        carAmountText.text = $"{city.carAmount}/{city.cityData.maxCarAmount}";
        lightAmountText.text = $"{city.averageJunctionWaitingTime}";
    }
}

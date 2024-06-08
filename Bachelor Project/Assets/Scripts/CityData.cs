using UnityEngine;

[CreateAssetMenu(fileName = "New City Data", menuName = "Create New City Data")]
public class CityData : ScriptableObject
{
    [Range(1, 200)]
    public int maxCarAmount;
}

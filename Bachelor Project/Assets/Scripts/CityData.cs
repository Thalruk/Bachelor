using UnityEngine;

[CreateAssetMenu(fileName = "New City Data", menuName = "Create New City Data")]
public class CityData : ScriptableObject
{
    [Range(1f, 50f)]
    public int maxCarAmount;
    [Range(1f, 5f)]
    public float spawnOffset;
    public bool showHelp = true;
}

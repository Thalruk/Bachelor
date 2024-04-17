using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Create New Car")]
public class CarData : ScriptableObject
{
    public string carName;
    [Range(0.7f, 1.5f)] public float reactionOffset;
    [Range(50, 150)] public float maxSpeed;
}

using UnityEngine;

[CreateAssetMenu(fileName = "CarsData", menuName = "ScriptableObject/CarsData", order = 0)]
public class CarsData : ScriptableObject
{
    public CarData[] cars;
}

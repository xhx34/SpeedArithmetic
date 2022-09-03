using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObject/CarData", order = 1)]
public class CarData : ScriptableObject
{
    public string carName;
    //各种车辆速度不宜拉开太大，保证两车间隔不会被追上
    public float speed;
    public float lifeTime;
    public int score;
    [Tooltip("分部区间（1~5）")]
    public int group;
}

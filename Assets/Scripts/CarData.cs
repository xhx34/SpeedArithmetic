using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObject/CarData", order = 1)]
public class CarData : ScriptableObject
{
    public string carName;
    //���ֳ����ٶȲ�������̫�󣬱�֤����������ᱻ׷��
    public float speed;
    public float lifeTime;
    public int score;
    [Tooltip("�ֲ����䣨1~5��")]
    public int group;
}

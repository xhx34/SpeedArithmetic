using UnityEngine;
using System.Collections.Generic;
public class CarManager : MonoBehaviour
{

    public static CarManager instance;

    public CarsData carsData;
    public Material[] materials;
    private List<string> carNames = new List<string>();
    private void Awake()
    {
        instance = this;
    }

    //根据当前游戏进程决定随机进程
    public string GetCarName(int group)
    {
        carNames.Clear();
        for (int i = 0;i < carsData.cars.Length; i++)
        {
            if(carsData.cars[i].group <= group)
            {
                carNames.Add(carsData.cars[i].carName);
            }
        }
        if (carNames.Count > 0)
        {
            int num = Random.Range(0, carNames.Count);
            return carNames[num];
        }

        return "";
    }

    public string GetCarNameInMenu()
    {
        int num = Random.Range(0, carsData.cars.Length);
        return carsData.cars[num].carName;
    }

    public Car CarCreate(string carName ,Vector3 pos,Vector3 forward)
    {
        Car car = null;
        foreach (Transform child in transform)
        {
            if (child.name == carName)
            {
                if (!child.gameObject.activeSelf)
                {
                    car = child.gameObject.GetComponent<Car>();
                    break;
                }
            }
        }

        if (car != null)
        {
            car.transform.position = pos;
            car.transform.forward = forward;
            car.transform.localScale = Vector3.one;
            car.gameObject.SetActive(true);
            int num = Random.Range(0, materials.Length);
            car.GetComponent<MeshRenderer>().material = materials[num];
            car.OnCreated();
            return car;
        }
        else
        {
            var carObj = Resources.Load("Prefab/Cars/" + carName);
            GameObject obj = Instantiate(carObj) as GameObject;
            var carNew = obj.GetComponent<Car>();
            carNew.name = carName;
            carNew.transform.position = pos;
            carNew.transform.forward = forward;
            carNew.transform.localScale = Vector3.one;
            carNew.transform.parent = transform;
            int num = Random.Range(0, materials.Length);
            carNew.GetComponent<MeshRenderer>().material = materials[num];
            carNew.OnCreated();
            return carNew;
        }
    }
}

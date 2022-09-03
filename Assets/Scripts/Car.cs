using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class Car : MonoBehaviour
{
    public CarData carData;
    public Rigidbody rd;
    public int carNumber;
    public TextMeshPro number;
    public TextMeshPro score;
    public AudioSource audioSource;

    private float timer;
    private BattleManager battleManager;

    private void Awake()
    {
        if (BattleManager.instance != null)
            battleManager = BattleManager.instance;
    }
    public void OnCreated()
    {
        score.color = new Color(1, 1, 1, 0);
        if (battleManager != null)
            rd.velocity = transform.forward * (carData.speed + battleManager.speedLevel * 0.1f);
        else
            rd.velocity = transform.forward * (carData.speed + 4);
        timer = carData.lifeTime;
        switch (carData.group)
        {
            case 1:
                if (battleManager != null)
                {
                    if(battleManager.gameMode == GameMode.Add)
                    {
                        carNumber = Random.Range(1, 10);
                    }
                    else
                    {
                        carNumber = Random.Range(1, 20);
                    }
                    number.text = carNumber.ToString();
                }
                else
                {
                    carNumber = Random.Range(1, 20);
                    number.text = carNumber.ToString();
                }
                break;
            case 2:
                if (battleManager != null)
                {
                    if (battleManager.gameMode == GameMode.Add)
                    {
                        carNumber = Random.Range(10, 20);
                    }
                    else
                    {
                        carNumber = Random.Range(20, 40);
                        if (carNumber == 23 || carNumber == 29 || carNumber == 31 || carNumber == 37)
                            carNumber++;
                    }
                    number.text = carNumber.ToString();
                }
                else
                {
                    carNumber = Random.Range(20, 40);
                    number.text = carNumber.ToString();
                }
                break;
            case 3:
                if (battleManager != null)
                {
                    if (battleManager.gameMode == GameMode.Add)
                    {
                        carNumber = Random.Range(20, 30);
                    }
                    else
                    {
                        carNumber = Random.Range(40, 60);
                        if (carNumber == 41 || carNumber == 43 || carNumber == 47 || carNumber == 53 || carNumber == 59)
                            carNumber++;
                    }
                    number.text = carNumber.ToString();
                }
                else
                {
                    carNumber = Random.Range(40, 60);
                    number.text = carNumber.ToString();
                }
                break;
            case 4:
                if (battleManager != null)
                {
                    if (battleManager.gameMode == GameMode.Add)
                    {
                        carNumber = Random.Range(30, 40);
                    }
                    else
                    {
                        carNumber = Random.Range(60, 80);
                        if (carNumber == 61 || carNumber == 67 || carNumber == 71 || carNumber == 73 || carNumber == 79)
                            carNumber++;
                    }
                    number.text = carNumber.ToString();
                }
                else
                {
                    carNumber = Random.Range(60, 80);
                    number.text = carNumber.ToString();
                }
                break;
            case 5:
                if (battleManager != null)
                {
                    if (battleManager.gameMode == GameMode.Add)
                    {
                        carNumber = Random.Range(40, 50);
                    }
                    else
                    {
                        carNumber = Random.Range(80, 100);
                        if (carNumber == 83 || carNumber == 89 || carNumber == 97)
                            carNumber++;
                    }
                    number.text = carNumber.ToString();
                }
                else
                {
                    carNumber = Random.Range(80, 100);
                    number.text = carNumber.ToString();
                }
                break;
        }
        //AudioManager.instance.PlaySound(audioSource, 7);

    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            gameObject.SetActive(false);
    }

    public void OnDie()
    {
        if (carData.score * battleManager.level > 1000)
            score.color = Color.red;
        if (carData.score * battleManager.level < 300)
            score.color = Color.green;
        if (carData.score * battleManager.level >= 300 && carData.score * battleManager.level <= 1000)
            score.color = Color.white;
        score.text = "+" + (carData.score * battleManager.level).ToString();
        score.DOFade(1f, 0.5f);
        transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1).OnComplete(() => gameObject.SetActive(false));
    }


}

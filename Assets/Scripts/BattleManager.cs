using UnityEngine;
using DG.Tweening;
using Michsky.MUIP;
using System.Collections.Generic;
//using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using I2.Loc;
public enum GameMode
{
    Add,
    Mul,
}

public enum GameState
{
    Play,
    Pause,
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public CanvasGroup menuGroup;
    public ModalWindowManager helpPanel;
    public ModalWindowManager gameOverPanel;
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public ButtonManager[] buttonNumbers;
    public ButtonManager buttonRandom;
    [HideInInspector]
    public GameState gameState;
    [HideInInspector]
    public GameMode gameMode;
    [HideInInspector]
    public int group;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textRecord;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textHelp;
    [HideInInspector]
    public int speedLevel = 0;

    //public UnityEvent start1;

    private float carTimer1 = 1;
    private float carTimer2 = 2;
    private float carTimer3 = 1;
    private float carTimer4 = 2;
    private float gameTimer = 0;
    private CarManager carManager;
    private List<Car> carList = new List<Car>();
    public int num1 = -1;
    public int num2 = -1;
    private int firstButtonId = 0;
    private int score = 0;
    [HideInInspector]
    //连击等级：（10秒）不间断输入正确答案
    public int level = 1;
    //连击计数
    private int levelCount = -1;
    private float levelTimer = -1;
    private int maxLevelCount = 0;
    private int count = 0;
    private void Awake()
    {
        instance = this;
        GamePause();
        buttonRandom.onClick.AddListener(RandomButtonNum);
        buttonNumbers[0].onClick.AddListener(() => OnClickButtonNumber(0));
        buttonNumbers[1].onClick.AddListener(() => OnClickButtonNumber(1));
        buttonNumbers[2].onClick.AddListener(() => OnClickButtonNumber(2));
        buttonNumbers[3].onClick.AddListener(() => OnClickButtonNumber(3));
        buttonNumbers[4].onClick.AddListener(() => OnClickButtonNumber(4));
        buttonNumbers[5].onClick.AddListener(() => OnClickButtonNumber(5));
        buttonNumbers[6].onClick.AddListener(() => OnClickButtonNumber(6));
        buttonNumbers[7].onClick.AddListener(() => OnClickButtonNumber(7));
        buttonNumbers[8].onClick.AddListener(() => OnClickButtonNumber(8));
        buttonNumbers[9].onClick.AddListener(() => OnClickButtonNumber(9));
        buttonNumbers[10].onClick.AddListener(() => OnClickButtonNumber(10));
        buttonNumbers[11].onClick.AddListener(() => OnClickButtonNumber(11));
        carTimer2 = Random.Range(1, 10);
        carTimer3 = Random.Range(1, 8);
        carTimer4 = Random.Range(1, 6);
        //奇怪，委托会报错？
        //for (int i = 0; i < buttonNumbers.Length; i++)
        //{
        //    Debug.LogWarning(i);
        //    buttonNumbers[i].onClick.AddListener(() => OnClickButtonNumber(i));
        //}
    }

    void Start()
    {
        carList.Clear();
        gameMode = ES3.Load("GameMode", "Game.es3", GameMode.Add);
        HelpPanelShow();
        LoManager.instance.loChange.AddListener(HelpPanelShow);
        menuGroup.alpha = 0;
        menuGroup.DOFade(1, 1f).OnComplete(() => helpPanel.OpenWindow()).SetUpdate(true);
        //start1.Invoke();
        carManager = CarManager.instance;
        group = 1;
        RandomButtonNum();
        textScore.text = score.ToString();
        AudioManager.instance.PlayMusic(1);
    }

    void Update()
    {
        carTimer1 -= Time.deltaTime;
        carTimer2 -= Time.deltaTime;
        carTimer3 -= Time.deltaTime;
        carTimer4 -= Time.deltaTime;
        gameTimer += Time.deltaTime;
        if (carTimer1 < 0)
        {
            string carName1 = carManager.GetCarName(group);
            var car = carManager.CarCreate(carName1, spawn1.position, spawn1.forward);
            carTimer1 = Random.Range(10, 15);
            carList.Add(car);
        }
        if (carTimer2 < 0)
        {
            string carName2 = carManager.GetCarName(group);
            var car = carManager.CarCreate(carName2, spawn2.position, spawn2.forward);
            carTimer2 = Random.Range(10, 15);
            carList.Add(car);
        }
        if (carTimer3 < 0)
        {
            string carName3 = carManager.GetCarName(group);
            var car = carManager.CarCreate(carName3, spawn3.position, spawn3.forward);
            carTimer3 = Random.Range(10, 15);
            carList.Add(car);
        }
        if (carTimer4 < 0)
        {
            string carName4 = carManager.GetCarName(group);
            var car = carManager.CarCreate(carName4, spawn4.position, spawn4.forward);
            carTimer4 = Random.Range(10, 15);
            carList.Add(car);
        }
        if (levelTimer > 0)
            levelTimer -= Time.deltaTime;
        else
        {
            level = 1;
            levelTimer = 8;
            if (levelCount > maxLevelCount)
                maxLevelCount = levelCount;
            levelCount = -1;
        }
    }

    public void HelpPanelShow()
    {
        if (gameMode == GameMode.Add)
            textHelp.text = LocalizationManager.GetTranslation("HelpPanel1");
        else
            textHelp.text = LocalizationManager.GetTranslation("HelpPanel2");
    }
    public void GamePlay()
    {
        gameState = GameState.Play;
        Time.timeScale = 1;
    }

    public void GamePause()
    {
        gameState = GameState.Pause;
        Time.timeScale = 0;
    }

    public void GameStateChange()
    {
        if (gameState == GameState.Pause)
            GamePlay();
        else
            GamePause();
    }

    public void GameOver()
    {
        AudioManager.instance.PlaySound(6);
        DOVirtual.DelayedCall(2, () => { gameOverPanel.OpenWindow(); GamePause(); });
        if (gameMode == GameMode.Add)
        {
            int highScore = ES3.Load("score", "Record.es3", 0);
            if (score > highScore)
                ES3.Save("score", score, "Record.es3");
            int highCount = ES3.Load("count", "Record.es3", 0);
            if (count > highCount)
                ES3.Save("count", count, "Record.es3");
            int highLevelCount = ES3.Load("levelCount", "Record.es3", 0);
            if (maxLevelCount > highLevelCount)
                ES3.Save("levelCount", maxLevelCount, "Record.es3");
            int highTime = ES3.Load("time", "Record.es3", 0);
            if ((int)gameTimer > highTime)
                ES3.Save("time", (int)gameTimer, "Record.es3");
            int minu = (int)gameTimer / 60;
            int sec = (int)gameTimer % 60;
        }
        else
        {
            int highScore = ES3.Load("score", "RecordMul.es3", 0);
            if (score > highScore)
                ES3.Save("score", score, "RecordMul.es3");
            int highCount = ES3.Load("count", "RecordMul.es3", 0);
            if (count > highCount)
                ES3.Save("count", count, "RecordMul.es3");
            int highLevelCount = ES3.Load("levelCount", "RecordMul.es3", 0);
            if (maxLevelCount > highLevelCount)
                ES3.Save("levelCount", maxLevelCount, "RecordMul.es3");
            int highTime = ES3.Load("time", "RecordMul.es3", 0);
            if ((int)gameTimer > highTime)
                ES3.Save("time", (int)gameTimer, "RecordMul.es3");
            int minu = (int)gameTimer / 60;
            int sec = (int)gameTimer % 60;
        }
    }

    public void OnModifyLocalization()
    {
        // if no MainTranslation then skip (most likely this localize component only changes the font)
        if (string.IsNullOrEmpty(Localize.MainTranslation))
            return;

        int minu = (int)gameTimer / 60;
        int sec = (int)gameTimer % 60;
        Localize.MainTranslation = Localize.MainTranslation.Replace("{score}", score.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{minu}", minu.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{sec}", sec.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{count}", count.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{levelCount}", maxLevelCount.ToString());
        if (gameMode == GameMode.Add)
        {
            int highScore = ES3.Load("score", "Record.es3", 0);
            Localize.MainTranslation = Localize.MainTranslation.Replace("{highScore}", highScore.ToString());
        }
        else
        {
            int highScore = ES3.Load("score", "RecordMul.es3", 0);
            Localize.MainTranslation = Localize.MainTranslation.Replace("{highScore}", highScore.ToString());
        }
    }

    public void RandomButtonNum()
    {
        //如果第一按钮已经按下，无法随机
        if (num1 > 0)
        {
            //音效
            AudioManager.instance.PlaySound(4);
            return;
        }

        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        int count4 = 0;
        int count5 = 0;
        switch (group)
        {
            case 1:
                for (int i = 0; i < buttonNumbers.Length; i++)
                {
                    if (gameMode == GameMode.Add)
                    {
                        buttonNumbers[i].SetText(Random.Range(0, 10).ToString());
                    }
                    else
                    {
                        //12个按钮在随机时大概有两个是10以内的数
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 4)
                            {
                                buttonNumbers[i].SetText(Random.Range(1, 10).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 20).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 20).ToString());
                        }
                        else
                        {
                            buttonNumbers[i].SetText(Random.Range(0, 20).ToString());
                        }
                    }
                }
                break;
            case 2:
                for (int i = 0; i < buttonNumbers.Length; i++)
                {
                    if (gameMode == GameMode.Add)
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 4)
                            {
                                buttonNumbers[i].SetText(Random.Range(1, 10).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 20).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 20).ToString());
                        }
                        else
                            buttonNumbers[i].SetText(Random.Range(0, 20).ToString());
                    }
                    else
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(1, 5).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(5, 10).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                        else if (num == 3)
                        {
                            if (count4 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(20, 30).ToString());
                                count4++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                        else
                        {
                            buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                    }
                }
                break;
            case 3:
                for (int i = 0; i < buttonNumbers.Length; i++)
                {
                    if (gameMode == GameMode.Add)
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 4)
                            {
                                buttonNumbers[i].SetText(Random.Range(0, 10).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 30).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 30).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(20, 30).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 30).ToString());
                        }
                        else
                            buttonNumbers[i].SetText(Random.Range(0, 30).ToString());
                    }
                    else
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(1, 5).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 60).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(5, 10).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 60).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 60).ToString());
                        }
                        else if (num == 3)
                        {
                            if (count4 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(20, 30).ToString());
                                count4++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 60).ToString());
                        }
                        else if (num == 4)
                        {
                            if (count5 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(30, 40).ToString());
                                count5++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 60).ToString());
                        }
                        else
                        {
                            buttonNumbers[i].SetText(Random.Range(0, 60).ToString());
                        }
                    }
                }
                break;
            case 4:
                for (int i = 0; i < buttonNumbers.Length; i++)
                {
                    if (gameMode == GameMode.Add)
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 4)
                            {
                                buttonNumbers[i].SetText(Random.Range(0, 10).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(20, 30).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                        else if (num == 3)
                        {
                            if (count4 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(30, 40).ToString());
                                count4++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                        }
                        else
                            buttonNumbers[i].SetText(Random.Range(0, 40).ToString());
                    }
                    else
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(1, 5).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 80).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(5, 10).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 80).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 80).ToString());
                        }
                        else if (num == 3)
                        {
                            if (count4 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(20, 40).ToString());
                                count4++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 80).ToString());
                        }
                        else if (num == 4)
                        {
                            if (count5 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(40, 60).ToString());
                                count5++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 80).ToString());
                        }
                        else
                        {
                            buttonNumbers[i].SetText(Random.Range(0, 80).ToString());
                        }
                    }
                }
                break;
            case 5:
                for (int i = 0; i < buttonNumbers.Length; i++)
                {
                    if (gameMode == GameMode.Add)
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 4)
                            {
                                buttonNumbers[i].SetText(Random.Range(0, 10).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 50).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 20).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 50).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(20, 30).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 50).ToString());
                        }
                        else if (num == 3)
                        {
                            if (count4 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(30, 40).ToString());
                                count4++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 50).ToString());
                        }
                        else if (num == 4)
                        {
                            if (count5 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(40, 50).ToString());
                                count5++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 50).ToString());
                        }
                        else
                            buttonNumbers[i].SetText(Random.Range(0, 50).ToString());
                    }
                    else
                    {
                        int num = Random.Range(0, 6);
                        if (num == 0)
                        {
                            if (count1 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(1, 5).ToString());
                                count1++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 100).ToString());

                        }
                        else if (num == 1)
                        {
                            if (count2 < 3)
                            {
                                buttonNumbers[i].SetText(Random.Range(5, 10).ToString());
                                count2++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 100).ToString());
                        }
                        else if (num == 2)
                        {
                            if (count3 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(10, 30).ToString());
                                count3++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 100).ToString());
                        }
                        else if (num == 3)
                        {
                            if (count4 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(30, 50).ToString());
                                count4++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 100).ToString());
                        }
                        else if (num == 4)
                        {
                            if (count5 < 2)
                            {
                                buttonNumbers[i].SetText(Random.Range(50, 70).ToString());
                                count5++;
                            }
                            else
                                buttonNumbers[i].SetText(Random.Range(0, 100).ToString());
                        }
                        else
                        {
                            buttonNumbers[i].SetText(Random.Range(0, 100).ToString());
                        }
                    }
                }
                break;
        }
    }

    public void OnClickButtonNumber(int id)
    {
        //当第一个按钮按下
        if (num1 < 0)
        {
            num1 = int.Parse(buttonNumbers[id].buttonText);
            firstButtonId = id;
            buttonNumbers[id].transform.Find("Highlighted/Background").GetComponent<Image>().color = new Color(1, 1, 0, 1);
            buttonNumbers[id].transform.Find("Normal/Background").GetComponent<Image>().color = new Color(1, 1, 0, 1);
            AudioManager.instance.PlaySound(3);
        }
        //当第二个按钮按下
        else
        {
            num2 = int.Parse(buttonNumbers[id].buttonText);
            buttonNumbers[firstButtonId].transform.Find("Normal/Background").GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            buttonNumbers[firstButtonId].transform.Find("Highlighted/Background").GetComponent<Image>().color = new Color(1, 1, 1, 1);
            if (id != firstButtonId) 
                CheckCarNumber();
            else
            {
                AudioManager.instance.PlaySound(3);
                num1 = -1;
                num2 = -1;
            }
        }
    }

    private void CheckCarNumber()
    {
        int anwser1 = num1 + num2;
        int anwser2;
        if (num1 > num2)
            anwser2 = num1 - num2;
        else
            anwser2 = num2 - num1;
        int anwser3 = num1 * num2;
        int anwser4;
        if (num1 > num2)
        {
            if (num2 != 0)
                anwser4 = num1 / num2;
            else
                anwser4 = 0;
        }
        else
        {
            if (num1 != 0)
                anwser4 = num2 / num1;
            else
                anwser4 = 0;
        }

        int anwserCount = 0;
        if (gameMode == GameMode.Add)
        {
            for (int i = 0; i < carList.Count; i++)
            {
                if (carList[i].carNumber == anwser1 || carList[i].carNumber == anwser2)
                {
                    score += carList[i].carData.score * level;
                    //todo得分效果
                    textScore.text = score.ToString();
                    carList[i].OnDie();
                    carList.Remove(carList[i]);
                    count++;
                    if (count % 30 == 0 && group < 5)
                        group++;
                    if (group == 5 && count % 10 == 0)
                        speedLevel++;
                    levelTimer = 8;
                    levelCount++;
                    if (levelCount > 0 && levelCount % 10 == 0 && level < 3)
                        level++;
                    LevelShow();
                    AudioManager.instance.PlaySound(level - 1);
                    anwserCount++;
                }
            }
        }
        else
        {
            for (int i = 0; i < carList.Count; i++)
            {
                if (carList[i].carNumber == anwser1 || carList[i].carNumber == anwser2 || carList[i].carNumber == anwser3 || carList[i].carNumber == anwser4)
                {
                    score += carList[i].carData.score * level;
                    textScore.text = score.ToString();
                    carList[i].OnDie();
                    carList.Remove(carList[i]);
                    count++;
                    if (count % 30 == 0 && group < 5)
                        group++;
                    if (group == 5 && count % 10 == 0)
                        speedLevel++;
                    levelTimer = 8;
                    levelCount++;
                    if (levelCount > 0 && levelCount % 10 == 0 && level < 3)
                        level++;
                    LevelShow();
                    AudioManager.instance.PlaySound(level - 1);
                    anwserCount++;
                }
            }
        }

        if (anwserCount == 0)
        {
            AudioManager.instance.PlaySound(4);
            level = 1;
            levelTimer = 8;
            if (levelCount > maxLevelCount)
                maxLevelCount = levelCount;
            levelCount = -1;
        }
        num1 = -1;
        num2 = -1;
    }

    public void BackMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void LevelShow()
    {
        textLevel.text = "x" + level;
        textLevel.fontSize = 50 + level * 30;
        textLevel.DOFade(1, 2).OnComplete(() => { textLevel.DOFade(0, 2).SetDelay(2); });
    }
}

using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using I2.Loc;

//using UnityEngine.Events;
public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public CanvasGroup menuGroup;
    public Transform spawn1;
    public Transform spawn2;
    public TextMeshProUGUI textRecord;
    //public UnityEvent start1;
    private float carTimer1 = 1;
    private float carTimer2 = 2;
    private CarManager carManager;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }
    void Start()
    {
        menuGroup.alpha = 0;
        menuGroup.DOFade(1, 2f);
        //start1.Invoke();
        carManager = CarManager.instance;
        AudioManager.instance.PlayMusic(0);
        //LoManager.instance.loChange.AddListener(RecordShow);
    }

    void Update()
    {
        carTimer1 -= Time.deltaTime;
        carTimer2 -= Time.deltaTime;
        if (carTimer1 < 0)
        {
            string carName1 = carManager.GetCarNameInMenu();
            carManager.CarCreate(carName1, spawn1.position, spawn1.forward);
            carTimer1 = Random.Range(10, 15);
        }
        if (carTimer2 < 0)
        {
            string carName2 = carManager.GetCarNameInMenu();
            carManager.CarCreate(carName2, spawn2.position, spawn2.forward);
            carTimer2 = Random.Range(10, 15);
        }
    }

    public void EnterBattle(int modeId)
    {
        ES3.Save("GameMode", (GameMode)modeId, "Game.es3");
        SceneManager.LoadScene("Battle");
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    //public void RecordShow()
    //{
    //    textRecord.text = LocalizationManager.GetTermTranslation("Recode");
    //        //"<color=yellow>加减运算模式：</color>\n最高得分：" + highScore + "\r\n最长用时：" + minu + "分" + sec + "秒" + "\r\n最多拦截数量：" + highCount + "\r\n最高连击：" + highLevelCount+
    //        //"\n\n<color=red>四则运算模式：</color>\n最高得分：" + highScoreMul + "\r\n最长用时：" + minuMul + "分" + secMul + "秒" + "\r\n最多拦截数量：" + highCountMul + "\r\n最高连击：" + highLevelCountMul;
    //}

    public void OnModifyLocalization()
    {
        // if no MainTranslation then skip (most likely this localize component only changes the font)
        if (string.IsNullOrEmpty(Localize.MainTranslation))
            return;

        int highScore = ES3.Load("score", "Record.es3", 0);
        int highCount = ES3.Load("count", "Record.es3", 0);
        int highLevelCount = ES3.Load("levelCount", "Record.es3", 0);
        int highTime = ES3.Load("time", "Record.es3", 0);
        int minu = highTime / 60;
        int sec = highTime % 60;
        int highScoreMul = ES3.Load("score", "RecordMul.es3", 0);
        int highCountMul = ES3.Load("count", "RecordMul.es3", 0);
        int highLevelCountMul = ES3.Load("levelCount", "RecordMul.es3", 0);
        int highTimeMul = ES3.Load("time", "RecordMul.es3", 0);
        int minuMul = highTimeMul / 60;
        int secMul = highTimeMul % 60;

        Localize.MainTranslation = Localize.MainTranslation.Replace("{highScore}", highScore.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{highCount}", highCount.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{highLevelCount}", highLevelCount.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{minu}", minu.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{sec}", sec.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{highScoreMul}", highScoreMul.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{highCountMul}", highCountMul.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{highLevelCountMul}", highLevelCountMul.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{minuMul}", minuMul.ToString());
        Localize.MainTranslation = Localize.MainTranslation.Replace("{secMul}", secMul.ToString());
    }
}

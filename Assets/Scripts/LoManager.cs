
using I2.Loc;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class LoManager : MonoBehaviour, ILocalizationParamsManager
{
    public static LoManager instance;
    public UnityEvent loChange;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (!LocalizationManager.ParamManagers.Contains(this))

        {

            LocalizationManager.ParamManagers.Add(this);

            LocalizationManager.LocalizeAll(true);

        }
    }

    private void OnDisable()
    {
        LocalizationManager.ParamManagers.Remove(this);
    }
    public void LoChange()
    {
        loChange.Invoke();
    }

    public virtual string GetParameterValue(string ParamName)
    {
        if (ParamName == "highScore")
            return "Javier";            // For your game, get this value from your Game Manager

        if (ParamName == "NUM PLAYERS")
            return 5.ToString();

        return null;
    }
}

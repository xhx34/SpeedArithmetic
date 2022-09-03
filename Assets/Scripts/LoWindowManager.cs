using UnityEngine;
using I2.Loc;
using Michsky.MUIP;
public class LoWindowManager : MonoBehaviour
{
    public ModalWindowManager windowManager;
    public string key;
    void Start()
    {
        ButtonTextLo();
        LoManager.instance.loChange.AddListener(ButtonTextLo);
    }

    void ButtonTextLo()
    {
        windowManager.titleText = LocalizationManager.GetTranslation(key);
        windowManager.UpdateUI();
    }
}

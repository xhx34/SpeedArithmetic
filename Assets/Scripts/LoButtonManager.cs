using UnityEngine;
using I2.Loc;
using Michsky.MUIP;
public class LoButtonManager : MonoBehaviour
{
    public ButtonManager buttonManager;
    public string key;
    void Start()
    {
        ButtonTextLo();
        LoManager.instance.loChange.AddListener(ButtonTextLo);
    }

    void ButtonTextLo()
    {
        buttonManager.SetText(LocalizationManager.GetTranslation(key));
    }
}

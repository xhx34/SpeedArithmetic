using UnityEngine;
public class GameOverTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            BattleManager.instance.GameOver();
            gameObject.SetActive(false);
        }
    }
}

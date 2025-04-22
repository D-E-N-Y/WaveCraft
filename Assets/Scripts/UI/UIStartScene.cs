using UnityEngine;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private UIBlackBaground ui_blackBaground;

    void Start()
    {
        ui_blackBaground.Show();
    }
}

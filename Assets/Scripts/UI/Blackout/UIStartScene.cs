using System.Collections;
using UnityEngine;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private UIBlackout ui_blackout;

    void Start()
    {
        ui_blackout.gameObject.SetActive(true);
        StartCoroutine(nameof(Show));
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(1f);
        
        ui_blackout.Show();
    }
}

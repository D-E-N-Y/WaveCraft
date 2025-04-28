using System.Collections;
using UnityEngine;

public class UIStartScene : MonoBehaviour
{
    [SerializeField] private UIBlackBackground ui_blackBaground;

    void Start()
    {
        ui_blackBaground.gameObject.SetActive(true);
        StartCoroutine(nameof(Show));
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(1f);
        
        ui_blackBaground.Show();
    }
}

using TMPro;
using UnityEngine;

public class UIInteractableWallPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    
    [SerializeField] private GameObject buttonPanel;
    
    private D_Wall wall;

    public void Initialize(D_Wall resource)
    {
        this.wall = resource;
        
        nameText.text = resource.nameActor;
        hpText.text = resource.GetCurrentHP().ToString();

        resource.UpdateCurrentHP += RefreshCurrentHP;
        resource.DestroyActor += ClosePanel;

        buttonPanel.SetActive(wall.Type() == E_WallType.Column);
    }

    void OnDisable()
    {
        wall.UpdateCurrentHP -= RefreshCurrentHP;
        wall.DestroyActor -= ClosePanel;
    }

    public void ContinueWall()
    {
        if(wall.isBuild)
        {
            BuildSystem.current.ContinueWall(wall);
        }
    }

    private void RefreshCurrentHP()
    {
        hpText.text = wall.GetCurrentHP().ToString();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

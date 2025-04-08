using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MainPanel, selectMapPanel;

    private Vector3 originalScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainPanel.SetActive(true);
        selectMapPanel.SetActive(false);  
    }

    public void StartGameButton()
    {
        selectMapPanel.SetActive(true);
        MainPanel.SetActive(false);
    }

    public void BackToMain()
    {
        selectMapPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    public void MapSelectButton(int mapSelected)
    {
        GameManager.instance.mapSelected = mapSelected;

        SceneManager.LoadScene(1);
    }

    public void OnButtonSelect(Transform buttonTrans)
    {
        if (originalScale == Vector3.zero)
        {
            originalScale = buttonTrans.localScale;
        }

        buttonTrans.localScale = originalScale * 1.1f;
        buttonTrans.GetChild(0).gameObject.SetActive(true);
    }

    public void OnButtonDeselect(Transform buttonTrans)
    {
        buttonTrans.localScale = originalScale;
        buttonTrans.GetChild(0).gameObject.SetActive(false);
    }
}

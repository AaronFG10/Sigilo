using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MainPanel, selectMapPanel;

    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();

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
        if (!originalScales.ContainsKey(buttonTrans))
        {
            originalScales[buttonTrans] = buttonTrans.localScale;
        }

        buttonTrans.localScale = originalScales[buttonTrans] * 1.1f;
        buttonTrans.GetChild(0).gameObject.SetActive(true);
    }

    public void OnButtonDeselect(Transform buttonTrans)
    {
        if (originalScales.ContainsKey(buttonTrans))
        {
            buttonTrans.localScale = originalScales[buttonTrans];
        }

        if (buttonTrans.childCount > 0)
        {
            buttonTrans.GetChild(0).gameObject.SetActive(false);
        }
    }
}

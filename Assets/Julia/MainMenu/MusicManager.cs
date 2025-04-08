using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Button musicButton;
    public Sprite musicOnIcon;
    public Sprite musicOffIcon;

    public bool isMuted = false;

    private void Start()
    {
        UpdateButtonIcon();

        musicButton.onClick.AddListener(ToggleMusic);
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;

        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (isMuted)
        {
            musicButton.image.sprite = musicOffIcon;
        }

        else
        {
            musicButton.image.sprite = musicOnIcon;
        }
    }
}

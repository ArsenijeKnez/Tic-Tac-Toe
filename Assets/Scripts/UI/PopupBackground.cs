using UnityEngine;

public class PopupBackground : MonoBehaviour
{
    public void OnBackgroundClicked()
    {
        UIManager.Instance.CloseAllPopups();
        this.gameObject.SetActive(false);
    }
}
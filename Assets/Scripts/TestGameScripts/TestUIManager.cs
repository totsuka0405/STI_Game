using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notificationText;

    public void ShowNotification(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;
        StartCoroutine(HideNotificationAfterDelay());
    }

    private IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        notificationPanel.SetActive(false);
    }
    private void HideNotification()
    {
        notificationText.gameObject.SetActive(false);
    }
    /*
    public void UpdateInventoryUI(List<Item> inventory)
    {
        foreach (Transform child in inventoryUI)
        {
            Destroy(child.gameObject); // 古いUIを削除
        }

        foreach (Item item in inventory)
        {
            GameObject newItem = new GameObject(item.name); // 仮にプレハブとする
            newItem.transform.SetParent(inventoryUI);
        }
    }
    */
}

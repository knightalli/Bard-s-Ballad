using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    private bool isOpen = false;
    public bool IsInventoryOpen => isOpen;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen) CloseInventory();
            else OpenInventory();
        }
        else if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInventory();
        }
    }

    private void OpenInventory()
    {
        isOpen = true;
        inventoryUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CloseInventory()
    {
        isOpen = false;
        inventoryUI.SetActive(false);
        Time.timeScale = 1f;
    }
}

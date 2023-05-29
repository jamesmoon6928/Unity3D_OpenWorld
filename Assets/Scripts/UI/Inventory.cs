using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //When inventory is visible, to stop all other actions
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        IfInventoryOpen();
    }

    private void IfInventoryOpen()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            {
                OpenInventory();
            } else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    public void ObtainItem(Item obt_item, int count = 1)
    {
        if (Item.ItemType.Equipment != obt_item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null) {
                    if (slots[i].item.itemName == obt_item.itemName)
                    {
                        slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(obt_item, count);
                return;
            }
        }
    }
}

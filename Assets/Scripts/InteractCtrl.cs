using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractCtrl : MonoBehaviour
{
    [SerializeField]
    private float range;

    private bool pickupActivated = false;

    private RaycastHit hitInfo;

    //Layermask to make interaction happen only when item layer was hit(ray)
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory inventory;

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        DoInteraction();
    }

    private void DoInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickup>().item.itemName + " obtained");
                inventory.ObtainItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        } else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "Obtain " + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "<color=yellow>" + "(E)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}

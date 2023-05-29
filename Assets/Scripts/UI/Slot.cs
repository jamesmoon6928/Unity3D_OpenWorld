using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;       //Obtained item
    public int itemCount;   //Number of an item
    public Image itemImage; //Image of the item

    [SerializeField]
    private Text text_Count;    //text to show the number of item
    [SerializeField]
    private GameObject go_CountImage;

    //Sets the image's transaparency
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    //Adds an item
    public void AddItem(Item add, int count = 1)
    {
        item = add;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        } else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        

        SetColor(1);
    }

    //Control the number of items
    public void SetSlotCount(int count)
    {
        itemCount += count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    //Clear/Empty slots
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }
}

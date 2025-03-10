﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private SlotClass[] items;
    [SerializeField] private SlotClass[] startingItems;

    [SerializeField] private SlotClass movingSlot;
    [SerializeField] private SlotClass originalSlot;
    [SerializeField] private SlotClass tempSlot;

    public Image itemCursor;
    public Button useButton;
    [SerializeField]private GameObject[] slots;
    public bool isMoving;
    [SerializeField] private SlotClass selectedSlot; // Lưu slot đã chọn

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }

        for(int i=0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        originalSlot = new SlotClass();
        movingSlot = new SlotClass();
        tempSlot = new SlotClass();

        RefreshUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving)
            {
                EndMove();
            }else
            {
                BeginMove();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (isMoving)
            {
                //EndMove();
            }
            else
            {
                BeginSplit();
            }
        }

        if (isMoving)
        {
            itemCursor.enabled = true;
            itemCursor.transform.position = Input.mousePosition;
            itemCursor.sprite = movingSlot.GetItem().itemIcon;
        }else
        {
            itemCursor.enabled = false;
            itemCursor.sprite = null;
        }
    }

    private void RefreshUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (!items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";
                }
            
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    /*public void AddItem(ItemClass item, int quantity)
    {
        SlotClass slot = ContainsItem(item);
        if(slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
        }

        RefreshUI();
    }*/
    //Add item new ChatGPT
    public void AddItem(ItemClass item, int quantity)
    {
        // Kiểm tra xem item có stackable (có thể chồng lên) không
        SlotClass slot = ContainsItem(item);
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity); // Nếu stackable, tăng số lượng trong slot
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                // Nếu slot trống, thêm item vào đó
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
        }

        RefreshUI(); // Cập nhật UI sau khi thay đổi inventory
    }

    public void RemoveItem(ItemClass item,int quanity)
    {
        SlotClass temp = ContainsItem(item);
        if (temp != null)
        {
            if(temp.GetQuantity() > 1)
            {
                temp.SubQuantity(quanity);
            }else
            {
                int slotToRemoveIndex = 0;
                for(int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                items[slotToRemoveIndex].RemoveItem();
            }
        }else
        {
            return;
        }

       

        RefreshUI();
    }

    private SlotClass ContainsItem(ItemClass item)
    {
        for(int i=0; i < items.Length; i++)
        {
            if(items[i].GetItem() == item)
            {
                return items[i];
            }
        }
        return null;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 64)
            {
                return items[i];
            }
        }

        return null;
    }

    private void BeginMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null) return;

        movingSlot.AddItem(originalSlot.GetItem(), originalSlot.GetQuantity());
        originalSlot.RemoveItem();

        selectedSlot = originalSlot; // Lưu lại slot đã chọn

        isMoving = true;
        RefreshUI();
    }


    private void BeginSplit()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null) return;
        if(originalSlot.GetQuantity() <= 1)
        {
            return;
        }

        movingSlot.AddItem(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));

        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));

        isMoving = true;
        RefreshUI();
        return;
    }

    private void EndMove()
    {
        originalSlot = GetClosestSlot();

        if(originalSlot == null)
        {
            AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                //If slot is the same item
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    //If slot item is stackable
                    if (originalSlot.GetItem().isStackable)
                    {
                        int itemMaxStack = originalSlot.GetItem().maxStackQuantity; //Apple: 20
                        int count = originalSlot.GetQuantity() + movingSlot.GetQuantity();// 25

                        if(count > itemMaxStack)
                        {
                            int remain = count - itemMaxStack; //5
                            originalSlot.SetQuantity(itemMaxStack);
                            movingSlot.SetQuantity(remain);

                            isMoving = true;
                            RefreshUI();
                            return;
                        }else
                        {
                            originalSlot.AddQuantity(movingSlot.GetQuantity());
                            movingSlot.RemoveItem();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    //Swap
                    tempSlot.AddItem(originalSlot.GetItem(), originalSlot.GetQuantity());
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    tempSlot.RemoveItem();

                    RefreshUI();
                    return;
                }
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.RemoveItem();
            }
        }

       

        isMoving = false;
        RefreshUI();
        return;
    }
    public void UseItem()
    {
        if (selectedSlot == null || selectedSlot.GetItem() == null)
        {
            Debug.Log("Không có vật phẩm nào được chọn!");
            return;
        }

        Debug.Log("Vật phẩm được chọn: " + selectedSlot.GetItem().itemName);

        ConsumableClass consumable = selectedSlot.GetItem().GetConsum();
        if (consumable != null) // Nếu là consumable item
        {
            Debug.Log("Vật phẩm là Consumable: " + consumable.itemName + " | Hồi máu: " + consumable.healthRecovery);

            FindObjectOfType<HealthBarSystem>().Heal(consumable.healthRecovery); // Hồi máu

            selectedSlot.SubQuantity(1); // Giảm số lượng item sau khi sử dụng
            if (selectedSlot.GetQuantity() <= 0) // Nếu hết thì xoá item khỏi slot
            {
                Debug.Log("Số lượng vật phẩm đã hết, xóa khỏi túi đồ.");
                selectedSlot.RemoveItem();
            }

            RefreshUI(); // Cập nhật lại UI
        }
        else
        {
            Debug.Log("Vật phẩm này không phải là Consumable.");
        }
    }



}
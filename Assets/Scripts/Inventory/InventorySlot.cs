using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;
public class InventorySlot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler,IPointerClickHandler
{
    [Header("Slot Detail")]
    public Inventory inventory;

    [Header("Slot Detail")]
    public SO_Item item;
    public int stack;

    [Header("UI")]
    public Color emptyColor;
    public Color itemColor;
    public Image icon;
    public TextMeshProUGUI stackText;

    [Header("Drag and Drop")]
    public int siblindIndex;
    public RectTransform draggable;
    Canvas canvas;
    CanvasGroup canvasGroup;
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        siblindIndex = transform.GetSiblingIndex();
    }
    #region Drag and Drop Methods
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
        inventory.SetLayoutControlChild(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggable.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        draggable.anchoredPosition = Vector2.zero;
        transform.SetSiblingIndex(siblindIndex);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventorySlot draggedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
            
            if (draggedSlot != null && draggedSlot != this)
            {
                if (this.item != null && this.item == draggedSlot.item)
                {
                    int amountToAdd = Mathf.Min(draggedSlot.stack, this.item.maxStack - this.stack);
                    
                    if (amountToAdd > 0)
                    {
                        this.stack += amountToAdd;

                        draggedSlot.stack -= amountToAdd;
                        
                        this.CheckShowText();
                        draggedSlot.CheckShowText();
                        
                        if (draggedSlot.stack <= 0)
                        {
                            draggedSlot.ClearSlot();
                        }
                    }
                }
                else
                {
                    SwapSlot(draggedSlot);
                }
            }
        }
    }

    public void ClearSlot()
    {
        // Очистка слота и обновление UI
        item = inventory.EMPTY_ITEM;
        stack = 0;
    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
         // Убрираем проверку если не нужно на правый кнопке мыши
        if(eventData.button == PointerEventData.InputButton.Right)
        {
           
        } if(item == inventory.EMPTY_ITEM)
                return;

            // inventory Open mini canvas
            inventory.OpenMiniCanvas(eventData.position);
            inventory.SetRightClickSlot(this);
        
    }
 #endregion

    public void UseItem()
    {
        stack = Mathf.Clamp(stack - 1, 0, item.maxStack);
        if(stack > 0)
        {
            CheckShowText();
        }
        else
        {
            inventory.RemoveItem(this);
        }
    }
    public void SwapSlot(InventorySlot newslot)
    {
        SO_Item keepItem;
        int keepStack;

        keepItem = item;
        keepStack = stack;

        SetSwap(newslot.item,newslot.stack);
        newslot.SetSwap(keepItem,keepStack);

    }
    public void SetSwap(SO_Item swapItem, int amount)
    {
        item = swapItem;
        stack = amount;
        icon.sprite = swapItem.icon;

        CheckShowText();
    }
    public void MergeThisSlot(SO_Item mergeItem, int mergeAmount)
    {
        item = mergeItem;
        icon.sprite = mergeItem.icon;

        int ItemAmount = stack + mergeAmount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack);
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if(amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(mergeItem,this);
            if(slot == null)
            {
                inventory.DropItem(mergeItem,amountLeft);
                return;
            }
            else
            {
                slot.MergeThisSlot(mergeItem,amountLeft);
            }
        }
       
    }
    public void SetThisSlot(SO_Item newItem,int amount)
    {
        item = newItem;
        icon.sprite = item.icon;
        
        int ItemAmount = amount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, item.maxStack);
        stack = intInthisSlot;

        CheckShowText();

        int amountLeft = ItemAmount - intInthisSlot;
        if(amountLeft > 0)
        {
            InventorySlot slot = inventory.IsEmptySlotLeft(newItem,this);
            if(slot == null)
            {
                //Drop item
                return;
            }
            else
            {
                slot.SetThisSlot(newItem,amountLeft);
            }
        }

    }
   public void CheckShowText()
{
    UpdateColorSlot();
    stackText.text = stack.ToString();
    
    // Если максимальный стек больше или равен 2, то показываем текст
    if(stack >= 2)
    {
        stackText.gameObject.SetActive(true);
    }
    else
    {
        // Иначе прячем текст
        stackText.gameObject.SetActive(false);
    }
}

    public void UpdateColorSlot()
    {
        if(item == inventory.EMPTY_ITEM)
        {
            icon.color = emptyColor;
        }
        else
        {
             icon.color = itemColor;
        }
    }
}

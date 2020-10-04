using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _selectedImage;
    
    public void OnSelect(BaseEventData eventData)
    {
        _selectedImage.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _selectedImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectedImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selectedImage.enabled = false;
    }

}

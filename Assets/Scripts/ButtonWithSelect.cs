using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonWithSelect : Button {

    public event Action<ButtonWithSelect> OnSelected = delegate { };

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        OnSelected(this);
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        Select();
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Mono.Data.Sqlite;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Action<PointerEventData> drag_begin = delegate {};
	public void OnBeginDrag(PointerEventData eventData) { drag_begin(eventData); }

	public Action<PointerEventData> drag = delegate {};
	public void OnDrag(PointerEventData eventData) { drag(eventData); }

	public Action<PointerEventData> drag_end = delegate {};
	public void OnEndDrag(PointerEventData eventData) { drag_end(eventData); }

	public Action<PointerEventData> click = delegate {};
	public void OnPointerClick(PointerEventData eventData) { click(eventData); }

	public Action<PointerEventData> click_down = delegate {};
	public void OnPointerDown(PointerEventData eventData) { click_down(eventData); }

	public Action<PointerEventData> mouse_enter = delegate {};
	public void OnPointerEnter(PointerEventData eventData) { mouse_enter(eventData); }

	public Action<PointerEventData> mouse_exit = delegate {};
	public void OnPointerExit(PointerEventData eventData) { mouse_exit(eventData); }

	public Action<PointerEventData> click_up = delegate {};
	public void OnPointerUp(PointerEventData eventData) { click_up(eventData); }
}

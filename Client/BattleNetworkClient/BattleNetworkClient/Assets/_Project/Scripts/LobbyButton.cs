using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LobbyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

    public Text Name;
    public Image Separator;

    public Color NormalColor;
    public Color HoverColor;
    public Color SelectedColor;

    private bool _isSelected = false;
    private Lobby _lobby;

    public Lobby Lobby
    {
        get{ return _lobby; }
    }

    public void Configure(Lobby lobby)
    {
        Name.text = lobby.Name;
        _lobby = lobby;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Name.color = HoverColor;
        Separator.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected)
        {
            Name.color = SelectedColor;
            Separator.color = SelectedColor;
        }
        else
        {
            Name.color = NormalColor;
            Separator.color = NormalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isSelected)
        {
            _isSelected = false;
            Name.color = NormalColor;
            Separator.color = NormalColor;
            GameObject.FindObjectOfType<MenuController>().DeselectLobby(this);
        }
        else
        {
            _isSelected = true;
            Name.color = SelectedColor;
            Separator.color = SelectedColor;
            GameObject.FindObjectOfType<MenuController>().SelectLobby(this);
        }
    }

    public void Deselect()
    {
        _isSelected = false;
        Name.color = NormalColor;
        Separator.color = NormalColor;
    }
}

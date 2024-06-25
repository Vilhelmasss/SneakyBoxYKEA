using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject playerController;
    [Header("Sidebars")]
    public GameObject addButtonBarSelection;
    public GameObject freeSelectButtonBarSelection;

    [Header("Color Sliders")]
    public GameObject colorSliders;
    public Slider colorRed, colorGreen, colorBlue;
    [Header("Grid")]
    public GameObject gridButtonIcon;
    public Sprite gridEnabledIcon;
    public Sprite gridDisabledIcon;
    [SerializeField] private bool isGridEnabled = false;

    public void ButtonFreeSelect()
    {
        playerController.GetComponent<PlayerController>().SwtichToFreeSelectState();
    }

    public void ButtonSelectAdd()
    {
        addButtonBarSelection.SetActive(true);
    }

    public void ObjectSelected()
    {
        freeSelectButtonBarSelection.SetActive(true);
    }

    public void ButtonGrid()
    {
        isGridEnabled = !isGridEnabled;

        playerController.GetComponent<PlayerController>().isGridEnabled = this.isGridEnabled;

        GridChangeIcon();
    }

    public void ButtonMove()
    {
        playerController.GetComponent<PlayerController>().MoveObjectButton();
        SidebarsEnabled(false);
    }

    public void ButtonDelete()
    {
        playerController.GetComponent<PlayerController>().ObjectDelete();
        SidebarsEnabled(false);
    }

    public void ButtonColor()
    {
        ColorSlidersEnabled();
    }

    public void OnColorChange()
    {
        Color color = new Color(colorRed.value, colorGreen.value, colorBlue.value);

        playerController.GetComponent<PlayerController>().ChangeColor(color);

    }

    private void GridChangeIcon()
    {
        if (isGridEnabled)
            gridButtonIcon.GetComponent<Image>().sprite = gridEnabledIcon;
        else
            gridButtonIcon.GetComponent<Image>().sprite = gridDisabledIcon;
    }

    public void ButtonSelectCube()
    {
        SelectShape("Cube");
    }

    public void ButtonSelectSphere()
    {
        SelectShape("Sphere");
    }

    public void ButtonSelectCylinder()
    {
        SelectShape("Cylinder");
    }

    private void SelectShape(string shape)
    {
        playerController.GetComponent<PlayerController>().currentKey = shape;
        playerController.GetComponent<PlayerController>().SwitchToPlaceObjectState();
        SidebarsEnabled(false);
    }

    private void SidebarsEnabled(bool enabled = true)
    {
        freeSelectButtonBarSelection.SetActive(enabled);
        addButtonBarSelection.SetActive(enabled);
        ColorSlidersEnabled(enabled);
    }

    private void ColorSlidersEnabled(bool enabled = true)
    {
        colorSliders.SetActive(enabled);
    }
}

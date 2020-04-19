using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EquipmentUI : MonoBehaviour
{

    public Canvas canvas;

    public GameObject radialButton;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
    }

    public bool IsUIEnabled()
    {
        return canvas.enabled;
    }

    public void ShowCreateUIFromAvailableEquipmentAndPosition(Vector2 position, List<GameObject> equipment)
    {
        // Cleanup any previous menu
        Cleanup();

        List<GameObject> buttons = new List<GameObject>();
        for (int i = 0; i < equipment.Count; i++) {
            GameObject button = Instantiate(radialButton, canvas.transform);
            //button.transform.SetParent(canvas.transform);
            button.GetComponent<Button>().onClick.AddListener(OnCreateButtonClick(equipment.ElementAt(i)));
            buttons.Add(button);
        }
        SetRadialButtonPositions(buttons.ToArray(), position);

        canvas.enabled = true;
    }

    public void ShowUpdateUIFromEquipmentAndPosition(Vector2 position, GameObject equipmentObject)
    {
        // Cleanup any previous menu
        Cleanup();

        // Check if we have actual equipment
        Equipment equipment = equipmentObject.GetComponent<Equipment>();
        if (!equipment) {
            Debug.Log("NO EQUIPMENT");
            return;
        }

        List<GameObject> buttons = new List<GameObject>();

        // TODO: Upgrade button
        if (equipment.upgrade) {

        }

        // TODO Connect button
        if (equipment.CanAddConnection()) {
            GameObject button = Instantiate(radialButton, canvas.transform);
            buttons.Add(button);
        }

        SetRadialButtonPositions(buttons.ToArray(), position);

        canvas.enabled = true;
    }

    protected void SetRadialButtonPositions(GameObject[] buttons, Vector2 position)
    {
        float angle = 360.0f / buttons.Length;
        float radius = 50.0f; // Radius is in pixels

        for (int i = 0; i < buttons.Length; i++) {
            GameObject button = buttons[i];
            button.transform.position = position + (new Vector2(Mathf.Sin((angle * i) * Mathf.Deg2Rad), Mathf.Cos((angle * i) * Mathf.Deg2Rad)) * radius);
        }
    }

    /// <summary>
    /// Handles radial button click, will add the assigned equipment to the
    /// equipment manager.
    /// </summary>
    protected UnityAction OnCreateButtonClick(GameObject equipment)
    {
        return () => {
            gameObject.GetComponent<EquipmentManager>().AddEquipment(equipment); 
            canvas.enabled = false;
        };
    }

    protected void Cleanup()
    {
        foreach (Transform child in canvas.transform) {
            Destroy(child.gameObject);
        }
    }
}

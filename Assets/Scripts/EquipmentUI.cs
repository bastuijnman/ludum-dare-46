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

    public void CreateUIFromAvailableEquipmentAndPosition(Vector2 position, List<GameObject> equipment)
    {

        // Cleanup any previous menu
        foreach (Transform child in canvas.transform) {
            Destroy(child.gameObject);
        }

        float angle = 360.0f / equipment.Count;
        float radius = 50.0f; // Radius is in pixels

        for (int i = 0; i < equipment.Count; i++) {
            GameObject button = Instantiate(radialButton);
            button.transform.SetParent(canvas.transform);
            button.transform.position = position + (new Vector2(Mathf.Sin((angle * i) * Mathf.Deg2Rad), Mathf.Cos((angle * i) * Mathf.Deg2Rad)) * radius);
            button.GetComponent<Button>().onClick.AddListener(OnButtonClick(equipment.ElementAt(i)));
        }

        canvas.enabled = true;
    }

    /// <summary>
    /// Handles radial button click, will add the assigned equipment to the
    /// equipment manager.
    /// </summary>
    private UnityAction OnButtonClick(GameObject equipment)
    {
        return () => { 
            gameObject.GetComponent<EquipmentManager>().AddEquipment(equipment); 
            canvas.enabled = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    
    public TMP_Text Textbox;

    // Start is called before the first frame update
    void Start()
    {
        var dropdown = transform.GetComponent<TMP_Dropdown>();

        dropdown.options.Clear();

        List<string> items = new List<string>();

        items.Add("Item1");
        items.Add("Item2");
        
        foreach (var item in items)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() {text = item});
        }    

        DropDownItemSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown);});

    }

    public void DropDownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;

        Textbox.text = dropdown.options[index].text;
    }

}

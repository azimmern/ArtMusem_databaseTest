using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using TMPro;

public class DBDropdownHandler : MonoBehaviour
{
    //list of items in the Dropdown
    List<string> items = new List<string>();

    //list of string arrays in the inventory
    //e.g. string[1] = {PaintingName, FirstName, LastName, Year, MediumName}
    List<string[]> inventory = new List<string[]>();
    List<byte[]> imageInventory = new List<byte[]>();
    public TMP_Text Textbox1_PaintingName;
    public TMP_Text Textbox2_PainterName_FirstLast;
    public TMP_Text Textbox3_Year;
    public TMP_Text Textbox4_Medium;
    public Image Icon;

    public GameObject Cube;
    
    
    void Start()
    {
        var dropdown = transform.GetComponent<TMP_Dropdown>();

        dropdown.options.Clear();

        addItemToItemsListFromDatabase();
        
        foreach (var item in items)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() {text = item});
        }    

        // I need to research these two methods more closely, I don't understand what's going on with the delegate
        DropDownItemSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown);});

        

    }

    public void DropDownItemSelected(TMP_Dropdown dropdown)
    {
        //set the dropdown UI value as an index
        int index = dropdown.value;
        
        //replace the text in TextBox1 with the text in the dropdown at that index position, the text should be the painting name.
        Textbox1_PaintingName.text = dropdown.options[index].text;
        
        // For these next 3 commands, see how the List of string[] arrays called "inventory" is composed in the function below.

        //replace the text in TextBox2 with a concatenation of Painter.FirstName + Painter.LastName
        Textbox2_PainterName_FirstLast.text = inventory[index][1] + " " + inventory[index][2];
        // replace the text in TextBox3 with Paintings.YearPainted
        Textbox3_Year.text = inventory[index][3];
        // replace the text in TextBox4 with Media.MediumName
        Textbox4_Medium.text = inventory[index][4];
        // replace the image component in Icon with the SpriteImage
        
        
        byte[] placeholder_img = imageInventory[index];
        Texture2D tex = new Texture2D(200,200);
        tex.LoadImage(imageInventory[index]);
        Icon.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(10f, 10f));

        Cube.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void addItemToItemsListFromDatabase()
    {
        string dbName = "URI=file:Inventory.db";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Paintings.ID, Paintings.PaintingName, Paintings.YearPainted, Paintings.SpriteImage, Paintings.PainterID, Painters.FirstName, Painters.LastName, Media.MediumName FROM ((Paintings INNER JOIN Painters ON Paintings.PainterID = Painters.ID) INNER JOIN Media ON Paintings.MediumID = Media.ID);" ;

                using (IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        // Print out to the Console the name of the painting and the Medium, just to check
                        Debug.Log("Painting: " + reader["PaintingName"] + " | Medium: " + reader["MediumName"] + "." );
                        
                        //Hopefully that's working!
                        //Now, first, add the Painting Name to the list of items in the UI Dropdown menu
                        items.Add(reader["PaintingName"].ToString());
                        
                        //Now store specifically string data/metadata about the Painting as a string array called "inventory row"
                        //An inventory row includes [Paintings.PaintingName, Painter.FirstName, Painter.LastName, Paintings.YearPainted, and Media.MediumName]
                        string[] inventory_row = new string[5] {
                            reader["PaintingName"].ToString(), reader["FirstName"].ToString(), reader["LastName"].ToString(), reader["YearPainted"].ToString(), reader["MediumName"].ToString()
                            };
                        //Now add that inventory row to the inventory List, to make it a List of string arrays.
                        inventory.Add(inventory_row);
                        // END OF STRING DATA/METADATA IMPORT

                        // The image runs a little differently.
                        // START OF IMAGE/BLOB DATA IMPORT
                        // Start by declaring a new Texture2D variable, let's call it texture, and assign a width and height.
                        //Texture2D texture = new Texture2D ((int)Icon.GetComponent<RectTransform>().rect.width, (int)Icon.GetComponent<RectTransform>().rect.height);
                        
                        //texture.LoadImage(reader["SpriteImage"]);
                        // Now store the SpriteImage from the database in that variable
                        byte[] img = (byte[])reader["SpriteImage"];
                        imageInventory.Add(img);
                        
                        
                        //The inventory and imageInventory get called in the DropDownItemSelected() function above
                        
                    }

                    reader.Close();
                }    

            }

            connection.Close();
        }
    }

}

    

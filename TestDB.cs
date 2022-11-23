using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class TestDB : MonoBehaviour
{
    //The name of the db is "Inventory"

    private string dbName = "URI=file:Inventory.db";
    public InputField userInput;

    // Start is called before the first frame update
    void Start()
    {

        //run the method to create a table
        CreateDB();

        DisplayPaintings();

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDB()
    {
        //create the db connection
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            //set up an object called "command" to enable database control
            using (var command = connection.CreateCommand())
            {
                //using SQL commands (the all-caps stuff) create a table called "paintings" if it doesn't already exist
                //it has 2 fields: name (of type VariableCharacter, up to 20 characters) and date (an integer YYYY, up to 4 characters)
                command.CommandText = "CREATE TABLE IF NOT EXISTS paintings (name VARCHAR(20), date INT(4));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void AddPaintingName(string paintingName)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            // set up an object called "command" to allow db control
            using (var command = connection.CreateCommand())
            {
                //Write an SQL Command that inserts a record with the values passed into the method
                //statement syntax: "INSERT INTO tablename (field1, field2) VALUES ('value1', 'value2');"
                //Because SQL demands single-quotes for values, we need to get creative with doublequotes
                command.CommandText = "INSERT INTO paintings (name) VALUES ('" + paintingName + "');";
                command.ExecuteNonQuery(); // this runs the SQL command
            }
            connection.Close();
        }
    }

    public void DisplayPaintings()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            // set up an object called "command" to allow db control
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM paintings;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    //show to console what is in field "name" and in field "date" for each row
                    //for reader ["xxxxx"] - replae the xxxxx with the field name you want to show
                    // this will display as many time as there are records returned

                    Debug.Log("Painting Name: " + reader["name"]);

                    reader.Close();
                }
            }
            connection.Close();
        }
    }
}

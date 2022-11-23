using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class TestDBBrowser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisplayPaintingMedium();
    }

    // Update is called once per frame
    void DisplayPaintingMedium()
    {
        string dbName = "URI=file:Inventory.db";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = " SELECT * FROM Paintings INNER JOIN Media ON Paintings.MediumID = Media.ID;" ;

                /*

                You can also do this across multiple lines

                "SELECT * " +
                "FROM Paintings " +
                "INNER JOIN Media " +
                "ON ..."

                
                */

                using (IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Debug.Log("Painting: " + reader["PaintingName"] + " | Medium: " + reader["MediumName"]);
                    
                    
                    }

                    reader.Close();
                }    

            }

            connection.Close();
        }
    }
}

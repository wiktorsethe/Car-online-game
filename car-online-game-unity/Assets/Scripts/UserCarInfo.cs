using UnityEngine;
using Mirror;

public class UserCarInfo : NetworkBehaviour
{
    // SyncVar will automatically sync the value across the network.
    // The hook is a method that gets called whenever the value changes.
    // 'spriteID' holds the ID of the selected car sprite.
    [SyncVar(hook = nameof(HandleDisplaySpriteChange))] 
    [SerializeField] private string spriteID;

    // Reference to the SpriteRenderer that will display the car's sprite.
    [SerializeField] private SpriteRenderer carSprite;
    
    // Reference to the car database which stores available cars sprites.
    [SerializeField] private CarsDatabase carsDatabase;
    
    // Command is called on the server to update the spriteID on the network.
    // This method changes the sprite ID on the server, and SyncVar will update all clients.
    [Command]
    public void SetDisplaySprite(string newSpriteID)
    {
        // Update the spriteID on the server, triggering the hook to update on clients.
        spriteID = newSpriteID;
    }

    // This method is called when the spriteID changes, either locally or on the network.
    // It takes the old and new spriteID as parameters.
    private void HandleDisplaySpriteChange(string oldSpriteID, string newSpriteID)
    {
        // Variable to hold the new sprite if it's found in the database.
        Sprite newSprite = null;

        // Loop through the cars database to find the matching car sprite using the spriteID.
        for (int i = 0; i < carsDatabase.cars.Length; i++)
        {
            // Check if the current car in the database has the same ID as the new spriteID.
            if (carsDatabase.cars[i].carID == newSpriteID)
            {
                // If found, set the newSprite to the car's image.
                newSprite = carsDatabase.cars[i].carImage;
            }
        }

        // If a valid sprite is found, update the carSprite's sprite to the new one.
        if (newSprite != null)
        {
            carSprite.sprite = newSprite;
        }
    }
}

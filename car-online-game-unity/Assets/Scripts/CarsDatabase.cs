using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Car
{
    public string carID;
    public Sprite carImage;
}

[CreateAssetMenu(fileName = "CarsDatabase", menuName = "GameData/Cars Database")]
public class CarsDatabase : ScriptableObject
{
    public Car[] cars;
}
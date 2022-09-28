using UnityEngine;

public abstract class MapObject
{
    public bool NeedPlace { get; set; }
    public bool HasCollectable { get; set; } = false;
    public abstract AssignedMapObjects Type { get; }
    public MapPosition MapPosition { get; }
    protected MapObject(MapPosition mapPosition)
    {
        MapPosition = mapPosition;
    }
    public static MapObject GetAssignedMapObject(char letter, int x, int y)
    {
        switch (letter)
        {
            case ' ': // EmptyField
                return new EmptyField(new MapPosition { X = x, Y = y});
            case 'w': // Wall
                return new Wall(new MapPosition { X = x, Y = y });
            case 'c': // Collectable
                return new Collectable(new MapPosition { X = x, Y = y });
            case 'p': // Player
                return new StartField(new MapPosition { X = x, Y = y });
            case 'k': // CloneField
                return new CloneField(new MapPosition { X = x, Y = y });
            default:
                Debug.Log("ERROR: A letter in 'LevelMap'.txt is not valid. Replaced by emptyField");
                return new StartField(new MapPosition { X = x, Y = y });
        }
    }
    public enum AssignedMapObjects
    {
        EmptyField,
        Wall,
        Collectable,
        Player,
        CloneField
    }
}

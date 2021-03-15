using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileDirection
{
    N, E, S, W
}

public static class TileDirectionExtensions
{
    public static TileDirection Opposite(this TileDirection direction)
    {
        //IF less than 3, direction +3, ELSE, direction -3
        if ((int)direction < 2) { return direction + 2; } // ?
        else { return direction - 2; }  // :
    }

    public static TileDirection Previous(this TileDirection direction)
    { // if NOT start of array, -1, if start of array, go to end of array
        return direction == TileDirection.N ? TileDirection.W : (direction - 1);
    }

    public static TileDirection Next(this TileDirection direction)
    {
        return direction == TileDirection.W ? TileDirection.N : (direction + 1);
    }
}

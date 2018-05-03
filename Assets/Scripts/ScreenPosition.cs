using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPosition {
    private Vector3 temp = new Vector3(0, 0, 0);

    public Vector3 GetScreenPosition(Vector3 position)
    {
        temp.x = position.x / 1000 * Screen.width;
        temp.y = position.y / 1000 * Screen.height;
        temp.z = position.z;
        return temp;
    }
}
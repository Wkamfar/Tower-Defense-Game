using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WFunctions
{
    //Already in List
    public static  bool AlreadyInList(List<GameObject> list, GameObject item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<RaycastHit> list, GameObject item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i].collider.gameObject == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<GameObject> list, RaycastHit item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item.collider.gameObject)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<RaycastHit> list, RaycastHit item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i].point == item.point)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<string> list, string item)
    { 
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<int> list, float item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<int> list, int item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<Vector2> list, Vector2 item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(List<Vector3> list, Vector3 item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(GameObject[] list, GameObject item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(RaycastHit[] list, GameObject item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i].collider.gameObject == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(GameObject[] list, RaycastHit item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item.collider.gameObject)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(RaycastHit[] list, RaycastHit item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i].point == item.point)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(string[] list, string item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(int[] list, float item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(int[] list, int item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(Vector2[] list, Vector2 item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    public static bool AlreadyInList(Vector3[] list, Vector3 item)
    {
        bool alreadyInList = false;
        for (int i = 0; i < list.Length; ++i)
            if (list[i] == item)
                alreadyInList = true;
        return alreadyInList;
    }
    //Horizontal Distance
    public static float HorizontalDistance(Vector3 point1, Vector3 point2)
    {
        float x = point2.x - point1.x;
        float z = point2.z - point1.z;
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));
    }
    public static float HorizontalDistance(float xPos1, float zPos1, float xPos2, float zPos2)
    {
        float x = xPos2 - xPos1;
        float z = zPos2 - xPos1;
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));
    }
}

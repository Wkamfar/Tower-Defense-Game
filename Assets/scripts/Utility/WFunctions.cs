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
    //Find in List
    public static int FindInList(List<GameObject> list, GameObject item)
    {
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                return i;
        Debug.LogError("NullReferenceException: Item could not be found in the list");
        return -1;
    }
    public static int FindInList(List<int> list, int item)
    {
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                return i;
        Debug.LogError("NullReferenceException: Item could not be found in the list");
        return -1;
    }
    public static int FindInList(List<float> list, float item)
    {
        for (int i = 0; i < list.Count; ++i)
            if (list[i] == item)
                return i;
        Debug.LogError("NullReferenceException: Item could not be found in the list");
        return -1;
    }
    //sort
    public static void Sort(List<int> list, int l, int r)
    {
        if (l < r)
        {
            int m = l + (r - l) / 2;
            Sort(list, m + 1, r);
            Sort(list, l, m);
            Merge(list, l, m, r);
        }
    }
    private static void Merge(List<int> list, int l, int m, int r) // rewrite this, but without looking at the source code
    {
        int n1 = m - l + 1;
        int n2 = r - m;
        int[] L = new int[n1];
        int[] R = new int[n2];
        int i, j;
        for (i = 0; i < n1; ++i)
            L[i] = list[l + i];
        for (j = 0; j < n2; ++j)
            R[j] = list[m + 1 + j];
        i = 0;
        j = 0;
        int k = l;
        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
            {
                list[k] = L[i];
                i++;
            }
            else
            {
                list[k] = R[j];
                j++;
            }
            k++;
        }
        while (i < n1)
        {
            list[k] = L[i];
            i++;
            k++;
        }
        while (j < n2)
        {
            list[k] = R[j];
            j++;
            k++;
        }
    }
    public static void Sort(List<float> list, int l, int r)
    {
        if (l < r)
        {
            int m = l + (r - l) / 2;
            Sort(list, m + 1, r);
            Sort(list, l, m);
            Merge(list, l, m, r);
        }
    }
    private static void Merge(List<float> list, int l, int m, int r) // rewrite this, but without looking at the source code
    {
        int n1 = m - l + 1;
        int n2 = r - m;
        float[] L = new float[n1];
        float[] R = new float[n2];
        int i, j;
        for (i = 0; i < n1; ++i)
            L[i] = list[l + i];
        for (j = 0; j < n2; ++j)
            R[j] = list[m + 1 + j];
        i = 0;
        j = 0;
        int k = l;
        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
            {
                list[k] = L[i];
                i++;
            }
            else
            {
                list[k] = R[j];
                j++;
            }
            k++;
        }
        while (i < n1)
        {
            list[k] = L[i];
            i++;
            k++;
        }
        while (j < n2)
        {
            list[k] = R[j];
            j++;
            k++;
        }
    }
    //invert
    public static void Invert(List<int> list)
    {
        int m = (int)Mathf.Floor(list.Count / 2);
        for (int i = 0; i < m; ++i)
        {
            int temp = list[i];
            list[i] = list[list.Count - 1 - i];
            list[list.Count - 1 - i] = temp;
        }
    }
    //clone

    //remove duplicates

    //merge 

    //Merge and Sort

    //Debug.Log array contents
    //Horizontal Distance
    public static float HorizontalDistance(Vector2 point1, Vector2 point2)
    {
        float x = point2.x - point1.x;
        float y = point2.y - point1.y;
        return Mathf.Sqrt(Mathf.Pow(y, 2) + Mathf.Pow(y, 2));
    }
    public static float HorizontalDistance(float xPos1, float zPos1, float xPos2, float zPos2)
    {
        float x = xPos2 - xPos1;
        float z = zPos2 - xPos1;
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));
    }
}

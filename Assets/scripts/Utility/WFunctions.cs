using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WForLoopFunctions
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
    public static void Sort(List<int> list)
    {
        Sort(list, 0, list.Count - 1);
    }
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
    public static void Sort(List<float> list)
    {
        Sort(list, 0, list.Count - 1);
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
    public static List<int> Clone(List<int> list)
    {
        List<int> clonedList = new List<int>();
        for (int i = 0; i < list.Count; ++i)
        {
            clonedList.Add(list[i]);
        }
        return clonedList;
    }
    //remove duplicates
    public static void RemoveDuplicates(List<int> list)
    {
        for (int i = 0; i < list.Count - 1; ++i)
        {
            for (int j = i; j < list.Count; ++j)
            {
                if (list[i] == list[j])
                {
                    list.Remove(j);
                    --j;
                }
            }
        }
    }
    //merge 
    public static List<int> UnsortedMerge(List<List<int>> lists)
    {
        List<int> mergedList = new List<int>();
        for (int i = 0; i < lists.Count; ++i)
        {
            for (int j = 0; j < lists[i].Count; ++j)
            {
                mergedList.Add(lists[i][j]);
            }
        }
        return mergedList;
    }
    //Merge and Sort
    public static List<int> SortedMerge(List<List<int>> lists)
    {
        List<int> mergedList = UnsortedMerge(lists);
        Sort(mergedList);
        return mergedList;
    }
    //Sum
    public static float Sum(List<float> list)
    {
        float sum = 0;
        for (int i = 0; i < list.Count; ++i)
        {
            sum += list[i];
        }
        return sum;
    }
    //Average
    public static float Average(List<float> list)
    {
        return Sum(list) / list.Count;
    }
    //FindMax
    public static float FindMax(List<float> list)
    {
        float min = list[0];
        for (int i = 1; i < list.Count; ++i)
            min = Mathf.Min(min, list[i]);
        return min;
    }
    //FindMin
    public static float FindMin(List<float> list)
    {
        float max = list[0];
        for (int i = 1; i < list.Count; ++i)
            max = Mathf.Max(max, list[i]);
        return max;
    }
    //Round
    public static void Round(List<float> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            list[i] = Mathf.Round(list[i]);
        }
    }
    //FloatToInt
    public static List<int> FloatToInt(List<float> list)
    {
        List<int> clonedList = new List<int>();
        for (int i = 0; i < list.Count; ++i)
        {
            clonedList.Add((int)list[i]);
        }
        return clonedList;
    }
    //IntToFloat
    public static List<float> IntToFloat(List<int> list)
    {
        List<float> clonedList = new List<float>();
        for (int i = 0; i < list.Count; ++i)
        {
            clonedList.Add((float)(list[i]));
        }
        return clonedList;
    }
    //Debug.Log array contents
    
    
}
public static class WDistanceFunctions
{
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

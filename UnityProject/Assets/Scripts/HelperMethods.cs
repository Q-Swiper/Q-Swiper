using UnityEngine;

public static class HelperMethods
{
    public static GameObject GetChildByName(GameObject parent, string nameToSearch)
    {
        GameObject searchedChild = null;
        foreach (Transform child in parent.transform)
        {
            if (child.name == nameToSearch)
            {
                searchedChild = child.gameObject;
                break;
            }
        }
        return searchedChild;
    }
    public static float GetWidth(GameObject gameObject)
    {
        RectTransform rectTransform = (RectTransform)gameObject.transform;
        return rectTransform.rect.width;
    }
    public static float GetHeight(GameObject gameObject)
    {
        RectTransform rectTransform = (RectTransform)gameObject.transform;
        return rectTransform.rect.height;
    }

    public static int ExtractInt(string text)
    {
        string newText = string.Empty;

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
                newText += text[i];
        }
        if (newText.Length > 0)
        {
            return int.Parse(newText);
        }
        return 0;
    }
    public static void RemoveAllChildsFromGameobject(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            Object.Destroy(child.gameObject);
        }
    }
    public static void SetHeightRelativeToWidth(GameObject gameObject, float factor)
    {
        var width = GetWidth(gameObject);
        gameObject.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width * factor);
    }
    public static void SetWidthRelativeToHeight(GameObject gameObject, float factor)
    {
        var height = GetHeight(gameObject);
        gameObject.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height * factor);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class AlphaButton : MonoBehaviour
{
    public float AlphaThreshold = .1f;

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }
}

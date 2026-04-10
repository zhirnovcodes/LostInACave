using UnityEngine;
using UnityEngine.UI;

public class NetworkElement : MonoBehaviour
{
    public Image[] LevelImages;    // 4 images, index 0 = level 1 … index 3 = level 4
    public Image NoConnectionImage;

    public void SetLevel(int level)
    {
        NoConnectionImage.enabled = level == 0;

        for (int i = 0; i < LevelImages.Length; i++)
        {
            LevelImages[i].enabled = i == level - 1;
        }
    }
}

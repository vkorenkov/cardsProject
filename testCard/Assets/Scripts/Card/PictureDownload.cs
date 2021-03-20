using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PictureDownload : MonoBehaviour
{
    /// <summary>
    /// Card image field
    /// </summary>
    [SerializeField] Image cardImage;
    /// <summary>
    /// Cardback image field
    /// </summary>
    [SerializeField] Image cardBack;

    private void Awake()
    {
        StartCoroutine("GetImage");
    }

    /// <summary>
    /// Random image download coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator GetImage()
    {
        float width = Mathf.Round(cardImage.rectTransform.rect.width);   // Retrieving the original width and height of an image
        float height = Mathf.Round(cardImage.rectTransform.rect.height); //

        var request = UnityWebRequestTexture.GetTexture($"https://picsum.photos/{width}/{height}"); // Making a request to download an image

        yield return request.SendWebRequest(); // Executing a request

        Sprite sprite;

        if (!request.isHttpError && !request.isNetworkError)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request); // Downloading an image

            sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f)); // Creating a sprite from a downloaded image

            cardImage.sprite = sprite;
            cardBack.sprite = sprite;
        }
    }
}

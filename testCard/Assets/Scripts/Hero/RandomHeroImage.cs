using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomHeroImage : MonoBehaviour
{
    /// <summary>
    /// Character image
    /// </summary>
    [SerializeField] Image heroImage;

    /// <summary>
    /// All available images
    /// </summary>
    [SerializeField] List<Sprite> heroImages;

    void Start()
    {
        heroImage.sprite = heroImages[Random.Range(0, heroImages.Count)]; // Random hero image selection
    }
}

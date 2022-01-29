using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    public Sprite brokenBox;
    public Sprite normalBox;
    private int currentChildren = 0;
    private Image[] lifeImages;

    private void Start()
    {
        currentChildren = this.gameObject.transform.childCount;
        lifeImages = new Image[currentChildren];
        for (int i = 0; i < lifeImages.Length; i++) {
            lifeImages[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }
    public void UpdateLives(int currentLives = 0)
    {
        for (int i = 0; i < lifeImages.Length; i++){
            lifeImages[i].sprite = (i < currentLives) ? normalBox : brokenBox;
        }
    }
}

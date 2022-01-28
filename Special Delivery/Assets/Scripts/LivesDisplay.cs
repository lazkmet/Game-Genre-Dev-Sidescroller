using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
    public Sprite brokenBox;
    public Sprite normalBox;
    private int currentID = 0;
    private Image[] lifeImages;

    private void Start()
    {
        currentID = this.gameObject.transform.childCount - 1;
        lifeImages = new Image[currentID + 1];
        for (int i = 0; i < lifeImages.Length; i++) {
            lifeImages[i] = transform.GetChild(currentID).gameObject.GetComponent<Image>();
        }
    }
    public void UpdateLives(int currentLives = 0)
    {
        for (int i = 0; i < lifeImages.Length; i++){
            lifeImages[i].sprite = (i < currentLives) ? normalBox : brokenBox;
        }
    }
}

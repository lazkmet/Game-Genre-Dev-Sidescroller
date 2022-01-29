using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public SpriteRenderer renderSprite { get; private set; }
    public Sprite[] sprites;
    public int defaultSprite = 0;
    public bool defaultFaceLeft = true;
    public int numInSet;
    public int currentIndex;
    private bool facingLeft;
    private bool packageHeld;
    private void Awake()
    {
        renderSprite = gameObject.GetComponent<SpriteRenderer>();
        Reset();
    }
    public void SetSprite(int newSpriteIndex)
    {
        if (newSpriteIndex < numInSet)
        {
            int trueIndex = newSpriteIndex + (numInSet * (facingLeft ? 0 : 1)) + numInSet * (packageHeld ? 2 : 0);
            try
            {
                renderSprite.sprite = sprites[trueIndex];
                currentIndex = newSpriteIndex;
            }
            catch (System.NullReferenceException)
            {

            }
            catch (System.Exception) { 
            
            }
        }
        else {
            print("Out of Range: " + newSpriteIndex);
        }
    }
    public void TurnLeft() {
        facingLeft = true;
        SetSprite(currentIndex);
    }
    public void TurnRight()
    {
        facingLeft = false;
        SetSprite(currentIndex);
    }
    public void PackageHeld(bool newStatus) {
        packageHeld = newStatus;
        SetSprite(currentIndex);
    }
    public void Reset()
    {
        facingLeft = defaultFaceLeft;
        SetSprite(defaultSprite);
        packageHeld = false;
    }
}

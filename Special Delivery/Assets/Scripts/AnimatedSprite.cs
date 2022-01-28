using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    //This script originated from the Unity Pacman Tutorial at https://www.youtube.com/watch?v=TKt_VlMn_aA
    public SpriteRenderer renderSprite { get; private set; }
    public Sprite[] sprites;
    public float animationTime = 0.25f;
    public int animationFrame { get; private set;}
    public bool looping = true;
    private void Awake()
    {
        renderSprite = this.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(NextFrame),animationTime, animationTime);
    }
    private void NextFrame() {
        if (renderSprite.enabled) {
            if (++animationFrame >= sprites.Length && looping)
            {
                animationFrame = 0;
            }
            if (animationFrame > -1 && animationFrame < sprites.Length)
            {
                renderSprite.sprite = sprites[animationFrame];
            }
        }
    }
    public void Restart()
    {
        animationFrame = -1;
        NextFrame();
    }
}

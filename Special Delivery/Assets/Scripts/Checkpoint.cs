using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector2 position { get; private set; }
    public Collider2D activeArea { get; private set; }
    public bool packageHad { get; private set; }
    private GameManager manager;
    private void Awake()
    {
        position = this.gameObject.transform.position;
        try
        {
            activeArea = this.gameObject.GetComponent<Collider2D>();
        }
        catch (System.Exception)
        {

        }
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        packageHad = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Activate();
        }
    }
    public void Activate(){
        try
        {
            print("Checkpoint Activated");
            manager.activeCheckpoint = this;
            if (manager.player.hasPackage) {
                packageHad = true;
            }
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
}

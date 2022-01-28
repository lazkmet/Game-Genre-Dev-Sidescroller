using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuManager))]
public class GameManager : MonoBehaviour
{
    public MenuManager menu { get; private set; }
    public PlayerMovement player;
    public Package package;
    public Checkpoint startPoint;
    [HideInInspector]
    public Checkpoint activeCheckpoint;
    public int maxLives = 1;
    private int currentLives;
    private bool midcrash;
    [HideInInspector]
    public bool gameOver;
    private void Awake()
    {
        menu = gameObject.GetComponent<MenuManager>();
    }
    void Start()
    {
        ResetLevel();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver) {
            menu.TogglePause();
            try {
                 player.movementEnabled = !menu.isPaused;
            }
            catch (System.Exception ex) {
                print(ex.Message);
            }
        }
    }
    public void Loss() {
        print("Crash");
        if (!midcrash) {
            midcrash = true;
            currentLives--;
            UpdateLives();
            //spawn broken box particles
            player.movementEnabled = false;
            StartCoroutine("DelayedGameOver");
        }
    }
    public void GameOver() {
        gameOver = true;
        try
        {
            menu.SetActiveScreen(1);
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
    private void ResetLevel() {
        currentLives = maxLives;
        menu.SetActiveScreen(0);
        activeCheckpoint = startPoint;
        UpdateLives();
        midcrash = false;
        gameOver = false;
    }
    private IEnumerator DelayedGameOver() {
        print("Current Speed: " + player.body.velocity.magnitude);
        while (player.body.velocity.magnitude > 0.001f) {
            player.body.velocity = player.body.velocity * 0.9975f;
            yield return null;
        }
        print("0 velocity");
        yield return new WaitForSecondsRealtime(0.5f);
        if (currentLives > 0)
        {
            player.ResetToCheckpoint(activeCheckpoint);
            package.Reset();
        }
        else
        {
            GameOver();
        }
        midcrash = false;
    }
    private void UpdateLives() {
        try
        {
            menu.screens[0].GetComponent<LivesDisplay>().UpdateLives(currentLives);
            activeCheckpoint = startPoint;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
}

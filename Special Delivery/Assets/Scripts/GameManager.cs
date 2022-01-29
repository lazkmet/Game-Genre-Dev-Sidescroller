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
    private EnemyBehavior[] enemies;
    [HideInInspector]
    public bool gameOver;
    private void Awake()
    {
        menu = gameObject.GetComponent<MenuManager>();
        enemies = FindObjectsOfType<EnemyBehavior>();
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
        if (Input.GetKeyDown(KeyCode.CapsLock) && !gameOver)
        {
            GameOver();
        }
    }
    public void Loss() {
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
    public void ResetLevel() {
        currentLives = maxLives;
        menu.Reset();
        activeCheckpoint = startPoint;
        UpdateLives();
        midcrash = false;
        gameOver = false;
        player.Reset();
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].Reset();
        }
    }
    private IEnumerator DelayedGameOver() {
        while (player.body.velocity.magnitude > 0.001f) {
            player.body.velocity = player.body.velocity * 0.9975f;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        if (currentLives > 0)
        {
            player.ResetToCheckpoint(activeCheckpoint);
            package.Reset();
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Reset();
            }
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
            menu.screens[0].GetComponentInChildren<LivesDisplay>().UpdateLives(currentLives);
            activeCheckpoint = startPoint;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
    public void Win() {
        Time.timeScale = 0;
        try
        {
            menu.SetActiveScreen(2);
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
}

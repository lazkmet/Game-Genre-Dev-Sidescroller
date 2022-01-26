using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MenuManager))]
public class GameManager : MonoBehaviour
{
    public MenuManager menu {get; private set;}
    public PlayerMovement player { get; private set; }
    private void Awake()
    {
        menu = gameObject.GetComponent<MenuManager>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
    void Start()
    {
        ResetLevel();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            menu.TogglePause();
            try {

            }
            catch (System.Exception ex) {
                print(ex.Message);
            }
        }
    }
    void ResetLevel() {
        if (menu.isPaused) {
            menu.TogglePause();
        }
    }
}

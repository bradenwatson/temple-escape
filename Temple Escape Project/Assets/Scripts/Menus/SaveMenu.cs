using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenus : MonoBehaviour
{
    public static SaveMenus GameOverMenu { get; set; }
    public static SaveMenus LevelMenu { get; set; }
    public static SaveMenus PauseMenu { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (GameOverMenu != null)
        {
            Destroy(this.gameObject);
        }
        if (LevelMenu != null)
        {
            Destroy(this.gameObject);
        }
        if (PauseMenu != null)
        {
            Destroy(this.gameObject);
        }
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}

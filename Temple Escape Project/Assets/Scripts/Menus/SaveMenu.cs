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
            Destroy(gameObject);
        }
        if (LevelMenu != null)
        {
            Destroy(gameObject);
        }
        if (PauseMenu != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        // need assign when == null
    }
}

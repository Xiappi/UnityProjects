using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Linq;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject Template;

    private GridLayout gridLayout;


    // Start is called before the first frame update
    void Start()
    {

        var scenes = EditorBuildSettings.scenes
                    .Where(scene => scene.enabled)
                    .Select(scene => scene.path)
                    .ToArray();

        var path = @"C:\Users\matta\Unity\TileVania\Assets\Scenes\Screenshots\Level 1.png";
        var bytes = System.IO.File.ReadAllBytes(path);
        var texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        var sceneSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        gridLayout = GetComponent<GridLayout>();

        foreach (var scene in scenes)
        {
            Template.GetComponent<Image>().sprite = sceneSprite;
            Template.GetComponentInChildren<TextMeshProUGUI>().text = scene;
            Instantiate(Template, transform);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


}

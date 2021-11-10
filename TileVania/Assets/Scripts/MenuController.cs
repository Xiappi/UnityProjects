using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Linq;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject Template;
    [SerializeField] Sprite DefaultImage;

    private GridLayout gridLayout;


    // Start is called before the first frame update
    void Start()
    {
        PopulateScenes();
    }

    private void PopulateScenes()
    {
        var rawScenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            rawScenes.Add(SceneUtility.GetScenePathByBuildIndex(i));
        }
        var scenes = rawScenes.Where(scene => scene.Contains("Level"));

        // lots of gross string manipulation 
        foreach (var scene in scenes)
        {
            var sceneFileName = scene.Substring(scene.LastIndexOf('/') + 1, scene.Length - scene.IndexOf('.') + 1);
            var path = Path.Combine("Assets/Scenes/Screenshots", (sceneFileName + ".png"));
            Sprite sceneSprite;
            try
            {
                var bytes = System.IO.File.ReadAllBytes(path);
                var texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);

                sceneSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            catch (Exception)
            {
                sceneSprite = DefaultImage;
            }

            gridLayout = GetComponent<GridLayout>();

            var gameObj = Instantiate(Template, transform);
            gameObj.GetComponent<Image>().sprite = sceneSprite;
            gameObj.GetComponentInChildren<TextMeshProUGUI>().text = sceneFileName;
            gameObj.GetComponent<Button>().onClick.AddListener(() => LoadScene(scene));
        }
    }

    public void LoadScene(string str)
    {
        SceneManager.LoadScene(SceneUtility.GetBuildIndexByScenePath(str));
    }

}

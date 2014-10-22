using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TagFrenzy;
using System;



public class Test : MonoBehaviour
{

    public string Foreground = "foreground";
    private string Background = "background";
    private string Enemy = "enemy";
    private string Friend = "friend";


    void OnGUI()
    {

        if (GUI.Button(new Rect(10, 10, 150, 20), "Select foreground"))
        {
            //Can also call with strongly typed enums.  Call with strings here in case people delete tags from demo scene to prevent errors
            //var gameObjects = MultiTag.FindGameObjectsWithTags(Tags.Foreground, TagMatch.Or);
            var gameObjects = MultiTag.FindGameObjectsWithTags(Foreground, TagMatch.Or);
            HideAllExceptSelected(gameObjects);
            LogTags(gameObjects);
        }
        if (GUI.Button(new Rect(10, 40, 150, 20), "Select background"))
        {
            var gameObjects = MultiTag.FindGameObjectsWithTags(Background, TagMatch.Or);
            HideAllExceptSelected(gameObjects);
            LogTags(gameObjects);
        }

        if (GUI.Button(new Rect(10, 70, 150, 20), "Select enemies"))
        {
            var gameObjects = MultiTag.FindGameObjectsWithTags(Enemy, TagMatch.Or);
            HideAllExceptSelected(gameObjects);
            LogTags(gameObjects);
        }

        if (GUI.Button(new Rect(10, 100, 150, 20), "Select friends"))
        {
            var gameObjects = MultiTag.FindGameObjectsWithTags(Friend, TagMatch.Or);
            HideAllExceptSelected(gameObjects);
            LogTags(gameObjects);
        }

        if (GUI.Button(new Rect(10, 130, 150, 20), "Background enemies"))
        {
            var gameObjects = MultiTag.FindGameObjectsWithTags(Background, Enemy, TagMatch.And);
            HideAllExceptSelected(gameObjects);
            LogTags(gameObjects);
        }

        if (GUI.Button(new Rect(10, 160, 150, 20), "Foreground friends"))
        {
            var gameObjects = MultiTag.FindGameObjectsWithTags(Foreground, Friend, TagMatch.And);
            HideAllExceptSelected(gameObjects);
            LogTags(gameObjects);
        }

        if (GUI.Button(new Rect(10, 190, 150, 20), "Show all"))
        {
            ShowAll();
        }

    }

    void ShowAll()
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.renderer != null)
            {
                g.renderer.enabled = true;
            }

        }
    }

    void LogTags(List<GameObject> selectedObjects)
    {
        foreach (GameObject go in selectedObjects)
        {
            Debug.Log(go.name + ":Attached tags - " + String.Join(",",go.tags().ToArray()));
        }
    }

    void HideAllExceptSelected(List<GameObject> selectedObjects)
    {
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.renderer != null)
            {
                if (selectedObjects.Contains(g))
                {
                    g.renderer.enabled = true;
                }
                else
                {
                    g.renderer.enabled = false;
                }
            }
          
        }
    }
}


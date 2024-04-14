using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURLOnClick : MonoBehaviour
{
    public string url;

    public void GoToURL()
    {
        Application.OpenURL(url);
    }
}
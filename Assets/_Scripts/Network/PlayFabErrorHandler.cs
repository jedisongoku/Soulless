using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;

public class PlayFabErrorHandler
{

    public static void HandlePlayFabError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
        if (error.ErrorDetails != null)
        {
            foreach (var ed in error.ErrorDetails)
            {
                Debug.Log(string.Format("ErrorDetail: {0}", ed));
                var kvp = ed.Value;
                foreach (var e in kvp)
                {
                    Debug.Log(string.Format("ErrorInfo: {0}", e));
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS
{
    public class PlayerPrefsData : IDataProvider
    {
        public FPSData Load()
        {
            var fpsData = new FPSData();

            if (PlayerPrefs.HasKey("PlayerName"))
                Debug.Log("User already in the game");

            fpsData.PlayerName = PlayerPrefs.GetString("PlayerName", "Player01");
            fpsData.Health = PlayerPrefs.GetFloat("Health", 100);
            fpsData.PlayerPosition = ConvertFunctions.GetVector3(PlayerPrefs.GetString("PlayerPosition"));

            Debug.Log("Playerprefs data loaded");
            return fpsData;
        }

        public void Save(FPSData fPSData)
        {
            PlayerPrefs.SetString("PlayerName", fPSData.PlayerName);
            PlayerPrefs.SetFloat("Health", fPSData.Health);
            PlayerPrefs.SetString("PlayerPosition", fPSData.PlayerPosition.ToString());

            Debug.Log("PlayerPrefs data saved");

            PlayerPrefs.Save();
        }

        public void SetOption(string path)
        {
        }
    }

}
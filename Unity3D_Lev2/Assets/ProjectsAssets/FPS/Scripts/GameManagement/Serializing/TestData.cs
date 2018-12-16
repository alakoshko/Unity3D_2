using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class TestData : BaseSceneObject
    {
        public enum DataProviders
        {
            TXT,
            JSON,
            XML,
            PLAYER_PREFS
        }

        public DataProviders Provider;
        private DataManager _dataManager = new DataManager();

        // Use this for initialization
        private void Start()
        {
            //Debug.Log(Application.dataPath);
            //Debug.Log(Application.persistentDataPath);
            //Debug.Log(Application.streamingAssetsPath);
            //Debug.Log(Application.temporaryCachePath);
            Debug.Log(Application.systemLanguage);

            var path = Application.dataPath;
            var Player = new FPSData
            {
                PlayerName = PlayerModel.LocalPlayer.PlayerName,
                Health = PlayerModel.LocalPlayer.MaxHealth,
                //PlayerPosition = new Vector3(157.71f, 43.321f, 55.53f)
                PlayerPosition = new Vector3
                {
                    x = PlayerModel.LocalPlayer.transform.position.x,
                    y = PlayerModel.LocalPlayer.transform.position.y,
                    z = PlayerModel.LocalPlayer.transform.position.z
                }
            };


            switch (Provider)
            {
                case DataProviders.JSON:
                    _dataManager.SetData<JsonData>();
                    break;
                case DataProviders.XML:
                    _dataManager.SetData<XMLData>();
                    break;
                case DataProviders.PLAYER_PREFS:
                    _dataManager.SetData<PlayerPrefsData>();
                    break;
                case DataProviders.TXT:
                    _dataManager.SetData<StreamData>();
                    break;

            }

            ////var dataProvider = new StreamData();
            //var dataProvider = new PlayerPrefsData();
            _dataManager.SetOption(path);
            Debug.Log(Player);
            _dataManager.Save(Player);
            Debug.Log(_dataManager.Load());


        }
    }
}
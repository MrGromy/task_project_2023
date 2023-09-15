using System;
using System.IO;
using Newtonsoft.Json;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;
namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string jsonFileDir = $"WordSearch/Levels/{levelIndex}";
            TextAsset textAsset = Resources.Load<TextAsset>(jsonFileDir);
            LevelInfo levelInfo = JsonConvert.DeserializeObject<LevelInfo>(textAsset.text.ToString());
            if (levelInfo == null) return null;// throw new Exception("Ошибки в файле уровня");
            return levelInfo;
        }
    }
}
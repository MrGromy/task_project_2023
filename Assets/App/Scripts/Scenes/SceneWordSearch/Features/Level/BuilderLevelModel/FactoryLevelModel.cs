using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            List<char> letters = new List<char>();
            foreach (var word in words)
            {
                foreach (char c in word)
                {
                    if (letters.Count(y=>y==c)<word.Count(y => y == c))
                    {
                        for(int i = 0; i < word.Count(y => y == c) - letters.Count(y => y == c); i++)
                        {
                            letters.Add(c);
                        }
                    }
                       
                }
            }
            return letters;
        }
    }
}
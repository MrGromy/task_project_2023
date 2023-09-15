using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        public GridFillWords LoadModel(int index)
        {
            
            List<string> lines = GetListFromFile("Fillwords/pack_0");
            string line = lines[index];
            if (line == null) return null;
            else
            {
                lines = GetListFromFile("Fillwords/words_list");
                string[] fillword = line.Split(' ');
                if (fillword.Length % 2 != 0) return null;
                int size = 0;
                for (int i = 0; i < fillword.Length; i += 2)
                {
                    string word = lines[int.Parse(fillword[i])];
                    if (word == null) return null;
                    else
                    {
                        fillword[i] = word;
                        size += word.Length;
                    }
                }
                int side = GetSide(size);
                if (side != -1)
                {
                    Vector2Int facet = new Vector2Int();
                    facet.Set(side, side);
                    GridFillWords result = new GridFillWords(facet);
                    for (int i = 1; i < fillword.Length; i += 2)
                    {
                        string[] nums = fillword[i].Split(";");
                        if (!Mathf.Approximately(nums.Length, fillword[i - 1].Length)) return null;

                        for (int j = 0; j < nums.Length; j++)
                        {
                            bool isNum = int.TryParse(nums[j], out int pos);
                            if (pos >= size || !isNum) return null;
                            if (result.Get(pos / side, pos % side) != null) return null;

                            result.Set(pos / side, pos % side, new CharGridModel(fillword[i - 1][j]));
                        }

                    }
                    return result;
                }
                else return null;
            }
        }
        public int GetSide(int size)
        {
            float i = 2, sqw = i * i;

            while (sqw <= size)
            {
                if (Mathf.Approximately(sqw, size)) return (int)i;
                i++;
                sqw = i * i;
            }
            return -1;
        }
        public List<string> GetListFromFile(string path)
        {
            var textAsset = (TextAsset)Resources.Load(path, typeof(TextAsset));
            List<string> lines = new List<string>(textAsset.text.Split("\r\n"));
            return lines;
        }

    }
}
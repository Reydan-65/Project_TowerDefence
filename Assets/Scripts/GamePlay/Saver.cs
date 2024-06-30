using SpaceShooter;
using System;
using System.IO;
using UnityEngine;

namespace TowerDefence
{
    [Serializable]
    public class Saver<T>
    {
        private static string Path(string filename)
        {
            return $"{Application.persistentDataPath}/{filename}";
        }

        public static void TryLoad(string filename, ref T data)
        {
            var path = Path(filename);

            if (File.Exists(path))
            {
                Debug.Log($"File {path} loaded.");
            }
            else
            {
                foreach (var properties in LevelSequencesController.Instance.LevelSequences.LevelsProperties)
                {
                    properties.LevelScore = 0;
                }

                Debug.Log($"File {path} not found.");
            }
        }

        public static void Save(string filename, T data)
        {
            Debug.Log($"File {Path(filename)} saved.");



            var dataString = JsonUtility.ToJson(data);

            //Debug.Log(dataString);

            File.WriteAllText(Path(filename), dataString);
        }
    }
}
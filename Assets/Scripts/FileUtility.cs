using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Scripts.Gongtong;
using Assets.Scripts.Sikao;

namespace Assets.Scripts
{
    public class FileUtility : MonoBehaviour
    {
        // プレイ中データディレクトリ名
        public const string GAME_DATA_DIR_NAME = "GameData";
        // 設定・プレイヤーディレクトリ名
        public const string SETTING_PLAYER_DATA_DIR_NAME = "SettingPlayerData";
        // 学習ディレクトリ名
        public const string TRANSITION_DIR_NAME = "Transition";

        // 設定ファイル名
        public const string SHE_DING_FILE_NAME = "SheDing";
        // ルールファイル名
        public const string GUI_ZE_FILE_NAME = "GuiZe";
        // 場ファイル名
        public const string CHANG_FILE_NAME = "Chang";
        // 牌ファイル名
        public const string PAI_FILE_NAME = "Pai";
        // 雀士ファイル名
        public const string QUE_SHI_FILE_NAME = "QueShi";

        // 書込用のディレクトリー取得
        public static string GetDirectory(string directoryName)
        {
            string directory = Path.Combine(Application.persistentDataPath, directoryName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }

        // ファイル削除
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        // ファイル移動
        public static void MoveFile(string fromFilePath, string toFilePath)
        {
            if (File.Exists(fromFilePath))
            {
                File.Move(fromFilePath, toFilePath);
            }
        }

        // 設定ファイル書込
        public static void WriteSheDingFile()
        {
            string directory = GetDirectory(SETTING_PLAYER_DATA_DIR_NAME);
            File.WriteAllText(Path.Combine(directory, $"{SHE_DING_FILE_NAME}.json"), JsonUtility.ToJson(SheDing.Instance));
        }
        // 設定ファイル削除
        public static void DeleteSheDingFile()
        {
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            DeleteFile(Path.Combine(directory, $"{SHE_DING_FILE_NAME}.json"));
        }

        // ルールファイル書込
        public static void WriteGuiZeFile()
        {
            string directory = GetDirectory(SETTING_PLAYER_DATA_DIR_NAME);
            File.WriteAllText(Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json"), JsonUtility.ToJson(GuiZe.Instance));
        }
        // ルールファイル削除
        public static void DeleteGuiZeFile()
        {
            string directory = GetDirectory(SETTING_PLAYER_DATA_DIR_NAME);
            DeleteFile(Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json"));
        }

        // 記録ファイル書込
        public static void WriteJiLuFile(string mingQian, JiLu jiLu)
        {
            string directory = GetDirectory(SETTING_PLAYER_DATA_DIR_NAME);
            File.WriteAllText(Path.Combine(directory, $"{mingQian}.json"), JsonUtility.ToJson(jiLu));
        }
        // 記録ファイル削除
        public static void DeleteJiLuFile(string mingQian)
        {
            string directory = GetDirectory(SETTING_PLAYER_DATA_DIR_NAME);
            DeleteFile(Path.Combine(directory, $"{mingQian}.json"));
        }
        // 記録ファイル読込
        public static JiLu ReadJiLuFile(string mingQian)
        {
            string directory = GetDirectory(SETTING_PLAYER_DATA_DIR_NAME);
            string filePath = Path.Combine(directory, $"{mingQian}.json");
            if (File.Exists(filePath))
            {
                return JsonUtility.FromJson<JiLu>(File.ReadAllText(filePath));
            }
            return null;
        }

        // ゲームデータ全削除
        public static void DeleteAllGameDataFile()
        {
            string directory = GetDirectory(GAME_DATA_DIR_NAME);
            DirectoryInfo directoryInfo = new(directory);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }

        // ゲームデータ全書込
        public static void WriteAllGameDataFile()
        {
            string directory = GetDirectory(GAME_DATA_DIR_NAME);
            File.WriteAllText(Path.Combine(directory, $"{CHANG_FILE_NAME}.json"), JsonUtility.ToJson(Chang.Instance));
            File.WriteAllText(Path.Combine(directory, $"{PAI_FILE_NAME}.json"), JsonUtility.ToJson(Pai.Instance));
            for (int i = 0; i < 4; i++)
            {
                DeleteFile(Path.Combine(directory, $"{QUE_SHI_FILE_NAME}{i}.json"));
            }
            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                File.WriteAllText(Path.Combine(directory, $"{QUE_SHI_FILE_NAME}{i}.json"), JsonUtility.ToJson(MaQue.Instance.queShis[i]));
            }
        }

        // 学習データ書込
        public static void WriteTransitionFile(List<Transition> ziJiaList, List<Transition> taJiaList)
        {
            string dirPath = GetDirectory(TRANSITION_DIR_NAME);
            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");

            string dirPathZiJia = Path.Combine(dirPath, "ZiJia");
            if (!Directory.Exists(dirPathZiJia))
            {
                Directory.CreateDirectory(dirPathZiJia);
            }
            if (ziJiaList.Count > 0)
            {
                File.WriteAllText(Path.Combine(dirPathZiJia, $"ZiJia_{timestamp}.json"), JsonConvert.SerializeObject(ziJiaList, Formatting.None));
            }

            string dirPathTaJia = Path.Combine(dirPath, "TaJia");
            if (!Directory.Exists(dirPathTaJia))
            {
                Directory.CreateDirectory(dirPathTaJia);
            }
            if (taJiaList.Count > 0)
            {
                File.WriteAllText(Path.Combine(dirPathTaJia, $"TaJia_{timestamp}.json"), JsonConvert.SerializeObject(taJiaList, Formatting.None));
            }
        }
    }
}

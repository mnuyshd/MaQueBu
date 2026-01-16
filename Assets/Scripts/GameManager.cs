using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Gongtong;
using Assets.Scripts.Sikao.Shi;
using Assets.Scripts.Sikao;

namespace Assets.Scripts
{
    [DefaultExecutionOrder(3)]
    public class GameManager : MonoBehaviour
    {
        // イベント
        public enum Event
        {
            // 開始
            KAI_SHI,
            // 雀士選択
            QUE_SHI_XUAN_ZE,
            // フォロー雀士選択
            FOLLOW_QUE_SHI_XUAN_ZE,
            // 場決
            CHANG_JUE,
            // 親決
            QIN_JUE,
            // 荘初期化
            ZHUANG_CHU_QI_HUA,
            // 配牌
            PEI_PAI,
            // 対局
            DUI_JU,
            // 対局終了
            DUI_JU_ZHONG_LE,
            // 役表示
            YI_BIAO_SHI,
            // 点表示
            DIAN_BIAO_SHI,
            // 荘終了
            ZHUANG_ZHONG_LE
        }

        // 全雀士
        public static List<QueShi> allQueShis = new()
        {
            new QueJiXie("機械雀士") { selected = true },
            new QueXiaoLu() {selected = true },
            new UchidaKou() { selected = true },
            new TakamiNozomu() { selected = false },
            new SomeyaMei() { selected = false },
            new KouzuNaruto() { selected = false },
            new KouzuTorako() { selected = false },
            new YakudaJunji() { selected = false },
            new MenzenJunko() { selected = false },
            new HikitaMamoru() { selected = false },
            // new QueXueXi() { selected = false },
        };

        // フレームレート
        public static readonly int FRAME_RATE = 60;
        // 待ち時間(デフォルト)
        public static readonly float WAIT_TIME = 0.3f;
        // 待ち時間
        public static float waitTime;
        // 画面向き
        public static ScreenOrientation orientation;
        // コルーチン処理中フラグ
        public static Dictionary<Event, Coroutine> coroutines = new();

        void Start()
        {
            Application.targetFrameRate = FRAME_RATE;
            waitTime = WAIT_TIME;
            orientation = Screen.orientation;
            if (orientation == ScreenOrientation.PortraitUpsideDown)
            {
                orientation = ScreenOrientation.Portrait;
            }

            coroutines.Clear();
            // データ読込
            LoadData();
            // データ読込後に描画
            Draw.Instance.DrawInitialDisplay();
        }

        // データ読込
        private void LoadData()
        {
            Debug.Log(Application.persistentDataPath);

            string settingPlayerDataDir = Path.Combine(Application.persistentDataPath, FileUtility.SETTING_PLAYER_DATA_DIR_NAME);
            // 設定の読込
            string sheDingFilePath = Path.Combine(settingPlayerDataDir, $"{FileUtility.SHE_DING_FILE_NAME}.json");
            LoadFromJson(sheDingFilePath, SheDing.Instance);
            // ルールの読込
            string guiZeFilePath = Path.Combine(settingPlayerDataDir, $"{FileUtility.GUI_ZE_FILE_NAME}.json");
            LoadFromJson(guiZeFilePath, GuiZe.Instance);

            string gameDataDir = Path.Combine(Application.persistentDataPath, FileUtility.GAME_DATA_DIR_NAME);
            // 場の読込
            string changFilePath = Path.Combine(gameDataDir, $"{FileUtility.CHANG_FILE_NAME}.json");
            LoadFromJson(changFilePath, Chang.Instance);
            // 牌の読込
            string paiFilePath = Path.Combine(gameDataDir, $"{FileUtility.PAI_FILE_NAME}.json");
            LoadFromJson(paiFilePath, Pai.Instance);
        }

        private void LoadFromJson<T>(string filePath, T target)
        {
            if (File.Exists(filePath))
            {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), target);
            }
        }

        void Update()
        {
            UpdateOrientation();
            UpdateMaQue();
            UpdateDraw();
        }

        // 画面回転
        private void UpdateOrientation()
        {
            if (orientation == Screen.orientation)
            {
                return;
            }

            orientation = Screen.orientation;
            if (orientation == ScreenOrientation.PortraitUpsideDown)
            {
                orientation = ScreenOrientation.Portrait;
            }
            // scale設定
            Draw.Instance.SetScale();
            switch (Chang.Instance.eventStatus)
            {
                // 対局
                case Event.DUI_JU:
                    Chang.Instance.isDuiJuDraw = true;
                    break;
                // 対局終了
                case Event.DUI_JU_ZHONG_LE:
                    Chang.Instance.isDuiJuZhongLeDraw = true;
                    break;
                // 役表示
                case Event.YI_BIAO_SHI:
                    Chang.Instance.isYiBiaoShiDraw = true;
                    break;
            }
        }

        // ゲーム
        private void UpdateMaQue()
        {
            switch (Chang.Instance.eventStatus)
            {
                // 開始
                case Event.KAI_SHI:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.KaiShi);
                    break;
                // 雀士選択
                case Event.QUE_SHI_XUAN_ZE:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.QueShiXuanZe);
                    break;
                // フォロー雀士選択
                case Event.FOLLOW_QUE_SHI_XUAN_ZE:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.FollowQueShiXuanZe);
                    break;
                // 場決
                case Event.CHANG_JUE:
                    MaQue.Instance.ChangJue();
                    break;
                // 親決
                case Event.QIN_JUE:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.QinJue);
                    break;
                // 荘初期化
                case Event.ZHUANG_CHU_QI_HUA:
                    MaQue.Instance.ZhuangChuQiHua();
                    break;
                // 配牌
                case Event.PEI_PAI:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.PeiPai);
                    break;
                // 対局
                case Event.DUI_JU:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.DuiJu);
                    break;
                // 対局終了
                case Event.DUI_JU_ZHONG_LE:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.DuiJuZhongLe);
                    break;
                // 役表示
                case Event.YI_BIAO_SHI:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.YiBiaoShi);
                    break;
                // 点表示
                case Event.DIAN_BIAO_SHI:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.DianBiaoShi);
                    break;
                // 荘終了
                case Event.ZHUANG_ZHONG_LE:
                    StartCoroutineProcess(Chang.Instance.eventStatus, MaQue.Instance.ZhuangZhong);
                    break;
            }
        }

        private Coroutine StartCoroutineProcess(Event status, Func<IEnumerator> action)
        {
            if (!coroutines.ContainsKey(status) || coroutines[status] == null)
            {
                coroutines[status] = StartCoroutine(action());
            }
            return coroutines[status];
        }

        // 描画
        private void UpdateDraw()
        {
            // 描画
            DrawProcess(ref Chang.Instance.isKaiShiDraw, Draw.Instance.DrawKaiShi);
            // 雀士選択
            DrawProcess(ref Chang.Instance.isQueShiXuanZeDraw, Draw.Instance.DrawQueShiXuanZe);
            // フォロー雀士選択
            DrawProcess(ref Chang.Instance.isFollowQueShiXuanZeDraw, Draw.Instance.DrawFollowQueShiXuanZe);
            // 親決
            DrawProcess(ref Chang.Instance.isQinJueDraw, Draw.Instance.DrawQinJue);
            // 配牌
            DrawProcess(ref Chang.Instance.isPeiPaiDraw, Draw.Instance.DrawPeiPai);
            // 対局
            DrawProcess(ref Chang.Instance.isDuiJuDraw, Draw.Instance.DrawDuiJu);
            // 対局終了
            DrawProcess(ref Chang.Instance.isDuiJuZhongLeDraw, Draw.Instance.DrawDuiJuZhongLe);
            // 役表示
            DrawProcess(ref Chang.Instance.isYiBiaoShiDraw, Draw.Instance.DrawYiBiaoShi);
            // 点表示
            DrawProcess(ref Chang.Instance.isDianBiaoShiDraw, Draw.Instance.DrawDianBiaoShi);
            // 荘終了
            DrawProcess(ref Chang.Instance.isZhuangZhongLeDraw, Draw.Instance.DrawZhuangZhongLe);
            // 点差
            if (Chang.Instance.isDianChaDraw)
            {
                Draw.Instance.DrawJuFrame();
                Chang.Instance.isDianChaDraw = Chang.Instance.isDianCha;
            }
        }

        private void DrawProcess(ref bool flag, Action drawAction)
        {
            if (flag)
            {
                drawAction();
                flag = false;
            }
        }
        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                string exceptionDirName = "Exception";
                string exceptionDirPath = Path.Combine(Application.persistentDataPath, exceptionDirName);
                Directory.CreateDirectory(exceptionDirPath);
                DirectoryInfo dir = new(exceptionDirPath);
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }

                FileUtility.MoveFile(Path.Combine(Application.persistentDataPath, FileUtility.GAME_DATA_DIR_NAME, $"{FileUtility.CHANG_FILE_NAME}.json"), Path.Combine(exceptionDirPath, $"{FileUtility.CHANG_FILE_NAME}.json"));
                FileUtility.MoveFile(Path.Combine(Application.persistentDataPath, FileUtility.GAME_DATA_DIR_NAME, $"{FileUtility.PAI_FILE_NAME}.json"), Path.Combine(Application.persistentDataPath, exceptionDirName, $"{FileUtility.PAI_FILE_NAME}.json"));
                for (int i = 0; i < 4; i++)
                {
                    FileUtility.MoveFile(Path.Combine(Application.persistentDataPath, FileUtility.GAME_DATA_DIR_NAME, $"{FileUtility.QUE_SHI_FILE_NAME}{i}.json"), Path.Combine(Application.persistentDataPath, exceptionDirName, $"{FileUtility.QUE_SHI_FILE_NAME}{i}.json"));
                }
            }
        }
    }
}

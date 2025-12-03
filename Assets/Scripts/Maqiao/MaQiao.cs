using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using Newtonsoft.Json;

using Assets.Scripts.Gongtong;
using Assets.Scripts.Sikao;
using Assets.Scripts.Sikao.Shi;
using State = Assets.Scripts.Sikao.State;
using Newtonsoft.Json.Linq;

namespace Assets.Scripts.Maqiao
{
    // 麻雀
    public class MaQiao : MonoBehaviour
    {
        private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);

        // イベント
        public enum Event
        {
            // 開始
            KAI_SHI,
            // 雀士選択
            QIAO_SHI_XUAN_ZE,
            // フォロー雀士選択
            FOLLOW_QIAO_SHI_XUAN_ZE,
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

        // 輪荘・連荘
        public enum Zhuang
        {
            // 続行
            XU_HANG,
            // 輪荘
            LUN_ZHUANG,
            // 連荘
            LIAN_ZHUANG
        }

        // フレームレート
        private static readonly int FRAME_RATE = 60;
        // 待ち時間(デフォルト)
        private static readonly float WAIT_TIME = 0.3f;
        // プレイヤー牌倍率
        private static readonly float PLAYER_PAI_SCALE = 1.4f;
        private static readonly float PLAYER_PAI_SCALE_LANDSCAPE = 1.2f;
        // プレイヤー名
        private static readonly string PLAYER_NAME = "プレイヤー";

        // プレイ中データディレクトリ名
        private static readonly string GAME_DATA_DIR_NAME = "GameData";
        // 設定・プレイヤーディレクトリ名
        private static readonly string SETTING_PLAYER_DATA_DIR_NAME = "SettingPlayerData";
        // 設定ファイル名
        private static readonly string SHE_DING_FILE_NAME = "SheDing";
        // ルールファイル名
        private static readonly string GUI_ZE_FILE_NAME = "GuiZe";
        // 場ファイル名
        private static readonly string CHANG_FILE_NAME = "Chang";
        // 牌ファイル名
        private static readonly string PAI_FILE_NAME = "Pai";
        // 雀士ファイル名
        private static readonly string QIAO_SHI_FILE_NAME = "QisoShi";

        // 送りモード
        private enum ForwardMode
        {
            // 通常
            NORMAL = 0,
            // 局早送り
            FAST_FORWARD = 1,
            // ずっと早送り
            FOREVER_FAST_FORWARD = 2,
        }
        // 送りモード
        private ForwardMode forwardMode = ForwardMode.NORMAL;

        // 待ち時間
        private static float waitTime = WAIT_TIME;

        // 雀士
        public static List<QiaoShi> qiaoShis;

        // 設定
        private static SheDing sheDing;
        // ルール
        public static GuiZe guiZe;
        // 場
        public static Chang chang;
        // 牌
        public static Pai pai;

        // スケール
        private Vector3 scale;
        // 牌横幅
        private float paiWidth;
        // 牌高さ
        private float paiHeight;
        // 画面向き
        private ScreenOrientation orientation;
        // 雀士ボタン幅
        private int quiaoShiButtonMaxLen;

        // サイコロ
        private int sai1 = 1;
        private int sai2 = 1;

        // コルーチン処理中フラグ
        private Coroutine kaiShiCoroutine;
        private Coroutine qiaoShiXuanZeCoroutine;
        private Coroutine followQiaoShiXuanZeCoroutine;
        private Coroutine qinJueCoroutine;
        private Coroutine peiPaiCoroutine;
        private Coroutine duiJuCoroutine;
        private Coroutine duiJuZhongLeCoroutine;
        private Coroutine yiBiaoShiCoroutine;
        private Coroutine dianBiaoShiCoroutine;
        private Coroutine zhuangZhongLeCoroutine;

        // Prefab
        public TextMeshProUGUI goText;
        public Image goFrame;
        public Image goLine;
        public Button goButton;
        public Slider goSlider;
        public GameObject[] goQiJias;
        public GameObject[] goSais;
        public Button goPai;
        public Image goDianBang100;
        public Image goDianBang1000;
        public Button goSpeech;
        public Button goBack;

        // Game Object
        private Canvas goCanvas;
        private Button goScreen;
        private Image goBenChang;
        private Image goGongTou;
        private GameObject[] goPais;
        private Sprite goJiXieShouPai;

        private TextMeshProUGUI goTitle;
        private TextMeshProUGUI goStart;
        private TextMeshProUGUI goJu;
        private Image goJuFrame;
        private Image goZiMoShiLine;
        private TextMeshProUGUI goBenChangText;
        private TextMeshProUGUI goGongTouText;
        private Image goCanShanPaiShu;
        private TextMeshProUGUI goFu;
        private Button[] goSheng;
        private TextMeshProUGUI[] goYi;
        private TextMeshProUGUI[] goFanShu;
        private TextMeshProUGUI[] goDianBang;
        private Image[] goFeng;
        private TextMeshProUGUI[] goShouQu;
        private TextMeshProUGUI[] goShouQuGongTuo;
        private TextMeshProUGUI[] goMingQian;
        private Button[] goQiaoShi;
        private Button goRandom;
        private Button goPlayerNoExists;
        private Button[] goYao;
        private Image[] goLizhiBang;
        private Image goQiJia;
        private Image goSai1;
        private Image goSai2;
        private Button goLeft;
        private Button goRight;
        private Button goSelect;
        private Button goMingWu;
        private Button goDianCha;
        private Button goSetting;
        private Button goCamera;
        private Button goScore;
        private Button goGuiZe;
        private Button goBackDuiJuZhongLe;
        private GameObject goSettingPanel;
        private GameObject goSettingDialogPanel;
        private GameObject goScorePanel;
        private Button[] goScoreQiaoShi;
        private GameObject goScoreDialogPanel;
        private GameObject goDataScrollView;
        private GameObject goDataContent;
        private GameObject goGuiZeScrollView;
        private GameObject goGuiZeContent;
        private GameObject goDataBackCanvas;
        private GameObject goGuiZeBackCanvas;
        private GameObject goGuiZeDialogPanel;

        private TextMeshProUGUI goJiJuMingQian;
        private Slider goJiLuNaoXuanShang;
        private TextMeshProUGUI goJiLuNaoXuanShangValue;
        private Slider goJiLuNaoYiPai;
        private TextMeshProUGUI goJiLuNaoYiPaiValue;
        private Slider goJiLuNaoShunZi;
        private TextMeshProUGUI goJiLuNaoShunZiValue;
        private Slider goJiLuNaoKeZi;
        private TextMeshProUGUI goJiLuNaoKeZiValue;
        private Slider goJiLuNaoLiZhi;
        private TextMeshProUGUI goJiLuNaoLiZhiValue;
        private Slider goJiLuNaoMing;
        private TextMeshProUGUI goJiLuNaoMingValue;
        private Slider goJiLuNaoRan;
        private TextMeshProUGUI goJiLuNaoRanValue;
        private Slider goJiLuNaoTao;
        private TextMeshProUGUI goJiLuNaoTaoValue;
        private TextMeshProUGUI goJiLuBanZhuangShu;
        private TextMeshProUGUI goJiLuDuiJuShu;
        private TextMeshProUGUI goJiLuJiJiDian;
        private TextMeshProUGUI goJiLuShunWei1Shuai;
        private TextMeshProUGUI goJiLuShunWei2Shuai;
        private TextMeshProUGUI goJiLuShunWei3Shuai;
        private TextMeshProUGUI goJiLuShunWei4Shuai;
        private TextMeshProUGUI goJiLuHeLeShuai;
        private TextMeshProUGUI goJiLuFangChongShuai;
        private TextMeshProUGUI goJiLuTingPaiShuai;
        private TextMeshProUGUI goJiLuPingJunHeLeDian;
        private TextMeshProUGUI goJiLuPingJunFangChongDian;
        private TextMeshProUGUI[] goYiShu;
        private TextMeshProUGUI[] goYiMing;
        private TextMeshProUGUI[] goYiManShu;
        private TextMeshProUGUI[] goYiManMing;

        // 雀士名前
        private readonly Dictionary<string, bool> qiaoShiMingQian = new()
        {
            { "機械雀士", true },
            { QiaoXiaoLu.MING_QIAN, true },
            { UchidaKou.MING_QIAN, true },
            { TakamiNozomu.MING_QIAN, false },
            { SomeyaMei.MING_QIAN, false },
            { KouzuNaruto.MING_QIAN, false },
            { KouzuTorako.MING_QIAN, false },
            { YakudaJunji.MING_QIAN, false },
            { MenzenJunko.MING_QIAN, false },
            { HikitaMamoru.MING_QIAN, false },
            { QiaoXueXi.MING_QIAN, false },
        };
        // 雀士取得
        private QiaoShi GetQiaoShi(string mingQian, bool isNew)
        {
            if (!isNew)
            {
                foreach (QiaoShi shi in qiaoShis)
                {
                    if (shi.mingQian == mingQian)
                    {
                        return shi;
                    }
                }
            }

            QiaoShi newShi = mingQian switch
            {
                QiaoXiaoLu.MING_QIAN => new QiaoXiaoLu(),
                QiaoXueXi.MING_QIAN => new QiaoXueXi(),
                HikitaMamoru.MING_QIAN => new HikitaMamoru(),
                SomeyaMei.MING_QIAN => new SomeyaMei(),
                UchidaKou.MING_QIAN => new UchidaKou(),
                KouzuNaruto.MING_QIAN => new KouzuNaruto(),
                KouzuTorako.MING_QIAN => new KouzuTorako(),
                YakudaJunji.MING_QIAN => new YakudaJunji(),
                MenzenJunko.MING_QIAN => new MenzenJunko(),
                TakamiNozomu.MING_QIAN => new TakamiNozomu(),
                _ => new QiaoJiXie(mingQian),
            };
            newShi.jiLu = new JiLu();
            return newShi;
        }

        private void AddQiaoShi(string mingQian, string jsonText)
        {
            QiaoShi shi = mingQian switch
            {
                QiaoXiaoLu.MING_QIAN => JsonUtility.FromJson<QiaoXiaoLu>(jsonText),
                QiaoXueXi.MING_QIAN => JsonUtility.FromJson<QiaoXueXi>(jsonText),
                HikitaMamoru.MING_QIAN => JsonUtility.FromJson<HikitaMamoru>(jsonText),
                SomeyaMei.MING_QIAN => JsonUtility.FromJson<SomeyaMei>(jsonText),
                UchidaKou.MING_QIAN => JsonUtility.FromJson<UchidaKou>(jsonText),
                KouzuNaruto.MING_QIAN => JsonUtility.FromJson<KouzuNaruto>(jsonText),
                KouzuTorako.MING_QIAN => JsonUtility.FromJson<KouzuTorako>(jsonText),
                YakudaJunji.MING_QIAN => JsonUtility.FromJson<YakudaJunji>(jsonText),
                MenzenJunko.MING_QIAN => JsonUtility.FromJson<MenzenJunko>(jsonText),
                TakamiNozomu.MING_QIAN => JsonUtility.FromJson<TakamiNozomu>(jsonText),
                _ => JsonUtility.FromJson<QiaoJiXie>(jsonText),
            };
            shi.jiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME, $"{mingQian}.json")));
            qiaoShis.Add(shi);
        }

        // データ書込
        private void WriteData()
        {
            string directory = Path.Combine(Application.persistentDataPath, GAME_DATA_DIR_NAME);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(Path.Combine(directory, $"{CHANG_FILE_NAME}.json"), JsonUtility.ToJson(chang));
            File.WriteAllText(Path.Combine(directory, $"{PAI_FILE_NAME}.json"), JsonUtility.ToJson(pai));
            for (int i = 0; i < 4; i++)
            {
                File.Delete(Path.Combine(directory, $"{QIAO_SHI_FILE_NAME}{i}.json"));
            }
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                File.WriteAllText(Path.Combine(directory, $"{QIAO_SHI_FILE_NAME}{i}.json"), JsonUtility.ToJson(qiaoShis[i]));
            }
        }

        // データ読込
        private void LoadData()
        {
            string directory = Path.Combine(Application.persistentDataPath, GAME_DATA_DIR_NAME);

            // 場の読込
            string changFilePath = Path.Combine(directory, $"{CHANG_FILE_NAME}.json");
            if (File.Exists(changFilePath))
            {
                chang = JsonUtility.FromJson<Chang>(File.ReadAllText(changFilePath));
            }
            else
            {
                chang = new()
                {
                    eventStatus = Event.KAI_SHI
                };
            }
            // 牌の読込
            string paiFilePath = Path.Combine(directory, $"{PAI_FILE_NAME}.json");
            if (File.Exists(paiFilePath))
            {
                pai = JsonUtility.FromJson<Pai>(File.ReadAllText(paiFilePath));
            }
            else
            {
                pai = new();
            }
            // 雀士の読込
            qiaoShis = new();
            for (int i = 0; i < 4; i++)
            {
                string qiaoShiFilePath = Path.Combine(directory, $"{QIAO_SHI_FILE_NAME}{i}.json");
                if (File.Exists(qiaoShiFilePath))
                {
                    string jsonText = File.ReadAllText(qiaoShiFilePath);
                    JObject jsonObj = JObject.Parse(File.ReadAllText(qiaoShiFilePath));
                    string mingQian = (string)jsonObj["mingQian"];
                    AddQiaoShi(mingQian, jsonText);
                }
            }
        }

        // データ削除
        private void DeleteData()
        {
            string directory = Path.Combine(Application.persistentDataPath, GAME_DATA_DIR_NAME);

            File.Delete(Path.Combine(directory, $"{CHANG_FILE_NAME}.json"));
            File.Delete(Path.Combine(directory, $"{PAI_FILE_NAME}.json"));
            for (int i = 0; i < 4; i++)
            {
                File.Delete(Path.Combine(directory, $"{QIAO_SHI_FILE_NAME}{i}.json"));
            }
        }

        void Start()
        {
            Application.targetFrameRate = FRAME_RATE;

            orientation = Screen.orientation;
            if (orientation == ScreenOrientation.PortraitUpsideDown)
            {
                orientation = ScreenOrientation.Portrait;
            }

            // 設定の読込
            Debug.Log(Application.persistentDataPath);
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            string sheDingFilePath = Path.Combine(directory, $"{SHE_DING_FILE_NAME}.json");
            if (File.Exists(sheDingFilePath))
            {
                sheDing = JsonUtility.FromJson<SheDing>(File.ReadAllText(sheDingFilePath));
            }
            else
            {
                sheDing = new SheDing();
            }
            // ルールの読込
            string guiZeFilePath = Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json");
            if (File.Exists(guiZeFilePath))
            {
                guiZe = JsonUtility.FromJson<GuiZe>(File.ReadAllText(guiZeFilePath));
            }
            else
            {
                guiZe = new GuiZe();
            }
            // データ読込
            LoadData();

            SetGameObject();

            goQiaoShi = new Button[qiaoShiMingQian.Count];
        }

        // Game Object設定
        private void SetGameObject()
        {
            // テキスト
            goSheng = new Button[4];
            goYi = new TextMeshProUGUI[0x10];
            goFanShu = new TextMeshProUGUI[goYi.Length];
            goDianBang = new TextMeshProUGUI[4];
            goFeng = new Image[4];
            goShouQu = new TextMeshProUGUI[4];
            goShouQuGongTuo = new TextMeshProUGUI[4];
            goMingQian = new TextMeshProUGUI[4];
            // スクリーン
            goCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            goScreen = GameObject.Find("Screen").GetComponent<Button>();
            goScreen.onClick.AddListener(() => OnClickScreen());
            goScreen.transform.SetSiblingIndex(10);
            // ボタン
            goYao = new Button[5];
            // 牌
            goPais = new GameObject[0xff];
            goPais[0x00] = GameObject.Find("0x00");
            foreach (int p in pai.qiaoPai)
            {
                goPais[p] = GameObject.Find($"0x{p:x2}");
            }
            foreach (int p in Pai.CHI_PAI_DING_YI)
            {
                int cp = p + QiaoShi.CHI_PAI;
                goPais[cp] = GameObject.Find($"0x{cp:x2}");
            }
            // プレイヤー以外の手牌画像作成
            Sprite sprite = goPais[0x00].GetComponent<SpriteRenderer>().sprite;
            int tHeight = sprite.texture.height / 7;
            Texture2D texture = new(sprite.texture.width, tHeight);
            texture.SetPixels(sprite.texture.GetPixels(0, sprite.texture.height - tHeight, sprite.texture.width, tHeight));
            texture.Apply();
            goJiXieShouPai = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            goLizhiBang = new Image[4];

            // scale設定
            SetScale();

            goText.rectTransform.localScale = Vector3.one * scale.x;
            goFrame.rectTransform.localScale = Vector3.one * scale.x;
            goLine.rectTransform.localScale = Vector3.one * scale.x;
            goDianBang100.rectTransform.localScale = Vector3.one * scale.x;
            goDianBang1000.rectTransform.localScale = Vector3.one * scale.x;
            goSlider.GetComponent<RectTransform>().localScale = Vector3.one * scale.x;
            goSpeech.GetComponent<RectTransform>().localScale = Vector3.one * scale.x;

            // ボタンのスケールとサイズを設定
            RectTransform rtButton = goButton.GetComponent<RectTransform>();
            TextMeshProUGUI buttonText = goButton.GetComponentInChildren<TextMeshProUGUI>();
            rtButton.localScale = Vector3.one * scale.x;
            rtButton.sizeDelta = new Vector2(buttonText.rectTransform.rect.size.x, rtButton.sizeDelta.y);
            foreach (var obj in goQiJias)
            {
                obj.GetComponent<RectTransform>().localScale = Vector3.one * scale.x;
            }
            foreach (var obj in goSais)
            {
                obj.GetComponent<RectTransform>().localScale = Vector3.one * scale.x;
            }

            // デバッグボタン
            DrawDebugOption();

            // 設定画面
            DrawSettingPanel();
            // 得点画面
            DrawScorePanel();
            // データ画面
            DrawDataScrollView();
            // ルール画面
            DrawGuiZeScrollView();
            // カメラボタン
            goCamera = GameObject.Find("Camera").GetComponent<Button>();
            goCamera.transform.SetSiblingIndex(20);
            goCamera.onClick.AddListener(() => StartCoroutine(CaptureAndFlash()));
            RectTransform rtCamera = goCamera.GetComponent<RectTransform>();
            rtCamera.localScale *= scale.x * 1.2f;
            rtCamera.anchorMin = rtCamera.anchorMax = new Vector2(0, 1);
            rtCamera.pivot = new Vector2(0, 1);
            rtCamera.anchoredPosition = new Vector2(paiWidth * 6f, 0);
        }

        // 画面キャプチャー・フラッシュ
        private IEnumerator CaptureAndFlash()
        {
            yield return new WaitForEndOfFrame();

            string directory = GetDownloadFolderPath();
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string fileName = $"screenshot-{timestamp}.png";
            string filePath = Path.Combine(directory, fileName);
            ScreenCapture.CaptureScreenshot(filePath);

            StartCoroutine(FlashCoroutine());
        }

        private string GetDownloadFolderPath()
        {
            string path = "";
#if UNITY_ANDROID
            path = "/storage/emulated/0/Download";
#elif UNITY_IOS
            path = Path.Combine(Application.persistentDataPath, "Downloads");
#elif UNITY_STANDALONE_WIN
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
#elif UNITY_STANDALONE_OSX
            path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
#else
            path = Path.Combine(Application.persistentDataPath, "Screenshot");
#endif
            return path;
        }

        // 画面フラッシュ
        private IEnumerator FlashCoroutine()
        {
            GameObject goFlash = new("FlashImage");
            goFlash.transform.SetParent(goCanvas.transform, false);

            Image img = goFlash.AddComponent<Image>();
            img.color = new Color(1, 1, 1, 0);

            RectTransform rect = img.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            img.color = new Color(1, 1, 1, 1);

            yield return _waitForSeconds0_1;

            float alpha = 1f;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * 5f;
                img.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            Destroy(goFlash);
        }

        // scale設定
        private void SetScale()
        {
            float w = Mathf.Min(Screen.safeArea.height, Screen.safeArea.width);
            RectTransform rtPai = goPai.GetComponent<RectTransform>();
            w = w / 20f / rtPai.rect.width;
            scale = new(w, w, w);
            goPai.transform.localScale = scale;
            paiWidth = rtPai.rect.width * scale.x;
            if (paiWidth * 5f > Math.Abs(Screen.safeArea.width - Screen.safeArea.height) / 2)
            {
                // 縦横の長さの差が牌5枚以内の場合、牌16枚分の大きさに縮小する
                w = paiWidth * 16f / 20f / rtPai.rect.width;
                scale = new(w, w, w);
                goPai.transform.localScale = scale;
                paiWidth = rtPai.rect.width * scale.x;
            }
            paiHeight = rtPai.rect.height * scale.y;

            quiaoShiButtonMaxLen = qiaoShiMingQian.Keys.Max(k => k.Length);
        }

        // 【描画】デバッグボタン
        private void DrawDebugOption()
        {
            // 早送り
            Button goFast = GameObject.Find("Fast").GetComponent<Button>();
            goFast.transform.SetSiblingIndex(20);
            goFast.onClick.AddListener(() =>
            {
                forwardMode = (ForwardMode)(((int)forwardMode + 1) % Enum.GetValues(typeof(ForwardMode)).Length);
                if (forwardMode == ForwardMode.NORMAL)
                {
                    forwardMode = ForwardMode.FAST_FORWARD;
                }
                Application.targetFrameRate = 0;
                waitTime = 0;
                if (chang.keyPress == false)
                {
                    chang.keyPress = true;
                }
                if (chang.eventStatus == Event.FOLLOW_QIAO_SHI_XUAN_ZE)
                {
                    OnClickScreenFollowNone();
                }
            });
            RectTransform rtFast = goFast.GetComponent<RectTransform>();
            rtFast.localScale *= scale.x;
            rtFast.anchorMin = rtFast.anchorMax = new Vector2(0, 1);
            rtFast.pivot = new Vector2(0, 1);
            rtFast.anchoredPosition = new Vector2(paiWidth * 3.2f, -paiHeight * 0.4f);
            // 再生
            Button goReproduction = GameObject.Find("Reproduction").GetComponent<Button>();
            goReproduction.transform.SetSiblingIndex(20);
            goReproduction.onClick.AddListener(() =>
            {
                forwardMode = ForwardMode.NORMAL;
                Application.targetFrameRate = FRAME_RATE;
                waitTime = WAIT_TIME;
            });
            RectTransform rtReproduction = goReproduction.GetComponent<RectTransform>();
            rtReproduction.localScale *= scale.x;
            rtReproduction.anchorMin = rtReproduction.anchorMax = new Vector2(0, 1);
            rtReproduction.pivot = new Vector2(0, 1);
            rtReproduction.anchoredPosition = new Vector2(paiWidth * 0.7f, -paiHeight * 0.4f);
        }

        // 【描画】設定画面
        private void DrawSettingPanel()
        {
            // 設定パネル
            goSettingPanel = GameObject.Find("SettingPanel");
            EventTrigger etSettingPanel = goSettingPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eSettingPanel = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            eSettingPanel.callback.AddListener((eventData) =>
            {
                goSettingPanel.SetActive(false);
            });
            etSettingPanel.triggers.Add(eSettingPanel);
            goSettingPanel.SetActive(false);
            // 設定ボタン
            goSetting = GameObject.Find("Setting").GetComponent<Button>();
            goSetting.transform.SetSiblingIndex(20);
            goSetting.onClick.AddListener(() => goSettingPanel.SetActive(true));
            RectTransform rtSetting = goSetting.GetComponent<RectTransform>();
            rtSetting.localScale *= scale.x;
            rtSetting.anchorMin = rtSetting.anchorMax = new Vector2(1, 1);
            rtSetting.pivot = new Vector2(1, 1);
            rtSetting.anchoredPosition = new Vector2(-(paiWidth * 0.5f), -(paiHeight * 0.2f));

            // リスタート
            Button goRestart = goSettingPanel.transform.Find("Restart").GetComponent<Button>();
            goRestart.onClick.AddListener(() =>
            {
                RestartGame();
            });
            RectTransform rtRestart = goRestart.GetComponent<RectTransform>();
            rtRestart.localScale *= scale.x;
            rtRestart.anchorMin = rtRestart.anchorMax = new Vector2(0, 1);
            rtRestart.pivot = new Vector2(0, 1);
            rtRestart.anchoredPosition = new Vector2(paiWidth, -(rtRestart.sizeDelta.y * scale.x));

            // オプションボタン
            DrawOption();

            goSettingDialogPanel = GameObject.Find("SettingDialogPanel");

            EventTrigger etResetPanel = goSettingDialogPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eResetPanel = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            eResetPanel.callback.AddListener((eventData) => goSettingDialogPanel.SetActive(false));
            etResetPanel.triggers.Add(eResetPanel);

            goSettingDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goSettingDialogPanel.transform);
            DrawText(ref message, "全ての設定をリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goSettingDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(() =>
            {
                ResetSheDing();
                Button[] buttons = goSettingPanel.GetComponentsInChildren<Button>();
                foreach (Button b in buttons)
                {
                    if (b.name == "Fast" || b.name == "Reproduction" || b.name == "Restart" || b.name == "ShouPaiOpen")
                    {
                        continue;
                    }
                    Destroy(b.gameObject);
                }
                goSettingDialogPanel.SetActive(false);
                DrawOption();
            });
            Button goNo = Instantiate(goButton, goSettingDialogPanel.transform);
            goNo.onClick.AddListener(() => goSettingDialogPanel.SetActive(false));
            DrawButton(ref goNo, "いいえ", new Vector2(paiWidth * 3f, 0));
        }

        // ゲームリスタート
        private void RestartGame()
        {
            Application.targetFrameRate = FRAME_RATE;
            waitTime = WAIT_TIME;
            DeleteData();
            SceneManager.LoadScene("GameScene");
        }

        // 【描画】オプションボタン
        private void DrawOption()
        {
            // オプション
            float x = paiWidth * 4.5f;
            float y = paiHeight * 4.5f;
            float offset = paiHeight * 1.6f;
            int len = 7;

            // int型オプション
            (Func<int> get, Action<int> set, string[] labels, Vector2 pos)[] intOptions = {
                (() => sheDing.daPaiFangFa, v => sheDing.daPaiFangFa = v, new[] { "１タップ打牌", "２タップ打牌" }, new Vector2(-x, y)),
            };
            foreach (var (get, set, labels, pos) in intOptions)
            {
                DrawToggleOption(get, set, labels, pos, len);
            }
            // bool型オプション
            (Func<bool> get, Action<bool> set, string[] labels, Vector2 pos)[] boolOptions = {
                (() => sheDing.liZhiAuto, v => sheDing.liZhiAuto = v, new[] { "立直後自動打牌", "立直後手動打牌" }, new Vector2(x, y)),
                (() => sheDing.xuanShangYin, v => sheDing.xuanShangYin = v, new[] { "ドラマーク有り", "ドラマーク無し" }, new Vector2(-x, y - offset)),
                (() => sheDing.ziMoQieBiaoShi, v => sheDing.ziMoQieBiaoShi = v, new[] { "ツモ切表示有り", "ツモ切表示無し" }, new Vector2(x, y - offset)),
                (() => sheDing.daiPaiBiaoShi, v => sheDing.daiPaiBiaoShi = v, new[] { "待牌表示有り", "待牌表示無し" }, new Vector2(-x, y - offset * 2)),
                (() => sheDing.xiangTingShuBiaoShi, v => sheDing.xiangTingShuBiaoShi = v, new[] { "向聴数表示有り", "向聴数表示無し" }, new Vector2(x, y - offset * 2)),
                (() => sheDing.mingQuXiao, v => sheDing.mingQuXiao = v, new[] { "鳴パスはボタン", "鳴パスはタップ" }, new Vector2(-x, y - offset * 3)),
                (() => sheDing.xiangShouPaiOpen, v => sheDing.xiangShouPaiOpen = v, new[] { "相手牌オープン", "相手牌クローズ" }, new Vector2(-x, y - offset * 4)),
                (() => sheDing.shouPaiDianBiaoShi, v => sheDing.shouPaiDianBiaoShi = v, new[] { "手牌点表示有り", "手牌点表示無し" }, new Vector2(x, y - offset * 4)),
                (() => sheDing.learningData, v => sheDing.learningData = v, new[] { "学習データ有り", "学習データ無し" }, new Vector2(-x, y - offset * 5)),
            };
            foreach (var (get, set, labels, pos) in boolOptions)
            {
                DrawToggleOption(get, set, labels, pos, len);
            }
            // リセット
            Button resetButton = Instantiate(goButton, goSettingPanel.transform);
            resetButton.onClick.AddListener(() => goSettingDialogPanel.SetActive(true));
            DrawButton(ref resetButton, "リセット", new Vector2(0, y - offset * 6));
        }

        // オプションボタン描画
        private void DrawToggleOption(Func<int> getValue, Action<int> setValue, string[] textOnOff, Vector2 xy, int len)
        {
            DrawToggleButton(getValue, setValue, v => textOnOff[v], v => (v + 1) % textOnOff.Length, xy, len);
        }
        private void DrawToggleOption(Func<bool> getValue, Action<bool> setValue, string[] textOnOff, Vector2 xy, int len)
        {
            DrawToggleButton(getValue, setValue, v => v ? textOnOff[0] : textOnOff[1], v => !v, xy, len);
        }
        private void DrawToggleButton<T>(Func<T> getValue, Action<T> setValue, Func<T, string> getText, Func<T, T> toggleValue, Vector2 xy, int len)
        {
            Button button = Instantiate(goButton, goSettingPanel.transform);
            DrawButton(ref button, getText(getValue()), xy, len);
            button.onClick.AddListener(() =>
            {
                T newValue = toggleValue(getValue());
                setValue(toggleValue(getValue()));
                button.GetComponentInChildren<TextMeshProUGUI>().text = getText(newValue);
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(Path.Combine(directory, $"{SHE_DING_FILE_NAME}.json"), JsonUtility.ToJson(sheDing));
                switch (chang.eventStatus)
                {
                    case Event.PEI_PAI:
                    case Event.DUI_JU:
                    case Event.DUI_JU_ZHONG_LE:
                        chang.isDuiJuDraw = true;
                        break;
                }
            });
        }

        // 設定オプションのリセット
        private void ResetSheDing()
        {
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            string filePath = Path.Combine(directory, $"{SHE_DING_FILE_NAME}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            sheDing = new SheDing();
            DrawOption();
            switch (chang.eventStatus)
            {
                case Event.PEI_PAI:
                case Event.DUI_JU:
                case Event.DUI_JU_ZHONG_LE:
                    chang.isDuiJuDraw = true;
                    break;
            }
        }

        // 【描画】得点画面
        private void DrawScorePanel()
        {
            // 得点パネル
            goScorePanel = GameObject.Find("ScorePanel");
            EventTrigger etScore = goScorePanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eScore = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            eScore.callback.AddListener((eventData) => goScorePanel.SetActive(false));
            etScore.triggers.Add(eScore);
            goScorePanel.SetActive(false);

            // 得点ボタン
            goScore = GameObject.Find("Score").GetComponent<Button>();
            goScore.transform.SetSiblingIndex(20);
            goScore.onClick.AddListener(() => goScorePanel.SetActive(true));

            RectTransform rtScore = goScore.GetComponent<RectTransform>();
            rtScore.localScale *= scale.x;
            rtScore.anchorMin = rtScore.anchorMax = rtScore.pivot = new Vector2(1, 1);
            rtScore.anchoredPosition = new Vector2(-(paiWidth * 3f), -(paiHeight * 0.2f));

            goScoreQiaoShi = new Button[qiaoShiMingQian.Count + 1];

            float x = 0;
            float y = paiHeight * 5f;

            goScoreQiaoShi[qiaoShiMingQian.Count] = Instantiate(goButton, goScorePanel.transform);
            goScoreQiaoShi[qiaoShiMingQian.Count].onClick.AddListener(() => OnClickScoreQiaoShi(PLAYER_NAME));
            DrawButton(ref goScoreQiaoShi[qiaoShiMingQian.Count], PLAYER_NAME, new Vector2(x, y));
            y -= paiHeight * 1.5f;

            int i = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                x = paiWidth * 4 * (i % 2 == 0 ? -1 : 1);
                int pos = i;
                goScoreQiaoShi[i] = Instantiate(goButton, goScorePanel.transform);
                goScoreQiaoShi[i].onClick.AddListener(() => OnClickScoreQiaoShi(kvp.Key));
                DrawButton(ref goScoreQiaoShi[i], kvp.Key, new Vector2(x, y), quiaoShiButtonMaxLen);
                if (i % 2 == 1 || i == qiaoShiMingQian.Count - 1)
                {
                    y -= paiHeight * 1.5f;
                }
                i++;
            }

            goScoreDialogPanel = GameObject.Find("ScoreDialogPanel");
            Button goScoreReset = Instantiate(goButton, goScorePanel.transform);
            goScoreReset.onClick.AddListener(() => goScoreDialogPanel.SetActive(true));
            DrawButton(ref goScoreReset, "リセット", new Vector2(0, y));

            EventTrigger etResetPanel = goScoreDialogPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eResetPanel = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            eResetPanel.callback.AddListener((eventData) => goScoreDialogPanel.SetActive(false));
            etResetPanel.triggers.Add(eResetPanel);

            goScoreDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goScoreDialogPanel.transform);
            DrawText(ref message, "全員の得点をリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goScoreDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(() =>
            {
                ResetJiLu();
                goScoreDialogPanel.SetActive(false);
            });
            Button goNo = Instantiate(goButton, goScoreDialogPanel.transform);
            goNo.onClick.AddListener(() => goScoreDialogPanel.SetActive(false));
            DrawButton(ref goNo, "いいえ", new Vector2(paiWidth * 3f, 0));
        }

        // 全員の記録リセット
        private void ResetJiLu()
        {
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            string filePath = Path.Combine(directory, $"{PLAYER_NAME}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                filePath = Path.Combine(directory, $"{kvp.Key}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            foreach (QiaoShi shi in qiaoShis)
            {
                Nao2JiLu(GetQiaoShi(shi.mingQian, true));
            }
        }

        // 【描画】データ画面
        private void DrawDataScrollView()
        {
            // データビュー
            goDataScrollView = GameObject.Find("DataScrollView");
            goDataContent = GameObject.Find("DataContent");
            goDataScrollView.SetActive(false);

            goDataBackCanvas = GameObject.Find("DataBackCanvas");
            goDataBackCanvas.SetActive(false);

            // データ画面の上の戻るボタン
            Button goDataBack = Instantiate(goBack, goDataBackCanvas.transform, false);
            goDataBack.onClick.AddListener(() =>
            {
                goDataScrollView.SetActive(false);
                goDataBackCanvas.SetActive(false);
            });
            RectTransform rtBack = goDataBack.GetComponent<RectTransform>();
            rtBack.localScale *= scale.x;
            rtBack.anchorMin = rtBack.anchorMax = rtBack.pivot = new Vector2(0, 1);
            rtBack.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 1.5f));

            goYiMing = new TextMeshProUGUI[QiaoShi.YiMing.Count];
            goYiShu = new TextMeshProUGUI[QiaoShi.YiMing.Count];
            goYiManMing = new TextMeshProUGUI[QiaoShi.YiManMing.Count];
            goYiManShu = new TextMeshProUGUI[QiaoShi.YiManMing.Count];
            float sizeY = paiHeight * (goYiShu.Length + goYiManShu.Length + 27f);
            float y = sizeY / 2 - (paiHeight * 2f);
            goJiJuMingQian = Instantiate(goText, goDataContent.transform);
            DrawText(ref goJiJuMingQian, "", new Vector2(0, y), 0, 25);

            // スライダー項目
            DrawDataSlider(ref goJiLuNaoXuanShang, ref goJiLuNaoXuanShangValue, "懸賞", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoYiPai, ref goJiLuNaoYiPaiValue, "役牌", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoShunZi, ref goJiLuNaoShunZiValue, "順子", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoKeZi, ref goJiLuNaoKeZiValue, "刻子", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoLiZhi, ref goJiLuNaoLiZhiValue, "立直", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoMing, ref goJiLuNaoMingValue, "鳴き", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoRan, ref goJiLuNaoRanValue, "染め", y -= paiHeight);
            DrawDataSlider(ref goJiLuNaoTao, ref goJiLuNaoTaoValue, "逃げ", y -= paiHeight);
            y -= paiHeight;
            DrawData(ref goJiLuJiJiDian, "集計点", y -= paiHeight);
            DrawData(ref goJiLuBanZhuangShu, "半荘数", y -= paiHeight);
            DrawData(ref goJiLuDuiJuShu, "対局数", y -= paiHeight);
            DrawData(ref goJiLuShunWei1Shuai, "１位", y -= paiHeight);
            DrawData(ref goJiLuShunWei2Shuai, "２位", y -= paiHeight);
            DrawData(ref goJiLuShunWei3Shuai, "３位", y -= paiHeight);
            DrawData(ref goJiLuShunWei4Shuai, "４位", y -= paiHeight);
            DrawData(ref goJiLuHeLeShuai, "和了率", y -= paiHeight);
            DrawData(ref goJiLuFangChongShuai, "放銃率", y -= paiHeight);
            DrawData(ref goJiLuTingPaiShuai, "聴牌率", y -= paiHeight);
            DrawData(ref goJiLuPingJunHeLeDian, "平均和了点", y -= paiHeight);
            DrawData(ref goJiLuPingJunFangChongDian, "平均放銃点", y -= paiHeight);
            y -= paiHeight;
            int index = 0;
            foreach (KeyValuePair<QiaoShi.YiDingYi, string> kvp in QiaoShi.YiMing)
            {
                DrawData(ref goYiMing[index], ref goYiShu[index], kvp.Value, y -= paiHeight);
                index++;
            }
            y -= paiHeight;
            index = 0;
            foreach (KeyValuePair<QiaoShi.YiManDingYi, string> kvp in QiaoShi.YiManMing)
            {
                DrawData(ref goYiManMing[index], ref goYiManShu[index], kvp.Value, y -= paiHeight);
                index++;
            }

            RectTransform rtDataContent = goDataContent.GetComponent<RectTransform>();
            Vector2 size = rtDataContent.sizeDelta;
            size.y = sizeY;
            rtDataContent.sizeDelta = size;
            ScrollRect scrollRect = goDataScrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        // 【描画】データ
        private void DrawData(ref TextMeshProUGUI goShu, string ming, float y)
        {
            TextMeshProUGUI text = Instantiate(goText, goDataContent.transform);
            DrawData(ref text, ref goShu, ming, y);
        }
        private void DrawData(ref TextMeshProUGUI goMing, ref TextMeshProUGUI goShu, string ming, float y)
        {
            if (goMing == null)
            {
                goMing = Instantiate(goText, goDataContent.transform);
            }
            DrawText(ref goMing, ming, new Vector2(-paiWidth * 2f, y), 0, 25, TextAlignmentOptions.Left, 7);

            goShu = Instantiate(goText, goDataContent.transform);
            DrawText(ref goShu, "", new Vector2(paiWidth * 5.5f, y), 0, 25, TextAlignmentOptions.Right, 0);
        }
        private void DrawDataSlider(ref Slider goShu, ref TextMeshProUGUI goValue, string ming, float y)
        {
            TextMeshProUGUI text = Instantiate(goText, goDataContent.transform);
            DrawDataSlider(ref text, ref goShu, ref goValue, ming, y);
        }
        private void DrawDataSlider(ref TextMeshProUGUI goMing, ref Slider goShu, ref TextMeshProUGUI goValue, string ming, float y)
        {
            if (goMing == null)
            {
                goMing = Instantiate(goText, goDataContent.transform);
            }
            DrawText(ref goMing, ming, new Vector2(-paiWidth * 2f, y), 0, 25, TextAlignmentOptions.Left, 7);

            goShu = Instantiate(goSlider, goDataContent.transform);
            RectTransform rt = goShu.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(paiWidth * 1.5f, y);

            if (goValue == null)
            {
                goValue = Instantiate(goText, goDataContent.transform);
            }
            DrawText(ref goValue, goShu.value.ToString(), new Vector2(paiWidth * 5.5f, y), 0, 25, TextAlignmentOptions.Right, 3);
        }

        private void ResizeYiManShu(QiaoShi shi)
        {
            if (shi.jiLu.yiManShu.Length != QiaoShi.YiManMing.Count)
            {
                Array.Resize(ref shi.jiLu.yiManShu, QiaoShi.YiManMing.Count);
            }
        }
        private void ResizeYiShu(QiaoShi shi)
        {
            if (shi.jiLu.yiShu.Length != QiaoShi.YiMing.Count)
            {
                Array.Resize(ref shi.jiLu.yiShu, QiaoShi.YiMing.Count);
            }
        }

        // 得点パネル 雀士名クリック
        private void OnClickScoreQiaoShi(string mingQian)
        {
            QiaoShi shi = GetQiaoShi(mingQian, false);
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filePath = Path.Combine(directory, $"{mingQian}.json");
            if (File.Exists(filePath))
            {
                shi.jiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(filePath));
                // ファイル書き込み後にプログラムでローカル役を追加した場合は、配列サイズを変更する
                ResizeYiManShu(shi);
                ResizeYiShu(shi);
            }
            else
            {
                Nao2JiLu(shi);
            }

            goJiJuMingQian.text = mingQian;

            (Slider slider, TextMeshProUGUI valueText, Action<JiLu, int> setValue, Func<JiLu, int> getValue)[] binds = {
                (goJiLuNaoXuanShang, goJiLuNaoXuanShangValue, (j, v) => j.naoXuanShang = v, j => j.naoXuanShang),
                (goJiLuNaoYiPai, goJiLuNaoYiPaiValue, (j, v) => j.naoYiPai = v, j => j.naoYiPai),
                (goJiLuNaoShunZi, goJiLuNaoShunZiValue, (j, v) => j.naoShunZi = v, j => j.naoShunZi),
                (goJiLuNaoKeZi, goJiLuNaoKeZiValue, (j, v) => j.naoKeZi = v, j => j.naoKeZi),
                (goJiLuNaoLiZhi, goJiLuNaoLiZhiValue, (j, v) => j.naoLiZhi = v, j => j.naoLiZhi),
                (goJiLuNaoMing, goJiLuNaoMingValue, (j, v) => j.naoMing = v, j => j.naoMing),
                (goJiLuNaoRan, goJiLuNaoRanValue, (j, v) => j.naoRan = v, j => j.naoRan),
                (goJiLuNaoTao, goJiLuNaoTaoValue, (j, v) => j.naoTao = v, j => j.naoTao),
            };
            foreach (var (slider, valueText, setValue, getValue) in binds)
            {
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(value =>
                {
                    valueText.text = ((int)value).ToString();
                    setValue(shi.jiLu, (int)value);
                    WriteJiLu(shi.mingQian, shi.jiLu);
                });
                slider.value = getValue(shi.jiLu);
            }

            goJiLuJiJiDian.text = $"{shi.jiLu.jiJiDian}点";
            goJiLuBanZhuangShu.text = $"{shi.jiLu.banZhuangShu}回";
            goJiLuDuiJuShu.text = $"{shi.jiLu.duiJuShu}回";
            goJiLuShunWei1Shuai.text = $"{(shi.jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.shunWei1 / shi.jiLu.banZhuangShu * 100))}％";
            goJiLuShunWei2Shuai.text = $"{(shi.jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.shunWei2 / shi.jiLu.banZhuangShu * 100))}％";
            goJiLuShunWei3Shuai.text = $"{(shi.jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.shunWei3 / shi.jiLu.banZhuangShu * 100))}％";
            goJiLuShunWei4Shuai.text = $"{(shi.jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.shunWei4 / shi.jiLu.banZhuangShu * 100))}％";
            goJiLuHeLeShuai.text = $"{(shi.jiLu.duiJuShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.heLeShu / shi.jiLu.duiJuShu * 100))}％";
            goJiLuFangChongShuai.text = $"{(shi.jiLu.duiJuShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.fangChongShu / shi.jiLu.duiJuShu * 100))}％";
            goJiLuTingPaiShuai.text = $"{(shi.jiLu.liuJuShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.tingPaiShu / shi.jiLu.liuJuShu * 100))}％";
            goJiLuPingJunHeLeDian.text = $"{(shi.jiLu.heLeShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.heLeDian / shi.jiLu.heLeShu))}点";
            goJiLuPingJunFangChongDian.text = $"{(shi.jiLu.fangChongShu == 0 ? "" : (int)Math.Floor((double)shi.jiLu.fangChongDian / shi.jiLu.fangChongShu))}点";
            for (int i = 0; i < goYiShu.Length; i++)
            {
                goYiShu[i].text = $"{shi.jiLu.yiShu[i]}回";
                goYiMing[i].color = goYiShu[i].color = shi.jiLu.yiShu[i] == 0 ? Color.gray : Color.black;
            }
            for (int i = 0; i < goYiManShu.Length; i++)
            {
                goYiManShu[i].text = $"{shi.jiLu.yiManShu[i]}回";
                goYiManMing[i].color = goYiManShu[i].color = shi.jiLu.yiManShu[i] == 0 ? Color.gray : Color.black;
            }

            goDataScrollView.SetActive(true);
            goDataBackCanvas.SetActive(true);
        }

        private void WriteJiLu(string ming, JiLu jiLu)
        {
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(Path.Combine(directory, $"{ming}.json"), JsonUtility.ToJson(jiLu));

            foreach (QiaoShi shi in qiaoShis)
            {
                if (shi.mingQian == ming)
                {
                    JiLu2Nao(shi);
                }
            }
        }

        // 【描画】ルール画面
        private void DrawGuiZeScrollView()
        {
            // ルールビュー
            goGuiZeScrollView = GameObject.Find("GuiZeScrollView");
            goGuiZeContent = GameObject.Find("GuiZeContent");
            goGuiZeScrollView.SetActive(false);

            // ルールボタン
            goGuiZe = GameObject.Find("GuiZe").GetComponent<Button>();
            goGuiZe.transform.SetSiblingIndex(20);
            goGuiZe.onClick.AddListener(() => goGuiZeScrollView.SetActive(true));

            RectTransform rtGuiZe = goGuiZe.GetComponent<RectTransform>();
            rtGuiZe.localScale *= scale.x;
            rtGuiZe.anchorMax = rtGuiZe.anchorMin = rtGuiZe.pivot = new Vector2(1, 1);
            rtGuiZe.anchoredPosition = new Vector2(-(paiWidth * 5.5f), -(paiHeight * 0.2f));

            goGuiZeBackCanvas = GameObject.Find("GuiZeBackCanvas");
            goGuiZeBackCanvas.SetActive(false);

            // ルール画面の上の戻るボタン
            Button goGuiZeBack = Instantiate(goBack, goGuiZeScrollView.transform);
            goGuiZeBack.onClick.AddListener(() =>
            {
                goGuiZeScrollView.SetActive(false);
                goGuiZeBackCanvas.SetActive(false);
            });
            RectTransform rtBack = goGuiZeBack.GetComponent<RectTransform>();
            rtBack.localScale *= scale.x;
            rtBack.anchorMin = rtBack.anchorMax = rtBack.pivot = new Vector2(0, 1);
            rtBack.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 1.5f));

            DrawGuiZe();

            goGuiZeDialogPanel = GameObject.Find("GuiZeDialogPanel");

            EventTrigger etResetPanel = goGuiZeDialogPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eResetPanel = new()
            {
                eventID = EventTriggerType.PointerClick
            };
            eResetPanel.callback.AddListener((eventData) => goGuiZeDialogPanel.SetActive(false));
            etResetPanel.triggers.Add(eResetPanel);

            goGuiZeDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goGuiZeDialogPanel.transform);
            DrawText(ref message, "全てのルールをリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goGuiZeDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(() =>
            {
                ResetGuiZe();
                Button[] buttons = goGuiZeContent.GetComponentsInChildren<Button>();
                foreach (Button btn in buttons)
                {
                    Destroy(btn.gameObject);
                }
                goGuiZeDialogPanel.SetActive(false);
                DrawGuiZe();
            });
            Button goNo = Instantiate(goButton, goGuiZeDialogPanel.transform);
            goNo.onClick.AddListener(() => goGuiZeDialogPanel.SetActive(false));
            DrawButton(ref goNo, "いいえ", new Vector2(paiWidth * 3f, 0));

            RectTransform rtGuiZeContent = goGuiZeContent.GetComponent<RectTransform>();
            Vector2 size = rtGuiZeContent.sizeDelta;
            size.y = paiHeight * 32f;
            rtGuiZeContent.sizeDelta = size;
            ScrollRect scrollRect = goGuiZeScrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        // ルール描画
        private void DrawGuiZe()
        {
            float y = paiHeight * 14.5f;
            float x = 0;
            float offset = paiHeight * 1.3f;

            TextMeshProUGUI title = Instantiate(goText, goGuiZeContent.transform);
            DrawText(ref title, "ルール", new Vector2(0, y), 0, 25);
            DrawToggleGuiZe(() => guiZe.banZhuang, v => guiZe.banZhuang = v, new string[] { "半荘戦", "東風戦" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.ziMoPingHe, v => guiZe.ziMoPingHe = v, new string[] { "ピンヅモ有り", "ピンヅモ無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.shiDuan, v => guiZe.shiDuan = v, new string[] { "食いタン有り", "食いタン無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.shiTi, v => guiZe.shiTi = v, new string[] { "食い替え有り", "食い替え無し（チョンボ扱い）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.wRongHe, v => guiZe.wRongHe = v, new string[] { "ダブルロン無し（頭ハネ）", "ダブルロン有り" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.tRongHe, v => guiZe.tRongHe = v, new string[] { "トリプルロン無し（頭ハネ）", "トリプルロン有り", "トリプルロンは流局（親連荘）", "トリプルロンは流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.baoZe, v => guiZe.baoZe = v, new string[] { "パオ（責任払い）有り", "パオ（責任払い）無し" }, new Vector2(x, y -= offset));
            string[] chiPaiShuText = new string[] { "赤ドラ無し", "赤ドラ有り（各１枚）" };
            Button buttonChiPaiShu = Instantiate(goButton, goGuiZeContent.transform);
            DrawButton(ref buttonChiPaiShu, guiZe.chiPaiShu[0] == 0 ? chiPaiShuText[0] : chiPaiShuText[1], new Vector2(x, y -= offset), 12);
            TextMeshProUGUI text = buttonChiPaiShu.GetComponentInChildren<TextMeshProUGUI>();
            text.fontSize = 17f;
            buttonChiPaiShu.onClick.AddListener(() =>
            {
                guiZe.chiPaiShu = guiZe.chiPaiShu[0] == 0 ? new int[] { 1, 1, 1 } : new int[] { 0, 0, 0 };
                text.text = guiZe.chiPaiShu[0] == 0 ? chiPaiShuText[0] : chiPaiShuText[1];
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json"), JsonUtility.ToJson(guiZe));
            });
            DrawToggleGuiZe(() => guiZe.jiuZhongJiuPaiLianZhuang, v => guiZe.jiuZhongJiuPaiLianZhuang = v, new string[] { "九種九牌無し", "九種九牌は流局（親連荘）", "九種九牌は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.siJiaLiZhiLianZhuang, v => guiZe.siJiaLiZhiLianZhuang = v, new string[] { "四家立直は続行", "四家立直は流局（親連荘）", "四家立直は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.siFengZiLianDaLianZhuang, v => guiZe.siFengZiLianDaLianZhuang = v, new string[] { "四風子連打無し", "四風子連打は流局（親連荘）", "四風子連打は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.siKaiGangLianZhuang, v => guiZe.siKaiGangLianZhuang = v, new string[] { "四開槓は流局（親連荘）", "四開槓は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.xiang, v => guiZe.xiang = v, new string[] { "箱（０点以下で終了）有り", "箱（０点以下で終了）無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.jieJinLiZhi, v => guiZe.jieJinLiZhi = v, new string[] { "１０００点未満のリーチ可能", "１０００点未満のリーチ不可" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.liuManGuan, v => guiZe.liuManGuan = v, new string[] { "流し満貫有り", "流し満貫無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.sanLianKe, v => guiZe.sanLianKe = v, new string[] { "三連刻有り", "三連刻無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.yanFan, v => guiZe.yanFan = v, new string[] { "燕返し有り", "燕返し無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.kaiLiZhi, v => guiZe.kaiLiZhi = v, new string[] { "オープンリーチ有り", "オープンリーチ無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.shiSanBuTa, v => guiZe.shiSanBuTa = v, new string[] { "十三不塔有り", "十三不塔無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.baLianZhuang, v => guiZe.baLianZhuang = v, new string[] { "八連荘有り", "八連荘無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => guiZe.localYiMan, v => guiZe.localYiMan = v, new string[] { "ローカル役満有り", "ローカル役満無し" }, new Vector2(x, y -= offset));

            Button resetButton = Instantiate(goButton, goGuiZeContent.transform);
            resetButton.onClick.AddListener(() =>
            {
                goGuiZeDialogPanel.SetActive(true);
            });
            DrawButton(ref resetButton, "リセット", new Vector2(0, y - paiHeight * 1.5f));
        }

        // トグルボタン描画
        private void DrawToggleGuiZe(Func<bool> getValue, Action<bool> setValue, string[] textOnOff, Vector2 xy)
        {
            Button button = Instantiate(goButton, goGuiZeContent.transform);

            string displayText = getValue() ? textOnOff[0] : textOnOff[1];
            DrawButton(ref button, displayText, xy, 12);
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.fontSize = 17f;
            button.onClick.AddListener(() =>
            {
                bool newValue = !getValue();
                setValue(newValue);
                text.text = newValue ? textOnOff[0] : textOnOff[1];
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json"), JsonUtility.ToJson(guiZe));
            });
        }
        private void DrawToggleGuiZe(Func<int> getValue, Action<int> setValue, string[] textOnOff, Vector2 xy)
        {
            Button button = Instantiate(goButton, goGuiZeContent.transform);

            string displayText = textOnOff[getValue()];
            DrawButton(ref button, displayText, xy, 12);
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.fontSize = 17f;
            button.onClick.AddListener(() =>
            {
                int newValue = (getValue() + 1) % textOnOff.Length;
                setValue(newValue);
                text.text = textOnOff[newValue];
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json"), JsonUtility.ToJson(guiZe));
            });
        }

        // ルールリセット
        private void ResetGuiZe()
        {
            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            string filePath = Path.Combine(directory, $"{GUI_ZE_FILE_NAME}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            guiZe = new GuiZe();
        }

        void Update()
        {
            if (orientation != Screen.orientation && !(orientation == ScreenOrientation.Portrait && Screen.orientation == ScreenOrientation.PortraitUpsideDown))
            {
                // 画面回転
                orientation = Screen.orientation;
                if (orientation == ScreenOrientation.PortraitUpsideDown)
                {
                    orientation = ScreenOrientation.Portrait;
                }
                // scale設定
                SetScale();
                switch (chang.eventStatus)
                {
                    // 対局
                    case Event.DUI_JU:
                        chang.isDuiJuDraw = true;
                        break;
                    // 対局終了
                    case Event.DUI_JU_ZHONG_LE:
                        chang.isDuiJuZhongLeDraw = true;
                        break;
                    // 役表示
                    case Event.YI_BIAO_SHI:
                        chang.isYiBiaoShiDraw = true;
                        break;
                }
            }

            switch (chang.eventStatus)
            {
                // 開始
                case Event.KAI_SHI:
                    kaiShiCoroutine ??= StartCoroutine(KaiShi());
                    break;
                // 雀士選択
                case Event.QIAO_SHI_XUAN_ZE:
                    qiaoShiXuanZeCoroutine ??= StartCoroutine(QiaoShiXuanZe());
                    break;
                // フォロー雀士選択
                case Event.FOLLOW_QIAO_SHI_XUAN_ZE:
                    followQiaoShiXuanZeCoroutine ??= StartCoroutine(FollowQiaoShiXuanZe());
                    break;
                // 場決
                case Event.CHANG_JUE:
                    ChangJue();
                    break;
                // 親決
                case Event.QIN_JUE:
                    qinJueCoroutine ??= StartCoroutine(QinJue());
                    break;
                // 荘初期化
                case Event.ZHUANG_CHU_QI_HUA:
                    ZhuangChuQiHua();
                    break;
                // 配牌
                case Event.PEI_PAI:
                    peiPaiCoroutine ??= StartCoroutine(PeiPai());
                    break;
                // 対局
                case Event.DUI_JU:
                    duiJuCoroutine ??= StartCoroutine(DuiJu());
                    break;
                // 対局終了
                case Event.DUI_JU_ZHONG_LE:
                    duiJuZhongLeCoroutine ??= StartCoroutine(DuiJuZhongLe());
                    break;
                // 役表示
                case Event.YI_BIAO_SHI:
                    yiBiaoShiCoroutine ??= StartCoroutine(YiBiaoShi());
                    break;
                // 点表示
                case Event.DIAN_BIAO_SHI:
                    dianBiaoShiCoroutine ??= StartCoroutine(DianBiaoShi());
                    break;
                // 荘終了
                case Event.ZHUANG_ZHONG_LE:
                    zhuangZhongLeCoroutine ??= StartCoroutine(ZhuangZhong());
                    break;
            }

            // 描画
            if (chang.isKaiShiDraw)
            {
                DrawKaiShi();
                chang.isKaiShiDraw = false;
            }
            // 雀士選択
            if (chang.isQiaoShiXuanZeDraw)
            {
                DrawQiaoShiXuanZe();
                chang.isQiaoShiXuanZeDraw = false;
            }
            // フォロー雀士選択
            if (chang.isFollowQiaoShiXuanZeDraw)
            {
                DrawFollowQiaoShiXuanZe();
                chang.isFollowQiaoShiXuanZeDraw = false;
            }
            // 親決
            if (chang.isQinJueDraw)
            {
                DrawQinJue();
                chang.isQinJueDraw = false;
            }
            // 配牌
            if (chang.isPeiPaiDraw)
            {
                DrawPeiPai();
                chang.isPeiPaiDraw = false;
            }
            // 対局
            if (chang.isDuiJuDraw)
            {
                DrawDuiJu();
                chang.isDuiJuDraw = false;
            }
            // 対局終了
            if (chang.isDuiJuZhongLeDraw)
            {
                DrawDuiJuZhongLe();
                chang.isDuiJuZhongLeDraw = false;
            }
            // 役表示
            if (chang.isYiBiaoShiDraw)
            {
                DrawYiBiaoShi();
                chang.isYiBiaoShiDraw = false;
            }
            // 点表示
            if (chang.isDianBiaoShiDraw)
            {
                DrawDianBiaoShi();
                chang.isDianBiaoShiDraw = false;
            }
            // 荘終了
            if (chang.isZhuangZhongLeDraw)
            {
                DrawZhuangZhongLe();
                chang.isZhuangZhongLeDraw = false;
            }
            // 点差
            if (chang.isDianChaDraw)
            {
                DrawJuFrame();
                chang.isDianChaDraw = chang.isDianCha;
            }
        }

        // スクリーンクリック
        private void OnClickScreen()
        {
            switch (chang.eventStatus)
            {
                // 開始
                case Event.KAI_SHI:
                // 場決
                case Event.CHANG_JUE:
                // 親決
                case Event.QIN_JUE:
                // 対局終了
                case Event.DUI_JU_ZHONG_LE:
                // 役表示
                case Event.YI_BIAO_SHI:
                // 点表示
                case Event.DIAN_BIAO_SHI:
                // 荘終了
                case Event.ZHUANG_ZHONG_LE:
                    chang.keyPress = true;
                    break;

                // 雀士選択
                case Event.QIAO_SHI_XUAN_ZE:
                    OnClickScreenQiaoShiXuanZe();
                    break;

                // フォロー雀士選択
                case Event.FOLLOW_QIAO_SHI_XUAN_ZE:
                    OnClickScreenFollowNone();
                    break;

                // 対局
                case Event.DUI_JU:
                    if (chang.tingPaiLianZhuang != Zhuang.XU_HANG)
                    {
                        chang.keyPress = true;
                    }
                    if (!sheDing.mingQuXiao)
                    {
                        for (int i = 0; i < qiaoShis.Count; i++)
                        {
                            QiaoShi shi = qiaoShis[i];
                            if (shi.player && !shi.jiJia)
                            {
                                OnClickTaJiaYao(i, shi, QiaoShi.YaoDingYi.Wu, 0);
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        // 雀士選択処理
        private void OnClickScreenQiaoShiXuanZe()
        {
            int selectedCount = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                if (kvp.Value)
                {
                    selectedCount++;
                }
            }
            if ((!chang.existsPlayer && selectedCount == 4) || selectedCount == 3 || selectedCount == 2 || (chang.existsPlayer && selectedCount == 1))
            {
                chang.keyPress = true;
            }
        }

        // フォロー無しクリック
        private void OnClickScreenFollowNone()
        {
            qiaoShis.Insert(0, new QiaoJiXie(PLAYER_NAME));
            qiaoShis[0].follow = false;
            qiaoShis[0].player = true;
            chang.keyPress = true;
        }

        // 一時停止
        private IEnumerator Pause(ForwardMode mode)
        {
            chang.keyPress = false;
            if (forwardMode > mode)
            {
                chang.keyPress = true;
            }
            while (!chang.keyPress)
            {
                yield return null;
            }
            chang.keyPress = false;
        }

        // 【ゲーム】開始
        private IEnumerator KaiShi()
        {
            chang.isKaiShiDraw = true;

            int fadingOut = 1;
            float alpha = 1f;
            chang.keyPress = false;
            while (!chang.keyPress)
            {
                alpha += Time.deltaTime * 1.1f * fadingOut;
                if (alpha <= 0)
                {
                    alpha = 0;
                    fadingOut *= -1;
                }
                if (alpha >= 1)
                {
                    alpha = 1;
                    fadingOut *= -1;
                }
                if (goStart != null)
                {
                    goStart.canvasRenderer.SetAlpha(alpha);
                }
                yield return null;
            }
            chang.keyPress = false;

            chang.eventStatus = Event.QIAO_SHI_XUAN_ZE;
            kaiShiCoroutine = null;
            WriteData();
        }

        // 【描画】開始
        private void DrawKaiShi()
        {
            DrawText(ref goTitle, "麻 雀 部", new Vector2(0, paiHeight * 2), 0, 60);
            DrawText(ref goStart, "タップ スタート!", new Vector2(0, -(paiHeight * 2)), 0, 30);
        }

        // 【ゲーム】雀士選択
        private IEnumerator QiaoShiXuanZe()
        {
            qiaoShis = new List<QiaoShi>();
            if (forwardMode > ForwardMode.FAST_FORWARD)
            {
                // ランダム自動選択
                RandomQiaoShiXuanZe();
            }
            else
            {
                chang.isQiaoShiXuanZeDraw = true;
            }
            yield return Pause(ForwardMode.FAST_FORWARD);

            int selectedCount = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                if (kvp.Value)
                {
                    selectedCount++;
                }
            }
            if (selectedCount == 0)
            {
                RandomQiaoShiXuanZe();
                OnClickScreenQiaoShiXuanZe();
            }

            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                if (kvp.Value)
                {
                    qiaoShis.Add(GetQiaoShi(kvp.Key, true));
                }
            }

            chang.eventStatus = chang.existsPlayer ? Event.FOLLOW_QIAO_SHI_XUAN_ZE : Event.CHANG_JUE;
            qiaoShiXuanZeCoroutine = null;
            WriteData();
        }

        // 【ゲーム】フォロー雀士選択
        private IEnumerator FollowQiaoShiXuanZe()
        {
            chang.isFollowQiaoShiXuanZeDraw = true;
            chang.keyPress = false;
            if (forwardMode > ForwardMode.NORMAL)
            {
                chang.keyPress = true;
                OnClickScreenFollowNone();
            }
            while (!chang.keyPress) { yield return null; }
            chang.keyPress = false;

            chang.eventStatus = Event.CHANG_JUE;
            followQiaoShiXuanZeCoroutine = null;
            WriteData();
        }

        // 【描画】局、残牌、供託、点、懸賞牌
        private void DrawJuFrame()
        {
            DrawJu();
            DrawGongTuo();
            if (chang.eventStatus != Event.DIAN_BIAO_SHI)
            {
                DrawCanShanPaiShu();
                DrawXuanShangPai(false);
            }
            DrawDianBang();
        }

        // 【描画】局
        private void DrawJu()
        {
            // 枠
            ClearGameObject(ref goJuFrame);
            goJuFrame = Instantiate(goFrame, goCanvas.transform);
            RectTransform rt = goJuFrame.rectTransform;
            rt.anchoredPosition = new Vector2(0, 0);
            rt.localScale = scale;
            rt.localScale *= 5.9f;
            rt.SetSiblingIndex(0);
            goJuFrame.color = new Color(0f, 0.8f, 0f, 0.2f);
            Outline outline = goJuFrame.GetComponent<Outline>();
            outline.effectDistance = new Vector2(0.2f, 0.2f);
            outline.effectColor = new Color(1, 1, 1, 0.5f);
            TextMeshProUGUI text = goJuFrame.GetComponentInChildren<TextMeshProUGUI>();
            Destroy(text);

            // 局
            ClearGameObject(ref goJu);
            goJu = Instantiate(goText, goJuFrame.transform.parent);
            float x = paiWidth * -0.8f;
            float y = paiHeight * 0.9f;
            string value = $"{Pai.FENG_PAI_MING[chang.changFeng - 0x31]}{chang.ju + 1}局";
            DrawText(ref goJu, value, new Vector2(x, y), 0, 17);
            goJu.rectTransform.SetSiblingIndex(1);
        }

        // 【描画】供託
        private void DrawGongTuo()
        {
            float x = paiWidth * 0.7f;
            float y = paiHeight * 1.0f;
            ClearGameObject(ref goBenChang);
            goBenChang = Instantiate(goDianBang100, goCanvas.transform);
            RectTransform rt100 = goBenChang.GetComponent<RectTransform>();
            rt100.sizeDelta = new Vector2(rt100.sizeDelta.x * 0.4f, rt100.sizeDelta.y);
            rt100.anchoredPosition = new Vector2(x, y);
            ClearGameObject(ref goBenChangText);
            goBenChangText = Instantiate(goText, goJuFrame.transform.parent);
            string valueBenChang = $"x{chang.benChang}";
            DrawText(ref goBenChangText, valueBenChang, new Vector2(x + paiWidth * 0.9f, y + paiHeight * 0.05f), 0, 12);

            y -= paiHeight * 0.3f;
            ClearGameObject(ref goGongTou);
            goGongTou = Instantiate(goDianBang1000, goCanvas.transform);
            RectTransform rt1000 = goGongTou.GetComponent<RectTransform>();
            rt1000.sizeDelta = new Vector2(rt1000.sizeDelta.x * 0.4f, rt1000.sizeDelta.y);
            rt1000.anchoredPosition = new Vector2(x, y);
            ClearGameObject(ref goGongTouText);
            goGongTouText = Instantiate(goText, goJuFrame.transform.parent);
            string valueGongTou = $"x{chang.gongTuo / 1000}";
            DrawText(ref goGongTouText, valueGongTou, new Vector2(x + paiWidth * 0.9f, y + paiHeight * 0.05f), 0, 12);
        }

        /**
         * 【描画】残山牌数
         */
        private void DrawCanShanPaiShu()
        {
            ClearGameObject(ref goCanShanPaiShu);
            goCanShanPaiShu = Instantiate(goFrame, goJuFrame.transform.parent);
            float alfa = pai.CanShanPaiShu() < 100 ? 1f : 0f;
            DrawFrame(ref goCanShanPaiShu, pai.CanShanPaiShu().ToString(), new Vector2(0, paiHeight * 0.2f), 0, 17, new Color(0, 0.6f, 0), new Color(1f, 1f, 1f, alfa), 3);
        }

        // 【描画】点数
        private void DrawDianBang()
        {
            int dianPlayer = 0;
            if (chang.isDianCha)
            {
                for (int i = 0; i < qiaoShis.Count; i++)
                {
                    int jia = (chang.qin + i) % qiaoShis.Count;
                    QiaoShi shi = qiaoShis[jia];
                    if (shi.player)
                    {
                        dianPlayer = shi.dianBang;
                        break;
                    }
                }
            }

            float x = 0f;
            float y = -(paiWidth * 2.5f);
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                Color background = shi.feng == 0x31 ? new Color(1f, 0.5f, 0.5f) : Color.black;
                ClearGameObject(ref goFeng[i]);
                goFeng[i] = Instantiate(goFrame, goJuFrame.transform.parent);
                if ((chang.eventStatus == Event.DUI_JU || chang.eventStatus == Event.DUI_JU_ZHONG_LE) && jia == chang.ziMoFan)
                {
                    ClearGameObject(ref goZiMoShiLine);
                    goZiMoShiLine = Instantiate(goLine, goJuFrame.transform.parent);
                    RectTransform rt = goZiMoShiLine.rectTransform;
                    rt.anchoredPosition = Cal(0, -(paiHeight * 2f), shi.playOrder);
                    rt.rotation = Quaternion.Euler(0, 0, 90 * GetDrawOrder(shi.playOrder));
                }
                DrawFrame(ref goFeng[i], Pai.FENG_PAI_MING[shi.feng - 0x31], Cal(x - paiWidth * 2f, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 16, background, Color.white);
                if (chang.eventStatus == Event.DIAN_BIAO_SHI)
                {
                    continue;
                }
                ClearGameObject(ref goDianBang[i]);
                goDianBang[i] = Instantiate(goText, goJuFrame.transform.parent);
                string value;
                if (chang.isDianCha && !shi.player)
                {
                    int dianCha = dianPlayer - shi.dianBang;
                    value = dianCha.ToString();
                    goDianBang[i].color = dianCha >= 0 ? Color.blue : Color.red;
                }
                else
                {
                    value = shi.dianBang.ToString();
                    goDianBang[i].color = Color.black;
                }
                DrawText(ref goDianBang[i], value, Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 16);

                if (shi.liZhi)
                {
                    ClearGameObject(ref goLizhiBang[jia]);
                    goLizhiBang[jia] = Instantiate(goDianBang1000, goJuFrame.transform.parent);
                    goLizhiBang[jia].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    RectTransform rt = goLizhiBang[jia].GetComponent<RectTransform>();
                    rt.SetSiblingIndex(2);
                    rt.anchoredPosition = Cal(x, y + paiHeight * 0.3f, shi.playOrder);
                }
            }
        }

        // 【描画】懸賞牌
        private void DrawXuanShangPai(bool isYiBiaoShi)
        {
            ClearGameObject(ref pai.goXuanShangPai);
            ClearGameObject(ref pai.goLiXuanShangPai);

            float xuanShangScalse = 0.7f;
            float X = -(paiWidth * 1.4f);
            float Y = -(paiHeight * 0.6f);
            if (isYiBiaoShi)
            {
                X = -(paiWidth * 2f);
                Y = paiHeight * 8f;
                if (orientation != ScreenOrientation.Portrait)
                {
                    X = -(paiWidth * 15f);
                    Y = paiHeight * 2f;
                }
                xuanShangScalse = 1.0f;
            }
            float offsetY = paiHeight * xuanShangScalse * 0.5f;

            bool isLiXuanShangPai = false;
            if (chang.heleFan >= 0 && qiaoShis[chang.heleFan].liZhi)
            {
                isLiXuanShangPai = true;
            }
            foreach (RongHeFan rongHeFan in chang.rongHeFans)
            {
                QiaoShi shi = qiaoShis[rongHeFan.fan];
                if (shi.liZhi)
                {
                    isLiXuanShangPai = true;
                    break;
                }
            }

            float x = X;
            float y = Y;
            for (int i = 0; i <= 4; i++)
            {
                pai.goXuanShangPai[i] = Instantiate(goPai, goCanvas.transform);
                pai.goXuanShangPai[i].transform.SetSiblingIndex(3);
                RectTransform rt = pai.goXuanShangPai[i].GetComponent<RectTransform>();
                rt.localScale *= xuanShangScalse;
                if (i < pai.xuanShangPai.Count)
                {
                    DrawPai(ref pai.goXuanShangPai[i], pai.xuanShangPai[i], new Vector2(x, isLiXuanShangPai ? y + offsetY : y), 0);
                }
                else
                {
                    DrawPai(ref pai.goXuanShangPai[i], 0x00, new Vector2(x, y), 0);
                }
                x += paiWidth * xuanShangScalse;
            }

            if (isLiXuanShangPai)
            {
                x = X;
                y -= paiHeight * xuanShangScalse;
                for (int i = 0; i <= 4; i++)
                {
                    if (i < pai.xuanShangPai.Count)
                    {
                        pai.goLiXuanShangPai[i] = Instantiate(goPai, goCanvas.transform);
                        pai.goLiXuanShangPai[i].transform.SetSiblingIndex(3);
                        RectTransform rt = pai.goLiXuanShangPai[i].GetComponent<RectTransform>();
                        rt.localScale *= xuanShangScalse;
                        DrawPai(ref pai.goLiXuanShangPai[i], pai.liXuanShangPai[i], new Vector2(x, y + offsetY), 0);
                    }
                    x += paiWidth * xuanShangScalse;
                }
            }
        }

        // 【描画】対局中オプション
        private void DrawJuOption()
        {
            if (goMingWu != null)
            {
                goMingWu.gameObject.SetActive(false);
            }
            if (goDianCha != null)
            {
                goDianCha.gameObject.SetActive(false);
            }

            if (!chang.existsPlayer)
            {
                return;
            }

            // 鳴 有り・無し
            string[] labelMingWu = new string[] { "鳴無", "鳴有" };
            if (goMingWu == null)
            {
                goMingWu = Instantiate(goButton, goCanvas.transform);
                goMingWu.onClick.AddListener(() =>
                {
                    sheDing.mingWu = !sheDing.mingWu;
                    goMingWu.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.mingWu ? labelMingWu[0] : labelMingWu[1];
                    string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    File.WriteAllText(Path.Combine(directory, $"{SHE_DING_FILE_NAME}.json"), JsonUtility.ToJson(sheDing));
                });
            }
            float x = paiWidth * 8f;
            float y = -(paiHeight * 9.3f);
            if (orientation != ScreenOrientation.Portrait)
            {
                x = paiWidth * 13f;
                y = -(paiHeight * 0.5f);
            }
            goMingWu.gameObject.SetActive(true);
            DrawButton(ref goMingWu, sheDing.mingWu ? labelMingWu[0] : labelMingWu[1], new Vector2(x, y));

            // 点差
            if (goDianCha == null)
            {
                goDianCha = Instantiate(goButton, goCanvas.transform);
                EventTrigger trigger = goDianCha.AddComponent<EventTrigger>();
                EventTrigger.Entry pointerDownEntry = new()
                {
                    eventID = EventTriggerType.PointerDown
                };
                pointerDownEntry.callback.AddListener((data) =>
                {
                    chang.isDianCha = true;
                    chang.isDianChaDraw = true;
                });
                trigger.triggers.Add(pointerDownEntry);

                EventTrigger.Entry pointerUpEntry = new()
                {
                    eventID = EventTriggerType.PointerUp
                };
                pointerUpEntry.callback.AddListener((data) =>
                {
                    chang.isDianCha = false;
                });
                trigger.triggers.Add(pointerUpEntry);
            }

            if (orientation == ScreenOrientation.Portrait)
            {
                x -= paiWidth * 3.5f;
            }
            else
            {
                y -= paiHeight * 1.5f;
            }
            goDianCha.gameObject.SetActive(true);
            DrawButton(ref goDianCha, "点差", new Vector2(x, y));
        }

        // 【描画】雀士選択
        private void DrawQiaoShiXuanZe()
        {
            ClearScreen();

            float x = 0;
            float y = paiHeight * 5f;

            string[] displayText = new string[] { "観戦", "対戦" };
            DrawButton(ref goPlayerNoExists, chang.existsPlayer ? displayText[1] : displayText[0], new Vector2(0, y), 4);
            goPlayerNoExists.onClick.AddListener(() =>
            {
                chang.existsPlayer = !chang.existsPlayer;
                goPlayerNoExists.GetComponentInChildren<TextMeshProUGUI>().text = chang.existsPlayer ? displayText[1] : displayText[0];

                var keys = new List<string>(qiaoShiMingQian.Keys);
                int selectedCount = 0;
                foreach (string key in keys)
                {
                    if (qiaoShiMingQian[key])
                    {
                        selectedCount++;
                    }
                }
                int index = 0;
                if ((chang.existsPlayer && selectedCount > 3) || (!chang.existsPlayer && selectedCount > 4))
                {
                    foreach (string key in keys)
                    {
                        if (qiaoShiMingQian[key])
                        {
                            index++;
                        }
                        if (index == selectedCount)
                        {
                            qiaoShiMingQian[key] = false;
                        }
                    }
                }

                index = 0;
                foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
                {
                    SetQiaoShiColor(kvp.Key, index++);
                }
            });
            y -= paiHeight * 1.5f;

            int index = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                x = paiWidth * 4 * (index % 2 == 0 ? -1 : 1);
                int pos = index;
                DrawButton(ref goQiaoShi[index], kvp.Key, new Vector2(x, y), quiaoShiButtonMaxLen);
                goQiaoShi[index].onClick.AddListener(() =>
                {
                    OnClickQiaoShi(kvp.Key, pos);
                });

                SetQiaoShiColor(kvp.Key, index);
                if (index % 2 == 1 || index == qiaoShiMingQian.Count - 1)
                {
                    y -= paiHeight * 1.5f;
                }
                index++;
            }

            DrawButton(ref goRandom, "ランダム", new Vector2(0, y));
            goRandom.onClick.AddListener(() =>
            {
                RandomQiaoShiXuanZe();
                index = 0;
                foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
                {
                    SetQiaoShiColor(kvp.Key, index++);
                }
            });
        }

        // ランダム雀士自動選択
        private void RandomQiaoShiXuanZe()
        {
            var keys = new List<string>(qiaoShiMingQian.Keys);
            int mianz = 0;
            foreach (string key in keys)
            {
                if (qiaoShiMingQian[key])
                {
                    mianz++;
                    qiaoShiMingQian[key] = false;
                }
            }
            int cnt = 0;
            while (cnt < mianz)
            {
                System.Random r = new();
                int n = r.Next(0, qiaoShiMingQian.Count);
                int i = 0;
                foreach (string key in keys)
                {
                    if (i == n)
                    {
                        if (!qiaoShiMingQian[key])
                        {
                            qiaoShiMingQian[key] = true;
                            cnt++;
                            break;
                        }
                    }
                    i++;
                }
            }
        }

        // 名前クリック
        private void OnClickQiaoShi(string mingQian, int pos)
        {
            bool selected = qiaoShiMingQian[mingQian];
            if (!selected)
            {
                int selectedCount = 0;
                string firstMing = "";
                int firstPos = 0;
                bool isFirst = true;
                string lastMing = "";
                int lastPos = 0;
                int index = 0;
                int cnt = chang.existsPlayer ? 2 : 3;
                foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
                {
                    if (kvp.Value)
                    {
                        selectedCount++;
                        if (isFirst)
                        {
                            firstMing = kvp.Key;
                            firstPos = index;
                            isFirst = false;
                        }
                        lastMing = kvp.Key;
                        lastPos = index;
                    }
                    if (selectedCount > cnt)
                    {
                        string changeMing = lastMing;
                        int changePos = lastPos;
                        if (pos > lastPos)
                        {
                            changeMing = firstMing;
                            changePos = firstPos;
                        }
                        qiaoShiMingQian[changeMing] = false;
                        SetQiaoShiColor(changeMing, changePos);
                        break;
                    }
                    index++;
                }
            }
            qiaoShiMingQian[mingQian] = !selected;
            SetQiaoShiColor(mingQian, pos);
        }

        // 名前色設定
        private void SetQiaoShiColor(string mingQian, int pos)
        {
            TextMeshProUGUI text = goQiaoShi[pos].GetComponentInChildren<TextMeshProUGUI>();
            text.color = qiaoShiMingQian[mingQian] ? Color.black : Color.gray;
        }

        // 【描画】フォロー雀士選択
        private void DrawFollowQiaoShiXuanZe()
        {
            ClearScreen();

            DrawText(ref goJu, "フォロー雀士", new Vector2(0, paiHeight * 5f), 0, 25);

            float x = 0;
            float y = paiHeight * 3.5f;
            int i = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                x = paiWidth * 4 * (i % 2 == 0 ? -1 : 1);
                int pos = i;
                DrawButton(ref goQiaoShi[i], kvp.Key, new Vector2(x, y), quiaoShiButtonMaxLen);
                goQiaoShi[i].onClick.AddListener(() =>
                {
                    OnClickFollowQiaoShi(kvp.Key);
                });

                if (i % 2 == 1)
                {
                    y -= paiHeight * 1.5f;
                }
                i++;
            }
        }

        // 名前クリック
        private void OnClickFollowQiaoShi(string mingQian)
        {
            qiaoShis.Insert(0, GetQiaoShi(mingQian, true));
            qiaoShis[0].mingQian = PLAYER_NAME;
            qiaoShis[0].follow = true;
            qiaoShis[0].player = true;
            chang.keyPress = true;
        }

        // 【ゲーム】場決
        private void ChangJue()
        {
            ClearScreen();

            if (qiaoShis[0] == null)
            {
                OnClickScreenFollowNone();
            }
            List<int> fengPai = new();
            for (int i = 0; i < Pai.FENG_PAI_DING_YI.Length - (4 - qiaoShis.Count); i++)
            {
                fengPai.Add(Pai.FENG_PAI_DING_YI[i]);
            }
            chang.Shuffle(fengPai, 20);

            Button[] goButton = new Button[qiaoShis.Count];

            int idx = 0;
            for (int i = 0x31; i <= 0x34 - (4 - qiaoShis.Count); i++)
            {
                for (int j = 0; j < fengPai.Count; j++)
                {
                    if (fengPai[j] == i)
                    {
                        (qiaoShis[j], qiaoShis[idx]) = (qiaoShis[idx], qiaoShis[j]);
                    }
                }
                idx++;
            }

            int order = 0;
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                QiaoShi shi = qiaoShis[i];
                shi.playOrder = i;
                if (shi.player)
                {
                    order = i;
                }
            }
            foreach (QiaoShi shi in qiaoShis)
            {
                shi.playOrder -= order;
                if (shi.playOrder < 0)
                {
                    shi.playOrder += qiaoShis.Count;
                }
            }

            DrawMingQian();

            // 記録の読込
            foreach (QiaoShi shi in qiaoShis)
            {
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                string filePath = Path.Combine(directory, $"{shi.mingQian}.json");
                if (File.Exists(filePath))
                {
                    shi.jiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(filePath));
                    JiLu2Nao(shi);
                }
                else
                {
                    shi.jiLu = new JiLu();
                    Nao2JiLu(shi);
                }
            }

            ClearGameObject(ref goButton);

            chang.eventStatus = Event.QIN_JUE;
            WriteData();
        }

        // 記録を脳へ反映
        private void JiLu2Nao(QiaoShi shi)
        {
            if (shi.player && shi.follow)
            {
                return;
            }
            if (shi is QiaoJiXie qjx)
            {
                qjx.naos[(int)QiaoShi.XingGe.XUAN_SHANG].score = shi.jiLu.naoXuanShang;
                qjx.naos[(int)QiaoShi.XingGe.YI_PAI].score = shi.jiLu.naoYiPai;
                qjx.naos[(int)QiaoShi.XingGe.SHUN_ZI].score = shi.jiLu.naoShunZi;
                qjx.naos[(int)QiaoShi.XingGe.KE_ZI].score = shi.jiLu.naoKeZi;
                qjx.naos[(int)QiaoShi.XingGe.LI_ZHI].score = shi.jiLu.naoLiZhi;
                qjx.naos[(int)QiaoShi.XingGe.MING].score = shi.jiLu.naoMing;
                qjx.naos[(int)QiaoShi.XingGe.RAN].score = shi.jiLu.naoRan;
                qjx.naos[(int)QiaoShi.XingGe.TAO].score = shi.jiLu.naoTao;

            }
        }

        // 脳を記録へ反映
        private void Nao2JiLu(QiaoShi shi)
        {
            if (shi is QiaoJiXie qjx)
            {
                shi.jiLu.naoXuanShang = qjx.naos[(int)QiaoShi.XingGe.XUAN_SHANG].score;
                shi.jiLu.naoYiPai = qjx.naos[(int)QiaoShi.XingGe.YI_PAI].score;
                shi.jiLu.naoShunZi = qjx.naos[(int)QiaoShi.XingGe.SHUN_ZI].score;
                shi.jiLu.naoKeZi = qjx.naos[(int)QiaoShi.XingGe.KE_ZI].score;
                shi.jiLu.naoLiZhi = qjx.naos[(int)QiaoShi.XingGe.LI_ZHI].score;
                shi.jiLu.naoMing = qjx.naos[(int)QiaoShi.XingGe.MING].score;
                shi.jiLu.naoRan = qjx.naos[(int)QiaoShi.XingGe.RAN].score;
                shi.jiLu.naoTao = qjx.naos[(int)QiaoShi.XingGe.TAO].score;
            }

            string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(Path.Combine(directory, $"{shi.mingQian}.json"), JsonUtility.ToJson(shi.jiLu));
        }

        // 【ゲーム】親決
        private IEnumerator QinJue()
        {
            System.Random r = new();
            if (forwardMode > ForwardMode.NORMAL)
            {
                chang.keyPress = true;
            }
            for (int i = 0; i < 60; i++)
            {
                sai1 = r.Next(0, 6) + 1;
                sai2 = r.Next(0, 6) + 1;
                chang.isQinJueDraw = true;
                if (chang.keyPress)
                {
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }

            yield return Pause(ForwardMode.NORMAL);

            // 親
            chang.qin = (sai1 + sai2 - 1) % qiaoShis.Count;
            // 起家
            chang.qiaJia = chang.qin;
            chang.changFeng = 0x31;
            chang.isQinJueDraw = true;

            yield return Pause(ForwardMode.NORMAL);

            chang.eventStatus = Event.ZHUANG_CHU_QI_HUA;
            qinJueCoroutine = null;
            WriteData();
        }

        // 【描画】親決
        private void DrawQinJue()
        {
            ClearScreen();
            DrawMingQian();

            DrawSais(0, sai1, sai2);
            DrawQiJia();
        }

        // 【描画】名前(4人分)
        private void DrawMingQian()
        {
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                DrawMingQian(jia);
            }
        }

        // 【描画】名前
        private void DrawMingQian(int jia)
        {
            QiaoShi shi = qiaoShis[jia];
            float x;
            float y;
            switch (chang.eventStatus)
            {
                case Event.PEI_PAI:
                case Event.DUI_JU:
                case Event.DUI_JU_ZHONG_LE:
                    x = -paiWidth * 5f;
                    y = -(paiWidth * 3f + paiHeight * 3f);
                    if (!shi.player)
                    {
                        DrawText(ref goMingQian[jia], shi.mingQian, Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 20);
                    }
                    break;
                default:
                    x = 0f;
                    y = -(paiHeight * 5);
                    DrawText(ref goMingQian[jia], shi.mingQian, Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 25);
                    break;
            }
        }

        // 【描画】サイコロ(2個分)
        private void DrawSais(int jia, int mu1, int mu2)
        {
            float margin = paiWidth / 3;
            DrawSai(ref goSai1, jia, mu1, -margin);
            DrawSai(ref goSai2, jia, mu2, margin);
        }

        // 【描画】サイコロ
        private void DrawSai(ref Image go, int jia, int mu, float margin)
        {
            ClearGameObject(ref go);
            go = Instantiate(goSais[mu - 1].GetComponent<Image>(), goCanvas.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            QiaoShi shi = qiaoShis[jia];
            go.transform.Rotate(0, 0, 90 * shi.playOrder);
            float x = 0;
            float y = -(paiWidth * 1.2f);
            rt.anchoredPosition = Cal(x + margin, y, shi.playOrder);
        }

        // 【描画】起家
        private void DrawQiJia()
        {
            float x, y;
            switch (chang.eventStatus)
            {
                case Event.QIN_JUE:
                    x = 0;
                    y = -(paiWidth * 2.5f + paiHeight * 2f);
                    break;
                default:
                    x = paiWidth * 7.2f;
                    y = -(paiHeight * 4.9f);
                    break;
            }
            if (chang.changFeng > 0x30)
            {
                for (int i = 0; i < qiaoShis.Count; i++)
                {
                    int jia = (chang.qin + i) % qiaoShis.Count;
                    if (chang.qiaJia == jia)
                    {
                        ClearGameObject(ref goQiJia);
                        QiaoShi shi = qiaoShis[jia];
                        if (shi.player && orientation != ScreenOrientation.Portrait)
                        {
                            x -= paiWidth * 1.2f;
                            y += paiHeight * 0.6f;
                        }
                        if (shi.playOrder == 3 && orientation != ScreenOrientation.Portrait)
                        {
                            x -= paiHeight * 0.6f;
                        }
                        goQiJia = Instantiate(goQiJias[chang.changFeng - 0x31].GetComponent<Image>(), goCanvas.transform);
                        goQiJia.transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                        RectTransform rt = goQiJia.GetComponent<RectTransform>();
                        rt.anchoredPosition = Cal(chang.eventStatus == Event.QIN_JUE ? 0 : x, y, shi.playOrder);
                    }
                }
            }
        }

        // 【ゲーム】荘初期化
        private void ZhuangChuQiHua()
        {
            chang.ZhuangChuQiHua();
            foreach (QiaoShi shi in qiaoShis)
            {
                shi.ZhuangChuQiHua();
            }

            chang.eventStatus = Event.PEI_PAI;
            WriteData();
        }

        // 【ゲーム】配牌
        private IEnumerator PeiPai()
        {
            // 局初期化
            JuChuQiHua();
            // 配牌
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < qiaoShis.Count; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        qiaoShis[(chang.qin + j) % qiaoShis.Count].ZiMo(pai.ShanPaiZiMo(), true);
                    }
                    chang.isPeiPaiDraw = true;
                    yield return new WaitForSeconds(waitTime / 3);
                }
            }
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                qiaoShis[(chang.qin + i) % qiaoShis.Count].ZiMo(pai.ShanPaiZiMo(), true);
                chang.isPeiPaiDraw = true;
            }
            yield return new WaitForSeconds(waitTime / 3);
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                qiaoShis[(chang.qin + i) % qiaoShis.Count].LiPai();
                chang.isPeiPaiDraw = true;
            }

            chang.eventStatus = Event.DUI_JU;
            peiPaiCoroutine = null;
            WriteData();
        }

        // 【描画】配牌
        private void DrawPeiPai()
        {
            ClearScreen();
            DrawJuFrame();
            DrawJuOption();
            DrawQiJia();
            DrawMingQian();

            for (int jia = 0; jia < qiaoShis.Count; jia++)
            {
                QiaoShi shi = qiaoShis[jia];
                DrawShouPai(jia, shi.ziJiaYao, shi.ziJiaXuanZe);
            }
        }

        // 局初期化
        private void JuChuQiHua()
        {
            // 局初期化
            chang.JuChuQiHua();
            // 雀士初期化
            int w = 0;
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                shi.JuChuQiHua(Pai.FENG_PAI_DING_YI[i]);

                if (shi.player)
                {
                    w = shi.feng - 0x31;
                }
            }

            // 洗牌
            pai.XiPai();
            // 積込
            // List<List<int>> jiRuPai = new()
            // {
            //    new() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x31, 0x31, 0x31, 0x37 },
            //    new() { 0x37, 0x37 },
            //    new() { 0x37 },
            // };
            // pai.JiRu(w, jiRuPai);

            // 洗牌嶺上
            pai.XiPaiLingShang();

            chang.tingPaiLianZhuang = Zhuang.XU_HANG;

            chang.eventStatus = Event.PEI_PAI;
            duiJuCoroutine = null;
        }

        private State GetState(int jia, bool isZiJia)
        {
            // 状態
            State state = new();

            QiaoShi ziJiaShi = qiaoShis[jia];
            ziJiaShi.ZhuangTai(state, true);
            for (int i = jia + 1; i < jia + qiaoShis.Count; i++)
            {
                int j = i % qiaoShis.Count;
                QiaoShi taJiaShi = qiaoShis[j];
                taJiaShi.ZhuangTai(state, false);
            }
            pai.ZhuangTai(state);
            chang.ZhuangTai(state, isZiJia);

            return state;
        }

        // 【ゲーム】対局
        private IEnumerator DuiJu()
        {
            chang.isZiJiaYaoDraw = false;
            chang.isTaJiaYaoDraw = false;

            QiaoShi.YaoDingYi changZiJiaYao = chang.ziJiaYao;
            QiaoShi.YaoDingYi changTaJiaYao = chang.taJiaYao;

            QiaoShi ziJiaShi = qiaoShis[chang.ziMoFan];
            if ((chang.ziJiaYao == QiaoShi.YaoDingYi.Wu || chang.ziJiaYao == QiaoShi.YaoDingYi.LiZhi) && chang.taJiaYao == QiaoShi.YaoDingYi.Wu)
            {
                // 山牌自摸
                ziJiaShi.ZiMo(pai.ShanPaiZiMo());
            }
            else if (chang.ziJiaYao == QiaoShi.YaoDingYi.AnGang || chang.ziJiaYao == QiaoShi.YaoDingYi.JiaGang || chang.taJiaYao == QiaoShi.YaoDingYi.DaMingGang)
            {
                // 嶺上牌自摸
                ziJiaShi.ZiMo(pai.LingShangPaiZiMo());
            }

            if (chang.ziJiaYao == QiaoShi.YaoDingYi.AnGang || chang.ziJiaYao == QiaoShi.YaoDingYi.JiaGang || chang.taJiaYao == QiaoShi.YaoDingYi.DaMingGang || chang.taJiaYao == QiaoShi.YaoDingYi.Chi || chang.taJiaYao == QiaoShi.YaoDingYi.Bing)
            {
                if (!ziJiaShi.player)
                {
                    yield return new WaitForSeconds(waitTime);
                }
                // 鳴処理
                ziJiaShi.MingChuLi();
                // 消
                foreach (QiaoShi shi in qiaoShis)
                {
                    shi.Xiao();
                }
                // 四風子連打処理
                chang.SiFengZiLianDaChuLi(0xff);
                chang.isDuiJuDraw = true;
                yield return new WaitForSeconds(waitTime);
            }

            chang.ziJiaYao = QiaoShi.YaoDingYi.Wu;
            chang.taJiaYao = QiaoShi.YaoDingYi.Wu;

            // 思考自家判定
            ziJiaShi.SiKaoZiJiaPanDing();

            yield return new WaitForSeconds(waitTime);

            // 学習(自家 状態)
            ziJiaShi.SetTransitionZiJiaState(GetState(chang.ziMoFan, true));

            // 思考自家
            if (ziJiaShi.waiBuSikao)
            {
                StartCoroutine(ziJiaShi.SiKaoZiJiaCoroutine());
                while (ziJiaShi.asyncStop) { yield return null; }
            }
            else
            {
                ziJiaShi.SiKaoZiJia();
            }
            if (ziJiaShi.player)
            {
                if (!(sheDing.liZhiAuto && ziJiaShi.liZhi) || ziJiaShi.heLe || ziJiaShi.anGangPaiWei.Count > 0 || ziJiaShi.jiaGangPaiWei.Count > 0)
                {
                    if (ziJiaShi.follow && ziJiaShi.ziJiaYao == QiaoShi.YaoDingYi.Wu)
                    {
                        ziJiaShi.DaiPaiXiangTingShuJiSuan(ziJiaShi.ziJiaXuanZe);
                    }
                    chang.isZiJiaYaoDraw = true;
                    chang.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.NORMAL);
                }
            }
            chang.isZiJiaYaoDraw = false;
            ziJiaShi.KaiLiZhiChuLi();
            chang.ziJiaYao = ziJiaShi.ziJiaYao;
            chang.ziJiaXuanZe = ziJiaShi.ziJiaXuanZe;

            // 学習(自家 行動)
            int paiOrIndex = ziJiaShi.ziJiaYao == QiaoShi.YaoDingYi.Wu ? ziJiaShi.shouPai[ziJiaShi.ziJiaXuanZe] & QiaoShi.QIAO_PAI : ziJiaShi.ziJiaXuanZe;
            ziJiaShi.SetTransitionZiJiaAction(new() { (int)ziJiaShi.ziJiaYao, paiOrIndex });

            // 錯和自家判定
            if (ziJiaShi.CuHeZiJiaPanDing())
            {
                // 錯和
                chang.CuHe(chang.ziMoFan);
                chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                chang.isDuiJuDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);
                chang.eventStatus = Event.DIAN_BIAO_SHI;
                WriteData();
                yield break;
            }

            switch (chang.ziJiaYao)
            {
                case QiaoShi.YaoDingYi.ZiMo:
                    // 自摸
                    ziJiaShi.HeLeChuLi();
                    chang.heleFan = chang.ziMoFan;
                    // 学習(自家 次状態)
                    ziJiaShi.SetTransitionZiJiaNextState(GetState(chang.ziMoFan, true));
                    chang.tingPaiLianZhuang = chang.ziMoFan == chang.qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    chang.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    chang.eventStatus = Event.YI_BIAO_SHI;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                case QiaoShi.YaoDingYi.JiuZhongJiuPai:
                    // 九種九牌
                    // 描画
                    chang.JiuZhongJiuPaiChuLi();
                    // 学習(自家 次状態)
                    ziJiaShi.SetTransitionZiJiaNextState(GetState(chang.ziMoFan, true));
                    chang.tingPaiLianZhuang = guiZe.jiuZhongJiuPaiLianZhuang == 1 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    chang.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    chang.eventStatus = Event.DIAN_BIAO_SHI;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                case QiaoShi.YaoDingYi.LiZhi:
                    // 立直・開立直
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    pai.LingShanChuLi();
                    // 立直
                    ziJiaShi.LiZhiChuLi();
                    break;
                case QiaoShi.YaoDingYi.AnGang:
                    // 暗槓
                    ziJiaShi.AnGang(chang.ziJiaXuanZe);
                    // 嶺上牌処理
                    pai.LingShangPaiChuLi();
                    // 四開槓判定
                    if (pai.SiKaiGangPanDing())
                    {
                        chang.tingPaiLianZhuang = guiZe.siKaiGangLianZhuang == 0 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        chang.isDuiJuDraw = true;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        chang.eventStatus = Event.DIAN_BIAO_SHI;
                        duiJuCoroutine = null;
                        WriteData();
                        yield break;
                    }
                    // 学習(自家 次状態)
                    ziJiaShi.SetTransitionZiJiaNextState(GetState(chang.ziMoFan, true));
                    chang.eventStatus = Event.DUI_JU;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                case QiaoShi.YaoDingYi.JiaGang:
                    // 加槓
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    pai.LingShanChuLi();
                    // 加槓
                    ziJiaShi.JiaGang(chang.ziJiaXuanZe);
                    pai.QiangGang();
                    break;
                default:
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    pai.LingShanChuLi();
                    break;
            }
            // 学習(自家 次状態)
            ziJiaShi.SetTransitionZiJiaNextState(GetState(chang.ziMoFan, true));

            if (chang.ziJiaYao == QiaoShi.YaoDingYi.Wu || chang.ziJiaYao == QiaoShi.YaoDingYi.LiZhi)
            {
                ziJiaShi.DaiPaiXiangTingShuJiSuan(chang.ziJiaXuanZe);
                // 打牌前
                int dp = ziJiaShi.DaPaiQian();
                chang.isDuiJuDraw = true;
                yield return new WaitForSeconds(waitTime / 2);
                // 打牌
                ziJiaShi.DaPai(dp, changZiJiaYao, changTaJiaYao);
                ziJiaShi.ShePaiChuLi(chang.ziJiaYao);

                ziJiaShi.taJiaYao = QiaoShi.YaoDingYi.Wu;
                ziJiaShi.taJiaXuanZe = 0;
                ziJiaShi.SiKaoQianChuQiHua();

                ziJiaShi.LiPai();
                chang.isDuiJuDraw = true;
                yield return new WaitForSeconds(waitTime / 2);
                // 四風子連打処理
                chang.SiFengZiLianDaChuLi(chang.shePai);
                // 四風子連打判定
                if (chang.SiFengZiLianDaPanDing())
                {
                    chang.tingPaiLianZhuang = guiZe.siFengZiLianDaLianZhuang == 1 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    chang.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    chang.eventStatus = Event.DIAN_BIAO_SHI;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                }
            }

            if (changZiJiaYao == QiaoShi.YaoDingYi.JiaGang || changTaJiaYao == QiaoShi.YaoDingYi.DaMingGang)
            {
                // 加槓・大明槓成立
                // 嶺上牌処理
                pai.LingShangPaiChuLi();
                // 四開槓判定
                if (pai.SiKaiGangPanDing())
                {
                    chang.tingPaiLianZhuang = guiZe.siKaiGangLianZhuang == 0 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    chang.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    chang.eventStatus = Event.DIAN_BIAO_SHI;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                }
            }

            // 思考他家
            chang.taJiaYao = QiaoShi.YaoDingYi.Wu;
            chang.mingFan = chang.ziMoFan;
            int playerJia = -1;
            int playerIndex = -1;
            for (int i = chang.ziMoFan + 1; i < chang.ziMoFan + qiaoShis.Count; i++)
            {
                int jia = i % qiaoShis.Count;
                QiaoShi taJiaShi = qiaoShis[jia];
                // 思考他家判定
                taJiaShi.ziJiaYao = QiaoShi.YaoDingYi.Wu;
                taJiaShi.ziJiaXuanZe = 0;
                taJiaShi.SiKaoTaJiaPanDing(i - chang.ziMoFan);
                if (taJiaShi.heLe || taJiaShi.chiPaiWei.Count > 0 || taJiaShi.bingPaiWei.Count > 0 || taJiaShi.daMingGangPaiWei.Count > 0)
                {
                    // 学習(他家 状態)
                    taJiaShi.SetTransitionTaJiaState(GetState(chang.mingFan, false));
                }
                // 思考他家
                if (taJiaShi.player)
                {
                    playerJia = jia;
                    playerIndex = i;
                    continue;
                }
                if (taJiaShi.waiBuSikao)
                {
                    StartCoroutine(taJiaShi.SiKaoTaJiaCoroutine());
                    while (taJiaShi.asyncStop) { yield return null; }
                }
                else
                {
                    taJiaShi.SiKaoTaJia();
                }
                // 錯和他家判定
                if (taJiaShi.CuHeTaJiaPanDing())
                {
                    chang.CuHe(jia);
                    chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                    chang.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    chang.eventStatus = Event.DIAN_BIAO_SHI;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                }
                if (taJiaShi.taJiaYao > chang.taJiaYao)
                {
                    // 栄和、大明槓、石並、吃
                    chang.taJiaYao = taJiaShi.taJiaYao;
                    chang.taJiaXuanZe = taJiaShi.taJiaXuanZe;
                    chang.mingFan = jia;
                }
                if (taJiaShi.taJiaYao == QiaoShi.YaoDingYi.RongHe)
                {
                    chang.rongHeFans.Add(new RongHeFan { fan = jia, index = i });
                }
            }
            if (playerJia >= 0)
            {
                // 思考他家プレイヤー分
                QiaoShi playerShi = qiaoShis[playerJia];
                if (playerShi.heLe || playerShi.chiPaiWei.Count > 0 || playerShi.bingPaiWei.Count > 0 || playerShi.daMingGangPaiWei.Count > 0)
                {
                    if (playerShi.heLe
                        || (playerShi.daMingGangPaiWei.Count > 0 && chang.taJiaYao < QiaoShi.YaoDingYi.DaMingGang)
                        || (playerShi.bingPaiWei.Count > 0 && chang.taJiaYao < QiaoShi.YaoDingYi.Bing)
                        || (playerShi.chiPaiWei.Count > 0 && chang.taJiaYao < QiaoShi.YaoDingYi.Chi))
                    {
                        if (playerShi.waiBuSikao)
                        {
                            StartCoroutine(playerShi.SiKaoTaJiaCoroutine());
                            while (playerShi.asyncStop) { yield return null; }
                        }
                        else
                        {
                            playerShi.SiKaoTaJia();
                        }
                        chang.isTaJiaYaoDraw = true;
                        chang.isDuiJuDraw = true;
                        yield return Pause(ForwardMode.NORMAL);
                        ClearGameObject(ref goYao);
                        if (chang.rongHeFans.Count == 0 && playerShi.taJiaYao != QiaoShi.YaoDingYi.Wu)
                        {
                            chang.taJiaYao = playerShi.taJiaYao;
                            chang.taJiaXuanZe = playerShi.taJiaXuanZe;
                            chang.mingFan = playerJia;
                        }
                        if (playerShi.taJiaYao == QiaoShi.YaoDingYi.RongHe)
                        {
                            chang.rongHeFans.Add(new RongHeFan { fan = playerJia, index = playerIndex });
                        }
                    }
                }
            }
            chang.isTaJiaYaoDraw = false;

            if (chang.taJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                // 栄和
                chang.rongHeFans = chang.rongHeFans.OrderBy(x => x.index).ToList();
                chang.mingFan = chang.rongHeFans[0].fan;
                // 捨牌処理
                foreach (RongHeFan rongHeFan in chang.rongHeFans)
                {
                    qiaoShis[rongHeFan.fan].ZiMo(chang.shePai);
                    // 和了
                    qiaoShis[rongHeFan.fan].HeLeChuLi();
                }

                chang.tingPaiLianZhuang = chang.mingFan == chang.qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                chang.isDuiJuDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);

                if (chang.rongHeFans.Count == 3)
                {
                    if (guiZe.tRongHe == 0)
                    {
                        // トリプル栄和 無し(頭ハネ)
                        chang.rongHeFans.RemoveRange(1, 2);
                    }
                    else if (guiZe.tRongHe >= 2)
                    {
                        // トリプル栄和 流局
                        chang.rongHeFans.Clear();
                        chang.tingPaiLianZhuang = guiZe.tRongHe == 2 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        chang.eventStatus = Event.DIAN_BIAO_SHI;
                        duiJuCoroutine = null;
                        WriteData();
                        yield break;
                    }
                }
                else if (chang.rongHeFans.Count == 2)
                {
                    if (guiZe.wRongHe == 0)
                    {
                        // ダブル栄和 無し(頭ハネ)
                        chang.rongHeFans.RemoveAt(1);
                    }
                }
                if (chang.rongHeFans.Count >= 2)
                {
                    // ダブル栄和・トリプル栄和の場合、親が栄和した場合は連荘
                    foreach (RongHeFan rongHeFan in chang.rongHeFans)
                    {
                        if (qiaoShis[rongHeFan.fan].feng == 0x31)
                        {
                            chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                }

                chang.heleFan = chang.mingFan;
                chang.eventStatus = Event.YI_BIAO_SHI;
                duiJuCoroutine = null;
                WriteData();
                yield break;
            }

            for (int i = chang.ziMoFan + 1; i < chang.ziMoFan + qiaoShis.Count; i++)
            {
                // 振聴牌処理
                qiaoShis[i % qiaoShis.Count].ZhenTingPaiChuLi();
            }

            switch (chang.ziJiaYao)
            {
                case QiaoShi.YaoDingYi.LiZhi:
                    // 立直成立
                    chang.LiZhiChuLi();
                    qiaoShis[chang.ziMoFan].LiZiChuLi();
                    qiaoShis[chang.ziMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.LiZhi);
                    // 四家立直判定
                    if (chang.SiJiaLiZhiPanDing())
                    {
                        chang.tingPaiLianZhuang = guiZe.siJiaLiZhiLianZhuang == 1 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        chang.isDuiJuDraw = true;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        chang.eventStatus = Event.DIAN_BIAO_SHI;
                        duiJuCoroutine = null;
                        WriteData();
                        yield break;
                    }
                    break;
                case QiaoShi.YaoDingYi.JiaGang:
                    // 加槓
                    // 加槓処理
                    pai.QiangGangChuLi();
                    chang.eventStatus = Event.DUI_JU;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                default:
                    break;
            }

            QiaoShi mingShi = qiaoShis[chang.mingFan];
            // 学習(他家 行動)
            mingShi.SetTransitionTaJiaAction(new() { (int)mingShi.taJiaYao, mingShi.taJiaXuanZe });

            switch (chang.taJiaYao)
            {
                case QiaoShi.YaoDingYi.DaMingGang:
                    // 大明槓
                    mingShi.DaMingGang();
                    // 捨牌処理
                    qiaoShis[chang.ziMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.DaMingGang);
                    chang.ziMoFan = chang.mingFan;
                    chang.eventStatus = Event.DUI_JU;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                case QiaoShi.YaoDingYi.Bing:
                    // 石並
                    mingShi.Bing();
                    // 捨牌処理
                    qiaoShis[chang.ziMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.Bing);
                    chang.ziMoFan = chang.mingFan;
                    chang.eventStatus = Event.DUI_JU;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                case QiaoShi.YaoDingYi.Chi:
                    // 吃
                    mingShi.Chi();
                    // 捨牌処理
                    qiaoShis[chang.ziMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.Chi);
                    chang.ziMoFan = chang.mingFan;
                    chang.eventStatus = Event.DUI_JU;
                    duiJuCoroutine = null;
                    WriteData();
                    yield break;
                default:
                    break;
            }

            // 流局判定
            if (pai.LiuJuPanDing())
            {
                // 流し満貫判定
                if (guiZe.liuManGuan)
                {
                    for (int i = 0; i < qiaoShis.Count; i++)
                    {
                        int jia = (chang.qin + i) % qiaoShis.Count;
                        if (qiaoShis[jia].LiuJu())
                        {
                            chang.ziJiaYao = QiaoShi.YaoDingYi.ZiMo;
                            chang.ziMoFan = jia;
                            chang.heleFan = jia;
                            break;
                        }
                    }
                }
                chang.tingPaiLianZhuang = Zhuang.LUN_ZHUANG;
                if (guiZe.nanChangBuTingLianZhuang && chang.changFeng >= 0x32)
                {
                    chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                }
                for (int i = 0; i < qiaoShis.Count; i++)
                {
                    int jia = (chang.qin + i) % qiaoShis.Count;
                    QiaoShi shi = qiaoShis[jia];
                    // 形聴判定
                    shi.XingTingPanDing();
                    if (shi.liuShiManGuan)
                    {
                        // 流し満貫
                        if (jia == chang.qin)
                        {
                            chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                    else if (shi.xingTing)
                    {
                        // 聴牌
                        if (jia == chang.qin)
                        {
                            chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                    else
                    {
                        // 不聴
                        if (shi.liZhi)
                        {
                            // 錯和(不聴立直)
                            chang.CuHe(jia);
                            chang.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                            chang.isDuiJuDraw = true;
                            yield return Pause(ForwardMode.FAST_FORWARD);
                            chang.eventStatus = Event.DIAN_BIAO_SHI;
                            duiJuCoroutine = null;
                            WriteData();
                            yield break;
                        }
                    }
                    chang.isDuiJuDraw = true;
                    shi.zhongLiao = true;
                    yield return new WaitForSeconds(waitTime);
                }

                chang.isDuiJuDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);
                chang.eventStatus = Event.DIAN_BIAO_SHI;
                duiJuCoroutine = null;
                WriteData();
                yield break;
            }

            chang.ziMoFan = (chang.ziMoFan + 1) % qiaoShis.Count;
            chang.eventStatus = Event.DUI_JU;
            duiJuCoroutine = null;
            chang.isDuiJuDraw = true;
            WriteData();
        }

        // 【描画】対局
        private void DrawDuiJu()
        {
            ClearScreen();
            DrawJuFrame();
            DrawJuOption();
            DrawQiJia();
            DrawMingQian();

            for (int jia = 0; jia < qiaoShis.Count; jia++)
            {
                QiaoShi shi = qiaoShis[jia];
                // 手牌
                if (shi.player)
                {
                    if (shi.ziJiaYao == QiaoShi.YaoDingYi.JiuZhongJiuPai || shi.ziJiaYao == QiaoShi.YaoDingYi.ZiMo || shi.ziJiaYao == QiaoShi.YaoDingYi.LiZhi || shi.ziJiaYao == QiaoShi.YaoDingYi.KaiLiZhi || shi.ziJiaYao == QiaoShi.YaoDingYi.AnGang || shi.ziJiaYao == QiaoShi.YaoDingYi.JiaGang)
                    {
                        DrawShouPai(jia, QiaoShi.YaoDingYi.Wu, -1);
                    }
                    else
                    {
                        DrawShouPai(jia, shi.ziJiaYao, shi.ziJiaXuanZe, shi.follow);
                    }
                }
                else
                {
                    if (shi.ziJiaYao == QiaoShi.YaoDingYi.ZiMo || (shi.taJiaYao == QiaoShi.YaoDingYi.RongHe && !chang.isTaJiaYaoDraw))
                    {
                        DrawShouPai(jia, QiaoShi.YaoDingYi.HeLe, -1);
                    }
                    else
                    {
                        DrawShouPai(jia, shi.ziJiaYao, shi.ziJiaXuanZe, shi.follow);
                    }
                }

                // 自家腰
                if (chang.isZiJiaYaoDraw)
                {
                    DrawZiJiaYao(shi, 0, shi.shouPai.Count - 1, true, false);
                }
                // 他家腰
                if (chang.isTaJiaYaoDraw)
                {
                    DrawTaJiaYao(jia, shi, 0, true);
                }
                // 待牌
                DrawDaiPai(jia);
                // 捨牌
                DrawShePai(jia);

                // 錯和
                if (shi.cuHeSheng != "")
                {
                    DrawCuHe(jia);
                }

                if (shi.zhongLiao)
                {
                    // 流し満貫
                    if (shi.liuShiManGuan)
                    {
                        DrawSheng(jia, QiaoShi.YaoDingYi.LiuManGuan);
                    }
                    else
                    {
                        DrawSheng(jia, shi.xingTing ? QiaoShi.YaoDingYi.TingPai : QiaoShi.YaoDingYi.BuTing);
                    }
                }
            }
            // 声
            DrawSheng(chang.ziMoFan, chang.ziJiaYao);
            DrawSheng(chang.mingFan, chang.taJiaYao);
            // 四家立直
            if (chang.SiJiaLiZhiPanDing())
            {
                DrawSheng(chang.ziMoFan, QiaoShi.YaoDingYi.SiJiaLiZhi);
            }
            // 四開槓
            if (pai.SiKaiGangPanDing())
            {
                DrawSheng(chang.ziMoFan, QiaoShi.YaoDingYi.SiJiaLiZhi);
            }
            // 四風子連打
            if (chang.SiFengZiLianDaPanDing())
            {
                DrawSheng(chang.ziMoFan, QiaoShi.YaoDingYi.SiFengZiLianDa);
            }
        }

        // 【描画】自家腰
        private void DrawZiJiaYao(QiaoShi shi, int mingWei, int ShouPaiWei, bool isFollow, bool isPass)
        {
            if (!shi.player)
            {
                return;
            }

            ClearGameObject(ref goYao);

            float width = paiWidth * 3.5f;
            float x = -(paiWidth * 7.5f);
            float y = -(paiHeight * 4.5f);
            if (orientation != ScreenOrientation.Portrait)
            {
                y += paiHeight * 0.5f;
            }

            int index = 0;
            if (shi.heLe && shi.taJiaYao == QiaoShi.YaoDingYi.Wu)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.ZiMo, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (!shi.liZhi && shi.liZhiPaiWei.Count > 0)
            {
                int wei = mingWei;
                if (isFollow && shi.ziJiaYao == QiaoShi.YaoDingYi.LiZhi)
                {
                    for (int i = 0; i < shi.liZhiPaiWei.Count; i++)
                    {
                        int w = shi.liZhiPaiWei[i];
                        if (w == shi.ziJiaXuanZe)
                        {
                            wei = i;
                            break;
                        }
                    }
                }
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.LiZhi, wei, ShouPaiWei, isFollow);
                x += width;
                index++;
                if (guiZe.kaiLiZhi)
                {
                    DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.KaiLiZhi, wei, ShouPaiWei, isFollow);
                    x += width;
                    index++;
                }
            }
            if (shi.anGangPaiWei.Count > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.AnGang, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.jiaGangPaiWei.Count > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.JiaGang, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.jiuZhongJiuPai)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.JiuZhongJiuPai, mingWei, ShouPaiWei, isFollow);
            }
            if (isPass)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(paiWidth * 7.5f, y), QiaoShi.YaoDingYi.Wu, mingWei, ShouPaiWei, isFollow);
            }
        }

        // 【描画】自家腰
        private void DrawOnClickZiJiaYao(ref Button go, QiaoShi shi, Vector2 xy, QiaoShi.YaoDingYi yao, int mingWei, int ShouPaiWei, bool isFollow)
        {
            go = Instantiate(goButton, goCanvas.transform);
            go.onClick.AddListener(() =>
            {
                OnClickZiJiaYao(chang.ziMoFan, shi, yao, mingWei, ShouPaiWei, isFollow);
            });
            string value = shi.YaoMingButton(yao);
            if (yao == QiaoShi.YaoDingYi.Wu)
            {
                value = shi.YaoMingButton(QiaoShi.YaoDingYi.Clear);
            }
            if (shi.follow && (shi.ziJiaYao != yao || shi.ziJiaYao == QiaoShi.YaoDingYi.Wu))
            {
                TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.gray;
            }
            DrawButton(ref go, value, xy, value.Length);
        }

        // 自家腰クリック
        private void OnClickZiJiaYao(int jia, QiaoShi shi, QiaoShi.YaoDingYi yao, int mingWei, int ShouPaiWei, bool isFollow)
        {
            ClearGameObject(ref goLeft);
            ClearGameObject(ref goRight);
            ClearGameObject(ref goSelect);

            if (yao == QiaoShi.YaoDingYi.Wu)
            {
                DrawZiJiaYao(shi, 0, 0, false, false);
                DrawShouPai(jia, yao, -1);
                shi.DaiPaiJiSuan(-1);
                DrawDaiPai(jia);
            }
            else if (yao == QiaoShi.YaoDingYi.LiZhi || yao == QiaoShi.YaoDingYi.KaiLiZhi)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.liZhiPaiWei.Count, ShouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, isFollow);
                shi.DaiPaiXiangTingShuJiSuan(shi.liZhiPaiWei[mingWei]);
                DrawDaiPai(jia);
            }
            else if (yao == QiaoShi.YaoDingYi.AnGang && shi.anGangPaiWei.Count > 1)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.anGangPaiWei.Count, ShouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, isFollow);
            }
            else if (yao == QiaoShi.YaoDingYi.JiaGang && shi.jiaGangPaiWei.Count > 1)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.jiaGangPaiWei.Count, ShouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, isFollow);
            }
            else if (yao == QiaoShi.YaoDingYi.Select)
            {
                DrawZiJiaYao(shi, mingWei, ShouPaiWei, true, false);
                DrawShouPai(jia, yao, ShouPaiWei, isFollow);
                shi.DaiPaiXiangTingShuJiSuan(ShouPaiWei);
                DrawDaiPai(jia);
            }
            else
            {
                shi.ziJiaYao = yao;
                if (yao == QiaoShi.YaoDingYi.LiZhi || yao == QiaoShi.YaoDingYi.KaiLiZhi)
                {
                    mingWei %= shi.liZhiPaiWei.Count;
                }
                else if (yao == QiaoShi.YaoDingYi.AnGang)
                {
                    mingWei %= shi.anGangPaiWei.Count;
                }
                else if (yao == QiaoShi.YaoDingYi.JiaGang)
                {
                    mingWei %= shi.jiaGangPaiWei.Count;
                }
                shi.ziJiaXuanZe = mingWei;
                chang.keyPress = true;
            }
        }

        // 【描画】他家腰
        private void DrawTaJiaYao(int jia, QiaoShi shi, int mingWei, bool isFollow)
        {
            if (!shi.player)
            {
                return;
            }
            ClearGameObject(ref goYao);

            float width = paiWidth * 4;
            float x = -(paiWidth * 7.5f);
            float y = -(paiHeight * 4.5f);
            if (orientation != ScreenOrientation.Portrait)
            {
                y += paiHeight * 0.5f;
            }

            int index = 0;
            if (shi.heLe)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.RongHe, mingWei);
                x += width;
                index++;
            }
            if (shi.chiPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.Chi, isFollow && shi.taJiaYao == QiaoShi.YaoDingYi.Chi ? shi.taJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.bingPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.Bing, isFollow && shi.taJiaYao == QiaoShi.YaoDingYi.Bing ? shi.taJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.daMingGangPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.DaMingGang, mingWei);
                index++;
            }
            if (index > 0)
            {
                if (sheDing.mingQuXiao)
                {
                    DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(paiWidth * 7.5f, y), QiaoShi.YaoDingYi.Wu, mingWei);
                }
            }
        }

        // 【描画】他家腰
        private void DrawOnClickTaJiaYao(ref Button go, int jia, QiaoShi shi, Vector2 xy, QiaoShi.YaoDingYi yao, int mingWei)
        {
            go = Instantiate(goButton, goCanvas.transform);
            go.onClick.AddListener(() =>
            {
                OnClickTaJiaYao(jia, shi, yao, mingWei);
            });
            string value = shi.YaoMingButton(yao);
            if (shi.follow && shi.taJiaYao != yao)
            {
                TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.gray;
            }
            DrawButton(ref go, value, xy);
        }

        // 他家腰クリック
        private void OnClickTaJiaYao(int jia, QiaoShi shi, QiaoShi.YaoDingYi yao, int mingWei)
        {
            if (yao == QiaoShi.YaoDingYi.Bing && shi.bingPaiWei.Count > 1)
            {
                DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.bingPaiWei.Count, false);
                DrawShouPai(jia, yao, mingWei);
            }
            else if (yao == QiaoShi.YaoDingYi.Chi && shi.chiPaiWei.Count > 1)
            {
                DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.chiPaiWei.Count, false);
                DrawShouPai(jia, yao, mingWei);
            }
            else
            {
                shi.taJiaYao = yao;
                shi.taJiaXuanZe = 0;
                chang.keyPress = true;
            }
        }

        // 【描画】手牌
        private void DrawShouPai(int jia, QiaoShi.YaoDingYi yao, int mingWei, bool isFollow = false)
        {
            QiaoShi shi = qiaoShis[jia];
            ClearGameObject(ref shi.goShouPai);
            for (int i = 0; i < shi.goFuLuPais.Length; i++)
            {
                ClearGameObject(ref shi.goFuLuPais[i]);
            }

            float pw = paiWidth;
            float ph = paiHeight;

            if (shi.player)
            {
                pw *= PLAYER_PAI_SCALE;
                if (orientation != ScreenOrientation.Portrait)
                {
                    pw *= PLAYER_PAI_SCALE_LANDSCAPE;
                }
            }

            float x = -(pw * 6.6f);
            float y = -(pw * 3f + ph * 3.8f);
            if (shi.player)
            {
                y = -(pw * 3f + ph * 3.2f);
                ph *= PLAYER_PAI_SCALE;
                if (orientation != ScreenOrientation.Portrait)
                {
                    ph *= PLAYER_PAI_SCALE_LANDSCAPE;
                }
            }

            // 手牌
            float margin = pw / 4;
            int shouPaiCount = shi.shouPai.Count;
            for (int i = 0; i < chang.rongHeFans.Count; i++)
            {
                if (chang.rongHeFans[i].fan == jia)
                {
                    shouPaiCount--;
                    break;
                }
            }
            for (int i = 0; i < shouPaiCount; i++)
            {
                int p = shi.shouPai[i];
                float yy = y;
                shi.goShouPai[i] = Instantiate(goPai, goCanvas.transform);
                if (shi.player)
                {
                    shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE;
                    if (orientation != ScreenOrientation.Portrait)
                    {
                        shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE_LANDSCAPE;
                    }

                    int wei = i;
                    if (yao == QiaoShi.YaoDingYi.LiZhi || yao == QiaoShi.YaoDingYi.KaiLiZhi)
                    {
                        if (shi.liZhiPaiWei[mingWei] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, yao, wei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.AnGang)
                    {
                        if (shi.anGangPaiWei[mingWei][0] == i || shi.anGangPaiWei[mingWei][1] == i || shi.anGangPaiWei[mingWei][2] == i || shi.anGangPaiWei[mingWei][3] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.JiaGang)
                    {
                        if (shi.jiaGangPaiWei[mingWei][0] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Bing)
                    {
                        if (shi.bingPaiWei[mingWei][0] == i || shi.bingPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Chi)
                    {
                        if (shi.chiPaiWei[mingWei][0] == i || shi.chiPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Wu && mingWei >= -1)
                    {
                        if (isFollow && mingWei == i && chang.isZiJiaYaoDraw && !shi.daPaiHou)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, QiaoShi.YaoDingYi.DaPai, wei); });
                            yy = y + margin;
                        }
                        else if (!shi.liZhi || shi.shouPai.Count - 1 == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, yao, wei); });
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Select)
                    {
                        if (mingWei == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, QiaoShi.YaoDingYi.DaPai, wei); });
                            yy = y + margin;
                        }
                        else
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { OnClickShouPai(jia, QiaoShi.YaoDingYi.Wu, wei); });
                        }
                    }
                    if (shi.shouPai.Count - 1 == i && !shi.daPaiHou)
                    {
                        x += pw / 5;
                    }
                    if (sheDing.xuanShangYin)
                    {
                        shi.goShouPai[i].GetComponentInChildren<TextMeshProUGUI>().text = (shi.XuanShangPaiPanDing(shi.shouPai[i]) > 0) ? "▼" : "";
                    }
                    if (sheDing.shouPaiDianBiaoShi && chang.isZiJiaYaoDraw && !shi.daPaiHou)
                    {
                        shi.goShouPai[i].GetComponentInChildren<TextMeshProUGUI>().text = shi.shouPaiDian[i].ToString();
                    }
                }
                else
                {
                    if (!(shi.zhongLiao && shi.xingTing) && yao != QiaoShi.YaoDingYi.HeLe && yao != QiaoShi.YaoDingYi.JiuZhongJiuPai && yao != QiaoShi.YaoDingYi.CuHe && p != 0xff)
                    {
                        if (!sheDing.xiangShouPaiOpen && !shi.kaiLiZhi)
                        {
                            p = 0x00;
                        }
                        shi.goShouPai[i].transform.SetSiblingIndex(0);
                    }
                }

                if (p != 0xff)
                {
                    if (p == 0x00 && !shi.player && !(shi.zhongLiao && !shi.xingTing))
                    {
                        DrawJiXiePai(ref shi.goShouPai[i], Cal(x, yy, shi.playOrder), 90 * GetDrawOrder(shi.playOrder));
                    }
                    else
                    {
                        float yyy = yy;
                        if (orientation != ScreenOrientation.Portrait && shi.player)
                        {
                            yyy += ph * 0.45f;
                        }
                        DrawPai(ref shi.goShouPai[i], p, Cal(x, yyy, shi.playOrder), 90 * GetDrawOrder(shi.playOrder));
                    }
                }
                x += pw;
                if (p == 0xff)
                {
                    ClearGameObject(ref shi.goShouPai[i]);
                }
            }

            // 副露牌
            x = paiWidth * 8f;
            if (shi.player)
            {
                x += paiWidth * 1.5f;
            }
            y = -(paiWidth * 3f + paiHeight * 4.1f);
            if (shi.player)
            {
                y -= paiHeight;
                if (orientation != ScreenOrientation.Portrait)
                {
                    x += paiWidth * 2f;
                    y = -Screen.safeArea.height / 2 + ph / 2;
                }
            }
            for (int i = 0; i < shi.fuLuPais.Count; i++)
            {
                FuLuPai fuLuPai = shi.fuLuPais[i];
                for (int j = fuLuPai.pais.Count - 1; j >= 0; j--)
                {
                    int p = fuLuPai.pais[j];
                    if (p == 0xff)
                    {
                        continue;
                    }
                    if (fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang && j == 3)
                    {
                        continue;
                    }
                    bool isMingPai = shi.MingPaiPanDing(fuLuPai.yao, fuLuPai.jia, j);
                    if (fuLuPai.yao == QiaoShi.YaoDingYi.AnGang && (j == 0 || j == 3))
                    {
                        p = 0x00;
                    }
                    shi.goFuLuPais[i].goFuLuPai[j] = Instantiate(goPai, goCanvas.transform);
                    shi.goFuLuPais[i].goFuLuPai[j].transform.SetSiblingIndex(2);
                    shi.goFuLuPais[i].goFuLuPai[j].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    if (fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang && isMingPai)
                    {
                        shi.goFuLuPais[i].goFuLuPai[3] = Instantiate(goPai, goCanvas.transform);
                        shi.goFuLuPais[i].goFuLuPai[3].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                        shi.goFuLuPais[i].goFuLuPai[3].transform.SetSiblingIndex(0);
                    }
                    if (isMingPai)
                    {
                        x -= paiHeight / 2;
                        DrawPai(ref shi.goFuLuPais[i].goFuLuPai[j], p, Cal(x, y - (paiHeight - paiWidth) / 2, shi.playOrder), 90);
                        if (fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang)
                        {
                            DrawPai(ref shi.goFuLuPais[i].goFuLuPai[3], p, Cal(x, y - (paiHeight - paiWidth) / 2 + paiWidth, shi.playOrder), 90);
                        }
                        x -= paiHeight / 2;
                    }
                    else
                    {
                        x -= paiWidth / 2;
                        DrawPai(ref shi.goFuLuPais[i].goFuLuPai[j], p, Cal(x, y, shi.playOrder), 0);
                        x -= paiWidth / 2;
                    }
                }
            }
        }

        // 牌クリック
        private void OnClickShouPai(int jia, QiaoShi.YaoDingYi yao, int xuanZe)
        {
            QiaoShi shi = qiaoShis[jia];
            if (yao == QiaoShi.YaoDingYi.LiZhi || yao == QiaoShi.YaoDingYi.KaiLiZhi || yao == QiaoShi.YaoDingYi.AnGang || yao == QiaoShi.YaoDingYi.JiaGang)
            {
                shi.ziJiaYao = yao;
                shi.ziJiaXuanZe = xuanZe;
            }
            if (yao == QiaoShi.YaoDingYi.Bing || yao == QiaoShi.YaoDingYi.Chi)
            {
                shi.taJiaYao = yao;
                shi.taJiaXuanZe = xuanZe;
            }
            if (yao == QiaoShi.YaoDingYi.Wu)
            {
                if (sheDing.daPaiFangFa == 0)
                {
                    shi.ziJiaYao = QiaoShi.YaoDingYi.Wu;
                    shi.ziJiaXuanZe = xuanZe;
                }
                else
                {
                    DrawShouPai(jia, QiaoShi.YaoDingYi.Select, xuanZe);
                    shi.DaiPaiXiangTingShuJiSuan(xuanZe);
                    DrawDaiPai(jia);
                    return;
                }
            }
            if (yao == QiaoShi.YaoDingYi.DaPai)
            {
                shi.ziJiaYao = QiaoShi.YaoDingYi.Wu;
                shi.ziJiaXuanZe = xuanZe;
                DrawShouPai(jia, QiaoShi.YaoDingYi.Clear, 0);
            }
            chang.keyPress = true;
        }

        // 【描画】待牌
        private void DrawDaiPai(int jia)
        {
            QiaoShi shi = qiaoShis[jia];
            // 待牌
            if (!shi.player)
            {
                return;
            }
            ClearGameObject(ref shi.goDaiPai);
            ClearGameObject(ref shi.goCanPaiShu);
            ClearGameObject(ref shi.goXiangTingShu);

            float x = -(paiWidth * 8.5f);
            float y = -(paiHeight * 7.8f);
            if (orientation != ScreenOrientation.Portrait)
            {
                x = paiWidth * 11f;
                y = -(paiHeight * 4f);
            }

            if (sheDing.daiPaiBiaoShi)
            {
                for (int i = 0; i < shi.daiPai.Count; i++)
                {
                    int p = shi.daiPai[i] & QiaoShi.QIAO_PAI;
                    if (p == 0xff)
                    {
                        ClearGameObject(ref shi.goDaiPai[i]);
                    }
                    else
                    {
                        DrawPai(ref shi.goDaiPai[i], p, Cal(x, y, shi.playOrder), 0);
                        DrawText(ref shi.goCanPaiShu[i], pai.CanShu(shi.gongKaiPaiShu[p]).ToString(), Cal(x, y + paiWidth * 1.2f, shi.playOrder), 0, 17);
                    }
                    x += paiWidth;
                }
            }

            if (sheDing.xiangTingShuBiaoShi)
            {
                // 向聴数計算
                x = -(paiWidth * 7f);
                if (orientation != ScreenOrientation.Portrait)
                {
                    x = paiWidth * 13f;
                }
                if (shi.daiPai.Count == 0)
                {
                    if (shi.xiangTingShu > 0)
                    {
                        DrawText(ref shi.goXiangTingShu, $"{shi.xiangTingShu}シャンテン", Cal(x, y, shi.playOrder), 0, 18);
                    }
                }
            }
        }

        // 【描画】捨牌
        private void DrawShePai(int jia)
        {
            QiaoShi shi = qiaoShis[jia];

            ClearGameObject(ref shi.goShePai);
            int shePaiEnter = 6;
            float shePaiLeft = 2.5f;
            if (qiaoShis.Count == 2)
            {
                shePaiLeft = 8.5f;
                shePaiEnter = 18;
            }
            float x = -(paiWidth * shePaiLeft);
            float y = -(paiHeight * 2.6f);

            int shu = 0;
            bool isDrawLizhi = !shi.liZhi;
            float dark = 0.8f;
            int playOrder = shi.playOrder;
            if (qiaoShis.Count == 2 && playOrder == 1)
            {
                playOrder = 2;
            }
            int index = 0;
            foreach (ShePai shePai in shi.shePais)
            {
                if (shePai.yao == QiaoShi.YaoDingYi.Wu || shePai.yao == QiaoShi.YaoDingYi.LiZhi || shePai.yao == QiaoShi.YaoDingYi.RongHe)
                {
                    shu++;

                    shi.goShePai[index] = Instantiate(goPai, goCanvas.transform);
                    shi.goShePai[index].transform.SetSiblingIndex(3);
                    if (shePai.yao == QiaoShi.YaoDingYi.LiZhi || (!isDrawLizhi && shi.liZhiWei < index))
                    {
                        x += (paiHeight - paiWidth) / 2;
                        shi.goShePai[index].transform.Rotate(0, 0, 90);
                        isDrawLizhi = true;
                        Button b = shi.goShePai[index];
                        DrawPai(ref b, shePai.pai, Cal(x, y, playOrder), 90 * GetDrawOrder(playOrder));
                        x += (paiHeight - paiWidth) / 2;
                    }
                    else
                    {
                        Button b = shi.goShePai[index];
                        DrawPai(ref b, shePai.pai, Cal(x, y, playOrder), 90 * GetDrawOrder(playOrder));
                    }
                    if (sheDing.ziMoQieBiaoShi && shePai.ziMoQie)
                    {
                        Image img = shi.goShePai[index].GetComponentInChildren<Image>();
                        Color c = img.color;
                        img.color = new Color(c.r * dark, c.g * dark, c.b * dark, c.a);
                    }
                    if (shu % shePaiEnter == 0)
                    {
                        y -= paiHeight;
                        x = -(paiWidth * shePaiLeft);
                    }
                    else
                    {
                        x += paiWidth;
                    }
                }
                index++;
            }
        }

        // 【描画】声
        private void DrawSheng(int jia, QiaoShi.YaoDingYi yao)
        {
            if (yao == QiaoShi.YaoDingYi.Wu || ((yao == QiaoShi.YaoDingYi.Chi || yao == QiaoShi.YaoDingYi.Bing || yao == QiaoShi.YaoDingYi.DaMingGang || yao == QiaoShi.YaoDingYi.RongHe) && chang.isTaJiaYaoDraw))
            {
                return;
            }

            // 栄和
            if (yao == QiaoShi.YaoDingYi.RongHe)
            {
                foreach (RongHeFan rongHeFan in chang.rongHeFans)
                {
                    DrawSheng(rongHeFan.fan, QiaoShi.YaoMing(QiaoShi.YaoDingYi.RongHe));
                }
                if (chang.rongHeFans.Count == 3)
                {
                    if (guiZe.tRongHe == 0)
                    {
                        DrawSheng(chang.mingFan, "ロン(頭ハネ)");
                    }
                    else if (guiZe.tRongHe >= 2)
                    {
                        DrawSheng(chang.rongHeFans[2].fan, "ロン(流局)");
                    }
                }
                else if (chang.rongHeFans.Count == 2)
                {
                    if (guiZe.wRongHe == 0)
                    {
                        DrawSheng(chang.mingFan, "ロン(頭ハネ)");
                    }
                }
                return;
            }

            string text = QiaoShi.YaoMing(yao);
            QiaoShi shi = qiaoShis[jia];
            if (yao == QiaoShi.YaoDingYi.LiZhi && shi.kaiLiZhi)
            {
                text = QiaoShi.YaoMing(QiaoShi.YaoDingYi.KaiLiZhi);
            }
            DrawSheng(jia, text);
        }
        private void DrawSheng(int jia, string text)
        {
            QiaoShi shi = qiaoShis[jia];
            if (goSheng[jia] == null)
            {
                goSheng[jia] = Instantiate(goSpeech, goCanvas.transform);
            }
            TextMeshProUGUI goText = goSheng[jia].GetComponentInChildren<TextMeshProUGUI>();
            goText.text = text;
            goText.fontSize = 23;
            goSheng[jia].transform.SetAsLastSibling();
            goSheng[jia].onClick.AddListener(() =>
            {
                ClearGameObject(ref goSheng[jia]);
            });
            goSheng[jia].transform.rotation = Quaternion.Euler(0, 0, 90 * GetDrawOrder(shi.playOrder));
            RectTransform rt = goSheng[jia].GetComponent<RectTransform>();
            rt.anchoredPosition = Cal(0, -(paiWidth * 3 + paiHeight * 2.3f), shi.playOrder);
            rt.sizeDelta = new Vector2(goText.preferredWidth + paiWidth / 2, rt.sizeDelta.y);
        }

        // 【描画】錯和
        private void DrawCuHe(int jia)
        {
            DrawSheng(jia, $"{QiaoShi.YaoMing(QiaoShi.YaoDingYi.CuHe)} {qiaoShis[chang.cuHeFan].cuHeSheng}");
            DrawShouPai(jia, QiaoShi.YaoDingYi.CuHe, 0);
        }

        // 【ゲーム】対局終了
        private IEnumerator DuiJuZhongLe()
        {
            chang.isDuiJuZhongLeDraw = true;
            yield return Pause(ForwardMode.FAST_FORWARD);

            chang.eventStatus = Event.YI_BIAO_SHI;
            duiJuZhongLeCoroutine = null;
            WriteData();
        }

        // 【描画】対局終了
        private void DrawDuiJuZhongLe()
        {
            ClearScreen();
            DrawDuiJu();
        }

        // 【ゲーム】役
        private IEnumerator YiBiaoShi()
        {
            chang.isBackDuiJuZhongLe = false;

            chang.yiBiaoShiFan = chang.heleFan;

            if (chang.rongHeFans.Count > 0)
            {
                foreach (RongHeFan rongHeFan in chang.rongHeFans)
                {
                    chang.isYiBiaoShiDraw = true;
                    chang.yiBiaoShiFan = rongHeFan.fan;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    if (chang.isBackDuiJuZhongLe)
                    {
                        break;
                    }
                }
            }
            else
            {
                chang.isYiBiaoShiDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);
            }

            chang.eventStatus = chang.isBackDuiJuZhongLe ? Event.DUI_JU_ZHONG_LE : Event.DIAN_BIAO_SHI;
            yiBiaoShiCoroutine = null;
            WriteData();
        }

        // 【描画】役表示
        private void DrawYiBiaoShi()
        {
            ClearScreen();
            DrawBackDuiJuZhongLe();
            DrawYi(chang.yiBiaoShiFan);
        }

        // 【描画】対局終了へ戻る
        private void DrawBackDuiJuZhongLe()
        {
            // 戻るボタン
            goBackDuiJuZhongLe = Instantiate(goBack, goCanvas.transform);
            goBackDuiJuZhongLe.onClick.AddListener(() =>
            {
                chang.isBackDuiJuZhongLe = true;
                chang.keyPress = true;
            });
            RectTransform rtBackDuiJuZhongLe = goBackDuiJuZhongLe.GetComponent<RectTransform>();
            rtBackDuiJuZhongLe.localScale *= scale.x;
            rtBackDuiJuZhongLe.anchorMin = new Vector2(0, 1);
            rtBackDuiJuZhongLe.anchorMax = new Vector2(0, 1);
            rtBackDuiJuZhongLe.pivot = new Vector2(0, 1);
            rtBackDuiJuZhongLe.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 1.5f));
        }

        // 【描画】役
        private void DrawYi(int jia)
        {
            float x;
            float y = paiWidth * 2.5f + paiHeight * 7.5f;
            if (jia >= 0)
            {
                DrawXuanShangPai(true);

                QiaoShi shi = qiaoShis[jia];

                // 名前
                x = 0f;
                y -= paiHeight * 3.5f;
                DrawText(ref goMingQian[jia], shi.mingQian, new Vector2(x, y), 0, 30);

                // 手牌
                if (!shi.liPaiDongZuo)
                {
                    int tp = shi.shouPai[^1];
                    shi.shouPai[^1] = 0xff;
                    shi.Sort();
                    shi.shouPai[^1] = tp;
                }

                float pw = paiWidth * PLAYER_PAI_SCALE;
                x = -(pw * 6.5f);
                y -= paiHeight * 1.5f;
                float ph = paiHeight * PLAYER_PAI_SCALE;
                for (int i = 0; i < shi.shouPai.Count; i++)
                {
                    int sp = shi.shouPai[i];
                    shi.goShouPai[i] = Instantiate(goPai, goCanvas.transform);
                    shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE;
                    if (sp != 0xff)
                    {
                        DrawPai(ref shi.goShouPai[i], sp, new Vector2(x, y), 0);
                    }
                    x += pw;
                    if (sp == 0xff)
                    {
                        ClearGameObject(ref shi.goShouPai[i]);
                    }
                }

                x = paiWidth * 9f;
                y -= ph * 1.1f;
                for (int i = 0; i < shi.fuLuPais.Count; i++)
                {
                    FuLuPai fuLuPai = shi.fuLuPais[i];
                    for (int j = fuLuPai.pais.Count - 1; j >= 0; j--)
                    {
                        int p = fuLuPai.pais[j];
                        if (p == 0xff)
                        {
                            continue;
                        }
                        if (fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang && j == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(fuLuPai.yao, fuLuPai.jia, j);
                        if (fuLuPai.yao == QiaoShi.YaoDingYi.AnGang && (j == 0 || j == 3))
                        {
                            p = 0x00;
                        }
                        shi.goFuLuPais[i].goFuLuPai[j] = Instantiate(goPai, goCanvas.transform);
                        shi.goFuLuPais[i].goFuLuPai[j].transform.localScale *= PLAYER_PAI_SCALE;
                        if (fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang && isMingPai)
                        {
                            shi.goFuLuPais[i].goFuLuPai[3] = Instantiate(goPai, goCanvas.transform);
                            shi.goFuLuPais[i].goFuLuPai[3].transform.localScale *= PLAYER_PAI_SCALE;
                        }
                        if (isMingPai)
                        {
                            x -= ph / 2;
                            DrawPai(ref shi.goFuLuPais[i].goFuLuPai[j], p, new Vector2(x, y - (ph - pw) / 2), 90);
                            if (fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang)
                            {
                                DrawPai(ref shi.goFuLuPais[i].goFuLuPai[3], p, new Vector2(x, y - (ph - pw) / 2 + pw), 90);
                            }
                            x -= ph / 2;
                        }
                        else
                        {
                            x -= pw / 2;
                            DrawPai(ref shi.goFuLuPais[i].goFuLuPai[j], p, new Vector2(x, y), 0);
                            x -= pw / 2;
                        }
                    }
                }

                string hele = shi.fanShuJi >= 13 ? QiaoShi.DeDianYi[13] : QiaoShi.DeDianYi[shi.fanShuJi];
                if (hele == "")
                {
                    if ((shi.fu >= 40 && shi.fanShuJi >= 4) || (shi.fu >= 70 && shi.fanShuJi >= 3))
                    {
                        hele = QiaoShi.DeDianYi[5];
                    }
                }
                string value = (shi.fu > 0 ? shi.fu + "符" : "") + (shi.yiMan ? "役満 " : shi.fanShuJi + "飜 ") + hele + " " + shi.heLeDian + "点";
                y -= paiHeight * 1.1f;
                DrawText(ref goFu, value, new Vector2(0, y), 0, 30);
                int index = 0;
                foreach (YiFan yiFan in shi.yiFans)
                {
                    y -= paiHeight;
                    goYi[index] = Instantiate(goText, goCanvas.transform);
                    string ming = shi.yiMan ? QiaoShi.YiManMing[(QiaoShi.YiManDingYi)yiFan.yi] : QiaoShi.YiMing[(QiaoShi.YiDingYi)yiFan.yi];
                    DrawText(ref goYi[index], ming, new Vector2(0, y), 0, 25, TextAlignmentOptions.Left, 7);
                    goFanShu[index] = Instantiate(goText, goCanvas.transform);
                    DrawText(ref goFanShu[index], yiFan.fanShu.ToString(), new Vector2(paiWidth * 3.3f, y), 0, 25, TextAlignmentOptions.Right, 2);
                    index++;
                }
            }
        }

        // 【ゲーム】点
        private IEnumerator DianBiaoShi()
        {
            chang.isDianBiaoShiDraw = true;
            chang.DianJiSuan();

            // 記録 役数・役満数
            if (chang.rongHeFans.Count > 0)
            {
                foreach (RongHeFan rongHeFan in chang.rongHeFans)
                {
                    QiaoShi shi = qiaoShis[rongHeFan.fan];
                    foreach (YiFan yiFan in shi.yiFans)
                    {
                        if (shi.yiMan)
                        {
                            ResizeYiManShu(shi);
                            shi.jiLu.yiManShu[yiFan.yi]++;
                        }
                        else
                        {
                            ResizeYiShu(shi);
                            shi.jiLu.yiShu[yiFan.yi]++;
                        }
                    }
                }
            }
            else
            {
                if (chang.heleFan >= 0)
                {
                    QiaoShi shi = qiaoShis[chang.heleFan];
                    foreach (YiFan yiFan in shi.yiFans)
                    {
                        if (shi.yiMan)
                        {
                            ResizeYiManShu(shi);
                            shi.jiLu.yiManShu[yiFan.yi]++;
                        }
                        else
                        {
                            ResizeYiShu(shi);
                            shi.jiLu.yiShu[yiFan.yi]++;
                        }
                    }
                }
            }

            int max = 0;
            List<Transition> ziJiaList = new();
            List<Transition> taJiaList = new();
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                if (shi.shouQu > 0)
                {
                    if (shi.TransitionZiJiaList != null)
                    {
                        shi.SetTransitionZiJiaReward(shi.shouQu);
                        ziJiaList.AddRange(shi.TransitionZiJiaList);
                    }
                    if (shi.TransitionTaJiaList != null)
                    {
                        taJiaList.AddRange(shi.TransitionTaJiaList);
                    }
                }

                if (Math.Abs(shi.shouQu) > max)
                {
                    max = Math.Abs(shi.shouQu);
                }
            }

            // 学習データ保存
            if (sheDing.learningData)
            {
                string dirPath = Path.Combine(Application.persistentDataPath, "Transition");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
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
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                shi.TransitionZiJiaList = null;
                shi.TransitionTaJiaList = null;
            }

            if (forwardMode > ForwardMode.NORMAL)
            {
                chang.keyPress = true;
            }
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                shi.shuBiao = shi.dianBang;
            }
            for (int shu = 0; shu <= max; shu += 100)
            {
                for (int i = 0; i < qiaoShis.Count; i++)
                {
                    int jia = (chang.qin + i) % qiaoShis.Count;
                    QiaoShi shi = qiaoShis[jia];
                    if (shi.shouQu > 0)
                    {
                        if (shi.shouQu - shu >= 0)
                        {
                            shi.shuBiao = shi.dianBang - shi.shouQu + shu;
                        }
                    }
                    else if (shi.shouQu < 0)
                    {
                        if (shi.shouQu + shu <= 0)
                        {
                            shi.shuBiao = shi.dianBang - shi.shouQu - shu;
                        }
                    }
                }

                chang.isDianBiaoShiDraw = true;
                if (chang.keyPress)
                {
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }

            for (int i = 0; i < qiaoShis.Count; i++)
            {
                int jia = (chang.qin + i) % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                shi.shuBiao = shi.dianBang;
            }
            chang.isDianBiaoShiDraw = true;

            // 記録の書込
            foreach (QiaoShi shi in qiaoShis)
            {
                // 記録 対局数
                shi.jiLu.duiJuShu++;
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(Path.Combine(directory, $"{shi.mingQian}.json"), JsonUtility.ToJson(shi.jiLu));
            }

            yield return Pause(ForwardMode.FAST_FORWARD);

            if (chang.tingPaiLianZhuang == Zhuang.LIAN_ZHUANG)
            {
                chang.LianZhuang();
            }
            else
            {
                chang.LunZhuang();
            }

            bool isQinTop = false;
            if (chang.changFeng == 0x32 && chang.ju == qiaoShis.Count)
            {
                QiaoShi qinShi = qiaoShis[chang.qin];
                if (qinShi.ziJiaYao == QiaoShi.YaoDingYi.ZiMo || qinShi.taJiaYao == QiaoShi.YaoDingYi.RongHe)
                {
                    // 親の和了
                    int maxDian = 0;
                    for (int i = 0; i < qiaoShis.Count; i++)
                    {
                        int jia = (chang.qin + i) % qiaoShis.Count;
                        QiaoShi shi = qiaoShis[jia];
                        if (maxDian < shi.dianBang)
                        {
                            maxDian = shi.dianBang;
                        }
                    }
                    if (maxDian == qinShi.dianBang)
                    {
                        // 親の和了辞め
                        isQinTop = true;
                    }
                }
            }

            // 点表示
            int zhan = guiZe.banZhuang ? 0x32 : 0x31;
            if (chang.changFeng > zhan || (guiZe.xiang && chang.XiangPanDing()) || isQinTop)
            {
                chang.eventStatus = Event.ZHUANG_ZHONG_LE;
            }
            else
            {
                chang.eventStatus = Event.PEI_PAI;
            }

            chang.DianGiSuanGongTuo();

            dianBiaoShiCoroutine = null;
            WriteData();
        }

        // 【描画】点表示
        private void DrawDianBiaoShi()
        {
            ClearScreen();
            DrawJuFrame();
            DrawMingQian();

            float x = 0;
            float y = -(paiHeight * 4);
            for (int i = 0; i < qiaoShis.Count; i++)
            {
                QiaoShi shi = qiaoShis[i];
                // 受取
                string shouQuGongTuo = $"{(shi.shouQuGongTuo > 0 ? "+" : "")}{(shi.shouQuGongTuo == 0 ? "" : shi.shouQuGongTuo)}";
                string shouQu = $"{(shi.shouQu - shi.shouQuGongTuo > 0 ? "+" : "")}{(shi.shouQu - shi.shouQuGongTuo == 0 ? "" : (shi.shouQu - shi.shouQuGongTuo))}";

                int len = shouQuGongTuo.Length;
                if (len < shouQu.Length)
                {
                    len = shouQu.Length;
                }
                DrawText(ref goShouQuGongTuo[i], shouQuGongTuo, Cal(0, -(paiHeight * 2.5f), shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 20, TextAlignmentOptions.Right, len - 1);
                DrawText(ref goShouQu[i], shouQu, Cal(0, -(paiHeight * 3f), shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 20, TextAlignmentOptions.Right, len - 1);
            }

            for (int i = 0; i < qiaoShis.Count; i++)
            {
                QiaoShi shi = qiaoShis[i];
                DrawText(ref goDianBang[i], shi.shuBiao.ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 30);
            }
        }

        // 【ゲーム】荘終了
        private IEnumerator ZhuangZhong()
        {
            // 得点設定
            SettingScore();

            chang.isZhuangZhongLeDraw = true;
            yield return Pause(ForwardMode.FAST_FORWARD);

            // 記録の書込
            List<(int dian, QiaoShi shi)> shunWei = new();
            for (int i = chang.qiaJia; i < chang.qiaJia + qiaoShis.Count; i++)
            {
                int jia = i % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                shunWei.Add((shi.dianBang, shi));
            }
            shunWei.Sort((x, y) => y.dian.CompareTo(x.dian));
            for (int i = 0; i < shunWei.Count; i++)
            {
                QiaoShi shi = shunWei[i].shi;
                // 半荘数
                shi.jiLu.banZhuangShu++;
                // 集計点
                shi.jiLu.jiJiDian += shi.jiJiDian;
                // 順位
                switch (i)
                {
                    case 0:
                        shi.jiLu.shunWei1++;
                        break;
                    case 1:
                        shi.jiLu.shunWei2++;
                        break;
                    case 2:
                        shi.jiLu.shunWei3++;
                        break;
                    default:
                        shi.jiLu.shunWei4++;
                        break;
                }
                string directory = Path.Combine(Application.persistentDataPath, SETTING_PLAYER_DATA_DIR_NAME);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(Path.Combine(directory, $"{shi.mingQian}.json"), JsonUtility.ToJson(shi.jiLu));
            }

            chang.banZhuangShu++;

            Debug.Log($"{chang.banZhuangShu}回戦");
            for (int i = 0; i < shunWei.Count; i++)
            {
                QiaoShi shi = shunWei[i].shi;
                Debug.Log($" {i + 1}位 {shi.mingQian}({shi.jiJiDian})");
            }
            chang.eventStatus = Event.QIAO_SHI_XUAN_ZE;

            zhuangZhongLeCoroutine = null;
            WriteData();
        }

        // 得点設定
        private void SettingScore()
        {
            int geHe = 0;
            int maxDian = 0;
            int top = 0;
            for (int i = chang.qiaJia; i < chang.qiaJia + qiaoShis.Count; i++)
            {
                int jia = i % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];

                if (maxDian < shi.dianBang)
                {
                    maxDian = shi.dianBang;
                    top = jia;
                }
                int deDian = shi.dianBang / 1000;
                deDian -= guiZe.fanDian / 1000;
                geHe += deDian;
                shi.JiJiDianJiSuan(deDian);
            }
            qiaoShis[top].JiJiDianJiSuan(qiaoShis[top].jiJiDian - geHe);
        }

        // 【描画】荘終了
        private void DrawZhuangZhongLe()
        {
            ClearScreen();
            DrawZhuangZhong();
        }

        // 【描画】荘終了
        private void DrawZhuangZhong()
        {
            float y = paiHeight * 3f;
            int maxMingQian = 0;
            int maxDianBang = 0;
            int maxDeDian = 0;
            foreach (QiaoShi shi in qiaoShis)
            {
                if (maxMingQian < shi.mingQian.Length)
                {
                    maxMingQian = shi.mingQian.Length;
                }
                if (maxDianBang < shi.dianBang.ToString().Length)
                {
                    maxDianBang = shi.dianBang.ToString().Length;
                }
                int len = shi.jiJiDian.ToString().Length;
                if (shi.jiJiDian > 0)
                {
                    len++;
                }
                if (maxDeDian < len)
                {
                    maxDeDian = len;
                }
            }

            List<(int dian, QiaoShi shi)> shunWei = new();
            for (int i = chang.qiaJia; i < chang.qiaJia + qiaoShis.Count; i++)
            {
                int jia = i % qiaoShis.Count;
                QiaoShi shi = qiaoShis[jia];
                shunWei.Add((shi.dianBang, shi));
            }
            shunWei.Sort((x, y) => y.dian.CompareTo(x.dian));

            for (int i = 0; i < shunWei.Count; i++)
            {
                QiaoShi shi = shunWei[i].shi;
                string dianBang = shi.dianBang.ToString();
                for (int j = dianBang.Length; j < maxDianBang; j++)
                {
                    dianBang = $" {dianBang}";
                }
                string deDian = $"({(shi.jiJiDian > 0 ? "+" : "")}{shi.jiJiDian})";
                for (int j = deDian.Length; j < maxDeDian; j++)
                {
                    deDian = $" {deDian}";
                }

                DrawText(ref goMingQian[i], shi.mingQian, new Vector2(-(paiWidth * 5f), y), 0, 30, TextAlignmentOptions.Left, quiaoShiButtonMaxLen);
                DrawText(ref goDianBang[i], dianBang, new Vector2(paiWidth * 3f, y), 0, 30, TextAlignmentOptions.Right, 6);
                DrawText(ref goShouQu[i], deDian, new Vector2(paiWidth * 7f, y), 0, 25, TextAlignmentOptions.Right, 4);
                y -= paiHeight * 2;
            }
        }

        // 【描画】テキスト
        private void DrawText(ref TextMeshProUGUI obj, string value, Vector2 xy, int quaternion, int fontSize, TextAlignmentOptions align = TextAlignmentOptions.Center, int len = -1)
        {
            if (obj == null)
            {
                obj = Instantiate(goText, goCanvas.transform);
            }
            obj.text = value;
            obj.fontSize = fontSize;
            obj.alignment = align;
            RectTransform rt = obj.rectTransform;
            rt.rotation = Quaternion.Euler(0, 0, quaternion);
            rt.anchoredPosition = xy;
            if (len == -1)
            {
                len = value.Length;
            }
            if (len == 0)
            {
                rt.sizeDelta = new Vector2(paiWidth, obj.preferredHeight);
            }
            else
            {
                rt.sizeDelta = new Vector2(obj.preferredWidth / value.Length * (len + 1), obj.preferredHeight);
            }
            rt.SetSiblingIndex(1);
        }

        // 【描画】フレームテキスト
        void DrawFrame(ref Image obj, string value, Vector2 xy, float quaternion, int fontSize, Color backgroundColor, Color foreColor, int len = -1)
        {
            if (obj == null)
            {
                obj = Instantiate(goFrame, goCanvas.transform);
            }
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = fontSize;
            text.color = foreColor;
            text.rectTransform.SetSiblingIndex(2);

            obj.color = backgroundColor;
            RectTransform rt = obj.rectTransform;
            rt.anchoredPosition = xy;
            rt.rotation = Quaternion.Euler(0, 0, quaternion);
            if (len == -1)
            {
                len = value.Length;
            }
            rt.sizeDelta = new Vector2(text.preferredWidth / value.Length * (len + 1), text.preferredHeight);
            rt.SetSiblingIndex(1);
        }

        // 【描画】ボタン
        private void DrawButton(ref Button obj, string value, Vector2 xy, int len = -1)
        {
            if (obj == null)
            {
                obj = Instantiate(goButton, goCanvas.transform);
            }
            obj.GetComponentInChildren<TextMeshProUGUI>().text = value;
            obj.GetComponent<RectTransform>().anchoredPosition = xy;

            RectTransform rt = obj.GetComponent<RectTransform>();
            TextMeshProUGUI text = obj.GetComponentInChildren<TextMeshProUGUI>();
            if (len == -1)
            {
                len = value.Length;
            }
            rt.sizeDelta = new Vector2(text.preferredWidth / value.Length * (len + 1), rt.sizeDelta.y);
        }

        // 【描画】牌
        private void DrawPai(ref Button obj, int p, Vector2 xy, int quaternion)
        {
            if (obj == null)
            {
                obj = Instantiate(goPai, goCanvas.transform);
            }
            obj.image.sprite = goPais[p].GetComponent<SpriteRenderer>().sprite;
            obj.transform.Rotate(0, 0, quaternion);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = xy;
        }

        // 【描画】相手(機械)牌
        private void DrawJiXiePai(ref Button obj, Vector2 xy, int quaternion)
        {
            obj.image.sprite = goJiXieShouPai;
            obj.transform.Rotate(0, 0, quaternion);
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, -obj.transform.localScale.y, obj.transform.localScale.z);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y * 0.5f);
            rt.anchoredPosition = xy;
        }

        // 座標計算
        private Vector2 Cal(float x, float y, int order)
        {
            int drawOrder = GetDrawOrder(order);
            int plusMinus = drawOrder == 0 || drawOrder == 3 ? 1 : -1;
            float xx = (drawOrder % 2 == 0 ? x : y) * plusMinus;
            float yy = (drawOrder % 2 == 0 ? y : -x) * plusMinus;

            return new Vector2(xx, yy);
        }

        private int GetDrawOrder(int order)
        {
            if (qiaoShis.Count == 3 && order == 2)
            {
                return 3;
            }
            if (qiaoShis.Count == 2 && order == 1)
            {
                return 2;
            }
            return order;
        }

        // 画面クリア
        private void ClearScreen()
        {
            if (goMingWu != null)
            {
                goMingWu.gameObject.SetActive(false);
            }
            if (goDianCha != null)
            {
                goDianCha.gameObject.SetActive(false);
            }

            ClearGameObject(ref goBackDuiJuZhongLe);
            ClearGameObject(ref goTitle);
            ClearGameObject(ref goStart);
            ClearGameObject(ref goJuFrame);
            ClearGameObject(ref goJu);
            ClearGameObject(ref goQiJia);
            ClearGameObject(ref goBenChang);
            ClearGameObject(ref goBenChangText);
            ClearGameObject(ref goGongTou);
            ClearGameObject(ref goGongTouText);
            ClearGameObject(ref goSai1);
            ClearGameObject(ref goSai2);

            ClearGameObject(ref goQiaoShi);
            ClearGameObject(ref goRandom);
            ClearGameObject(ref goPlayerNoExists);

            ClearGameObject(ref goLeft);
            ClearGameObject(ref goRight);
            ClearGameObject(ref goSelect);

            ClearGameObject(ref pai.goXuanShangPai);
            ClearGameObject(ref pai.goLiXuanShangPai);

            foreach (QiaoShi shi in qiaoShis)
            {
                if (shi == null)
                {
                    continue;
                }
                ClearGameObject(ref shi.goShouPai);
                for (int j = 0; j < shi.goFuLuPais.Length; j++)
                {
                    ClearGameObject(ref shi.goFuLuPais[j]);
                }
                ClearGameObject(ref shi.goShePai);
                ClearGameObject(ref shi.goDaiPai);
                ClearGameObject(ref shi.goCanPaiShu);
                ClearGameObject(ref shi.goXiangTingShu);
            }
            ClearGameObject(ref goZiMoShiLine);

            ClearGameObject(ref goMingQian);
            ClearGameObject(ref goFu);
            ClearGameObject(ref goCanShanPaiShu);
            ClearGameObject(ref goYi);
            ClearGameObject(ref goFanShu);
            ClearGameObject(ref goDianBang);
            ClearGameObject(ref goFeng);
            ClearGameObject(ref goLizhiBang);
            ClearGameObject(ref goShouQu);
            ClearGameObject(ref goShouQuGongTuo);
            ClearGameObject(ref goYao);
            ClearGameObject(ref goSheng);
        }

        private void ClearGameObject<T>(ref T go) where T : Component
        {
            if (go == null) return;
            Destroy(go.gameObject);
            go = null;
        }

        private void ClearGameObject<T>(ref T[] go) where T : Component
        {
            for (int i = 0; i < go.Length; i++)
            {
                ClearGameObject(ref go[i]);
            }
        }
        private void ClearGameObject(ref GoFuLuPai[] go)
        {
            for (int i = 0; i < go.Length; i++)
            {
                ClearGameObject(ref go[i]);
            }
        }
        private void ClearGameObject(ref GoFuLuPai go)
        {
            for (int i = 0; i < go.goFuLuPai.Length; i++)
            {
                ClearGameObject(ref go.goFuLuPai[i]);
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

                File.Move(Path.Combine(Application.persistentDataPath, GAME_DATA_DIR_NAME, $"{CHANG_FILE_NAME}.json"), Path.Combine(exceptionDirPath, $"{CHANG_FILE_NAME}.json"));
                File.Move(Path.Combine(Application.persistentDataPath, GAME_DATA_DIR_NAME, $"{PAI_FILE_NAME}.json"), Path.Combine(Application.persistentDataPath, exceptionDirName, $"{PAI_FILE_NAME}.json"));
                for (int i = 0; i < 4; i++)
                {
                    File.Move(Path.Combine(Application.persistentDataPath, GAME_DATA_DIR_NAME, $"{QIAO_SHI_FILE_NAME}{i}.json"), Path.Combine(Application.persistentDataPath, exceptionDirName, $"{QIAO_SHI_FILE_NAME}{i}.json"));
                }
            }
        }
    }
}

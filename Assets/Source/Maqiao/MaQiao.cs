using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

using Gongtong;
using Sikao;
using Sikao.Shi;
using Unity.VisualScripting;

namespace Maqiao
{
    // 麻雀
    internal class MaQiao : MonoBehaviour
    {
        // イベント
        private enum Event
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
        private enum Zhuang
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
        // 設定ファイル名
        private static readonly string SHE_DING_FILE_NAME = "SheDing";

        // 送りモード
        private enum ForwardMode
        {
            // 通常
            NORMAL = 0,
            // 局早送り
            FAST_FORWARD = 1,
            // ずっと早送り
            FOREVER_FAST_FORWARD = 2
        }
        // 送りモード
        private ForwardMode forwardMode = ForwardMode.NORMAL;
        // 相手牌オープン
        private bool shouPaiOpen = false;

        // 待ち時間
        private static float waitTime = WAIT_TIME;
        // 半荘数
        private static int banZhuangShu = 0;
        // 連荘
        private static Zhuang tingPaiLianZhuang;

        // 設定
        private SheDing sheDing;

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

        // イベント
        private static Event eventStatus;
        // キー状態
        private bool keyPress = false;

        // コルーチン処理中フラグ
        private bool isKaiShiCoroutine;
        private bool isQiaoShiXuanZeCoroutine;
        private bool isFollowQiaoShiXuanZeCoroutine;
        private bool isQinJueCoroutine;
        private bool isPeiPaiCoroutine;
        private bool isDuiJuCoroutine;
        private bool isDuiJuZhongLeCoroutine;
        private bool isYiBiaoShiCoroutine;
        private bool isDianBiaoShiCoroutine;
        private bool isZhuangZhongLeCoroutine;

        private bool isBackDuiJuZhongLe = false;
        private bool isDianCha = false;
        private bool isQuXiao = false;

        // Game Object
        private Button goScreen;
        private TextMeshProUGUI goText;
        private Image goFrame;
        private Image goLine;
        private Button goButton;
        private Button goFast;
        private Button goReproduction;
        private Button goRestart;
        private Button goShouPaiOpen;
        private Image goBenChang;
        private Image goGongTou;
        private GameObject[] goQiJias;
        private GameObject[] goSais;
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
        private Button goPai;
        private Button[] goSheng;
        private TextMeshProUGUI[] goYi;
        private TextMeshProUGUI[] goFanShu;
        private TextMeshProUGUI[] goDianBang;
        private Image[] goFeng;
        private TextMeshProUGUI[] goShouQu;
        private TextMeshProUGUI[] goShouQuGongTuo;
        private TextMeshProUGUI[] goMingQian;
        private Button[] goQiaoShi;
        private Button[] goYao;
        private Image[] goLizhiBang;
        private Image goDianBang100;
        private Image goDianBang1000;
        private Button goSpeech;
        private Image goQiJia;
        private Image goSai1;
        private Image goSai2;
        private Button goLeft;
        private Button goRight;
        private Button goSelect;
        private Button goMingWu;
        private Button goDianCha;
        private Button goLiZhiAuto;
        private Button goDaPaiFangFa;
        private Button goXuanShangYin;
        private Button goZiMoQieBiaoShi;
        private Button goDaiPaiBiaoShi;
        private Button goXiangTingShuBiaoShi;
        private Button goMingquXiao;
        private Button goSetting;
        private Button goScore;
        private Button goGuiZe;
        private Button goBack;
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

        private TextMeshProUGUI goJiJuMingQian;
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
            { "効率雀士", true },
            { "機械雀士", true },
            { HikitaMamoru.MING_QIAN, true },
            { UchidaKou.MING_QIAN, false },
            { SomeyaMei.MING_QIAN, false },
            { KouzuNaruto.MING_QIAN, false },
            { KouzuTorako.MING_QIAN, false },
            { YakudaJunji.MING_QIAN, false },
            { MenzenJunko.MING_QIAN, false },
        };
        // 雀士取得
        private QiaoShi GetQiaoShi(string mingQian)
        {
            return mingQian switch
            {
                "効率雀士" => new QiaoXiaoLu(mingQian),
                "機械雀士" => new QiaoJiXie(mingQian),
                HikitaMamoru.MING_QIAN => new HikitaMamoru(),
                SomeyaMei.MING_QIAN => new SomeyaMei(),
                UchidaKou.MING_QIAN => new UchidaKou(),
                KouzuNaruto.MING_QIAN => new KouzuNaruto(),
                KouzuTorako.MING_QIAN => new KouzuTorako(),
                YakudaJunji.MING_QIAN => new YakudaJunji(),
                MenzenJunko.MING_QIAN => new MenzenJunko(),
                _ => new QiaoJiXie(mingQian),
            };
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
            string filePath = Application.persistentDataPath + "/" + SHE_DING_FILE_NAME + ".json";
            if (File.Exists(filePath))
            {
                sheDing = JsonUtility.FromJson<SheDing>(File.ReadAllText(filePath));
            }
            else
            {
                sheDing = new SheDing();
            }

            SetGameObject();

            goQiaoShi = new Button[qiaoShiMingQian.Count];

            eventStatus = Event.KAI_SHI;
        }

        // Game Object設定
        private void SetGameObject()
        {
            // テキスト
            goText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
            goSheng = new Button[4];
            goYi = new TextMeshProUGUI[0x10];
            goFanShu = new TextMeshProUGUI[goYi.Length];
            goDianBang = new TextMeshProUGUI[4];
            goFeng = new Image[4];
            goShouQu = new TextMeshProUGUI[4];
            goShouQuGongTuo = new TextMeshProUGUI[4];
            goMingQian = new TextMeshProUGUI[4];
            // イメージ
            goFrame = GameObject.Find("Frame").GetComponent<Image>();
            goLine = GameObject.Find("Line").GetComponent<Image>();
            // スクリーン
            goScreen = GameObject.Find("Screen").GetComponent<Button>();
            goScreen.onClick.AddListener(delegate {
                OnClickScreen();
            });
            goScreen.transform.SetSiblingIndex(10);
            // ボタン
            goButton = GameObject.Find("Button").GetComponent<Button>();
            goPai = GameObject.Find("Pai").GetComponent<Button>();
            goYao = new Button[5];
            // 牌
            goPais = new GameObject[0xff];
            goPais[0x00] = GameObject.Find("0x00");
            foreach (int p in Pai.QiaoPai)
            {
                goPais[p] = GameObject.Find("0x" + p.ToString("x2"));
            }
            foreach (int p in Pai.ChiPaiDingYi)
            {
                int cp = p + QiaoShi.CHI_PAI;
                goPais[cp] = GameObject.Find("0x" + cp.ToString("x2"));
            }
            // プレイヤー以外の手牌画像作成
            Sprite sprite = GameObject.Find("0x00").GetComponent<SpriteRenderer>().sprite;
            int spriteHeight = sprite.texture.height;
            int spriteWidth = sprite.texture.width;
            int tHeight = spriteHeight / 7;
            Rect rect = new(0, spriteHeight - tHeight, spriteWidth, tHeight);
            Texture2D texture = new(spriteWidth, tHeight);
            Color[] pixels = sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            texture.SetPixels(pixels);
            texture.Apply();
            goJiXieShouPai = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // 点棒
            goDianBang100 = GameObject.Find("DianBang100").GetComponent<Image>();
            goDianBang1000 = GameObject.Find("DianBang1000").GetComponent<Image>();
            goLizhiBang = new Image[4];
            // 声
            goSpeech = GameObject.Find("Speech").GetComponent<Button>();
            // 起家マーク
            goQiJias = new GameObject[4];
            goQiJias[0] = GameObject.Find("QiJiaDong");
            goQiJias[1] = GameObject.Find("QiJiaNan");
            goQiJias[2] = GameObject.Find("QiJiaXi");
            goQiJias[3] = GameObject.Find("QiJiaBei");
            // サイコロ
            goSais = new GameObject[6];
            for (int i = 0; i < goSais.Length; i++)
            {
                goSais[i] = GameObject.Find("Sai" + (i + 1));
            }

            // scale設定
            SetScale();

            TextMeshProUGUI text = goText.GetComponent<TextMeshProUGUI>();
            RectTransform rtText = text.rectTransform;
            rtText.localScale *= scale.x;

            RectTransform rtButton = goButton.GetComponent<RectTransform>();
            TextMeshProUGUI buttonText = goButton.GetComponentInChildren<TextMeshProUGUI>();
            RectTransform rtFrame = goFrame.rectTransform;
            rtFrame.localScale *= scale.x;

            RectTransform rtLine = goLine.GetComponent<RectTransform>();
            rtLine.localScale *= scale.x;

            // ボタンのスケールとサイズを設定
            rtButton.localScale *= scale.x;
            rtButton.sizeDelta = new Vector2(buttonText.rectTransform.rect.size.x, rtButton.sizeDelta.y);
            goDianBang100.rectTransform.localScale *= scale.x;
            goDianBang1000.rectTransform.localScale *= scale.x;
            goSpeech.GetComponent<RectTransform>().localScale *= scale.x;
            for (int i = 0; i < goQiJias.Length; i++)
            {
                goQiJias[i].GetComponent<RectTransform>().localScale *= scale.x;
            }
            for (int i = 0; i < goSais.Length; i++)
            {
                goSais[i].GetComponent<RectTransform>().localScale *= scale.x;
            }

            goBack = GameObject.Find("Back").GetComponent<Button>();

            // 設定画面
            DrawSettingPanel();
            // 得点画面
            DrawScorePanel();
            // データ画面
            DrawDataScrollView();
            // ルール画面
            DrawGuiZeScrollView();
        }

        // scale設定
        private void SetScale()
        {
            float w = (Screen.safeArea.height < Screen.safeArea.width) ? Screen.safeArea.height : Screen.safeArea.width;
            RectTransform rtPai = goPai.GetComponent<RectTransform>();
            w = w / 20f / rtPai.rect.width;
            scale = new(w, w, w);
            goPai.transform.localScale = scale;
            paiWidth = rtPai.rect.width * scale.x;
            if (paiWidth * 5f > Math.Abs(Screen.safeArea.width - Screen.safeArea.height) / 2)
            {
                // 縦横の長さの差が牌5枚以内の場合、牌16枚分の大きさに縮小する
                w = paiWidth * 16f;
                w = w / 20f / rtPai.rect.width;
                scale = new(w, w, w);
                goPai.transform.localScale = scale;
                paiWidth = rtPai.rect.width * scale.x;
            }
            paiHeight = rtPai.rect.height * scale.y;

            quiaoShiButtonMaxLen = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                int len = kvp.Key.Length;
                if (len > quiaoShiButtonMaxLen)
                {
                    quiaoShiButtonMaxLen = len;
                }
            }
        }

        // 【描画】設定画面
        private void DrawSettingPanel()
        {
            // 設定パネル
            goSettingPanel = GameObject.Find("SettingPanel");
            EventTrigger etSettingPanel = goSettingPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eSettingPanel = new();
            eSettingPanel.eventID = EventTriggerType.PointerClick;
            eSettingPanel.callback.AddListener((eventData) => {
                goSettingPanel.SetActive(false);
            });
            etSettingPanel.triggers.Add(eSettingPanel);
            goSettingPanel.SetActive(false);
            // 設定ボタン
            goSetting = GameObject.Find("Setting").GetComponent<Button>();
            goSetting.onClick.AddListener(delegate {
                goSettingPanel.SetActive(true);
            });
            RectTransform rtSetting = goSetting.GetComponent<RectTransform>();
            rtSetting.localScale *= scale.x;
            rtSetting.anchorMin = new Vector2(1, 1);
            rtSetting.anchorMax = new Vector2(1, 1);
            rtSetting.pivot = new Vector2(1, 1);
            rtSetting.anchoredPosition = new Vector2(-(paiWidth * 0.5f), -(paiHeight * 0.5f));

            // 早送り
            goFast = goSettingPanel.transform.Find("Fast").GetComponent<Button>();
            goFast.onClick.AddListener(delegate {
                forwardMode = (ForwardMode)(((int)forwardMode + 1) % Enum.GetValues(typeof(ForwardMode)).Length);
                if (forwardMode == ForwardMode.NORMAL)
                {
                    forwardMode = ForwardMode.FAST_FORWARD;
                }
                Application.targetFrameRate = 0;
                waitTime = 0;
                if (keyPress == false)
                {
                    keyPress = true;
                }
                goSettingPanel.SetActive(false);
            });
            RectTransform rtFast = goFast.GetComponent<RectTransform>();
            rtFast.localScale *= scale.x;
            rtFast.anchorMin = new Vector2(1, 1);
            rtFast.anchorMax = new Vector2(1, 1);
            rtFast.pivot = new Vector2(1, 1);
            rtFast.anchoredPosition = new Vector2(-paiWidth, -(rtFast.sizeDelta.y * scale.x));
            // 再生
            goReproduction = goSettingPanel.transform.Find("Reproduction").GetComponent<Button>();
            goReproduction.onClick.AddListener(delegate {
                forwardMode = ForwardMode.NORMAL;
                Application.targetFrameRate = FRAME_RATE;
                waitTime = WAIT_TIME;
                goSettingPanel.SetActive(false);
            });
            RectTransform rtReproduction = goReproduction.GetComponent<RectTransform>();
            rtReproduction.localScale *= scale.x;
            rtReproduction.anchorMin = new Vector2(1, 1);
            rtReproduction.anchorMax = new Vector2(1, 1);
            rtReproduction.pivot = new Vector2(1, 1);
            rtReproduction.anchoredPosition = new Vector2(-(rtReproduction.sizeDelta.x * scale.x + (paiWidth * 1.5f)), -(rtReproduction.sizeDelta.y * scale.x));
            // リスタート
            goRestart = goSettingPanel.transform.Find("Restart").GetComponent<Button>();
            goRestart.onClick.AddListener(delegate {
                SceneManager.LoadScene("GameScene");
            });
            RectTransform rtRestart = goRestart.GetComponent<RectTransform>();
            rtRestart.localScale *= scale.x;
            rtRestart.anchorMin = new Vector2(0, 1);
            rtRestart.anchorMax = new Vector2(0, 1);
            rtRestart.pivot = new Vector2(0, 1);
            rtRestart.anchoredPosition = new Vector2(paiWidth, -(rtRestart.sizeDelta.y * scale.x));
            // 相手牌オープン
            goShouPaiOpen = Instantiate(goPai, goSettingPanel.transform);
            goShouPaiOpen.onClick.AddListener(delegate {
                shouPaiOpen = !shouPaiOpen;
                SwitchShouPaiOpenImage();
                switch (eventStatus)
                {
                    case Event.PEI_PAI:
                    case Event.DUI_JU:
                    case Event.DUI_JU_ZHONG_LE:
                        DrawDuiJu();
                        break;
                }
            });
            RectTransform rtShouPaiOpen = goShouPaiOpen.GetComponent<RectTransform>();
            rtShouPaiOpen.localScale *= scale.x * 0.7f;
            rtShouPaiOpen.anchorMin = new Vector2(0, 0);
            rtShouPaiOpen.anchorMax = new Vector2(0, 0);
            rtShouPaiOpen.pivot = new Vector2(0, 0);
            rtShouPaiOpen.anchoredPosition = new Vector2(paiWidth, paiHeight / 2f);
            SwitchShouPaiOpenImage();

            // オプションボタン
            DrawOption();

            EventTrigger etResetPanel = goSettingDialogPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eResetPanel = new();
            eResetPanel.eventID = EventTriggerType.PointerClick;
            eResetPanel.callback.AddListener((eventData) => {
                goSettingDialogPanel.SetActive(false);
            });
            etResetPanel.triggers.Add(eResetPanel);

            goSettingDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goSettingDialogPanel.transform);
            DrawText(ref message, "全ての設定をリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goSettingDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(delegate {
                ResetSheDing();
                goSettingDialogPanel.SetActive(false);
            });
            Button goNo = Instantiate(goButton, goSettingDialogPanel.transform);
            goNo.onClick.AddListener(delegate {
                goSettingDialogPanel.SetActive(false);
            });
            DrawButton(ref goNo, "いいえ", new Vector2(paiWidth * 3f, 0));
        }

        // 【描画】オプションボタン
        private void DrawOption()
        {
            // オプション
            float x = paiWidth * 4.5f;
            float y = paiHeight * 2.5f;
            float offset = paiHeight * 1.8f;
            int len = 7;
            // 1タップ打牌
            string[] labelDaPaiFangFa = new string[] { "選択して打牌", "１タップ打牌", "２タップ打牌" };
            ClearGameObject(ref goDaPaiFangFa);
            goDaPaiFangFa = Instantiate(goButton, goSettingPanel.transform);
            goDaPaiFangFa.onClick.AddListener(delegate {
                sheDing.daPaiFangFa = (SheDing.DaPaiFangFa)((int)(sheDing.daPaiFangFa + 1) % Enum.GetValues(typeof(SheDing.DaPaiFangFa)).Length);
                goDaPaiFangFa.GetComponentInChildren<TextMeshProUGUI>().text = labelDaPaiFangFa[(int)sheDing.daPaiFangFa];
                WriteSheDing();
            });
            DrawButton(ref goDaPaiFangFa, labelDaPaiFangFa[(int)sheDing.daPaiFangFa], new Vector2(-x, y), len);
            // 立直後 自動・手動
            string[] labelLiZhiAuto = new string[] { "立直後自動打牌", "立直後手動打牌" };
            ClearGameObject(ref goLiZhiAuto);
            goLiZhiAuto = Instantiate(goButton, goSettingPanel.transform);
            goLiZhiAuto.onClick.AddListener(delegate {
                sheDing.liZhiAuto = !sheDing.liZhiAuto;
                goLiZhiAuto.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.liZhiAuto ? labelLiZhiAuto[0] : labelLiZhiAuto[1];
                WriteSheDing();
            });
            DrawButton(ref goLiZhiAuto, sheDing.liZhiAuto ? labelLiZhiAuto[0] : labelLiZhiAuto[1], new Vector2(x, y), len);
            y -= offset;
            // ドラマーク
            string[] labelXuanShangYin = new string[] { "ドラマーク有", "ドラマーク無" };
            ClearGameObject(ref goXuanShangYin);
            goXuanShangYin = Instantiate(goButton, goSettingPanel.transform);
            goXuanShangYin.onClick.AddListener(delegate {
                sheDing.xuanShangYin = !sheDing.xuanShangYin;
                goXuanShangYin.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.xuanShangYin ? labelXuanShangYin[0] : labelXuanShangYin[1];
                WriteSheDing();
            });
            DrawButton(ref goXuanShangYin, sheDing.xuanShangYin ? labelXuanShangYin[0] : labelXuanShangYin[1], new Vector2(-x, y), len);
            // ツモ切表示有り・無し
            string[] labelZiMoQieBiaoShi = new string[] { "ツモ切表示有", "ツモ切表示無" };
            ClearGameObject(ref goZiMoQieBiaoShi);
            goZiMoQieBiaoShi = Instantiate(goButton, goSettingPanel.transform);
            goZiMoQieBiaoShi.onClick.AddListener(delegate {
                sheDing.ziMoQieBiaoShi = !sheDing.ziMoQieBiaoShi;
                goZiMoQieBiaoShi.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.ziMoQieBiaoShi ? labelZiMoQieBiaoShi[0] : labelZiMoQieBiaoShi[1];
                WriteSheDing();
            });
            DrawButton(ref goZiMoQieBiaoShi, sheDing.ziMoQieBiaoShi ? labelZiMoQieBiaoShi[0] : labelZiMoQieBiaoShi[1], new Vector2(x, y), len);
            y -= offset;
            // 待牌表示有り・無し
            string[] labelDaiPaiBiaoShi = new string[] { "待牌表示有", "待牌表示無" };
            ClearGameObject(ref goDaiPaiBiaoShi);
            goDaiPaiBiaoShi = Instantiate(goButton, goSettingPanel.transform);
            goDaiPaiBiaoShi.onClick.AddListener(delegate {
                sheDing.daiPaiBiaoShi = !sheDing.daiPaiBiaoShi;
                goDaiPaiBiaoShi.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.daiPaiBiaoShi ? labelDaiPaiBiaoShi[0] : labelDaiPaiBiaoShi[1];
                WriteSheDing();
            });
            DrawButton(ref goDaiPaiBiaoShi, sheDing.daiPaiBiaoShi ? labelDaiPaiBiaoShi[0] : labelDaiPaiBiaoShi[1], new Vector2(-x, y), len);
            // 向聴数表示有り・無し
            string[] labelXiangTingShuBiaoShi = new string[] { "向聴数表示有", "向聴数表示無" };
            ClearGameObject(ref goXiangTingShuBiaoShi);
            goXiangTingShuBiaoShi = Instantiate(goButton, goSettingPanel.transform);
            goXiangTingShuBiaoShi.onClick.AddListener(delegate {
                sheDing.xiangTingShuBiaoShi = !sheDing.xiangTingShuBiaoShi;
                goXiangTingShuBiaoShi.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.xiangTingShuBiaoShi ? labelXiangTingShuBiaoShi[0] : labelXiangTingShuBiaoShi[1];
                WriteSheDing();
            });
            DrawButton(ref goXiangTingShuBiaoShi, sheDing.xiangTingShuBiaoShi ? labelXiangTingShuBiaoShi[0] : labelXiangTingShuBiaoShi[1], new Vector2(x, y), len);
            y -= offset;
            // 鳴きの取消
            string[] labelMingQuXiao = new string[] { "鳴パスはボタン", "鳴パスはタップ" };
            ClearGameObject(ref goMingquXiao);
            goMingquXiao = Instantiate(goButton, goSettingPanel.transform);
            goMingquXiao.onClick.AddListener(delegate {
                sheDing.mingQuXiao = !sheDing.mingQuXiao;
                goMingquXiao.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.mingQuXiao ? labelMingQuXiao[0] : labelMingQuXiao[1];
                WriteSheDing();
            });
            DrawButton(ref goMingquXiao, sheDing.mingQuXiao ? labelMingQuXiao[0] : labelMingQuXiao[1], new Vector2(-x, y), len);
            y -= offset;
            // リセット
            goSettingDialogPanel = GameObject.Find("SettingDialogPanel");
            Button goOptionReset = Instantiate(goButton, goSettingPanel.transform);
            goOptionReset.onClick.AddListener(delegate {
                goSettingDialogPanel.SetActive(true);
            });
            DrawButton(ref goOptionReset, "リセット", new Vector2(0, y));
        }

        // 相手牌オープン切り替え
        private void SwitchShouPaiOpenImage()
        {
            goShouPaiOpen.image.sprite = goPais[shouPaiOpen ? 0x31 : 0x00].GetComponent<SpriteRenderer>().sprite;
        }

        // 設定オプションのリセット
        private void ResetSheDing()
        {
            string filePath = Application.persistentDataPath + "/" + SHE_DING_FILE_NAME + ".json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            sheDing = new SheDing();
            DrawOption();
            if (eventStatus == Event.DUI_JU)
            {
                DrawJuOption();
            }
        }

        // 設定の書込
        private void WriteSheDing()
        {
            File.WriteAllText(Application.persistentDataPath + "/" + SHE_DING_FILE_NAME + ".json", JsonUtility.ToJson(sheDing));
        }

        // 【描画】得点画面
        private void DrawScorePanel()
        {
            // 得点パネル
            goScorePanel = GameObject.Find("ScorePanel");
            EventTrigger etScore = goScorePanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eScore = new();
            eScore.eventID = EventTriggerType.PointerClick;
            eScore.callback.AddListener((eventData) => {
                goScorePanel.SetActive(false);
            });
            etScore.triggers.Add(eScore);
            goScorePanel.SetActive(false);

            // 得点ボタン
            goScore = GameObject.Find("Score").GetComponent<Button>();
            goScore.onClick.AddListener(delegate {
                goScorePanel.SetActive(true);
            });

            RectTransform rtScore = goScore.GetComponent<RectTransform>();
            rtScore.localScale *= scale.x;
            rtScore.anchorMin = new Vector2(1, 1);
            rtScore.anchorMax = new Vector2(1, 1);
            rtScore.pivot = new Vector2(1, 1);
            rtScore.anchoredPosition = new Vector2(-(paiWidth * 3f), -(paiHeight * 0.5f));

            goScoreQiaoShi = new Button[qiaoShiMingQian.Count + 1];

            float x = 0;
            float y = paiHeight * 4;

            goScoreQiaoShi[qiaoShiMingQian.Count] = Instantiate(goButton, goScorePanel.transform);
            goScoreQiaoShi[qiaoShiMingQian.Count].onClick.AddListener(delegate {
                OnClickScoreQiaoShi(PLAYER_NAME);
            });
            DrawButton(ref goScoreQiaoShi[qiaoShiMingQian.Count], PLAYER_NAME, new Vector2(x, y));
            y -= paiHeight * 1.5f;

            int i = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                x = paiWidth * 4 * (i % 2 == 0 ? -1 : 1);
                int pos = i;
                goScoreQiaoShi[i] = Instantiate(goButton, goScorePanel.transform);
                goScoreQiaoShi[i].onClick.AddListener(delegate {
                    OnClickScoreQiaoShi(kvp.Key);
                });
                DrawButton(ref goScoreQiaoShi[i], kvp.Key, new Vector2(x, y), quiaoShiButtonMaxLen);

                if (i % 2 == 1)
                {
                    y -= paiHeight * 1.5f;
                }
                i++;
            }

            goScoreDialogPanel = GameObject.Find("ScoreDialogPanel");
            Button goScoreReset = Instantiate(goButton, goScorePanel.transform);
            goScoreReset.onClick.AddListener(delegate {
                goScoreDialogPanel.SetActive(true);
            });
            DrawButton(ref goScoreReset, "リセット", new Vector2(0, y - paiHeight * 1.5f));

            EventTrigger etResetPanel = goScoreDialogPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eResetPanel = new();
            eResetPanel.eventID = EventTriggerType.PointerClick;
            eResetPanel.callback.AddListener((eventData) => {
                goScoreDialogPanel.SetActive(false);
            });
            etResetPanel.triggers.Add(eResetPanel);

            goScoreDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goScoreDialogPanel.transform);
            DrawText(ref message, "全員の得点をリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goScoreDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(delegate {
                ResetJiLu();
                goScoreDialogPanel.SetActive(false);
            });
            Button goNo = Instantiate(goButton, goScoreDialogPanel.transform);
            goNo.onClick.AddListener(delegate {
                goScoreDialogPanel.SetActive(false);
            });
            DrawButton(ref goNo, "いいえ", new Vector2(paiWidth * 3f, 0));
        }

        // 全員の記録リセット
        private void ResetJiLu()
        {
            string filePath = Application.persistentDataPath + "/" + PLAYER_NAME + ".json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                filePath = Application.persistentDataPath + "/" + kvp.Key + ".json";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi != null)
                {
                    shi.JiLu = new JiLu();
                }
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
            goDataBack.onClick.AddListener(delegate {
                goDataScrollView.SetActive(false);
                goDataBackCanvas.SetActive(false);
            });
            RectTransform rtBack = goDataBack.GetComponent<RectTransform>();
            rtBack.localScale *= scale.x;
            rtBack.anchorMin = new Vector2(0, 1);
            rtBack.anchorMax = new Vector2(0, 1);
            rtBack.pivot = new Vector2(0, 1);
            rtBack.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 0.5f));

            float y = paiHeight * 30f;
            goJiJuMingQian = Instantiate(goText, goDataContent.transform);
            DrawText(ref goJiJuMingQian, "", new Vector2(0, y), 0, 25);

            float x = -(paiWidth * 2f);
            y -= paiHeight;
            DrawData(ref goJiLuJiJiDian, "集計点", y);
            y -= paiHeight;
            DrawData(ref goJiLuBanZhuangShu, "半荘数", y);
            y -= paiHeight;
            DrawData(ref goJiLuDuiJuShu, "対局数", y);
            y -= paiHeight;
            DrawData(ref goJiLuShunWei1Shuai, "１位", y);
            y -= paiHeight;
            DrawData(ref goJiLuShunWei2Shuai, "２位", y);
            y -= paiHeight;
            DrawData(ref goJiLuShunWei3Shuai, "３位", y);
            y -= paiHeight;
            DrawData(ref goJiLuShunWei4Shuai, "４位", y);
            y -= paiHeight;
            DrawData(ref goJiLuHeLeShuai, "和了率", y);
            y -= paiHeight;
            DrawData(ref goJiLuFangChongShuai, "放銃率", y);
            y -= paiHeight;
            DrawData(ref goJiLuTingPaiShuai, "聴牌率", y);
            y -= paiHeight;
            DrawData(ref goJiLuPingJunHeLeDian, "平均和了点", y);
            y -= paiHeight;
            DrawData(ref goJiLuPingJunFangChongDian, "平均放銃点", y);
            y -= paiHeight;
            goYiMing = new TextMeshProUGUI[QiaoShi.YiMing.Length];
            goYiShu = new TextMeshProUGUI[QiaoShi.YiMing.Length];
            for (int i = 0; i < goYiShu.Length; i++)
            {
                if (QiaoShi.YiMing[i] == "ドラ")
                {
                    continue;
                }
                y -= paiHeight;
                DrawData(ref goYiMing[i], ref goYiShu[i], QiaoShi.YiMing[i], y);
            }
            y -= paiHeight;
            goYiManMing = new TextMeshProUGUI[QiaoShi.YiManMing.Length];
            goYiManShu = new TextMeshProUGUI[QiaoShi.YiManMing.Length];
            for (int i = 0; i < goYiManShu.Length; i++)
            {
                y -= paiHeight;
                DrawData(ref goYiManMing[i], ref goYiManShu[i], QiaoShi.YiManMing[i], y);
            }

            RectTransform rtDataContent = goDataContent.GetComponent<RectTransform>();
            Vector2 size = rtDataContent.sizeDelta;
            size.y = paiHeight * 65f;
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

        // 得点パネル 雀士名クリック
        private void OnClickScoreQiaoShi(string mingQian)
        {
            QiaoShi shi = GetQiaoShi(mingQian);
            JiLu jiLu = new();
            string filePath = Application.persistentDataPath + "/" + shi.MingQian + ".json";
            if (File.Exists(filePath))
            {
                jiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(filePath));
            }

            goJiJuMingQian.text = mingQian;
            goJiLuJiJiDian.text = jiLu.jiJiDian + "点";
            goJiLuBanZhuangShu.text = jiLu.banZhuangShu + "回";
            goJiLuDuiJuShu.text = jiLu.duiJuShu + "回";
            goJiLuShunWei1Shuai.text = jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)jiLu.shunWei1 / jiLu.banZhuangShu * 100) + "％";
            goJiLuShunWei2Shuai.text = jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)jiLu.shunWei2 / jiLu.banZhuangShu * 100) + "％";
            goJiLuShunWei3Shuai.text = jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)jiLu.shunWei3 / jiLu.banZhuangShu * 100) + "％";
            goJiLuShunWei4Shuai.text = jiLu.banZhuangShu == 0 ? "" : (int)Math.Floor((double)jiLu.shunWei4 / jiLu.banZhuangShu * 100) + "％";
            goJiLuHeLeShuai.text = jiLu.duiJuShu == 0 ? "" : (int)Math.Floor((double)jiLu.heLeShu / jiLu.duiJuShu * 100) + "％";
            goJiLuFangChongShuai.text = jiLu.duiJuShu == 0 ? "" : (int)Math.Floor((double)jiLu.fangChongShu / jiLu.duiJuShu * 100) + "％";
            goJiLuTingPaiShuai.text = jiLu.liuJuShu == 0 ? "" : (int)Math.Floor((double)jiLu.tingPaiShu / jiLu.liuJuShu * 100) + "％";
            goJiLuPingJunHeLeDian.text = jiLu.heLeShu == 0 ? "" : (int)Math.Floor((double)jiLu.heLeDian / jiLu.heLeShu) + "点";
            goJiLuPingJunFangChongDian.text = jiLu.fangChongShu == 0 ? "" : (int)Math.Floor((double)jiLu.fangChongDian / jiLu.fangChongShu) + "点";
            for (int i = 0; i < goYiShu.Length; i++)
            {
                if (QiaoShi.YiMing[i] == "ドラ")
                {
                    continue;
                }
                goYiShu[i].text = jiLu.yiShu[i] + "回";
                goYiMing[i].color = jiLu.yiShu[i] == 0 ? Color.gray : Color.black;
                goYiShu[i].color = jiLu.yiShu[i] == 0 ? Color.gray : Color.black;
            }
            for (int i = 0; i < goYiManShu.Length; i++)
            {
                goYiManShu[i].text = jiLu.yiManShu[i] + "回";
                goYiManMing[i].color = jiLu.yiManShu[i] == 0 ? Color.gray : Color.black;
                goYiManShu[i].color = jiLu.yiManShu[i] == 0 ? Color.gray : Color.black;
            }

            goDataScrollView.SetActive(true);
            goDataBackCanvas.SetActive(true);
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
            goGuiZe.onClick.AddListener(delegate {
                goGuiZeScrollView.SetActive(true);
            });

            RectTransform rtGuiZe = goGuiZe.GetComponent<RectTransform>();
            rtGuiZe.localScale *= scale.x;
            rtGuiZe.anchorMin = new Vector2(1, 1);
            rtGuiZe.anchorMax = new Vector2(1, 1);
            rtGuiZe.pivot = new Vector2(1, 1);
            rtGuiZe.anchoredPosition = new Vector2(-(paiWidth * 5.5f), -(paiHeight * 0.5f));

            goGuiZeBackCanvas = GameObject.Find("GuiZeBackCanvas");
            goGuiZeBackCanvas.SetActive(false);

            // ルール画面の上の戻るボタン
            Button goGuiZeBack = Instantiate(goBack, goGuiZeScrollView.transform);
            goGuiZeBack.onClick.AddListener(delegate
            {
                goGuiZeScrollView.SetActive(false);
                goGuiZeBackCanvas.SetActive(false);
            });
            RectTransform rtBack = goGuiZeBack.GetComponent<RectTransform>();
            rtBack.localScale *= scale.x;
            rtBack.anchorMin = new Vector2(0, 1);
            rtBack.anchorMax = new Vector2(0, 1);
            rtBack.pivot = new Vector2(0, 1);
            rtBack.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 0.5f));

            float y = paiHeight * 8.5f;
            float x = 0;
            TextMeshProUGUI title = Instantiate(goText, goGuiZeContent.transform);
            DrawText(ref title, "ルール", new Vector2(0, y), 0, 25);
            y -= paiHeight;
            List<string> guiZe = new();
            guiZe.Add("半荘戦（" + Number2Full(GuiZe.kaiShiDian) + "点開始、" + Number2Full(GuiZe.fanDian) + "点返し）");
            guiZe.Add("ピンヅモ" + (GuiZe.ziMoPingHe ? "有り" : "無し"));
            guiZe.Add("食いタン" + (GuiZe.shiDuan ? "有り" : "無し"));
            guiZe.Add("後づけ有り");
            guiZe.Add("食い替え" + (GuiZe.shiTi ? "有り" : "無し（食い替えの場合、チョンボ）"));
            guiZe.Add("ダブロン、トリプルロン" + (GuiZe.wRongHe ? "有り" : "無し（頭ハネ）"));
            guiZe.Add("供託あがりどり" + (GuiZe.wRongHe ? "（ダブロン以上は上家どり）" : ""));
            guiZe.Add("パオ（責任払い）" + (GuiZe.baoZe ? "有り" : "無し"));
            guiZe.Add("赤ドラ（萬子" + Number2Full(GuiZe.chiPaiShu[0]) + "枚、筒子" + Number2Full(GuiZe.chiPaiShu[1]) + "枚、索子" + Number2Full(GuiZe.chiPaiShu[2]) + "枚）");
            guiZe.Add("九種九牌は" + (GuiZe.jiuZhongJiuPaiLianZhuang ? "親の連荘" : "流局（親流れ）"));
            guiZe.Add("四家立直は" + (GuiZe.siJiaLiZhiLianZhuang ? "親の連荘" : "流局（親流れ）"));
            guiZe.Add("四風子連打は" + (GuiZe.siFengZiLianDaLianZhuang ? "親の連荘" : "流局（親流れ）"));
            guiZe.Add("四開槓は" + (GuiZe.siKaiGangLianZhuang ? "親の連荘" : "流局（親流れ）"));
            guiZe.Add("箱（０点以下で終了）" + (GuiZe.xiang ? "有り" : "無し"));
            guiZe.Add("流し満貫" + (GuiZe.liuManGuan ? "有り（親は連荘）" : "無し"));
            guiZe.Add("１０００点未満のリーチ" + (GuiZe.jieJinLiZhi ? "可能" : "不可"));
            foreach (string g in guiZe)
            {
                TextMeshProUGUI t = Instantiate(goText, goGuiZeContent.transform);
                DrawText(ref t, g, new Vector2(x, y), 0, 20, TextAlignmentOptions.Left, 21);
                y -= paiHeight;
            }

            RectTransform rtGuiZeContent = goGuiZeContent.GetComponent<RectTransform>();
            Vector2 size = rtGuiZeContent.sizeDelta;
            size.y = paiHeight * 19f;
            rtGuiZeContent.sizeDelta = size;
            ScrollRect scrollRect = goGuiZeScrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        // 数値全角変換
        private static string Number2Full(int num)
        {
            string str = num.ToString();
            char[] result = new char[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c >= '0' && c <= '9')
                {
                    result[i] = (char)(c + 0xFEE0);
                }
                else
                {
                    result[i] = c;
                }
            }

            return new string(result);
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
                switch (eventStatus)
                {
                    // 雀士選択
                    case Event.QIAO_SHI_XUAN_ZE:
                    // フォロー雀士選択
                    case Event.FOLLOW_QIAO_SHI_XUAN_ZE:
                        DrawTitle();
                        break;

                    // 配牌
                    case Event.PEI_PAI:
                        break;
                    // 対局
                    case Event.DUI_JU:
                    // 対局終了
                    case Event.DUI_JU_ZHONG_LE:
                        DrawDuiJu();
                        break;
                    // 役表示
                    case Event.YI_BIAO_SHI:
                        DrawXuanShangPai();
                        OrientationSelectDaPai();
                        break;
                }
            }

            switch (eventStatus)
            {
                // 開始
                case Event.KAI_SHI:
                    if (!isKaiShiCoroutine)
                    {
                        StartCoroutine(KaiShi());
                    }
                    break;

                // 雀士選択
                case Event.QIAO_SHI_XUAN_ZE:
                    if (!isQiaoShiXuanZeCoroutine)
                    {
                        StartCoroutine(QiaoShiXuanZe());
                    }
                    break;

                // フォロー雀士選択
                case Event.FOLLOW_QIAO_SHI_XUAN_ZE:
                    if (!isFollowQiaoShiXuanZeCoroutine)
                    {
                        StartCoroutine(FollowQiaoShiXuanZe());
                    }
                    break;

                // 場決
                case Event.CHANG_JUE:
                    ChangJue();
                    break;

                // 親決
                case Event.QIN_JUE:
                    if (!isQinJueCoroutine)
                    {
                        StartCoroutine(QinJue());
                    }
                    break;

                // 荘初期化
                case Event.ZHUANG_CHU_QI_HUA:
                    ZhuangChuQiHua();
                    break;

                // 配牌
                case Event.PEI_PAI:
                    if (!isPeiPaiCoroutine)
                    {
                        StartCoroutine(PeiPai());
                    }
                    break;

                // 対局
                case Event.DUI_JU:
                    if (!isDuiJuCoroutine)
                    {
                        StartCoroutine(DuiJu());
                    }
                    break;

                // 対局終了
                case Event.DUI_JU_ZHONG_LE:
                    if (!isDuiJuZhongLeCoroutine)
                    {
                        StartCoroutine(DuiJuZhongLe());
                    }
                    break;

                // 役表示
                case Event.YI_BIAO_SHI:
                    if (!isYiBiaoShiCoroutine)
                    {
                        StartCoroutine(YiBiaoShi());
                    }
                    break;

                // 点表示
                case Event.DIAN_BIAO_SHI:
                    if (!isDianBiaoShiCoroutine)
                    {
                        StartCoroutine(DianBiaoShi());
                    }
                    break;

                // 荘終了
                case Event.ZHUANG_ZHONG_LE:
                    if (!isZhuangZhongLeCoroutine)
                    {
                        StartCoroutine(ZhuangZhong());
                    }
                    break;
            }
        }

        // 【描画】対局
        private void DrawDuiJu()
        {
            DrawJuFrame();
            DrawJuOption();
            DrawQiJia();
            DrawXuanShangPai();
            DrawMingQian();

            bool isHeLe = false;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                if (shi.Player)
                {
                    continue;
                }
                QiaoShi.YaoDingYi yao = QiaoShi.YaoDingYi.Wu;
                int xuanZe = shi.ZiJiaXuanZe;
                if (shi.TaJiaYao != QiaoShi.YaoDingYi.Wu)
                {
                    yao = shi.TaJiaYao;
                    xuanZe = shi.TaJiaXuanZe;
                }
                if (yao == QiaoShi.YaoDingYi.ZiMo || yao == QiaoShi.YaoDingYi.RongHe)
                {
                    yao = QiaoShi.YaoDingYi.HeLe;
                    isHeLe = true;
                }
                DrawShouPai(i, yao, xuanZe, shi.Player, shi.Follow);
            }
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                if (!shi.Player)
                {
                    continue;
                }
                if (isHeLe)
                {
                    DrawShouPai(i, QiaoShi.YaoDingYi.Wu, -2);
                    break;
                }
                if (goYao[0] != null)
                {
                    DrawZiJiaYao(shi, 0, shi.ShouPai.Count - 1, true, false);
                    if (goYao[0] == null)
                    {
                        DrawTaJiaYao(i, shi, 0, shi.Follow);
                        if (goYao[0] != null)
                        {
                            DrawShouPai(i, QiaoShi.YaoDingYi.Wu, -2);
                            break;
                        }
                    }
                }
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                {
                    int x = shi.Follow ? shi.ZiJiaXuanZe : shi.ShouPai.Count - 1;
                    DrawSelectDaPai(i, shi, x);
                    DrawDaiPai(i, x);
                }
                else
                {
                    QiaoShi.YaoDingYi yao = QiaoShi.YaoDingYi.Wu;
                    int xuanZe = shi.ZiJiaXuanZe;
                    if (shi.TaJiaYao != QiaoShi.YaoDingYi.Wu)
                    {
                        yao = shi.TaJiaYao;
                        xuanZe = shi.TaJiaXuanZe;
                    }
                    if (yao == QiaoShi.YaoDingYi.ZiMo || yao == QiaoShi.YaoDingYi.RongHe)
                    {
                        yao = QiaoShi.YaoDingYi.HeLe;
                    }
                    DrawShouPai(i, yao, xuanZe, shi.Player, shi.Follow);
                }
            }

            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                DrawDaiPai(i, -2);
                DrawShePai(i);
            }
            OrientationSelectDaPai();
        }

        // スクリーンクリック
        private void OnClickScreen()
        {
            switch (eventStatus)
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
                    keyPress = true;
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
                    if (tingPaiLianZhuang != Zhuang.XU_HANG)
                    {
                        keyPress = true;
                    }
                    if (isQuXiao && !sheDing.mingQuXiao)
                    {
                        for (int i = 0; i < Chang.QiaoShis.Count; i++)
                        {
                            QiaoShi shi = Chang.QiaoShis[i];
                            if (shi.Player && !shi.JiJia)
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
            if (selectedCount == 3 || selectedCount == 2 || selectedCount == 1)
            {
                keyPress = true;
            }
        }

        // フォロー無しクリック
        private void OnClickScreenFollowNone()
        {
            Chang.QiaoShis.Insert(0, new QiaoJiXie(PLAYER_NAME));
            //Chang.QiaoShi.Insert(0, new QiaoXiaoLu(PLAYER_NAME));
            Chang.QiaoShis[0].Follow = false;
            Chang.QiaoShis[0].Player = true;
            keyPress = true;
        }

        // 一時停止
        private IEnumerator Pause(ForwardMode mode)
        {
            keyPress = false;
            if ((int)forwardMode > (int)mode)
            {
                keyPress = true;
            }
            while (!keyPress)
            {
                yield return null;
            }
            keyPress = false;
        }

        // 【ゲーム】開始
        private IEnumerator KaiShi()
        {
            isKaiShiCoroutine = true;

            // 描画
            ClearScreen();
            DrawKaiShi();
            int fadingOut = 1;
            float alpha = 1f;
            keyPress = false;
            while (!keyPress)
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
                goStart.canvasRenderer.SetAlpha(alpha);
                yield return null;
            }
            keyPress = false;

            eventStatus = Event.QIAO_SHI_XUAN_ZE;
            isKaiShiCoroutine = false;
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
            isQiaoShiXuanZeCoroutine = true;

            Chang.QiaoShis = new List<QiaoShi>();

            // 描画
            ClearScreen();
            DrawTitle();
            DrawQiaoShiXuanZe();

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
                    Chang.QiaoShis.Add(GetQiaoShi(kvp.Key));
                }
            }

            eventStatus = Event.FOLLOW_QIAO_SHI_XUAN_ZE;
            //eventStatus = Event.CHANG_JUE;
            //OnClickScreenFollowNone();
            isQiaoShiXuanZeCoroutine = false;
        }

        // 【ゲーム】フォロー雀士選択
        private IEnumerator FollowQiaoShiXuanZe()
        {
            isFollowQiaoShiXuanZeCoroutine = true;

            // 描画
            ClearScreen();
            DrawTitle();
            DrawFollowQiaoShiXuanZe();

            keyPress = false;
            if (forwardMode > 0)
            {
                keyPress = true;
                OnClickScreenFollowNone();
            }
            while (!keyPress) { yield return null; }
            keyPress = false;

            eventStatus = Event.CHANG_JUE;
            isFollowQiaoShiXuanZeCoroutine = false;
        }

        // 【描画】タイトル
        private void DrawTitle()
        {
            float x = 0;
            float y = paiHeight * 3.5f;
            string value;
            switch (eventStatus)
            {
                case Event.QIAO_SHI_XUAN_ZE:
                    value = "相手雀士";
                    break;
                case Event.FOLLOW_QIAO_SHI_XUAN_ZE:
                    value = "フォロー雀士";
                    break;
                default:
                    value = "";
                    break;
            }
            DrawText(ref goJu, value, new Vector2(x, y), 0, 20);
        }

        // 【描画】局、残牌、供託、点
        private void DrawJuFrame()
        {
            DrawJu();
            DrawGongTuo();
            if (eventStatus != Event.DIAN_BIAO_SHI)
            {
                DrawCanShanPaiShu();
            }
            DrawDianBang();
        }

        // 【描画】局
        private void DrawJu()
        {
            // 枠
            ClearGameObject(ref goJuFrame);
            goJuFrame = Instantiate(goFrame, goFrame.transform.parent);
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
            float x = 0;
            float y = paiHeight * 0.9f;
            string value = Pai.FengPaiMing[Chang.ChangFeng - 0x31] + (Chang.Ju + 1) + "局";
            DrawText(ref goJu, value, new Vector2(x, y), 0, 18);
            goJu.rectTransform.SetSiblingIndex(1);
        }

        // 【描画】供託
        private void DrawGongTuo()
        {
            float x = -paiWidth * 0.3f;
            float y = paiHeight * 0.4f;
            ClearGameObject(ref goBenChang);
            goBenChang = Instantiate(goDianBang100, goDianBang100.transform.parent);
            RectTransform rt100 = goBenChang.GetComponent<RectTransform>();
            rt100.sizeDelta = new Vector2(rt100.sizeDelta.x * 0.5f, rt100.sizeDelta.y);
            rt100.anchoredPosition = new Vector2(x, y);
            ClearGameObject(ref goBenChangText);
            goBenChangText = Instantiate(goText, goJuFrame.transform.parent);
            string valueBenChang = "x" + Chang.BenChang.ToString();
            DrawText(ref goBenChangText, valueBenChang, new Vector2(x + paiWidth * 1.1f, y + paiHeight * 0.05f), 0, 14);

            y -= paiHeight * 0.4f;
            ClearGameObject(ref goGongTou);
            goGongTou = Instantiate(goDianBang1000, goDianBang1000.transform.parent);
            RectTransform rt1000 = goGongTou.GetComponent<RectTransform>();
            rt1000.sizeDelta = new Vector2(rt1000.sizeDelta.x * 0.5f, rt1000.sizeDelta.y);
            rt1000.anchoredPosition = new Vector2(x, y);
            ClearGameObject(ref goGongTouText);
            goGongTouText = Instantiate(goText, goJuFrame.transform.parent);
            string valueGongTou = "x" + (Chang.GongTuo / 1000).ToString();
            DrawText(ref goGongTouText, valueGongTou, new Vector2(x + paiWidth * 1.1f, y + paiHeight * 0.05f), 0, 14);
        }

        /**
         * 【描画】残山牌数
         */
        private void DrawCanShanPaiShu()
        {
            ClearGameObject(ref goCanShanPaiShu);
            goCanShanPaiShu = Instantiate(goFrame, goJuFrame.transform.parent);
            float alfa = Pai.CanShanPaiShu() < 100 ? 1f : 0f;
            DrawFrame(ref goCanShanPaiShu, Pai.CanShanPaiShu().ToString(), new Vector2(0, -(paiHeight * 0.6f)), 0, 20, new Color(0, 0.6f, 0), new Color(1f, 1f, 1f, alfa), 3);
        }

        // 【描画】点数
        private void DrawDianBang()
        {
            int dianPlayer = 0;
            if (isDianCha)
            {
                for (int i = 0; i < Chang.QiaoShis.Count; i++)
                {
                    int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                    QiaoShi shi = Chang.QiaoShis[jia];
                    if (shi.Player)
                    {
                        dianPlayer = shi.DianBang;
                        break;
                    }
                }
            }

            float x = 0f;
            float y = -(paiWidth * 2.5f);
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                QiaoShi shi = Chang.QiaoShis[jia];
                Color background = shi.Feng == 0x31 ? new Color(1f, 0.5f, 0.5f) : Color.black;
                ClearGameObject(ref goFeng[i]);
                goFeng[i] = Instantiate(goFrame, goJuFrame.transform.parent);
                if ((eventStatus == Event.DUI_JU || eventStatus == Event.DUI_JU_ZHONG_LE) && jia == Chang.ZiMoFan)
                {
                    ClearGameObject(ref goZiMoShiLine);
                    goZiMoShiLine = Instantiate(goLine, goJuFrame.transform.parent);
                    RectTransform rt = goZiMoShiLine.rectTransform;
                    rt.anchoredPosition = Cal(0, -(paiHeight * 2f), shi.PlayOrder);
                    rt.rotation = Quaternion.Euler(0, 0, 90 * GetDrawOrder(shi.PlayOrder));
                }
                DrawFrame(ref goFeng[i], Pai.FengPaiMing[shi.Feng - 0x31], Cal(x - paiWidth * 2f, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 16, background, Color.white);
                if (eventStatus == Event.DIAN_BIAO_SHI)
                {
                    continue;
                }
                ClearGameObject(ref goDianBang[i]);
                goDianBang[i] = Instantiate(goText, goJuFrame.transform.parent);
                string value;
                if (isDianCha && !shi.Player)
                {
                    int dianCha = dianPlayer - shi.DianBang;
                    value = dianCha.ToString();
                    goDianBang[i].color = (dianCha >= 0 ? Color.blue : Color.red);
                }
                else
                {
                    value = shi.DianBang.ToString();
                    goDianBang[i].color = Color.black;
                }
                DrawText(ref goDianBang[i], value, Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 16);

                if (shi.LiZhi)
                {
                    ClearGameObject(ref goLizhiBang[jia]);
                    goLizhiBang[jia] = Instantiate(goDianBang1000, goJuFrame.transform.parent);
                    goLizhiBang[jia].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.PlayOrder));
                    RectTransform rt = goLizhiBang[jia].GetComponent<RectTransform>();
                    rt.anchoredPosition = Cal(x, y + paiHeight * 0.4f, shi.PlayOrder);
                }
            }
        }

        // 【描画】対局中オプション
        private void DrawJuOption()
        {
            // 鳴 有り・無し
            string[] labelMingWu = new string[] { "鳴無", "鳴有" };
            ClearGameObject(ref goMingWu);
            goMingWu = Instantiate(goButton, goButton.transform.parent);
            goMingWu.onClick.AddListener(delegate
            {
                sheDing.mingWu = !sheDing.mingWu;
                goMingWu.GetComponentInChildren<TextMeshProUGUI>().text = sheDing.mingWu ? labelMingWu[0] : labelMingWu[1];
                WriteSheDing();
            });
            float x = paiWidth * 8f;
            float y = -(paiHeight * 9.3f);
            if (orientation != ScreenOrientation.Portrait)
            {
                x = paiWidth * 13f;
                y = -(paiHeight * 0.5f);
            }
            DrawButton(ref goMingWu, sheDing.mingWu ? labelMingWu[0] : labelMingWu[1], new Vector2(x, y));

            // 点差
            ClearGameObject(ref goDianCha);
            goDianCha = Instantiate(goButton, goButton.transform.parent);

            if (orientation == ScreenOrientation.Portrait)
            {
                x -= paiWidth * 3.5f;
            }
            else
            {
                y -= paiHeight * 1.5f;
            }
            DrawButton(ref goDianCha, "点差", new Vector2(x, y));

            EventTrigger trigger = goDianCha.AddComponent<EventTrigger>();
            EventTrigger.Entry pointerDownEntry = new()
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDownEntry.callback.AddListener((data) =>
            {
                isDianCha = true;
                DrawJuFrame();
            });
            trigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new()
            {
                eventID = EventTriggerType.PointerUp
            };
            pointerUpEntry.callback.AddListener((data) =>
            {
                isDianCha = false;
                DrawJuFrame();
            });
            trigger.triggers.Add(pointerUpEntry);
        }

        // 【描画】雀士選択
        private void DrawQiaoShiXuanZe()
        {
            if ((int)forwardMode > (int)ForwardMode.FAST_FORWARD)
            {
                // ランダム自動選択
                RandomQiaoShiXuanZe();
            }

            float x = 0;
            float y = paiHeight * 2f;
            int index = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                x = paiWidth * 4 * (index % 2 == 0 ? -1 : 1);
                int pos = index;
                DrawButton(ref goQiaoShi[index], kvp.Key, new Vector2(x, y), quiaoShiButtonMaxLen);
                goQiaoShi[index].onClick.AddListener(delegate {
                    OnClickQiaoShi(kvp.Key, pos);
                });

                SetQiaoShiColor(kvp.Key, index);
                if (index % 2 == 1)
                {
                    y -= paiHeight * 1.5f;
                }
                index++;
            }
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
                string lastMing = "";
                int lastPos = 0;
                int index = 0;
                foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
                {
                    if (kvp.Value)
                    {
                        selectedCount++;
                        lastMing = kvp.Key;
                        lastPos = index;
                    }
                    if (selectedCount > 2)
                    {
                        qiaoShiMingQian[lastMing] = false;
                        SetQiaoShiColor(lastMing, lastPos);
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
            float x = 0;
            float y = paiHeight * 2f;
            int i = 0;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                x = paiWidth * 4 * (i % 2 == 0 ? -1 : 1);
                int pos = i;
                DrawButton(ref goQiaoShi[i], kvp.Key, new Vector2(x, y), quiaoShiButtonMaxLen);
                goQiaoShi[i].onClick.AddListener(delegate {
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
            Chang.QiaoShis.Insert(0, GetQiaoShi(mingQian));
            Chang.QiaoShis[0].MingQian = PLAYER_NAME;
            Chang.QiaoShis[0].Follow = true;
            Chang.QiaoShis[0].Player = true;
            keyPress = true;
        }

        // 【ゲーム】場決
        private void ChangJue()
        {
            ClearScreen();

            if (Chang.QiaoShis[0] == null)
            {
                OnClickScreenFollowNone();
            }
            List<int> fengPai = new();
            for (int i = 0; i < Pai.FengPaiDingYi.Length - (4 - Chang.QiaoShis.Count); i++)
            {
                fengPai.Add(Pai.FengPaiDingYi[i]);
            }
            Chang.Shuffle(fengPai, 20);

            Button[] goButton = new Button[Chang.QiaoShis.Count];

            int idx = 0;
            for (int i = 0x31; i <= 0x34 - (4 - Chang.QiaoShis.Count); i++)
            {
                for (int j = 0; j < fengPai.Count; j++)
                {
                    if (fengPai[j] == i)
                    {
                        QiaoShi shi = Chang.QiaoShis[idx];
                        Chang.QiaoShis[idx] = Chang.QiaoShis[j];
                        Chang.QiaoShis[j] = shi;
                    }
                }
                idx++;
            }

            int order = 0;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                shi.PlayOrder = i;
                if (shi.Player)
                {
                    order = i;
                }
            }
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                shi.PlayOrder -= order;
                if (shi.PlayOrder < 0)
                {
                    shi.PlayOrder += Chang.QiaoShis.Count;
                }
            }

            DrawMingQian();

            // 記録の読込
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                string filePath = Application.persistentDataPath + "/" + shi.MingQian + ".json";
                if (File.Exists(filePath))
                {
                    shi.JiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(filePath));
                }
                else
                {
                    shi.JiLu = new JiLu();
                }
            }

            ClearGameObject(ref goButton);

            eventStatus = Event.QIN_JUE;
        }

        // 【ゲーム】親決
        private IEnumerator QinJue()
        {
            isQinJueCoroutine = true;

            ClearScreen();
            DrawMingQian();

            int sai1 = 1;
            int sai2 = 1;
            System.Random r = new();
            if (forwardMode > 0)
            {
                keyPress = true;
            }
            for (int i = 0; i < 60; i++)
            {
                sai1 = r.Next(0, 6) + 1;
                sai2 = r.Next(0, 6) + 1;
                DrawSais(0, sai1, sai2);
                if (keyPress)
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
            Chang.Qin = (sai1 + sai2 - 1) % Chang.QiaoShis.Count;
            // 起家
            Chang.QiaJia = Chang.Qin;
            Chang.ChangFeng = 0x31;
            DrawQiJia();

            yield return Pause(ForwardMode.NORMAL);

            eventStatus = Event.ZHUANG_CHU_QI_HUA;
            isQinJueCoroutine = false;
        }

        // 【描画】名前(4人分)
        private void DrawMingQian()
        {
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                DrawMingQian(jia);
            }
        }

        // 【描画】名前
        private void DrawMingQian(int jia)
        {
            QiaoShi shi = Chang.QiaoShis[jia];
            float x;
            float y;
            switch (eventStatus)
            {
                case Event.PEI_PAI:
                case Event.DUI_JU:
                case Event.DUI_JU_ZHONG_LE:
                    x = -paiWidth * 5f;
                    y = -(paiWidth * 3f + paiHeight * 3f);
                    if (!shi.Player)
                    {
                        DrawText(ref goMingQian[jia], shi.MingQian, Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 20);
                    }
                    break;
                default:
                    x = 0f;
                    y = -(paiHeight * 5);
                    DrawText(ref goMingQian[jia], shi.MingQian, Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 25);
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
            go = Instantiate(goSais[mu - 1].GetComponent<Image>(), goSais[mu - 1].GetComponent<Image>().transform.parent);
            RectTransform rt = go.GetComponent<RectTransform>();
            QiaoShi shi = Chang.QiaoShis[jia];
            go.transform.Rotate(0, 0, 90 * shi.PlayOrder);
            float x = 0;
            float y = -(paiWidth * 1.2f);
            rt.anchoredPosition = Cal(x + margin, y, shi.PlayOrder);
        }

        // 【描画】起家
        private void DrawQiJia()
        {
            float x, y;
            switch (eventStatus)
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
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                if (Chang.QiaJia == jia)
                {
                    ClearGameObject(ref goQiJia);
                    QiaoShi shi = Chang.QiaoShis[jia];
                    if (shi.Player && orientation != ScreenOrientation.Portrait)
                    {
                        x -= paiWidth * 1.2f;
                        y += paiHeight * 0.6f;
                    }
                    if (shi.PlayOrder == 3 && orientation != ScreenOrientation.Portrait)
                    {
                        x -= paiHeight * 0.6f;
                    }
                    goQiJia = Instantiate(goQiJias[Chang.ChangFeng - 0x31].GetComponent<Image>(), goQiJias[Chang.ChangFeng - 0x31].GetComponent<Image>().transform.parent);
                    goQiJia.transform.Rotate(0, 0, 90 * GetDrawOrder(shi.PlayOrder));
                    RectTransform rt = goQiJia.GetComponent<RectTransform>();
                    rt.anchoredPosition = Cal(eventStatus == Event.QIN_JUE ? 0 : x, y, shi.PlayOrder);
                }
            }
        }

        // 【ゲーム】荘初期化
        private void ZhuangChuQiHua()
        {
            Chang.ZhuangChuQiHua();
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                shi.ZhuangChuQiHua();
            }

            eventStatus = Event.PEI_PAI;
        }

        // 【ゲーム】配牌
        private IEnumerator PeiPai()
        {
            isPeiPaiCoroutine = true;

            // 局初期化
            JuChuQiHua();

            System.Random r = new();

            // 描画
            ClearScreen();
            DrawJuFrame();
            DrawJuOption();
            DrawQiJia();
            DrawXuanShangPai();
            DrawMingQian();

            // 配牌
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Chang.QiaoShis.Count; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Chang.QiaoShis[(Chang.Qin + j) % Chang.QiaoShis.Count].ZiMo(Pai.ShanPaiZiMo());
                    }
                    DrawShouPai((Chang.Qin + j) % Chang.QiaoShis.Count, QiaoShi.YaoDingYi.Wu, -1);
                    yield return new WaitForSeconds(waitTime / 3);
                }
            }
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                // 理牌
                Chang.QiaoShis[(Chang.Qin + i) % Chang.QiaoShis.Count].LiPai();
                DrawShouPai((Chang.Qin + i) % Chang.QiaoShis.Count, QiaoShi.YaoDingYi.Wu, -1);
            }
            yield return new WaitForSeconds(waitTime / 3);
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                    Chang.QiaoShis[(Chang.Qin + i) % Chang.QiaoShis.Count].ZiMo(Pai.ShanPaiZiMo());
                DrawShouPai((Chang.Qin + i) % Chang.QiaoShis.Count, QiaoShi.YaoDingYi.Wu, -1);
            }
            yield return new WaitForSeconds(waitTime / 3);
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                Chang.QiaoShis[(Chang.Qin + i) % Chang.QiaoShis.Count].LiPai();
                DrawShouPai((Chang.Qin + i) % Chang.QiaoShis.Count, QiaoShi.YaoDingYi.Wu, -1);
            }

            eventStatus = Event.DUI_JU;
            isPeiPaiCoroutine = false;
        }

        // 局初期化
        private void JuChuQiHua()
        {
            // 局初期化
            Chang.JuChuQiHua();
            // 雀士初期化
            int w = 0;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                QiaoShi shi = Chang.QiaoShis[jia];
                shi.JuChuQiHua(Pai.FengPaiDingYi[i]);

                if (shi.Player)
                {
                    w = shi.Feng - 0x31;
                }
            }

            // 洗牌
            Pai.XiPai();
            // 積込
            //List<List<int>> jiRuPai = new()
            //{
            //    new() { 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x07, 0x07, 0x31, 0x31, 0x31, 0x31, 0x32, 0x32 },
            //    //new() { 0x31, 0x31, 0x31, 0x32, 0x32, 0x32, 0x33, 0x33, 0x33, 0x34, 0x34, 0x34 },
            //    //new() { 0x31, 0x32 },
            //    //new() { 0x33 },
            //    //new() { 0x34 }
            //};
            //Pai.JiRu(w, jiRuPai);

            // 洗牌嶺上
            Pai.XiPaiLingShang();

            tingPaiLianZhuang = Zhuang.XU_HANG;
            isDuiJuCoroutine = false;

            eventStatus = Event.PEI_PAI;
        }

        // 【描画】懸賞牌
        private void DrawXuanShangPai()
        {
            ClearGameObject(ref Pai.goXuanShangPai);
            ClearGameObject(ref Pai.goLiXuanShangPai);

            float X = -(paiWidth * 2f);
            float Y = paiHeight * 8f;
            if (orientation != ScreenOrientation.Portrait)
            {
                X = -(paiWidth * 15f);
                Y = paiHeight * 2f;
            }
            float x = X;
            float y = Y;
            for (int i = 0; i <= 4; i++)
            {
                Pai.goXuanShangPai[i] = Instantiate(goPai, goPai.transform.parent);
                Pai.goXuanShangPai[i].transform.SetSiblingIndex(1);
                DrawPai(ref Pai.goXuanShangPai[i], i < Pai.XuanShangPai.Count ? Pai.XuanShangPai[i] : 0x00, new Vector2(x, y), 0);
                x += paiWidth;
            }

            bool isLiXuanShangPai = false;
            if (Chang.HeleFan >= 0 && Chang.QiaoShis[Chang.HeleFan].LiZhi)
            {
                isLiXuanShangPai = true;
            }
            foreach (int fan in Chang.RongHeFan)
            {
                QiaoShi shi = Chang.QiaoShis[fan];
                if (shi.LiZhi)
                {
                    isLiXuanShangPai = true;
                    break;
                }
            }
            if (isLiXuanShangPai)
            {
                x = X;
                y -= paiHeight;
                for (int i = 0; i <= 4; i++)
                {
                    Pai.goLiXuanShangPai[i] = Instantiate(goPai, goPai.transform.parent);
                    Pai.goLiXuanShangPai[i].transform.SetSiblingIndex(0);
                    DrawPai(ref Pai.goLiXuanShangPai[i], i < Pai.XuanShangPai.Count ? Pai.LiXuanShangPai[i] : 0x00, new Vector2(x, y), 0);
                    x += paiWidth;
                }
            }
        }

        // 【ゲーム】対局
        private IEnumerator DuiJu()
        {
            isDuiJuCoroutine = true;

            QiaoShi ziJiaShi = Chang.QiaoShis[Chang.ZiMoFan];
            if ((Chang.ZiJiaYao == QiaoShi.YaoDingYi.Wu || Chang.ZiJiaYao == QiaoShi.YaoDingYi.LiZhi) && Chang.TaJiaYao == QiaoShi.YaoDingYi.Wu)
            {
                // 山牌自摸
                ziJiaShi.ZiMo(Pai.ShanPaiZiMo());
            }
            if (Chang.ZiJiaYao == QiaoShi.YaoDingYi.AnGang || Chang.ZiJiaYao == QiaoShi.YaoDingYi.JiaGang || Chang.TaJiaYao == QiaoShi.YaoDingYi.DaMingGang)
            {
                // 嶺上牌自摸
                ziJiaShi.ZiMo(Pai.LingShangPaiZiMo());
            }
            // 描画
            DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -2, true, false);
            DrawJuFrame();
            yield return new WaitForSeconds(waitTime);
            if (Chang.TaJiaYao != QiaoShi.YaoDingYi.Wu)
            {
                yield return new WaitForSeconds(waitTime);
            }
            ClearGameObject(ref goSheng);

            // 思考自家判定
            ziJiaShi.TaJiaYao = QiaoShi.YaoDingYi.Wu;
            ziJiaShi.TaJiaXuanZe = 0;
            ziJiaShi.SiKaoZiJiaPanDing();
            DrawDianBang();
            DrawDaiPai(Chang.ZiMoFan, -1);
            // 思考自家
            if (ziJiaShi.Player)
            {
                ziJiaShi.SiKaoZiJia();
                DrawZiJiaYao(ziJiaShi, 0, ziJiaShi.ShouPai.Count - 1, true, false);
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                {
                    int x = ziJiaShi.Follow ? ziJiaShi.ZiJiaXuanZe : ziJiaShi.ShouPai.Count - 1;
                    DrawSelectDaPai(Chang.ZiMoFan, ziJiaShi, x);
                    DrawDaiPai(Chang.ZiMoFan, x);
                }
                else if (ziJiaShi.ZiJiaYao == QiaoShi.YaoDingYi.JiuZhongJiuPai || ziJiaShi.ZiJiaYao == QiaoShi.YaoDingYi.ZiMo || ziJiaShi.ZiJiaYao == QiaoShi.YaoDingYi.LiZhi || ziJiaShi.ZiJiaYao == QiaoShi.YaoDingYi.AnGang || ziJiaShi.ZiJiaYao == QiaoShi.YaoDingYi.JiaGang)
                {
                    DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -1, true, false);
                }
                else
                {
                    DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -1, true, ziJiaShi.Follow);
                }
                if (!(sheDing.liZhiAuto && ziJiaShi.LiZhi) || ziJiaShi.HeLe || ziJiaShi.AnGangPaiWei.Count > 0 || ziJiaShi.JiaGangPaiWei.Count > 0)
                {
                    if (ziJiaShi.Follow && ziJiaShi.ZiJiaYao == QiaoShi.YaoDingYi.Wu)
                    {
                        DrawDaiPai(Chang.ZiMoFan, ziJiaShi.ZiJiaXuanZe);
                    }
                    yield return Pause(ForwardMode.NORMAL);
                }
                Chang.ZiJiaYao = ziJiaShi.ZiJiaYao;
                Chang.ZiJiaXuanZe = ziJiaShi.ZiJiaXuanZe;
                ClearGameObject(ref goYao);
                ClearGameObject(ref goLeft);
                ClearGameObject(ref goRight);
                ClearGameObject(ref goSelect);
            }
            else
            {
                ziJiaShi.SiKaoZiJia();
                Chang.ZiJiaYao = ziJiaShi.ZiJiaYao;
                Chang.ZiJiaXuanZe = ziJiaShi.ZiJiaXuanZe;
            }
            // 錯和自家判定
            if (ziJiaShi.CuHeZiJiaPanDing())
            {
                // 錯和
                Chang.CuHe(Chang.ZiMoFan);
                // 描画
                DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -1);
                DrawShePai(Chang.ZiMoFan);
                DrawCuHe(Chang.ZiMoFan);
                tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                yield return Pause(ForwardMode.FAST_FORWARD);
                eventStatus = Event.DIAN_BIAO_SHI;
                yield break;
            }

            switch (Chang.ZiJiaYao)
            {
                case QiaoShi.YaoDingYi.ZiMo:
                    // 自摸
                    ziJiaShi.HeLeChuLi();
                    Chang.HeleFan = Chang.ZiMoFan;
                    // 描画
                    DrawZiMo(Chang.ZiMoFan);
                    DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.HeLe, -1);
                    DrawXuanShangPai();
                    tingPaiLianZhuang = (Chang.ZiMoFan == Chang.Qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG);
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.YI_BIAO_SHI;
                    yield break;

                case QiaoShi.YaoDingYi.JiuZhongJiuPai:
                    // 九種九牌
                    // 描画
                    Chang.JiuZhongJiuPaiChuLi();
                    DrawJiuZhongJiuPai(Chang.ZiMoFan);
                    tingPaiLianZhuang = GuiZe.jiuZhongJiuPaiLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    DrawShouPai(Chang.ZiMoFan, Chang.ZiJiaYao, 0);
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.DIAN_BIAO_SHI;
                    yield break;

                case QiaoShi.YaoDingYi.LiZhi:
                    // 立直
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.LingShanChuLi();
                    // 立直
                    ziJiaShi.LiZi();
                    break;

                case QiaoShi.YaoDingYi.AnGang:
                    // 暗槓
                    ziJiaShi.AnGang(Chang.ZiJiaXuanZe);
                    // 描画
                    // 消
                    foreach (QiaoShi shi in Chang.QiaoShis)
                    {
                        shi.Xiao();
                    }
                    // 嶺上牌処理
                    Pai.LingShangPaiChuLi();
                    // 描画
                    DrawAnGang(Chang.ZiMoFan);
                    DrawJuFrame();
                    DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -1);
                    DrawXuanShangPai();
                    // 四開槓判定
                    if (Pai.SiKaiGangPanDing())
                    {
                        // 描画
                        DrawSiKaiGangPanDing(Chang.ZiMoFan);
                        tingPaiLianZhuang = GuiZe.siKaiGangLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        eventStatus = Event.DIAN_BIAO_SHI;
                        yield break;
                    }
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);
                    isDuiJuCoroutine = false;
                    eventStatus = Event.DUI_JU;
                    yield break;

                case QiaoShi.YaoDingYi.JiaGang:
                    // 加槓
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.LingShanChuLi();
                    // 加槓
                    ziJiaShi.JiaGang(Chang.ZiJiaXuanZe);
                    Pai.QiangGang();
                    // 描画
                    DrawJuFrame();
                    DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);
                    break;

                default:
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.LingShanChuLi();
                    break;
            }

            if (Chang.ZiJiaYao == QiaoShi.YaoDingYi.Wu || Chang.ZiJiaYao == QiaoShi.YaoDingYi.LiZhi)
            {
                // 描画
                DrawDaiPai(Chang.ZiMoFan, Chang.ZiJiaXuanZe);
                // 打牌前
                int dp = ziJiaShi.DaPaiQian();
                DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -2, true, false);
                yield return new WaitForSeconds(waitTime / 2);
                // 打牌
                ziJiaShi.DaPai(dp);
                Chang.QiaoShis[Chang.ZiMoFan].ShePaiChuLi(Chang.ZiJiaYao);
                DrawShePai(Chang.ZiMoFan);
                yield return new WaitForSeconds(waitTime / 2);
                if (Chang.ZiJiaYao == QiaoShi.YaoDingYi.LiZhi)
                {
                    DrawLiZi(Chang.ZiMoFan);
                }
                ziJiaShi.LiPai();
                DrawShouPai(Chang.ZiMoFan, QiaoShi.YaoDingYi.Wu, -2);
                // 四風子連打処理
                Chang.SiFengZiLianDaChuLi(Chang.ShePai);
                // 四風子連打判定
                if (Chang.SiFengZiLianDa)
                {
                    // 描画
                    DrawSiFengZiLianDa(Chang.ZiMoFan);
                    tingPaiLianZhuang = GuiZe.siFengZiLianDaLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.DIAN_BIAO_SHI;
                    yield break;
                }
            }

            switch (Chang.TaJiaYao)
            {
                case QiaoShi.YaoDingYi.DaMingGang:
                    // 大明槓成立
                    // 嶺上牌処理
                    Pai.LingShangPaiChuLi();
                    // 描画
                    DrawXuanShangPai();
                    // 四開槓判定
                    if (Pai.SiKaiGangPanDing())
                    {
                        // 描画
                        DrawSiKaiGangPanDing(Chang.ZiMoFan);
                        tingPaiLianZhuang = GuiZe.siKaiGangLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        eventStatus = Event.DIAN_BIAO_SHI;
                        yield break;
                    }
                    break;

                default:
                    break;
            }

            // 副露判定初期化
            Chang.TaJiaYao = QiaoShi.YaoDingYi.Wu;
            Chang.MingFan = Chang.ZiMoFan;
            for (int i = Chang.ZiMoFan + 1; i < Chang.ZiMoFan + Chang.QiaoShis.Count; i++)
            {
                int jia = i % Chang.QiaoShis.Count;
                QiaoShi taJiaShi = Chang.QiaoShis[jia];
                // 思考他家判定
                taJiaShi.ZiJiaYao = QiaoShi.YaoDingYi.Wu;
                taJiaShi.ZiJiaXuanZe = 0;
                taJiaShi.SiKaoTaJiaPanDing(i - Chang.ZiMoFan);
                // 思考他家
                if (taJiaShi.Player)
                {
                    continue;
                }
                taJiaShi.SiKaoTaJia(jia);
                // 錯和他家判定
                if (taJiaShi.CuHeTaJiaPanDing())
                {
                    Chang.CuHe(jia);
                    // 描画
                    DrawCuHe(jia);
                    tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.DIAN_BIAO_SHI;
                    yield break;
                }
                if ((taJiaShi.TaJiaYao == QiaoShi.YaoDingYi.RongHe && Chang.TaJiaYao < QiaoShi.YaoDingYi.RongHe)
                    || (taJiaShi.TaJiaYao == QiaoShi.YaoDingYi.DaMingGang && Chang.TaJiaYao < QiaoShi.YaoDingYi.DaMingGang)
                    || (taJiaShi.TaJiaYao == QiaoShi.YaoDingYi.Bing && Chang.TaJiaYao < QiaoShi.YaoDingYi.Bing)
                    || (taJiaShi.TaJiaYao == QiaoShi.YaoDingYi.Chi && Chang.TaJiaYao < QiaoShi.YaoDingYi.Chi))
                {
                    // 栄和、大明槓、石並、吃
                    Chang.TaJiaYao = taJiaShi.TaJiaYao;
                    Chang.TaJiaXuanZe = taJiaShi.TaJiaXuanZe;
                    Chang.MingFan = jia;
                }
                if (GuiZe.wRongHe && taJiaShi.TaJiaYao == QiaoShi.YaoDingYi.RongHe)
                {
                    Chang.RongHeFan.Add(jia);
                }
            }
            // 思考他家プレイヤー分
            for (int i = Chang.ZiMoFan + 1; i < Chang.ZiMoFan + Chang.QiaoShis.Count; i++)
            {
                int jia = i % Chang.QiaoShis.Count;
                QiaoShi taJiaShi = Chang.QiaoShis[jia];
                if (!taJiaShi.Player)
                {
                    continue;
                }
                if (sheDing.mingWu && (!taJiaShi.HeLe))
                {
                    continue;
                }
                if (taJiaShi.HeLe || taJiaShi.ChiPaiWei.Count > 0 || taJiaShi.BingPaiWei.Count > 0 || taJiaShi.DaMingGangPaiWei.Count > 0)
                {
                    if ((taJiaShi.HeLe && Chang.TaJiaYao < QiaoShi.YaoDingYi.RongHe)
                        || (GuiZe.wRongHe && taJiaShi.HeLe)
                        || (taJiaShi.DaMingGangPaiWei.Count > 0 && Chang.TaJiaYao < QiaoShi.YaoDingYi.DaMingGang)
                        || (taJiaShi.BingPaiWei.Count > 0 && Chang.TaJiaYao < QiaoShi.YaoDingYi.Bing)
                        || (taJiaShi.ChiPaiWei.Count > 0 && Chang.TaJiaYao < QiaoShi.YaoDingYi.Chi))
                    {
                        DrawShePai(Chang.ZiMoFan, true);

                        taJiaShi.SiKaoTaJia(jia);
                        DrawTaJiaYao(jia, taJiaShi, 0, true);
                        DrawShouPai(jia, QiaoShi.YaoDingYi.Wu, -2);
                        yield return Pause(ForwardMode.NORMAL);
                        ClearGameObject(ref goYao);
                        if (Chang.RongHeFan.Count == 0)
                        {
                            Chang.TaJiaYao = taJiaShi.TaJiaYao;
                            Chang.TaJiaXuanZe = taJiaShi.TaJiaXuanZe;
                            Chang.MingFan = jia;
                        }
                        if (GuiZe.wRongHe && taJiaShi.TaJiaYao == QiaoShi.YaoDingYi.RongHe)
                        {
                            Chang.RongHeFan.Add(jia);
                        }
                        DrawShePai(Chang.ZiMoFan);
                    }
                }
            }

            if (Chang.TaJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                // 栄和
                if (Chang.RongHeFan.Count == 0)
                {
                    Chang.RongHeFan.Add(Chang.MingFan);
                }
                Chang.RongHeFan.Sort();
                Chang.MingFan = Chang.RongHeFan[0];

                // 捨牌処理
                Chang.QiaoShis[Chang.ZiMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.RongHe);
                foreach (int fan in Chang.RongHeFan)
                {
                    // 描画
                    DrawRongHe(fan);
                    DrawShouPai(fan, QiaoShi.YaoDingYi.HeLe, -1);
                    Chang.QiaoShis[fan].ZiMo(Chang.ShePai);
                    // 和了
                    Chang.QiaoShis[fan].HeLeChuLi();
                }
                Chang.HeleFan = Chang.MingFan;
                DrawXuanShangPai();
                tingPaiLianZhuang = (Chang.MingFan == Chang.Qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG);
                yield return Pause(ForwardMode.FAST_FORWARD);
                eventStatus = Event.YI_BIAO_SHI;
                yield break;
            }

            for (int i = Chang.ZiMoFan + 1; i < Chang.ZiMoFan + Chang.QiaoShis.Count; i++)
            {
                // 振聴牌処理
                Chang.QiaoShis[i % Chang.QiaoShis.Count].ZhenTingPaiChuLi();
            }

            switch (Chang.ZiJiaYao)
            {
                case QiaoShi.YaoDingYi.LiZhi:
                    // 立直成立
                    Chang.LiZhiChuLi();
                    Chang.QiaoShis[Chang.ZiMoFan].LiZiChuLi();
                    Chang.QiaoShis[Chang.ZiMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.LiZhi);
                    // 描画
                    DrawShePai(Chang.ZiMoFan);
                    DrawLiZi(Chang.ZiMoFan);
                    DrawJuFrame();
                    // 四家立直判定
                    if (Chang.SiJiaLiZhiPanDing())
                    {
                        // 描画
                        DrawSiJiaLiZhi(Chang.ZiMoFan);
                        tingPaiLianZhuang = GuiZe.siJiaLiZhiLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        eventStatus = Event.DIAN_BIAO_SHI;
                        yield break;
                    }
                    break;

                case QiaoShi.YaoDingYi.JiaGang:
                    // 加槓成立
                    // 消
                    foreach (QiaoShi shi in Chang.QiaoShis)
                    {
                        shi.Xiao();
                    }
                    // 加槓処理
                    Pai.QiangGangChuLi();
                    // 嶺上牌処理
                    Pai.LingShangPaiChuLi();
                    // 描画
                    DrawJuFrame();
                    DrawJiaGang(Chang.ZiMoFan);
                    DrawXuanShangPai();
                    // 四開槓判定
                    if (Pai.SiKaiGangPanDing())
                    {
                        // 描画
                        DrawSiKaiGangPanDing(Chang.ZiMoFan);
                        tingPaiLianZhuang = GuiZe.siKaiGangLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        eventStatus = Event.DIAN_BIAO_SHI;
                        yield break;
                    }
                    eventStatus = Event.DUI_JU;
                    isDuiJuCoroutine = false;
                    yield break;

                default:
                    break;
            }

            switch (Chang.TaJiaYao)
            {
                case QiaoShi.YaoDingYi.DaMingGang:
                    // 大明槓
                    Chang.QiaoShis[Chang.MingFan].DaMingGang();
                    // 描画
                    DrawDaMingGang(Chang.MingFan);
                    // 捨牌処理
                    Chang.QiaoShis[Chang.ZiMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.DaMingGang);
                    // 消
                    foreach (QiaoShi shi in Chang.QiaoShis)
                    {
                        shi.Xiao();
                    }
                    // 描画
                    DrawShePai(Chang.ZiMoFan);
                    DrawJuFrame();
                    DrawShouPai(Chang.MingFan, QiaoShi.YaoDingYi.Wu, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);

                    Chang.ZiMoFan = Chang.MingFan;
                    eventStatus = Event.DUI_JU;
                    isDuiJuCoroutine = false;
                    yield break;

                case QiaoShi.YaoDingYi.Bing:
                    // 石並
                    Chang.QiaoShis[Chang.MingFan].Bing();
                    // 描画
                    DrawBing(Chang.MingFan);
                    // 捨牌処理
                    Chang.QiaoShis[Chang.ZiMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.Bing);
                    // 消
                    foreach (QiaoShi shi in Chang.QiaoShis)
                    {
                        shi.Xiao();
                    }
                    // 描画
                    DrawShePai(Chang.ZiMoFan);
                    DrawShouPai(Chang.MingFan, QiaoShi.YaoDingYi.Wu, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);

                    Chang.ZiMoFan = Chang.MingFan;
                    eventStatus = Event.DUI_JU;
                    isDuiJuCoroutine = false;
                    yield break;

                case QiaoShi.YaoDingYi.Chi:
                    // 吃
                    Chang.QiaoShis[Chang.MingFan].Chi();
                    // 描画
                    DrawChi(Chang.MingFan);
                    // 捨牌処理
                    Chang.QiaoShis[Chang.ZiMoFan].ShePaiChuLi(QiaoShi.YaoDingYi.Chi);
                    // 消
                    foreach (QiaoShi shi in Chang.QiaoShis)
                    {
                        shi.Xiao();
                    }
                    // 描画
                    DrawShePai(Chang.ZiMoFan);
                    DrawShouPai(Chang.MingFan, QiaoShi.YaoDingYi.Wu, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);

                    Chang.ZiMoFan = Chang.MingFan;
                    eventStatus = Event.DUI_JU;
                    isDuiJuCoroutine = false;
                    yield break;

                default:
                    break;
            }

            // 流局判定
            if (Pai.LiuJuPanDing())
            {
                // 流し満貫判定
                int liuManGuan = -1;
                if (GuiZe.liuManGuan)
                {
                    for (int i = 0; i < Chang.QiaoShis.Count; i++)
                    {
                        int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                        if (Chang.QiaoShis[jia].LiuJu())
                        {
                            Chang.ZiJiaYao = QiaoShi.YaoDingYi.ZiMo;
                            Chang.ZiMoFan = jia;
                            Chang.HeleFan = jia;
                            liuManGuan = jia;
                            break;
                        }
                    }
                }
                tingPaiLianZhuang = Zhuang.LUN_ZHUANG;
                if (GuiZe.nanChangBuTingLianZhuang && Chang.ChangFeng >= 0x32)
                {
                    tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                }
                for (int i = 0; i < Chang.QiaoShis.Count; i++)
                {
                    int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                    QiaoShi shi = Chang.QiaoShis[jia];
                    // 形聴判定
                    shi.XingTingPanDing();
                    if (jia == liuManGuan)
                    {
                        // 流し満貫
                        // 描画
                        DrawLiuManGuan(jia);
                        if (jia == Chang.Qin)
                        {
                            tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }

                    }
                    else if (shi.XingTing)
                    {
                        // 聴牌
                        // 描画
                        DrawTingPai(jia);
                        if (jia == Chang.Qin)
                        {
                            tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                    else
                    {
                        // 不聴
                        if (shi.LiZhi)
                        {
                            // 錯和(不聴立直)
                            Chang.CuHe(jia);
                            // 描画
                            DrawCuHe(jia);
                            tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                            yield return Pause(ForwardMode.FAST_FORWARD);
                            eventStatus = Event.DIAN_BIAO_SHI;
                            yield break;
                        }
                        // 描画
                        DrawBuTing(jia);
                    }
                    yield return new WaitForSeconds(waitTime);
                }

                yield return Pause(ForwardMode.FAST_FORWARD);

                eventStatus = Event.DIAN_BIAO_SHI;
                yield break;
            }

            Chang.ZiMoFan = (Chang.ZiMoFan + 1) % Chang.QiaoShis.Count;
            eventStatus = Event.DUI_JU;
            isDuiJuCoroutine = false;
        }

        // 【描画】自家腰
        private void DrawZiJiaYao(QiaoShi shi, int mingWei, int ShouPaiWei, bool isFollow, bool isPass)
        {
            if (!shi.Player)
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
            if (shi.HeLe && shi.TaJiaYao == QiaoShi.YaoDingYi.Wu)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.ZiMo, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (!shi.LiZhi && shi.LiZhiPaiWei.Count > 0)
            {
                int wei = mingWei;
                if (isFollow && shi.ZiJiaYao == QiaoShi.YaoDingYi.LiZhi)
                {
                    for (int i = 0; i < shi.LiZhiPaiWei.Count; i++)
                    {
                        int w = shi.LiZhiPaiWei[i];
                        if (w == shi.ZiJiaXuanZe)
                        {
                            wei = i;
                            break;
                        }
                    }
                }
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.LiZhi, wei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.AnGangPaiWei.Count > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.AnGang, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.JiaGangPaiWei.Count > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.YaoDingYi.JiaGang, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.JiuZhongJiuPai)
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
            go = Instantiate(goButton, goButton.transform.parent);
            go.onClick.AddListener(delegate {
                OnClickZiJiaYao(Chang.ZiMoFan, shi, yao, mingWei, ShouPaiWei, isFollow);
            });
            string value = shi.YaoMingButton(yao);
            if (yao == QiaoShi.YaoDingYi.Wu)
            {
                value = shi.YaoMingButton(QiaoShi.YaoDingYi.Clear);
            }
            if (shi.Follow && (shi.ZiJiaYao != yao || shi.ZiJiaYao == QiaoShi.YaoDingYi.Wu))
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
                DrawShouPai(jia, yao, -1, true, false);
                DrawDaiPai(jia, -1);
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                {
                    DrawSelectDaPai(jia, shi, shi.ShouPai.Count - 1);
                }
            }
            else if (yao == QiaoShi.YaoDingYi.LiZhi)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.LiZhiPaiWei.Count, ShouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, true, isFollow);
                DrawDaiPai(jia, shi.LiZhiPaiWei[mingWei]);
            }
            else if (yao == QiaoShi.YaoDingYi.AnGang && shi.AnGangPaiWei.Count > 1)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.AnGangPaiWei.Count, ShouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, true, isFollow);
            }
            else if (yao == QiaoShi.YaoDingYi.JiaGang && shi.JiaGangPaiWei.Count > 1)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.JiaGangPaiWei.Count, ShouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, true, isFollow);
            }
            else if (yao == QiaoShi.YaoDingYi.Select)
            {
                DrawZiJiaYao(shi, mingWei, ShouPaiWei, true, false);
                DrawShouPai(jia, yao, ShouPaiWei, true, isFollow);
                DrawDaiPai(jia, ShouPaiWei);
            }
            else
            {
                shi.ZiJiaYao = yao;
                shi.ZiJiaXuanZe = mingWei;
                keyPress = true;
            }
        }

        // 【描画】他家腰
        private void DrawTaJiaYao(int jia, QiaoShi shi, int mingWei, bool isFollow)
        {
            if (!shi.Player)
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
            if (shi.HeLe)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.RongHe, mingWei);
                x += width;
                index++;
            }
            if (shi.ChiPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.Chi, isFollow && shi.TaJiaYao == QiaoShi.YaoDingYi.Chi ? shi.TaJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.BingPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.Bing, isFollow && shi.TaJiaYao == QiaoShi.YaoDingYi.Bing ? shi.TaJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.DaMingGangPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.YaoDingYi.DaMingGang, mingWei);
                index++;
            }
            if (index > 0)
            {
                isQuXiao = true;
                if (sheDing.mingQuXiao)
                {
                    DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(paiWidth * 7.5f, y), QiaoShi.YaoDingYi.Wu, mingWei);
                }
            }
        }

        // 【描画】他家腰
        private void DrawOnClickTaJiaYao(ref Button go, int jia, QiaoShi shi, Vector2 xy, QiaoShi.YaoDingYi yao, int mingWei)
        {
            go = Instantiate(goButton, goButton.transform.parent);
            go.onClick.AddListener(delegate {
                OnClickTaJiaYao(jia, shi, yao, mingWei);
            });
            string value = shi.YaoMingButton(yao);
            if (shi.Follow && shi.TaJiaYao != yao)
            {
                TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.gray;
            }
            DrawButton(ref go, value, xy);
        }

        // 他家腰クリック
        private void OnClickTaJiaYao(int jia, QiaoShi shi, QiaoShi.YaoDingYi yao, int mingWei)
        {
            if (yao == QiaoShi.YaoDingYi.Bing && shi.BingPaiWei.Count > 1)
            {
                DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.BingPaiWei.Count, false);
                DrawShouPai(jia, yao, mingWei);
            }
            else if (yao == QiaoShi.YaoDingYi.Chi && shi.ChiPaiWei.Count > 1)
            {
                DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.ChiPaiWei.Count, false);
                DrawShouPai(jia, yao, mingWei);
            }
            else
            {
                shi.TaJiaYao = yao;
                shi.TaJiaXuanZe = 0;
                isQuXiao = false;
                keyPress = true;
            }
        }

        // 【描画】手牌
        private void DrawShouPai(int jia, QiaoShi.YaoDingYi yao, int mingWei)
        {
            DrawShouPai(jia, yao, mingWei, false, false);
        }
        private void DrawShouPai(int jia, QiaoShi.YaoDingYi yao, int mingWei, bool isPlayerZiMo, bool isFollow)
        {
            QiaoShi shi = Chang.QiaoShis[jia];
            ClearGameObject(ref shi.goShouPai);
            for (int i = 0; i < shi.goFuLuPai.Length; i++)
            {
                ClearGameObject(ref shi.goFuLuPai[i]);
            }

            float pw = paiWidth;
            float ph = paiHeight;

            if (shi.Player)
            {
                pw *= PLAYER_PAI_SCALE;
                if (orientation != ScreenOrientation.Portrait)
                {
                    pw *= PLAYER_PAI_SCALE_LANDSCAPE;
                }
            }

            float x = -(pw * 6.6f);
            float y = -(pw * 3f + ph * 3.8f);
            if (shi.Player)
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
            for (int i = 0; i < shi.ShouPai.Count; i++)
            {
                int p = shi.ShouPai[i];
                shi.goShouPai[i] = Instantiate(goPai, goPai.transform.parent);
                if (shi.PlayOrder != 0 && yao != QiaoShi.YaoDingYi.TingPai && yao != QiaoShi.YaoDingYi.HeLe && yao != QiaoShi.YaoDingYi.JiuZhongJiuPai && yao != QiaoShi.YaoDingYi.CuHe && p != 0xff)
                {
                    if (!shouPaiOpen)
                    {
                        p = 0x00;
                    }
                    shi.goShouPai[i].transform.SetSiblingIndex(0);
                }
                float yy = y;
                if (shi.Player)
                {
                    shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE;
                    if (orientation != ScreenOrientation.Portrait)
                    {
                        shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE_LANDSCAPE;
                    }

                    int wei = i;
                    if (yao == QiaoShi.YaoDingYi.LiZhi)
                    {
                        if (shi.LiZhiPaiWei[mingWei] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, wei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.AnGang)
                    {
                        if (shi.AnGangPaiWei[mingWei][0] == i || shi.AnGangPaiWei[mingWei][1] == i || shi.AnGangPaiWei[mingWei][2] == i || shi.AnGangPaiWei[mingWei][3] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.JiaGang)
                    {
                        if (shi.JiaGangPaiWei[mingWei][0] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Bing)
                    {
                        if (shi.BingPaiWei[mingWei][0] == i || shi.BingPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Chi)
                    {
                        if (shi.ChiPaiWei[mingWei][0] == i || shi.ChiPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Wu && mingWei >= -1)
                    {
                        if (isFollow && shi.ZiJiaXuanZe == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, QiaoShi.YaoDingYi.DaPai, wei); });
                            yy = y + margin;
                        }
                        else if (!shi.LiZhi || shi.ShouPai.Count - 1 == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, wei); });
                        }
                    }
                    if (yao == QiaoShi.YaoDingYi.Select)
                    {
                        if (mingWei == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, QiaoShi.YaoDingYi.DaPai, wei); });
                            yy = y + margin;
                        }
                        else
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, QiaoShi.YaoDingYi.Wu, wei); });
                        }
                    }
                    if (isPlayerZiMo && shi.ShouPai.Count - 1 == i && !(shi.TaJiaYao == QiaoShi.YaoDingYi.Bing || shi.TaJiaYao == QiaoShi.YaoDingYi.Chi))
                    {
                        x += pw / 5;
                    }
                    if (sheDing.xuanShangYin)
                    {
                        shi.goShouPai[i].GetComponentInChildren<TextMeshProUGUI>().text = (shi.XuanShangPaiPanDing(shi.ShouPai[i]) > 0) ? "▼" : "";
                    }
                }
                if (p != 0xff)
                {
                    if (p == 0x00 && !shi.Player && yao != QiaoShi.YaoDingYi.BuTing)
                    {
                        DrawJiXiePai(ref shi.goShouPai[i], Cal(x, yy, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder));
                    }
                    else
                    {
                        float yyy = yy;
                        if (orientation != ScreenOrientation.Portrait && shi.Player)
                        {
                            yyy += ph * 0.45f;
                        }
                        DrawPai(ref shi.goShouPai[i], p, Cal(x, yyy, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder));
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
            if (shi.Player)
            {
                x += paiWidth * 1.5f;
            }
            y = -(paiWidth * 3f + paiHeight * 4.1f);
            if (shi.Player)
            {
                y -= paiHeight;
                if (orientation != ScreenOrientation.Portrait)
                {
                    x += paiWidth * 2f;
                    y = -Screen.safeArea.height / 2 + ph / 2;
                }
            }
            for (int i = 0; i < shi.FuLuPai.Count; i++)
            {
                (List<int> pais, int ji, QiaoShi.YaoDingYi fYao) = shi.FuLuPai[i];
                for (int j = pais.Count - 1; j >= 0; j--)
                {
                    int p = pais[j];
                    if (p == 0xff)
                    {
                        continue;
                    }
                    if (fYao == QiaoShi.YaoDingYi.JiaGang && j == 3)
                    {
                        continue;
                    }
                    bool isMingPai = shi.MingPaiPanDing(fYao, ji, j);
                    if (fYao == QiaoShi.YaoDingYi.AnGang && (j == 0 || j == 3))
                    {
                        p = 0x00;
                    }
                    shi.goFuLuPai[i][j] = Instantiate(goPai, goPai.transform.parent);
                    shi.goFuLuPai[i][j].transform.SetSiblingIndex(2);
                    shi.goFuLuPai[i][j].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.PlayOrder));
                    if (fYao == QiaoShi.YaoDingYi.JiaGang && isMingPai)
                    {
                        shi.goFuLuPai[i][3] = Instantiate(goPai, goPai.transform.parent);
                        shi.goFuLuPai[i][3].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.PlayOrder));
                    }
                    if (isMingPai)
                    {
                        x -= (paiHeight / 2);
                        DrawPai(ref shi.goFuLuPai[i][j], p, Cal(x, y - (paiHeight - paiWidth) / 2, shi.PlayOrder), 90);
                        if (fYao == QiaoShi.YaoDingYi.JiaGang)
                        {
                            DrawPai(ref shi.goFuLuPai[i][3], p, Cal(x, y - (paiHeight - paiWidth) / 2 + paiWidth, shi.PlayOrder), 90);
                        }
                        x -= (paiHeight / 2);
                    }
                    else
                    {
                        x -= (paiWidth / 2);
                        DrawPai(ref shi.goFuLuPai[i][j], p, Cal(x, y, shi.PlayOrder), 0);
                        x -= (paiWidth / 2);
                    }
                }
            }
        }

        // 【描画】牌選択ボタン
        private void DrawSelectDaPai(int jia, QiaoShi shi, int xuanZe)
        {
            ClearGameObject(ref goLeft);
            ClearGameObject(ref goRight);
            ClearGameObject(ref goSelect);

            DrawShouPai(jia, QiaoShi.YaoDingYi.Select, xuanZe, true, true);

            goLeft = Instantiate(goButton, goButton.transform.parent);
            int leftWei = xuanZe - 1;
            if (leftWei < 0)
            {
                leftWei = shi.ShouPai.Count - 1;
            }
            goLeft.onClick.AddListener(delegate {
                DrawSelectDaPai(jia, shi, leftWei);
                DrawDaiPai(jia, leftWei);
            });

            goRight = Instantiate(goButton, goButton.transform.parent);
            int rightWei = xuanZe + 1;
            if (rightWei > shi.ShouPai.Count - 1)
            {
                rightWei = 0;
            }
            goRight.onClick.AddListener(delegate {
                DrawSelectDaPai(jia, shi, rightWei);
                DrawDaiPai(jia, rightWei);
            });

            goSelect = Instantiate(goButton, goButton.transform.parent);
            goSelect.onClick.AddListener(delegate {
                shi.ZiJiaYao = QiaoShi.YaoDingYi.Wu;
                shi.ZiJiaXuanZe = xuanZe;
                keyPress = true;
            });

            OrientationSelectDaPai();
        }

        private void OrientationSelectDaPai()
        {
            float x = -(paiWidth * 8.5f);
            float y = -(paiHeight * 9.3f);
            if (orientation != ScreenOrientation.Portrait)
            {
                x = -(paiWidth * 14.5f);
                y = -(paiHeight * 4f);
            }
            if (goLeft != null)
            {
                DrawButton(ref goLeft, "◀", new Vector2(x, y));
            }
            if (goRight != null)
            {
                DrawButton(ref goRight, "▶", new Vector2(x + paiWidth * 2.5f, y));
            }

            x = -(paiWidth * 3f);
            if (orientation != ScreenOrientation.Portrait)
            {
                x = -paiWidth * 13.2f;
                y += paiHeight * 1.5f;
            }
            if (goSelect != null)
            {
                DrawButton(ref goSelect, "打牌", new Vector2(x, y));
            }
        }

        // 牌クリック
        private void OnClickShouPai(int jia, QiaoShi shi, QiaoShi.YaoDingYi yao, int xuanZe)
        {
            if (yao == QiaoShi.YaoDingYi.LiZhi || yao == QiaoShi.YaoDingYi.AnGang || yao == QiaoShi.YaoDingYi.JiaGang)
            {
                shi.ZiJiaYao = yao;
                shi.ZiJiaXuanZe = xuanZe;
            }
            if (yao == QiaoShi.YaoDingYi.Bing || yao == QiaoShi.YaoDingYi.Chi)
            {
                shi.TaJiaYao = yao;
                shi.TaJiaXuanZe = xuanZe;
            }
            if (yao == QiaoShi.YaoDingYi.Wu)
            {
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.TAP_1)
                {
                    shi.ZiJiaYao = QiaoShi.YaoDingYi.Wu;
                    shi.ZiJiaXuanZe = xuanZe;
                }
                else
                {
                    DrawShouPai(jia, QiaoShi.YaoDingYi.Select, xuanZe, true, true);
                    DrawDaiPai(jia, xuanZe);
                    if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                    {
                        DrawSelectDaPai(jia, shi, xuanZe);
                    }
                    return;
                }
            }
            if (yao == QiaoShi.YaoDingYi.DaPai)
            {
                shi.ZiJiaYao = QiaoShi.YaoDingYi.Wu;
                shi.ZiJiaXuanZe = xuanZe;
                DrawShouPai(jia, QiaoShi.YaoDingYi.Clear, 0, false, false);
            }
            keyPress = true;
        }

        // 【描画】待牌
        private void DrawDaiPai(int jia, int xuanZe)
        {
            QiaoShi shi = Chang.QiaoShis[jia];
            // 待牌
            if (!shi.Player)
            {
                return;
            }
            ClearGameObject(ref shi.goDaiPai);
            ClearGameObject(ref shi.goCanPaiShu);
            ClearGameObject(ref shi.goXiangTingShu);
            if (xuanZe == -1)
            {
                return;
            }

            float x = -(paiWidth * 8.5f);
            float y = -(paiHeight * 7.8f);
            if (orientation != ScreenOrientation.Portrait)
            {
                x = paiWidth * 11f;
                y = -(paiHeight * 4f);
            }

            if (sheDing.daiPaiBiaoShi)
            {
                if (xuanZe != -2)
                {
                    shi.DaiPaiJiSuan(xuanZe);
                    shi.GongKaiPaiShuJiSuan();
                }
                for (int i = 0; i < shi.DaiPai.Count; i++)
                {
                    int p = shi.DaiPai[i] & QiaoShi.QIAO_PAI;
                    if (p == 0xff)
                    {
                        ClearGameObject(ref shi.goDaiPai[i]);
                    }
                    else
                    {
                        DrawPai(ref shi.goDaiPai[i], p, Cal(x, y, shi.PlayOrder), 0);
                        DrawText(ref shi.goCanPaiShu[i], Pai.CanShu(shi.GongKaiPaiShu[p]).ToString(), Cal(x, y + paiWidth * 1.2f, shi.PlayOrder), 0, 17);
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
                if (shi.DaiPai.Count == 0)
                {
                    if (xuanZe != -2)
                    {
                        shi.XiangTingShuJiSuan(xuanZe);
                    }
                    if (shi.XiangTingShu > 0)
                    {
                        DrawText(ref shi.goXiangTingShu, shi.XiangTingShu.ToString() + "シャンテン", Cal(x, y, shi.PlayOrder), 0, 18);
                    }
                }
            }
        }

        // 【描画】捨牌
        private void DrawShePai(int jia)
        {
            DrawShePai(jia, false);
        }
        private void DrawShePai(int jia, bool ming)
        {
            QiaoShi shi = Chang.QiaoShis[jia];

            ClearGameObject(ref shi.goShePai);
            int shePaiEnter = 6;
            float shePaiLeft = 2.5f;
            if (Chang.QiaoShis.Count == 2)
            {
                shePaiLeft = 8.5f;
                shePaiEnter = 18;
            }
            float x = -(paiWidth * shePaiLeft);
            float y = -(paiHeight * 2.6f);

            int shu = 0;
            bool isDrawLizhi = !shi.LiZhi;
            float dark = 0.8f;
            int playOrder = shi.PlayOrder;
            if (Chang.QiaoShis.Count == 2 && playOrder == 1)
            {
                playOrder = 2;
            }
            int index = 0;
            foreach ((int pai, QiaoShi.YaoDingYi yao, bool ziMoQie) in shi.ShePai)
            {
                if (yao == QiaoShi.YaoDingYi.Wu || yao == QiaoShi.YaoDingYi.LiZhi)
                {
                    shu++;

                    shi.goShePai[index] = Instantiate(goPai, goPai.transform.parent);
                    shi.goShePai[index].transform.SetSiblingIndex(3);
                    if (yao == QiaoShi.YaoDingYi.LiZhi || (!isDrawLizhi && shi.LiZhiWei < index))
                    {
                        x += (paiHeight - paiWidth) / 2;
                        shi.goShePai[index].transform.Rotate(0, 0, 90);
                        isDrawLizhi = true;
                        Button b = shi.goShePai[index];
                        DrawPai(ref b, pai, Cal(x, y, playOrder), 90 * GetDrawOrder(playOrder));
                        x += (paiHeight - paiWidth) / 2;
                    }
                    else
                    {
                        if (ming && index == shi.ShePai.Count - 1)
                        {
                            shi.goShePai[index].GetComponentInChildren<TextMeshProUGUI>().text = "▼";
                            shi.goShePai[index].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                        }
                        Button b = shi.goShePai[index];
                        DrawPai(ref b, pai, Cal(x, y, playOrder), 90 * GetDrawOrder(playOrder));
                    }
                    if (sheDing.ziMoQieBiaoShi && ziMoQie)
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
        private void DrawShang(int jia, string text)
        {
            QiaoShi shi = Chang.QiaoShis[jia];
            if (goSheng[jia] == null)
            {
                goSheng[jia] = Instantiate(goSpeech, goSpeech.transform.parent);
            }
            TextMeshProUGUI goText = goSheng[jia].GetComponentInChildren<TextMeshProUGUI>();
            goText.text = text;
            goText.fontSize = 23;
            goSheng[jia].transform.SetAsLastSibling();
            goSheng[jia].onClick.AddListener(delegate {
                ClearGameObject(ref goSheng[jia]);
            });
            goSheng[jia].transform.rotation = Quaternion.Euler(0, 0, 90 * GetDrawOrder(shi.PlayOrder));
            RectTransform rt = goSheng[jia].GetComponent<RectTransform>();
            rt.anchoredPosition = Cal(0, -(paiWidth * 3 + paiHeight * 2.3f), shi.PlayOrder);
            rt.sizeDelta = new Vector2(goText.preferredWidth + paiWidth / 2, rt.sizeDelta.y);
        }

        // 【描画】自摸
        private void DrawZiMo(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.ZiMo));
        }

        // 【描画】九種九牌
        private void DrawJiuZhongJiuPai(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.JiuZhongJiuPai));
        }

        // 【描画】立直
        private void DrawLiZi(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.LiZhi));
        }

        // 【描画】暗槓
        private void DrawAnGang(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.AnGang));
        }

        // 【描画】流し満貫
        private void DrawLiuManGuan(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.LiuManGuan));
        }

        // 【描画】四開槓
        private void DrawSiKaiGangPanDing(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.SiKaiGang));
        }

        // 【描画】加槓
        private void DrawJiaGang(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.JiaGang));
        }

        // 【描画】大明槓
        private void DrawDaMingGang(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.DaMingGang));
        }

        // 【描画】石並
        private void DrawBing(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.Bing));
        }

        // 【描画】吃
        private void DrawChi(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.Chi));
        }

        // 【描画】四風子連打
        private void DrawSiFengZiLianDa(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.SiFengZiLianDa));
        }

        // 【描画】四家立直
        private void DrawSiJiaLiZhi(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.SiJiaLiZhi));
        }

        // 【描画】栄和
        private void DrawRongHe(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.RongHe));
        }

        // 【描画】聴牌
        private void DrawTingPai(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.TingPai));
            DrawShouPai(jia, QiaoShi.YaoDingYi.TingPai, -1);
        }

        // 【描画】不聴
        private void DrawBuTing(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.BuTing));
            DrawShouPai(jia, QiaoShi.YaoDingYi.BuTing, -1);
        }

        // 【描画】錯和
        private void DrawCuHe(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.YaoDingYi.CuHe) + " " + Chang.QiaoShis[Chang.CuHeFan].CuHeSheng);
            DrawShouPai(jia, QiaoShi.YaoDingYi.CuHe, 0);
        }

        // 【ゲーム】対局終了
        private IEnumerator DuiJuZhongLe()
        {
            isDuiJuZhongLeCoroutine = true;

            ClearScreen();
            DrawDuiJu();
            yield return Pause(ForwardMode.FAST_FORWARD);

            eventStatus = Event.YI_BIAO_SHI;
            isDuiJuZhongLeCoroutine = false;
        }

        // 【ゲーム】役
        private IEnumerator YiBiaoShi()
        {
            isYiBiaoShiCoroutine = true;
            isBackDuiJuZhongLe = false;

            if (Chang.RongHeFan.Count > 0)
            {
                foreach (int fan in Chang.RongHeFan)
                {
                    // 描画
                    ClearScreen();
                    DrawBackDuiJuZhongLe();
                    DrawYi(fan);

                    yield return Pause(ForwardMode.FAST_FORWARD);
                }
            }
            else
            {
                // 描画
                ClearScreen();
                DrawBackDuiJuZhongLe();
                DrawYi(Chang.HeleFan);

                yield return Pause(ForwardMode.FAST_FORWARD);
            }

            eventStatus = isBackDuiJuZhongLe ? Event.DUI_JU_ZHONG_LE : Event.DIAN_BIAO_SHI;
            isYiBiaoShiCoroutine = false;
        }

        // 【描画】対局終了へ戻る
        private void DrawBackDuiJuZhongLe()
        {
            // 戻るボタン
            goBackDuiJuZhongLe = Instantiate(goBack, goBack.transform.parent);
            goBackDuiJuZhongLe.onClick.AddListener(delegate {
                isBackDuiJuZhongLe = true;
                keyPress = true;
            });
            RectTransform rtBackDuiJuZhongLe = goBackDuiJuZhongLe.GetComponent<RectTransform>();
            rtBackDuiJuZhongLe.localScale *= scale.x;
            rtBackDuiJuZhongLe.anchorMin = new Vector2(0, 1);
            rtBackDuiJuZhongLe.anchorMax = new Vector2(0, 1);
            rtBackDuiJuZhongLe.pivot = new Vector2(0, 1);
            rtBackDuiJuZhongLe.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 0.5f));
        }

        // 【描画】役
        private void DrawYi(int jia)
        {
            float x;
            float y = paiWidth * 2.5f + paiHeight * 7.5f;
            if (jia >= 0)
            {
                DrawXuanShangPai();

                QiaoShi shi = Chang.QiaoShis[jia];

                // 名前
                x = 0f;
                y -= (paiHeight * 3.5f);
                DrawText(ref goMingQian[jia], shi.MingQian, new Vector2(x, y), 0, 30);

                // 手牌
                if (!shi.LiPaiDongZuo)
                {
                    int tp = shi.ShouPai[^1];
                    shi.ShouPai[^1] = 0xff;
                    shi.Sort();
                    shi.ShouPai[^1] = tp;
                }

                float pw = paiWidth * PLAYER_PAI_SCALE;
                x = -(pw * 6.5f);
                y -= paiHeight * 1.5f;
                float ph = paiHeight * PLAYER_PAI_SCALE;
                for (int i = 0; i < shi.ShouPai.Count; i++)
                {
                    int sp = shi.ShouPai[i];
                    shi.goShouPai[i] = Instantiate(goPai, goPai.transform.parent);
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
                for (int i = 0; i < shi.FuLuPai.Count; i++)
                {
                    (List<int> pais, int ji, QiaoShi.YaoDingYi zhong) = shi.FuLuPai[i];
                    for (int j = pais.Count - 1; j >= 0; j--)
                    {
                        int p = pais[j];
                        if (p == 0xff)
                        {
                            continue;
                        }
                        if (zhong == QiaoShi.YaoDingYi.JiaGang && j == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(zhong, ji, j);
                        if (zhong == QiaoShi.YaoDingYi.AnGang && (j == 0 || j == 3))
                        {
                            p = 0x00;
                        }
                        shi.goFuLuPai[i][j] = Instantiate(goPai, goPai.transform.parent);
                        if (zhong == QiaoShi.YaoDingYi.JiaGang && isMingPai)
                        {
                            shi.goFuLuPai[i][3] = Instantiate(goPai, goPai.transform.parent);
                        }
                        if (isMingPai)
                        {
                            x -= (paiHeight / 2);
                            DrawPai(ref shi.goFuLuPai[i][j], p, new Vector2(x, y - (paiHeight - paiWidth) / 2), 90);
                            if (zhong == QiaoShi.YaoDingYi.JiaGang)
                            {
                                DrawPai(ref shi.goFuLuPai[i][3], p, new Vector2(x, y - (paiHeight - paiWidth) / 2 + paiWidth), 90);
                            }
                            x -= (paiHeight / 2);
                        }
                        else
                        {
                            x -= (paiWidth / 2);
                            DrawPai(ref shi.goFuLuPai[i][j], p, new Vector2(x, y), 0);
                            x -= (paiWidth / 2);
                        }
                    }
                }

                string hele = (shi.FanShuJi >= 13 ? QiaoShi.DeDianYi[13] : QiaoShi.DeDianYi[shi.FanShuJi]);
                if (hele == "")
                {
                    if ((shi.Fu >= 40 && shi.FanShuJi >= 4) || (shi.Fu >= 70 && shi.FanShuJi >= 3))
                    {
                        hele = QiaoShi.DeDianYi[5];
                    }
                }
                string value = (shi.Fu > 0 ? shi.Fu + "符" : "") + (shi.YiMan ? "役満 " : shi.FanShuJi + "飜 ") + hele + " " + shi.HeLeDian + "点";
                y -= paiHeight * 1.1f;
                DrawText(ref goFu, value, new Vector2(0, y), 0, 30);
                int index = 0;
                foreach ((int yi, int fanShu) in shi.YiFan)
                {
                    y -= paiHeight;
                    goYi[index] = Instantiate(goText, goText.transform.parent);
                    string ming = (shi.YiMan ? QiaoShi.YiManMing[yi] : QiaoShi.YiMing[yi]);
                    DrawText(ref goYi[index], ming, new Vector2(0, y), 0, 25, TextAlignmentOptions.Left, 7);
                    goFanShu[index] = Instantiate(goText, goText.transform.parent);
                    DrawText(ref goFanShu[index], fanShu.ToString(), new Vector2(paiWidth * 3.3f, y), 0, 25, TextAlignmentOptions.Right, 2);
                    index++;
                }
            }
        }

        // 【ゲーム】点
        private IEnumerator DianBiaoShi()
        {
            isDianBiaoShiCoroutine = true;

            // 描画
            ClearScreen();
            DrawJuFrame();
            DrawMingQian();
            Chang.DianJiSuan();

            // 記録 役数・役満数
            if (Chang.RongHeFan.Count > 0)
            {
                foreach (int fan in Chang.RongHeFan)
                {
                    QiaoShi shi = Chang.QiaoShis[fan];
                    foreach ((int yi, _) in shi.YiFan)
                    {
                        if (shi.YiMan)
                        {
                            shi.JiLu.yiManShu[yi]++;
                        }
                        else
                        {
                            shi.JiLu.yiShu[yi]++;
                        }
                    }
                }
            }
            else
            {
                if (Chang.HeleFan >= 0)
                {
                    QiaoShi shi = Chang.QiaoShis[Chang.HeleFan];
                    foreach ((int yi, _) in shi.YiFan)
                    {
                        if (shi.YiMan)
                        {
                            shi.JiLu.yiManShu[yi]++;
                        }
                        else
                        {
                            shi.JiLu.yiShu[yi]++;
                        }
                    }
                }
            }

            float x = 0;
            float y = -(paiHeight * 4);
            int max = 0;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                QiaoShi shi = Chang.QiaoShis[jia];
                // 受取
                string shouQuGongTuo = (shi.ShouQuGongTuo > 0 ? "+" : "") + (shi.ShouQuGongTuo == 0 ? "" : shi.ShouQuGongTuo.ToString());
                string shouQu = (shi.ShouQu - shi.ShouQuGongTuo > 0 ? "+" : "") + (shi.ShouQu - shi.ShouQuGongTuo == 0 ? "" : (shi.ShouQu - shi.ShouQuGongTuo).ToString());
                int len = shouQuGongTuo.Length;
                if (len < shouQu.Length)
                {
                    len = shouQu.Length;
                }
                DrawText(ref goShouQuGongTuo[jia], shouQuGongTuo, Cal(0, -(paiHeight * 2.5f), shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 20, TextAlignmentOptions.Right, len - 1);
                DrawText(ref goShouQu[jia], shouQu, Cal(0, -(paiHeight * 3f), shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 20, TextAlignmentOptions.Right, len - 1);

                DrawText(ref goDianBang[jia], (shi.DianBang - shi.ShouQu).ToString(), Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 30);
                if (Math.Abs(shi.ShouQu) > max)
                {
                    max = Math.Abs(shi.ShouQu);
                }
            }
            if (forwardMode > 0)
            {
                keyPress = true;
            }
            for (int shu = 0; shu <= max; shu += 100)
            {
                for (int i = 0; i < Chang.QiaoShis.Count; i++)
                {
                    int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                    QiaoShi shi = Chang.QiaoShis[jia];
                    if (shi.ShouQu > 0)
                    {
                        if (shi.ShouQu - shu >= 0)
                        {
                            DrawText(ref goDianBang[jia], (shi.DianBang - shi.ShouQu + shu).ToString(), Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 30);
                        }
                    }
                    else if (shi.ShouQu < 0)
                    {
                        if (shi.ShouQu + shu <= 0)
                        {
                            DrawText(ref goDianBang[jia], (shi.DianBang - shi.ShouQu - shu).ToString(), Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 30);
                        }
                    }
                }
                if (keyPress)
                {
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                QiaoShi shi = Chang.QiaoShis[jia];
                DrawText(ref goDianBang[jia], (shi.DianBang).ToString(), Cal(x, y, shi.PlayOrder), 90 * GetDrawOrder(shi.PlayOrder), 30);
            }

            // 記録の書込
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                // 記録 対局数
                shi.JiLu.duiJuShu++;
                File.WriteAllText(Application.persistentDataPath + "/" + shi.MingQian + ".json", JsonUtility.ToJson(shi.JiLu));
            }

            yield return Pause(ForwardMode.FAST_FORWARD);

            if (tingPaiLianZhuang == Zhuang.LIAN_ZHUANG)
            {
                Chang.LianZhuang();
            }
            else
            {
                Chang.LunZhuang();
            }

            bool isQinTop = false;
            if (Chang.ChangFeng == 0x32 && Chang.Ju == Chang.QiaoShis.Count)
            {
                QiaoShi qinShi = Chang.QiaoShis[Chang.Qin];
                if (qinShi.ZiJiaYao == QiaoShi.YaoDingYi.ZiMo || qinShi.TaJiaYao == QiaoShi.YaoDingYi.RongHe)
                {
                    // 親の和了
                    int maxDian = 0;
                    for (int i = 0; i < Chang.QiaoShis.Count; i++)
                    {
                        int jia = (Chang.Qin + i) % Chang.QiaoShis.Count;
                        QiaoShi shi = Chang.QiaoShis[jia];
                        if (maxDian < shi.DianBang)
                        {
                            maxDian = shi.DianBang;
                        }
                    }
                    if (maxDian == qinShi.DianBang)
                    {
                        // 親の和了辞め
                        isQinTop = true;
                    }
                }
            }

            // 点表示
            if (Chang.ChangFeng > 0x32 || (GuiZe.xiang && Chang.XiangPanDing()) || isQinTop)
            {
                eventStatus = Event.ZHUANG_ZHONG_LE;
            }
            else
            {
                eventStatus = Event.PEI_PAI;
            }

            isDianBiaoShiCoroutine = false;
        }

        // 【ゲーム】荘終了
        private IEnumerator ZhuangZhong()
        {
            isZhuangZhongLeCoroutine = true;

            // 得点設定
            SettingScore();

            // 描画
            ClearScreen();
            DrawZhuangZhong();

            yield return Pause(ForwardMode.FAST_FORWARD);

            // 記録の書込
            List<(int dian, QiaoShi shi)> shunWei = new();
            for (int i = Chang.QiaJia; i < Chang.QiaJia + Chang.QiaoShis.Count; i++)
            {
                int jia = i % Chang.QiaoShis.Count;
                QiaoShi shi = Chang.QiaoShis[jia];
                shunWei.Add((shi.DianBang, shi));
            }
            shunWei.Sort((x, y) => y.dian.CompareTo(x.dian));
            for (int i = 0; i < shunWei.Count; i++)
            {
                QiaoShi shi = shunWei[i].shi;
                // 半荘数
                shi.JiLu.banZhuangShu++;
                // 集計点
                shi.JiLu.jiJiDian += shi.JiJiDian;
                // 順位
                switch (i)
                {
                    case 0:
                        shi.JiLu.shunWei1++;
                        break;
                    case 1:
                        shi.JiLu.shunWei2++;
                        break;
                    case 2:
                        shi.JiLu.shunWei3++;
                        break;
                    default:
                        shi.JiLu.shunWei4++;
                        break;
                }
                File.WriteAllText(Application.persistentDataPath + "/" + shi.MingQian + ".json", JsonUtility.ToJson(shi.JiLu));
            }

            banZhuangShu++;
            Debug.Log("半荘" + banZhuangShu + "回終了");
            eventStatus = Event.QIAO_SHI_XUAN_ZE;

            isZhuangZhongLeCoroutine = false;
        }

        // 得点設定
        private static void SettingScore()
        {
            int geHe = 0;
            int maxDian = 0;
            int top = 0;
            for (int i = Chang.QiaJia; i < Chang.QiaJia + Chang.QiaoShis.Count; i++)
            {
                int jia = i % Chang.QiaoShis.Count;
                QiaoShi shi = Chang.QiaoShis[jia];

                if (maxDian < shi.DianBang)
                {
                    maxDian = shi.DianBang;
                    top = jia;
                }
                int deDian = shi.DianBang / 1000;
                deDian -= (GuiZe.fanDian / 1000);
                geHe += deDian;
                shi.JiJiDianJiSuan(deDian);
            }
            Chang.QiaoShis[top].JiJiDianJiSuan(Chang.QiaoShis[top].JiJiDian - geHe);
        }

        // 【描画】荘終了
        private void DrawZhuangZhong()
        {
            float y = paiHeight * 4;
            int maxMingQian = 0;
            int maxDianBang = 0;
            int maxDeDian = 0;
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (maxMingQian < shi.MingQian.Length)
                {
                    maxMingQian = shi.MingQian.Length;
                }
                if (maxDianBang < shi.DianBang.ToString().Length)
                {
                    maxDianBang = shi.DianBang.ToString().Length;
                }
                int len = shi.JiJiDian.ToString().Length;
                if (shi.JiJiDian > 0)
                {
                    len++;
                }
                if (maxDeDian < len)
                {
                    maxDeDian = len;
                }
            }

            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                string dianBang = shi.DianBang.ToString();
                for (int j = dianBang.Length; j < maxDianBang; j++)
                {
                    dianBang = " " + dianBang;
                }
                string deDian = (shi.JiJiDian > 0 ? "+" : "") + shi.JiJiDian.ToString();
                for (int j = deDian.Length; j < maxDeDian; j++)
                {
                    deDian = " " + deDian;
                }

                DrawText(ref goMingQian[i], shi.MingQian, new Vector2(-(paiWidth * 5f), y), 0, 30, TextAlignmentOptions.Left, quiaoShiButtonMaxLen);
                DrawText(ref goDianBang[i], dianBang, new Vector2(paiWidth * 3f, y), 0, 30, TextAlignmentOptions.Right, 6);
                DrawText(ref goShouQu[i], deDian, new Vector2(paiWidth * 7f, y), 0, 25, TextAlignmentOptions.Right, 4);
                y -= paiHeight * 2;
            }
        }

        // 【描画】テキスト
        private void DrawText(ref TextMeshProUGUI obj, string value, Vector2 xy, int quaternion, int fontSize)
        {
            DrawText(ref obj, value, xy, quaternion, fontSize, TextAlignmentOptions.Center, -1);
        }
        private void DrawText(ref TextMeshProUGUI obj, string value, Vector2 xy, int quaternion, int fontSize, TextAlignmentOptions align, int len)
        {
            if (obj == null)
            {
                obj = Instantiate(goText, goText.transform.parent);
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
        void DrawFrame(ref Image obj, string value, Vector2 xy, float quaternion, int fontSize, Color backgroundColor, Color foreColor)
        {
            DrawFrame(ref obj, value, xy, quaternion, fontSize, backgroundColor, foreColor, -1);
        }
        void DrawFrame(ref Image obj, string value, Vector2 xy, float quaternion, int fontSize, Color backgroundColor, Color foreColor, int len)
        {
            if (obj == null)
            {
                obj = Instantiate(goFrame, goFrame.transform.parent);
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
        private void DrawButton(ref Button obj, string value, Vector2 xy)
        {
            DrawButton(ref obj, value, xy, -1);
        }
        private void DrawButton(ref Button obj, string value, Vector2 xy, int len)
        {
            if (obj == null)
            {
                obj = Instantiate(goButton, goButton.transform.parent);
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
                obj = Instantiate(goPai, goPai.transform.parent);
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
            int plusMinus = (drawOrder == 0 || drawOrder == 3 ? 1 : -1);
            float xx = (drawOrder % 2 == 0 ? x : y) * plusMinus;
            float yy = (drawOrder % 2 == 0 ? y : -x) * plusMinus;

            return new Vector2(xx, yy);
        }

        private int GetDrawOrder(int order)
        {
            if (Chang.QiaoShis.Count == 3 && order == 2)
            {
                return 3;
            }
            if (Chang.QiaoShis.Count == 2 && order == 1)
            {
                return 2;
            }
            return order;
        }

        // 画面クリア
        private void ClearScreen()
        {
            ClearGameObject(ref goBackDuiJuZhongLe);
            ClearGameObject(ref goTitle);
            ClearGameObject(ref goStart);
            ClearGameObject(ref goJuFrame);
            ClearGameObject(ref goJu);
            ClearGameObject(ref goMingWu);
            ClearGameObject(ref goDianCha);
            ClearGameObject(ref goQiJia);
            ClearGameObject(ref goBenChang);
            ClearGameObject(ref goBenChangText);
            ClearGameObject(ref goGongTou);
            ClearGameObject(ref goGongTouText);
            ClearGameObject(ref goSai1);
            ClearGameObject(ref goSai2);

            ClearGameObject(ref goQiaoShi);

            ClearGameObject(ref goLeft);
            ClearGameObject(ref goRight);
            ClearGameObject(ref goSelect);

            ClearGameObject(ref Pai.goXuanShangPai);
            ClearGameObject(ref Pai.goLiXuanShangPai);

            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi == null)
                {
                    continue;
                }
                ClearGameObject(ref shi.goShouPai);
                for (int j = 0; j < shi.goFuLuPai.Length; j++)
                {
                    ClearGameObject(ref shi.goFuLuPai[j]);
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
            ClearGameObject(ref goSheng);
        }

        // テキストクリア
        private void ClearGameObject(ref TextMeshProUGUI go)
        {
            if (go == null)
            {
                return;
            }
            Destroy(go.gameObject);
            go = null;
        }

        // ボタンクリア
        private void ClearGameObject(ref Button go)
        {
            if (go == null)
            {
                return;
            }
            Destroy(go.gameObject);
            go = null;
        }

        // イメージクリア
        private void ClearGameObject(ref Image go)
        {
            if (go == null)
            {
                return;
            }
            Destroy(go.gameObject);
            go = null;
        }

        // テキストクリア
        private void ClearGameObject(ref List<TextMeshProUGUI> go)
        {
            for (int i = 0; i < go.Count; i++)
            {
                TextMeshProUGUI t = go[i];
                ClearGameObject(ref t);
            }
        }
        private void ClearGameObject(ref TextMeshProUGUI[] go)
        {
            for (int i = 0; i < go.Length; i++)
            {
                ClearGameObject(ref go[i]);
            }
        }

        // ボタンクリア
        private void ClearGameObject(ref List<Button> go)
        {
            for (int i = 0; i < go.Count; i++)
            {
                Button b = go[i];
                ClearGameObject(ref b);
            }
        }
        private void ClearGameObject(ref Button[] go)
        {
            for (int i = 0; i < go.Length; i++)
            {
                ClearGameObject(ref go[i]);
            }
        }

        // イメージクリア
        private void ClearGameObject(ref Image[] go)
        {
            for (int i = 0; i < go.Length; i++)
            {
                ClearGameObject(ref go[i]);
            }
        }
    }
}

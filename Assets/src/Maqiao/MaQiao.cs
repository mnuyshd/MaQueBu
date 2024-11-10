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

        // 待ち時間(デフォルト)
        private const float WAIT_TIME = 0.3f;
        // プレイヤー牌倍率
        private const float PLAYER_PAI_SCALE = 1.4f;
        private const float PLAYER_PAI_SCALE_LANDSCAPE = 1.2f;
        // プレイヤー名
        private const string PLAYER_NAME = "プレイヤー";
        // 設定ファイル名
        private const string SHE_DING_FILE_NAME = "SheDing";

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
        // 雀士
        private static QiaoShi[] qiaoShi;

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
        private bool isChangJueCoroutine;
        private bool isQinJueCoroutine;
        private bool isPeiPaiCoroutine;
        private bool isDuiJuCoroutine;
        private bool isDuiJuZhongLeCoroutine;
        private bool isYiBiaoShiCoroutine;
        private bool isDianBiaoShiCoroutine;
        private bool isZhuangZhongLeCoroutine;

        private bool isBackDuiJuZhongLe;
        private bool isDianCha;

        // Game Object
        private Button goScreen;
        private TextMeshProUGUI goText;
        private Image goFrame;
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
            { "機械雀士", true },
            { HikitaMamoru.MING_QIAN, true },
            { UchidaKou.MING_QIAN, true },
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
                HikitaMamoru.MING_QIAN => new HikitaMamoru(),
                SomeyaMei.MING_QIAN => new SomeyaMei(),
                UchidaKou.MING_QIAN => new UchidaKou(),
                KouzuNaruto.MING_QIAN => new KouzuNaruto(),
                KouzuTorako.MING_QIAN => new KouzuTorako(),
                YakudaJunji.MING_QIAN => new KouzuTorako(),
                MenzenJunko.MING_QIAN => new MenzenJunko(),
                _ => new QiaoJiXie(mingQian),
            };
        }

        void Start()
        {
            Application.targetFrameRate = 60;

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
            qiaoShi = new QiaoShi[4];

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
            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    goPais[(i * 0x10) + j] = GameObject.Find("0x" + i + j);
                }
            }
            for (int i = 1; i <= 7; i++)
            {
                goPais[0x30 + i] = GameObject.Find("0x3" + i);
            }
            goPais[0x45] = GameObject.Find("0x45");
            goPais[0x55] = GameObject.Find("0x55");
            goPais[0x65] = GameObject.Find("0x65");
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
                for (int i = 0; i < Chang.mianZi; i++)
                {
                    QiaoShi shi = Chang.qiaoShi[i];
                    if (shi == null)
                    {
                        break;
                    }
                    if (!shi.player)
                    {
                        DrawShouPai(i, QiaoShi.Yao.WU, 0);
                    }
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
            float offset = paiHeight * 2;
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
            DrawButton(ref goScoreReset, "リセット", new Vector2(0, y - paiHeight));

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

            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                if (shi != null)
                {
                    shi.jiLu = new JiLu();
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
            goYiMing = new TextMeshProUGUI[QiaoShi.YI_MING.Length];
            goYiShu = new TextMeshProUGUI[QiaoShi.YI_MING.Length];
            for (int i = 0; i < goYiShu.Length; i++)
            {
                if (QiaoShi.YI_MING[i] == "ドラ")
                {
                    continue;
                }
                y -= paiHeight;
                DrawData(ref goYiMing[i], ref goYiShu[i], QiaoShi.YI_MING[i], y);
            }
            y -= paiHeight;
            goYiManMing = new TextMeshProUGUI[QiaoShi.YI_MAN_MING.Length];
            goYiManShu = new TextMeshProUGUI[QiaoShi.YI_MAN_MING.Length];
            for (int i = 0; i < goYiManShu.Length; i++)
            {
                y -= paiHeight;
                DrawData(ref goYiManMing[i], ref goYiManShu[i], QiaoShi.YI_MAN_MING[i], y);
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
            DrawText(ref goMing, ming, new Vector2(-paiWidth * 5f, y), 0, 25, TextAlignmentOptions.Left);

            goShu = Instantiate(goText, goDataContent.transform);
            DrawText(ref goShu, "", new Vector2(paiWidth * 5f, y), 0, 25, TextAlignmentOptions.Right);
        }

        // 得点パネル 雀士名クリック
        private void OnClickScoreQiaoShi(string mingQian)
        {
            QiaoShi shi = GetQiaoShi(mingQian);
            JiLu jiLu = new();
            string filePath = Application.persistentDataPath + "/" + shi.mingQian + ".json";
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
                if (QiaoShi.YI_MING[i] == "ドラ")
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
            float x = -(paiWidth * 8.5f);
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
                DrawText(ref t, g, new Vector2(x, y), 0, 20, TextAlignmentOptions.Left);
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
                    if (!isChangJueCoroutine)
                    {
                        StartCoroutine(ChangJue());
                    }
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
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                if (shi.player)
                {
                    continue;
                }
                QiaoShi.Yao yao = shi.ziJiaYao;
                int xuanZe = shi.ziJiaXuanZe;
                if (shi.taJiaYao != QiaoShi.Yao.WU)
                {
                    yao = shi.taJiaYao;
                    xuanZe = shi.taJiaXuanZe;
                }
                if (yao == QiaoShi.Yao.ZI_MO || yao == QiaoShi.Yao.RONG_HE)
                {
                    yao = QiaoShi.Yao.HE_LE;
                    isHeLe = true;
                }
                DrawShouPai(i, yao, xuanZe, shi.player, shi.follow);
            }
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                if (!shi.player)
                {
                    continue;
                }
                if (isHeLe)
                {
                    DrawShouPai(i, QiaoShi.Yao.WU, -2);
                    break;
                }
                if (goYao[0] != null)
                {
                    DrawZiJiaYao(shi, 0, shi.shouPaiWei - 1, true, false);
                    if (goYao[0] == null)
                    {
                        DrawTaJiaYao(i, shi, 0, shi.follow);
                        if (goYao[0] != null)
                        {
                            DrawShouPai(i, QiaoShi.Yao.WU, -2);
                            break;
                        }
                    }
                }
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                {
                    int x = shi.follow ? shi.ziJiaXuanZe : shi.shouPaiWei - 1;
                    DrawSelectDaPai(i, shi, x);
                    DrawDaiPai(i, x);
                }
                else
                {
                    QiaoShi.Yao yao = shi.ziJiaYao;
                    int xuanZe = shi.ziJiaXuanZe;
                    if (shi.taJiaYao != QiaoShi.Yao.WU)
                    {
                        yao = shi.taJiaYao;
                        xuanZe = shi.taJiaXuanZe;
                    }
                    if (yao == QiaoShi.Yao.ZI_MO || yao == QiaoShi.Yao.RONG_HE)
                    {
                        yao = QiaoShi.Yao.HE_LE;
                    }
                    DrawShouPai(i, yao, xuanZe, shi.player, shi.follow);
                }
            }

            for (int i = 0; i < Chang.mianZi; i++)
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
                Chang.mianZi = selectedCount + 1;
                Chang.qiaoShi = new QiaoShi[Chang.mianZi];
                keyPress = true;
            }
        }

        // フォロー無しクリック
        private void OnClickScreenFollowNone()
        {
            qiaoShi[0] = new QiaoJiXie(PLAYER_NAME)
            {
                follow = false,
                player = true
            };
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

            int index = 1;
            foreach (KeyValuePair<string, bool> kvp in qiaoShiMingQian)
            {
                if (kvp.Value)
                {
                    qiaoShi[index++] = GetQiaoShi(kvp.Key);
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
                    value = "相手雀士を選択";
                    break;
                case Event.FOLLOW_QIAO_SHI_XUAN_ZE:
                    value = "フォロー雀士を選択";
                    break;
                default:
                    value = "";
                    break;
            }
            DrawText(ref goJu, value, new Vector2(x, y), 0, 18);
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
            string value = Pai.FENG_PAI_MING[Chang.changFeng - 0x31] + (Chang.ju + 1) + "局";
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
            string valueBenChang = "x" + Chang.benChang.ToString();
            DrawText(ref goBenChangText, valueBenChang, new Vector2(x + paiWidth * 1.1f, y + paiHeight * 0.05f), 0, 14);

            y -= paiHeight * 0.4f;
            ClearGameObject(ref goGongTou);
            goGongTou = Instantiate(goDianBang1000, goDianBang1000.transform.parent);
            RectTransform rt1000 = goGongTou.GetComponent<RectTransform>();
            rt1000.sizeDelta = new Vector2(rt1000.sizeDelta.x * 0.5f, rt1000.sizeDelta.y);
            rt1000.anchoredPosition = new Vector2(x, y);
            ClearGameObject(ref goGongTouText);
            goGongTouText = Instantiate(goText, goJuFrame.transform.parent);
            string valueGongTou = "x" + (Chang.gongTuo / 1000).ToString();
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
                for (int i = 0; i < Chang.mianZi; i++)
                {
                    int jia = (Chang.qin + i) % Chang.mianZi;
                    QiaoShi shi = Chang.qiaoShi[jia];
                    if (shi.player)
                    {
                        dianPlayer = shi.dianBang;
                        break;
                    }
                }
            }

            float x = 0f;
            float y = -(paiWidth * 2.5f);
            for (int i = 0; i < Chang.mianZi; i++)
            {
                int jia = (Chang.qin + i) % Chang.mianZi;
                QiaoShi shi = Chang.qiaoShi[jia];
                Color background = shi.feng == 0x31 ? new Color(1f, 0.4f, 0.4f) : Color.black;
                ClearGameObject(ref goFeng[i]);
                goFeng[i] = Instantiate(goFrame, goJuFrame.transform.parent);
                DrawFrame(ref goFeng[i], Pai.FENG_PAI_MING[shi.feng - 0x31], Cal(x - paiWidth * 2f, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 16, background, Color.white);
                if (eventStatus == Event.DIAN_BIAO_SHI)
                {
                    continue;
                }
                ClearGameObject(ref goDianBang[i]);
                goDianBang[i] = Instantiate(goText, goJuFrame.transform.parent);
                if (isDianCha && !shi.player)
                {
                    int dianCha = dianPlayer - shi.dianBang;
                    DrawText(ref goDianBang[i], dianCha.ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 16);
                    goDianBang[i].color = (dianCha >= 0 ? Color.blue : Color.red);
                }
                else
                {
                    DrawText(ref goDianBang[i], shi.dianBang.ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 16);
                    goDianBang[i].color = Color.black;
                }

                if (shi.liZhi)
                {
                    ClearGameObject(ref goLizhiBang[jia]);
                    goLizhiBang[jia] = Instantiate(goDianBang1000, goJuFrame.transform.parent);
                    goLizhiBang[jia].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    RectTransform rt = goLizhiBang[jia].GetComponent<RectTransform>();
                    rt.anchoredPosition = Cal(x, y + paiHeight * 0.4f, shi.playOrder);
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
            foreach (string key in keys)
            {
                qiaoShiMingQian[key] = false;
            }
            int cnt = 0;
            while (cnt < Chang.mianZi - 1)
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
            qiaoShi[0] = GetQiaoShi(mingQian);
            qiaoShi[0].mingQian = PLAYER_NAME;
            qiaoShi[0].follow = true;
            qiaoShi[0].player = true;
            keyPress = true;
        }

        // 【ゲーム】場決
        private IEnumerator ChangJue()
        {
            isChangJueCoroutine = true;

            ClearScreen();

            if (qiaoShi[0] == null)
            {
                OnClickScreenFollowNone();
            }
            int[] fengPai = new int[Pai.FENG_PAI.Length - (4 - Chang.mianZi)];
            for (int i = 0; i < Pai.FENG_PAI.Length - (4 - Chang.mianZi); i++)
            {
                fengPai[i] = Pai.FENG_PAI[i];
            }
            Chang.Shuffle(fengPai, 20);

            Button[] goButton = new Button[Chang.mianZi];

            int jia = 0;
            for (int i = 0x31; i <= 0x34 - (4 - Chang.mianZi); i++)
            {
                for (int j = 0; j < fengPai.Length; j++)
                {
                    if (fengPai[j] == i)
                    {
                        Chang.qiaoShi[jia] = qiaoShi[j];
                        float y = -(paiWidth * 2.5f + paiHeight * 2f);
                        DrawPai(ref goButton[jia], i, Cal(0, y, jia), 90 * GetDrawOrder(jia));
                        jia++;
                    }
                }
            }

            int order = 0;
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                shi.playOrder = i;
                if (shi.player)
                {
                    order = i;
                }
            }
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                shi.playOrder -= order;
                if (shi.playOrder < 0)
                {
                    shi.playOrder += Chang.mianZi;
                }
            }

            DrawMingQian();

            // 記録の読込
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                string filePath = Application.persistentDataPath + "/" + shi.mingQian + ".json";
                if (File.Exists(filePath))
                {
                    shi.jiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(filePath));
                }
                else
                {
                    shi.jiLu = new JiLu();
                }
            }

            yield return Pause(ForwardMode.NORMAL);

            ClearGameObject(ref goButton);

            eventStatus = Event.QIN_JUE;
            isChangJueCoroutine = false;
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
            Chang.qin = (sai1 + sai2 - 1) % Chang.mianZi;
            // 起家
            Chang.qiaJia = Chang.qin;
            Chang.changFeng = 0x31;
            DrawQiJia();

            yield return Pause(ForwardMode.NORMAL);

            eventStatus = Event.ZHUANG_CHU_QI_HUA;
            isQinJueCoroutine = false;
        }

        // 【描画】名前(4人分)
        private void DrawMingQian()
        {
            for (int i = 0; i < Chang.mianZi; i++)
            {
                int jia = (Chang.qin + i) % Chang.mianZi;
                DrawMingQian(jia);
            }
        }

        // 【描画】名前
        private void DrawMingQian(int jia)
        {
            QiaoShi shi = Chang.qiaoShi[jia];
            float x;
            float y;
            switch (eventStatus)
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
            go = Instantiate(goSais[mu - 1].GetComponent<Image>(), goSais[mu - 1].GetComponent<Image>().transform.parent);
            RectTransform rt = go.GetComponent<RectTransform>();
            QiaoShi shi = Chang.qiaoShi[jia];
            go.transform.Rotate(0, 0, 90 * shi.playOrder);
            float x = 0;
            float y = -(paiWidth * 1.2f);
            rt.anchoredPosition = Cal(x + margin, y, shi.playOrder);
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
            for (int i = 0; i < Chang.mianZi; i++)
            {
                int jia = (Chang.qin + i) % Chang.mianZi;
                if (Chang.qiaJia == jia)
                {
                    ClearGameObject(ref goQiJia);
                    QiaoShi shi = Chang.qiaoShi[jia];
                    if (shi.player && orientation != ScreenOrientation.Portrait)
                    {
                        x -= paiWidth * 1.2f;
                        y += paiHeight * 0.6f;
                    }
                    if (shi.playOrder == 3 && orientation != ScreenOrientation.Portrait)
                    {
                        x -= paiHeight * 0.6f;
                    }
                    goQiJia = Instantiate(goQiJias[Chang.changFeng - 0x31].GetComponent<Image>(), goQiJias[Chang.changFeng - 0x31].GetComponent<Image>().transform.parent);
                    goQiJia.transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    RectTransform rt = goQiJia.GetComponent<RectTransform>();
                    rt.anchoredPosition = Cal(eventStatus == Event.QIN_JUE ? 0 : x, y, shi.playOrder);
                }
            }
        }

        // 【ゲーム】荘初期化
        private void ZhuangChuQiHua()
        {
            Chang.ZhuangChuQiHua();
            for (int i = 0; i < Chang.mianZi; i++)
            {
                Chang.qiaoShi[i].ZhuangChuQiHua();
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
            Chang.sai1 = r.Next(0, 6) + 1;
            Chang.sai2 = r.Next(0, 6) + 1;

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
                for (int j = 0; j < Chang.mianZi; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Chang.qiaoShi[(Chang.qin + j) % Chang.mianZi].ZiMo(Pai.ShanPaiZiMo());
                    }
                    DrawShouPai((Chang.qin + j) % Chang.mianZi, QiaoShi.Yao.WU, -1);
                    yield return new WaitForSeconds(waitTime / 3);
                }
            }
            for (int i = 0; i < Chang.mianZi; i++)
            {
                // 理牌
                Chang.qiaoShi[(Chang.qin + i) % Chang.mianZi].LiPai();
                DrawShouPai((Chang.qin + i) % Chang.mianZi, QiaoShi.Yao.WU, -1);
            }
            yield return new WaitForSeconds(waitTime / 3);
            for (int i = 0; i < Chang.mianZi; i++)
            {
                Chang.qiaoShi[(Chang.qin + i) % Chang.mianZi].ZiMo(Pai.ShanPaiZiMo());
                DrawShouPai((Chang.qin + i) % Chang.mianZi, QiaoShi.Yao.WU, -1);
            }
            yield return new WaitForSeconds(waitTime / 3);
            for (int i = 0; i < Chang.mianZi; i++)
            {
                Chang.qiaoShi[(Chang.qin + i) % Chang.mianZi].LiPai();
                DrawShouPai((Chang.qin + i) % Chang.mianZi, QiaoShi.Yao.WU, -1);
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
            for (int i = 0; i < Chang.mianZi; i++)
            {
                int jia = (Chang.qin + i) % Chang.mianZi;
                QiaoShi shi = Chang.qiaoShi[jia];
                shi.JuChuQiHua(Pai.FENG_PAI[i]);

                if (shi.player)
                {
                    w = shi.feng - 0x31;
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
                int p = Pai.xuanShangPai[i];
                Pai.goXuanShangPai[i] = Instantiate(goPai, goPai.transform.parent);
                Pai.goXuanShangPai[i].transform.SetSiblingIndex(1);
                DrawPai(ref Pai.goXuanShangPai[i], i < Pai.xuanShangPaiWei ? p : 0x00, new Vector2(x, y), 0);
                x += paiWidth;
            }

            bool isLiXuanShangPai = false;
            if (Chang.heleFan >= 0 && Chang.qiaoShi[Chang.heleFan].liZhi)
            {
                isLiXuanShangPai = true;
            }
            for (int i = 0; i < Chang.rongHeShu; i++)
            {
                QiaoShi shi = Chang.qiaoShi[Chang.rongHeFan[i]];
                if (shi.liZhi)
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
                    int p = Pai.liXuanShangPai[i];
                    Pai.goLiXuanShangPai[i] = Instantiate(goPai, goPai.transform.parent);
                    Pai.goLiXuanShangPai[i].transform.SetSiblingIndex(0);
                    DrawPai(ref Pai.goLiXuanShangPai[i], i < Pai.xuanShangPaiWei ? p : 0x00, new Vector2(x, y), 0);
                    x += paiWidth;
                }
            }
        }

        // 【ゲーム】対局
        private IEnumerator DuiJu()
        {
            isDuiJuCoroutine = true;

            QiaoShi ziJiaShi = Chang.qiaoShi[Chang.ziMoFan];
            if ((Chang.ziJiaYao == QiaoShi.Yao.WU || Chang.ziJiaYao == QiaoShi.Yao.LI_ZHI) && Chang.taJiaYao == QiaoShi.Yao.WU)
            {
                // 山牌自摸
                ziJiaShi.ZiMo(Pai.ShanPaiZiMo());
            }
            if (Chang.ziJiaYao == QiaoShi.Yao.AN_GANG || Chang.ziJiaYao == QiaoShi.Yao.JIA_GANG || Chang.taJiaYao == QiaoShi.Yao.DA_MING_GANG)
            {
                // 嶺上牌自摸
                ziJiaShi.ZiMo(Pai.LingShangPaiZiMo());
            }
            // 描画
            DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -2, true, false);
            DrawJuFrame();
            yield return new WaitForSeconds(waitTime);
            if (Chang.taJiaYao != QiaoShi.Yao.WU)
            {
                yield return new WaitForSeconds(waitTime);
            }
            ClearGameObject(ref goSheng);

            // 思考自家判定
            ziJiaShi.taJiaYao = QiaoShi.Yao.WU;
            ziJiaShi.taJiaXuanZe = 0;
            ziJiaShi.SiKaoZiJiaPanDing();
            DrawDaiPai(Chang.ziMoFan, -1);
            // 思考自家
            if (ziJiaShi.player)
            {
                ziJiaShi.SiKaoZiJia();
                DrawZiJiaYao(ziJiaShi, 0, ziJiaShi.shouPaiWei - 1, true, false);
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                {
                    int x = ziJiaShi.follow ? ziJiaShi.ziJiaXuanZe : ziJiaShi.shouPaiWei - 1;
                    DrawSelectDaPai(Chang.ziMoFan, ziJiaShi, x);
                    DrawDaiPai(Chang.ziMoFan, x);
                }
                else if (ziJiaShi.ziJiaYao == QiaoShi.Yao.JIU_ZHONG_JIU_PAI || ziJiaShi.ziJiaYao == QiaoShi.Yao.ZI_MO || ziJiaShi.ziJiaYao == QiaoShi.Yao.LI_ZHI || ziJiaShi.ziJiaYao == QiaoShi.Yao.AN_GANG || ziJiaShi.ziJiaYao == QiaoShi.Yao.JIA_GANG)
                {
                    DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -1, true, false);
                }
                else
                {
                    DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -1, true, ziJiaShi.follow);
                }
                if (!(sheDing.liZhiAuto && ziJiaShi.liZhi) || ziJiaShi.heLe || ziJiaShi.anGangKeNengShu > 0 || ziJiaShi.jiaGangKeNengShu > 0)
                {
                    if (ziJiaShi.follow && ziJiaShi.ziJiaYao == QiaoShi.Yao.WU)
                    {
                        DrawDaiPai(Chang.ziMoFan, ziJiaShi.ziJiaXuanZe);
                    }
                    yield return Pause(ForwardMode.NORMAL);
                }
                Chang.ziJiaYao = ziJiaShi.ziJiaYao;
                Chang.ziJiaXuanZe = ziJiaShi.ziJiaXuanZe;
                ClearGameObject(ref goYao);
                ClearGameObject(ref goLeft);
                ClearGameObject(ref goRight);
                ClearGameObject(ref goSelect);
            }
            else
            {
                ziJiaShi.SiKaoZiJia();
                Chang.ziJiaYao = ziJiaShi.ziJiaYao;
                Chang.ziJiaXuanZe = ziJiaShi.ziJiaXuanZe;
            }
            // 錯和自家判定
            if (ziJiaShi.CuHeZiJiaPanDing())
            {
                // 錯和
                Chang.CuHe(Chang.qiaoShi, Chang.ziMoFan);
                // 描画
                DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -1);
                DrawShePai(Chang.ziMoFan);
                DrawCuHe(Chang.ziMoFan);
                tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                yield return Pause(ForwardMode.FAST_FORWARD);
                eventStatus = Event.DIAN_BIAO_SHI;
                yield break;
            }

            switch (Chang.ziJiaYao)
            {
                case QiaoShi.Yao.ZI_MO:
                    // 自摸
                    ziJiaShi.HeLe();
                    Chang.heleFan = Chang.ziMoFan;
                    // 描画
                    DrawZiMo(Chang.ziMoFan);
                    DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.HE_LE, -1);
                    DrawXuanShangPai();
                    tingPaiLianZhuang = (Chang.ziMoFan == Chang.qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG);
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.YI_BIAO_SHI;
                    yield break;

                case QiaoShi.Yao.JIU_ZHONG_JIU_PAI:
                    // 九種九牌
                    // 描画
                    Chang.jiuZhongJiuPaiChuLi();
                    DrawJiuZhongJiuPai(Chang.ziMoFan);
                    tingPaiLianZhuang = GuiZe.jiuZhongJiuPaiLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    DrawShouPai(Chang.ziMoFan, Chang.ziJiaYao, 0);
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.DIAN_BIAO_SHI;
                    yield break;

                case QiaoShi.Yao.LI_ZHI:
                    // 立直
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.LingShanChuLi();
                    // 立直
                    ziJiaShi.LiZi();
                    break;

                case QiaoShi.Yao.AN_GANG:
                    // 暗槓
                    ziJiaShi.AnGang(Chang.ziJiaXuanZe);
                    // 描画
                    // 消
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        Chang.qiaoShi[i].Xiao();
                    }
                    // 嶺上牌処理
                    Pai.LingShangPaiChuLi(Chang.ziMoFan);
                    // 描画
                    DrawAnGang(Chang.ziMoFan);
                    DrawJuFrame();
                    DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -1);
                    DrawXuanShangPai();
                    // 四開槓判定
                    if (Pai.SiKaiGangPanDing())
                    {
                        // 描画
                        DrawSiKaiGangPanDing(Chang.ziMoFan);
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

                case QiaoShi.Yao.JIA_GANG:
                    // 加槓
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.LingShanChuLi();
                    // 加槓
                    ziJiaShi.JiaGang(Chang.ziJiaXuanZe);
                    Pai.QiangGang();
                    // 描画
                    DrawJuFrame();
                    DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -1);
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

            if (Chang.ziJiaYao == QiaoShi.Yao.WU || Chang.ziJiaYao == QiaoShi.Yao.LI_ZHI)
            {
                // 打牌
                DrawDaiPai(Chang.ziMoFan, Chang.ziJiaXuanZe);
                ziJiaShi.DaPai();
                // 描画
                ziJiaShi.shouPaiWei++;
                DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -2, true, false);
                ziJiaShi.shouPaiWei--;
                Chang.qiaoShi[Chang.ziMoFan].ShePaiChuLi(Chang.ziJiaYao);
                DrawShePai(Chang.ziMoFan);
                if (Chang.ziJiaYao == QiaoShi.Yao.LI_ZHI)
                {
                    DrawLiZi(Chang.ziMoFan);
                }
                yield return new WaitForSeconds(waitTime);
                ziJiaShi.LiPai();
                DrawShouPai(Chang.ziMoFan, QiaoShi.Yao.WU, -2);
                // 四風子連打処理
                Chang.SiFengZiLianDaChuLi(Chang.shePai);
                // 四風子連打判定
                if (Chang.SiFengZiLianDaPanDing())
                {
                    // 描画
                    DrawSiFengZiLianDa(Chang.ziMoFan);
                    tingPaiLianZhuang = GuiZe.siFengZiLianDaLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.DIAN_BIAO_SHI;
                    yield break;
                }
            }

            switch (Chang.taJiaYao)
            {
                case QiaoShi.Yao.DA_MING_GANG:
                    // 大明槓成立
                    // 嶺上牌処理
                    Pai.LingShangPaiChuLi(Chang.ziMoFan);
                    // 描画
                    DrawXuanShangPai();
                    // 四開槓判定
                    if (Pai.SiKaiGangPanDing())
                    {
                        // 描画
                        DrawSiKaiGangPanDing(Chang.ziMoFan);
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
            Chang.taJiaYao = QiaoShi.Yao.WU;
            Chang.mingFan = Chang.ziMoFan;
            for (int i = Chang.ziMoFan + 1; i < Chang.ziMoFan + Chang.mianZi; i++)
            {
                int jia = i % Chang.mianZi;
                QiaoShi taJiaShi = Chang.qiaoShi[jia];
                // 思考他家判定
                taJiaShi.ziJiaYao = QiaoShi.Yao.WU;
                taJiaShi.ziJiaXuanZe = 0;
                taJiaShi.SiKaoTaJiaPanDing(i - Chang.ziMoFan);
                // 思考他家
                if (taJiaShi.player)
                {
                    continue;
                }
                taJiaShi.SiKaoTaJia(jia);
                // 錯和他家判定
                if (taJiaShi.CuHeTaJiaPanDing())
                {
                    Chang.CuHe(Chang.qiaoShi, jia);
                    // 描画
                    DrawCuHe(jia);
                    tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    eventStatus = Event.DIAN_BIAO_SHI;
                    yield break;
                }
                if ((taJiaShi.taJiaYao == QiaoShi.Yao.RONG_HE && Chang.taJiaYao < QiaoShi.Yao.RONG_HE)
                    || (taJiaShi.taJiaYao == QiaoShi.Yao.DA_MING_GANG && Chang.taJiaYao < QiaoShi.Yao.DA_MING_GANG)
                    || (taJiaShi.taJiaYao == QiaoShi.Yao.BING && Chang.taJiaYao < QiaoShi.Yao.BING)
                    || (taJiaShi.taJiaYao == QiaoShi.Yao.CHI && Chang.taJiaYao < QiaoShi.Yao.CHI))
                {
                    // 栄和、大明槓、石並、吃
                    Chang.taJiaYao = taJiaShi.taJiaYao;
                    Chang.taJiaXuanZe = taJiaShi.taJiaXuanZe;
                    Chang.mingFan = jia;
                }
                if (GuiZe.wRongHe && taJiaShi.taJiaYao == QiaoShi.Yao.RONG_HE)
                {
                    Chang.rongHeFan[Chang.rongHeShu] = jia;
                    Chang.rongHeShu++;
                }
            }
            // 思考他家プレイヤー分
            for (int i = Chang.ziMoFan + 1; i < Chang.ziMoFan + Chang.mianZi; i++)
            {
                int jia = i % Chang.mianZi;
                QiaoShi taJiaShi = Chang.qiaoShi[jia];
                if (!taJiaShi.player)
                {
                    continue;
                }
                if (sheDing.mingWu && (!taJiaShi.heLe))
                {
                    continue;
                }
                if (taJiaShi.heLe || taJiaShi.chiKeNengShu > 0 || taJiaShi.bingKeNengShu > 0 || taJiaShi.daMingGangKeNengShu > 0)
                {
                    if ((taJiaShi.heLe && Chang.taJiaYao < QiaoShi.Yao.RONG_HE)
                        || (GuiZe.wRongHe && taJiaShi.heLe)
                        || (taJiaShi.daMingGangKeNengShu > 0 && Chang.taJiaYao < QiaoShi.Yao.DA_MING_GANG)
                        || (taJiaShi.bingKeNengShu > 0 && Chang.taJiaYao < QiaoShi.Yao.BING)
                        || (taJiaShi.chiKeNengShu > 0 && Chang.taJiaYao < QiaoShi.Yao.CHI))
                    {
                        DrawShePai(Chang.ziMoFan, true);

                        taJiaShi.SiKaoTaJia(jia);
                        DrawTaJiaYao(jia, taJiaShi, 0, true);
                        DrawShouPai(jia, QiaoShi.Yao.WU, -2);
                        yield return Pause(ForwardMode.NORMAL);
                        ClearGameObject(ref goYao);
                        if (Chang.rongHeShu == 0)
                        {
                            Chang.taJiaYao = taJiaShi.taJiaYao;
                            Chang.taJiaXuanZe = taJiaShi.taJiaXuanZe;
                            Chang.mingFan = jia;
                        }
                        if (GuiZe.wRongHe && taJiaShi.taJiaYao == QiaoShi.Yao.RONG_HE)
                        {
                            Chang.rongHeFan[Chang.rongHeShu] = jia;
                            Chang.rongHeShu++;
                        }
                        DrawShePai(Chang.ziMoFan);
                    }
                }
            }

            if (Chang.taJiaYao == QiaoShi.Yao.RONG_HE)
            {
                // 栄和
                if (Chang.rongHeShu == 0)
                {
                    Chang.rongHeShu = 1;
                    Chang.rongHeFan[0] = Chang.mingFan;
                }
                QiaoShi.Sort(Chang.rongHeFan);
                Chang.mingFan = Chang.rongHeFan[0];

                // 捨牌処理
                Chang.qiaoShi[Chang.ziMoFan].ShePaiChuLi(QiaoShi.Yao.RONG_HE);
                for (int i = 0; i < Chang.rongHeShu; i++)
                {
                    // 描画
                    DrawRongHe(Chang.rongHeFan[i]);
                    DrawShouPai(Chang.rongHeFan[i], QiaoShi.Yao.HE_LE, -1);
                    Chang.qiaoShi[Chang.rongHeFan[i]].ZiMo(Chang.shePai);
                    // 和了
                    Chang.qiaoShi[Chang.rongHeFan[i]].HeLe();
                }
                Chang.heleFan = Chang.mingFan;
                DrawXuanShangPai();
                tingPaiLianZhuang = (Chang.mingFan == Chang.qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG);
                yield return Pause(ForwardMode.FAST_FORWARD);
                eventStatus = Event.YI_BIAO_SHI;
                yield break;
            }

            for (int i = Chang.ziMoFan + 1; i < Chang.ziMoFan + Chang.mianZi; i++)
            {
                // 振聴牌処理
                Chang.qiaoShi[i % Chang.mianZi].ZhenTingPaiChuLi();
            }

            switch (Chang.ziJiaYao)
            {
                case QiaoShi.Yao.LI_ZHI:
                    // 立直成立
                    Chang.LiZhiChuLi();
                    Chang.qiaoShi[Chang.ziMoFan].LiZiChuLi();
                    Chang.qiaoShi[Chang.ziMoFan].ShePaiChuLi(QiaoShi.Yao.LI_ZHI);
                    // 描画
                    DrawShePai(Chang.ziMoFan);
                    DrawLiZi(Chang.ziMoFan);
                    DrawJuFrame();
                    // 四家立直判定
                    if (Chang.SiJiaLiZhiPanDing())
                    {
                        // 描画
                        DrawSiJiaLiZhi(Chang.ziMoFan);
                        tingPaiLianZhuang = GuiZe.siJiaLiZhiLianZhuang ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        eventStatus = Event.DIAN_BIAO_SHI;
                        yield break;
                    }
                    break;

                case QiaoShi.Yao.JIA_GANG:
                    // 加槓成立
                    // 消
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        Chang.qiaoShi[i].Xiao();
                    }
                    // 加槓処理
                    Pai.QiangGangChuLi();
                    // 嶺上牌処理
                    Pai.LingShangPaiChuLi(Chang.ziMoFan);
                    // 描画
                    DrawJuFrame();
                    DrawJiaGang(Chang.ziMoFan);
                    DrawXuanShangPai();
                    // 四開槓判定
                    if (Pai.SiKaiGangPanDing())
                    {
                        // 描画
                        DrawSiKaiGangPanDing(Chang.ziMoFan);
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

            switch (Chang.taJiaYao)
            {
                case QiaoShi.Yao.DA_MING_GANG:
                    // 大明槓
                    Chang.qiaoShi[Chang.mingFan].DaMingGang();
                    // 描画
                    DrawDaMingGang(Chang.mingFan);
                    // 捨牌処理
                    Chang.qiaoShi[Chang.ziMoFan].ShePaiChuLi(QiaoShi.Yao.DA_MING_GANG);
                    // 消
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        Chang.qiaoShi[i].Xiao();
                    }
                    // 描画
                    DrawShePai(Chang.ziMoFan);
                    DrawJuFrame();
                    DrawShouPai(Chang.mingFan, QiaoShi.Yao.WU, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);

                    Chang.ziMoFan = Chang.mingFan;
                    eventStatus = Event.DUI_JU;
                    isDuiJuCoroutine = false;
                    yield break;

                case QiaoShi.Yao.BING:
                    // 石並
                    Chang.qiaoShi[Chang.mingFan].Bing();
                    // 描画
                    DrawBing(Chang.mingFan);
                    // 捨牌処理
                    Chang.qiaoShi[Chang.ziMoFan].ShePaiChuLi(QiaoShi.Yao.BING);
                    // 消
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        Chang.qiaoShi[i].Xiao();
                    }
                    // 描画
                    DrawShePai(Chang.ziMoFan);
                    DrawShouPai(Chang.mingFan, QiaoShi.Yao.WU, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);

                    Chang.ziMoFan = Chang.mingFan;
                    eventStatus = Event.DUI_JU;
                    isDuiJuCoroutine = false;
                    yield break;

                case QiaoShi.Yao.CHI:
                    // 吃
                    Chang.qiaoShi[Chang.mingFan].Chi();
                    // 描画
                    DrawChi(Chang.mingFan);
                    // 捨牌処理
                    Chang.qiaoShi[Chang.ziMoFan].ShePaiChuLi(QiaoShi.Yao.CHI);
                    // 消
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        Chang.qiaoShi[i].Xiao();
                    }
                    // 描画
                    DrawShePai(Chang.ziMoFan);
                    DrawShouPai(Chang.mingFan, QiaoShi.Yao.WU, -1);
                    // 四風子連打処理
                    Chang.SiFengZiLianDaChuLi(0xff);

                    Chang.ziMoFan = Chang.mingFan;
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
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        int jia = (Chang.qin + i) % Chang.mianZi;
                        if (Chang.qiaoShi[jia].LiuJu())
                        {
                            Chang.ziJiaYao = QiaoShi.Yao.ZI_MO;
                            Chang.ziMoFan = jia;
                            Chang.heleFan = jia;
                            liuManGuan = jia;
                            break;
                        }
                    }
                }
                tingPaiLianZhuang = Zhuang.LUN_ZHUANG;
                if (GuiZe.nanChangBuTingLianZhuang && Chang.changFeng >= 0x32)
                {
                    tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                }
                for (int i = 0; i < Chang.mianZi; i++)
                {
                    int jia = (Chang.qin + i) % Chang.mianZi;
                    QiaoShi shi = Chang.qiaoShi[jia];
                    // 形聴判定
                    shi.XingTingPanDing();
                    if (jia == liuManGuan)
                    {
                        // 流し満貫
                        // 描画
                        DrawLiuManGuan(jia);
                        if (jia == Chang.qin)
                        {
                            tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }

                    }
                    else if (shi.xingTing)
                    {
                        // 聴牌
                        // 描画
                        DrawTingPai(jia);
                        if (jia == Chang.qin)
                        {
                            tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                    else
                    {
                        // 不聴
                        if (shi.liZhi)
                        {
                            // 錯和(不聴立直)
                            Chang.CuHe(Chang.qiaoShi, jia);
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

            Chang.ziMoFan = (Chang.ziMoFan + 1) % Chang.mianZi;
            eventStatus = Event.DUI_JU;
            isDuiJuCoroutine = false;
        }

        // 【描画】自家腰
        private void DrawZiJiaYao(QiaoShi shi, int mingWei, int shouPaiWei, bool isFollow, bool isPass)
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
            if (shi.heLe && shi.taJiaYao == QiaoShi.Yao.WU)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.Yao.ZI_MO, mingWei, shouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (!shi.liZhi && shi.liZhiKeNengShu > 0)
            {
                int wei = mingWei;
                if (isFollow && shi.ziJiaYao == QiaoShi.Yao.LI_ZHI)
                {
                    for (int i = 0; i < shi.liZhiKeNengShu; i++)
                    {
                        if (shi.liZhiPaiWei[i] == shi.ziJiaXuanZe)
                        {
                            wei = i;
                            break;
                        }
                    }
                }
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.Yao.LI_ZHI, wei, shouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.anGangKeNengShu > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.Yao.AN_GANG, mingWei, shouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.jiaGangKeNengShu > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.Yao.JIA_GANG, mingWei, shouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.jiuZhongJiuPai)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QiaoShi.Yao.JIU_ZHONG_JIU_PAI, mingWei, shouPaiWei, isFollow);
            }
            if (isPass)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(paiWidth * 7.5f, y), QiaoShi.Yao.WU, mingWei, shouPaiWei, isFollow);
            }
        }

        // 【描画】自家腰
        private void DrawOnClickZiJiaYao(ref Button go, QiaoShi shi, Vector2 xy, QiaoShi.Yao yao, int mingWei, int shouPaiWei, bool isFollow)
        {
            go = Instantiate(goButton, goButton.transform.parent);
            go.onClick.AddListener(delegate {
                OnClickZiJiaYao(Chang.ziMoFan, shi, yao, mingWei, shouPaiWei, isFollow);
            });
            string value = shi.YaoMingButton(yao);
            if (yao == QiaoShi.Yao.WU)
            {
                value = shi.YaoMingButton(QiaoShi.Yao.CLEAR);
            }
            if (shi.follow && (shi.ziJiaYao != yao || shi.ziJiaYao == QiaoShi.Yao.WU))
            {
                TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.gray;
            }
            DrawButton(ref go, value, xy, value.Length);
        }

        // 自家腰クリック
        private void OnClickZiJiaYao(int jia, QiaoShi shi, QiaoShi.Yao yao, int mingWei, int shouPaiWei, bool isFollow)
        {
            ClearGameObject(ref goLeft);
            ClearGameObject(ref goRight);
            ClearGameObject(ref goSelect);

            if (yao == QiaoShi.Yao.WU)
            {
                DrawZiJiaYao(shi, 0, 0, false, false);
                DrawShouPai(jia, yao, -1, true, false);
                DrawDaiPai(jia, -1);
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                {
                    DrawSelectDaPai(jia, shi, shi.shouPaiWei - 1);
                }
            }
            else if (yao == QiaoShi.Yao.LI_ZHI)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.liZhiKeNengShu, shouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, true, isFollow);
                DrawDaiPai(jia, shi.liZhiPaiWei[mingWei]);
            }
            else if (yao == QiaoShi.Yao.AN_GANG && shi.anGangKeNengShu > 1)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.anGangKeNengShu, shouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, true, isFollow);
            }
            else if (yao == QiaoShi.Yao.JIA_GANG && shi.jiaGangKeNengShu > 1)
            {
                DrawZiJiaYao(shi, (mingWei + 1) % shi.jiaGangKeNengShu, shouPaiWei, false, true);
                DrawShouPai(jia, yao, mingWei, true, isFollow);
            }
            else if (yao == QiaoShi.Yao.SELECT)
            {
                DrawZiJiaYao(shi, mingWei, shouPaiWei, true, false);
                DrawShouPai(jia, yao, shouPaiWei, true, isFollow);
                DrawDaiPai(jia, shouPaiWei);
            }
            else
            {
                shi.ziJiaYao = yao;
                shi.ziJiaXuanZe = mingWei;
                keyPress = true;
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
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.Yao.RONG_HE, mingWei);
                x += width;
                index++;
            }
            if (shi.chiKeNengShu > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.Yao.CHI, isFollow && shi.taJiaYao == QiaoShi.Yao.CHI ? shi.taJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.bingKeNengShu > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.Yao.BING, isFollow && shi.taJiaYao == QiaoShi.Yao.BING ? shi.taJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.daMingGangKeNengShu > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QiaoShi.Yao.DA_MING_GANG, mingWei);
                index++;
            }
            if (index > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(paiWidth * 7.5f, y), QiaoShi.Yao.WU, mingWei);
            }
        }

        // 【描画】他家腰
        private void DrawOnClickTaJiaYao(ref Button go, int jia, QiaoShi shi, Vector2 xy, QiaoShi.Yao yao, int mingWei)
        {
            go = Instantiate(goButton, goButton.transform.parent);
            go.onClick.AddListener(delegate {
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
        private void OnClickTaJiaYao(int jia, QiaoShi shi, QiaoShi.Yao yao, int mingWei)
        {
            if (yao == QiaoShi.Yao.BING && shi.bingKeNengShu > 1)
            {
                DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.bingKeNengShu, false);
                DrawShouPai(jia, yao, mingWei);
            }
            else if (yao == QiaoShi.Yao.CHI && shi.chiKeNengShu > 1)
            {
                DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.chiKeNengShu, false);
                DrawShouPai(jia, yao, mingWei);
            }
            else
            {
                shi.taJiaYao = yao;
                shi.taJiaXuanZe = 0;
                keyPress = true;
            }
        }

        // 【描画】手牌
        private void DrawShouPai(int jia, QiaoShi.Yao yao, int mingWei)
        {
            DrawShouPai(jia, yao, mingWei, false, false);
        }
        private void DrawShouPai(int jia, QiaoShi.Yao yao, int mingWei, bool isPlayerZiMo, bool isFollow)
        {
            QiaoShi shi = Chang.qiaoShi[jia];
            ClearGameObject(ref shi.goShouPai);
            for (int i = 0; i < shi.goFuLuPai.Length; i++)
            {
                ClearGameObject(ref shi.goFuLuPai[i]);
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
            for (int i = 0; i < shi.shouPaiWei; i++)
            {
                int p = shi.shouPai[i];
                shi.goShouPai[i] = Instantiate(goPai, goPai.transform.parent);
                if (shi.playOrder != 0 && yao != QiaoShi.Yao.TING_PAI && yao != QiaoShi.Yao.HE_LE && yao != QiaoShi.Yao.JIU_ZHONG_JIU_PAI && yao != QiaoShi.Yao.CU_HE && p != 0xff)
                {
                    if (!shouPaiOpen)
                    {
                        p = 0x00;
                    }
                    shi.goShouPai[i].transform.SetSiblingIndex(0);
                }
                float yy = y;
                if (shi.player)
                {
                    shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE;
                    if (orientation != ScreenOrientation.Portrait)
                    {
                        shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE_LANDSCAPE;
                    }

                    int wei = i;
                    if (yao == QiaoShi.Yao.LI_ZHI)
                    {
                        if (shi.liZhiPaiWei[mingWei] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, wei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.Yao.AN_GANG)
                    {
                        if (shi.anGangPaiWei[mingWei][0] == i || shi.anGangPaiWei[mingWei][1] == i || shi.anGangPaiWei[mingWei][2] == i || shi.anGangPaiWei[mingWei][3] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.Yao.JIA_GANG)
                    {
                        if (shi.jiaGangPaiWei[mingWei][0] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.Yao.BING)
                    {
                        if (shi.bingPaiWei[mingWei][0] == i || shi.bingPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.Yao.CHI)
                    {
                        if (shi.chiPaiWei[mingWei][0] == i || shi.chiPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QiaoShi.Yao.WU && mingWei >= -1)
                    {
                        if (isFollow && shi.ziJiaXuanZe == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, QiaoShi.Yao.DA_PAI, wei); });
                            yy = y + margin;
                        }
                        else if (!shi.liZhi || shi.shouPaiWei - 1 == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, yao, wei); });
                        }
                    }
                    if (yao == QiaoShi.Yao.SELECT)
                    {
                        if (mingWei == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, QiaoShi.Yao.DA_PAI, wei); });
                            yy = y + margin;
                        }
                        else
                        {
                            shi.goShouPai[i].onClick.AddListener(delegate { OnClickShouPai(jia, shi, QiaoShi.Yao.WU, wei); });
                        }
                    }
                    if (isPlayerZiMo && shi.shouPaiWei - 1 == i && !(shi.taJiaYao == QiaoShi.Yao.BING || shi.taJiaYao == QiaoShi.Yao.CHI))
                    {
                        x += pw / 5;
                    }
                    if (sheDing.xuanShangYin)
                    {
                        shi.goShouPai[i].GetComponentInChildren<TextMeshProUGUI>().text = shi.shouPaiXuanShang[i] ? "▼" : "";
                    }
                }
                if (p != 0xff)
                {
                    if (p == 0x00 && !shi.player && yao != QiaoShi.Yao.BU_TING)
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
            for (int i = 0; i < shi.fuLuPaiWei; i++)
            {
                int fuLuJia = shi.fuLuJia[i];
                QiaoShi.Yao fuLuZhong = shi.fuLuZhong[i];
                for (int j = shi.fuLuPai[i].Length - 1; j >= 0; j--)
                {
                    int p = shi.fuLuPai[i][j];
                    if (p == 0xff)
                    {
                        continue;
                    }
                    if (fuLuZhong == QiaoShi.Yao.JIA_GANG && j == 3)
                    {
                        continue;
                    }
                    bool isMingPai = shi.MingPaiPanDing(fuLuZhong, fuLuJia, j);
                    if (fuLuZhong == QiaoShi.Yao.AN_GANG && (j == 0 || j == 3))
                    {
                        p = 0x00;
                    }
                    shi.goFuLuPai[i][j] = Instantiate(goPai, goPai.transform.parent);
                    shi.goFuLuPai[i][j].transform.SetSiblingIndex(2);
                    shi.goFuLuPai[i][j].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    if (fuLuZhong == QiaoShi.Yao.JIA_GANG && isMingPai)
                    {
                        shi.goFuLuPai[i][3] = Instantiate(goPai, goPai.transform.parent);
                        shi.goFuLuPai[i][3].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    }
                    if (isMingPai)
                    {
                        x -= (paiHeight / 2);
                        DrawPai(ref shi.goFuLuPai[i][j], p, Cal(x, y - (paiHeight - paiWidth) / 2, shi.playOrder), 90);
                        if (fuLuZhong == QiaoShi.Yao.JIA_GANG)
                        {
                            DrawPai(ref shi.goFuLuPai[i][3], p, Cal(x, y - (paiHeight - paiWidth) / 2 + paiWidth, shi.playOrder), 90);
                        }
                        x -= (paiHeight / 2);
                    }
                    else
                    {
                        x -= (paiWidth / 2);
                        DrawPai(ref shi.goFuLuPai[i][j], p, Cal(x, y, shi.playOrder), 0);
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

            DrawShouPai(jia, QiaoShi.Yao.SELECT, xuanZe, true, true);

            goLeft = Instantiate(goButton, goButton.transform.parent);
            int leftWei = xuanZe - 1;
            if (leftWei < 0)
            {
                leftWei = shi.shouPaiWei - 1;
            }
            goLeft.onClick.AddListener(delegate {
                DrawSelectDaPai(jia, shi, leftWei);
                DrawDaiPai(jia, leftWei);
            });

            goRight = Instantiate(goButton, goButton.transform.parent);
            int rightWei = xuanZe + 1;
            if (rightWei > shi.shouPaiWei - 1)
            {
                rightWei = 0;
            }
            goRight.onClick.AddListener(delegate {
                DrawSelectDaPai(jia, shi, rightWei);
                DrawDaiPai(jia, rightWei);
            });

            goSelect = Instantiate(goButton, goButton.transform.parent);
            goSelect.onClick.AddListener(delegate {
                shi.ziJiaYao = QiaoShi.Yao.WU;
                shi.ziJiaXuanZe = xuanZe;
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
        private void OnClickShouPai(int jia, QiaoShi shi, QiaoShi.Yao yao, int xuanZe)
        {
            if (yao == QiaoShi.Yao.LI_ZHI || yao == QiaoShi.Yao.AN_GANG || yao == QiaoShi.Yao.JIA_GANG)
            {
                shi.ziJiaYao = yao;
                shi.ziJiaXuanZe = xuanZe;
            }
            if (yao == QiaoShi.Yao.BING || yao == QiaoShi.Yao.CHI)
            {
                shi.taJiaYao = yao;
                shi.taJiaXuanZe = xuanZe;
            }
            if (yao == QiaoShi.Yao.WU)
            {
                if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.TAP_1)
                {
                    shi.ziJiaYao = QiaoShi.Yao.WU;
                    shi.ziJiaXuanZe = xuanZe;
                }
                else
                {
                    DrawShouPai(jia, QiaoShi.Yao.SELECT, xuanZe, true, true);
                    DrawDaiPai(jia, xuanZe);
                    if (sheDing.daPaiFangFa == SheDing.DaPaiFangFa.SELECT)
                    {
                        DrawSelectDaPai(jia, shi, xuanZe);
                    }
                    return;
                }
            }
            if (yao == QiaoShi.Yao.DA_PAI)
            {
                shi.ziJiaYao = QiaoShi.Yao.WU;
                shi.ziJiaXuanZe = xuanZe;
                DrawShouPai(jia, QiaoShi.Yao.CLEAR, 0, false, false);
            }
            keyPress = true;
        }

        // 【描画】待牌
        private void DrawDaiPai(int jia, int xuanZe)
        {
            QiaoShi shi = Chang.qiaoShi[jia];
            // 待牌
            if (!shi.player)
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
                for (int i = 0; i < shi.daiPaiShu; i++)
                {
                    int p = shi.daiPai[i] & QiaoShi.QIAO_PAI;
                    if (p == 0xff)
                    {
                        ClearGameObject(ref shi.goDaiPai[i]);
                    }
                    else
                    {
                        DrawPai(ref shi.goDaiPai[i], p, Cal(x, y, shi.playOrder), 0);
                        DrawText(ref shi.goCanPaiShu[i], (4 - shi.gongKaiPaiShu[p]).ToString(), Cal(x, y + paiWidth * 1.2f, shi.playOrder), 0, 17);
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
                if (shi.daiPaiShu == 0)
                {
                    if (xuanZe != -2)
                    {
                        shi.XiangTingShuJiSuan(xuanZe);
                    }
                    if (shi.xiangTingShu > 0)
                    {
                        DrawText(ref shi.goXiangTingShu, shi.xiangTingShu.ToString() + "シャンテン", Cal(x, y, shi.playOrder), 0, 18);
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
            QiaoShi shi = Chang.qiaoShi[jia];

            ClearGameObject(ref shi.goShePai);
            int shePaiEnter = 6;
            float shePaiLeft = 2.5f;
            if (Chang.mianZi == 2)
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
            if (Chang.mianZi == 2 && playOrder == 1)
            {
                playOrder = 2;
            }
            for (int i = 0; i < shi.shePaiWei; i++)
            {
                int p = shi.shePai[i] & 0xff;
                if (shi.shePaiYao[i] == QiaoShi.Yao.WU || shi.shePaiYao[i] == QiaoShi.Yao.LI_ZHI)
                {
                    shu++;

                    shi.goShePai[i] = Instantiate(goPai, goPai.transform.parent);
                    shi.goShePai[i].transform.SetSiblingIndex(3);
                    if (shi.shePaiYao[i] == QiaoShi.Yao.LI_ZHI || (!isDrawLizhi && shi.liZhiWei < i))
                    {
                        x += (paiHeight - paiWidth) / 2;
                        shi.goShePai[i].transform.Rotate(0, 0, 90);
                        isDrawLizhi = true;
                        DrawPai(ref shi.goShePai[i], p, Cal(x, y, playOrder), 90 * GetDrawOrder(playOrder));
                        x += (paiHeight - paiWidth) / 2;
                    }
                    else
                    {
                        if (ming && i == shi.shePaiWei - 1)
                        {
                            shi.goShePai[i].GetComponentInChildren<TextMeshProUGUI>().text = "▼";
                            shi.goShePai[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
                        }
                        DrawPai(ref shi.goShePai[i], p, Cal(x, y, playOrder), 90 * GetDrawOrder(playOrder));
                    }
                    if (sheDing.ziMoQieBiaoShi && shi.shePaiZiMoQie[i])
                    {
                        Image img = shi.goShePai[i].GetComponentInChildren<Image>();
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
            }
        }

        // 【描画】声
        private void DrawShang(int jia, string text)
        {
            QiaoShi shi = Chang.qiaoShi[jia];
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
            goSheng[jia].transform.rotation = Quaternion.Euler(0, 0, 90 * GetDrawOrder(shi.playOrder));
            RectTransform rt = goSheng[jia].GetComponent<RectTransform>();
            rt.anchoredPosition = Cal(0, -(paiWidth * 3 + paiHeight * 2.3f), shi.playOrder);
            rt.sizeDelta = new Vector2(goText.preferredWidth + paiWidth / 2, rt.sizeDelta.y);
        }

        // 【描画】自摸
        private void DrawZiMo(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.ZI_MO));
        }

        // 【描画】九種九牌
        private void DrawJiuZhongJiuPai(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.JIU_ZHONG_JIU_PAI));
        }

        // 【描画】立直
        private void DrawLiZi(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.LI_ZHI));
        }

        // 【描画】暗槓
        private void DrawAnGang(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.AN_GANG));
        }

        // 【描画】流し満貫
        private void DrawLiuManGuan(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.LIU_MAN_GUAN));
        }

        // 【描画】四開槓
        private void DrawSiKaiGangPanDing(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.SI_KAI_GANG));
        }

        // 【描画】加槓
        private void DrawJiaGang(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.JIA_GANG));
        }

        // 【描画】大明槓
        private void DrawDaMingGang(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.DA_MING_GANG));
        }

        // 【描画】石並
        private void DrawBing(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.BING));
        }

        // 【描画】吃
        private void DrawChi(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.CHI));
        }

        // 【描画】四風子連打
        private void DrawSiFengZiLianDa(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.SI_FENG_ZI_LIAN_DA));
        }

        // 【描画】四家立直
        private void DrawSiJiaLiZhi(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.SI_JIA_LI_ZHI));
        }

        // 【描画】栄和
        private void DrawRongHe(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.RONG_HE));
        }

        // 【描画】聴牌
        private void DrawTingPai(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.TING_PAI));
            DrawShouPai(jia, QiaoShi.Yao.TING_PAI, -1);
        }

        // 【描画】不聴
        private void DrawBuTing(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.BU_TING));
            DrawShouPai(jia, QiaoShi.Yao.BU_TING, -1);
        }

        // 【描画】錯和
        private void DrawCuHe(int jia)
        {
            DrawShang(jia, QiaoShi.YaoMing(QiaoShi.Yao.CU_HE) + " " + Chang.qiaoShi[Chang.cuHeFan].cuHeSheng);
            DrawShouPai(jia, QiaoShi.Yao.CU_HE, 0);
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

            if (Chang.rongHeShu > 0)
            {
                for (int i = 0; i < Chang.rongHeShu; i++)
                {
                    // 描画
                    ClearScreen();
                    DrawBackDuiJuZhongLe();
                    DrawYi(Chang.rongHeFan[i]);

                    yield return Pause(ForwardMode.FAST_FORWARD);
                }
            }
            else
            {
                // 描画
                ClearScreen();
                DrawBackDuiJuZhongLe();
                DrawYi(Chang.heleFan);

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

                QiaoShi shi = Chang.qiaoShi[jia];

                // 名前
                x = 0f;
                y -= (paiHeight * 3.5f);
                DrawText(ref goMingQian[jia], shi.mingQian, new Vector2(x, y), 0, 30);

                // 手牌
                int tp = shi.shouPai[shi.shouPaiWei - 1];
                shi.shouPai[shi.shouPaiWei - 1] = 0xff;
                QiaoShi.Sort(shi.shouPai);
                shi.shouPai[shi.shouPaiWei - 1] = tp;

                float pw = paiWidth * PLAYER_PAI_SCALE;
                x = -(pw * 6.5f);
                y -= paiHeight * 1.5f;
                float ph = paiHeight * PLAYER_PAI_SCALE;
                for (int i = 0; i < shi.shouPaiWei; i++)
                {
                    int p = shi.shouPai[i];
                    shi.goShouPai[i] = Instantiate(goPai, goPai.transform.parent);
                    shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE;
                    if (p != 0xff)
                    {
                        DrawPai(ref shi.goShouPai[i], p, new Vector2(x, y), 0);
                    }
                    x += pw;
                    if (p == 0xff)
                    {
                        ClearGameObject(ref shi.goShouPai[i]);
                    }
                }

                x = paiWidth * 9f;
                y -= ph;
                for (int i = 0; i < shi.fuLuPaiWei; i++)
                {
                    int fuLuJia = shi.fuLuJia[i];
                    QiaoShi.Yao fuLuZhong = shi.fuLuZhong[i];

                    for (int j = shi.fuLuPai[i].Length - 1; j >= 0; j--)
                    {
                        int p = shi.fuLuPai[i][j];
                        if (p == 0xff)
                        {
                            continue;
                        }
                        if (fuLuZhong == QiaoShi.Yao.JIA_GANG && j == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(fuLuZhong, fuLuJia, j);
                        if (fuLuZhong == QiaoShi.Yao.AN_GANG && (j == 0 || j == 3))
                        {
                            p = 0x00;
                        }
                        shi.goFuLuPai[i][j] = Instantiate(goPai, goPai.transform.parent);
                        if (fuLuZhong == QiaoShi.Yao.JIA_GANG && isMingPai)
                        {
                            shi.goFuLuPai[i][3] = Instantiate(goPai, goPai.transform.parent);
                        }
                        if (isMingPai)
                        {
                            x -= (paiHeight / 2);
                            DrawPai(ref shi.goFuLuPai[i][j], p, new Vector2(x, y - (paiHeight - paiWidth) / 2), 90);
                            if (fuLuZhong == QiaoShi.Yao.JIA_GANG)
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

                string hele = (shi.fanShuJi >= 13 ? QiaoShi.DE_DIAN_YI[13] : QiaoShi.DE_DIAN_YI[shi.fanShuJi]);
                if (hele == "")
                {
                    if ((shi.fu >= 40 && shi.fanShuJi >= 4) || (shi.fu >= 70 && shi.fanShuJi >= 3))
                    {
                        hele = QiaoShi.DE_DIAN_YI[5];
                    }
                }
                string value = (shi.fu > 0 ? shi.fu + "符" : "") + (shi.yiMan ? "役満 " : shi.fanShuJi + "飜 ") + hele + " " + shi.heLeDian + "点";
                y -= paiHeight * 1.5f;
                DrawText(ref goFu, value, new Vector2(0, y), 0, 30);
                y -= paiHeight * 0.5f;
                string[] yi = new string[shi.yiShu];
                for (int i = 0; i < shi.yiShu; i++)
                {
                    yi[i] = (shi.yiMan ? QiaoShi.YI_MAN_MING[shi.yi[i]] : QiaoShi.YI_MING[shi.yi[i]]);
                    // 記録 役数・役満数
                    if (shi.yiMan)
                    {
                        shi.jiLu.yiManShu[shi.yi[i]]++;
                    }
                    else
                    {
                        shi.jiLu.yiShu[shi.yi[i]]++;
                    }
                }
                for (int i = 0; i < shi.yiShu; i++)
                {
                    y -= paiHeight;
                    string text = yi[i];
                    for (int j = text.Length; j < 6; j++)
                    {
                        text += "　";
                    }
                    goYi[i] = Instantiate(goText, goText.transform.parent);
                    DrawText(ref goYi[i], text, new Vector2(-paiWidth * 3.5f, y), 0, 25, TextAlignmentOptions.Left);
                    goFanShu[i] = Instantiate(goText, goText.transform.parent);
                    DrawText(ref goFanShu[i], shi.fanShu[i].ToString(), new Vector2(paiWidth * 3.5f, y), 0, 25, TextAlignmentOptions.Right);
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
            Chang.DianJiSuan(Chang.qiaoShi);

            float x = 0;
            float y = -(paiHeight * 4);
            int max = 0;
            for (int i = 0; i < Chang.mianZi; i++)
            {
                int jia = (Chang.qin + i) % Chang.mianZi;
                QiaoShi shi = Chang.qiaoShi[jia];
                // 受取
                string shouQuGongTuo = (shi.shouQuGongTuo > 0 ? "+" : "") + (shi.shouQuGongTuo == 0 ? "" : shi.shouQuGongTuo.ToString());
                string shouQu = (shi.shouQu - shi.shouQuGongTuo > 0 ? "+" : "") + (shi.shouQu - shi.shouQuGongTuo == 0 ? "" : (shi.shouQu - shi.shouQuGongTuo).ToString());
                DrawText(ref goShouQuGongTuo[jia], shouQuGongTuo, Cal(0, -(paiHeight * 2.5f), shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 20, TextAlignmentOptions.Right);
                DrawText(ref goShouQu[jia], shouQu, Cal(0, -(paiHeight * 3), shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 20, TextAlignmentOptions.Right);

                DrawText(ref goDianBang[jia], (shi.dianBang - shi.shouQu).ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 30);
                if (Math.Abs(shi.shouQu) > max)
                {
                    max = Math.Abs(shi.shouQu);
                }
            }
            if (forwardMode > 0)
            {
                keyPress = true;
            }
            for (int shu = 0; shu <= max; shu += 100)
            {
                for (int i = 0; i < Chang.mianZi; i++)
                {
                    int jia = (Chang.qin + i) % Chang.mianZi;
                    QiaoShi shi = Chang.qiaoShi[jia];
                    if (shi.shouQu > 0)
                    {
                        if (shi.shouQu - shu >= 0)
                        {
                            DrawText(ref goDianBang[jia], (shi.dianBang - shi.shouQu + shu).ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 30);
                        }
                    }
                    else if (shi.shouQu < 0)
                    {
                        if (shi.shouQu + shu <= 0)
                        {
                            DrawText(ref goDianBang[jia], (shi.dianBang - shi.shouQu - shu).ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 30);
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
            for (int i = 0; i < Chang.mianZi; i++)
            {
                int jia = (Chang.qin + i) % Chang.mianZi;
                QiaoShi shi = Chang.qiaoShi[jia];
                DrawText(ref goDianBang[jia], (shi.dianBang).ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 30);
            }

            // 記録の書込
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                // 記録 対局数
                shi.jiLu.duiJuShu++;
                File.WriteAllText(Application.persistentDataPath + "/" + shi.mingQian + ".json", JsonUtility.ToJson(shi.jiLu));
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
            if (Chang.changFeng == 0x32 && Chang.ju == Chang.mianZi)
            {
                QiaoShi qinShi = Chang.qiaoShi[Chang.qin];
                if (qinShi.ziJiaYao == QiaoShi.Yao.ZI_MO || qinShi.taJiaYao == QiaoShi.Yao.RONG_HE)
                {
                    // 親の和了
                    int maxDian = 0;
                    for (int i = 0; i < Chang.mianZi; i++)
                    {
                        int jia = (Chang.qin + i) % Chang.mianZi;
                        QiaoShi shi = Chang.qiaoShi[jia];
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
            if (Chang.changFeng > 0x32 || (GuiZe.xiang && Chang.XiangPanDing(Chang.qiaoShi)) || isQinTop)
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
            for (int i = Chang.qiaJia; i < Chang.qiaJia + Chang.mianZi; i++)
            {
                int jia = i % Chang.mianZi;
                QiaoShi shi = Chang.qiaoShi[jia];
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
                File.WriteAllText(Application.persistentDataPath + "/" + shi.mingQian + ".json", JsonUtility.ToJson(shi.jiLu));
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
            for (int i = Chang.qiaJia; i < Chang.qiaJia + Chang.mianZi; i++)
            {
                int jia = i % Chang.mianZi;
                QiaoShi shi = Chang.qiaoShi[jia];

                if (maxDian < shi.dianBang)
                {
                    maxDian = shi.dianBang;
                    top = jia;
                }
                int deDian = shi.dianBang / 1000;
                deDian -= (GuiZe.fanDian / 1000);
                geHe += deDian;
                shi.jiJiDian = deDian;
            }
            Chang.qiaoShi[top].jiJiDian -= geHe;
        }

        // 【描画】荘終了
        private void DrawZhuangZhong()
        {
            float y = paiHeight * 4;
            int maxMingQian = 0;
            int maxDianBang = 0;
            int maxDeDian = 0;
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
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

            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                string dianBang = shi.dianBang.ToString();
                for (int j = dianBang.Length; j < maxDianBang; j++)
                {
                    dianBang = " " + dianBang;
                }
                string deDian = (shi.jiJiDian > 0 ? "+" : "") + shi.jiJiDian.ToString();
                for (int j = deDian.Length; j < maxDeDian; j++)
                {
                    deDian = " " + deDian;
                }

                DrawText(ref goMingQian[i], shi.mingQian, new Vector2(-(paiWidth * 7f), y), 0, 30, TextAlignmentOptions.Left);
                DrawText(ref goDianBang[i], dianBang, new Vector2(paiWidth * 3f, y), 0, 30, TextAlignmentOptions.Right);
                DrawText(ref goShouQu[i], deDian, new Vector2(paiWidth * 7f, y), 0, 25, TextAlignmentOptions.Right);
                y -= paiHeight * 2;
            }
        }

        // 【描画】テキスト
        private void DrawText(ref TextMeshProUGUI obj, string value, Vector2 xy, int quaternion, int fontSize)
        {
            DrawText(ref obj, value, xy, quaternion, fontSize, TextAlignmentOptions.Center);
        }
        private void DrawText(ref TextMeshProUGUI obj, string value, Vector2 xy, int quaternion, int fontSize, TextAlignmentOptions align)
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
            rt.sizeDelta = new Vector2(paiWidth, obj.preferredHeight);
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
            if (Chang.mianZi == 3 && order == 2)
            {
                return 3;
            }
            if (Chang.mianZi == 2 && order == 1)
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

            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi qs = Chang.qiaoShi[i];
                if (qs == null)
                {
                    continue;
                }
                ClearGameObject(ref qs.goShouPai);
                for (int j = 0; j < qs.goFuLuPai.Length; j++)
                {
                    ClearGameObject(ref qs.goFuLuPai[j]);
                }
                ClearGameObject(ref qs.goShePai);
                ClearGameObject(ref qs.goDaiPai);
                ClearGameObject(ref qs.goCanPaiShu);
                ClearGameObject(ref qs.goXiangTingShu);
            }

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
            go.text = "";
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
        private void ClearGameObject(ref TextMeshProUGUI[] go)
        {
            for (int i = 0; i < go.Length; i++)
            {
                ClearGameObject(ref go[i]);
            }
        }

        // ボタンクリア
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

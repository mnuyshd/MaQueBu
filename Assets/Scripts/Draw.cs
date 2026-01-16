using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Assets.Scripts.Gongtong;
using Assets.Scripts.Sikao;

namespace Assets.Scripts
{
    [DefaultExecutionOrder(2)]
    public class Draw : MonoBehaviour
    {
        public static Draw Instance;

        // プレイヤー名
        public readonly string PLAYER_NAME = "プレイヤー";

        // プレイヤー牌倍率
        private static readonly float PLAYER_PAI_SCALE = 1.4f;
        private static readonly float PLAYER_PAI_SCALE_LANDSCAPE = 1.2f;

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
        private Button[] goQueShi;
        private Button goRandom;
        private Button goDuiJuMode;
        private Button[] goYao;
        private Image[] goLizhiBang;
        private Image goQiJia;
        private Image goSai1;
        private Image goSai2;
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
        private Button[] goScoreQueShi;
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

        // スケール
        private Vector3 scale;
        // 牌横幅
        private float paiWidth;
        // 牌高さ
        private float paiHeight;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Init()
        {
            goQueShi = new Button[GameManager.allQueShis.Count];
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
            goScreen.onClick.AddListener(() => MaQue.Instance.OnClickScreen());
            goScreen.transform.SetSiblingIndex(10);
            // ボタン
            goYao = new Button[5];
            goLizhiBang = new Image[4];
            // プレイヤー以外の手牌画像作成
            Sprite sprite = Resources.Load<Sprite>("0x00");
            int tHeight = sprite.texture.height / 7;
            Texture2D texture = new(sprite.texture.width, tHeight);
            texture.SetPixels(sprite.texture.GetPixels(0, sprite.texture.height - tHeight, sprite.texture.width, tHeight));
            texture.Apply();
            goJiXieShouPai = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // スケール設定
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
        }

        // スケール設定
        public void SetScale()
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
        }

        // 初期画面表示
        public void DrawInitialDisplay()
        {
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

        // デバッグボタン
        private void DrawDebugOption()
        {
            // 早送り
            Button goFast = GameObject.Find("Fast").GetComponent<Button>();
            goFast.transform.SetSiblingIndex(20);
            goFast.onClick.AddListener(MaQue.Instance.OnClickFast);
            RectTransform rtFast = goFast.GetComponent<RectTransform>();
            rtFast.localScale *= scale.x;
            rtFast.anchorMin = rtFast.anchorMax = new Vector2(0, 1);
            rtFast.pivot = new Vector2(0, 1);
            rtFast.anchoredPosition = new Vector2(paiWidth * 3.2f, -paiHeight * 0.4f);
            // 再生
            Button goReproduction = GameObject.Find("Reproduction").GetComponent<Button>();
            goReproduction.transform.SetSiblingIndex(20);
            goReproduction.onClick.AddListener(MaQue.Instance.OnClickReproduction);
            RectTransform rtReproduction = goReproduction.GetComponent<RectTransform>();
            rtReproduction.localScale *= scale.x;
            rtReproduction.anchorMin = rtReproduction.anchorMax = new Vector2(0, 1);
            rtReproduction.pivot = new Vector2(0, 1);
            rtReproduction.anchoredPosition = new Vector2(paiWidth * 0.7f, -paiHeight * 0.4f);
        }

        // 設定画面
        private void DrawSettingPanel()
        {
            // 設定パネル
            goSettingPanel = GameObject.Find("SettingPanel");
            EventTrigger etSettingPanel = goSettingPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eSettingPanel = new() { eventID = EventTriggerType.PointerClick };
            eSettingPanel.callback.AddListener((eventData) => { goSettingPanel.SetActive(false); });
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
            goRestart.onClick.AddListener(MaQue.Instance.RestartGame);
            RectTransform rtRestart = goRestart.GetComponent<RectTransform>();
            rtRestart.localScale *= scale.x;
            rtRestart.anchorMin = rtRestart.anchorMax = new Vector2(0, 1);
            rtRestart.pivot = new Vector2(0, 1);
            rtRestart.anchoredPosition = new Vector2(paiWidth, -(rtRestart.sizeDelta.y * scale.x));

            // オプションボタン
            DrawOption();

            goSettingDialogPanel = GameObject.Find("SettingDialogPanel");

            EventTrigger etResetPanel = goSettingDialogPanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eResetPanel = new() { eventID = EventTriggerType.PointerClick };
            eResetPanel.callback.AddListener((eventData) => goSettingDialogPanel.SetActive(false));
            etResetPanel.triggers.Add(eResetPanel);

            goSettingDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goSettingDialogPanel.transform);
            DrawText(ref message, "全ての設定をリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goSettingDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(() =>
            {
                MaQue.Instance.ResetSheDing();
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

        // オプションボタン
        public void DrawOption()
        {
            // オプション
            float x = paiWidth * 4.5f;
            float y = paiHeight * 4.5f;
            float offset = paiHeight * 1.6f;
            int len = 7;

            // int型オプション
            (Func<int> get, Action<int> set, string[] labels, Vector2 pos)[] intOptions = {
                (() => SheDing.Instance.daPaiFangFa, v => SheDing.Instance.daPaiFangFa = v, new[] { "１タップ打牌", "２タップ打牌" }, new Vector2(-x, y)),
            };
            foreach (var (get, set, labels, pos) in intOptions)
            {
                DrawToggleOption(get, set, labels, pos, len);
            }
            // bool型オプション
            (Func<bool> get, Action<bool> set, string[] labels, Vector2 pos)[] boolOptions = {
                (() => SheDing.Instance.liZhiAuto, v => SheDing.Instance.liZhiAuto = v, new[] { "立直後自動打牌", "立直後手動打牌" }, new Vector2(x, y)),
                (() => SheDing.Instance.xuanShangYin, v => SheDing.Instance.xuanShangYin = v, new[] { "ドラマーク有り", "ドラマーク無し" }, new Vector2(-x, y - offset)),
                (() => SheDing.Instance.ziMoQieBiaoShi, v => SheDing.Instance.ziMoQieBiaoShi = v, new[] { "ツモ切表示有り", "ツモ切表示無し" }, new Vector2(x, y - offset)),
                (() => SheDing.Instance.daiPaiBiaoShi, v => SheDing.Instance.daiPaiBiaoShi = v, new[] { "待牌表示有り", "待牌表示無し" }, new Vector2(-x, y - offset * 2)),
                (() => SheDing.Instance.xiangTingShuBiaoShi, v => SheDing.Instance.xiangTingShuBiaoShi = v, new[] { "向聴数表示有り", "向聴数表示無し" }, new Vector2(x, y - offset * 2)),
                (() => SheDing.Instance.mingQuXiao, v => SheDing.Instance.mingQuXiao = v, new[] { "鳴パスはボタン", "鳴パスはタップ" }, new Vector2(-x, y - offset * 3)),
                (() => SheDing.Instance.xiangShouPaiOpen, v => SheDing.Instance.xiangShouPaiOpen = v, new[] { "相手牌オープン", "相手牌クローズ" }, new Vector2(-x, y - offset * 4)),
                (() => SheDing.Instance.shouPaiDianBiaoShi, v => SheDing.Instance.shouPaiDianBiaoShi = v, new[] { "手牌点表示有り", "手牌点表示無し" }, new Vector2(x, y - offset * 4)),
                (() => SheDing.Instance.learningData, v => SheDing.Instance.learningData = v, new[] { "学習データ有り", "学習データ無し" }, new Vector2(-x, y - offset * 5)),
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
                setValue(newValue);
                button.GetComponentInChildren<TextMeshProUGUI>().text = getText(newValue);
                FileUtility.WriteSheDingFile();
                switch (Chang.Instance.eventStatus)
                {
                    case GameManager.Event.PEI_PAI:
                    case GameManager.Event.DUI_JU:
                    case GameManager.Event.DUI_JU_ZHONG_LE:
                        Chang.Instance.isDuiJuDraw = true;
                        break;
                }
            });
        }

        // 得点画面
        public void DrawScorePanel()
        {
            // 得点パネル
            goScorePanel = GameObject.Find("ScorePanel");
            EventTrigger etScore = goScorePanel.AddComponent<EventTrigger>();
            EventTrigger.Entry eScore = new() { eventID = EventTriggerType.PointerClick };
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

            goScoreQueShi = new Button[GameManager.allQueShis.Count + 1];

            float x = 0;
            float y = paiHeight * 5f;

            goScoreQueShi[GameManager.allQueShis.Count] = Instantiate(goButton, goScorePanel.transform);
            goScoreQueShi[GameManager.allQueShis.Count].onClick.AddListener(() => OnClickScoreQueShi(PLAYER_NAME));
            DrawButton(ref goScoreQueShi[GameManager.allQueShis.Count], PLAYER_NAME, new Vector2(x, y));
            y -= paiHeight * 1.5f;

            int i = 0;
            int queShiButtonMaxLen = GameManager.allQueShis.Max(shi => shi.mingQian.Length);
            foreach (QueShi shi in GameManager.allQueShis)
            {
                x = paiWidth * 4 * (i % 2 == 0 ? -1 : 1);
                int pos = i;
                goScoreQueShi[i] = Instantiate(goButton, goScorePanel.transform);
                goScoreQueShi[i].onClick.AddListener(() => OnClickScoreQueShi(shi.mingQian));
                DrawButton(ref goScoreQueShi[i], shi.mingQian, new Vector2(x, y), queShiButtonMaxLen);
                if (i % 2 == 1 || i == GameManager.allQueShis.Count - 1)
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
                MaQue.Instance.ResetJiLu();
                goScoreDialogPanel.SetActive(false);
            });
            Button goNo = Instantiate(goButton, goScoreDialogPanel.transform);
            goNo.onClick.AddListener(() => goScoreDialogPanel.SetActive(false));
            DrawButton(ref goNo, "いいえ", new Vector2(paiWidth * 3f, 0));
        }

        // 得点パネル 雀士名クリック
        private void OnClickScoreQueShi(string mingQian)
        {
            QueShi shi = MaQue.Instance.GetQueShi(mingQian, false);
            shi.jiLu = FileUtility.ReadJiLuFile(shi.mingQian);
            if (shi.jiLu == null)
            {
                shi.jiLu = new JiLu();
                MaQue.Instance.Nao2JiLu(shi);
            }
            else
            {
                // ファイル書き込み後にプログラムでローカル役を追加した場合は、配列サイズを変更する
                ResizeYiManShu(shi);
                ResizeYiShu(shi);
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
                    FileUtility.WriteJiLuFile(shi.mingQian, shi.jiLu);
                    MaQue.Instance.JiLu2Nao(shi);
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

        // データ画面
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

            goYiMing = new TextMeshProUGUI[QueShi.YiMing.Count];
            goYiShu = new TextMeshProUGUI[QueShi.YiMing.Count];
            goYiManMing = new TextMeshProUGUI[QueShi.YiManMing.Count];
            goYiManShu = new TextMeshProUGUI[QueShi.YiManMing.Count];
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
            foreach (KeyValuePair<QueShi.YiDingYi, string> kvp in QueShi.YiMing)
            {
                DrawData(ref goYiMing[index], ref goYiShu[index], kvp.Value, y -= paiHeight);
                index++;
            }
            y -= paiHeight;
            index = 0;
            foreach (KeyValuePair<QueShi.YiManDingYi, string> kvp in QueShi.YiManMing)
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

        // データ
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

        public void ResizeYiManShu(QueShi shi)
        {
            if (shi.jiLu.yiManShu.Length != QueShi.YiManMing.Count)
            {
                Array.Resize(ref shi.jiLu.yiManShu, QueShi.YiManMing.Count);
            }
        }
        public void ResizeYiShu(QueShi shi)
        {
            if (shi.jiLu.yiShu.Length != QueShi.YiMing.Count)
            {
                Array.Resize(ref shi.jiLu.yiShu, QueShi.YiMing.Count);
            }
        }

        // ルール画面
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
            EventTrigger.Entry eResetPanel = new() { eventID = EventTriggerType.PointerClick };
            eResetPanel.callback.AddListener((eventData) => goGuiZeDialogPanel.SetActive(false));
            etResetPanel.triggers.Add(eResetPanel);

            goGuiZeDialogPanel.SetActive(false);
            TextMeshProUGUI message = Instantiate(goText, goGuiZeDialogPanel.transform);
            DrawText(ref message, "全てのルールをリセットしますか？", new Vector2(0, paiHeight * 2f), 0, 25);
            Button goYes = Instantiate(goButton, goGuiZeDialogPanel.transform);
            DrawButton(ref goYes, "は　い", new Vector2(-paiWidth * 3f, 0));
            goYes.onClick.AddListener(() =>
            {
                MaQue.Instance.ResetGuiZe();
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
            DrawToggleGuiZe(() => GuiZe.Instance.banZhuang, v => GuiZe.Instance.banZhuang = v, new string[] { "半荘戦", "東風戦" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.ziMoPingHe, v => GuiZe.Instance.ziMoPingHe = v, new string[] { "ピンヅモ有り", "ピンヅモ無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.shiDuan, v => GuiZe.Instance.shiDuan = v, new string[] { "食いタン有り", "食いタン無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.shiTi, v => GuiZe.Instance.shiTi = v, new string[] { "食い替え有り", "食い替え無し（チョンボ扱い）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.wRongHe, v => GuiZe.Instance.wRongHe = v, new string[] { "ダブルロン無し（頭ハネ）", "ダブルロン有り" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.tRongHe, v => GuiZe.Instance.tRongHe = v, new string[] { "トリプルロン無し（頭ハネ）", "トリプルロン有り", "トリプルロンは流局（親連荘）", "トリプルロンは流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.baoZe, v => GuiZe.Instance.baoZe = v, new string[] { "パオ（責任払い）有り", "パオ（責任払い）無し" }, new Vector2(x, y -= offset));
            string[] chiPaiShuText = new string[] { "赤ドラ無し", "赤ドラ有り（各１枚）" };
            Button buttonChiPaiShu = Instantiate(goButton, goGuiZeContent.transform);
            DrawButton(ref buttonChiPaiShu, GuiZe.Instance.chiPaiShu[0] == 0 ? chiPaiShuText[0] : chiPaiShuText[1], new Vector2(x, y -= offset), 12);
            TextMeshProUGUI text = buttonChiPaiShu.GetComponentInChildren<TextMeshProUGUI>();
            text.fontSize = 17f;
            buttonChiPaiShu.onClick.AddListener(() =>
            {
                GuiZe.Instance.chiPaiShu = GuiZe.Instance.chiPaiShu[0] == 0 ? new int[] { 1, 1, 1 } : new int[] { 0, 0, 0 };
                text.text = GuiZe.Instance.chiPaiShu[0] == 0 ? chiPaiShuText[0] : chiPaiShuText[1];
                FileUtility.WriteGuiZeFile();
            });
            DrawToggleGuiZe(() => GuiZe.Instance.jiuZhongJiuPaiLianZhuang, v => GuiZe.Instance.jiuZhongJiuPaiLianZhuang = v, new string[] { "九種九牌無し", "九種九牌は流局（親連荘）", "九種九牌は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.siJiaLiZhiLianZhuang, v => GuiZe.Instance.siJiaLiZhiLianZhuang = v, new string[] { "四家立直は続行", "四家立直は流局（親連荘）", "四家立直は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.siFengZiLianDaLianZhuang, v => GuiZe.Instance.siFengZiLianDaLianZhuang = v, new string[] { "四風子連打無し", "四風子連打は流局（親連荘）", "四風子連打は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.siKaiGangLianZhuang, v => GuiZe.Instance.siKaiGangLianZhuang = v, new string[] { "四開槓は流局（親連荘）", "四開槓は流局（親流れ）" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.xiang, v => GuiZe.Instance.xiang = v, new string[] { "箱（０点以下で終了）有り", "箱（０点以下で終了）無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.jieJinLiZhi, v => GuiZe.Instance.jieJinLiZhi = v, new string[] { "１０００点未満のリーチ可能", "１０００点未満のリーチ不可" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.liuManGuan, v => GuiZe.Instance.liuManGuan = v, new string[] { "流し満貫有り", "流し満貫無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.sanLianKe, v => GuiZe.Instance.sanLianKe = v, new string[] { "三連刻有り", "三連刻無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.yanFan, v => GuiZe.Instance.yanFan = v, new string[] { "燕返し有り", "燕返し無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.kaiLiZhi, v => GuiZe.Instance.kaiLiZhi = v, new string[] { "オープンリーチ有り", "オープンリーチ無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.shiSanBuTa, v => GuiZe.Instance.shiSanBuTa = v, new string[] { "十三不塔有り", "十三不塔無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.baLianZhuang, v => GuiZe.Instance.baLianZhuang = v, new string[] { "八連荘有り", "八連荘無し" }, new Vector2(x, y -= offset));
            DrawToggleGuiZe(() => GuiZe.Instance.localYiMan, v => GuiZe.Instance.localYiMan = v, new string[] { "ローカル役満有り", "ローカル役満無し" }, new Vector2(x, y -= offset));

            Button resetButton = Instantiate(goButton, goGuiZeContent.transform);
            resetButton.onClick.AddListener(() => { goGuiZeDialogPanel.SetActive(true); });
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
                FileUtility.WriteGuiZeFile();
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
                FileUtility.WriteGuiZeFile();
            });
        }

        // 画面キャプチャー・フラッシュ
        public IEnumerator CaptureAndFlash()
        {
            yield return new WaitForEndOfFrame();

            string directory = FileUtility.GetDirectory("Screenshot");

            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string fileName = $"screenshot-{timestamp}.png";
            string filePath = Path.Combine(directory, fileName);
            ScreenCapture.CaptureScreenshot(filePath);

            StartCoroutine(FlashCoroutine());
        }

        // 画面フラッシュ
        public IEnumerator FlashCoroutine()
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

            yield return new WaitForSeconds(0.1f);

            float alpha = 1f;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime * 5f;
                img.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            Destroy(goFlash);
        }

        // 開始
        public void DrawKaiShi()
        {
            DrawText(ref goTitle, "麻 雀 部", new Vector2(0, paiHeight * 2), 0, 60);
            DrawText(ref goStart, "タップ スタート!", new Vector2(0, -(paiHeight * 2)), 0, 30);
        }
        public void DrawKaiShiStartAlpha(float alpha)
        {
            if (goStart != null)
            {
                goStart.canvasRenderer.SetAlpha(alpha);
            }
        }

        // 雀士選択
        public void DrawQueShiXuanZe()
        {
            ClearScreen();

            float x = 0;
            float y = paiHeight * 5f;

            DrawButton(ref goDuiJuMode, Chang.Instance.duiJuMode ? "対局" : "観戦", new Vector2(0, y), 4);
            goDuiJuMode.onClick.AddListener(MaQue.Instance.OnClickDuiJuMode);
            y -= paiHeight * 1.5f;

            int index = 0;
            int queShiButtonMaxLen = GameManager.allQueShis.Max(shi => shi.mingQian.Length);
            foreach (QueShi shi in GameManager.allQueShis)
            {
                x = paiWidth * 4 * (index % 2 == 0 ? -1 : 1);
                int pos = index;
                DrawButton(ref goQueShi[index], shi.mingQian, new Vector2(x, y), queShiButtonMaxLen);
                goQueShi[index].onClick.AddListener(() =>
                {
                    MaQue.Instance.OnClickQueShi(pos);
                });

                SetQueShiColor(index);
                if (index % 2 == 1 || index == GameManager.allQueShis.Count - 1)
                {
                    y -= paiHeight * 1.5f;
                }
                index++;
            }

            DrawButton(ref goRandom, "ランダム", new Vector2(0, y));
            goRandom.onClick.AddListener(() =>
            {
                MaQue.Instance.RandomQueShiXuanZe();
                index = 0;
                foreach (QueShi shi in GameManager.allQueShis)
                {
                    SetQueShiColor(index++);
                }
            });
        }

        // 名前色設定
        public void SetQueShiColor(int pos)
        {
            TextMeshProUGUI text = goQueShi[pos].GetComponentInChildren<TextMeshProUGUI>();
            text.color = GameManager.allQueShis[pos].selected ? Color.black : Color.gray;
        }

        // フォロー雀士選択
        public void DrawFollowQueShiXuanZe()
        {
            ClearScreen();

            DrawText(ref goJu, "フォロー雀士", new Vector2(0, paiHeight * 5f), 0, 25);

            float x = 0;
            float y = paiHeight * 3.5f;
            int i = 0;
            int queShiButtonMaxLen = GameManager.allQueShis.Max(shi => shi.mingQian.Length);
            foreach (QueShi shi in GameManager.allQueShis)
            {
                x = paiWidth * 4 * (i % 2 == 0 ? -1 : 1);
                int pos = i;
                DrawButton(ref goQueShi[i], shi.mingQian, new Vector2(x, y), queShiButtonMaxLen);
                goQueShi[i].onClick.AddListener(() => { MaQue.Instance.OnClickFollowQueShi(shi.mingQian); });

                if (i % 2 == 1)
                {
                    y -= paiHeight * 1.5f;
                }
                i++;
            }
        }

        // 親決
        public void DrawQinJue()
        {
            ClearScreen();
            DrawMingQian();

            DrawSais(0, MaQue.Instance.sai1, MaQue.Instance.sai2);
            DrawQiJia();
        }

        // 配牌
        public void DrawPeiPai()
        {
            ClearScreen();
            DrawJuFrame();
            DrawJuOption();
            DrawQiJia();
            DrawMingQian();

            for (int jia = 0; jia < MaQue.Instance.queShis.Count; jia++)
            {
                QueShi shi = MaQue.Instance.queShis[jia];
                DrawShouPai(jia, shi.ziJiaYao, shi.ziJiaXuanZe);
            }
        }

        // 対局
        public void DrawDuiJu()
        {
            ClearScreen();
            DrawJuFrame();
            DrawJuOption();
            DrawQiJia();
            DrawMingQian();

            for (int jia = 0; jia < MaQue.Instance.queShis.Count; jia++)
            {
                QueShi shi = MaQue.Instance.queShis[jia];
                // 手牌
                if (shi.player)
                {
                    if (shi.ziJiaYao == QueShi.YaoDingYi.JiuZhongJiuPai || shi.ziJiaYao == QueShi.YaoDingYi.ZiMo || shi.ziJiaYao == QueShi.YaoDingYi.LiZhi || shi.ziJiaYao == QueShi.YaoDingYi.KaiLiZhi || shi.ziJiaYao == QueShi.YaoDingYi.AnGang || shi.ziJiaYao == QueShi.YaoDingYi.JiaGang)
                    {
                        DrawShouPai(jia, QueShi.YaoDingYi.Wu, -1);
                    }
                    else
                    {
                        DrawShouPai(jia, shi.ziJiaYao, shi.ziJiaXuanZe, shi.follow);
                    }
                }
                else
                {
                    if (shi.ziJiaYao == QueShi.YaoDingYi.ZiMo || (shi.taJiaYao == QueShi.YaoDingYi.RongHe && !Chang.Instance.isTaJiaYaoDraw))
                    {
                        DrawShouPai(jia, QueShi.YaoDingYi.HeLe, -1);
                    }
                    else
                    {
                        DrawShouPai(jia, shi.ziJiaYao, shi.ziJiaXuanZe, shi.follow);
                    }
                }

                // 自家腰
                if (Chang.Instance.isZiJiaYaoDraw)
                {
                    DrawZiJiaYao(shi, 0, shi.shouPai.Count - 1, true, false);
                }
                // 他家腰
                if (Chang.Instance.isTaJiaYaoDraw)
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
                        DrawSheng(jia, QueShi.YaoDingYi.LiuManGuan);
                    }
                    else
                    {
                        DrawSheng(jia, shi.xingTing ? QueShi.YaoDingYi.TingPai : QueShi.YaoDingYi.BuTing);
                    }
                }
            }
            // 声
            DrawSheng(Chang.Instance.ziMoFan, Chang.Instance.ziJiaYao);
            DrawSheng(Chang.Instance.mingFan, Chang.Instance.taJiaYao);
            // 四家立直
            if (Chang.Instance.SiJiaLiZhiPanDing())
            {
                DrawSheng(Chang.Instance.ziMoFan, QueShi.YaoDingYi.SiJiaLiZhi);
            }
            // 四開槓
            if (Pai.Instance.SiKaiGangPanDing())
            {
                DrawSheng(Chang.Instance.ziMoFan, QueShi.YaoDingYi.SiJiaLiZhi);
            }
            // 四風子連打
            if (Chang.Instance.SiFengZiLianDaPanDing())
            {
                DrawSheng(Chang.Instance.ziMoFan, QueShi.YaoDingYi.SiFengZiLianDa);
            }
        }

        // 対局終了
        public void DrawDuiJuZhongLe()
        {
            ClearScreen();
            DrawDuiJu();
        }

        // 役表示
        public void DrawYiBiaoShi()
        {
            ClearScreen();
            DrawBackDuiJuZhongLe();
            DrawYi(Chang.Instance.yiBiaoShiFan);
        }

        // 点表示
        public void DrawDianBiaoShi()
        {
            ClearScreen();
            DrawJuFrame();
            DrawMingQian();

            float x = 0;
            float y = -(paiHeight * 4);
            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                QueShi shi = MaQue.Instance.queShis[i];
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

            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                QueShi shi = MaQue.Instance.queShis[i];
                DrawText(ref goDianBang[i], shi.shuBiao.ToString(), Cal(x, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 30);
            }
        }

        // 荘終了
        public void DrawZhuangZhongLe()
        {
            ClearScreen();
            DrawZhuangZhong();
        }

        // 局、残牌、供託、点、懸賞牌
        public void DrawJuFrame()
        {
            DrawJu();
            DrawGongTuo();
            if (Chang.Instance.eventStatus != GameManager.Event.DIAN_BIAO_SHI)
            {
                DrawCanShanPaiShu();
                DrawXuanShangPai(false);
            }
            DrawDianBang();
        }

        // 名前(4人分)
        public void DrawMingQian()
        {
            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % MaQue.Instance.queShis.Count;
                DrawMingQian(jia);
            }
        }

        // 名前
        private void DrawMingQian(int jia)
        {
            QueShi shi = MaQue.Instance.queShis[jia];
            float x;
            float y;
            switch (Chang.Instance.eventStatus)
            {
                case GameManager.Event.PEI_PAI:
                case GameManager.Event.DUI_JU:
                case GameManager.Event.DUI_JU_ZHONG_LE:
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

        // サイコロ(2個分)
        private void DrawSais(int jia, int mu1, int mu2)
        {
            float margin = paiWidth / 3;
            DrawSai(ref goSai1, jia, mu1, -margin);
            DrawSai(ref goSai2, jia, mu2, margin);
        }

        // サイコロ
        private void DrawSai(ref Image go, int jia, int mu, float margin)
        {
            ClearGameObject(ref go);
            go = Instantiate(goSais[mu - 1].GetComponent<Image>(), goCanvas.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            QueShi shi = MaQue.Instance.queShis[jia];
            go.transform.Rotate(0, 0, 90 * shi.playOrder);
            float x = 0;
            float y = -(paiWidth * 1.2f);
            rt.anchoredPosition = Cal(x + margin, y, shi.playOrder);
        }

        // 起家
        private void DrawQiJia()
        {
            float x, y;
            switch (Chang.Instance.eventStatus)
            {
                case GameManager.Event.QIN_JUE:
                    x = 0;
                    y = -(paiWidth * 2.5f + paiHeight * 2f);
                    break;
                default:
                    x = paiWidth * 7.2f;
                    y = -(paiHeight * 4.9f);
                    break;
            }
            if (Chang.Instance.changFeng > 0x30)
            {
                for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
                {
                    int jia = (Chang.Instance.qin + i) % MaQue.Instance.queShis.Count;
                    if (Chang.Instance.qiaJia == jia)
                    {
                        ClearGameObject(ref goQiJia);
                        QueShi shi = MaQue.Instance.queShis[jia];
                        if (shi.player && GameManager.orientation != ScreenOrientation.Portrait)
                        {
                            x -= paiWidth * 1.2f;
                            y += paiHeight * 0.6f;
                        }
                        if (shi.playOrder == 3 && GameManager.orientation != ScreenOrientation.Portrait)
                        {
                            x -= paiHeight * 0.6f;
                        }
                        goQiJia = Instantiate(goQiJias[Chang.Instance.changFeng - 0x31].GetComponent<Image>(), goCanvas.transform);
                        goQiJia.transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                        RectTransform rt = goQiJia.GetComponent<RectTransform>();
                        rt.anchoredPosition = Cal(Chang.Instance.eventStatus == GameManager.Event.QIN_JUE ? 0 : x, y, shi.playOrder);
                    }
                }
            }
        }
        // 対局中オプション
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

            if (!Chang.Instance.duiJuMode)
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
                    SheDing.Instance.mingWu = !SheDing.Instance.mingWu;
                    goMingWu.GetComponentInChildren<TextMeshProUGUI>().text = SheDing.Instance.mingWu ? labelMingWu[0] : labelMingWu[1];
                    FileUtility.WriteSheDingFile();
                });
            }
            float x = paiWidth * 8f;
            float y = -(paiHeight * 9.3f);
            if (GameManager.orientation != ScreenOrientation.Portrait)
            {
                x = paiWidth * 13f;
                y = -(paiHeight * 0.5f);
            }
            goMingWu.gameObject.SetActive(true);
            DrawButton(ref goMingWu, SheDing.Instance.mingWu ? labelMingWu[0] : labelMingWu[1], new Vector2(x, y));

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
                    Chang.Instance.isDianCha = true;
                    Chang.Instance.isDianChaDraw = true;
                });
                trigger.triggers.Add(pointerDownEntry);

                EventTrigger.Entry pointerUpEntry = new()
                {
                    eventID = EventTriggerType.PointerUp
                };
                pointerUpEntry.callback.AddListener((data) =>
                {
                    Chang.Instance.isDianCha = false;
                });
                trigger.triggers.Add(pointerUpEntry);
            }

            if (GameManager.orientation == ScreenOrientation.Portrait)
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

        // 局
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
            string value = $"{Pai.FENG_PAI_MING[Chang.Instance.changFeng - 0x31]}{Chang.Instance.ju + 1}局";
            DrawText(ref goJu, value, new Vector2(x, y), 0, 17);
            goJu.rectTransform.SetSiblingIndex(1);
        }

        // 供託
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
            string valueBenChang = $"x{Chang.Instance.benChang}";
            DrawText(ref goBenChangText, valueBenChang, new Vector2(x + paiWidth * 0.9f, y + paiHeight * 0.05f), 0, 12);

            y -= paiHeight * 0.3f;
            ClearGameObject(ref goGongTou);
            goGongTou = Instantiate(goDianBang1000, goCanvas.transform);
            RectTransform rt1000 = goGongTou.GetComponent<RectTransform>();
            rt1000.sizeDelta = new Vector2(rt1000.sizeDelta.x * 0.4f, rt1000.sizeDelta.y);
            rt1000.anchoredPosition = new Vector2(x, y);
            ClearGameObject(ref goGongTouText);
            goGongTouText = Instantiate(goText, goJuFrame.transform.parent);
            string valueGongTou = $"x{Chang.Instance.gongTuo / 1000}";
            DrawText(ref goGongTouText, valueGongTou, new Vector2(x + paiWidth * 0.9f, y + paiHeight * 0.05f), 0, 12);
        }

        /**
         * 残山牌数
         */
        private void DrawCanShanPaiShu()
        {
            ClearGameObject(ref goCanShanPaiShu);
            goCanShanPaiShu = Instantiate(goFrame, goJuFrame.transform.parent);
            float alfa = Pai.Instance.CanShanPaiShu() < 100 ? 1f : 0f;
            DrawFrame(ref goCanShanPaiShu, Pai.Instance.CanShanPaiShu().ToString(), new Vector2(0, paiHeight * 0.2f), 0, 17, new Color(0, 0.6f, 0), new Color(1f, 1f, 1f, alfa), 3);
        }

        // 点数
        private void DrawDianBang()
        {
            int dianPlayer = 0;
            if (Chang.Instance.isDianCha)
            {
                for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
                {
                    int jia = (Chang.Instance.qin + i) % MaQue.Instance.queShis.Count;
                    QueShi shi = MaQue.Instance.queShis[jia];
                    if (shi.player)
                    {
                        dianPlayer = shi.dianBang;
                        break;
                    }
                }
            }

            float x = 0f;
            float y = -(paiWidth * 2.5f);
            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % MaQue.Instance.queShis.Count;
                QueShi shi = MaQue.Instance.queShis[jia];
                Color background = shi.feng == 0x31 ? new Color(1f, 0.5f, 0.5f) : Color.black;
                ClearGameObject(ref goFeng[i]);
                goFeng[i] = Instantiate(goFrame, goJuFrame.transform.parent);
                if ((Chang.Instance.eventStatus == GameManager.Event.DUI_JU || Chang.Instance.eventStatus == GameManager.Event.DUI_JU_ZHONG_LE) && jia == Chang.Instance.ziMoFan)
                {
                    ClearGameObject(ref goZiMoShiLine);
                    goZiMoShiLine = Instantiate(goLine, goJuFrame.transform.parent);
                    RectTransform rt = goZiMoShiLine.rectTransform;
                    rt.anchoredPosition = Cal(0, -(paiHeight * 2f), shi.playOrder);
                    rt.rotation = Quaternion.Euler(0, 0, 90 * GetDrawOrder(shi.playOrder));
                }
                DrawFrame(ref goFeng[i], Pai.FENG_PAI_MING[shi.feng - 0x31], Cal(x - paiWidth * 2f, y, shi.playOrder), 90 * GetDrawOrder(shi.playOrder), 16, background, Color.white);
                if (Chang.Instance.eventStatus == GameManager.Event.DIAN_BIAO_SHI)
                {
                    continue;
                }
                ClearGameObject(ref goDianBang[i]);
                goDianBang[i] = Instantiate(goText, goJuFrame.transform.parent);
                string value;
                if (Chang.Instance.isDianCha && !shi.player)
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

        // 懸賞牌
        private void DrawXuanShangPai(bool isYiBiaoShi)
        {
            ClearGameObject(ref Pai.Instance.goXuanShangPai);
            ClearGameObject(ref Pai.Instance.goLiXuanShangPai);

            float xuanShangScalse = 0.7f;
            float X = -(paiWidth * 1.4f);
            float Y = -(paiHeight * 0.6f);
            if (isYiBiaoShi)
            {
                X = -(paiWidth * 2f);
                Y = paiHeight * 8f;
                if (GameManager.orientation != ScreenOrientation.Portrait)
                {
                    X = -(paiWidth * 15f);
                    Y = paiHeight * 2f;
                }
                xuanShangScalse = 1.0f;
            }
            float offsetY = paiHeight * xuanShangScalse * 0.5f;

            bool isLiXuanShangPai = false;
            if (Chang.Instance.heleFan >= 0 && MaQue.Instance.queShis[Chang.Instance.heleFan].liZhi)
            {
                isLiXuanShangPai = true;
            }
            foreach (RongHeFan rongHeFan in Chang.Instance.rongHeFans)
            {
                QueShi shi = MaQue.Instance.queShis[rongHeFan.fan];
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
                Pai.Instance.goXuanShangPai[i] = Instantiate(goPai, goCanvas.transform);
                Pai.Instance.goXuanShangPai[i].transform.SetSiblingIndex(3);
                RectTransform rt = Pai.Instance.goXuanShangPai[i].GetComponent<RectTransform>();
                rt.localScale *= xuanShangScalse;
                if (i < Pai.Instance.xuanShangPai.Count)
                {
                    DrawPai(ref Pai.Instance.goXuanShangPai[i], Pai.Instance.xuanShangPai[i], new Vector2(x, isLiXuanShangPai ? y + offsetY : y), 0);
                }
                else
                {
                    DrawPai(ref Pai.Instance.goXuanShangPai[i], 0x00, new Vector2(x, y), 0);
                }
                x += paiWidth * xuanShangScalse;
            }

            if (isLiXuanShangPai)
            {
                x = X;
                y -= paiHeight * xuanShangScalse;
                for (int i = 0; i <= 4; i++)
                {
                    if (i < Pai.Instance.xuanShangPai.Count)
                    {
                        Pai.Instance.goLiXuanShangPai[i] = Instantiate(goPai, goCanvas.transform);
                        Pai.Instance.goLiXuanShangPai[i].transform.SetSiblingIndex(3);
                        RectTransform rt = Pai.Instance.goLiXuanShangPai[i].GetComponent<RectTransform>();
                        rt.localScale *= xuanShangScalse;
                        DrawPai(ref Pai.Instance.goLiXuanShangPai[i], Pai.Instance.liXuanShangPai[i], new Vector2(x, y + offsetY), 0);
                    }
                    x += paiWidth * xuanShangScalse;
                }
            }
        }

        // 手牌
        public void DrawShouPai(int jia, QueShi.YaoDingYi yao, int mingWei, bool isFollow = false)
        {
            QueShi shi = MaQue.Instance.queShis[jia];
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
                if (GameManager.orientation != ScreenOrientation.Portrait)
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
                if (GameManager.orientation != ScreenOrientation.Portrait)
                {
                    ph *= PLAYER_PAI_SCALE_LANDSCAPE;
                }
            }

            // 手牌
            float margin = pw / 4;
            int shouPaiCount = shi.shouPai.Count;
            for (int i = 0; i < Chang.Instance.rongHeFans.Count; i++)
            {
                if (Chang.Instance.rongHeFans[i].fan == jia)
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
                    if (GameManager.orientation != ScreenOrientation.Portrait)
                    {
                        shi.goShouPai[i].transform.localScale *= PLAYER_PAI_SCALE_LANDSCAPE;
                    }

                    int wei = i;
                    if (yao == QueShi.YaoDingYi.LiZhi || yao == QueShi.YaoDingYi.KaiLiZhi)
                    {
                        if (shi.liZhiPaiWei[mingWei] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, yao, wei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QueShi.YaoDingYi.AnGang)
                    {
                        if (shi.anGangPaiWei[mingWei][0] == i || shi.anGangPaiWei[mingWei][1] == i || shi.anGangPaiWei[mingWei][2] == i || shi.anGangPaiWei[mingWei][3] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QueShi.YaoDingYi.JiaGang)
                    {
                        if (shi.jiaGangPaiWei[mingWei][0] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QueShi.YaoDingYi.Bing)
                    {
                        if (shi.bingPaiWei[mingWei][0] == i || shi.bingPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QueShi.YaoDingYi.Chi)
                    {
                        if (shi.chiPaiWei[mingWei][0] == i || shi.chiPaiWei[mingWei][1] == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, yao, mingWei); });
                            yy = y + margin;
                        }
                    }
                    if (yao == QueShi.YaoDingYi.Wu && mingWei >= -1)
                    {
                        if (isFollow && mingWei == i && Chang.Instance.isZiJiaYaoDraw && !shi.daPaiHou)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, QueShi.YaoDingYi.DaPai, wei); });
                            yy = y + margin;
                        }
                        else if (!shi.liZhi || shi.shouPai.Count - 1 == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, yao, wei); });
                        }
                    }
                    if (yao == QueShi.YaoDingYi.Select)
                    {
                        if (mingWei == i)
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, QueShi.YaoDingYi.DaPai, wei); });
                            yy = y + margin;
                        }
                        else
                        {
                            shi.goShouPai[i].onClick.AddListener(() => { MaQue.Instance.OnClickShouPai(jia, QueShi.YaoDingYi.Wu, wei); });
                        }
                    }
                    if (shi.shouPai.Count - 1 == i && !shi.daPaiHou)
                    {
                        x += pw / 5;
                    }
                    if (SheDing.Instance.xuanShangYin)
                    {
                        shi.goShouPai[i].GetComponentInChildren<TextMeshProUGUI>().text = (shi.XuanShangPaiPanDing(shi.shouPai[i]) > 0) ? "▼" : "";
                    }
                    if (SheDing.Instance.shouPaiDianBiaoShi && Chang.Instance.isZiJiaYaoDraw && !shi.daPaiHou)
                    {
                        shi.goShouPai[i].GetComponentInChildren<TextMeshProUGUI>().text = shi.shouPaiDian[i].ToString();
                    }
                }
                else
                {
                    if (!(shi.zhongLiao && shi.xingTing) && yao != QueShi.YaoDingYi.HeLe && yao != QueShi.YaoDingYi.JiuZhongJiuPai && yao != QueShi.YaoDingYi.CuHe && p != 0xff)
                    {
                        if (!SheDing.Instance.xiangShouPaiOpen && !shi.kaiLiZhi)
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
                        if (GameManager.orientation != ScreenOrientation.Portrait && shi.player)
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
                if (GameManager.orientation != ScreenOrientation.Portrait)
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
                    if (fuLuPai.yao == QueShi.YaoDingYi.JiaGang && j == 3)
                    {
                        continue;
                    }
                    bool isMingPai = shi.MingPaiPanDing(fuLuPai.yao, fuLuPai.jia, j);
                    if (fuLuPai.yao == QueShi.YaoDingYi.AnGang && (j == 0 || j == 3))
                    {
                        p = 0x00;
                    }
                    shi.goFuLuPais[i].goFuLuPai[j] = Instantiate(goPai, goCanvas.transform);
                    shi.goFuLuPais[i].goFuLuPai[j].transform.SetSiblingIndex(2);
                    shi.goFuLuPais[i].goFuLuPai[j].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                    if (fuLuPai.yao == QueShi.YaoDingYi.JiaGang && isMingPai)
                    {
                        shi.goFuLuPais[i].goFuLuPai[3] = Instantiate(goPai, goCanvas.transform);
                        shi.goFuLuPais[i].goFuLuPai[3].transform.Rotate(0, 0, 90 * GetDrawOrder(shi.playOrder));
                        shi.goFuLuPais[i].goFuLuPai[3].transform.SetSiblingIndex(0);
                    }
                    if (isMingPai)
                    {
                        x -= paiHeight / 2;
                        DrawPai(ref shi.goFuLuPais[i].goFuLuPai[j], p, Cal(x, y - (paiHeight - paiWidth) / 2, shi.playOrder), 90);
                        if (fuLuPai.yao == QueShi.YaoDingYi.JiaGang)
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

        // 自家腰
        public void DrawZiJiaYao(QueShi shi, int mingWei, int ShouPaiWei, bool isFollow, bool isPass)
        {
            if (!shi.player)
            {
                return;
            }

            ClearGameObject(ref goYao);

            float width = paiWidth * 3.5f;
            float x = -(paiWidth * 7.5f);
            float y = -(paiHeight * 4.5f);
            if (GameManager.orientation != ScreenOrientation.Portrait)
            {
                y += paiHeight * 0.5f;
            }

            int index = 0;
            if (shi.heLe && shi.taJiaYao == QueShi.YaoDingYi.Wu)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QueShi.YaoDingYi.ZiMo, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (!shi.liZhi && shi.liZhiPaiWei.Count > 0)
            {
                int wei = mingWei;
                if (isFollow && shi.ziJiaYao == QueShi.YaoDingYi.LiZhi)
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
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QueShi.YaoDingYi.LiZhi, wei, ShouPaiWei, isFollow);
                x += width;
                index++;
                if (GuiZe.Instance.kaiLiZhi)
                {
                    DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QueShi.YaoDingYi.KaiLiZhi, wei, ShouPaiWei, isFollow);
                    x += width;
                    index++;
                }
            }
            if (shi.anGangPaiWei.Count > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QueShi.YaoDingYi.AnGang, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.jiaGangPaiWei.Count > 0)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QueShi.YaoDingYi.JiaGang, mingWei, ShouPaiWei, isFollow);
                x += width;
                index++;
            }
            if (shi.jiuZhongJiuPai)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(x, y), QueShi.YaoDingYi.JiuZhongJiuPai, mingWei, ShouPaiWei, isFollow);
            }
            if (isPass)
            {
                DrawOnClickZiJiaYao(ref goYao[index], shi, new Vector2(paiWidth * 7.5f, y), QueShi.YaoDingYi.Wu, mingWei, ShouPaiWei, isFollow);
            }
        }

        // 自家腰
        private void DrawOnClickZiJiaYao(ref Button go, QueShi shi, Vector2 xy, QueShi.YaoDingYi yao, int mingWei, int ShouPaiWei, bool isFollow)
        {
            go = Instantiate(goButton, goCanvas.transform);
            go.onClick.AddListener(() =>
            {
                MaQue.Instance.OnClickZiJiaYao(Chang.Instance.ziMoFan, shi, yao, mingWei, ShouPaiWei, isFollow);
            });
            string value = shi.YaoMingButton(yao);
            if (yao == QueShi.YaoDingYi.Wu)
            {
                value = shi.YaoMingButton(QueShi.YaoDingYi.Clear);
            }
            if (shi.follow && (shi.ziJiaYao != yao || shi.ziJiaYao == QueShi.YaoDingYi.Wu))
            {
                TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.gray;
            }
            DrawButton(ref go, value, xy, value.Length);
        }

        // 他家腰
        public void DrawTaJiaYao(int jia, QueShi shi, int mingWei, bool isFollow)
        {
            if (!shi.player)
            {
                return;
            }
            ClearGameObject(ref goYao);

            float width = paiWidth * 4;
            float x = -(paiWidth * 7.5f);
            float y = -(paiHeight * 4.5f);
            if (GameManager.orientation != ScreenOrientation.Portrait)
            {
                y += paiHeight * 0.5f;
            }

            int index = 0;
            if (shi.heLe)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QueShi.YaoDingYi.RongHe, mingWei);
                x += width;
                index++;
            }
            if (shi.chiPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QueShi.YaoDingYi.Chi, isFollow && shi.taJiaYao == QueShi.YaoDingYi.Chi ? shi.taJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.bingPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QueShi.YaoDingYi.Bing, isFollow && shi.taJiaYao == QueShi.YaoDingYi.Bing ? shi.taJiaXuanZe : mingWei);
                x += width;
                index++;
            }
            if (shi.daMingGangPaiWei.Count > 0)
            {
                DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(x, y), QueShi.YaoDingYi.DaMingGang, mingWei);
                index++;
            }
            if (index > 0)
            {
                if (SheDing.Instance.mingQuXiao)
                {
                    DrawOnClickTaJiaYao(ref goYao[index], jia, shi, new Vector2(paiWidth * 7.5f, y), QueShi.YaoDingYi.Wu, mingWei);
                }
            }
        }

        // 他家腰
        private void DrawOnClickTaJiaYao(ref Button go, int jia, QueShi shi, Vector2 xy, QueShi.YaoDingYi yao, int mingWei)
        {
            go = Instantiate(goButton, goCanvas.transform);
            go.onClick.AddListener(() =>
            {
                MaQue.Instance.OnClickTaJiaYao(jia, shi, yao, mingWei);
            });
            string value = shi.YaoMingButton(yao);
            if (shi.follow && shi.taJiaYao != yao)
            {
                TextMeshProUGUI text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.gray;
            }
            DrawButton(ref go, value, xy);
        }

        // 待牌
        public void DrawDaiPai(int jia)
        {
            QueShi shi = MaQue.Instance.queShis[jia];
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
            if (GameManager.orientation != ScreenOrientation.Portrait)
            {
                x = paiWidth * 11f;
                y = -(paiHeight * 4f);
            }

            if (SheDing.Instance.daiPaiBiaoShi)
            {
                for (int i = 0; i < shi.daiPai.Count; i++)
                {
                    int p = shi.daiPai[i] & QueShi.QUE_PAI;
                    if (p == 0xff)
                    {
                        ClearGameObject(ref shi.goDaiPai[i]);
                    }
                    else
                    {
                        DrawPai(ref shi.goDaiPai[i], p, Cal(x, y, shi.playOrder), 0);
                        DrawText(ref shi.goCanPaiShu[i], Pai.Instance.CanShu(shi.gongKaiPaiShu[p]).ToString(), Cal(x, y + paiWidth * 1.2f, shi.playOrder), 0, 17);
                    }
                    x += paiWidth;
                }
            }

            if (SheDing.Instance.xiangTingShuBiaoShi)
            {
                // 向聴数計算
                x = -(paiWidth * 7f);
                if (GameManager.orientation != ScreenOrientation.Portrait)
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

        // 捨牌
        private void DrawShePai(int jia)
        {
            QueShi shi = MaQue.Instance.queShis[jia];

            ClearGameObject(ref shi.goShePai);
            int shePaiEnter = 6;
            float shePaiLeft = 2.5f;
            if (MaQue.Instance.queShis.Count == 2)
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
            if (MaQue.Instance.queShis.Count == 2 && playOrder == 1)
            {
                playOrder = 2;
            }
            int index = 0;
            foreach (ShePai shePai in shi.shePais)
            {
                if (shePai.yao == QueShi.YaoDingYi.Wu || shePai.yao == QueShi.YaoDingYi.LiZhi || shePai.yao == QueShi.YaoDingYi.RongHe)
                {
                    shu++;

                    shi.goShePai[index] = Instantiate(goPai, goCanvas.transform);
                    shi.goShePai[index].transform.SetSiblingIndex(3);
                    if (shePai.yao == QueShi.YaoDingYi.LiZhi || (!isDrawLizhi && shi.liZhiWei < index))
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
                    if (SheDing.Instance.ziMoQieBiaoShi && shePai.ziMoQie)
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

        // 声
        private void DrawSheng(int jia, QueShi.YaoDingYi yao)
        {
            if (yao == QueShi.YaoDingYi.Wu || ((yao == QueShi.YaoDingYi.Chi || yao == QueShi.YaoDingYi.Bing || yao == QueShi.YaoDingYi.DaMingGang || yao == QueShi.YaoDingYi.RongHe) && Chang.Instance.isTaJiaYaoDraw))
            {
                return;
            }

            // 栄和
            if (yao == QueShi.YaoDingYi.RongHe)
            {
                foreach (RongHeFan rongHeFan in Chang.Instance.rongHeFans)
                {
                    DrawSheng(rongHeFan.fan, QueShi.YaoMing(QueShi.YaoDingYi.RongHe));
                }
                if (Chang.Instance.rongHeFans.Count == 3)
                {
                    if (GuiZe.Instance.tRongHe == 0)
                    {
                        DrawSheng(Chang.Instance.mingFan, "ロン(頭ハネ)");
                    }
                    else if (GuiZe.Instance.tRongHe >= 2)
                    {
                        DrawSheng(Chang.Instance.rongHeFans[2].fan, "ロン(流局)");
                    }
                }
                else if (Chang.Instance.rongHeFans.Count == 2)
                {
                    if (GuiZe.Instance.wRongHe == 0)
                    {
                        DrawSheng(Chang.Instance.mingFan, "ロン(頭ハネ)");
                    }
                }
                return;
            }

            string text = QueShi.YaoMing(yao);
            QueShi shi = MaQue.Instance.queShis[jia];
            if (yao == QueShi.YaoDingYi.LiZhi && shi.kaiLiZhi)
            {
                text = QueShi.YaoMing(QueShi.YaoDingYi.KaiLiZhi);
            }
            DrawSheng(jia, text);
        }
        private void DrawSheng(int jia, string text)
        {
            QueShi shi = MaQue.Instance.queShis[jia];
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

        // 錯和
        private void DrawCuHe(int jia)
        {
            DrawSheng(jia, $"{QueShi.YaoMing(QueShi.YaoDingYi.CuHe)} {MaQue.Instance.queShis[Chang.Instance.cuHeFan].cuHeSheng}");
            DrawShouPai(jia, QueShi.YaoDingYi.CuHe, 0);
        }

        // 対局終了へ戻る
        private void DrawBackDuiJuZhongLe()
        {
            // 戻るボタン
            goBackDuiJuZhongLe = Instantiate(goBack, goCanvas.transform);
            goBackDuiJuZhongLe.onClick.AddListener(() =>
            {
                Chang.Instance.isBackDuiJuZhongLe = true;
                Chang.Instance.keyPress = true;
            });
            RectTransform rtBackDuiJuZhongLe = goBackDuiJuZhongLe.GetComponent<RectTransform>();
            rtBackDuiJuZhongLe.localScale *= scale.x;
            rtBackDuiJuZhongLe.anchorMin = new Vector2(0, 1);
            rtBackDuiJuZhongLe.anchorMax = new Vector2(0, 1);
            rtBackDuiJuZhongLe.pivot = new Vector2(0, 1);
            rtBackDuiJuZhongLe.anchoredPosition = new Vector2(paiWidth * 0.5f, -(paiHeight * 1.5f));
        }

        // 役
        private void DrawYi(int jia)
        {
            float x;
            float y = paiWidth * 2.5f + paiHeight * 7.5f;
            if (jia >= 0)
            {
                DrawXuanShangPai(true);

                QueShi shi = MaQue.Instance.queShis[jia];

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
                        if (fuLuPai.yao == QueShi.YaoDingYi.JiaGang && j == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(fuLuPai.yao, fuLuPai.jia, j);
                        if (fuLuPai.yao == QueShi.YaoDingYi.AnGang && (j == 0 || j == 3))
                        {
                            p = 0x00;
                        }
                        shi.goFuLuPais[i].goFuLuPai[j] = Instantiate(goPai, goCanvas.transform);
                        shi.goFuLuPais[i].goFuLuPai[j].transform.localScale *= PLAYER_PAI_SCALE;
                        if (fuLuPai.yao == QueShi.YaoDingYi.JiaGang && isMingPai)
                        {
                            shi.goFuLuPais[i].goFuLuPai[3] = Instantiate(goPai, goCanvas.transform);
                            shi.goFuLuPais[i].goFuLuPai[3].transform.localScale *= PLAYER_PAI_SCALE;
                        }
                        if (isMingPai)
                        {
                            x -= ph / 2;
                            DrawPai(ref shi.goFuLuPais[i].goFuLuPai[j], p, new Vector2(x, y - (ph - pw) / 2), 90);
                            if (fuLuPai.yao == QueShi.YaoDingYi.JiaGang)
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

                string hele = shi.fanShuJi >= 13 ? QueShi.DeDianYi[13] : QueShi.DeDianYi[shi.fanShuJi];
                if (hele == "")
                {
                    if ((shi.fu >= 40 && shi.fanShuJi >= 4) || (shi.fu >= 70 && shi.fanShuJi >= 3))
                    {
                        hele = QueShi.DeDianYi[5];
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
                    string ming = shi.yiMan ? QueShi.YiManMing[(QueShi.YiManDingYi)yiFan.yi] : QueShi.YiMing[(QueShi.YiDingYi)yiFan.yi];
                    DrawText(ref goYi[index], ming, new Vector2(0, y), 0, 25, TextAlignmentOptions.Left, 7);
                    goFanShu[index] = Instantiate(goText, goCanvas.transform);
                    DrawText(ref goFanShu[index], yiFan.fanShu.ToString(), new Vector2(paiWidth * 3.3f, y), 0, 25, TextAlignmentOptions.Right, 2);
                    index++;
                }
            }
        }

        // 荘終了
        private void DrawZhuangZhong()
        {
            float y = paiHeight * 3f;
            int maxMingQian = 0;
            int maxDianBang = 0;
            int maxDeDian = 0;
            foreach (QueShi shi in MaQue.Instance.queShis)
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

            List<(int dian, QueShi shi)> shunWei = new();
            for (int i = Chang.Instance.qiaJia; i < Chang.Instance.qiaJia + MaQue.Instance.queShis.Count; i++)
            {
                int jia = i % MaQue.Instance.queShis.Count;
                QueShi shi = MaQue.Instance.queShis[jia];
                shunWei.Add((shi.dianBang, shi));
            }
            shunWei.Sort((x, y) => y.dian.CompareTo(x.dian));

            for (int i = 0; i < shunWei.Count; i++)
            {
                QueShi shi = shunWei[i].shi;
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

                int queShiButtonMaxLen = GameManager.allQueShis.Max(shi => shi.mingQian.Length);
                DrawText(ref goMingQian[i], shi.mingQian, new Vector2(-(paiWidth * 5f), y), 0, 30, TextAlignmentOptions.Left, queShiButtonMaxLen);
                DrawText(ref goDianBang[i], dianBang, new Vector2(paiWidth * 3f, y), 0, 30, TextAlignmentOptions.Right, 6);
                DrawText(ref goShouQu[i], deDian, new Vector2(paiWidth * 7f, y), 0, 25, TextAlignmentOptions.Right, 4);
                y -= paiHeight * 2;
            }
        }

        // テキスト
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

        // フレームテキスト
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

        // ボタン
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

        // 牌
        private void DrawPai(ref Button obj, int p, Vector2 xy, int quaternion)
        {
            if (obj == null)
            {
                obj = Instantiate(goPai, goCanvas.transform);
            }
            obj.image.sprite = Resources.Load<Sprite>($"0x{p:x02}");
            obj.transform.Rotate(0, 0, quaternion);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = xy;
        }

        // 相手(機械)牌
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
            if (MaQue.Instance.queShis.Count == 3 && order == 2)
            {
                return 3;
            }
            if (MaQue.Instance.queShis.Count == 2 && order == 1)
            {
                return 2;
            }
            return order;
        }

        // 画面クリア
        public void ClearScreen()
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

            ClearGameObject(ref goQueShi);
            ClearGameObject(ref goRandom);
            ClearGameObject(ref goDuiJuMode);

            ClearGameObject(ref Pai.Instance.goXuanShangPai);
            ClearGameObject(ref Pai.Instance.goLiXuanShangPai);

            foreach (QueShi shi in MaQue.Instance.queShis)
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

        public void ClearGameObject<T>(ref T go) where T : Component
        {
            if (go == null) return;
            Destroy(go.gameObject);
            go = null;
        }
        public void ClearGameObject<T>(ref T[] go) where T : Component
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
    }
}
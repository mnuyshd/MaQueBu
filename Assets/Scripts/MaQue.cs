using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Assets.Scripts.Gongtong;
using Assets.Scripts.Sikao;
using State = Assets.Scripts.Sikao.State;

namespace Assets.Scripts
{
    // 麻雀
    [DefaultExecutionOrder(2)]
    public class MaQue : MonoBehaviour
    {
        public static MaQue Instance;

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

        // 送りモード
        public enum ForwardMode
        {
            // 通常
            NORMAL = 0,
            // 局早送り
            FAST_FORWARD = 1,
            // ずっと早送り
            FOREVER_FAST_FORWARD = 2,
        }
        // 送りモード
        public ForwardMode forwardMode = ForwardMode.NORMAL;

        // 雀士
        public List<QueShi> queShis;
        // サイコロ
        public int sai1 = 1;
        public int sai2 = 1;

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
            string directory = Path.Combine(Application.persistentDataPath, FileUtility.GAME_DATA_DIR_NAME);
            // 雀士の読込
            queShis = new();
            for (int i = 0; i < 4; i++)
            {
                string queShiFilePath = Path.Combine(directory, $"{FileUtility.QUE_SHI_FILE_NAME}{i}.json");
                if (File.Exists(queShiFilePath))
                {
                    string jsonText = File.ReadAllText(queShiFilePath);
                    queShis.Add(GetQueShiFromJson(jsonText));
                }
            }
        }

        // ゲームリスタート
        public void RestartGame()
        {
            Application.targetFrameRate = GameManager.FRAME_RATE;
            GameManager.waitTime = GameManager.WAIT_TIME;
            FileUtility.DeleteAllGameDataFile();

            DestroyImmediate(Chang.Instance.gameObject);
            Chang.Instance = null;
            DestroyImmediate(Pai.Instance.gameObject);
            Pai.Instance = null;
            DestroyImmediate(GuiZe.Instance.gameObject);
            GuiZe.Instance = null;
            DestroyImmediate(SheDing.Instance.gameObject);
            SheDing.Instance = null;
            DestroyImmediate(Draw.Instance.gameObject);
            Draw.Instance = null;
            DestroyImmediate(MaQue.Instance.gameObject);
            MaQue.Instance = null;

            GameManager.coroutines.Clear();
            DestroyImmediate(FindFirstObjectByType<GameManager>().gameObject);
            SceneManager.LoadScene("GameScene");
        }

        // Jsonファイルから雀士取得
        private QueShi GetQueShiFromJson(string jsonText)
        {
            string mingQian = (string)JObject.Parse(jsonText)["mingQian"];
            QueShi newShi = null;
            foreach (QueShi shi in GameManager.allQueShis)
            {
                if (shi.mingQian == mingQian)
                {
                    newShi = shi.GetQueShi(jsonText);
                    break;
                }
            }
            newShi ??= JsonUtility.FromJson<QueJiXie>(jsonText);

            string jiLuFilePath = Path.Combine(Application.persistentDataPath, FileUtility.SETTING_PLAYER_DATA_DIR_NAME, $"{mingQian}.json");
            newShi.jiLu = JsonUtility.FromJson<JiLu>(File.ReadAllText(jiLuFilePath));

            return newShi;
        }

        // 雀士取得
        public QueShi GetQueShi(string mingQian, bool isNew)
        {
            if (!isNew)
            {
                foreach (QueShi shi in queShis)
                {
                    if (shi.mingQian == mingQian)
                    {
                        return shi;
                    }
                }
            }

            QueShi allShi = null;
            foreach (QueShi shi in GameManager.allQueShis)
            {
                if (shi.mingQian == mingQian)
                {
                    // インスタンスをコピー
                    allShi = (QueShi)JsonUtility.FromJson(JsonUtility.ToJson(shi), shi.GetType());
                    break;
                }
            }
            allShi ??= new QueJiXie(mingQian);
            allShi.jiLu = new();
            return allShi;
        }

        // 設定オプションのリセット
        public void ResetSheDing()
        {
            FileUtility.DeleteSheDingFile();
            DestroyImmediate(SheDing.Instance.gameObject);
            GameObject sheDing = new("SheDing");
            sheDing.AddComponent<SheDing>();
            Draw.Instance.DrawOption();
            switch (Chang.Instance.eventStatus)
            {
                case GameManager.Event.PEI_PAI:
                case GameManager.Event.DUI_JU:
                case GameManager.Event.DUI_JU_ZHONG_LE:
                    Chang.Instance.isDuiJuDraw = true;
                    break;
            }
        }

        // 全員の記録リセット
        public void ResetJiLu()
        {
            FileUtility.DeleteJiLuFile(Draw.Instance.PLAYER_NAME);
            foreach (QueShi shi in GameManager.allQueShis)
            {
                FileUtility.DeleteJiLuFile(shi.mingQian);
                Instance.Nao2JiLu(Instance.GetQueShi(shi.mingQian, true));
            }
        }

        // ルールリセット
        public void ResetGuiZe()
        {
            FileUtility.DeleteGuiZeFile();
            DestroyImmediate(GuiZe.Instance.gameObject);
            GameObject guiZe = new("GuiZe");
            guiZe.AddComponent<GuiZe>();
        }

        // スクリーンクリック
        public void OnClickScreen()
        {
            switch (Chang.Instance.eventStatus)
            {
                // 開始
                case GameManager.Event.KAI_SHI:
                // 場決
                case GameManager.Event.CHANG_JUE:
                // 親決
                case GameManager.Event.QIN_JUE:
                // 対局終了
                case GameManager.Event.DUI_JU_ZHONG_LE:
                // 役表示
                case GameManager.Event.YI_BIAO_SHI:
                // 点表示
                case GameManager.Event.DIAN_BIAO_SHI:
                // 荘終了
                case GameManager.Event.ZHUANG_ZHONG_LE:
                    Chang.Instance.keyPress = true;
                    break;

                // 雀士選択
                case GameManager.Event.QUE_SHI_XUAN_ZE:
                    OnClickScreenQueShiXuanZe();
                    break;

                // フォロー雀士選択
                case GameManager.Event.FOLLOW_QUE_SHI_XUAN_ZE:
                    OnClickScreenFollowNone();
                    break;

                // 対局
                case GameManager.Event.DUI_JU:
                    if (Chang.Instance.tingPaiLianZhuang != Zhuang.XU_HANG)
                    {
                        Chang.Instance.keyPress = true;
                    }
                    if (!SheDing.Instance.mingQuXiao)
                    {
                        for (int i = 0; i < queShis.Count; i++)
                        {
                            QueShi shi = queShis[i];
                            if (shi.player && !shi.jiJia)
                            {
                                OnClickTaJiaYao(i, shi, QueShi.YaoDingYi.Wu, 0);
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        // 早送りクリック
        public void OnClickFast()
        {
            forwardMode = (ForwardMode)(((int)forwardMode + 1) % Enum.GetValues(typeof(ForwardMode)).Length);
            if (forwardMode == ForwardMode.NORMAL)
            {
                forwardMode = ForwardMode.FAST_FORWARD;
            }
            Application.targetFrameRate = 0;
            GameManager.waitTime = 0;
            if (Chang.Instance.keyPress == false)
            {
                Chang.Instance.keyPress = true;
            }
            if (Chang.Instance.eventStatus == GameManager.Event.FOLLOW_QUE_SHI_XUAN_ZE)
            {
                OnClickScreenFollowNone();
            }
        }

        // 再生クリック
        public void OnClickReproduction()
        {
            forwardMode = ForwardMode.NORMAL;
            Application.targetFrameRate = GameManager.FRAME_RATE;
            GameManager.waitTime = GameManager.WAIT_TIME;
        }

        // 雀士選択処理
        private void OnClickScreenQueShiXuanZe()
        {
            int max = Chang.Instance.duiJuMode ? 3 : 4;
            int min = Chang.Instance.duiJuMode ? 1 : 2;
            int selectedCount = GameManager.allQueShis.Count(shi => shi.selected);
            if (selectedCount <= max && selectedCount >= min)
            {
                Chang.Instance.keyPress = true;
            }
        }

        // フォロー無しクリック
        public void OnClickScreenFollowNone()
        {
            queShis.Insert(0, new QueJiXie(Draw.Instance.PLAYER_NAME));
            queShis[0].follow = false;
            queShis[0].player = true;
            Chang.Instance.keyPress = true;
        }

        // 一時停止
        private IEnumerator Pause(ForwardMode mode)
        {
            Chang.Instance.keyPress = false;
            if (forwardMode > mode)
            {
                Chang.Instance.keyPress = true;
            }
            while (!Chang.Instance.keyPress)
            {
                yield return null;
            }
            Chang.Instance.keyPress = false;
        }

        // 開始
        public IEnumerator KaiShi()
        {
            Chang.Instance.isKaiShiDraw = true;

            int fadingOut = 1;
            float alpha = 0f;
            Chang.Instance.keyPress = false;
            while (!Chang.Instance.keyPress)
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
                Draw.Instance.DrawKaiShiStartAlpha(alpha);
                yield return null;
            }
            Chang.Instance.keyPress = false;

            Chang.Instance.eventStatus = GameManager.Event.QUE_SHI_XUAN_ZE;
            GameManager.coroutines[GameManager.Event.KAI_SHI] = null;
            FileUtility.WriteAllGameDataFile();
        }

        // 雀士選択
        public IEnumerator QueShiXuanZe()
        {
            queShis = new();
            if (forwardMode > ForwardMode.FAST_FORWARD)
            {
                // ランダム自動選択
                RandomQueShiXuanZe();
            }
            else
            {
                Chang.Instance.isQueShiXuanZeDraw = true;
            }
            yield return Pause(ForwardMode.FAST_FORWARD);

            int selectedCount = GameManager.allQueShis.Count(shi => shi.selected);
            if (selectedCount == 0)
            {
                RandomQueShiXuanZe();
                OnClickScreenQueShiXuanZe();
            }

            foreach (QueShi shi in GameManager.allQueShis)
            {
                if (shi.selected)
                {
                    queShis.Add(GetQueShi(shi.mingQian, true));
                }
            }

            Chang.Instance.eventStatus = Chang.Instance.duiJuMode ? GameManager.Event.FOLLOW_QUE_SHI_XUAN_ZE : GameManager.Event.CHANG_JUE;
            GameManager.coroutines[GameManager.Event.QUE_SHI_XUAN_ZE] = null;
            FileUtility.WriteAllGameDataFile();
        }

        // 対局モードクリック
        public void OnClickDuiJuMode()
        {
            Chang.Instance.duiJuMode = !Chang.Instance.duiJuMode;
            Draw.Instance.DrawQueShiXuanZe();

            int selectedCount = GameManager.allQueShis.Count(shi => shi.selected);
            int index = 0;
            if ((Chang.Instance.duiJuMode && selectedCount > 3) || (!Chang.Instance.duiJuMode && selectedCount > 4))
            {
                foreach (QueShi shi in GameManager.allQueShis)
                {
                    if (shi.selected)
                    {
                        index++;
                    }
                    if (index == selectedCount)
                    {
                        shi.selected = false;
                    }
                }
            }

            index = 0;
            foreach (QueShi shi in GameManager.allQueShis)
            {
                Draw.Instance.SetQueShiColor(index++);
            }
        }

        // ランダム雀士自動選択
        public void RandomQueShiXuanZe()
        {
            int mianz = 0;
            foreach (QueShi shi in GameManager.allQueShis)
            {
                if (shi.selected)
                {
                    mianz++;
                    shi.selected = false;
                }
            }
            int cnt = 0;
            System.Random r = new();
            while (cnt < mianz)
            {
                int n = r.Next(0, GameManager.allQueShis.Count);
                int i = 0;
                foreach (QueShi shi in GameManager.allQueShis)
                {
                    if (i == n)
                    {
                        if (!shi.selected)
                        {
                            shi.selected = true;
                            cnt++;
                            break;
                        }
                    }
                    i++;
                }
            }
        }

        // フォロー雀士選択
        public IEnumerator FollowQueShiXuanZe()
        {
            Chang.Instance.isFollowQueShiXuanZeDraw = true;
            Chang.Instance.keyPress = false;
            if (forwardMode > ForwardMode.NORMAL)
            {
                Chang.Instance.keyPress = true;
                OnClickScreenFollowNone();
            }
            while (!Chang.Instance.keyPress) { yield return null; }
            Chang.Instance.keyPress = false;

            Chang.Instance.eventStatus = GameManager.Event.CHANG_JUE;
            GameManager.coroutines[GameManager.Event.FOLLOW_QUE_SHI_XUAN_ZE] = null;
            FileUtility.WriteAllGameDataFile();
        }

        // 名前クリック
        public void OnClickQueShi(int pos)
        {
            bool selected = GameManager.allQueShis[pos].selected;
            if (!selected)
            {
                int selectedCount = 0;
                int firstPos = 0;
                bool isFirst = true;
                int lastPos = 0;
                int index = 0;
                int cnt = Chang.Instance.duiJuMode ? 2 : 3;
                foreach (QueShi shi in GameManager.allQueShis)
                {
                    if (shi.selected)
                    {
                        selectedCount++;
                        if (isFirst)
                        {
                            firstPos = index;
                            isFirst = false;
                        }
                        lastPos = index;
                    }
                    if (selectedCount > cnt)
                    {
                        int changePos = lastPos;
                        if (pos > lastPos)
                        {
                            changePos = firstPos;
                        }
                        GameManager.allQueShis[changePos].selected = false;
                        Draw.Instance.SetQueShiColor(changePos);
                        break;
                    }
                    index++;
                }
            }
            GameManager.allQueShis[pos].selected = !selected;
            Draw.Instance.SetQueShiColor(pos);
        }

        // 名前クリック
        public void OnClickFollowQueShi(string mingQian)
        {
            queShis.Insert(0, GetQueShi(mingQian, true));
            queShis[0].mingQian = Draw.Instance.PLAYER_NAME;
            queShis[0].follow = true;
            queShis[0].player = true;
            Chang.Instance.keyPress = true;
        }

        // 場決
        public void ChangJue()
        {
            Draw.Instance.ClearScreen();

            if (queShis[0] == null)
            {
                OnClickScreenFollowNone();
            }
            List<int> fengPai = new();
            for (int i = 0; i < Pai.FENG_PAI_DING_YI.Length - (4 - queShis.Count); i++)
            {
                fengPai.Add(Pai.FENG_PAI_DING_YI[i]);
            }
            Chang.Instance.Shuffle(fengPai, 20);

            Button[] goButton = new Button[queShis.Count];

            int idx = 0;
            for (int i = 0x31; i <= 0x34 - (4 - queShis.Count); i++)
            {
                for (int j = 0; j < fengPai.Count; j++)
                {
                    if (fengPai[j] == i)
                    {
                        (queShis[j], queShis[idx]) = (queShis[idx], queShis[j]);
                    }
                }
                idx++;
            }

            int order = 0;
            for (int i = 0; i < queShis.Count; i++)
            {
                QueShi shi = queShis[i];
                shi.playOrder = i;
                if (shi.player)
                {
                    order = i;
                }
            }
            foreach (QueShi shi in queShis)
            {
                shi.playOrder -= order;
                if (shi.playOrder < 0)
                {
                    shi.playOrder += queShis.Count;
                }
            }

            Draw.Instance.DrawMingQian();

            // 記録の読込
            foreach (QueShi shi in queShis)
            {
                string directory = Path.Combine(Application.persistentDataPath, FileUtility.SETTING_PLAYER_DATA_DIR_NAME);
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

            Draw.Instance.ClearGameObject(ref goButton);

            Chang.Instance.eventStatus = GameManager.Event.QIN_JUE;
            FileUtility.WriteAllGameDataFile();
        }

        // 記録を脳へ反映
        public void JiLu2Nao(QueShi shi)
        {
            if (shi.player && shi.follow)
            {
                return;
            }
            if (shi is QueJiXie qjx)
            {
                qjx.naos[(int)QueShi.XingGe.XUAN_SHANG].score = shi.jiLu.naoXuanShang;
                qjx.naos[(int)QueShi.XingGe.YI_PAI].score = shi.jiLu.naoYiPai;
                qjx.naos[(int)QueShi.XingGe.SHUN_ZI].score = shi.jiLu.naoShunZi;
                qjx.naos[(int)QueShi.XingGe.KE_ZI].score = shi.jiLu.naoKeZi;
                qjx.naos[(int)QueShi.XingGe.LI_ZHI].score = shi.jiLu.naoLiZhi;
                qjx.naos[(int)QueShi.XingGe.MING].score = shi.jiLu.naoMing;
                qjx.naos[(int)QueShi.XingGe.RAN].score = shi.jiLu.naoRan;
                qjx.naos[(int)QueShi.XingGe.TAO].score = shi.jiLu.naoTao;
            }
        }

        // 脳を記録へ反映
        public void Nao2JiLu(QueShi shi)
        {
            if (shi is QueJiXie qjx)
            {
                shi.jiLu.naoXuanShang = qjx.naos[(int)QueShi.XingGe.XUAN_SHANG].score;
                shi.jiLu.naoYiPai = qjx.naos[(int)QueShi.XingGe.YI_PAI].score;
                shi.jiLu.naoShunZi = qjx.naos[(int)QueShi.XingGe.SHUN_ZI].score;
                shi.jiLu.naoKeZi = qjx.naos[(int)QueShi.XingGe.KE_ZI].score;
                shi.jiLu.naoLiZhi = qjx.naos[(int)QueShi.XingGe.LI_ZHI].score;
                shi.jiLu.naoMing = qjx.naos[(int)QueShi.XingGe.MING].score;
                shi.jiLu.naoRan = qjx.naos[(int)QueShi.XingGe.RAN].score;
                shi.jiLu.naoTao = qjx.naos[(int)QueShi.XingGe.TAO].score;
            }
            FileUtility.WriteJiLuFile(shi.mingQian, shi.jiLu);
        }

        // 親決
        public IEnumerator QinJue()
        {
            System.Random r = new();
            if (forwardMode > ForwardMode.NORMAL)
            {
                Chang.Instance.keyPress = true;
            }
            for (int i = 0; i < 60; i++)
            {
                sai1 = r.Next(0, 6) + 1;
                sai2 = r.Next(0, 6) + 1;
                Chang.Instance.isQinJueDraw = true;
                if (Chang.Instance.keyPress)
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
            Chang.Instance.qin = (sai1 + sai2 - 1) % queShis.Count;
            // 起家
            Chang.Instance.qiaJia = Chang.Instance.qin;
            Chang.Instance.changFeng = 0x31;
            Chang.Instance.isQinJueDraw = true;

            yield return Pause(ForwardMode.NORMAL);

            Chang.Instance.eventStatus = GameManager.Event.ZHUANG_CHU_QI_HUA;
            GameManager.coroutines[GameManager.Event.QIN_JUE] = null;
            FileUtility.WriteAllGameDataFile();
        }


        // 荘初期化
        public void ZhuangChuQiHua()
        {
            Chang.Instance.ZhuangChuQiHua();
            foreach (QueShi shi in queShis)
            {
                shi.ZhuangChuQiHua();
            }

            Chang.Instance.eventStatus = GameManager.Event.PEI_PAI;
            FileUtility.WriteAllGameDataFile();
        }

        // 配牌
        public IEnumerator PeiPai()
        {
            // 局初期化
            JuChuQiHua();
            // 配牌
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < queShis.Count; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        queShis[(Chang.Instance.qin + j) % queShis.Count].ZiMo(Pai.Instance.ShanPaiZiMo(), true);
                    }
                    Chang.Instance.isPeiPaiDraw = true;
                    yield return new WaitForSeconds(GameManager.waitTime / 3);
                }
            }
            for (int i = 0; i < queShis.Count; i++)
            {
                queShis[(Chang.Instance.qin + i) % queShis.Count].ZiMo(Pai.Instance.ShanPaiZiMo(), true);
                Chang.Instance.isPeiPaiDraw = true;
            }
            yield return new WaitForSeconds(GameManager.waitTime / 3);
            for (int i = 0; i < queShis.Count; i++)
            {
                queShis[(Chang.Instance.qin + i) % queShis.Count].LiPai();
                Chang.Instance.isPeiPaiDraw = true;
            }

            Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
            GameManager.coroutines[GameManager.Event.PEI_PAI] = null;
            FileUtility.WriteAllGameDataFile();
        }


        // 局初期化
        private void JuChuQiHua()
        {
            // 局初期化
            Chang.Instance.JuChuQiHua();
            // 雀士初期化
            int w = 0;
            for (int i = 0; i < queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % queShis.Count;
                QueShi shi = queShis[jia];
                shi.JuChuQiHua(Pai.FENG_PAI_DING_YI[i]);

                if (shi.player)
                {
                    w = shi.feng - 0x31;
                }
            }

            // 洗牌
            Pai.Instance.XiPai();
            // 積込
            // List<List<int>> jiRuPai = new()
            // {
            //    new() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x31, 0x31, 0x31, 0x37 },
            //    new() { 0x37, 0x37 },
            //    new() { 0x37 },
            // };
            // pai.JiRu(w, jiRuPai);

            // 洗牌嶺上
            Pai.Instance.XiPaiLingShang();

            Chang.Instance.tingPaiLianZhuang = Zhuang.XU_HANG;

            Chang.Instance.eventStatus = GameManager.Event.PEI_PAI;
            GameManager.coroutines[GameManager.Event.DUI_JU] = null;
        }

        private State GetState(int jia, bool isZiJia)
        {
            // 状態
            State state = new();

            QueShi ziJiaShi = queShis[jia];
            ziJiaShi.ZhuangTai(state, true);
            for (int i = jia + 1; i < jia + queShis.Count; i++)
            {
                int j = i % queShis.Count;
                QueShi taJiaShi = queShis[j];
                taJiaShi.ZhuangTai(state, false);
            }
            Pai.Instance.ZhuangTai(state);
            Chang.Instance.ZhuangTai(state, isZiJia);

            return state;
        }

        // 対局
        public IEnumerator DuiJu()
        {
            Chang.Instance.isZiJiaYaoDraw = false;
            Chang.Instance.isTaJiaYaoDraw = false;

            QueShi.YaoDingYi changZiJiaYao = Chang.Instance.ziJiaYao;
            QueShi.YaoDingYi changTaJiaYao = Chang.Instance.taJiaYao;

            QueShi ziJiaShi = queShis[Chang.Instance.ziMoFan];
            if ((Chang.Instance.ziJiaYao == QueShi.YaoDingYi.Wu || Chang.Instance.ziJiaYao == QueShi.YaoDingYi.LiZhi) && Chang.Instance.taJiaYao == QueShi.YaoDingYi.Wu)
            {
                // 山牌自摸
                ziJiaShi.ZiMo(Pai.Instance.ShanPaiZiMo());
            }
            else if (Chang.Instance.ziJiaYao == QueShi.YaoDingYi.AnGang || Chang.Instance.ziJiaYao == QueShi.YaoDingYi.JiaGang || Chang.Instance.taJiaYao == QueShi.YaoDingYi.DaMingGang)
            {
                // 嶺上牌自摸
                ziJiaShi.ZiMo(Pai.Instance.LingShangPaiZiMo());
            }

            if (Chang.Instance.ziJiaYao == QueShi.YaoDingYi.AnGang || Chang.Instance.ziJiaYao == QueShi.YaoDingYi.JiaGang || Chang.Instance.taJiaYao == QueShi.YaoDingYi.DaMingGang || Chang.Instance.taJiaYao == QueShi.YaoDingYi.Chi || Chang.Instance.taJiaYao == QueShi.YaoDingYi.Bing)
            {
                if (!ziJiaShi.player)
                {
                    yield return new WaitForSeconds(GameManager.waitTime);
                }
                // 鳴処理
                ziJiaShi.MingChuLi();
                // 消
                foreach (QueShi shi in queShis)
                {
                    shi.Xiao();
                }
                // 四風子連打処理
                Chang.Instance.SiFengZiLianDaChuLi(0xff);
                Chang.Instance.isDuiJuDraw = true;
                yield return new WaitForSeconds(GameManager.waitTime);
            }

            Chang.Instance.ziJiaYao = QueShi.YaoDingYi.Wu;
            Chang.Instance.taJiaYao = QueShi.YaoDingYi.Wu;

            // 思考自家判定
            ziJiaShi.SiKaoZiJiaPanDing();

            yield return new WaitForSeconds(GameManager.waitTime);

            // 学習(自家 状態)
            ziJiaShi.SetTransitionZiJiaState(GetState(Chang.Instance.ziMoFan, true));

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
                if (!(SheDing.Instance.liZhiAuto && ziJiaShi.liZhi) || ziJiaShi.heLe || ziJiaShi.anGangPaiWei.Count > 0 || ziJiaShi.jiaGangPaiWei.Count > 0)
                {
                    if (ziJiaShi.follow && ziJiaShi.ziJiaYao == QueShi.YaoDingYi.Wu)
                    {
                        ziJiaShi.DaiPaiXiangTingShuJiSuan(ziJiaShi.ziJiaXuanZe);
                    }
                    Chang.Instance.isZiJiaYaoDraw = true;
                    Chang.Instance.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.NORMAL);
                }
            }
            Chang.Instance.isZiJiaYaoDraw = false;
            ziJiaShi.KaiLiZhiChuLi();
            Chang.Instance.ziJiaYao = ziJiaShi.ziJiaYao;
            Chang.Instance.ziJiaXuanZe = ziJiaShi.ziJiaXuanZe;

            // 学習(自家 行動)
            int paiOrIndex = ziJiaShi.ziJiaYao == QueShi.YaoDingYi.Wu ? ziJiaShi.shouPai[ziJiaShi.ziJiaXuanZe] & QueShi.QUE_PAI : ziJiaShi.ziJiaXuanZe;
            ziJiaShi.SetTransitionZiJiaAction(new() { (int)ziJiaShi.ziJiaYao, paiOrIndex });

            // 錯和自家判定
            if (ziJiaShi.CuHeZiJiaPanDing())
            {
                // 錯和
                Chang.Instance.CuHe(Chang.Instance.ziMoFan);
                Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                Chang.Instance.isDuiJuDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);
                Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                FileUtility.WriteAllGameDataFile();
                yield break;
            }

            switch (Chang.Instance.ziJiaYao)
            {
                case QueShi.YaoDingYi.ZiMo:
                    // 自摸
                    ziJiaShi.HeLeChuLi();
                    Chang.Instance.heleFan = Chang.Instance.ziMoFan;
                    // 学習(自家 次状態)
                    ziJiaShi.SetTransitionZiJiaNextState(GetState(Chang.Instance.ziMoFan, true));
                    Chang.Instance.tingPaiLianZhuang = Chang.Instance.ziMoFan == Chang.Instance.qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    Chang.Instance.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    Chang.Instance.eventStatus = GameManager.Event.YI_BIAO_SHI;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                case QueShi.YaoDingYi.JiuZhongJiuPai:
                    // 九種九牌
                    // 描画
                    Chang.Instance.JiuZhongJiuPaiChuLi();
                    // 学習(自家 次状態)
                    ziJiaShi.SetTransitionZiJiaNextState(GetState(Chang.Instance.ziMoFan, true));
                    Chang.Instance.tingPaiLianZhuang = GuiZe.Instance.jiuZhongJiuPaiLianZhuang == 1 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    Chang.Instance.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                case QueShi.YaoDingYi.LiZhi:
                    // 立直・開立直
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.Instance.LingShanChuLi();
                    // 立直
                    ziJiaShi.LiZhiChuLi();
                    break;
                case QueShi.YaoDingYi.AnGang:
                    // 暗槓
                    ziJiaShi.AnGang(Chang.Instance.ziJiaXuanZe);
                    // 嶺上牌処理
                    Pai.Instance.LingShangPaiChuLi();
                    // 四開槓判定
                    if (Pai.Instance.SiKaiGangPanDing())
                    {
                        Chang.Instance.tingPaiLianZhuang = GuiZe.Instance.siKaiGangLianZhuang == 0 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        Chang.Instance.isDuiJuDraw = true;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                        GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                        FileUtility.WriteAllGameDataFile();
                        yield break;
                    }
                    // 学習(自家 次状態)
                    ziJiaShi.SetTransitionZiJiaNextState(GetState(Chang.Instance.ziMoFan, true));
                    Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                case QueShi.YaoDingYi.JiaGang:
                    // 加槓
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.Instance.LingShanChuLi();
                    // 加槓
                    ziJiaShi.JiaGang(Chang.Instance.ziJiaXuanZe);
                    Pai.Instance.QiangGang();
                    break;
                default:
                    // 消
                    ziJiaShi.Xiao();
                    // 嶺上処理
                    Pai.Instance.LingShanChuLi();
                    break;
            }
            // 学習(自家 次状態)
            ziJiaShi.SetTransitionZiJiaNextState(GetState(Chang.Instance.ziMoFan, true));

            if (Chang.Instance.ziJiaYao == QueShi.YaoDingYi.Wu || Chang.Instance.ziJiaYao == QueShi.YaoDingYi.LiZhi)
            {
                ziJiaShi.DaiPaiXiangTingShuJiSuan(Chang.Instance.ziJiaXuanZe);
                // 打牌前
                int dp = ziJiaShi.DaPaiQian();
                Chang.Instance.isDuiJuDraw = true;
                yield return new WaitForSeconds(GameManager.waitTime / 2);
                // 打牌
                ziJiaShi.DaPai(dp, changZiJiaYao, changTaJiaYao);
                ziJiaShi.ShePaiChuLi(Chang.Instance.ziJiaYao);

                ziJiaShi.taJiaYao = QueShi.YaoDingYi.Wu;
                ziJiaShi.taJiaXuanZe = 0;
                ziJiaShi.SiKaoQianChuQiHua();

                ziJiaShi.LiPai();
                Chang.Instance.isDuiJuDraw = true;
                yield return new WaitForSeconds(GameManager.waitTime / 2);
                // 四風子連打処理
                Chang.Instance.SiFengZiLianDaChuLi(Chang.Instance.shePai);
                // 四風子連打判定
                if (Chang.Instance.SiFengZiLianDaPanDing())
                {
                    Chang.Instance.tingPaiLianZhuang = GuiZe.Instance.siFengZiLianDaLianZhuang == 1 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    Chang.Instance.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                }
            }

            if (changZiJiaYao == QueShi.YaoDingYi.JiaGang || changTaJiaYao == QueShi.YaoDingYi.DaMingGang)
            {
                // 加槓・大明槓成立
                // 嶺上牌処理
                Pai.Instance.LingShangPaiChuLi();
                // 四開槓判定
                if (Pai.Instance.SiKaiGangPanDing())
                {
                    Chang.Instance.tingPaiLianZhuang = GuiZe.Instance.siKaiGangLianZhuang == 0 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                    Chang.Instance.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                }
            }

            // 思考他家
            Chang.Instance.taJiaYao = QueShi.YaoDingYi.Wu;
            Chang.Instance.mingFan = Chang.Instance.ziMoFan;
            int playerJia = -1;
            int playerIndex = -1;
            for (int i = Chang.Instance.ziMoFan + 1; i < Chang.Instance.ziMoFan + queShis.Count; i++)
            {
                int jia = i % queShis.Count;
                QueShi taJiaShi = queShis[jia];
                // 思考他家判定
                taJiaShi.ziJiaYao = QueShi.YaoDingYi.Wu;
                taJiaShi.ziJiaXuanZe = 0;
                taJiaShi.SiKaoTaJiaPanDing(i - Chang.Instance.ziMoFan);
                if (taJiaShi.heLe || taJiaShi.chiPaiWei.Count > 0 || taJiaShi.bingPaiWei.Count > 0 || taJiaShi.daMingGangPaiWei.Count > 0)
                {
                    // 学習(他家 状態)
                    taJiaShi.SetTransitionTaJiaState(GetState(Chang.Instance.mingFan, false));
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
                    Chang.Instance.CuHe(jia);
                    Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                    Chang.Instance.isDuiJuDraw = true;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                }
                if (taJiaShi.taJiaYao > Chang.Instance.taJiaYao)
                {
                    // 栄和、大明槓、石並、吃
                    Chang.Instance.taJiaYao = taJiaShi.taJiaYao;
                    Chang.Instance.taJiaXuanZe = taJiaShi.taJiaXuanZe;
                    Chang.Instance.mingFan = jia;
                }
                if (taJiaShi.taJiaYao == QueShi.YaoDingYi.RongHe)
                {
                    Chang.Instance.rongHeFans.Add(new RongHeFan { fan = jia, index = i });
                }
            }
            if (playerJia >= 0)
            {
                // 思考他家プレイヤー分
                QueShi playerShi = queShis[playerJia];
                if (playerShi.heLe || playerShi.chiPaiWei.Count > 0 || playerShi.bingPaiWei.Count > 0 || playerShi.daMingGangPaiWei.Count > 0)
                {
                    if (playerShi.heLe
                        || (playerShi.daMingGangPaiWei.Count > 0 && Chang.Instance.taJiaYao < QueShi.YaoDingYi.DaMingGang)
                        || (playerShi.bingPaiWei.Count > 0 && Chang.Instance.taJiaYao < QueShi.YaoDingYi.Bing)
                        || (playerShi.chiPaiWei.Count > 0 && Chang.Instance.taJiaYao < QueShi.YaoDingYi.Chi))
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
                        Chang.Instance.isTaJiaYaoDraw = true;
                        Chang.Instance.isDuiJuDraw = true;
                        yield return Pause(ForwardMode.NORMAL);
                        if (Chang.Instance.rongHeFans.Count == 0 && playerShi.taJiaYao != QueShi.YaoDingYi.Wu)
                        {
                            Chang.Instance.taJiaYao = playerShi.taJiaYao;
                            Chang.Instance.taJiaXuanZe = playerShi.taJiaXuanZe;
                            Chang.Instance.mingFan = playerJia;
                        }
                        if (playerShi.taJiaYao == QueShi.YaoDingYi.RongHe)
                        {
                            Chang.Instance.rongHeFans.Add(new RongHeFan { fan = playerJia, index = playerIndex });
                        }
                    }
                }
            }
            Chang.Instance.isTaJiaYaoDraw = false;

            if (Chang.Instance.taJiaYao == QueShi.YaoDingYi.RongHe)
            {
                // 栄和
                Chang.Instance.rongHeFans = Chang.Instance.rongHeFans.OrderBy(x => x.index).ToList();
                Chang.Instance.mingFan = Chang.Instance.rongHeFans[0].fan;
                // 捨牌処理
                foreach (RongHeFan rongHeFan in Chang.Instance.rongHeFans)
                {
                    queShis[rongHeFan.fan].ZiMo(Chang.Instance.shePai);
                    // 和了
                    queShis[rongHeFan.fan].HeLeChuLi();
                }

                Chang.Instance.tingPaiLianZhuang = Chang.Instance.mingFan == Chang.Instance.qin ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                Chang.Instance.isDuiJuDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);

                if (Chang.Instance.rongHeFans.Count == 3)
                {
                    if (GuiZe.Instance.tRongHe == 0)
                    {
                        // トリプル栄和 無し(頭ハネ)
                        Chang.Instance.rongHeFans.RemoveRange(1, 2);
                    }
                    else if (GuiZe.Instance.tRongHe >= 2)
                    {
                        // トリプル栄和 流局
                        Chang.Instance.rongHeFans.Clear();
                        Chang.Instance.tingPaiLianZhuang = GuiZe.Instance.tRongHe == 2 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                        GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                        FileUtility.WriteAllGameDataFile();
                        yield break;
                    }
                }
                else if (Chang.Instance.rongHeFans.Count == 2)
                {
                    if (GuiZe.Instance.wRongHe == 0)
                    {
                        // ダブル栄和 無し(頭ハネ)
                        Chang.Instance.rongHeFans.RemoveAt(1);
                    }
                }
                if (Chang.Instance.rongHeFans.Count >= 2)
                {
                    // ダブル栄和・トリプル栄和の場合、親が栄和した場合は連荘
                    foreach (RongHeFan rongHeFan in Chang.Instance.rongHeFans)
                    {
                        if (queShis[rongHeFan.fan].feng == 0x31)
                        {
                            Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                }

                Chang.Instance.heleFan = Chang.Instance.mingFan;
                Chang.Instance.eventStatus = GameManager.Event.YI_BIAO_SHI;
                GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                FileUtility.WriteAllGameDataFile();
                yield break;
            }

            for (int i = Chang.Instance.ziMoFan + 1; i < Chang.Instance.ziMoFan + queShis.Count; i++)
            {
                // 振聴牌処理
                queShis[i % queShis.Count].ZhenTingPaiChuLi();
            }

            switch (Chang.Instance.ziJiaYao)
            {
                case QueShi.YaoDingYi.LiZhi:
                    // 立直成立
                    Chang.Instance.LiZhiChuLi();
                    queShis[Chang.Instance.ziMoFan].LiZiChuLi();
                    queShis[Chang.Instance.ziMoFan].ShePaiChuLi(QueShi.YaoDingYi.LiZhi);
                    // 四家立直判定
                    if (Chang.Instance.SiJiaLiZhiPanDing())
                    {
                        Chang.Instance.tingPaiLianZhuang = GuiZe.Instance.siJiaLiZhiLianZhuang == 1 ? Zhuang.LIAN_ZHUANG : Zhuang.LUN_ZHUANG;
                        Chang.Instance.isDuiJuDraw = true;
                        yield return Pause(ForwardMode.FAST_FORWARD);
                        Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                        GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                        FileUtility.WriteAllGameDataFile();
                        yield break;
                    }
                    break;
                case QueShi.YaoDingYi.JiaGang:
                    // 加槓
                    // 加槓処理
                    Pai.Instance.QiangGangChuLi();
                    Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                default:
                    break;
            }

            QueShi mingShi = queShis[Chang.Instance.mingFan];
            // 学習(他家 行動)
            mingShi.SetTransitionTaJiaAction(new() { (int)mingShi.taJiaYao, mingShi.taJiaXuanZe });

            switch (Chang.Instance.taJiaYao)
            {
                case QueShi.YaoDingYi.DaMingGang:
                    // 大明槓
                    mingShi.DaMingGang();
                    // 捨牌処理
                    queShis[Chang.Instance.ziMoFan].ShePaiChuLi(QueShi.YaoDingYi.DaMingGang);
                    Chang.Instance.ziMoFan = Chang.Instance.mingFan;
                    Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                case QueShi.YaoDingYi.Bing:
                    // 石並
                    mingShi.Bing();
                    // 捨牌処理
                    queShis[Chang.Instance.ziMoFan].ShePaiChuLi(QueShi.YaoDingYi.Bing);
                    Chang.Instance.ziMoFan = Chang.Instance.mingFan;
                    Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                case QueShi.YaoDingYi.Chi:
                    // 吃
                    mingShi.Chi();
                    // 捨牌処理
                    queShis[Chang.Instance.ziMoFan].ShePaiChuLi(QueShi.YaoDingYi.Chi);
                    Chang.Instance.ziMoFan = Chang.Instance.mingFan;
                    Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
                    GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                    FileUtility.WriteAllGameDataFile();
                    yield break;
                default:
                    break;
            }

            // 流局判定
            if (Pai.Instance.LiuJuPanDing())
            {
                // 流し満貫判定
                if (GuiZe.Instance.liuManGuan)
                {
                    for (int i = 0; i < queShis.Count; i++)
                    {
                        int jia = (Chang.Instance.qin + i) % queShis.Count;
                        if (queShis[jia].LiuJu())
                        {
                            Chang.Instance.ziJiaYao = QueShi.YaoDingYi.ZiMo;
                            Chang.Instance.ziMoFan = jia;
                            Chang.Instance.heleFan = jia;
                            break;
                        }
                    }
                }
                Chang.Instance.tingPaiLianZhuang = Zhuang.LUN_ZHUANG;
                if (GuiZe.Instance.nanChangBuTingLianZhuang && Chang.Instance.changFeng >= 0x32)
                {
                    Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                }
                for (int i = 0; i < queShis.Count; i++)
                {
                    int jia = (Chang.Instance.qin + i) % queShis.Count;
                    QueShi shi = queShis[jia];
                    // 形聴判定
                    shi.XingTingPanDing();
                    if (shi.liuShiManGuan)
                    {
                        // 流し満貫
                        if (jia == Chang.Instance.qin)
                        {
                            Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                    else if (shi.xingTing)
                    {
                        // 聴牌
                        if (jia == Chang.Instance.qin)
                        {
                            Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                        }
                    }
                    else
                    {
                        // 不聴
                        if (shi.liZhi)
                        {
                            // 錯和(不聴立直)
                            Chang.Instance.CuHe(jia);
                            Chang.Instance.tingPaiLianZhuang = Zhuang.LIAN_ZHUANG;
                            Chang.Instance.isDuiJuDraw = true;
                            yield return Pause(ForwardMode.FAST_FORWARD);
                            Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                            GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                            FileUtility.WriteAllGameDataFile();
                            yield break;
                        }
                    }
                    Chang.Instance.isDuiJuDraw = true;
                    shi.zhongLiao = true;
                    yield return new WaitForSeconds(GameManager.waitTime);
                }

                Chang.Instance.isDuiJuDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);
                Chang.Instance.eventStatus = GameManager.Event.DIAN_BIAO_SHI;
                GameManager.coroutines[GameManager.Event.DUI_JU] = null;
                FileUtility.WriteAllGameDataFile();
                yield break;
            }

            Chang.Instance.ziMoFan = (Chang.Instance.ziMoFan + 1) % queShis.Count;
            Chang.Instance.eventStatus = GameManager.Event.DUI_JU;
            GameManager.coroutines[GameManager.Event.DUI_JU] = null;
            Chang.Instance.isDuiJuDraw = true;
            FileUtility.WriteAllGameDataFile();
        }

        // 自家腰クリック
        public void OnClickZiJiaYao(int jia, QueShi shi, QueShi.YaoDingYi yao, int mingWei, int ShouPaiWei, bool isFollow)
        {
            if (yao == QueShi.YaoDingYi.Wu)
            {
                Draw.Instance.DrawZiJiaYao(shi, 0, 0, false, false);
                Draw.Instance.DrawShouPai(jia, yao, -1);
                shi.DaiPaiJiSuan(-1);
                Draw.Instance.DrawDaiPai(jia);
            }
            else if (yao == QueShi.YaoDingYi.LiZhi || yao == QueShi.YaoDingYi.KaiLiZhi)
            {
                Draw.Instance.DrawZiJiaYao(shi, (mingWei + 1) % shi.liZhiPaiWei.Count, ShouPaiWei, false, true);
                Draw.Instance.DrawShouPai(jia, yao, mingWei, isFollow);
                shi.DaiPaiXiangTingShuJiSuan(shi.liZhiPaiWei[mingWei]);
                Draw.Instance.DrawDaiPai(jia);
            }
            else if (yao == QueShi.YaoDingYi.AnGang && shi.anGangPaiWei.Count > 1)
            {
                Draw.Instance.DrawZiJiaYao(shi, (mingWei + 1) % shi.anGangPaiWei.Count, ShouPaiWei, false, true);
                Draw.Instance.DrawShouPai(jia, yao, mingWei, isFollow);
            }
            else if (yao == QueShi.YaoDingYi.JiaGang && shi.jiaGangPaiWei.Count > 1)
            {
                Draw.Instance.DrawZiJiaYao(shi, (mingWei + 1) % shi.jiaGangPaiWei.Count, ShouPaiWei, false, true);
                Draw.Instance.DrawShouPai(jia, yao, mingWei, isFollow);
            }
            else if (yao == QueShi.YaoDingYi.Select)
            {
                Draw.Instance.DrawZiJiaYao(shi, mingWei, ShouPaiWei, true, false);
                Draw.Instance.DrawShouPai(jia, yao, ShouPaiWei, isFollow);
                shi.DaiPaiXiangTingShuJiSuan(ShouPaiWei);
                Draw.Instance.DrawDaiPai(jia);
            }
            else
            {
                shi.ziJiaYao = yao;
                if (yao == QueShi.YaoDingYi.LiZhi || yao == QueShi.YaoDingYi.KaiLiZhi)
                {
                    mingWei %= shi.liZhiPaiWei.Count;
                }
                else if (yao == QueShi.YaoDingYi.AnGang)
                {
                    mingWei %= shi.anGangPaiWei.Count;
                }
                else if (yao == QueShi.YaoDingYi.JiaGang)
                {
                    mingWei %= shi.jiaGangPaiWei.Count;
                }
                shi.ziJiaXuanZe = mingWei;
                Chang.Instance.keyPress = true;
            }
        }


        // 他家腰クリック
        public void OnClickTaJiaYao(int jia, QueShi shi, QueShi.YaoDingYi yao, int mingWei)
        {
            if (yao == QueShi.YaoDingYi.Bing && shi.bingPaiWei.Count > 1)
            {
                Draw.Instance.DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.bingPaiWei.Count, false);
                Draw.Instance.DrawShouPai(jia, yao, mingWei);
            }
            else if (yao == QueShi.YaoDingYi.Chi && shi.chiPaiWei.Count > 1)
            {
                Draw.Instance.DrawTaJiaYao(jia, shi, (mingWei + 1) % shi.chiPaiWei.Count, false);
                Draw.Instance.DrawShouPai(jia, yao, mingWei);
            }
            else
            {
                shi.taJiaYao = yao;
                shi.taJiaXuanZe = 0;
                Chang.Instance.keyPress = true;
            }
        }

        // 牌クリック
        public void OnClickShouPai(int jia, QueShi.YaoDingYi yao, int xuanZe)
        {
            QueShi shi = queShis[jia];
            if (yao == QueShi.YaoDingYi.LiZhi || yao == QueShi.YaoDingYi.KaiLiZhi || yao == QueShi.YaoDingYi.AnGang || yao == QueShi.YaoDingYi.JiaGang)
            {
                shi.ziJiaYao = yao;
                shi.ziJiaXuanZe = xuanZe;
            }
            if (yao == QueShi.YaoDingYi.Bing || yao == QueShi.YaoDingYi.Chi)
            {
                shi.taJiaYao = yao;
                shi.taJiaXuanZe = xuanZe;
            }
            if (yao == QueShi.YaoDingYi.Wu)
            {
                if (SheDing.Instance.daPaiFangFa == 0)
                {
                    shi.ziJiaYao = QueShi.YaoDingYi.Wu;
                    shi.ziJiaXuanZe = xuanZe;
                }
                else
                {
                    Draw.Instance.DrawShouPai(jia, QueShi.YaoDingYi.Select, xuanZe);
                    shi.DaiPaiXiangTingShuJiSuan(xuanZe);
                    Draw.Instance.DrawDaiPai(jia);
                    return;
                }
            }
            if (yao == QueShi.YaoDingYi.DaPai)
            {
                shi.ziJiaYao = QueShi.YaoDingYi.Wu;
                shi.ziJiaXuanZe = xuanZe;
                Draw.Instance.DrawShouPai(jia, QueShi.YaoDingYi.Clear, 0);
            }
            Chang.Instance.keyPress = true;
        }


        // 対局終了
        public IEnumerator DuiJuZhongLe()
        {
            Chang.Instance.isDuiJuZhongLeDraw = true;
            yield return Pause(ForwardMode.FAST_FORWARD);

            Chang.Instance.eventStatus = GameManager.Event.YI_BIAO_SHI;
            GameManager.coroutines[GameManager.Event.DUI_JU_ZHONG_LE] = null;
            FileUtility.WriteAllGameDataFile();
        }

        // 役
        public IEnumerator YiBiaoShi()
        {
            Chang.Instance.isBackDuiJuZhongLe = false;

            Chang.Instance.yiBiaoShiFan = Chang.Instance.heleFan;

            if (Chang.Instance.rongHeFans.Count > 0)
            {
                foreach (RongHeFan rongHeFan in Chang.Instance.rongHeFans)
                {
                    Chang.Instance.isYiBiaoShiDraw = true;
                    Chang.Instance.yiBiaoShiFan = rongHeFan.fan;
                    yield return Pause(ForwardMode.FAST_FORWARD);
                    if (Chang.Instance.isBackDuiJuZhongLe)
                    {
                        break;
                    }
                }
            }
            else
            {
                Chang.Instance.isYiBiaoShiDraw = true;
                yield return Pause(ForwardMode.FAST_FORWARD);
            }

            Chang.Instance.eventStatus = Chang.Instance.isBackDuiJuZhongLe ? GameManager.Event.DUI_JU_ZHONG_LE : GameManager.Event.DIAN_BIAO_SHI;
            GameManager.coroutines[GameManager.Event.YI_BIAO_SHI] = null;
            FileUtility.WriteAllGameDataFile();
        }

        // 点
        public IEnumerator DianBiaoShi()
        {
            Chang.Instance.isDianBiaoShiDraw = true;
            Chang.Instance.DianJiSuan();

            // 記録 役数・役満数
            if (Chang.Instance.rongHeFans.Count > 0)
            {
                foreach (RongHeFan rongHeFan in Chang.Instance.rongHeFans)
                {
                    QueShi shi = queShis[rongHeFan.fan];
                    foreach (YiFan yiFan in shi.yiFans)
                    {
                        if (shi.yiMan)
                        {
                            Draw.Instance.ResizeYiManShu(shi);
                            shi.jiLu.yiManShu[yiFan.yi]++;
                        }
                        else
                        {
                            Draw.Instance.ResizeYiShu(shi);
                            shi.jiLu.yiShu[yiFan.yi]++;
                        }
                    }
                    if (shi.fanShuJi >= 13)
                    {
                        shi.jiLu.yiManShu[(int)QueShi.YiManDingYi.ShuYiMan]++;
                    }
                }
            }
            else
            {
                if (Chang.Instance.heleFan >= 0)
                {
                    QueShi shi = queShis[Chang.Instance.heleFan];
                    foreach (YiFan yiFan in shi.yiFans)
                    {
                        if (shi.yiMan)
                        {
                            Draw.Instance.ResizeYiManShu(shi);
                            shi.jiLu.yiManShu[yiFan.yi]++;
                        }
                        else
                        {
                            Draw.Instance.ResizeYiShu(shi);
                            shi.jiLu.yiShu[yiFan.yi]++;
                        }
                    }
                    if (shi.fanShuJi >= 13)
                    {
                        shi.jiLu.yiManShu[(int)QueShi.YiManDingYi.ShuYiMan]++;
                    }
                }
            }

            int max = 0;
            List<Transition> ziJiaList = new();
            List<Transition> taJiaList = new();
            for (int i = 0; i < queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % queShis.Count;
                QueShi shi = queShis[jia];
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
            if (SheDing.Instance.learningData)
            {
                FileUtility.WriteTransitionFile(ziJiaList, taJiaList);
            }
            for (int i = 0; i < queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % queShis.Count;
                QueShi shi = queShis[jia];
                shi.TransitionZiJiaList = null;
                shi.TransitionTaJiaList = null;
            }

            if (forwardMode > ForwardMode.NORMAL)
            {
                Chang.Instance.keyPress = true;
            }
            for (int i = 0; i < queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % queShis.Count;
                QueShi shi = queShis[jia];
                shi.shuBiao = shi.dianBang;
            }
            for (int shu = 0; shu <= max; shu += 100)
            {
                for (int i = 0; i < queShis.Count; i++)
                {
                    int jia = (Chang.Instance.qin + i) % queShis.Count;
                    QueShi shi = queShis[jia];
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

                Chang.Instance.isDianBiaoShiDraw = true;
                if (Chang.Instance.keyPress)
                {
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(0);
                }
            }

            for (int i = 0; i < queShis.Count; i++)
            {
                int jia = (Chang.Instance.qin + i) % queShis.Count;
                QueShi shi = queShis[jia];
                shi.shuBiao = shi.dianBang;
            }
            Chang.Instance.isDianBiaoShiDraw = true;

            // 記録の書込
            foreach (QueShi shi in queShis)
            {
                // 記録 対局数
                shi.jiLu.duiJuShu++;
                FileUtility.WriteJiLuFile(shi.mingQian, shi.jiLu);
            }

            yield return Pause(ForwardMode.FAST_FORWARD);

            if (Chang.Instance.tingPaiLianZhuang == Zhuang.LIAN_ZHUANG)
            {
                Chang.Instance.LianZhuang();
            }
            else
            {
                Chang.Instance.LunZhuang();
            }

            bool isQinTop = false;
            if (Chang.Instance.changFeng == 0x32 && Chang.Instance.ju == queShis.Count)
            {
                QueShi qinShi = queShis[Chang.Instance.qin];
                if (qinShi.ziJiaYao == QueShi.YaoDingYi.ZiMo || qinShi.taJiaYao == QueShi.YaoDingYi.RongHe)
                {
                    // 親の和了
                    int maxDian = 0;
                    for (int i = 0; i < queShis.Count; i++)
                    {
                        int jia = (Chang.Instance.qin + i) % queShis.Count;
                        QueShi shi = queShis[jia];
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
            int zhan = GuiZe.Instance.banZhuang ? 0x32 : 0x31;
            if (Chang.Instance.changFeng > zhan || (GuiZe.Instance.xiang && Chang.Instance.XiangPanDing()) || isQinTop)
            {
                Chang.Instance.eventStatus = GameManager.Event.ZHUANG_ZHONG_LE;
            }
            else
            {
                Chang.Instance.eventStatus = GameManager.Event.PEI_PAI;
            }

            Chang.Instance.DianGiSuanGongTuo();

            GameManager.coroutines[GameManager.Event.DIAN_BIAO_SHI] = null;
            FileUtility.WriteAllGameDataFile();
        }


        // 荘終了
        public IEnumerator ZhuangZhong()
        {
            // 得点設定
            SettingScore();

            Chang.Instance.isZhuangZhongLeDraw = true;
            yield return Pause(ForwardMode.FAST_FORWARD);

            // 記録の書込
            List<(int dian, QueShi shi)> shunWei = new();
            for (int i = Chang.Instance.qiaJia; i < Chang.Instance.qiaJia + queShis.Count; i++)
            {
                int jia = i % queShis.Count;
                QueShi shi = queShis[jia];
                shunWei.Add((shi.dianBang, shi));
            }
            shunWei.Sort((x, y) => y.dian.CompareTo(x.dian));
            for (int i = 0; i < shunWei.Count; i++)
            {
                QueShi shi = shunWei[i].shi;
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
                FileUtility.WriteJiLuFile(shi.mingQian, shi.jiLu);
            }

            Chang.Instance.banZhuangShu++;

            Debug.Log($"{Chang.Instance.banZhuangShu}回戦");
            for (int i = 0; i < shunWei.Count; i++)
            {
                QueShi shi = shunWei[i].shi;
                Debug.Log($" {i + 1}位 {shi.mingQian}({shi.jiJiDian})");
            }
            Chang.Instance.eventStatus = GameManager.Event.QUE_SHI_XUAN_ZE;

            GameManager.coroutines[GameManager.Event.ZHUANG_ZHONG_LE] = null;
            FileUtility.WriteAllGameDataFile();
        }

        // 得点設定
        private void SettingScore()
        {
            int geHe = 0;
            int maxDian = 0;
            int top = 0;
            for (int i = Chang.Instance.qiaJia; i < Chang.Instance.qiaJia + queShis.Count; i++)
            {
                int jia = i % queShis.Count;
                QueShi shi = queShis[jia];

                if (maxDian < shi.dianBang)
                {
                    maxDian = shi.dianBang;
                    top = jia;
                }
                int deDian = shi.dianBang / 1000;
                deDian -= GuiZe.Instance.fanDian / 1000;
                geHe += deDian;
                shi.JiJiDianJiSuan(deDian);
            }
            queShis[top].JiJiDianJiSuan(queShis[top].jiJiDian - geHe);
        }
    }
}

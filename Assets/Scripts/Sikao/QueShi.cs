using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Gongtong;

namespace Assets.Scripts.Sikao
{
    // 雀士
    public abstract class QueShi
    {
        // 牌
        public const int QUE_PAI = 0x3f;
        // 数牌
        public const int SHU_PAI = 0x0f;
        // 牌色
        public const int SE_PAI = 0x30;
        // 字牌
        public const int ZI_PAI = 0x30;
        // 赤牌
        public const int CHI_PAI = 0x40;

        // 【腰】
        public enum YaoDingYi
        {
            // 無
            Wu,
            // 吃
            Chi,
            // 石並
            Bing,
            // 大明槓
            DaMingGang,
            // 加槓
            JiaGang,
            // 暗槓
            AnGang,
            // 立直
            LiZhi,
            // 自摸
            ZiMo,
            // 栄和
            RongHe,
            // 九種九牌
            JiuZhongJiuPai,
            // 聴牌
            TingPai,
            // 不聴
            BuTing,
            // 和了
            HeLe,
            // 四開槓
            SiKaiGang,
            // 四家立直
            SiJiaLiZhi,
            // 流し満貫
            LiuManGuan,
            // 四風子連打
            SiFengZiLianDa,
            // 開立直
            KaiLiZhi,
            // 錯和
            CuHe,
            // 選択
            Select,
            // 打牌
            DaPai,
            // 取消
            Clear,
        }

        // 腰
        private static readonly Dictionary<YaoDingYi, (string ming, string yao, string button)> YaoMingDingYi = new(){
            { YaoDingYi.Wu, ("無", "", "パス") },
            { YaoDingYi.Chi, ("吃", "チー", "チー") },
            { YaoDingYi.Bing, ("石並", "ポン", "ポン") },
            { YaoDingYi.DaMingGang, ("大明槓", "カン", "カン") },
            { YaoDingYi.JiaGang, ("加槓", "カン", "加槓") },
            { YaoDingYi.AnGang, ("暗槓", "カン", "暗槓") },
            { YaoDingYi.LiZhi, ("立直", "リーチ", "立直") },
            { YaoDingYi.ZiMo, ("自摸", "ツモ", "ツモ") },
            { YaoDingYi.RongHe, ("栄和", "ロン", "ロン") },
            { YaoDingYi.JiuZhongJiuPai, ("九種九牌", "九種九牌", "九種") },
            { YaoDingYi.TingPai, ("聴牌", "テンパイ", "") },
            { YaoDingYi.BuTing, ("不聴", "ノーテン", "") },
            { YaoDingYi.HeLe, ("和了", "和了", "") },
            { YaoDingYi.SiKaiGang, ("四開槓", "四開槓", "") },
            { YaoDingYi.SiJiaLiZhi, ("四家立直", "四家立直", "") },
            { YaoDingYi.LiuManGuan, ("流し満貫", "流し満貫", "") },
            { YaoDingYi.SiFengZiLianDa, ("四風子連打", "四風子連打", "") },
            { YaoDingYi.KaiLiZhi, ("開立直", "オープンリーチ", "開立直") },
            { YaoDingYi.CuHe, ("錯和", "チョンボ", "") },
            { YaoDingYi.Select, ("選択", "", "") },
            { YaoDingYi.DaPai, ("打牌", "", "") },
            { YaoDingYi.Clear, ("取消", "", "取消") },
        };

        // 腰名
        public static string YaoMing(YaoDingYi yao)
        {
            return YaoMingDingYi[yao].yao;
        }
        // ボタン腰名
        public string YaoMingButton(YaoDingYi yao)
        {
            if ((yao == YaoDingYi.JiaGang || yao == YaoDingYi.AnGang) && (anGangPaiWei.Count == 0 || jiaGangPaiWei.Count == 0))
            {
                // 加槓、暗槓で片方のみ可能な場合、ボタン名は「カン」
                yao = YaoDingYi.DaMingGang;
            }
            return YaoMingDingYi[yao].button;
        }

        // 役満
        public enum YiManDingYi
        {
            // 数え役満
            ShuYiMan,
            // 天和
            TianHe,
            // 地和
            DeHe,
            // 国士無双
            GuoShiWuShuang,
            // 国士無双十三面
            GuoShiShiSanMian,
            // 四暗刻
            SiAnKe,
            // 四暗刻単騎
            SiAnKeDanQi,
            // 四槓子
            SiGangZi,
            // 大三元
            DaSanYuan,
            // 小四喜
            XiaoSiXi,
            // 大四喜
            DaSiXi,
            // 字一色
            ZiYiSe,
            // 清老頭
            QingLaoTou,
            // 九連宝燈
            JiuLianBaoDegn,
            // 純正九連宝燈
            ChunZhengJiuLian,
            // 緑一色
            LuYiSe,
            // 人和(ローカル)
            RenHe,
            // 四連刻(ローカル)
            SiLianKe,
            // 小車輪(ローカル)
            XiaoCheLun,
            // 大車輪(ローカル)
            DaCheLun,
            // 大竹林(ローカル)
            DaZhuLin,
            // 大数隣(ローカル)
            DaShuLin,
            // 紅孔雀(ローカル)
            GongKongQue,
            // 百万石(ローカル)
            BaiWanShi,
            // 純正百万石(ローカル)
            ChunZhengBaiWanShi,
            // 十三不塔(ローカル)
            ShiSanBuTa,
            // 八連荘(ローカル)
            BaLianZhuang,
        }

        // 役満名
        public static readonly Dictionary<YiManDingYi, string> YiManMing = new()
        {
            { YiManDingYi.ShuYiMan, "数え役満" },
            { YiManDingYi.TianHe, "天和" },
            { YiManDingYi.DeHe, "地和" },
            { YiManDingYi.GuoShiWuShuang, "国士無双" },
            { YiManDingYi.GuoShiShiSanMian, "国士無双十三面" },
            { YiManDingYi.SiAnKe, "四暗刻" },
            { YiManDingYi.SiAnKeDanQi, "四暗刻単騎" },
            { YiManDingYi.SiGangZi, "四槓子" },
            { YiManDingYi.DaSanYuan, "大三元" },
            { YiManDingYi.XiaoSiXi, "小四喜" },
            { YiManDingYi.DaSiXi, "大四喜" },
            { YiManDingYi.ZiYiSe, "字一色" },
            { YiManDingYi.QingLaoTou, "清老頭" },
            { YiManDingYi.JiuLianBaoDegn, "九連宝燈" },
            { YiManDingYi.ChunZhengJiuLian, "純正九連宝燈" },
            { YiManDingYi.LuYiSe, "緑一色" },
            { YiManDingYi.RenHe, "人和" },
            { YiManDingYi.SiLianKe, "四連刻" },
            { YiManDingYi.XiaoCheLun, "小車輪" },
            { YiManDingYi.DaCheLun, "大車輪" },
            { YiManDingYi.DaZhuLin, "大竹林" },
            { YiManDingYi.DaShuLin, "大数隣" },
            { YiManDingYi.GongKongQue, "紅孔雀" },
            { YiManDingYi.BaiWanShi, "百万石" },
            { YiManDingYi.ChunZhengBaiWanShi, "純正百万石" },
            { YiManDingYi.ShiSanBuTa, "十三不塔" },
            { YiManDingYi.BaLianZhuang, "八連荘" },
        };

        // 役
        public enum YiDingYi
        {
            // 立直
            LiZi,
            // Ｗ立直
            WLiZi,
            // 一発
            YiFa,
            // 海底撈月
            HaiDiLaoYue,
            // 河底撈魚
            HeDiLaoYu,
            // 嶺上開花
            LingShangKaiHua,
            // 槍槓
            QiangGang,
            // 面前清自摸和
            MianQianQingZiMoHe,
            // 平和
            PingHe,
            // 断幺九
            DuanYaoJiu,
            // 一盃口
            YiBeiKou,
            // 二盃口
            ErBeiKou,
            // 一気通貫
            YiQiTongGuan,
            // 三色同順
            SanSeTongShun,
            // 全帯幺
            QuanDaiYao,
            // 純全帯
            ChunQuanDai,
            // 混老頭
            HunLaoTou,
            // 三暗刻
            SanAnKe,
            // 三槓子
            SanGangZi,
            // 小三元
            XiaoSanYuan,
            // 混一色
            HunYiSe,
            // 清一色
            QingYiSe,
            // 対々和
            DuiDuiHe,
            // 役牌
            YiPai,
            // 七対子
            QiDuiZi,
            // 懸賞
            XuanShang,
            // 流し満貫(ローカル)
            LiuManGuan,
            // 三連刻(ローカル)
            SanLianKe,
            // 燕返し(ローカル)
            YanFan,
            // 開立直(ローカル)
            KaiLiZhi,
            // 開Ｗ立直(ローカル)
            KaiWLiZhi,
        }

        // 役名
        public static readonly Dictionary<YiDingYi, string> YiMing = new()
        {
            { YiDingYi.LiZi, "立直" },
            { YiDingYi.WLiZi, "Ｗ立直" },
            { YiDingYi.YiFa, "一発" },
            { YiDingYi.HaiDiLaoYue, "海底撈月" },
            { YiDingYi.HeDiLaoYu, "河底撈魚" },
            { YiDingYi.LingShangKaiHua, "嶺上開花" },
            { YiDingYi.QiangGang, "槍槓" },
            { YiDingYi.MianQianQingZiMoHe, "面前清自摸和" },
            { YiDingYi.PingHe, "平和" },
            { YiDingYi.DuanYaoJiu, "断幺九" },
            { YiDingYi.YiBeiKou, "一盃口" },
            { YiDingYi.ErBeiKou, "二盃口" },
            { YiDingYi.YiQiTongGuan, "一気通貫" },
            { YiDingYi.SanSeTongShun, "三色同順" },
            { YiDingYi.QuanDaiYao, "混全帯么九" },
            { YiDingYi.ChunQuanDai, "純全帯么九" },
            { YiDingYi.HunLaoTou, "混老頭" },
            { YiDingYi.SanAnKe, "三暗刻" },
            { YiDingYi.SanGangZi, "三槓子" },
            { YiDingYi.XiaoSanYuan, "小三元" },
            { YiDingYi.HunYiSe, "混一色" },
            { YiDingYi.QingYiSe, "清一色" },
            { YiDingYi.DuiDuiHe, "対々和" },
            { YiDingYi.YiPai, "役牌" },
            { YiDingYi.QiDuiZi, "七対子" },
            { YiDingYi.XuanShang, "ドラ" },
            { YiDingYi.LiuManGuan, "流し満貫" },
            { YiDingYi.SanLianKe, "三連刻" },
            { YiDingYi.YanFan, "燕返し" },
            { YiDingYi.KaiLiZhi, "開立直" },
            { YiDingYi.KaiWLiZhi, "開Ｗ立直" },
        };

        // 得点役
        public static readonly string[] DeDianYi = new string[] {
            "", "", "", "", "", "満貫", "跳満", "跳満", "倍満", "倍満", "倍満", "三倍満", "三倍満", "数え役満"
        };

        protected enum Ting
        {
            // 不聴
            BuTing = 0,
            // 形聴
            XingTing = 1,
            // 聴牌
            TingPai = 2,
        }

        // 名前
        public string mingQian = "";
        // 選択
        public bool selected;
        // 自家思考結果
        public YaoDingYi ziJiaYao;
        // 自家選択
        public int ziJiaXuanZe;
        // 他家思考結果
        public YaoDingYi taJiaYao;
        // 他家選択
        public int taJiaXuanZe;

        // プレイヤー
        public bool player = false;
        // プレイヤー順(プレイヤーが必ず0となり、そこから順番にふられる)
        public int playOrder = -1;
        // 理牌
        public bool liPaiDongZuo = true;
        // 外部思考
        public bool waiBuSikao = false;
        // 非同期停止
        public bool asyncStop = false;
        // フォロー
        public bool follow = false;
        // 記録
        public JiLu jiLu;

        // 役満
        public bool yiMan;
        // 役・飜(役、飜数)
        public List<YiFan> yiFans;
        // 飜数計
        public int fanShuJi;
        // 符
        public int fu;
        // 和了点
        public int heLeDian;
        // 点棒
        public int dianBang;
        // 集計点
        public int jiJiDian;
        // 風
        public int feng;
        // 手牌
        public List<int> shouPai;
        public Button[] goShouPai;
        // 副露牌(牌、家、腰)
        public List<FuLuPai> fuLuPais;
        public GoFuLuPai[] goFuLuPais;
        // 包則番
        public int baoZeFan;
        // 他家副露数
        public int taJiaFuLuShu;
        // 捨牌(牌、腰、自摸切)
        public List<ShePai> shePais;
        public Button[] goShePai = new Button[0x30];
        // 立直位
        public int liZhiWei;
        // 同順牌
        public List<int> tongShunPai = new();
        // 立直後牌
        public List<int> liZhiHouPai;
        // 待牌
        public List<int> daiPai;
        public Button[] goDaiPai = new Button[13];
        // 残牌数
        public TextMeshProUGUI[] goCanPaiShu = new TextMeshProUGUI[13];
        // 有効牌数
        public List<int> youXiaoPaiShu;
        // 向聴数
        public int xiangTingShu;
        public TextMeshProUGUI goXiangTingShu;
        // 対子
        public List<DuiZi> duiZis;
        // 刻子
        public List<KeZi> keZis;
        // 順子
        public List<ShunZi> shunZis;
        // 塔子
        public List<List<int>> taZis;
        // 手牌数
        public int[] shouPaiShu = new int[0x40];
        // 副露牌数
        public int[] fuLuPaiShu = new int[0x40];
        // 捨牌数
        public int[] shePaiShu = new int[0x40];
        // 立直後捨牌数
        public int[] liZhiShePaiShu = new int[0x40];
        // 公開牌数
        public int[] gongKaiPaiShu = new int[0x40];
        // 和了
        public bool heLe;
        // 立直
        public bool liZhi;
        // 開立直
        public bool kaiLiZhi;
        // W立直
        public bool wLiZhi;
        // 一発
        public bool yiFa;
        // 一巡目
        public bool yiXunMu;
        // 副露順
        public bool fuLuShun;
        // 形聴
        public bool xingTing;
        // 終了
        public bool zhongLiao;
        // 自家
        public bool jiJia;
        // 和了牌(牌、位、予想点)
        public List<(List<int> pais, int wei, int[] yuXiangDian)> heLePai;
        // 立直牌位
        public List<int> liZhiPaiWei;
        // 暗槓牌位
        public List<List<int>> anGangPaiWei;
        // 加槓牌位
        public List<List<int>> jiaGangPaiWei;
        // 大明槓牌位
        public List<List<int>> daMingGangPaiWei;
        // 石並牌位
        public List<List<int>> bingPaiWei;
        // 吃牌位
        public List<List<int>> chiPaiWei;
        // 九種九牌
        public bool jiuZhongJiuPai;
        // 受取
        public int shouQu;
        // 受取(供託)
        public int shouQuGongTuo;
        // 錯和声
        public string cuHeSheng;
        // 食替牌
        public List<int> shiTiPai = new();
        // 手牌点数
        public int[] shouPaiDian;
        // 連荘数
        public int lianZhuangShu;

        // 打牌後
        public bool daPaiHou;

        // 流し満貫
        public bool liuShiManGuan;
        // 点数（表示用）
        public int shuBiao;
        public enum XingGe
        {
            // 懸賞
            XUAN_SHANG = 0,
            // 役牌
            YI_PAI = 1,
            // 順子
            SHUN_ZI = 2,
            // 刻子
            KE_ZI = 3,
            // 立直
            LI_ZHI = 4,
            // 鳴き
            MING = 5,
            // 染め
            RAN = 6,
            // 逃げ
            TAO = 7,
        }

        public List<Nao> naos = new();

        // 遷移(自家)
        protected Transition transitionZiJia;
        public void SetTransitionZiJiaState(State state)
        {
            transitionZiJia = new()
            {
                mingQian = mingQian,
                state = state,
            };
        }
        public void SetTransitionZiJiaAction(List<int> action)
        {
            transitionZiJia.action = action;
        }
        public void SetTransitionZiJiaNextState(State state)
        {
            transitionZiJia.nextState = state;
            TransitionZiJiaList ??= new();
            TransitionZiJiaList.Add(transitionZiJia);
        }
        public void SetTransitionZiJiaReward(int score)
        {
            double gamma = 0.9;
            double reward = score;
            for (int i = TransitionZiJiaList.Count - 1; i >= 0; i--)
            {
                Transition transitionZiJia = TransitionZiJiaList[i];
                transitionZiJia.reward = reward;
                reward = Math.Round(reward * gamma, 2);
            }
        }
        public List<Transition> TransitionZiJiaList;
        // 遷移(他家)
        protected Transition transitionTaJia;
        public void SetTransitionTaJiaState(State state)
        {
            transitionTaJia = new()
            {
                mingQian = mingQian,
                state = state,
                action = new() { (int)YaoDingYi.Wu, 0 },
            };
        }
        public void SetTransitionTaJiaAction(List<int> action)
        {
            if (transitionTaJia == null)
            {
                return;
            }
            transitionTaJia.action = action;
            TransitionTaJiaList ??= new();
            TransitionTaJiaList.Add(transitionTaJia);
        }
        public List<Transition> TransitionTaJiaList;

        // コンストラクタ
        public QueShi()
        {
            goShouPai = new Button[14];
            goFuLuPais = new GoFuLuPai[4];
            for (int i = 0; i < goFuLuPais.Length; i++)
            {
                goFuLuPais[i] = new()
                {
                    goFuLuPai = new Button[4]
                };
            }
            shouPaiDian = new int[14];
        }

        // コンストラクタ
        public QueShi(string mingQian) : this()
        {
            this.mingQian = mingQian;
        }

        public abstract QueShi GetQueShi(string jsonText);

        // 状態
        public void ZhuangTai(State state, bool ziJia)
        {
            if (ziJia)
            {
                // 手牌数
                ShouPaiShuJiSuan();
                state.shouPaiShu = new(shouPaiShu);
                // 副露牌
                FuLuPaiShuJiSuan();
                state.fuLuPaiShu = new(fuLuPaiShu);
                // 立直
                state.liZhi = liZhi;
                // 向聴数
                state.xiangTingShu = xiangTingShu;
            }
            // 捨牌
            List<int> sp = new();
            for (int i = 0; i < 0x30; i++)
            {
                sp.Add(i < shePais.Count ? shePais[i].pai : 0);
            }
            if (ziJia)
            {
                state.shePai = sp;
            }
            else
            {
                state.taJiaShePai.Add(sp);
            }
        }

        // 思考自家
        public abstract void SiKaoZiJia();
        public abstract IEnumerator SiKaoZiJiaCoroutine();

        // 思考他家
        public abstract void SiKaoTaJia();
        public abstract IEnumerator SiKaoTaJiaCoroutine();

        // 荘初期化
        public void ZhuangChuQiHua()
        {
            dianBang = GuiZe.Instance.kaiShiDian;
            jiJiDian = 0;
        }

        // 局初期化
        public void JuChuQiHua(int feng)
        {
            this.feng = feng;
            shouPai = new();
            fuLuPais = new();
            baoZeFan = -1;
            taJiaFuLuShu = 0;
            shePais = new();
            Array.Fill(shePaiShu, 0);
            Array.Fill(liZhiShePaiShu, 0);
            liZhiHouPai = new();
            liZhiWei = -1;
            daiPai = new();
            youXiaoPaiShu = new();
            xiangTingShu = 0;
            heLePai = new();

            liZhi = false;
            kaiLiZhi = false;
            wLiZhi = false;
            yiFa = false;
            yiXunMu = true;
            fuLuShun = false;

            shouQu = 0;
            shouQuGongTuo = 0;
            cuHeSheng = "";
            daPaiHou = true;
            liuShiManGuan = false;
            zhongLiao = false;

            ziJiaYao = YaoDingYi.Wu;
            taJiaYao = YaoDingYi.Wu;
        }

        // 思考自家判定
        public void SiKaoZiJiaPanDing()
        {
            jiJia = true;

            ziJiaYao = YaoDingYi.Wu;
            ziJiaXuanZe = shouPai.Count - 1;
            taJiaYao = YaoDingYi.Wu;
            taJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            // 九種九牌判定
            JiuZhongJiuPaiPanDing();
            // 十三不塔判定
            if (ShiSanBuTaPanDing())
            {
                heLe = true;
            }

            // 食替牌判定
            ShiTiPaiPanDing();

            // 和了判定
            if (HeLePanDing() == Ting.TingPai)
            {
                heLe = true;
            }
            // 聴牌判定
            TingPaiPanDing();

            if (Pai.Instance.HaiDiPanDing())
            {
                return;
            }

            if (Pai.Instance.XuanShangPaiShu() <= 4 && Pai.Instance.CanShanPaiShu() >= MaQue.Instance.queShis.Count)
            {
                // 暗槓判定
                AnGangPanDing();
                // 加槓判定
                JiaGangPanDing();
            }

            // 向聴数計算
            XiangTingShuJiSuan();
        }

        // 思考他家判定
        public void SiKaoTaJiaPanDing(int jia)
        {
            jiJia = false;

            taJiaYao = YaoDingYi.Wu;
            taJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            shouPai.Add(Chang.Instance.shePai);
            // 和了判定
            if (HeLePanDing() == Ting.TingPai)
            {
                // 振聴判定
                if (!ZhenTingPanDing())
                {
                    heLe = true;
                }
            }
            shouPai.RemoveAt(shouPai.Count - 1);

            if (liZhi || wLiZhi)
            {
                return;
            }
            if (Pai.Instance.QiangGangPanDing())
            {
                return;
            }
            if (Pai.Instance.HaiDiPanDing())
            {
                return;
            }

            if (Pai.Instance.XuanShangPaiShu() <= 4)
            {
                // 大明槓判定
                DaMingGangPanDing();
            }
            // 石並判定
            BingPanDing();
            // 吃判定
            if (jia == 1)
            {
                ChiPanDing();
            }
        }

        // 理牌
        public void LiPai()
        {
            if (!liPaiDongZuo)
            {
                return;
            }
            Sort();
        }
        public void Sort()
        {
            shouPai.Sort((p1, p2) => (p1 & QUE_PAI).CompareTo(p2 & QUE_PAI));
        }

        // 自摸
        public void ZiMo(int p, bool daPaiHou = false)
        {
            shouPai.Add(p);
            this.daPaiHou = daPaiHou;
        }

        // 打牌前
        public int DaPaiQian()
        {
            int p = shouPai[Chang.Instance.ziJiaXuanZe];
            shouPai[Chang.Instance.ziJiaXuanZe] = 0xff;
            return p;
        }
        // 打牌
        public void DaPai(int p, YaoDingYi ziJiaYao = YaoDingYi.Wu, YaoDingYi taJiaYao = YaoDingYi.Wu)
        {
            Chang.Instance.shePai = p;
            shePaiShu[p & QUE_PAI]++;
            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                QueShi shi = MaQue.Instance.queShis[i];
                if (i != Chang.Instance.ziMoFan && shi.liZhi)
                {
                    shi.liZhiShePaiShu[Chang.Instance.shePai & QUE_PAI]++;
                }
            }
            shouPai.RemoveAt(Chang.Instance.ziJiaXuanZe);
            bool ziMoQie = false;
            if (ziJiaXuanZe == shouPai.Count)
            {
                if (ziJiaYao != YaoDingYi.AnGang && ziJiaYao != YaoDingYi.JiaGang && taJiaYao != YaoDingYi.Chi && taJiaYao != YaoDingYi.Bing && taJiaYao != YaoDingYi.DaMingGang)
                {
                    ziMoQie = true;
                }
            }
            shePais.Add(new ShePai { pai = Chang.Instance.shePai, yao = YaoDingYi.Wu, ziMoQie = ziMoQie });
            tongShunPai = new List<int>();

            fuLuShun = false;
            daPaiHou = true;
        }

        // 待牌計算
        public void DaiPaiJiSuan(int ziJiaXuanZe)
        {
            daiPai = new List<int>();
            foreach ((List<int> pais, int wei, _) in heLePai)
            {
                if (wei == ziJiaXuanZe)
                {
                    foreach (int hp in pais)
                    {
                        daiPai.Add(hp);
                    }
                }
            }
        }

        // 待牌・向聴数計算
        public void DaiPaiXiangTingShuJiSuan(int xuanZe)
        {
            DaiPaiJiSuan(xuanZe);
            GongKaiPaiShuJiSuan();
            XiangTingShuJiSuan(xuanZe);
        }

        // 和了処理
        public void HeLeChuLi()
        {
            // 和了判定
            HeLePanDing();
            // 点計算
            DianJiSuan();
            if (yiMan || fanShuJi >= 5)
            {
                fu = 0;
            }
        }

        // 立直処理
        public void LiZhiChuLi()
        {
            if (shePais.Count == 0)
            {
                wLiZhi = true;
            }
            liZhiWei = shePais.Count;
            liZhi = true;
            yiFa = true;
        }

        // 開立直処理
        public void KaiLiZhiChuLi()
        {
            if (ziJiaYao == YaoDingYi.KaiLiZhi)
            {
                kaiLiZhi = true;
                ziJiaYao = YaoDingYi.LiZhi;
            }
        }

        // 暗槓
        public void AnGang(int wei)
        {
            List<int> pais = new();
            foreach (int w in anGangPaiWei[wei])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPais.Add(new FuLuPai { pais = pais, jia = 0, yao = YaoDingYi.AnGang });

            List<int> paiWei = new(anGangPaiWei[wei]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                shouPai.RemoveAt(w);
            }

            // 理牌
            LiPai();
        }

        // 加槓
        public void JiaGang(int wei)
        {
            for (int i = 0; i < fuLuPais.Count; i++)
            {
                FuLuPai fuLuPai = fuLuPais[i];
                if (fuLuPai.yao == YaoDingYi.Bing)
                {
                    if ((fuLuPai.pais[0] & QUE_PAI) == (shouPai[jiaGangPaiWei[wei][0]] & QUE_PAI))
                    {
                        fuLuPais[i] = new FuLuPai { pais = new List<int>() { fuLuPai.pais[0], fuLuPai.pais[1], fuLuPai.pais[2], shouPai[jiaGangPaiWei[wei][0]] }, jia = fuLuPai.jia, yao = YaoDingYi.JiaGang };
                        Chang.Instance.shePai = shouPai[jiaGangPaiWei[wei][0]];
                        shouPai.RemoveAt(jiaGangPaiWei[wei][0]);
                        // 理牌
                        LiPai();
                        return;
                    }
                }
            }
        }

        // 副露家計算
        private int FuLuJiaJiSuan()
        {
            int mingFan = Chang.Instance.mingFan;
            int ziMoFan = Chang.Instance.ziMoFan;
            if (mingFan < ziMoFan)
            {
                mingFan += MaQue.Instance.queShis.Count;
            }
            return mingFan - ziMoFan;
        }

        // 包則判定
        private void BaoZePanDing()
        {
            if (!GuiZe.Instance.baoZe)
            {
                return;
            }
            if (baoZeFan >= 0)
            {
                return;
            }

            // 大四喜、大三元、四槓子
            int fengShu = 0;
            int sanYuanShu = 0;
            int gangShu = 0;
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                if (fuLuPai.yao == YaoDingYi.DaMingGang || fuLuPai.yao == YaoDingYi.Bing)
                {
                    int p = fuLuPai.pais[0] & QUE_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        fengShu++;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        sanYuanShu++;
                    }
                }
                if (fuLuPai.yao == YaoDingYi.AnGang || fuLuPai.yao == YaoDingYi.JiaGang || fuLuPai.yao == YaoDingYi.DaMingGang)
                {
                    gangShu++;
                }
            }
            if (fengShu == 4 || sanYuanShu == 3 || gangShu == 4)
            {
                FuLuPai fuLuPai = fuLuPais[^1];
                if (fuLuPai.yao == YaoDingYi.DaMingGang || fuLuPai.yao == YaoDingYi.Bing)
                {
                    int p = fuLuPai.pais[0] & QUE_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        baoZeFan = fuLuPai.jia;
                        return;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        baoZeFan = fuLuPai.jia;
                        return;
                    }
                    if (fuLuPai.yao == YaoDingYi.AnGang || fuLuPai.yao == YaoDingYi.JiaGang || fuLuPai.yao == YaoDingYi.DaMingGang)
                    {
                        baoZeFan = fuLuPai.jia;
                        return;
                    }
                }
            }
        }

        // 大明槓
        public void DaMingGang()
        {
            List<int> pais = new()
            {
                Chang.Instance.shePai
            };
            foreach (int w in daMingGangPaiWei[Chang.Instance.taJiaXuanZe])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPais.Add(new FuLuPai { pais = pais, jia = FuLuJiaJiSuan(), yao = YaoDingYi.DaMingGang });

            List<int> paiWei = new(daMingGangPaiWei[Chang.Instance.taJiaXuanZe]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                shouPai.RemoveAt(w);
            }

            taJiaFuLuShu++;
            // 理牌
            LiPai();

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 石並
        public void Bing()
        {
            List<int> pais = new()
            {
                Chang.Instance.shePai
            };
            foreach (int w in bingPaiWei[Chang.Instance.taJiaXuanZe])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPais.Add(new FuLuPai { pais = pais, jia = FuLuJiaJiSuan(), yao = YaoDingYi.Bing });

            List<int> paiWei = new(bingPaiWei[Chang.Instance.taJiaXuanZe]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                shouPai.RemoveAt(w);
            }

            taJiaFuLuShu++;
            // 理牌
            LiPai();

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 吃
        public void Chi()
        {
            List<int> pais = new()
            {
                Chang.Instance.shePai
            };
            foreach (int w in chiPaiWei[Chang.Instance.taJiaXuanZe])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPais.Add(new FuLuPai { pais = pais, jia = FuLuJiaJiSuan(), yao = YaoDingYi.Chi });

            List<int> paiWei = new(chiPaiWei[Chang.Instance.taJiaXuanZe]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                shouPai.RemoveAt(w);
            }

            taJiaFuLuShu++;
            // 理牌
            LiPai();

            fuLuShun = true;
        }

        // 振聴牌処理
        public void ZhenTingPaiChuLi()
        {
            tongShunPai.Add(Chang.Instance.shePai);
            if (liZhi)
            {
                liZhiHouPai.Add(Chang.Instance.shePai);
            }
        }

        // 捨牌処理
        public void ShePaiChuLi(YaoDingYi yao)
        {
            ShePai sp = shePais[^1];
            sp.yao = yao;
            shePais[^1] = sp;
            daPaiHou = true;
        }

        // 立直処理
        public void LiZiChuLi()
        {
            DianBangJiSuan(-1000, false);
        }

        // 鳴処理
        public void MingChuLi()
        {
            if (follow)
            {
                daPaiHou = false;
            }
        }

        // 流局
        public bool LiuJu()
        {
            yiMan = false;

            yiFans = new();
            fanShuJi = 0;

            heLeDian = 0;

            // 流し満貫
            LiuManGuan();

            // 点計算
            if (yiFans.Count > 0)
            {
                fanShuJi = 1;
                if (feng == 0x31)
                {
                    heLeDian = 12000;
                }
                else
                {
                    heLeDian = 8000;
                }
                liuShiManGuan = true;
                return true;
            }
            return false;
        }

        // 消
        public void Xiao()
        {
            yiFa = false;
            yiXunMu = false;
        }

        // 形聴判定
        public void XingTingPanDing()
        {
            xingTing = false;

            foreach (int p in Pai.Instance.quePai)
            {
                shouPai.Add(p);
                if (HeLePanDing() >= Ting.XingTing)
                {
                    xingTing = true;
                    shouPai.RemoveAt(shouPai.Count - 1);
                    return;
                }
                shouPai.RemoveAt(shouPai.Count - 1);
            }
        }

        // 点棒計算
        public void DianBangJiSuan(int dian)
        {
            DianBangJiSuan(dian, true);
        }
        public void DianBangJiSuan(int dian, bool isShouQu)
        {
            dianBang += dian;
            if (isShouQu)
            {
                shouQu += dian;
            }
        }

        public void ShouQuGongTuoJiSuan(int dian)
        {
            shouQuGongTuo = dian;
        }

        public void JiJiDianJiSuan(int dian)
        {
            jiJiDian = dian;
        }

        // 思考前初期化
        public void SiKaoQianChuQiHua()
        {
            heLe = false;
            heLePai = new();
            liZhiPaiWei = new();
            if (jiJia)
            {
                anGangPaiWei = new();
                jiaGangPaiWei = new();
                shiTiPai = new();
            }
            else
            {
                daMingGangPaiWei = new();
                bingPaiWei = new();
                chiPaiWei = new();
            }
            jiuZhongJiuPai = false;

            Array.Fill(shouPaiDian, 0);
        }

        // 字牌判定
        protected bool ZiPaiPanDing(int pai)
        {
            int p = pai & QUE_PAI;
            if (p > ZI_PAI)
            {
                return true;
            }
            return false;
        }

        // 幺九牌判定
        protected bool YaoJiuPaiPanDing(int pai)
        {
            int p = pai & QUE_PAI;
            int s = p & SHU_PAI;
            if (p > ZI_PAI)
            {
                return true;
            }
            else if (s == 1 || s == 9)
            {
                return true;
            }
            return false;
        }

        // 懸賞牌判定
        public int XuanShangPaiPanDing(int pai)
        {
            int p = pai & QUE_PAI;
            int xuan = 0;
            foreach (int x in Pai.Instance.xuanShangPai)
            {
                if (Pai.XUAN_SHANG_PAI_DING_YI[x & QUE_PAI] == p)
                {
                    xuan++;
                }
                if (pai >= CHI_PAI)
                {
                    xuan++;
                }
            }
            return xuan;
        }

        // 役牌判定
        protected int YiPaiPanDing(int pai)
        {
            int fan = 0;
            int p = pai & QUE_PAI;
            if (p == Chang.Instance.changFeng)
            {
                fan++;
            }
            if (p == feng)
            {
                fan++;
            }
            if (p == 0x35 || p == 0x36 || p == 0x37)
            {
                fan++;
            }

            return fan;
        }

        // 和了判定
        protected Ting HeLePanDing()
        {
            fu = 0;
            // 国士無双判定
            if (GuoShiWuShuangPanDing())
            {
                // 役満判定
                YiManPanDing();
                if (yiFans.Count > 0)
                {
                    // 聴牌
                    return Ting.TingPai;
                }
            }
            // 七対子判定
            if (QiDuiZiPanDing())
            {
                // 役満判定
                YiManPanDing();
                if (yiFans.Count > 0)
                {
                    return Ting.TingPai;
                }
                // 役判定
                YiPanDing();
                if (yiFans.Count > 0)
                {
                    fu = 25;
                    // 聴牌
                    return Ting.TingPai;
                }
            }

            List<YiFan> maxYiFan = new();
            int maxFanShuJi = 0;
            int maxFu = 0;
            int maxYiShu = 0;
            // 不聴
            Ting ret = Ting.BuTing;
            foreach (int sp in shouPai)
            {
                // 手牌数計算
                ShouPaiShuJiSuan();
                // 頭
                int t = sp & QUE_PAI;
                if (shouPaiShu[t] < 2)
                {
                    continue;
                }
                for (int x = 0; x < 2; x++)
                {
                    // 面子初期化
                    MianZiChuQiHua();

                    // 手牌数計算
                    ShouPaiShuJiSuan();
                    // 対子計算
                    DuiZiJiSuan(t, false);

                    for (int y = 0; y < 2; y++)
                    {
                        foreach (int p in Pai.Instance.quePai)
                        {
                            if ((x ^ y) == 0)
                            {
                                // 刻子計算
                                KeZiJiSuan(p);
                            }
                            else
                            {
                                // 順子計算
                                ShunZiJiSuan(p);
                            }
                        }
                    }
                    // 副露計算
                    FuLuJiSuan();
                    // 計
                    int shu = 0;
                    foreach (int s in shouPaiShu)
                    {
                        shu += s;
                        if (shu > 0)
                        {
                            break;
                        }
                    }
                    if (shu == 0)
                    {
                        // 役満判定
                        YiManPanDing();
                        if (yiFans.Count > 0)
                        {
                            // 聴牌
                            return Ting.TingPai;
                        }
                        // 役判定
                        YiPanDing();
                        FuJiSuan();
                        if (fanShuJi > maxFanShuJi)
                        {
                            maxFanShuJi = fanShuJi;
                            maxFu = fu;
                            maxYiFan = new(yiFans);
                            maxYiShu = yiFans.Count;
                        }
                        // 形聴
                        ret = Ting.XingTing;
                    }
                }
            }
            if (maxYiShu > 0)
            {
                // 聴牌
                ret = Ting.TingPai;
                fanShuJi = maxFanShuJi;
                fu = maxFu;
                yiFans = new(maxYiFan);
            }
            return ret;
        }

        // 有効牌数計算
        public void YouXiaoPaiShuJiSuan()
        {
            youXiaoPaiShu = new();
            int minXiangTingShu = 99;
            List<int> xiangTingShu = new();
            for (int i = 0; i < shouPai.Count; i++)
            {
                XiangTingShuJiSuan(i);
                xiangTingShu.Add(this.xiangTingShu);
                if (minXiangTingShu > this.xiangTingShu)
                {
                    minXiangTingShu = this.xiangTingShu;
                }
            }
            GongKaiPaiShuJiSuan();
            for (int i = 0; i < shouPai.Count; i++)
            {
                if (xiangTingShu[i] != minXiangTingShu)
                {
                    youXiaoPaiShu.Add(0);
                    continue;
                }
                int youXiaoPai = 0;
                List<int> shouPaiC = new(shouPai);
                foreach (int p in Pai.Instance.quePai)
                {
                    shouPai[i] = p;
                    gongKaiPaiShu[shouPai[i]]--;
                    gongKaiPaiShu[p]++;
                    for (int k = 0; k < shouPai.Count; k++)
                    {
                        if (i == k)
                        {
                            continue;
                        }
                        XiangTingShuJiSuan(k);
                        if (minXiangTingShu > this.xiangTingShu)
                        {
                            youXiaoPai += Pai.Instance.CanShu(gongKaiPaiShu[p]);
                            break;
                        }
                    }
                    gongKaiPaiShu[shouPai[i]]++;
                    gongKaiPaiShu[p]--;
                }
                youXiaoPaiShu.Add(youXiaoPai);
                shouPai = new(shouPaiC);
            }
        }

        // 向聴数計算
        public void XiangTingShuJiSuan()
        {
            int minXiangTing = 99;
            for (int i = 0; i < shouPai.Count; i++)
            {
                XiangTingShuJiSuan(i);
                if (minXiangTing > xiangTingShu)
                {
                    minXiangTing = xiangTingShu;
                }
            }
            xiangTingShu = minXiangTing;

            // 手牌数計算(向聴数計算で手牌の一番右を切った状態で手牌数を計算して終わるので、再度 手牌数を計算しておく)
            ShouPaiShuJiSuan();
        }
        public void XiangTingShuJiSuan(int wei, int plusMianZiShu = 0)
        {
            xiangTingShu = 99;
            int xiang;

            List<int> shouPaiC = new(shouPai);
            if (wei >= 0)
            {
                shouPai[wei] = 0xff;
            }
            shouPai.Sort();

            if (fuLuPais.Count == 0)
            {
                // 七対子向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();

                // 対子種数
                int chongShu = 0;
                foreach (int p in Pai.Instance.quePai)
                {
                    if (shouPaiShu[p] >= 2)
                    {
                        chongShu++;
                    }
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                xiangTingShu = 6 - duiZis.Count + (duiZis.Count - chongShu);

                // 国士無双向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();
                int shu = 0;
                bool chuHui = true;
                foreach (int p in Pai.YAO_JIU_PAI_DING_YI)
                {
                    if (shouPaiShu[p] > 0)
                    {
                        if (chuHui && shouPaiShu[p] > 1)
                        {
                            chuHui = false;
                            shu++;
                        }
                        shu++;
                    }
                }
                xiang = 13 - shu;
                if (xiangTingShu > xiang)
                {
                    xiangTingShu = xiang;
                }
            }

            for (int x = 0; x < 2; x++)
            {
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();

                for (int y = 0; y < 2; y++)
                {
                    foreach (int p in Pai.Instance.quePai)
                    {
                        if ((x ^ y) == 0)
                        {
                            // 刻子計算
                            KeZiJiSuan(p);
                        }
                        else
                        {
                            // 順子計算
                            ShunZiJiSuan(p);
                        }
                    }
                }
                // 副露計算
                FuLuJiSuan();

                foreach (int p in Pai.Instance.quePai)
                {
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                foreach (int p in Pai.Instance.quePai)
                {
                    // 塔子計算
                    TaZiJiSuan(p);
                }
                // 計
                int mianZiShu = keZis.Count + shunZis.Count + plusMianZiShu;
                int xingShu = mianZiShu;
                while (xingShu < 4 && taZis.Count > 0)
                {
                    xingShu++;
                }
                int duiZiShu = duiZis.Count;
                while (xingShu < 4 && duiZis.Count > 0)
                {
                    duiZiShu--;
                    xingShu++;
                }
                xiang = 8 - (2 * mianZiShu) - (xingShu - mianZiShu);
                if (duiZiShu > 0)
                {
                    xiang--;
                }
                if (xiangTingShu > xiang)
                {
                    xiangTingShu = xiang;
                }
            }

            shouPai = new(shouPaiC);
        }

        // 符計算
        private void FuJiSuan()
        {
            foreach (YiFan yiFan in yiFans)
            {
                if (yiFan.yi == (int)YiDingYi.QiDuiZi)
                {
                    fu = 25;
                    return;
                }
                if (yiFan.yi == (int)YiDingYi.PingHe)
                {
                    if (GuiZe.Instance.ziMoPingHe && jiJia)
                    {
                        fu = 20;
                        return;
                    }
                    fu = 30;
                    return;
                }
            }

            // 副底
            fu = 20;
            if (!jiJia && taJiaFuLuShu == 0)
            {
                // 門前栄和加符
                fu += 10;
            }
            // 雀頭
            foreach (DuiZi duiZi in duiZis)
            {
                int p = duiZi.pais[0] & QUE_PAI;
                if (p == Chang.Instance.changFeng)
                {
                    fu += 2;
                }
                if (p == feng)
                {
                    fu += 2;
                }
                if (ZiPaiPanDing(p))
                {
                    fu += 2;
                }
            }
            // 刻子
            foreach (KeZi keZi in keZis)
            {
                bool yaoJiu = false;
                if (YaoJiuPaiPanDing(keZi.pais[0]))
                {
                    yaoJiu = true;
                }
                switch (keZi.yao)
                {
                    case YaoDingYi.Wu:
                        // 暗刻
                        fu += yaoJiu ? 8 : 4;
                        break;
                    case YaoDingYi.Bing:
                        // 明刻
                        fu += yaoJiu ? 4 : 2;
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        fu += yaoJiu ? 32 : 16;
                        break;
                    case YaoDingYi.JiaGang:
                    case YaoDingYi.DaMingGang:
                        // 加槓・大明槓
                        fu += yaoJiu ? 16 : 8;
                        break;
                }
            }
            // 待
            if (jiJia)
            {
                fu += 2;
            }
            int heLePai = shouPai[^1] & QUE_PAI;
            bool daiDian = false;
            foreach (DuiZi duiZi in duiZis)
            {
                if (duiZi.pais[0] == heLePai)
                {
                    fu += 2;
                    daiDian = true;
                    break;
                }
            }
            if (!daiDian)
            {
                foreach (ShunZi shunZi in shunZis)
                {
                    if (shunZi.pais[1] == heLePai || (heLePai & SHU_PAI) == 3 || (heLePai & SHU_PAI) == 7)
                    {
                        fu += 2;
                        break;
                    }
                }
            }
            // 切上
            fu = Chang.Instance.Ceil(fu, 10);
            if (fu == 20)
            {
                // 食い平和形
                fu = 30;
            }
        }

        // 点計算
        private void DianJiSuan()
        {
            heLeDian = 0;
            if (yiMan)
            {
                heLeDian = (feng == 0x31 ? 48000 : 32000) * fanShuJi;
                return;
            }

            if (fanShuJi < 5)
            {
                heLeDian = (feng == 0x31 ? 6 : 4) * 2 * 2 * fu;
                for (int i = 0; i < fanShuJi; i++)
                {
                    heLeDian *= 2;
                }
                heLeDian = Chang.Instance.Ceil(heLeDian, 100);
                int limit = feng == 0x31 ? 12000 : 8000;
                if (heLeDian > limit)
                {
                    heLeDian = limit;
                }
            }
            else if (fanShuJi == 5)
            {
                // 満貫
                heLeDian = feng == 0x31 ? 12000 : 8000;
            }
            else if (fanShuJi <= 7)
            {
                // 跳満
                heLeDian = feng == 0x31 ? 18000 : 12000;
            }
            else if (fanShuJi <= 10)
            {
                // 倍満
                heLeDian = feng == 0x31 ? 24000 : 16000;
            }
            else if (fanShuJi <= 12)
            {
                // 三倍満
                heLeDian = feng == 0x31 ? 36000 : 24000;
            }
            else
            {
                // 役満
                heLeDian = feng == 0x31 ? 48000 : 32000;
            }
        }

        // 九種九牌判定
        private void JiuZhongJiuPaiPanDing()
        {
            if (!yiXunMu)
            {
                return;
            }

            if (YaoJiuPaiJiSuan() >= 9)
            {
                jiuZhongJiuPai = true;
            }
        }

        // 幺九牌種類数計算
        protected int YaoJiuPaiJiSuan()
        {
            int yaoJiuPaiShu = 0;
            foreach (int p in Pai.YAO_JIU_PAI_DING_YI)
            {
                if (shouPai.Exists(sp => (sp & QUE_PAI) == p))
                {
                    yaoJiuPaiShu++;
                }
            }
            return yaoJiuPaiShu;
        }

        // 振聴判定
        public bool ZhenTingPanDing()
        {
            foreach (int dp in daiPai)
            {
                // 振聴
                foreach (ShePai shePai in shePais)
                {
                    if (dp == (shePai.pai & QUE_PAI))
                    {
                        return true;
                    }
                }
                // 同順内振聴
                foreach (int pai in tongShunPai)
                {
                    if (dp == (pai & QUE_PAI))
                    {
                        return true;
                    }
                }
                // 立直後振聴
                foreach (int pai in liZhiHouPai)
                {
                    if (dp == (pai & QUE_PAI))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // 聴牌判定
        private void TingPaiPanDing()
        {
            List<int> shouPaiC = new(shouPai);
            for (int i = 0; i < shouPai.Count; i++)
            {
                shouPai[i] = 0xff;
                shouPai.Sort();

                int wei = 0;
                List<int> hp = new();
                int[] dian = new int[0x40];
                Array.Fill(dian, 0);
                foreach (int p in Pai.Instance.quePai)
                {
                    shouPai[^1] = p;

                    if (HeLePanDing() == Ting.TingPai)
                    {
                        hp.Add(p);
                        DianJiSuan();
                        dian[p] = heLeDian;
                        wei++;
                    }
                }
                if (wei > 0)
                {
                    heLePai.Add((hp, i, dian));

                    if (Pai.Instance.CanShanPaiShu() >= 4 && taJiaFuLuShu == 0 && (dianBang >= 1000 || GuiZe.Instance.jieJinLiZhi))
                    {
                        liZhiPaiWei.Add(i);
                    }
                }

                shouPai = new(shouPaiC);
            }
        }

        // 暗槓判定
        private void AnGangPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            foreach (int p in Pai.Instance.quePai)
            {
                if (shouPaiShu[p] >= 4)
                {
                    List<int> anGang = new();
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (p == (shouPai[j] & QUE_PAI))
                        {
                            anGang.Add(j);
                        }
                    }
                    anGangPaiWei.Add(anGang);
                }
            }
            // 立直後槓判定
            LiZhiHouGangPanDing();
        }

        // 立直後槓判定
        private void LiZhiHouGangPanDing()
        {
            if (!liZhi || anGangPaiWei.Count == 0)
            {
                return;
            }

            // 待牌計算
            DaiPaiJiSuan(shouPai.Count - 1);

            bool heLe = true;
            for (int i = 0; i < anGangPaiWei.Count; i++)
            {
                List<int> shouPaiC = new(shouPai);
                List<FuLuPai> fuLuPaiC = new(fuLuPais);

                AnGang(i);
                shouPai.Sort();
                foreach (int dp in daiPai)
                {
                    shouPai.Add(dp);
                    if (HeLePanDing() != Ting.TingPai)
                    {
                        heLe = false;
                        break;
                    }
                    shouPai.RemoveAt(shouPai.Count - 1);
                }

                shouPai = new(shouPaiC);
                fuLuPais = new(fuLuPaiC);
            }

            if (!heLe)
            {
                anGangPaiWei = new();
            }
        }

        // 加槓判定
        private void JiaGangPanDing()
        {
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                if (fuLuPai.yao != YaoDingYi.Bing)
                {
                    continue;
                }
                for (int j = 0; j < shouPai.Count; j++)
                {
                    if ((fuLuPai.pais[0] & QUE_PAI) == (shouPai[j] & QUE_PAI))
                    {
                        jiaGangPaiWei.Add(new List<int>() { j });
                    }
                }
            }
        }

        // 大明槓判定
        private void DaMingGangPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            int shePai = Chang.Instance.shePai & QUE_PAI;
            if (shouPaiShu[shePai] < 3)
            {
                return;
            }
            List<int> daMingGang = new();
            for (int i = 0; i < shouPai.Count; i++)
            {
                int p = shouPai[i] & QUE_PAI;
                if (p == shePai)
                {
                    daMingGang.Add(i);
                }
            }
            daMingGangPaiWei.Add(daMingGang);
        }

        // 石並判定
        private void BingPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            int shePai = Chang.Instance.shePai & QUE_PAI;
            if (shouPaiShu[shePai] < 2)
            {
                return;
            }
            if (shouPai.Count < 3)
            {
                return;
            }
            for (int i = 0; i < shouPai.Count - 1; i++)
            {
                int p1 = shouPai[i] & QUE_PAI;
                if (p1 == shePai)
                {
                    for (int j = i + 1; j < shouPai.Count; j++)
                    {
                        int p2 = shouPai[j] & QUE_PAI;
                        if (p2 == shePai)
                        {
                            bingPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                }
            }
        }

        // 吃判定
        private void ChiPanDing()
        {
            if (MaQue.Instance.queShis.Count <= 2)
            {
                return;
            }
            if (ZiPaiPanDing(Chang.Instance.shePai))
            {
                return;
            }
            if (shouPai.Count < 3)
            {
                return;
            }
            int se = Chang.Instance.shePai & SE_PAI;
            for (int i = 0; i < shouPai.Count - 1; i++)
            {
                int p1 = shouPai[i] & QUE_PAI;
                int s1 = p1 & SHU_PAI;
                if (ZiPaiPanDing(p1) || ((p1 & SE_PAI) != se))
                {
                    continue;
                }
                for (int j = i + 1; j < shouPai.Count; j++)
                {
                    int p2 = shouPai[j] & QUE_PAI;
                    int s2 = p2 & SHU_PAI;
                    if (ZiPaiPanDing(p2) || ((p2 & SE_PAI) != se))
                    {
                        continue;
                    }

                    int sps = Chang.Instance.shePai & SHU_PAI;
                    if (sps <= 7)
                    {
                        if (s1 == (sps + 1) && (s2 == sps + 2))
                        {
                            chiPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                    if (sps >= 2 && sps <= 8)
                    {
                        if (s1 == (sps - 1) && (s2 == sps + 1))
                        {
                            chiPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                    if (sps >= 3)
                    {
                        if (s1 == (sps - 2) && (s2 == sps - 1))
                        {
                            chiPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                }
            }
        }

        // 鳴牌判定
        public bool MingPaiPanDing(YaoDingYi fuLuZhong, int fuLuJia, int wei)
        {
            if (fuLuZhong == YaoDingYi.Chi && wei == 0)
            {
                return true;
            }
            else if (fuLuZhong == YaoDingYi.Bing)
            {
                if ((fuLuJia == 3 && wei == 2) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            else if (fuLuZhong == YaoDingYi.DaMingGang)
            {
                if ((fuLuJia == 3 && wei == 3) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            else if (fuLuZhong == YaoDingYi.JiaGang)
            {
                if ((fuLuJia == 3 && wei == 2) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            return false;
        }

        // 食替牌判定
        private void ShiTiPaiPanDing()
        {
            if (GuiZe.Instance.shiTi || !fuLuShun)
            {
                return;
            }

            FuLuPai fuLuPai = fuLuPais[^1];
            int mingPai = fuLuPai.pais[0] & QUE_PAI;
            shiTiPai.Add(mingPai);
            if ((mingPai & ZI_PAI) != ZI_PAI)
            {
                int p1 = fuLuPai.pais[1] & QUE_PAI;
                int p2 = fuLuPai.pais[2] & QUE_PAI;
                if (Math.Abs(p1 - p2) == 1)
                {
                    int mingShu = mingPai & SHU_PAI;
                    if (mingPai < p1 && mingShu < 7)
                    {
                        shiTiPai.Add(mingPai + 3);
                    }
                    if (mingPai > p2 && mingShu > 3)
                    {
                        shiTiPai.Add(mingPai - 3);
                    }
                }
            }
        }

        // 国士無双判定
        private bool GuoShiWuShuangPanDing()
        {
            if (taJiaFuLuShu > 0)
            {
                return false;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();

            int shu = 0;
            foreach (int p in Pai.YAO_JIU_PAI_DING_YI)
            {
                if (shouPaiShu[p] == 0)
                {
                    return false;
                }
                if (shouPaiShu[p] == 2)
                {
                    duiZis.Add(new DuiZi { pais = new List<int>() { p, p } });
                    shu++;
                }
            }
            if (shu == 1)
            {
                return true;
            }
            return false;
        }

        // 七対子判定
        private bool QiDuiZiPanDing()
        {
            if (taJiaFuLuShu > 0)
            {
                return false;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();

            foreach (int p in Pai.Instance.quePai)
            {
                int shu = shouPaiShu[p];
                if (shu == 0)
                {
                    continue;
                }
                if (shu == 2)
                {
                    DuiZiJiSuan(p);
                }
                if (shu != 2)
                {
                    return false;
                }
            }

            int beiKou = 0;
            for (int i = 0; i < duiZis.Count - 2; i++)
            {
                if (ZiPaiPanDing(duiZis[i].pais[0]))
                {
                    continue;
                }

                if ((duiZis[i].pais[0] + 1) == duiZis[i + 1].pais[0] && (duiZis[i].pais[0] + 2) == duiZis[i + 2].pais[0])
                {
                    beiKou++;
                    i += 2;
                }
            }
            if (beiKou == 2)
            {
                // 二盃口
                return false;
            }

            return true;
        }

        // 面子初期化
        protected void MianZiChuQiHua()
        {
            duiZis = new();
            keZis = new();
            shunZis = new();
            taZis = new();
        }

        /**
         * 役満判定
         */
        private void YiManPanDing()
        {
            yiMan = false;

            yiFans = new();
            fanShuJi = 0;

            // 天和
            TianHe();
            // 地和
            DeHe();
            // 国士無双・国士無双十三面
            GuoShiWuShuang();
            // 四暗刻・四暗刻単騎・四槓子
            SiAnKeSiGangZi();
            // 大三元
            DaSanYuan();
            // 小四喜・大四喜
            XiaoSiXiDaSiXi();
            // 字一色
            ZiYiSe();
            // 清老頭
            QingLaoTou();
            // 九連宝燈・純正九連宝燈
            JiuLianBaoDeng();
            // 緑一色
            LuYiSe();
            // 人和
            RenHe();
            // 四連刻
            SiLianKe();
            // 小車輪・大車輪
            DaCheLun();
            // 大竹林
            DaZhuLin();
            // 大数隣
            DaShuLin();
            // 紅孔雀
            GongKongQue();
            // 百万石・純正百万石
            BaiWanShi();
            if (GuiZe.Instance.baLianZhuang && lianZhuangShu == 7)
            {
                // 八連荘
                YiZhuiJia(YiManDingYi.BaLianZhuang, 1);
            }

            fanShuJi = 0;
            foreach (YiFan yiFan in yiFans)
            {
                fanShuJi += yiFan.fanShu;
            }

            if (fanShuJi > 0)
            {
                yiMan = true;
            }
        }

        // 天和
        private void TianHe()
        {
            if (feng != 0x31)
            {
                return;
            }
            if (yiXunMu)
            {
                YiZhuiJia(YiManDingYi.TianHe, 1);
            }
        }

        // 地和
        private void DeHe()
        {
            if (feng == 0x31)
            {
                return;
            }
            if (yiXunMu)
            {
                if (jiJia)
                {
                    YiZhuiJia(YiManDingYi.DeHe, 1);
                }
            }
        }

        // 国士無双・国士無双十三面
        private void GuoShiWuShuang()
        {
            if (duiZis.Count == 1 && keZis.Count == 0 && shunZis.Count == 0)
            {
                if ((shouPai[^1] & QUE_PAI) == duiZis[0].pais[0])
                {
                    YiZhuiJia(YiManDingYi.GuoShiShiSanMian, 2);
                }
                else
                {
                    YiZhuiJia(YiManDingYi.GuoShiWuShuang, 1);
                }
            }
        }

        // 四暗刻・四暗刻単騎・四槓子
        private void SiAnKeSiGangZi()
        {
            if (keZis.Count < 4)
            {
                return;
            }
            int anKe = 0;
            int gangZi = 0;
            foreach (KeZi keZi in keZis)
            {
                if (keZi.yao == YaoDingYi.Wu || keZi.yao == YaoDingYi.AnGang)
                {
                    anKe++;
                }
                if (keZi.yao == YaoDingYi.AnGang || keZi.yao == YaoDingYi.JiaGang || keZi.yao == YaoDingYi.DaMingGang)
                {
                    gangZi++;
                }
            }
            if (anKe == 4)
            {
                if ((shouPai[^1] & QUE_PAI) == duiZis[0].pais[0])
                {
                    YiZhuiJia(YiManDingYi.SiAnKeDanQi, 2);
                }
                else
                {
                    YiZhuiJia(YiManDingYi.SiAnKe, 1);
                }
            }
            if (gangZi == 4)
            {
                YiZhuiJia(YiManDingYi.SiGangZi, 1);
            }
        }

        // 大三元
        private void DaSanYuan()
        {
            if (keZis.Count < 3)
            {
                return;
            }
            int yuan = 0;
            foreach (KeZi keZi in keZis)
            {
                int p = keZi.pais[0];
                if (p == 0x35 || p == 0x36 || p == 0x37)
                {
                    yuan++;
                }
            }
            if (yuan == 3)
            {
                YiZhuiJia(YiManDingYi.DaSanYuan, 1);
            }
        }

        // 小四喜・大四喜
        private void XiaoSiXiDaSiXi()
        {
            if (keZis.Count < 3)
            {
                return;
            }
            int sixi = 0;
            foreach (KeZi keZi in keZis)
            {
                if (keZi.pais[0] >= 0x31 && keZi.pais[0] <= 0x34)
                {
                    sixi++;
                }
            }
            int dui = 0;
            foreach (DuiZi duiZi in duiZis)
            {
                if (duiZi.pais[0] >= 0x31 && duiZi.pais[0] <= 0x34)
                {
                    dui++;
                }
            }
            if (sixi == 4)
            {
                YiZhuiJia(YiManDingYi.DaSiXi, 2);
            }
            if (sixi == 3 && dui == 1)
            {
                YiZhuiJia(YiManDingYi.XiaoSiXi, 1);
            }
        }

        // 字一色
        private void ZiYiSe()
        {
            foreach (int sp in shouPai)
            {
                if (!ZiPaiPanDing(sp))
                {
                    return;
                }
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    if (!ZiPaiPanDing(fp))
                    {
                        return;
                    }
                }
            }
            YiZhuiJia(YiManDingYi.ZiYiSe, 1);
        }

        // 清老頭
        private void QingLaoTou()
        {
            foreach (int sp in shouPai)
            {
                int p = sp & QUE_PAI;
                int s = p & SHU_PAI;
                if (ZiPaiPanDing(p))
                {
                    return;
                }
                if (s >= 2 && s <= 8)
                {
                    return;
                }
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int p = fp & QUE_PAI;
                    int s = p & SHU_PAI;
                    if (ZiPaiPanDing(p))
                    {
                        return;
                    }
                    if (s >= 2 && s <= 8)
                    {
                        return;
                    }
                }
            }
            YiZhuiJia(YiManDingYi.QingLaoTou, 1);
        }

        // 九連宝燈・純正九連宝燈
        private void JiuLianBaoDeng()
        {
            if (taJiaFuLuShu > 0)
            {
                return;
            }
            int[] jiuLian = new int[10];
            Array.Fill(jiuLian, 0);
            int se = -1;
            foreach (int sp in shouPai)
            {
                int p = sp & QUE_PAI;
                if (p > QUE_PAI)
                {
                    return;
                }
                if (se == -1)
                {
                    se = p & SE_PAI;
                }
                else
                {
                    if (se != (p & SE_PAI))
                    {
                        return;
                    }
                }
                jiuLian[p & SHU_PAI]++;
            }
            for (int i = 1; i < jiuLian.Length; i++)
            {
                if (i == 1 || i == 9)
                {
                    if (jiuLian[i] < 3)
                    {
                        return;
                    }
                }
                else
                {
                    if (jiuLian[i] == 0)
                    {
                        return;
                    }
                }
            }
            bool chunZheng = false;
            int heLePai = shouPai[^1];
            int s = heLePai & SHU_PAI;
            jiuLian[s]--;
            if (s == 1 || s == 9)
            {
                if (jiuLian[s] == 3)
                {
                    chunZheng = true;
                }
            }
            else
            {
                if (jiuLian[s] == 1)
                {
                    chunZheng = true;
                }
            }
            if (chunZheng)
            {
                YiZhuiJia(YiManDingYi.ChunZhengJiuLian, 2);
            }
            else
            {
                YiZhuiJia(YiManDingYi.JiuLianBaoDegn, 1);
            }
        }

        // 緑一色
        private void LuYiSe()
        {
            foreach (int sp in shouPai)
            {
                int p = sp & QUE_PAI;
                bool luYi = false;
                foreach (int lp in Pai.LU_YI_SE_PAI_DING_YI)
                {
                    if (p == lp)
                    {
                        luYi = true;
                        break;
                    }
                }
                if (!luYi)
                {
                    return;
                }
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int p = fp & QUE_PAI;
                    bool luYi = false;
                    foreach (int lp in Pai.LU_YI_SE_PAI_DING_YI)
                    {
                        if (p == lp)
                        {
                            luYi = true;
                            break;
                        }
                    }
                    if (!luYi)
                    {
                        return;
                    }
                }
            }
            YiZhuiJia(YiManDingYi.LuYiSe, 1);
        }

        // 人和
        private void RenHe()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            if (feng == 0x31)
            {
                return;
            }
            if (yiXunMu)
            {
                if (!jiJia)
                {
                    YiZhuiJia(YiManDingYi.RenHe, 1);
                }
            }
        }

        // 四連刻
        private void SiLianKe()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            if (keZis.Count < 4)
            {
                return;
            }
            List<int> k = new()
            {
                keZis[0].pais[0],
                keZis[1].pais[0],
                keZis[2].pais[0],
                keZis[3].pais[0]
            };
            k.Sort();
            foreach (int p in k)
            {
                if (ZiPaiPanDing(p))
                {
                    return;
                }
            }
            if ((k[0] + 1 == k[1]) && (k[0] + 2 == k[2]) && (k[0] + 3 == k[3]))
            {
                YiZhuiJia(YiManDingYi.SiLianKe, 1);
            }
        }

        // 小車輪・大車輪
        private void DaCheLun()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 大車輪
            bool daCheLun = true;
            for (int i = 0x12; i <= 0x18; i++)
            {
                if (shouPaiShu[i] != 2)
                {
                    daCheLun = false;
                    break;
                }
            }
            // 小車輪(1-7)
            bool xiaoCheLun17 = true;
            for (int i = 0x11; i <= 0x17; i++)
            {
                if (shouPaiShu[i] != 2)
                {
                    xiaoCheLun17 = false;
                    break;
                }
            }
            // 小車輪(3-9)
            bool xiaoCheLun39 = true;
            for (int i = 0x13; i <= 0x19; i++)
            {
                if (shouPaiShu[i] != 2)
                {
                    xiaoCheLun39 = false;
                    break;
                }
            }

            if (daCheLun)
            {
                YiZhuiJia(YiManDingYi.DaCheLun, 1);
            }
            else if (xiaoCheLun17 || xiaoCheLun39)
            {
                YiZhuiJia(YiManDingYi.XiaoCheLun, 1);
            }
        }

        // 大竹林
        private void DaZhuLin()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            for (int i = 0x22; i <= 0x28; i++)
            {
                if (shouPaiShu[i] != 2)
                {
                    return;
                }
            }
            YiZhuiJia(YiManDingYi.DaZhuLin, 1);
        }

        // 大数隣
        private void DaShuLin()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            for (int i = 0x02; i <= 0x08; i++)
            {
                if (shouPaiShu[i] != 2)
                {
                    return;
                }
            }
            YiZhuiJia(YiManDingYi.DaShuLin, 1);
        }

        // 紅孔雀
        private void GongKongQue()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            foreach (int sp in shouPai)
            {
                int p = sp & QUE_PAI;
                bool gongKong = false;
                foreach (int lp in Pai.GONG_KONG_QUE_PAI_DING_YI)
                {
                    if (p == lp)
                    {
                        gongKong = true;
                        break;
                    }
                }
                if (!gongKong)
                {
                    return;
                }
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int p = fp & QUE_PAI;
                    bool gongKong = false;
                    foreach (int lp in Pai.GONG_KONG_QUE_PAI_DING_YI)
                    {
                        if (p == lp)
                        {
                            gongKong = true;
                            break;
                        }
                    }
                    if (!gongKong)
                    {
                        return;
                    }
                }
            }
            YiZhuiJia(YiManDingYi.GongKongQue, 1);
        }

        // 百万石・純正百万石
        private void BaiWanShi()
        {
            if (!GuiZe.Instance.localYiMan)
            {
                return;
            }
            int wanShi = 0;
            foreach (int sp in shouPai)
            {
                int se = sp & SE_PAI;
                if (se != 0x00)
                {
                    return;
                }
                wanShi += sp & SHU_PAI;
            }

            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int se = fp & SE_PAI;
                    if (se != 0x00)
                    {
                        return;
                    }
                    wanShi += fp & SHU_PAI;
                }
            }

            if (wanShi == 100)
            {
                YiZhuiJia(YiManDingYi.ChunZhengBaiWanShi, 2);
            }
            else if (wanShi > 100)
            {
                YiZhuiJia(YiManDingYi.BaiWanShi, 1);
            }
        }

        // 十三不塔判定
        private bool ShiSanBuTaPanDing()
        {
            if (!GuiZe.Instance.shiSanBuTa)
            {
                return false;
            }

            yiMan = false;
            yiFans = new();
            fanShuJi = 0;

            if (!yiXunMu)
            {
                return false;
            }
            if (!jiJia)
            {
                return false;
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();
            foreach (int p in Pai.Instance.quePai)
            {
                if (shouPaiShu[p] == 2)
                {
                    DuiZiJiSuan(p);
                    shouPaiShu[p] += 2;
                    break;
                }
            }
            foreach (int p in Pai.Instance.quePai)
            {
                int shu = shouPaiShu[p];
                if (shu > 1)
                {
                    return false;
                }
                if (ZiPaiPanDing(p))
                {
                    continue;
                }
                if (shu == 1)
                {
                    int s = p & SHU_PAI;
                    if (s <= 7)
                    {
                        if (shouPaiShu[p + 1] > 0 || shouPaiShu[p + 2] > 0)
                        {
                            return false;
                        }
                    }
                    if (s >= 3)
                    {
                        if (shouPaiShu[p - 1] > 0 || shouPaiShu[p - 2] > 0)
                        {
                            return false;
                        }
                    }
                }
            }

            yiMan = true;
            YiZhuiJia(YiManDingYi.ShiSanBuTa, 1);
            fanShuJi = 1;
            return true;
        }

        // 役判定
        private void YiPanDing()
        {
            yiFans = new();
            fanShuJi = 0;

            // 立直・Ｗ立直・一発
            LiZhiWLiZhiYiFa();
            // 面前清自摸和
            MianQianQingZiMohe();
            // 嶺上開花
            LingShangKaiHua();
            // 槍槓
            QiangGang();
            // 海底撈月・河底撈魚
            HaiDiLaoYueHeDiLaoYu();
            // 平和
            PingHe();
            // 断幺九
            DuanYaoJiu();
            // 一盃口・二盃口
            YiBeiKouErBeiKou();
            // 役牌
            YiPai();
            // 三色同順
            SanSeTongShun();
            // 一気通貫
            YiQiTongGuan();
            // 全帯幺・純全帯・混老頭
            QuanDaiYaoChunQuanDaiHunLaoTou();
            // 対々和
            DuiDuiHe();
            // 三暗刻・三槓子
            SanAnKeSanGangZi();
            // 小三元
            XiaoSanYuan();
            // 混一色・清一色
            HunYiSeQingYiSe();
            // 七対子
            QiDuiZi();
            // 三連刻
            SanLianKe();

            if (yiFans.Count > 0)
            {
                // 懸賞牌
                XuanShangPai();
                if (!jiJia)
                {
                    if (GuiZe.Instance.yanFan && MaQue.Instance.queShis[Chang.Instance.ziMoFan].ziJiaYao == YaoDingYi.LiZhi)
                    {
                        // 燕返し
                        YiZhuiJia(YiDingYi.YanFan, 1);
                    }
                }
                if (kaiLiZhi)
                {
                    // 開立直
                    if (taJiaYao == YaoDingYi.RongHe)
                    {
                        bool isYiman = false;
                        QueShi ziMoShi = MaQue.Instance.queShis[Chang.Instance.ziMoFan];
                        if (!ziMoShi.liZhi)
                        {
                            isYiman = true;
                        }
                        int ziMoFanLiZhiWei = 0;
                        for (int i = 0; i < ziMoShi.shePais.Count; i++)
                        {
                            if (ziMoShi.shePais[i].yao == YaoDingYi.LiZhi)
                            {
                                break;
                            }
                            ziMoFanLiZhiWei++;
                        }
                        int ronHeFanLiZhiWei = 0;
                        for (int i = 0; i < shePais.Count; i++)
                        {
                            if (shePais[i].yao == YaoDingYi.LiZhi)
                            {
                                break;
                            }
                            ronHeFanLiZhiWei++;
                        }
                        if (ziMoFanLiZhiWei > ronHeFanLiZhiWei || (ziMoFanLiZhiWei == ronHeFanLiZhiWei && ziMoShi.feng > feng))
                        {
                            isYiman = true;
                        }

                        if (isYiman)
                        {
                            yiFans = new();
                            YiZhuiJia(YiDingYi.KaiLiZhi, 13);
                        }
                    }
                }
                if (lianZhuangShu == 7)
                {
                    // 八連荘
                    yiFans = new();
                    YiZhuiJia(YiManDingYi.BaLianZhuang, 1);
                    yiMan = true;
                }
            }

            fanShuJi = 0;
            foreach (YiFan yiFan in yiFans)
            {
                fanShuJi += yiFan.fanShu;
            }
        }

        // 立直・Ｗ立直・一発
        private void LiZhiWLiZhiYiFa()
        {
            if (kaiLiZhi)
            {
                if (wLiZhi)
                {
                    YiZhuiJia(YiDingYi.KaiWLiZhi, 3);
                }
                else if (liZhi)
                {
                    YiZhuiJia(YiDingYi.KaiLiZhi, 2);
                }
            }
            else
            {
                if (wLiZhi)
                {
                    YiZhuiJia(YiDingYi.WLiZi, 2);
                }
                else if (liZhi)
                {
                    YiZhuiJia(YiDingYi.LiZi, 1);
                }
            }

            if (yiFa)
            {
                YiZhuiJia(YiDingYi.YiFa, 1);
            }
        }

        // 面前清自摸和
        private void MianQianQingZiMohe()
        {
            if (jiJia && taJiaFuLuShu == 0)
            {
                YiZhuiJia(YiDingYi.MianQianQingZiMoHe, 1);
            }
        }

        // 嶺上開花
        private void LingShangKaiHua()
        {
            if (Pai.Instance.LingShanKaiHuaPanDing())
            {
                YiZhuiJia(YiDingYi.LingShangKaiHua, 1);
            }
        }

        // 槍槓
        private void QiangGang()
        {
            if (Pai.Instance.QiangGangPanDing())
            {
                YiZhuiJia(YiDingYi.QiangGang, 1);
            }
        }

        // 海底撈月・河底撈魚
        private void HaiDiLaoYueHeDiLaoYu()
        {
            if (Pai.Instance.LingShanKaiHuaPanDing())
            {
                return;
            }

            if (Pai.Instance.HaiDiPanDing())
            {
                if (jiJia)
                {
                    YiZhuiJia(YiDingYi.HaiDiLaoYue, 1);
                }
                else
                {
                    YiZhuiJia(YiDingYi.HeDiLaoYu, 1);
                }
            }
        }

        // 平和
        private void PingHe()
        {
            if (taJiaFuLuShu > 0)
            {
                return;
            }
            if (shunZis.Count < 4)
            {
                return;
            }
            if (!GuiZe.Instance.ziMoPingHe && jiJia)
            {
                // 自摸平和無し
                return;
            }
            int tou = duiZis[0].pais[0] & QUE_PAI;
            if (tou > 0x34 || tou == Chang.Instance.changFeng || tou == feng)
            {
                return;
            }
            int heLePai = shouPai[^1] & QUE_PAI;
            foreach (ShunZi shunZi in shunZis)
            {
                if ((shunZi.pais[0] == heLePai && (heLePai & SHU_PAI) != 7) || (shunZi.pais[2] == heLePai && (heLePai & SHU_PAI) != 3))
                {
                    YiZhuiJia(YiDingYi.PingHe, 1);
                    return;
                }
            }
        }

        // 断幺九
        private void DuanYaoJiu()
        {
            foreach (int sp in shouPai)
            {
                int p = sp & QUE_PAI;
                if (ZiPaiPanDing(p) || ((p & SHU_PAI) == 1) || ((p & SHU_PAI) == 9))
                {
                    return;
                }
            }
            if (!GuiZe.Instance.shiDuan && fuLuPais.Count > 0)
            {
                // 喰断無し
                return;
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int p = fp & QUE_PAI;
                    if (p == QUE_PAI)
                    {
                        break;
                    }
                    if (ZiPaiPanDing(p) || ((p & SHU_PAI) == 1) || ((p & SHU_PAI) == 9))
                    {
                        return;
                    }
                }
            }
            YiZhuiJia(YiDingYi.DuanYaoJiu, 1);
        }

        // 一盃口・二盃口
        private void YiBeiKouErBeiKou()
        {
            if (taJiaFuLuShu > 0)
            {
                return;
            }
            if (shunZis.Count < 2)
            {
                return;
            }
            int beiKou = 0;
            for (int i = 0; i < shunZis.Count - 1; i++)
            {
                for (int j = i + 1; j < shunZis.Count; j++)
                {
                    if (shunZis[i].pais[0] == shunZis[j].pais[0] && shunZis[i].pais[1] == shunZis[j].pais[1] && shunZis[i].pais[2] == shunZis[j].pais[2])
                    {
                        beiKou++;
                        break;
                    }
                }
            }

            if (beiKou == 2)
            {
                // 重複
                bool chongFu = false;
                for (int i = 0; i < shunZis.Count; i++)
                {
                    List<int> pais1 = shunZis[i].pais;
                    int shu = 0;
                    for (int j = 0; j < shunZis.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        List<int> pais2 = shunZis[j].pais;
                        if (pais1[0] == pais2[0])
                        {
                            shu++;
                        }
                    }
                    if (shu > 1)
                    {
                        chongFu = true;
                    }
                }
                if (chongFu)
                {
                    YiZhuiJia(YiDingYi.YiBeiKou, 1);
                }
                else
                {
                    YiZhuiJia(YiDingYi.ErBeiKou, 2);
                }
            }
            if (beiKou == 1)
            {
                YiZhuiJia(YiDingYi.YiBeiKou, 1);
            }
        }

        // 役牌
        private void YiPai()
        {
            int fan = 0;
            foreach (KeZi keZi in keZis)
            {
                int p = keZi.pais[0];
                if (p == Chang.Instance.changFeng)
                {
                    fan++;
                }
                if (p == feng)
                {
                    fan++;
                }
                if (p == 0x35 || p == 0x36 || p == 0x37)
                {
                    fan++;
                }
            }

            if (fan > 0)
            {
                YiZhuiJia(YiDingYi.YiPai, fan);
            }
        }

        // 三色同順
        private void SanSeTongShun()
        {
            if (shunZis.Count < 3)
            {
                return;
            }
            for (int i = 0; i < shunZis.Count; i++)
            {
                List<int> pais1 = shunZis[i].pais;
                bool[] se = new bool[3];
                for (int j = 0; j < shunZis.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    List<int> pais2 = shunZis[j].pais;
                    if ((pais1[0] & SHU_PAI) == (pais2[0] & SHU_PAI)
                        && (pais1[1] & SHU_PAI) == (pais2[1] & SHU_PAI)
                        && (pais1[2] & SHU_PAI) == (pais2[2] & SHU_PAI))
                    {
                        se[(pais1[0] & SE_PAI) >> 4] = true;
                        se[(pais2[0] & SE_PAI) >> 4] = true;
                    }
                }
                if (se[0] && se[1] && se[2])
                {
                    if (taJiaFuLuShu > 0)
                    {
                        YiZhuiJia(YiDingYi.SanSeTongShun, 1);
                    }
                    else
                    {
                        YiZhuiJia(YiDingYi.SanSeTongShun, 2);
                    }
                    return;
                }
            }
        }

        // 一気通貫
        private void YiQiTongGuan()
        {
            bool[][] xing = new bool[3][];
            for (int i = 0; i < xing.Length; i++)
            {
                xing[i] = new bool[3];
                for (int j = 0; j < xing[i].Length; j++)
                {
                    xing[i][j] = false;
                }
            }

            foreach (ShunZi shunZi in shunZis)
            {
                int s1 = shunZi.pais[0] & SHU_PAI;
                int s2 = shunZi.pais[1] & SHU_PAI;
                int s3 = shunZi.pais[2] & SHU_PAI;
                if (s1 == 1 && s2 == 2 && s3 == 3)
                {
                    xing[(shunZi.pais[0] & SE_PAI) >> 4][0] = true;
                }
                else if (s1 == 4 && s2 == 5 && s3 == 6)
                {
                    xing[(shunZi.pais[0] & SE_PAI) >> 4][1] = true;
                }
                else if (s1 == 7 && s2 == 8 && s3 == 9)
                {
                    xing[(shunZi.pais[0] & SE_PAI) >> 4][2] = true;
                }
            }
            for (int i = 0; i < xing.Length; i++)
            {
                if (xing[i][0] && xing[i][1] && xing[i][2])
                {
                    if (taJiaFuLuShu > 0)
                    {
                        YiZhuiJia(YiDingYi.YiQiTongGuan, 1);
                    }
                    else
                    {
                        YiZhuiJia(YiDingYi.YiQiTongGuan, 2);
                    }
                    return;
                }
            }
        }

        // 全帯幺・純全帯・混老頭
        private void QuanDaiYaoChunQuanDaiHunLaoTou()
        {
            int quan = 0;
            foreach (DuiZi duiZi in duiZis)
            {
                int p = duiZi.pais[0];
                if (ZiPaiPanDing(p))
                {
                    quan++;
                }
                else if ((p & SHU_PAI) == 1 || (p & SHU_PAI) == 9)
                {
                }
                else
                {
                    return;
                }
            }
            foreach (KeZi keZi in keZis)
            {
                int p = keZi.pais[0];
                if (ZiPaiPanDing(p))
                {
                    quan++;
                }
                else if ((p & SHU_PAI) == 1 || (p & SHU_PAI) == 9)
                {
                }
                else
                {
                    return;
                }
            }
            foreach (ShunZi shunZi in shunZis)
            {
                int shu = (shunZi.pais[0] & SHU_PAI) + (shunZi.pais[1] & SHU_PAI) + (shunZi.pais[2] & SHU_PAI);
                if (shu == (1 + 2 + 3) || shu == (7 + 8 + 9))
                {
                }
                else
                {
                    return;
                }
            }

            if (quan > 0)
            {
                if (shunZis.Count == 0)
                {
                    YiZhuiJia(YiDingYi.HunLaoTou, 2);
                }
                else if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(YiDingYi.QuanDaiYao, 1);
                }
                else
                {
                    YiZhuiJia(YiDingYi.QuanDaiYao, 2);
                }
            }
            else
            {
                if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(YiDingYi.ChunQuanDai, 2);
                }
                else
                {
                    YiZhuiJia(YiDingYi.ChunQuanDai, 3);
                }
            }
        }

        // 対々和
        private void DuiDuiHe()
        {
            if (keZis.Count == 4)
            {
                YiZhuiJia(YiDingYi.DuiDuiHe, 2);
            }
        }

        // 三暗刻・三槓子
        private void SanAnKeSanGangZi()
        {
            if (keZis.Count < 3)
            {
                return;
            }
            int anKe = 0;
            int gang = 0;
            foreach (KeZi keZi in keZis)
            {
                if (keZi.yao == YaoDingYi.Wu || keZi.yao == YaoDingYi.AnGang)
                {
                    anKe++;
                }
                if (keZi.yao == YaoDingYi.AnGang || keZi.yao == YaoDingYi.JiaGang || keZi.yao == YaoDingYi.DaMingGang)
                {
                    gang++;
                }
            }
            if (anKe == 3)
            {
                YiZhuiJia(YiDingYi.SanAnKe, 2);
            }
            if (gang == 3)
            {
                YiZhuiJia(YiDingYi.SanGangZi, 2);
            }
        }

        // 小三元
        private void XiaoSanYuan()
        {
            if (keZis.Count < 2)
            {
                return;
            }
            int yuan = 0;
            foreach (KeZi keZi in keZis)
            {
                int p = keZi.pais[0];
                if (p == 0x35 || p == 0x36 || p == 0x37)
                {
                    yuan++;
                }
            }
            int dui = 0;
            foreach (DuiZi duiZi in duiZis)
            {
                int p = duiZi.pais[0];
                if (p == 0x35 || p == 0x36 || p == 0x37)
                {
                    dui++;
                }
            }
            if (dui == 1 && yuan == 2)
            {
                YiZhuiJia(YiDingYi.XiaoSanYuan, 2);
            }
        }

        // 混一色・清一色
        private void HunYiSeQingYiSe()
        {
            int se = -1;
            bool hun = false;
            foreach (int sp in shouPai)
            {
                int p = sp & QUE_PAI;
                if (ZiPaiPanDing(p))
                {
                    hun = true;
                }
                else
                {
                    if (se == -1)
                    {
                        se = p & SE_PAI;
                    }
                    else
                    {
                        if (se != (p & SE_PAI))
                        {
                            return;
                        }
                    }
                }
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int p = fp & QUE_PAI;
                    if (p == QUE_PAI)
                    {
                        break;
                    }
                    if (ZiPaiPanDing(p))
                    {
                        hun = true;
                    }
                    else
                    {
                        if (se == -1)
                        {
                            se = p & SE_PAI;
                        }
                        else
                        {
                            if (se != (p & SE_PAI))
                            {
                                return;
                            }
                        }
                    }
                }
            }
            if (hun)
            {
                if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(YiDingYi.HunYiSe, 2);
                }
                else
                {
                    YiZhuiJia(YiDingYi.HunYiSe, 3);
                }
            }
            else
            {
                if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(YiDingYi.QingYiSe, 5);
                }
                else
                {
                    YiZhuiJia(YiDingYi.QingYiSe, 6);
                }
            }
        }

        // 七対子
        private void QiDuiZi()
        {
            if (duiZis.Count == 7)
            {
                YiZhuiJia(YiDingYi.QiDuiZi, 2);
            }
        }

        // 流し満貫
        private void LiuManGuan()
        {
            foreach (ShePai shePai in shePais)
            {
                if (shePai.yao != YaoDingYi.Wu && shePai.yao != YaoDingYi.LiZhi)
                {
                    return;
                }
                int p = shePai.pai & QUE_PAI;
                int s = p & SHU_PAI;
                if (!ZiPaiPanDing(p))
                {
                    if (s != 1 && s != 9)
                    {
                        return;
                    }
                }
            }

            YiZhuiJia(YiDingYi.LiuManGuan, 5);
        }

        // 懸賞牌
        private void XuanShangPai()
        {
            // 手牌数計算
            ShouPaiShuJiSuan(true);

            int xuan = 0;
            foreach (int xp in Pai.Instance.xuanShangPai)
            {
                xuan += shouPaiShu[Pai.XUAN_SHANG_PAI_DING_YI[xp & QUE_PAI]];
            }
            if (liZhi)
            {
                foreach (int lxp in Pai.Instance.liXuanShangPai)
                {
                    xuan += shouPaiShu[Pai.XUAN_SHANG_PAI_DING_YI[lxp & QUE_PAI]];
                }
            }
            foreach (int sp in shouPai)
            {
                if ((sp & CHI_PAI) == CHI_PAI)
                {
                    xuan += 1;
                }
            }
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    int p = fp;
                    if ((p & CHI_PAI) == CHI_PAI)
                    {
                        xuan += 1;
                    }
                }
            }
            if (xuan > 0)
            {
                YiZhuiJia(YiDingYi.XuanShang, xuan);
            }
        }

        // 三連刻
        private void SanLianKe()
        {
            if (!GuiZe.Instance.sanLianKe)
            {
                return;
            }
            if (keZis.Count < 3)
            {
                return;
            }
            for (int i = 0; i < keZis.Count - 2; i++)
            {
                if (ZiPaiPanDing(keZis[i].pais[0]))
                {
                    continue;
                }
                List<int> k = new()
                {
                    keZis[i].pais[0],
                    keZis[i + 1].pais[0],
                    keZis[i + 2].pais[0]
                };
                k.Sort();
                if ((k[0] + 1 == k[1]) && (k[0] + 2 == k[2]))
                {
                    YiZhuiJia(YiDingYi.SanLianKe, 2);
                }
            }
        }

        // 役追加
        private void YiZhuiJia(YiManDingYi ming, int fan)
        {
            yiFans.Add(new YiFan { yi = (int)ming, fanShu = fan });
        }
        private void YiZhuiJia(YiDingYi ming, int fan)
        {
            yiFans.Add(new YiFan { yi = (int)ming, fanShu = fan });
        }

        // 対子計算
        protected void DuiZiJiSuan(int p, bool quanXiao = true)
        {
            while (shouPaiShu[p] >= 2)
            {
                shouPaiShu[p] -= 2;

                duiZis.Add(new DuiZi { pais = new List<int>() { p, p } });
                if (!quanXiao)
                {
                    break;
                }
            }
        }

        // 刻子計算
        protected void KeZiJiSuan(int p)
        {
            if (shouPaiShu[p] >= 3)
            {
                shouPaiShu[p] -= 3;

                keZis.Add(new KeZi { pais = new List<int>() { p, p, p }, yao = YaoDingYi.Wu });
            }
        }

        // 順子計算
        protected void ShunZiJiSuan(int p)
        {
            if (ZiPaiPanDing(p))
            {
                return;
            }

            int s = p & SHU_PAI;
            if (s <= 7)
            {
                while (shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1 && shouPaiShu[p + 2] >= 1)
                {
                    shouPaiShu[p]--;
                    shouPaiShu[p + 1]--;
                    shouPaiShu[p + 2]--;

                    shunZis.Add(new ShunZi { pais = new List<int>() { p, p + 1, p + 2 } });
                }
            }
            if (s >= 2 && s <= 8)
            {
                while (shouPaiShu[p - 1] >= 1 && shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1)
                {
                    shouPaiShu[p - 1]--;
                    shouPaiShu[p]--;
                    shouPaiShu[p + 1]--;

                    shunZis.Add(new ShunZi { pais = new List<int>() { p - 1, p, p + 1 } });
                }
            }
            if (s >= 3)
            {
                while (shouPaiShu[p - 2] >= 1 && shouPaiShu[p - 1] >= 1 && shouPaiShu[p] >= 1)
                {
                    shouPaiShu[p - 2]--;
                    shouPaiShu[p - 1]--;
                    shouPaiShu[p]--;

                    shunZis.Add(new ShunZi { pais = new List<int>() { p - 2, p - 1, p } });
                }
            }
        }

        // 塔子計算
        protected void TaZiJiSuan(int p)
        {
            if (ZiPaiPanDing(p))
            {
                return;
            }

            while (shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1)
            {
                shouPaiShu[p]--;
                shouPaiShu[p + 1]--;

                taZis.Add(new List<int>() { p, p + 1 });
            }
            while (shouPaiShu[p] >= 1 && shouPaiShu[p + 2] >= 1)
            {
                shouPaiShu[p]--;
                shouPaiShu[p + 2]--;

                taZis.Add(new List<int>() { p, p + 2 });
            }
        }

        // 副露計算
        private void FuLuJiSuan()
        {
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                if (fuLuPai.yao == YaoDingYi.Chi)
                {
                    List<int> s = new();
                    foreach (int fp in fuLuPai.pais)
                    {
                        s.Add(fp & QUE_PAI);
                    }
                    s.Sort();
                    shunZis.Add(new ShunZi { pais = s });
                }
                else
                {
                    List<int> k = new();
                    foreach (int fp in fuLuPai.pais)
                    {
                        k.Add(fp & QUE_PAI);
                    }
                    keZis.Add(new KeZi { pais = k, yao = fuLuPai.yao });
                }
            }

            int heLePai = shouPai[^1] & QUE_PAI;
            foreach (DuiZi duiZi in duiZis)
            {
                if (duiZi.pais[0] == heLePai)
                {
                    return;
                }
            }
            foreach (ShunZi shunZi in shunZis)
            {
                for (int j = 0; j < shunZi.pais.Count; j++)
                {
                    if (shunZi.pais[j] == heLePai)
                    {
                        return;
                    }
                }
            }
            for (int i = 0; i < keZis.Count; i++)
            {
                if (keZis[i].yao == YaoDingYi.Wu && keZis[i].pais[0] == heLePai && !jiJia)
                {
                    keZis[i].pais = keZis[i].pais;
                    keZis[i].yao = YaoDingYi.Bing;
                    return;
                }
            }
        }

        // 手牌数計算
        protected void ShouPaiShuJiSuan(bool fuLu = false)
        {
            Array.Fill(shouPaiShu, 0);
            foreach (int sp in shouPai)
            {
                shouPaiShu[sp & QUE_PAI]++;
            }

            if (fuLu)
            {
                // 副露牌加算
                foreach (FuLuPai fuLuPai in fuLuPais)
                {
                    foreach (int fp in fuLuPai.pais)
                    {
                        shouPaiShu[fp & QUE_PAI]++;
                    }
                }
            }
        }

        // 公開牌数計算
        public void GongKaiPaiShuJiSuan()
        {
            Array.Fill(gongKaiPaiShu, 0);
            foreach (QueShi shi in MaQue.Instance.queShis)
            {
                if (shi.kaiLiZhi && shi.feng != feng)
                {
                    // 開立直の場合、手牌
                    foreach (int sp in shi.shouPai)
                    {
                        int p = sp & QUE_PAI;
                        gongKaiPaiShu[p]++;
                    }
                }
                // 捨牌
                foreach (ShePai shePai in shi.shePais)
                {
                    int p = shePai.pai & QUE_PAI;
                    gongKaiPaiShu[p]++;
                }
                // 副露牌
                for (int i = 0; i < shi.fuLuPais.Count; i++)
                {
                    FuLuPai fuLuPai = shi.fuLuPais[i];
                    int wei = 0;
                    foreach (int fp in fuLuPai.pais)
                    {
                        int p = fp & QUE_PAI;
                        if (fuLuPai.yao == YaoDingYi.JiaGang && i == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(fuLuPai.yao, fuLuPai.jia, wei);
                        if (!isMingPai)
                        {
                            gongKaiPaiShu[p]++;
                        }
                        wei++;
                    }
                }
            }
            // 懸賞牌
            foreach (int pai in Pai.Instance.xuanShangPai)
            {
                gongKaiPaiShu[pai & QUE_PAI]++;
            }
            // 手牌
            foreach (int pai in shouPai)
            {
                gongKaiPaiShu[pai & QUE_PAI]++;
            }
        }

        // 副露牌数計算
        protected void FuLuPaiShuJiSuan()
        {
            Array.Fill(fuLuPaiShu, 0);
            foreach (FuLuPai fuLuPai in fuLuPais)
            {
                foreach (int fp in fuLuPai.pais)
                {
                    fuLuPaiShu[fp & QUE_PAI]++;
                }
            }
        }

        // 錯和自家判定
        public bool CuHeZiJiaPanDing()
        {
            if (ziJiaXuanZe < 0)
            {
                cuHeSheng = "打牌選択間違い";
                return true;
            }

            if (liZhi)
            {
                switch (ziJiaYao)
                {
                    case YaoDingYi.Wu:
                        // 無
                        if (ziJiaXuanZe != shouPai.Count - 1)
                        {
                            cuHeSheng = "立直後打牌手出し";
                            return true;
                        }
                        break;
                    case YaoDingYi.JiaGang:
                        // 加槓
                        if (jiaGangPaiWei.Count <= ziJiaXuanZe)
                        {
                            cuHeSheng = "立直後加槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        if (anGangPaiWei.Count <= ziJiaXuanZe)
                        {
                            cuHeSheng = "立直後暗槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.ZiMo:
                        // 自摸
                        if (!heLe)
                        {
                            cuHeSheng = "立直後誤自摸";
                            return true;
                        }
                        break;
                    default:
                        cuHeSheng = "立直後腰間違い";
                        return true;
                }

            }
            else
            {
                switch (ziJiaYao)
                {
                    case YaoDingYi.Wu:
                        // 無
                        if (ziJiaXuanZe > shouPai.Count - 1)
                        {
                            cuHeSheng = "打牌選択間違い";
                            return true;
                        }
                        break;
                    case YaoDingYi.JiaGang:
                        // 加槓
                        if (jiaGangPaiWei.Count <= ziJiaXuanZe)
                        {
                            cuHeSheng = "加槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        if (anGangPaiWei.Count <= ziJiaXuanZe)
                        {
                            cuHeSheng = "暗槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.LiZhi:
                        // 立直
                        if (taJiaFuLuShu > 0)
                        {
                            cuHeSheng = "立直不可";
                            return true;
                        }
                        if (!GuiZe.Instance.kaiLiZhi && kaiLiZhi)
                        {
                            cuHeSheng = "開立直不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.ZiMo:
                        // 自摸
                        if (!heLe)
                        {
                            cuHeSheng = "誤自摸";
                            return true;
                        }
                        break;
                    case YaoDingYi.JiuZhongJiuPai:
                        // 九種九牌
                        if (!jiuZhongJiuPai || GuiZe.Instance.jiuZhongJiuPaiLianZhuang == 0)
                        {
                            cuHeSheng = "誤九種九牌";
                            return true;
                        }
                        break;
                    case YaoDingYi.KaiLiZhi:
                        if (!GuiZe.Instance.kaiLiZhi)
                        {
                            cuHeSheng = "開立直不可";
                            return true;
                        }
                        break;
                    default:
                        cuHeSheng = "腰間違い";
                        return true;
                }
            }

            foreach (int stp in shiTiPai)
            {
                if (stp == (shouPai[ziJiaXuanZe] & QUE_PAI))
                {
                    cuHeSheng = "食い替え不可";
                    DaPai(shouPai[ziJiaXuanZe]);
                    return true;
                }
            }
            return false;
        }

        // 錯和他家判定
        public bool CuHeTaJiaPanDing()
        {
            if (taJiaXuanZe < 0)
            {
                cuHeSheng = "腰選択間違い";
                return true;
            }

            switch (taJiaYao)
            {
                case YaoDingYi.Wu:
                    // 無
                    break;

                case YaoDingYi.Chi:
                    // 吃
                    if (chiPaiWei.Count <= taJiaXuanZe)
                    {
                        cuHeSheng = "吃不可";
                        return true;
                    }
                    break;
                case YaoDingYi.Bing:
                    // 石並
                    if (bingPaiWei.Count <= taJiaXuanZe)
                    {
                        cuHeSheng = "石並不可";
                        return true;
                    }
                    break;
                case YaoDingYi.DaMingGang:
                    // 大明槓
                    if (daMingGangPaiWei.Count <= taJiaXuanZe)
                    {
                        cuHeSheng = "大明槓不可";
                        return true;
                    }
                    break;
                case YaoDingYi.RongHe:
                    // 栄和
                    if (!heLe)
                    {
                        cuHeSheng = "誤栄和";
                        return true;
                    }
                    if (ZhenTingPanDing())
                    {
                        cuHeSheng = "振聴";
                        return true;
                    }
                    break;
                default:
                    cuHeSheng = "腰間違い";
                    return true;
            }

            return false;
        }
    }

    [Serializable]
    public class YiFan
    {
        public int yi;
        public int fanShu;
    }
    [Serializable]
    public class FuLuPai
    {
        public List<int> pais;
        public int jia;
        public QueShi.YaoDingYi yao;
    }
    [Serializable]
    public class ShePai
    {
        public int pai;
        public QueShi.YaoDingYi yao;
        public bool ziMoQie;
    }
    [Serializable]
    public class GoFuLuPai
    {
        public Button[] goFuLuPai;
    }
    [Serializable]
    public class DuiZi
    {
        public List<int> pais;
    }
    [Serializable]
    public class KeZi
    {
        public List<int> pais;
        public QueShi.YaoDingYi yao;
    }
    [Serializable]
    public class ShunZi
    {
        public List<int> pais;
    }
    [Serializable]
    public class Nao
    {
        public QueShi.XingGe xingGe;
        public int score;
    }
}

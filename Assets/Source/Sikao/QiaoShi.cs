using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

using Assets.Source.Gongtong;

namespace Assets.Source.Sikao
{
    // 雀士
    internal abstract class QiaoShi
    {
        // 牌
        internal const int QIAO_PAI = 0x3f;
        // 数牌
        internal const int SHU_PAI = 0x0f;
        // 牌色
        internal const int SE_PAI = 0x30;
        // 字牌
        internal const int ZI_PAI = 0x30;
        // 赤牌
        internal const int CHI_PAI = 0x40;

        // 【腰】
        internal enum YaoDingYi
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
        internal static string YaoMing(YaoDingYi yao)
        {
            return YaoMingDingYi[yao].yao;
        }
        // ボタン腰名
        internal string YaoMingButton(YaoDingYi yao)
        {
            if ((yao == YaoDingYi.JiaGang || yao == YaoDingYi.AnGang) && (AnGangPaiWei.Count == 0 || JiaGangPaiWei.Count == 0))
            {
                // 加槓、暗槓で片方のみ可能な場合、ボタン名は「カン」
                yao = YaoDingYi.DaMingGang;
            }
            return YaoMingDingYi[yao].button;
        }

        // 役満
        internal enum YiManDingYi
        {
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
            GongKongQiao,
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
        internal static readonly Dictionary<YiManDingYi, string> YiManMing = new()
        {
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
            { YiManDingYi.GongKongQiao, "紅孔雀" },
            { YiManDingYi.BaiWanShi, "百万石" },
            { YiManDingYi.ChunZhengBaiWanShi, "純正百万石" },
            { YiManDingYi.ShiSanBuTa, "十三不塔" },
            { YiManDingYi.BaLianZhuang, "八連荘" },
        };

        // 役
        internal enum YiDingYi
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
        internal static readonly Dictionary<YiDingYi, string> YiMing = new()
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
        internal static readonly string[] DeDianYi = new string[] {
            "", "", "", "", "", "満貫", "跳満", "跳満", "倍満", "倍満", "倍満", "三倍満", "三倍満", "役満"
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
        internal string MingQian { get; set; } = "";
        // 自家思考結果
        internal YaoDingYi ZiJiaYao { get; set; }
        // 自家選択
        internal int ZiJiaXuanZe { get; set; }
        // 他家思考結果
        internal YaoDingYi TaJiaYao { get; set; }
        // 他家選択
        internal int TaJiaXuanZe { get; set; }

        // プレイヤー
        internal bool Player { get; set; } = false;
        // プレイヤー順(プレイヤーが必ず0となり、そこから順番にふられる)
        internal int PlayOrder { get; set; } = -1;
        // 理牌
        internal bool LiPaiDongZuo { get; set; } = true;
        // 外部思考
        internal bool WaiBuSikao { get; set; } = false;
        // 非同期停止
        internal bool AsyncStop { get; set; } = false;
        // フォロー
        internal bool Follow { get; set; } = false;
        // 記録
        internal Maqiao.JiLu JiLu { get; set; }

        // 役満
        internal bool YiMan { get; private set; }
        // 役・飜(役、飜数)
        internal List<(int yi, int fanShu)> YiFan { get; private set; }
        // 飜数計
        internal int FanShuJi { get; private set; }
        // 符
        internal int Fu { get; private set; }
        // 和了点
        internal int HeLeDian { get; private set; }
        // 点棒
        internal int DianBang { get; set; }
        // 集計点
        internal int JiJiDian { get; set; }
        // 風
        internal int Feng { get; set; }
        // 手牌
        internal List<int> ShouPai { get; set; }
        internal Button[] goShouPai;
        // 副露牌(牌、家、腰)
        internal List<(List<int> pais, int jia, YaoDingYi yao)> FuLuPai { get; set; }
        internal Button[][] goFuLuPai;
        // 包則番
        internal int BaoZeFan { get; set; }
        // 他家副露数
        internal int TaJiaFuLuShu { get; set; }
        // 捨牌(牌、腰、自摸切)
        internal List<(int pai, YaoDingYi yao, bool ziMoQie)> ShePai { get; set; }
        internal Button[] goShePai = new Button[0x30];
        // 立直位
        internal int LiZhiWei { get; set; }
        // 同順牌
        private List<int> tongShunPai = new();
        // 立直後牌
        private List<int> liZhiHouPai;
        // 待牌
        internal List<int> DaiPai { get; set; }
        internal Button[] goDaiPai = new Button[13];
        // 残牌数
        internal TextMeshProUGUI[] goCanPaiShu = new TextMeshProUGUI[13];
        // 有効牌数
        internal List<int> YouXiaoPaiShu { get; set; }
        // 向聴数
        internal int XiangTingShu { get; private set; }
        internal TextMeshProUGUI goXiangTingShu;
        // 対子
        private List<List<int>> duiZi;
        // 刻子
        private List<(List<int> pais, YaoDingYi yao)> keZi;
        // 順子
        private List<List<int>> shunZi;
        // 塔子
        private List<List<int>> taZi;
        // 手牌数
        internal int[] ShouPaiShu { get; set; } = new int[0x40];
        // 副露牌数
        internal int[] FuLuPaiShu { get; set; } = new int[0x40];
        // 捨牌数
        internal int[] ShePaiShu { get; set; } = new int[0x40];
        // 立直後捨牌数
        internal int[] LiZhiShePaiShu { get; set; } = new int[0x40];
        // 公開牌数
        internal int[] GongKaiPaiShu { get; set; } = new int[0x40];
        // 和了
        internal bool HeLe { get; set; }
        // 立直
        internal bool LiZhi { get; set; }
        // 開立直
        internal bool KaiLiZhi { get; set; }
        // W立直
        private bool wLiZhi;
        // 一発
        internal bool YiFa { get; set; }
        // 一巡目
        private bool yiXunMu;
        // 副露順
        private bool fuLuShun;
        // 形聴
        internal bool XingTing { get; private set; }
        // 終了
        internal bool ZhongLiao { get; set; }
        // 自家
        internal bool JiJia { get; set; }
        // 和了牌(牌、位、予想点)
        internal List<(List<int> pais, int wei, int[] yuXiangDian)> HeLePai { get; set; }
        // 立直牌位
        internal List<int> LiZhiPaiWei { get; set; }
        // 暗槓牌位
        internal List<List<int>> AnGangPaiWei { get; set; }
        // 加槓牌位
        internal List<List<int>> JiaGangPaiWei { get; set; }
        // 大明槓牌位
        internal List<List<int>> DaMingGangPaiWei { get; set; }
        // 石並牌位
        internal List<List<int>> BingPaiWei { get; set; }
        // 吃牌位
        internal List<List<int>> ChiPaiWei { get; set; }
        // 九種九牌
        internal bool JiuZhongJiuPai { get; set; }
        // 受取
        internal int ShouQu { get; set; }
        // 受取(供託)
        internal int ShouQuGongTuo { get; set; }
        // 錯和声
        internal string CuHeSheng { get; set; }
        // 食替牌
        internal List<int> ShiTiPai { get; set; } = new();
        // 手牌点数
        internal int[] ShouPaiDian { get; set; }
        // 連荘数
        internal int LianZhuangShu { get; set; }

        // 打牌後
        internal bool DaPaiHou { get; set; }

        // 流し満貫
        internal bool LiuShiManGuan { get; set; }
        // 点数（表示用）
        internal int ShuBiao { get; set; }

        // 遷移(自家)
        protected Transition transitionZiJia;
        internal void SetTransitionZiJiaState(State state)
        {
            transitionZiJia = new()
            {
                mingQian = MingQian,
                state = state,
            };
        }
        internal void SetTransitionZiJiaAction(List<int> action)
        {
            transitionZiJia.action = action;
        }
        internal void SetTransitionZiJiaNextState(State state)
        {
            transitionZiJia.nextState = state;
            TransitionZiJiaList ??= new();
            TransitionZiJiaList.Add(transitionZiJia);
        }
        internal void SetTransitionZiJiaReward(int score)
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
        internal List<Transition> TransitionZiJiaList { get; set; }
        // 遷移(他家)
        protected Transition transitionTaJia;
        internal void SetTransitionTaJiaState(State state)
        {
            transitionTaJia = new()
            {
                mingQian = MingQian,
                state = state,
                action = new() { (int)YaoDingYi.Wu, 0 },
            };
        }
        internal void SetTransitionTaJiaAction(List<int> action)
        {
            if (transitionTaJia == null)
            {
                return;
            }
            transitionTaJia.action = action;
            TransitionTaJiaList ??= new();
            TransitionTaJiaList.Add(transitionTaJia);
        }
        internal List<Transition> TransitionTaJiaList { get; set; }

        // コンストラクタ
        internal QiaoShi()
        {
            goShouPai = new Button[14];
            goFuLuPai = new Button[4][];
            for (int i = 0; i < goFuLuPai.Length; i++)
            {
                goFuLuPai[i] = new Button[4];
            }
            ShouPaiDian = new int[14];
        }

        // コンストラクタ
        internal QiaoShi(string mingQian) : this()
        {
            MingQian = mingQian;
        }

        // 状態
        internal void ZhuangTai(State state, bool ziJia)
        {
            if (ziJia)
            {
                // 手牌数
                ShouPaiShuJiSuan();
                state.shouPaiShu = new(ShouPaiShu);
                // 副露牌
                FuLuPaiShuJiSuan();
                state.fuLuPaiShu = new(FuLuPaiShu);
                // 立直
                state.liZhi = LiZhi;
                // 向聴数
                state.xiangTingShu = XiangTingShu;
            }
            // 捨牌
            List<int> sp = new();
            for (int i = 0; i < 0x30; i++)
            {
                if (i < ShePai.Count)
                {
                    (int pai, YaoDingYi _, bool _) = ShePai[i];
                    sp.Add(pai);
                }
                else
                {
                    sp.Add(0);
                }
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
        internal abstract void SiKaoZiJia();
        internal abstract IEnumerator SiKaoZiJiaCoroutine();

        // 思考他家
        internal abstract void SiKaoTaJia();
        internal abstract IEnumerator SiKaoTaJiaCoroutine();

        // 初期化
        protected static void Init(int[] list, int value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = value;
            }
        }

        // 荘初期化
        internal void ZhuangChuQiHua()
        {
            DianBang = Chang.guiZe.kaiShiDian;
            JiJiDian = 0;
        }

        // 局初期化
        internal void JuChuQiHua(int feng)
        {
            Feng = feng;
            ShouPai = new();
            FuLuPai = new();
            BaoZeFan = -1;
            TaJiaFuLuShu = 0;
            ShePai = new();
            Init(ShePaiShu, 0);
            Init(LiZhiShePaiShu, 0);
            liZhiHouPai = new();
            LiZhiWei = -1;
            DaiPai = new();
            YouXiaoPaiShu = new();
            XiangTingShu = 0;
            HeLePai = new();

            LiZhi = false;
            KaiLiZhi = false;
            wLiZhi = false;
            YiFa = false;
            yiXunMu = true;
            fuLuShun = false;

            ShouQu = 0;
            ShouQuGongTuo = 0;
            CuHeSheng = "";
            DaPaiHou = true;
            LiuShiManGuan = false;
            ZhongLiao = false;

            ZiJiaYao = YaoDingYi.Wu;
            TaJiaYao = YaoDingYi.Wu;
        }

        // 思考自家判定
        internal void SiKaoZiJiaPanDing()
        {
            JiJia = true;

            ZiJiaYao = YaoDingYi.Wu;
            ZiJiaXuanZe = ShouPai.Count - 1;
            TaJiaYao = YaoDingYi.Wu;
            TaJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            // 九種九牌判定
            JiuZhongJiuPaiPanDing();
            // 十三不塔判定
            if (ShiSanBuTaPanDing())
            {
                HeLe = true;
            }

            // 食替牌判定
            ShiTiPaiPanDing();

            // 和了判定
            if (HeLePanDing() == Ting.TingPai)
            {
                HeLe = true;
            }
            // 聴牌判定
            TingPaiPanDing();

            if (Pai.HaiDiPanDing())
            {
                return;
            }

            if (Pai.XuanShangPaiShu() <= 4 && Pai.CanShanPaiShu() >= Chang.QiaoShis.Count)
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
        internal void SiKaoTaJiaPanDing(int jia)
        {
            JiJia = false;

            TaJiaYao = YaoDingYi.Wu;
            TaJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            ShouPai.Add(Chang.ShePai);
            // 和了判定
            if (HeLePanDing() == Ting.TingPai)
            {
                // 振聴判定
                if (!ZhenTingPanDing())
                {
                    HeLe = true;
                }
            }
            ShouPai.RemoveAt(ShouPai.Count - 1);

            if (LiZhi || wLiZhi)
            {
                return;
            }
            if (Pai.QiangGangPanDing())
            {
                return;
            }
            if (Pai.HaiDiPanDing())
            {
                return;
            }

            if (Pai.XuanShangPaiShu() <= 4)
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
        internal void LiPai()
        {
            if (!LiPaiDongZuo)
            {
                return;
            }
            Sort();
        }
        internal void Sort()
        {
            ShouPai.Sort((p1, p2) => (p1 & QIAO_PAI).CompareTo(p2 & QIAO_PAI));
        }

        // 自摸
        internal void ZiMo(int p, bool daPaiHou = false)
        {
            ShouPai.Add(p);
            DaPaiHou = daPaiHou;
        }

        // 打牌前
        internal int DaPaiQian()
        {
            int p = ShouPai[Chang.ZiJiaXuanZe];
            ShouPai[Chang.ZiJiaXuanZe] = 0xff;
            return p;
        }
        // 打牌
        internal void DaPai(int p)
        {
            Chang.ShePai = p;
            ShePaiShu[p & QIAO_PAI]++;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                if (i != Chang.ZiMoFan && shi.LiZhi)
                {
                    shi.LiZhiShePaiShu[Chang.ShePai & QIAO_PAI]++;
                }
            }
            ShouPai.RemoveAt(Chang.ZiJiaXuanZe);
            bool ziMoQie = false;
            if (ZiJiaXuanZe == ShouPai.Count)
            {
                if (ZiJiaYao == YaoDingYi.Wu || ZiJiaYao == YaoDingYi.LiZhi)
                {
                    ziMoQie = true;
                }
            }
            ShePai.Add((Chang.ShePai, YaoDingYi.Wu, ziMoQie));
            tongShunPai = new List<int>();

            fuLuShun = false;
            DaPaiHou = true;
        }

        // 待牌計算
        internal void DaiPaiJiSuan(int ziJiaXuanZe)
        {
            DaiPai = new List<int>();
            foreach ((List<int> pais, int wei, _) in HeLePai)
            {
                if (wei == ziJiaXuanZe)
                {
                    foreach (int hp in pais)
                    {
                        DaiPai.Add(hp);
                    }
                }
            }
        }

        // 待牌・向聴数計算
        internal void DaiPaiXiangTingShuJiSuan(int xuanZe)
        {
            DaiPaiJiSuan(xuanZe);
            GongKaiPaiShuJiSuan();
            XiangTingShuJiSuan(xuanZe);
        }

        // 和了処理
        internal void HeLeChuLi()
        {
            // 和了判定
            HeLePanDing();
            // 点計算
            DianJiSuan();
            if (YiMan || FanShuJi >= 5)
            {
                Fu = 0;
            }
        }

        // 立直処理
        internal void LiZhiChuLi()
        {
            if (ShePai.Count == 0)
            {
                wLiZhi = true;
            }
            LiZhiWei = ShePai.Count;
            LiZhi = true;
            YiFa = true;
        }

        // 開立直処理
        internal void KaiLiZhiChuLi()
        {
            if (ZiJiaYao == YaoDingYi.KaiLiZhi)
            {
                KaiLiZhi = true;
                ZiJiaYao = YaoDingYi.LiZhi;
            }
        }

        // 暗槓
        internal void AnGang(int wei)
        {
            List<int> pais = new();
            foreach (int w in AnGangPaiWei[wei])
            {
                pais.Add(ShouPai[w]);
            }
            FuLuPai.Add((pais, 0, YaoDingYi.AnGang));

            List<int> paiWei = new(AnGangPaiWei[wei]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                ShouPai.RemoveAt(w);
            }

            // 理牌
            LiPai();
        }

        // 加槓
        internal void JiaGang(int wei)
        {
            for (int i = 0; i < FuLuPai.Count; i++)
            {
                (List<int> pais, int jia, YaoDingYi yao) = FuLuPai[i];
                if (yao == YaoDingYi.Bing)
                {
                    if ((pais[0] & QIAO_PAI) == (ShouPai[JiaGangPaiWei[wei][0]] & QIAO_PAI))
                    {
                        FuLuPai[i] = (new() { pais[0], pais[1], pais[2], ShouPai[JiaGangPaiWei[wei][0]] }, jia, YaoDingYi.JiaGang);
                        Chang.ShePai = ShouPai[JiaGangPaiWei[wei][0]];
                        ShouPai.RemoveAt(JiaGangPaiWei[wei][0]);
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
            int mingFan = Chang.MingFan;
            int ziMoFan = Chang.ZiMoFan;
            if (mingFan < ziMoFan)
            {
                mingFan += Chang.QiaoShis.Count;
            }
            return mingFan - ziMoFan;
        }

        // 包則判定
        private void BaoZePanDing()
        {
            if (!Chang.guiZe.baoZe)
            {
                return;
            }
            if (BaoZeFan >= 0)
            {
                return;
            }

            // 大四喜、大三元、四槓子
            int fengShu = 0;
            int sanYuanShu = 0;
            int gangShu = 0;
            foreach ((List<int> pais, _, YaoDingYi yao) in FuLuPai)
            {
                if (yao == YaoDingYi.DaMingGang || yao == YaoDingYi.Bing)
                {
                    int p = pais[0] & QIAO_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        fengShu++;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        sanYuanShu++;
                    }
                }
                if (yao == YaoDingYi.AnGang || yao == YaoDingYi.JiaGang || yao == YaoDingYi.DaMingGang)
                {
                    gangShu++;
                }
            }
            if (fengShu == 4 || sanYuanShu == 3 || gangShu == 4)
            {
                (List<int> pais, int jia, YaoDingYi yao) = FuLuPai[^1];
                if (yao == YaoDingYi.DaMingGang || yao == YaoDingYi.Bing)
                {
                    int p = pais[0] & QIAO_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        BaoZeFan = jia;
                        return;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        BaoZeFan = jia;
                        return;
                    }
                    if (yao == YaoDingYi.AnGang || yao == YaoDingYi.JiaGang || yao == YaoDingYi.DaMingGang)
                    {
                        BaoZeFan = jia;
                        return;
                    }
                }
            }
        }

        // 大明槓
        internal void DaMingGang()
        {
            List<int> pais = new()
            {
                Chang.ShePai
            };
            foreach (int w in DaMingGangPaiWei[Chang.TaJiaXuanZe])
            {
                pais.Add(ShouPai[w]);
            }
            FuLuPai.Add((pais, FuLuJiaJiSuan(), YaoDingYi.DaMingGang));

            List<int> paiWei = new(DaMingGangPaiWei[Chang.TaJiaXuanZe]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                ShouPai.RemoveAt(w);
            }

            TaJiaFuLuShu++;
            // 理牌
            LiPai();

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 石並
        internal void Bing()
        {
            List<int> pais = new()
            {
                Chang.ShePai
            };
            foreach (int w in BingPaiWei[Chang.TaJiaXuanZe])
            {
                pais.Add(ShouPai[w]);
            }
            FuLuPai.Add((pais, FuLuJiaJiSuan(), YaoDingYi.Bing));

            List<int> paiWei = new(BingPaiWei[Chang.TaJiaXuanZe]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                ShouPai.RemoveAt(w);
            }

            TaJiaFuLuShu++;
            // 理牌
            LiPai();

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 吃
        internal void Chi()
        {
            List<int> pais = new()
            {
                Chang.ShePai
            };
            foreach (int w in ChiPaiWei[Chang.TaJiaXuanZe])
            {
                pais.Add(ShouPai[w]);
            }
            FuLuPai.Add((pais, FuLuJiaJiSuan(), YaoDingYi.Chi));

            List<int> paiWei = new(ChiPaiWei[Chang.TaJiaXuanZe]);
            paiWei.Sort();
            paiWei.Reverse();
            foreach (int w in paiWei)
            {
                ShouPai.RemoveAt(w);
            }

            TaJiaFuLuShu++;
            // 理牌
            LiPai();

            fuLuShun = true;
        }

        // 振聴牌処理
        internal void ZhenTingPaiChuLi()
        {
            tongShunPai.Add(Chang.ShePai);
            if (LiZhi)
            {
                liZhiHouPai.Add(Chang.ShePai);
            }
        }

        // 捨牌処理
        internal void ShePaiChuLi(YaoDingYi yao)
        {
            (int pai, YaoDingYi yao, bool ziMoQue) sp = ShePai[^1];
            sp.yao = yao;
            ShePai[^1] = sp;
            DaPaiHou = true;
        }

        // 立直処理
        internal void LiZiChuLi()
        {
            DianBangJiSuan(-1000, false);
        }

        // 鳴処理
        internal void MingChuLi()
        {
            if (Follow)
            {
                DaPaiHou = false;
            }
        }

        // 流局
        internal bool LiuJu()
        {
            YiMan = false;

            YiFan = new();
            FanShuJi = 0;

            HeLeDian = 0;

            // 流し満貫
            LiuManGuan();

            // 点計算
            if (YiFan.Count > 0)
            {
                FanShuJi = 1;
                if (Feng == 0x31)
                {
                    HeLeDian = 12000;
                }
                else
                {
                    HeLeDian = 8000;
                }
                return true;
            }
            return false;
        }

        // 消
        internal void Xiao()
        {
            YiFa = false;
            yiXunMu = false;
        }

        // 形聴判定
        internal void XingTingPanDing()
        {
            XingTing = false;

            foreach (int p in Pai.QiaoPai)
            {
                ShouPai.Add(p);
                if (HeLePanDing() >= Ting.XingTing)
                {
                    XingTing = true;
                    ShouPai.RemoveAt(ShouPai.Count - 1);
                    return;
                }
                ShouPai.RemoveAt(ShouPai.Count - 1);
            }
        }

        // 点棒計算
        internal void DianBangJiSuan(int dian)
        {
            DianBangJiSuan(dian, true);
        }
        internal void DianBangJiSuan(int dian, bool isShouQu)
        {
            DianBang += dian;
            if (isShouQu)
            {
                ShouQu += dian;
            }
        }

        internal void ShouQuGongTuoJiSuan(int dian)
        {
            ShouQuGongTuo = dian;
        }

        internal void JiJiDianJiSuan(int dian)
        {
            JiJiDian = dian;
        }

        // 思考前初期化
        internal void SiKaoQianChuQiHua()
        {
            HeLe = false;
            HeLePai = new();
            LiZhiPaiWei = new();
            if (JiJia)
            {
                AnGangPaiWei = new();
                JiaGangPaiWei = new();
                ShiTiPai = new();
            }
            else
            {
                DaMingGangPaiWei = new();
                BingPaiWei = new();
                ChiPaiWei = new();
            }
            JiuZhongJiuPai = false;

            Init(ShouPaiDian, 0);
        }

        // 字牌判定
        protected bool ZiPaiPanDing(int pai)
        {
            int p = pai & QIAO_PAI;
            if (p > ZI_PAI)
            {
                return true;
            }
            return false;
        }

        // 幺九牌判定
        protected bool YaoJiuPaiPanDing(int pai)
        {
            int p = pai & QIAO_PAI;
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
        internal int XuanShangPaiPanDing(int pai)
        {
            int p = pai & QIAO_PAI;
            int xuan = 0;
            foreach (int x in Pai.XuanShangPai)
            {
                if (Pai.XuanShangPaiDingYi[x & QIAO_PAI] == p)
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
            int p = pai & QIAO_PAI;
            if (p == Chang.ChangFeng)
            {
                fan++;
            }
            if (p == Feng)
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
            Fu = 0;
            // 国士無双判定
            if (GuoShiWuShuangPanDing())
            {
                // 役満判定
                YiManPanDing();
                if (YiFan.Count > 0)
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
                if (YiFan.Count > 0)
                {
                    return Ting.TingPai;
                }
                // 役判定
                YiPanDing();
                if (YiFan.Count > 0)
                {
                    Fu = 25;
                    // 聴牌
                    return Ting.TingPai;
                }
            }

            List<(int, int)> maxYiFan = new();
            int maxFanShuJi = 0;
            int maxFu = 0;
            int maxYiShu = 0;
            // 不聴
            Ting ret = Ting.BuTing;
            foreach (int sp in ShouPai)
            {
                // 手牌数計算
                ShouPaiShuJiSuan();
                // 頭
                int t = sp & QIAO_PAI;
                if (ShouPaiShu[t] < 2)
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
                        foreach (int p in Pai.QiaoPai)
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
                    foreach (int s in ShouPaiShu)
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
                        if (YiFan.Count > 0)
                        {
                            // 聴牌
                            return Ting.TingPai;
                        }
                        // 役判定
                        YiPanDing();
                        FuJiSuan();
                        if (FanShuJi > maxFanShuJi)
                        {
                            maxFanShuJi = FanShuJi;
                            maxFu = Fu;
                            maxYiFan = new(YiFan);
                            maxYiShu = YiFan.Count;
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
                FanShuJi = maxFanShuJi;
                Fu = maxFu;
                YiFan = new(maxYiFan);
            }
            return ret;
        }

        // 有効牌数計算
        internal void YouXiaoPaiShuJiSuan()
        {
            YouXiaoPaiShu = new();
            int minXiangTingShu = 99;
            List<int> xiangTingShu = new();
            for (int i = 0; i < ShouPai.Count; i++)
            {
                XiangTingShuJiSuan(i);
                xiangTingShu.Add(XiangTingShu);
                if (minXiangTingShu > XiangTingShu)
                {
                    minXiangTingShu = XiangTingShu;
                }
            }
            GongKaiPaiShuJiSuan();
            for (int i = 0; i < ShouPai.Count; i++)
            {
                if (xiangTingShu[i] != minXiangTingShu)
                {
                    YouXiaoPaiShu.Add(0);
                    continue;
                }
                int youXiaoPai = 0;
                List<int> shouPaiC = new(ShouPai);
                foreach (int p in Pai.QiaoPai)
                {
                    ShouPai[i] = p;
                    GongKaiPaiShu[ShouPai[i]]--;
                    GongKaiPaiShu[p]++;
                    for (int k = 0; k < ShouPai.Count; k++)
                    {
                        if (i == k)
                        {
                            continue;
                        }
                        XiangTingShuJiSuan(k);
                        if (minXiangTingShu > XiangTingShu)
                        {
                            youXiaoPai += Pai.CanShu(GongKaiPaiShu[p]);
                            break;
                        }
                    }
                    GongKaiPaiShu[ShouPai[i]]++;
                    GongKaiPaiShu[p]--;
                }
                YouXiaoPaiShu.Add(youXiaoPai);
                ShouPai = new(shouPaiC);
            }
        }

        // 向聴数計算
        internal void XiangTingShuJiSuan()
        {
            int minXiangTing = 99;
            for (int i = 0; i < ShouPai.Count; i++)
            {
                XiangTingShuJiSuan(i, FuLuPai.Count);
                if (minXiangTing > XiangTingShu)
                {
                    minXiangTing = XiangTingShu;
                }
            }
            XiangTingShu = minXiangTing;

            // 手牌数計算(向聴数計算で手牌の一番右を切った状態で手牌数を計算して終わるので、再度 手牌数を計算しておく)
            ShouPaiShuJiSuan();
        }
        internal void XiangTingShuJiSuan(int wei, int plusMianZiShu = 0)
        {
            XiangTingShu = 99;
            int xiang;

            List<int> shouPaiC = new(ShouPai);
            if (wei >= 0)
            {
                ShouPai[wei] = 0xff;
            }
            ShouPai.Sort();

            if (FuLuPai.Count == 0)
            {
                // 七対子向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();

                // 対子種数
                int chongShu = 0;
                foreach (int p in Pai.QiaoPai)
                {
                    if (ShouPaiShu[p] >= 2)
                    {
                        chongShu++;
                    }
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                XiangTingShu = 6 - duiZi.Count + (duiZi.Count - chongShu);

                // 国士無双向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();
                int shu = 0;
                bool chuHui = true;
                foreach (int p in Pai.YaoJiuPaiDingYi)
                {
                    if (ShouPaiShu[p] > 0)
                    {
                        if (chuHui && ShouPaiShu[p] > 1)
                        {
                            chuHui = false;
                            shu++;
                        }
                        shu++;
                    }
                }
                xiang = 13 - shu;
                if (XiangTingShu > xiang)
                {
                    XiangTingShu = xiang;
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
                    foreach (int p in Pai.QiaoPai)
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

                foreach (int p in Pai.QiaoPai)
                {
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                foreach (int p in Pai.QiaoPai)
                {
                    // 塔子計算
                    TaZiJiSuan(p);
                }
                // 計
                int mianZiShu = keZi.Count + shunZi.Count + plusMianZiShu;
                int xingShu = mianZiShu;
                while (xingShu < 4 && taZi.Count > 0)
                {
                    xingShu++;
                }
                int duiZiShu = duiZi.Count;
                while (xingShu < 4 && duiZi.Count > 0)
                {
                    duiZiShu--;
                    xingShu++;
                }
                xiang = 8 - (2 * mianZiShu) - (xingShu - mianZiShu);
                if (duiZiShu > 0)
                {
                    xiang--;
                }
                if (XiangTingShu > xiang)
                {
                    XiangTingShu = xiang;
                }
            }

            ShouPai = new(shouPaiC);
        }

        // 符計算
        private void FuJiSuan()
        {
            foreach ((int yi, _) in YiFan)
            {
                if (yi == (int)YiDingYi.QiDuiZi)
                {
                    Fu = 25;
                    return;
                }
                if (yi == (int)YiDingYi.PingHe)
                {
                    if (Chang.guiZe.ziMoPingHe && JiJia)
                    {
                        Fu = 20;
                        return;
                    }
                    Fu = 30;
                    return;
                }
            }

            // 副底
            Fu = 20;
            if (!JiJia && TaJiaFuLuShu == 0)
            {
                // 門前栄和加符
                Fu += 10;
            }
            // 雀頭
            foreach (List<int> pais in duiZi)
            {
                int p = pais[0] & QIAO_PAI;
                if (p == Chang.ChangFeng)
                {
                    Fu += 2;
                }
                if (p == Feng)
                {
                    Fu += 2;
                }
                if (ZiPaiPanDing(p))
                {
                    Fu += 2;
                }
            }
            // 刻子
            foreach ((List<int> pais, YaoDingYi yao) in keZi)
            {
                bool yaoJiu = false;
                if (YaoJiuPaiPanDing(pais[0]))
                {
                    yaoJiu = true;
                }
                switch (yao)
                {
                    case YaoDingYi.Wu:
                        // 暗刻
                        Fu += yaoJiu ? 8 : 4;
                        break;
                    case YaoDingYi.Bing:
                        // 明刻
                        Fu += yaoJiu ? 4 : 2;
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        Fu += yaoJiu ? 32 : 16;
                        break;
                    case YaoDingYi.JiaGang:
                    case YaoDingYi.DaMingGang:
                        // 加槓・大明槓
                        Fu += yaoJiu ? 16 : 8;
                        break;
                }
            }
            // 待
            if (JiJia)
            {
                Fu += 2;
            }
            int heLePai = ShouPai[^1] & QIAO_PAI;
            bool daiDian = false;
            foreach (List<int> pais in duiZi)
            {
                if (pais[0] == heLePai)
                {
                    Fu += 2;
                    daiDian = true;
                    break;
                }
            }
            if (!daiDian)
            {
                foreach (List<int> pais in shunZi)
                {
                    if (pais[1] == heLePai || (heLePai & SHU_PAI) == 3 || (heLePai & SHU_PAI) == 7)
                    {
                        Fu += 2;
                        break;
                    }
                }
            }
            // 切上
            Fu = Chang.Ceil(Fu, 10);
            if (Fu == 20)
            {
                // 食い平和形
                Fu = 30;
            }
        }

        // 点計算
        private void DianJiSuan()
        {
            HeLeDian = 0;
            if (!YiMan)
            {
                if (FanShuJi < 5)
                {
                    HeLeDian = (Feng == 0x31 ? 6 : 4) * 2 * 2 * Fu;
                    for (int i = 0; i < FanShuJi; i++)
                    {
                        HeLeDian *= 2;
                    }
                    HeLeDian = Chang.Ceil(HeLeDian, 100);
                    if (Feng == 0x31)
                    {
                        if (HeLeDian > 12000)
                        {
                            HeLeDian = 12000;
                        }
                    }
                    else
                    {
                        if (HeLeDian > 8000)
                        {
                            HeLeDian = 8000;
                        }
                    }
                }
                else if (FanShuJi == 5)
                {
                    // 満貫
                    HeLeDian = Feng == 0x31 ? 12000 : 8000;
                }
                else if (FanShuJi == 6 || FanShuJi == 7)
                {
                    // 跳満
                    HeLeDian = Feng == 0x31 ? 18000 : 12000;
                }
                else if (FanShuJi == 8 || FanShuJi == 9 || FanShuJi == 10)
                {
                    // 倍満
                    HeLeDian = Feng == 0x31 ? 24000 : 16000;
                }
                else if (FanShuJi == 11 || FanShuJi == 12)
                {
                    // 三倍満
                    HeLeDian = Feng == 0x31 ? 36000 : 24000;
                }
                else if (FanShuJi >= 13)
                {
                    // 役満
                    HeLeDian = Feng == 0x31 ? 48000 : 32000;
                }

            }
            else
            {
                HeLeDian = (Feng == 0x31 ? 48000 : 32000) * FanShuJi;
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
                JiuZhongJiuPai = true;
            }
        }

        // 幺九牌種類数計算
        protected int YaoJiuPaiJiSuan()
        {
            int yaoJiuPaiShu = 0;
            foreach (int p in Pai.YaoJiuPaiDingYi)
            {
                for (int j = 0; j < ShouPai.Count; j++)
                {
                    int sp = ShouPai[j] & QIAO_PAI;
                    if (p == sp)
                    {
                        yaoJiuPaiShu++;
                        break;
                    }
                }
            }
            return yaoJiuPaiShu;
        }

        // 振聴判定
        internal bool ZhenTingPanDing()
        {
            foreach (int dp in DaiPai)
            {
                // 振聴
                foreach ((int pai, _, _) in ShePai)
                {
                    if (dp == (pai & QIAO_PAI))
                    {
                        return true;
                    }
                }
                // 同順内振聴
                foreach (int pai in tongShunPai)
                {
                    if (dp == (pai & QIAO_PAI))
                    {
                        return true;
                    }
                }
                // 立直後振聴
                foreach (int pai in liZhiHouPai)
                {
                    if (dp == (pai & QIAO_PAI))
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
            List<int> shouPaiC = new(ShouPai);
            for (int i = 0; i < ShouPai.Count; i++)
            {
                ShouPai[i] = 0xff;
                ShouPai.Sort();

                int wei = 0;
                List<int> hp = new();
                int[] dian = new int[0x40];
                Init(dian, 0);
                foreach (int p in Pai.QiaoPai)
                {
                    ShouPai[^1] = p;

                    if (HeLePanDing() == Ting.TingPai)
                    {
                        hp.Add(p);
                        DianJiSuan();
                        dian[p] = HeLeDian;
                        wei++;
                    }
                }
                if (wei > 0)
                {
                    HeLePai.Add((hp, i, dian));

                    if (Pai.CanShanPaiShu() >= 4 && TaJiaFuLuShu == 0 && (DianBang >= 1000 || Chang.guiZe.jieJinLiZhi))
                    {
                        LiZhiPaiWei.Add(i);
                    }
                }

                ShouPai = new(shouPaiC);
            }
        }

        // 暗槓判定
        private void AnGangPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            foreach (int p in Pai.QiaoPai)
            {
                if (ShouPaiShu[p] >= 4)
                {
                    List<int> anGang = new();
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (p == (ShouPai[j] & QIAO_PAI))
                        {
                            anGang.Add(j);
                        }
                    }
                    AnGangPaiWei.Add(anGang);
                }
            }
            // 立直後槓判定
            LiZhiHouGangPanDing();
        }

        // 立直後槓判定
        private void LiZhiHouGangPanDing()
        {
            if (!LiZhi || AnGangPaiWei.Count == 0)
            {
                return;
            }

            // 待牌計算
            DaiPaiJiSuan(ShouPai.Count - 1);

            bool heLe = true;
            for (int i = 0; i < AnGangPaiWei.Count; i++)
            {
                List<int> shouPaiC = new(ShouPai);
                List<(List<int>, int, YaoDingYi)> fuLuPaiC = new(FuLuPai);

                AnGang(i);
                ShouPai.Sort();
                foreach (int dp in DaiPai)
                {
                    ShouPai.Add(dp);
                    if (HeLePanDing() != Ting.TingPai)
                    {
                        heLe = false;
                        break;
                    }
                    ShouPai.RemoveAt(ShouPai.Count - 1);
                }

                ShouPai = new(shouPaiC);
                FuLuPai = new(fuLuPaiC);
            }

            if (!heLe)
            {
                AnGangPaiWei = new();
            }
        }

        // 加槓判定
        private void JiaGangPanDing()
        {
            foreach ((List<int> pais, _, YaoDingYi yao) in FuLuPai)
            {
                if (yao != YaoDingYi.Bing)
                {
                    continue;
                }
                for (int j = 0; j < ShouPai.Count; j++)
                {
                    if ((pais[0] & QIAO_PAI) == (ShouPai[j] & QIAO_PAI))
                    {
                        JiaGangPaiWei.Add(new List<int>() { j });
                    }
                }
            }
        }

        // 大明槓判定
        private void DaMingGangPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            int shePai = Chang.ShePai & QIAO_PAI;
            if (ShouPaiShu[shePai] < 3)
            {
                return;
            }
            List<int> daMingGang = new();
            for (int i = 0; i < ShouPai.Count; i++)
            {
                int p = ShouPai[i] & QIAO_PAI;
                if (p == shePai)
                {
                    daMingGang.Add(i);
                }
            }
            DaMingGangPaiWei.Add(daMingGang);
        }

        // 石並判定
        private void BingPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            int shePai = Chang.ShePai & QIAO_PAI;
            if (ShouPaiShu[shePai] < 2)
            {
                return;
            }
            if (ShouPai.Count < 3)
            {
                return;
            }
            for (int i = 0; i < ShouPai.Count - 1; i++)
            {
                int p1 = ShouPai[i] & QIAO_PAI;
                if (p1 == shePai)
                {
                    for (int j = i + 1; j < ShouPai.Count; j++)
                    {
                        int p2 = ShouPai[j] & QIAO_PAI;
                        if (p2 == shePai)
                        {
                            BingPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                }
            }
        }

        // 吃判定
        private void ChiPanDing()
        {
            if (Chang.QiaoShis.Count <= 2)
            {
                return;
            }
            if (ZiPaiPanDing(Chang.ShePai))
            {
                return;
            }
            if (ShouPai.Count < 3)
            {
                return;
            }
            int se = Chang.ShePai & SE_PAI;
            for (int i = 0; i < ShouPai.Count - 1; i++)
            {
                int p1 = ShouPai[i] & QIAO_PAI;
                int s1 = p1 & SHU_PAI;
                if (ZiPaiPanDing(p1) || ((p1 & SE_PAI) != se))
                {
                    continue;
                }
                for (int j = i + 1; j < ShouPai.Count; j++)
                {
                    int p2 = ShouPai[j] & QIAO_PAI;
                    int s2 = p2 & SHU_PAI;
                    if (ZiPaiPanDing(p2) || ((p2 & SE_PAI) != se))
                    {
                        continue;
                    }

                    int sps = Chang.ShePai & SHU_PAI;
                    if (sps <= 7)
                    {
                        if (s1 == (sps + 1) && (s2 == sps + 2))
                        {
                            ChiPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                    if (sps >= 2 && sps <= 8)
                    {
                        if (s1 == (sps - 1) && (s2 == sps + 1))
                        {
                            ChiPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                    if (sps >= 3)
                    {
                        if (s1 == (sps - 2) && (s2 == sps - 1))
                        {
                            ChiPaiWei.Add(new List<int>() { i, j });
                        }
                    }
                }
            }
        }

        // 鳴牌判定
        internal bool MingPaiPanDing(YaoDingYi fuLuZhong, int fuLuJia, int wei)
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

        // 鳴選択
        protected int MingXuanZe(List<List<int>> paiWei)
        {
            // 向聴数計算
            XiangTingShuJiSuan(-1);
            int xiangTing = XiangTingShu;
            int xuanZe = -1;
            for (int i = 0; i < paiWei.Count; i++)
            {
                List<int> shouPaiC = new(ShouPai);
                for (int j = 0; j < paiWei[i].Count; j++)
                {
                    ShouPai[paiWei[i][j]] = 0xff;
                }
                LiPai();
                int minXiangTing = 99;
                for (int j = 0; j < ShouPai.Count - paiWei[i].Count; j++)
                {
                    // 向聴数計算(副露牌は処理していない状態なので面子数を1加算して計算)
                    XiangTingShuJiSuan(j, 1);
                    if (minXiangTing > XiangTingShu)
                    {
                        minXiangTing = XiangTingShu;
                    }
                }
                if (xiangTing > minXiangTing)
                {
                    xiangTing = minXiangTing;
                    xuanZe = i;
                }
                ShouPai = new(shouPaiC);
            }
            return xuanZe;
        }

        // 食替牌判定
        private void ShiTiPaiPanDing()
        {
            if (Chang.guiZe.shiTi || !fuLuShun)
            {
                return;
            }

            (List<int> pais, _, _) = FuLuPai[^1];
            int mingPai = pais[0] & QIAO_PAI;
            ShiTiPai.Add(mingPai);
            if ((mingPai & ZI_PAI) != ZI_PAI)
            {
                int p1 = pais[1] & QIAO_PAI;
                int p2 = pais[2] & QIAO_PAI;
                if (Math.Abs(p1 - p2) == 1)
                {
                    int mingShu = mingPai & SHU_PAI;
                    if (mingPai < p1 && mingShu < 7)
                    {
                        ShiTiPai.Add(mingPai + 3);
                    }
                    if (mingPai > p2 && mingShu > 3)
                    {
                        ShiTiPai.Add(mingPai - 3);
                    }
                }
            }
        }

        // 国士無双判定
        private bool GuoShiWuShuangPanDing()
        {
            if (TaJiaFuLuShu > 0)
            {
                return false;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();

            int shu = 0;
            foreach (int p in Pai.YaoJiuPaiDingYi)
            {
                if (ShouPaiShu[p] == 0)
                {
                    return false;
                }
                if (ShouPaiShu[p] == 2)
                {
                    duiZi.Add(new List<int>() { p, p });
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
            if (TaJiaFuLuShu > 0)
            {
                return false;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();

            foreach (int p in Pai.QiaoPai)
            {
                int shu = ShouPaiShu[p];
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
            for (int i = 0; i < duiZi.Count - 2; i++)
            {
                if (ZiPaiPanDing(duiZi[i][0]))
                {
                    continue;
                }

                if ((duiZi[i][0] + 1) == duiZi[i + 1][0] && (duiZi[i][0] + 2) == duiZi[i + 2][0])
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
            duiZi = new();
            keZi = new();
            shunZi = new();
            taZi = new();
        }

        /**
         * 役満判定
         */
        private void YiManPanDing()
        {
            YiMan = false;

            YiFan = new();
            FanShuJi = 0;

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
            GongKongQiao();
            // 百万石・純正百万石
            BaiWanShi();
            if (Chang.guiZe.baLianZhuang && LianZhuangShu == 7)
            {
                // 八連荘
                YiZhuiJia(YiManDingYi.BaLianZhuang, 1);
            }

            FanShuJi = 0;
            foreach ((_, int fanShu) in YiFan)
            {
                FanShuJi += fanShu;
            }

            if (FanShuJi > 0)
            {
                YiMan = true;
            }
        }

        // 天和
        private void TianHe()
        {
            if (Feng != 0x31)
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
            if (Feng == 0x31)
            {
                return;
            }
            if (yiXunMu)
            {
                if (JiJia)
                {
                    YiZhuiJia(YiManDingYi.DeHe, 1);
                }
            }
        }

        // 国士無双・国士無双十三面
        private void GuoShiWuShuang()
        {
            if (duiZi.Count == 1 && keZi.Count == 0 && shunZi.Count == 0)
            {
                if ((ShouPai[^1] & QIAO_PAI) == duiZi[0][0])
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
            if (keZi.Count < 4)
            {
                return;
            }
            int anKe = 0;
            int gangZi = 0;
            foreach ((_, YaoDingYi yao) in keZi)
            {
                if (yao == YaoDingYi.Wu || yao == YaoDingYi.AnGang)
                {
                    anKe++;
                }
                if (yao == YaoDingYi.AnGang || yao == YaoDingYi.JiaGang || yao == YaoDingYi.DaMingGang)
                {
                    gangZi++;
                }
            }
            if (anKe == 4)
            {
                if ((ShouPai[^1] & QIAO_PAI) == duiZi[0][0])
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
            if (keZi.Count < 3)
            {
                return;
            }
            int yuan = 0;
            foreach ((List<int> pais, _) in keZi)
            {
                int p = pais[0];
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
            if (keZi.Count < 3)
            {
                return;
            }
            int sixi = 0;
            foreach ((List<int> pais, _) in keZi)
            {
                if (pais[0] >= 0x31 && pais[0] <= 0x34)
                {
                    sixi++;
                }
            }
            int dui = 0;
            foreach (List<int> pais in duiZi)
            {
                if (pais[0] >= 0x31 && pais[0] <= 0x34)
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
            foreach (int sp in ShouPai)
            {
                if (!ZiPaiPanDing(sp))
                {
                    return;
                }
            }
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
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
            foreach (int sp in ShouPai)
            {
                int p = sp & QIAO_PAI;
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
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
                {
                    int p = fp & QIAO_PAI;
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
            if (TaJiaFuLuShu > 0)
            {
                return;
            }
            int[] jiuLian = new int[10];
            Init(jiuLian, 0);
            int se = -1;
            foreach (int sp in ShouPai)
            {
                int p = sp & QIAO_PAI;
                if (p > QIAO_PAI)
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
            int heLePai = ShouPai[^1];
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
            foreach (int sp in ShouPai)
            {
                int p = sp & QIAO_PAI;
                bool luYi = false;
                foreach (int lp in Pai.LuYiSePaiDingYi)
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
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
                {
                    int p = fp & QIAO_PAI;
                    bool luYi = false;
                    foreach (int lp in Pai.LuYiSePaiDingYi)
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
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            if (Feng == 0x31)
            {
                return;
            }
            if (yiXunMu)
            {
                if (!JiJia)
                {
                    YiZhuiJia(YiManDingYi.RenHe, 1);
                }
            }
        }

        // 四連刻
        private void SiLianKe()
        {
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            if (keZi.Count < 4)
            {
                return;
            }
            List<int> k = new()
            {
                keZi[0].pais[0],
                keZi[1].pais[0],
                keZi[2].pais[0],
                keZi[3].pais[0]
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
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 大車輪
            bool daCheLun = true;
            for (int i = 0x12; i <= 0x18; i++)
            {
                if (ShouPaiShu[i] != 2)
                {
                    daCheLun = false;
                    break;
                }
            }
            // 小車輪(1-7)
            bool xiaoCheLun17 = true;
            for (int i = 0x11; i <= 0x17; i++)
            {
                if (ShouPaiShu[i] != 2)
                {
                    xiaoCheLun17 = false;
                    break;
                }
            }
            // 小車輪(3-9)
            bool xiaoCheLun39 = true;
            for (int i = 0x13; i <= 0x19; i++)
            {
                if (ShouPaiShu[i] != 2)
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
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            for (int i = 0x22; i <= 0x28; i++)
            {
                if (ShouPaiShu[i] != 2)
                {
                    return;
                }
            }
            YiZhuiJia(YiManDingYi.DaZhuLin, 1);
        }

        // 大数隣
        private void DaShuLin()
        {
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            for (int i = 0x02; i <= 0x08; i++)
            {
                if (ShouPaiShu[i] != 2)
                {
                    return;
                }
            }
            YiZhuiJia(YiManDingYi.DaShuLin, 1);
        }

        // 紅孔雀
        private void GongKongQiao()
        {
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            foreach (int sp in ShouPai)
            {
                int p = sp & QIAO_PAI;
                bool gongKong = false;
                foreach (int lp in Pai.GongKongQiaoPaiDingYi)
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
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
                {
                    int p = fp & QIAO_PAI;
                    bool gongKong = false;
                    foreach (int lp in Pai.GongKongQiaoPaiDingYi)
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
            YiZhuiJia(YiManDingYi.GongKongQiao, 1);
        }

        // 百万石・純正百万石
        private void BaiWanShi()
        {
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            int wanShi = 0;
            foreach (int sp in ShouPai)
            {
                int se = sp & SE_PAI;
                if (se != 0x00)
                {
                    return;
                }
                wanShi += sp & SHU_PAI;
            }

            foreach ((List<int> pais, int jia, YaoDingYi yao) in FuLuPai)
            {
                foreach (int fp in pais)
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
            if (!Chang.guiZe.shiSanBuTa)
            {
                return false;
            }

            YiMan = false;
            YiFan = new();
            FanShuJi = 0;

            if (!yiXunMu)
            {
                return false;
            }
            if (!JiJia)
            {
                return false;
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();
            foreach (int p in Pai.QiaoPai)
            {
                if (ShouPaiShu[p] == 2)
                {
                    DuiZiJiSuan(p);
                    ShouPaiShu[p] += 2;
                    break;
                }
            }
            foreach (int p in Pai.QiaoPai)
            {
                int shu = ShouPaiShu[p];
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
                        if (ShouPaiShu[p + 1] > 0 || ShouPaiShu[p + 2] > 0)
                        {
                            return false;
                        }
                    }
                    if (s >= 3)
                    {
                        if (ShouPaiShu[p - 1] > 0 || ShouPaiShu[p - 2] > 0)
                        {
                            return false;
                        }
                    }
                }
            }

            YiMan = true;
            YiZhuiJia(YiManDingYi.ShiSanBuTa, 1);
            FanShuJi = 1;
            return true;
        }

        // 役判定
        private void YiPanDing()
        {
            YiFan = new();
            FanShuJi = 0;

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

            if (YiFan.Count > 0)
            {
                // 懸賞牌
                XuanShangPai();
                if (!JiJia)
                {
                    if (Chang.guiZe.yanFan && Chang.QiaoShis[Chang.ZiMoFan].ZiJiaYao == YaoDingYi.LiZhi)
                    {
                        // 燕返し
                        YiZhuiJia(YiDingYi.YanFan, 1);
                    }
                }
                if (KaiLiZhi)
                {
                    // 開立直
                    if (TaJiaYao == YaoDingYi.RongHe)
                    {
                        bool isYiman = false;
                        QiaoShi ziMoShi = Chang.QiaoShis[Chang.ZiMoFan];
                        if (!ziMoShi.LiZhi)
                        {
                            isYiman = true;
                        }
                        int ziMoFanLiZhiWei = 0;
                        for (int i = 0; i < ziMoShi.ShePai.Count; i++)
                        {
                            if (ziMoShi.ShePai[i].yao == YaoDingYi.LiZhi)
                            {
                                break;
                            }
                            ziMoFanLiZhiWei++;
                        }
                        int ronHeFanLiZhiWei = 0;
                        for (int i = 0; i < ShePai.Count; i++)
                        {
                            if (ShePai[i].yao == YaoDingYi.LiZhi)
                            {
                                break;
                            }
                            ronHeFanLiZhiWei++;
                        }
                        if (ziMoFanLiZhiWei > ronHeFanLiZhiWei || (ziMoFanLiZhiWei == ronHeFanLiZhiWei && ziMoShi.Feng > Feng))
                        {
                            isYiman = true;
                        }

                        if (isYiman)
                        {
                            YiFan = new();
                            YiZhuiJia(YiDingYi.KaiLiZhi, 13);
                        }
                    }
                }
                if (LianZhuangShu == 7)
                {
                    // 八連荘
                    YiFan = new();
                    YiZhuiJia(YiManDingYi.BaLianZhuang, 1);
                    YiMan = true;
                }
            }

            FanShuJi = 0;
            foreach ((_, int fanShu) in YiFan)
            {
                FanShuJi += fanShu;
            }
        }

        // 立直・Ｗ立直・一発
        private void LiZhiWLiZhiYiFa()
        {
            if (KaiLiZhi)
            {
                if (wLiZhi)
                {
                    YiZhuiJia(YiDingYi.KaiWLiZhi, 3);
                }
                else if (LiZhi)
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
                else if (LiZhi)
                {
                    YiZhuiJia(YiDingYi.LiZi, 1);
                }
            }

            if (YiFa)
            {
                YiZhuiJia(YiDingYi.YiFa, 1);
            }
        }

        // 面前清自摸和
        private void MianQianQingZiMohe()
        {
            if (JiJia && TaJiaFuLuShu == 0)
            {
                YiZhuiJia(YiDingYi.MianQianQingZiMoHe, 1);
            }
        }

        // 嶺上開花
        private void LingShangKaiHua()
        {
            if (Pai.LingShanKaiHuaPanDing())
            {
                YiZhuiJia(YiDingYi.LingShangKaiHua, 1);
            }
        }

        // 槍槓
        private void QiangGang()
        {
            if (Pai.QiangGangPanDing())
            {
                YiZhuiJia(YiDingYi.QiangGang, 1);
            }
        }

        // 海底撈月・河底撈魚
        private void HaiDiLaoYueHeDiLaoYu()
        {
            if (Pai.LingShanKaiHuaPanDing())
            {
                return;
            }

            if (Pai.HaiDiPanDing())
            {
                if (JiJia)
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
            if (TaJiaFuLuShu > 0)
            {
                return;
            }
            if (shunZi.Count < 4)
            {
                return;
            }
            if (!Chang.guiZe.ziMoPingHe && JiJia)
            {
                // 自摸平和無し
                return;
            }
            int tou = duiZi[0][0] & QIAO_PAI;
            if (tou > 0x34 || tou == Chang.ChangFeng || tou == Feng)
            {
                return;
            }
            int heLePai = ShouPai[^1] & QIAO_PAI;
            foreach (List<int> pais in shunZi)
            {
                if ((pais[0] == heLePai && (heLePai & SHU_PAI) != 7) || (pais[2] == heLePai && (heLePai & SHU_PAI) != 3))
                {
                    YiZhuiJia(YiDingYi.PingHe, 1);
                    return;
                }
            }
        }

        // 断幺九
        private void DuanYaoJiu()
        {
            foreach (int sp in ShouPai)
            {
                int p = sp & QIAO_PAI;
                if (ZiPaiPanDing(p) || ((p & SHU_PAI) == 1) || ((p & SHU_PAI) == 9))
                {
                    return;
                }
            }
            if (!Chang.guiZe.shiDuan && FuLuPai.Count > 0)
            {
                // 喰断無し
                return;
            }
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
                {
                    int p = fp & QIAO_PAI;
                    if (p == QIAO_PAI)
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
            if (TaJiaFuLuShu > 0)
            {
                return;
            }
            if (shunZi.Count < 2)
            {
                return;
            }
            int beiKou = 0;
            for (int i = 0; i < shunZi.Count - 1; i++)
            {
                for (int j = i + 1; j < shunZi.Count; j++)
                {
                    if (shunZi[i][0] == shunZi[j][0] && shunZi[i][1] == shunZi[j][1] && shunZi[i][2] == shunZi[j][2])
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
                for (int i = 0; i < shunZi.Count; i++)
                {
                    List<int> pais1 = shunZi[i];
                    int shu = 0;
                    for (int j = 0; j < shunZi.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        List<int> pais2 = shunZi[j];
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
            foreach ((List<int> pais, _) in keZi)
            {
                int p = pais[0];
                if (p == Chang.ChangFeng)
                {
                    fan++;
                }
                if (p == Feng)
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
            if (shunZi.Count < 3)
            {
                return;
            }
            for (int i = 0; i < shunZi.Count; i++)
            {
                List<int> pais1 = shunZi[i];
                bool[] se = new bool[3];
                for (int j = 0; j < shunZi.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    List<int> pais2 = shunZi[j];
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
                    if (TaJiaFuLuShu > 0)
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

            foreach (List<int> pais in shunZi)
            {
                int s1 = pais[0] & SHU_PAI;
                int s2 = pais[1] & SHU_PAI;
                int s3 = pais[2] & SHU_PAI;
                if (s1 == 1 && s2 == 2 && s3 == 3)
                {
                    xing[(pais[0] & SE_PAI) >> 4][0] = true;
                }
                else if (s1 == 4 && s2 == 5 && s3 == 6)
                {
                    xing[(pais[0] & SE_PAI) >> 4][1] = true;
                }
                else if (s1 == 7 && s2 == 8 && s3 == 9)
                {
                    xing[(pais[0] & SE_PAI) >> 4][2] = true;
                }
            }
            for (int i = 0; i < xing.Length; i++)
            {
                if (xing[i][0] && xing[i][1] && xing[i][2])
                {
                    if (TaJiaFuLuShu > 0)
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
            foreach (List<int> pais in duiZi)
            {
                int p = pais[0];
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
            foreach ((List<int> pais, _) in keZi)
            {
                int p = pais[0];
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
            foreach (List<int> pais in shunZi)
            {
                int shu = (pais[0] & SHU_PAI) + (pais[1] & SHU_PAI) + (pais[2] & SHU_PAI);
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
                if (shunZi.Count == 0)
                {
                    YiZhuiJia(YiDingYi.HunLaoTou, 2);
                }
                else if (TaJiaFuLuShu > 0)
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
                if (TaJiaFuLuShu > 0)
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
            if (keZi.Count == 4)
            {
                YiZhuiJia(YiDingYi.DuiDuiHe, 2);
            }
        }

        // 三暗刻・三槓子
        private void SanAnKeSanGangZi()
        {
            if (keZi.Count < 3)
            {
                return;
            }
            int anKe = 0;
            int gang = 0;
            foreach ((_, YaoDingYi yao) in keZi)
            {
                if (yao == YaoDingYi.Wu || yao == YaoDingYi.AnGang)
                {
                    anKe++;
                }
                if (yao == YaoDingYi.AnGang || yao == YaoDingYi.JiaGang || yao == YaoDingYi.DaMingGang)
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
            if (keZi.Count < 2)
            {
                return;
            }
            int yuan = 0;
            foreach ((List<int> pais, _) in keZi)
            {
                int p = pais[0];
                if (p == 0x35 || p == 0x36 || p == 0x37)
                {
                    yuan++;
                }
            }
            int dui = 0;
            foreach (List<int> pais in duiZi)
            {
                int p = pais[0];
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
            foreach (int sp in ShouPai)
            {
                int p = sp & QIAO_PAI;
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
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
                {
                    int p = fp & QIAO_PAI;
                    if (p == QIAO_PAI)
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
                if (TaJiaFuLuShu > 0)
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
                if (TaJiaFuLuShu > 0)
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
            if (duiZi.Count == 7)
            {
                YiZhuiJia(YiDingYi.QiDuiZi, 2);
            }
        }

        // 流し満貫
        private void LiuManGuan()
        {
            foreach ((int pai, YaoDingYi yao, _) in ShePai)
            {
                if (yao != YaoDingYi.Wu && yao != YaoDingYi.LiZhi)
                {
                    return;
                }
                int p = pai & QIAO_PAI;
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
            foreach (int xp in Pai.XuanShangPai)
            {
                xuan += ShouPaiShu[Pai.XuanShangPaiDingYi[xp & QIAO_PAI]];
            }
            if (LiZhi)
            {
                foreach (int lxp in Pai.LiXuanShangPai)
                {
                    xuan += ShouPaiShu[Pai.XuanShangPaiDingYi[lxp & QIAO_PAI]];
                }
            }
            foreach (int sp in ShouPai)
            {
                if ((sp & CHI_PAI) == CHI_PAI)
                {
                    xuan += 1;
                }
            }
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
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
            if (!Chang.guiZe.sanLianKe)
            {
                return;
            }
            if (keZi.Count < 3)
            {
                return;
            }
            for (int i = 0; i < keZi.Count - 2; i++)
            {
                if (ZiPaiPanDing(keZi[i].pais[0]))
                {
                    continue;
                }
                List<int> k = new()
                {
                    keZi[i].pais[0],
                    keZi[i + 1].pais[0],
                    keZi[i + 2].pais[0]
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
            YiFan.Add(((int)ming, fan));
        }
        private void YiZhuiJia(YiDingYi ming, int fan)
        {
            YiFan.Add(((int)ming, fan));
        }

        // 対子計算
        protected void DuiZiJiSuan(int p, bool quanXiao = true)
        {
            while (ShouPaiShu[p] >= 2)
            {
                ShouPaiShu[p] -= 2;

                duiZi.Add(new List<int>() { p, p });
                if (!quanXiao)
                {
                    break;
                }
            }
        }

        // 刻子計算
        protected void KeZiJiSuan(int p)
        {
            if (ShouPaiShu[p] >= 3)
            {
                ShouPaiShu[p] -= 3;

                keZi.Add((new List<int>() { p, p, p }, YaoDingYi.Wu));
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
                while (ShouPaiShu[p] >= 1 && ShouPaiShu[p + 1] >= 1 && ShouPaiShu[p + 2] >= 1)
                {
                    ShouPaiShu[p]--;
                    ShouPaiShu[p + 1]--;
                    ShouPaiShu[p + 2]--;

                    shunZi.Add(new List<int>() { p, p + 1, p + 2 });
                }
            }
            if (s >= 2 && s <= 8)
            {
                while (ShouPaiShu[p - 1] >= 1 && ShouPaiShu[p] >= 1 && ShouPaiShu[p + 1] >= 1)
                {
                    ShouPaiShu[p - 1]--;
                    ShouPaiShu[p]--;
                    ShouPaiShu[p + 1]--;

                    shunZi.Add(new List<int>() { p - 1, p, p + 1 });
                }
            }
            if (s >= 3)
            {
                while (ShouPaiShu[p - 2] >= 1 && ShouPaiShu[p - 1] >= 1 && ShouPaiShu[p] >= 1)
                {
                    ShouPaiShu[p - 2]--;
                    ShouPaiShu[p - 1]--;
                    ShouPaiShu[p]--;

                    shunZi.Add(new List<int>() { p - 2, p - 1, p });
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

            while (ShouPaiShu[p] >= 1 && ShouPaiShu[p + 1] >= 1)
            {
                ShouPaiShu[p]--;
                ShouPaiShu[p + 1]--;

                taZi.Add(new List<int>() { p, p + 1 });
            }
            while (ShouPaiShu[p] >= 1 && ShouPaiShu[p + 2] >= 1)
            {
                ShouPaiShu[p]--;
                ShouPaiShu[p + 2]--;

                taZi.Add(new List<int>() { p, p + 2 });
            }
        }

        // 副露計算
        private void FuLuJiSuan()
        {
            foreach ((List<int> pais, _, YaoDingYi yao) in FuLuPai)
            {
                if (yao == YaoDingYi.Chi)
                {
                    List<int> s = new();
                    foreach (int fp in pais)
                    {
                        s.Add(fp & QIAO_PAI);
                    }
                    s.Sort();
                    shunZi.Add(s);
                }
                else
                {
                    List<int> k = new();
                    foreach (int fp in pais)
                    {
                        k.Add(fp & QIAO_PAI);
                    }
                    keZi.Add((k, yao));
                }
            }

            int heLePai = ShouPai[^1] & QIAO_PAI;
            foreach (List<int> pais in duiZi)
            {
                if (pais[0] == heLePai)
                {
                    return;
                }
            }
            foreach (List<int> pais in shunZi)
            {
                for (int j = 0; j < pais.Count; j++)
                {
                    if (pais[j] == heLePai)
                    {
                        return;
                    }
                }
            }
            for (int i = 0; i < keZi.Count; i++)
            {
                if (keZi[i].yao == YaoDingYi.Wu && keZi[i].pais[0] == heLePai && !JiJia)
                {
                    keZi[i] = (keZi[i].pais, YaoDingYi.Bing);
                    return;
                }
            }
        }

        // 手牌数計算
        protected void ShouPaiShuJiSuan()
        {
            ShouPaiShuJiSuan(false);
        }
        protected void ShouPaiShuJiSuan(bool fuLu)
        {
            Init(ShouPaiShu, 0);
            foreach (int sp in ShouPai)
            {
                ShouPaiShu[sp & QIAO_PAI]++;
            }

            if (fuLu)
            {
                // 副露牌加算
                foreach ((List<int> pais, _, _) in FuLuPai)
                {
                    foreach (int fp in pais)
                    {
                        ShouPaiShu[fp & QIAO_PAI]++;
                    }
                }
            }
        }

        // 公開牌数計算
        internal void GongKaiPaiShuJiSuan()
        {
            Init(GongKaiPaiShu, 0);
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi.KaiLiZhi && shi.Feng != Feng)
                {
                    // 開立直の場合、手牌
                    foreach (int sp in ShouPai)
                    {
                        int p = sp & QIAO_PAI;
                        GongKaiPaiShu[p]++;
                    }
                }
                // 捨牌
                foreach ((int pai, _, _) in shi.ShePai)
                {
                    int p = pai & QIAO_PAI;
                    GongKaiPaiShu[p]++;
                }
                // 副露牌
                for (int i = 0; i < shi.FuLuPai.Count; i++)
                {
                    (List<int> pais, int jia, YaoDingYi yao) = shi.FuLuPai[i];
                    int wei = 0;
                    foreach (int fp in pais)
                    {
                        int p = fp & QIAO_PAI;
                        if (yao == YaoDingYi.JiaGang && i == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(yao, jia, wei);
                        if (!isMingPai)
                        {
                            GongKaiPaiShu[p]++;
                        }
                        wei++;
                    }
                }
            }
            // 懸賞牌
            foreach (int pai in Pai.XuanShangPai)
            {
                GongKaiPaiShu[pai & QIAO_PAI]++;
            }
            // 手牌
            foreach (int pai in ShouPai)
            {
                GongKaiPaiShu[pai & QIAO_PAI]++;
            }
        }

        // 副露牌数計算
        protected void FuLuPaiShuJiSuan()
        {
            Init(FuLuPaiShu, 0);
            foreach ((List<int> pais, _, _) in FuLuPai)
            {
                foreach (int fp in pais)
                {
                    FuLuPaiShu[fp & QIAO_PAI]++;
                }
            }
        }

        // 錯和自家判定
        internal bool CuHeZiJiaPanDing()
        {
            if (ZiJiaXuanZe < 0)
            {
                CuHeSheng = "打牌選択間違い";
                return true;
            }

            if (LiZhi)
            {
                switch (ZiJiaYao)
                {
                    case YaoDingYi.Wu:
                        // 無
                        if (ZiJiaXuanZe != ShouPai.Count - 1)
                        {
                            CuHeSheng = "立直後打牌手出し";
                            return true;
                        }
                        break;
                    case YaoDingYi.JiaGang:
                        // 加槓
                        if (JiaGangPaiWei.Count <= ZiJiaXuanZe)
                        {
                            CuHeSheng = "立直後加槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        if (AnGangPaiWei.Count <= ZiJiaXuanZe)
                        {
                            CuHeSheng = "立直後暗槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.ZiMo:
                        // 自摸
                        if (!HeLe)
                        {
                            CuHeSheng = "立直後誤自摸";
                            return true;
                        }
                        break;
                    default:
                        CuHeSheng = "立直後腰間違い";
                        return true;
                }

            }
            else
            {
                switch (ZiJiaYao)
                {
                    case YaoDingYi.Wu:
                        // 無
                        if (ZiJiaXuanZe > ShouPai.Count - 1)
                        {
                            CuHeSheng = "打牌選択間違い";
                            return true;
                        }
                        break;
                    case YaoDingYi.JiaGang:
                        // 加槓
                        if (JiaGangPaiWei.Count <= ZiJiaXuanZe)
                        {
                            CuHeSheng = "加槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        if (AnGangPaiWei.Count <= ZiJiaXuanZe)
                        {
                            CuHeSheng = "暗槓不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.LiZhi:
                        // 立直
                        if (TaJiaFuLuShu > 0)
                        {
                            CuHeSheng = "立直不可";
                            return true;
                        }
                        if (!Chang.guiZe.kaiLiZhi && KaiLiZhi)
                        {
                            CuHeSheng = "開立直不可";
                            return true;
                        }
                        break;
                    case YaoDingYi.ZiMo:
                        // 自摸
                        if (!HeLe)
                        {
                            CuHeSheng = "誤自摸";
                            return true;
                        }
                        break;
                    case YaoDingYi.JiuZhongJiuPai:
                        // 九種九牌
                        if (!JiuZhongJiuPai || Chang.guiZe.jiuZhongJiuPaiLianZhuang == 0)
                        {
                            CuHeSheng = "誤九種九牌";
                            return true;
                        }
                        break;
                    case YaoDingYi.KaiLiZhi:
                        if (!Chang.guiZe.kaiLiZhi)
                        {
                            CuHeSheng = "開立直不可";
                            return true;
                        }
                        break;
                    default:
                        CuHeSheng = "腰間違い";
                        return true;
                }
            }

            foreach (int stp in ShiTiPai)
            {
                if (stp == (ShouPai[ZiJiaXuanZe] & QIAO_PAI))
                {
                    CuHeSheng = "食い替え不可";
                    DaPai(ShouPai[ZiJiaXuanZe]);
                    return true;
                }
            }
            return false;
        }

        // 錯和他家判定
        internal bool CuHeTaJiaPanDing()
        {
            if (TaJiaXuanZe < 0)
            {
                CuHeSheng = "腰選択間違い";
                return true;
            }

            switch (TaJiaYao)
            {
                case YaoDingYi.Wu:
                    // 無
                    break;

                case YaoDingYi.Chi:
                    // 吃
                    if (ChiPaiWei.Count <= TaJiaXuanZe)
                    {
                        CuHeSheng = "吃不可";
                        return true;
                    }
                    break;
                case YaoDingYi.Bing:
                    // 石並
                    if (BingPaiWei.Count <= TaJiaXuanZe)
                    {
                        CuHeSheng = "石並不可";
                        return true;
                    }
                    break;
                case YaoDingYi.DaMingGang:
                    // 大明槓
                    if (DaMingGangPaiWei.Count <= TaJiaXuanZe)
                    {
                        CuHeSheng = "大明槓不可";
                        return true;
                    }
                    break;
                case YaoDingYi.RongHe:
                    // 栄和
                    if (!HeLe)
                    {
                        CuHeSheng = "誤栄和";
                        return true;
                    }
                    if (ZhenTingPanDing())
                    {
                        CuHeSheng = "振聴";
                        return true;
                    }
                    break;
                default:
                    CuHeSheng = "腰間違い";
                    return true;
            }

            return false;
        }
    }
}

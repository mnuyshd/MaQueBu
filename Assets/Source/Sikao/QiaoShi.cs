using System;
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
            if ((yao == YaoDingYi.JiaGang || yao == YaoDingYi.AnGang) && (anGangPaiWei.Count == 0 || jiaGangPaiWei.Count == 0))
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
        internal static Dictionary<YiManDingYi, string> YiManMing = new()
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
        }

        // 役名
        internal static Dictionary<YiDingYi, string> YiMing = new()
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
        private string mingQian = "";
        internal string MingQian
        {
            get { return mingQian; }
            set { mingQian = value; }
        }
        // 自家思考結果
        private YaoDingYi ziJiaYao;
        internal YaoDingYi ZiJiaYao
        {
            get { return ziJiaYao; }
            set { ziJiaYao = value; }
        }
        // 自家選択
        private int ziJiaXuanZe;
        internal int ZiJiaXuanZe
        {
            get { return ziJiaXuanZe; }
            set { ziJiaXuanZe = value; }
        }
        // 他家思考結果
        private YaoDingYi taJiaYao;
        internal YaoDingYi TaJiaYao
        {
            get { return taJiaYao; }
            set { taJiaYao = value; }
        }
        // 他家選択
        private int taJiaXuanZe;
        internal int TaJiaXuanZe
        {
            get { return taJiaXuanZe; }
            set { taJiaXuanZe = value; }
        }

        // プレイヤー
        private bool player = false;
        internal bool Player
        {
            get { return player; }
            set { player = value; }
        }
        // プレイヤー順(プレイヤーが必ず0となり、そこから順番にふられる)
        private int playOrder = -1;
        internal int PlayOrder
        {
            get { return playOrder; }
            set { playOrder = value; }
        }
        // 理牌
        private bool liPaiDongZuo = true;
        internal bool LiPaiDongZuo
        {
            get { return liPaiDongZuo; }
            set { liPaiDongZuo = value; }
        }
        // フォロー
        private bool follow = false;
        internal bool Follow
        {
            get { return follow; }
            set { follow = value; }
        }
        // 記録
        private Maqiao.JiLu jiLu;
        internal Maqiao.JiLu JiLu
        {
            get { return jiLu; }
            set { jiLu = value; }
        }

        // 役満
        private bool yiMan;
        internal bool YiMan
        {
            get { return yiMan; }
        }
        // 役・飜(役、飜数)
        private List<(int yi, int fanShu)> yiFan;
        internal List<(int yi, int fanShu)> YiFan
        {
            get { return yiFan; }
        }
        // 飜数計
        private int fanShuJi;
        internal int FanShuJi
        {
            get { return fanShuJi; }
        }
        // 符
        private int fu;
        internal int Fu
        {
            get { return fu; }
        }
        // 和了点
        private int heLeDian;
        internal int HeLeDian
        {
            get { return heLeDian; }
        }
        // 点棒
        private int dianBang;
        internal int DianBang
        {
            get { return dianBang; }
        }
        // 集計点
        private int jiJiDian;
        internal int JiJiDian
        {
            get { return jiJiDian; }
            set { jiJiDian = value; }
        }
        // 風
        private int feng;
        internal int Feng
        {
            get { return feng; }
        }
        // 手牌
        private List<int> shouPai;
        internal List<int> ShouPai
        {
            get { return shouPai; }
        }
        internal Button[] goShouPai;
        // 副露牌(牌、家、腰)
        private List<(List<int> pais, int jia, YaoDingYi yao)> fuLuPai;
        internal List<(List<int> pais, int jia, YaoDingYi yao)> FuLuPai
        {
            get { return fuLuPai; }
        }
        internal Button[][] goFuLuPai;
        // 包則番
        private int baoZeFan;
        internal int BaoZeFan
        {
            get { return baoZeFan; }
        }
        // 他家副露数
        private int taJiaFuLuShu;
        protected int TaJiaFuLuShu
        {
            get { return taJiaFuLuShu; }
        }
        // 捨牌(牌、腰、自摸切)
        private List<(int pai, YaoDingYi yao, bool ziMoQie)> shePai;
        internal List<(int pai, YaoDingYi yao, bool ziMoQie)> ShePai
        {
            get { return shePai; }
        }
        internal Button[] goShePai = new Button[0x50];
        // 立直位
        private int liZhiWei;
        internal int LiZhiWei
        {
            get { return liZhiWei; }
        }
        // 同順牌
        private List<int> tongShunPai = new();
        // 立直後牌
        private List<int> liZhiHouPai;
        // 待牌
        private List<int> daiPai;
        internal List<int> DaiPai
        {
            get { return daiPai; }
        }
        internal Button[] goDaiPai = new Button[13];
        // 残牌数
        internal TextMeshProUGUI[] goCanPaiShu = new TextMeshProUGUI[13];
        // 有効牌数
        private List<int> youXiaoPaiShu;
        protected List<int> YouXiaoPaiShu
        {
            get { return youXiaoPaiShu; }
        }
        // 向聴数
        private int xiangTingShu;
        internal int XiangTingShu
        {
            get { return xiangTingShu; }
        }
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
        private readonly int[] shouPaiShu = new int[0x40];
        internal int[] ShouPaiShu
        {
            get { return shouPaiShu; }
        }
        // 副露牌数
        private readonly int[] fuLuPaiShu = new int[0x40];
        internal int[] FuLuPaiShu
        {
            get { return fuLuPaiShu; }
        }
        // 捨牌数
        private readonly int[] shePaiShu = new int[0x40];
        internal int[] ShePaiShu
        {
            get { return shePaiShu; }
        }
        // 立直後捨牌数
        private readonly int[] liZhiShePaiShu = new int[0x40];
        internal int[] LiZhiShePaiShu
        {
            get { return liZhiShePaiShu; }
        }
        // 公開牌数
        private readonly int[] gongKaiPaiShu = new int[0x40];
        internal int[] GongKaiPaiShu
        {
            get { return gongKaiPaiShu; }
        }
        // 和了
        private bool heLe;
        internal bool HeLe
        {
            get { return heLe; }
        }
        // 立直
        private bool liZhi;
        internal bool LiZhi
        {
            get { return liZhi; }
        }
        // W立直
        private bool wLiZhi;
        // 一発
        private bool yiFa;
        internal bool YiFa
        {
            get { return yiFa; }
        }
        // 一巡目
        private bool yiXunMu;
        // 副露順
        private bool fuLuShun;
        // 形聴
        private bool xingTing;
        internal bool XingTing
        {
            get { return xingTing; }
        }
        // 自家
        private bool jiJia;
        internal bool JiJia
        {
            get { return jiJia; }
        }
        // 和了牌(牌、位、予想点)
        private List<(List<int> pais, int wei, int[] yuXiangDian)> heLePai;
        internal List<(List<int> pais, int wei, int[] yuXiangDian)> HeLePai
        {
            get { return heLePai; }
        }
        // 立直牌位
        private List<int> liZhiPaiWei;
        internal List<int> LiZhiPaiWei
        {
            get { return liZhiPaiWei; }
        }
        // 暗槓牌位
        private List<List<int>> anGangPaiWei;
        internal List<List<int>> AnGangPaiWei
        {
            get { return anGangPaiWei; }
        }
        // 加槓牌位
        private List<List<int>> jiaGangPaiWei;
        internal List<List<int>> JiaGangPaiWei
        {
            get { return jiaGangPaiWei; }
        }
        // 大明槓牌位
        private List<List<int>> daMingGangPaiWei;
        internal List<List<int>> DaMingGangPaiWei
        {
            get { return daMingGangPaiWei; }
        }
        // 石並牌位
        private List<List<int>> bingPaiWei;
        internal List<List<int>> BingPaiWei
        {
            get { return bingPaiWei; }
        }
        // 吃牌位
        private List<List<int>> chiPaiWei;
        internal List<List<int>> ChiPaiWei
        {
            get { return chiPaiWei; }
        }
        // 九種九牌
        private bool jiuZhongJiuPai;
        internal bool JiuZhongJiuPai
        {
            get { return jiuZhongJiuPai; }
        }
        // 受取
        private int shouQu;
        internal int ShouQu
        {
            get { return shouQu; }
        }
        // 受取(供託)
        private int shouQuGongTuo;
        internal int ShouQuGongTuo
        {
            get { return shouQuGongTuo; }
        }
        // 錯和声
        private string cuHeSheng;
        internal string CuHeSheng
        {
            get { return cuHeSheng; }
        }
        // 食替牌
        private List<int> shiTiPai = new();
        protected List<int> ShiTiPai
        {
            get { return shiTiPai; }
        }
        // 手牌点数
        protected int[] shouPaiDian;
        internal int[] ShouPaiDian
        {
            get { return shouPaiDian; }
        }
        // 連荘数
        private int lianZhuangShu;
        internal int LianZhuangShu
        {
            get { return lianZhuangShu; }
            set { lianZhuangShu = value; }
        }

        // コンストラクタ
        internal QiaoShi()
        {
            goShouPai = new Button[14];
            goFuLuPai = new Button[4][];
            for (int i = 0; i < goFuLuPai.Length; i++)
            {
                goFuLuPai[i] = new Button[4];
            }
            shouPaiDian = new int[14];
        }

        // コンストラクタ
        internal QiaoShi(string mingQian) : this()
        {
            this.mingQian = mingQian;
        }

        // 思考自家
        internal abstract void SiKaoZiJia();

        // 思考他家
        internal abstract void SiKaoTaJia(int jia);

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
            dianBang = Chang.guiZe.kaiShiDian;
            jiJiDian = 0;
        }

        // 局初期化
        internal void JuChuQiHua(int feng)
        {
            this.feng = feng;
            shouPai = new();
            fuLuPai = new();
            baoZeFan = -1;
            taJiaFuLuShu = 0;
            shePai = new();
            Init(shePaiShu, 0);
            Init(liZhiShePaiShu, 0);
            liZhiHouPai = new();
            liZhiWei = -1;
            daiPai = new();
            youXiaoPaiShu = new();
            xiangTingShu = 0;

            liZhi = false;
            wLiZhi = false;
            yiFa = false;
            yiXunMu = true;
            fuLuShun = false;

            shouQu = 0;
            shouQuGongTuo = 0;
            cuHeSheng = "";
        }

        // 思考自家判定
        internal void SiKaoZiJiaPanDing()
        {
            jiJia = true;

            ziJiaYao = YaoDingYi.Wu;
            ziJiaXuanZe = shouPai.Count - 1;

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
        }

        // 思考他家判定
        internal void SiKaoTaJiaPanDing(int jia)
        {
            jiJia = false;

            taJiaYao = YaoDingYi.Wu;
            taJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            shouPai.Add(Chang.ShePai);
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
            if (!liPaiDongZuo)
            {
                return;
            }
            Sort();
        }
        internal void Sort()
        {
            for (int i = 0; i < shouPai.Count; i++)
            {
                for (int j = i + 1; j < shouPai.Count; j++)
                {
                    if ((shouPai[i] & QIAO_PAI) > (shouPai[j] & QIAO_PAI))
                    {
                        (shouPai[i], shouPai[j]) = (shouPai[j], shouPai[i]);
                    }
                }
            }
        }

        // 自摸
        internal void ZiMo(int p)
        {
            shouPai.Add(p);
        }

        // 打牌前
        internal int DaPaiQian()
        {
            int p = shouPai[Chang.ZiJiaXuanZe];
            shouPai[Chang.ZiJiaXuanZe] = 0xff;
            return p;
        }
        // 打牌
        internal void DaPai(int p)
        {
            Chang.ShePai = p;
            shePaiShu[p & QIAO_PAI]++;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                if (i != Chang.ZiMoFan && shi.LiZhi)
                {
                    shi.liZhiShePaiShu[Chang.ShePai & QIAO_PAI]++;
                }
            }
            shouPai.RemoveAt(Chang.ZiJiaXuanZe);
            bool ziMoQie = false;
            if (ziJiaXuanZe == shouPai.Count)
            {
                if (ziJiaYao == YaoDingYi.Wu || ziJiaYao == YaoDingYi.LiZhi)
                {
                    ziMoQie = true;
                }
            }
            shePai.Add((Chang.ShePai, YaoDingYi.Wu, ziMoQie));
            tongShunPai = new List<int>();

            fuLuShun = false;
        }

        // 待牌計算
        internal void DaiPaiJiSuan(int ziJiaXuanZe)
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

        // 和了処理
        internal void HeLeChuLi()
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

        // 立直
        internal void LiZhiChuLi()
        {
            if (shePai.Count == 0)
            {
                wLiZhi = true;
            }
            liZhiWei = shePai.Count;
            liZhi = true;
            yiFa = true;
        }

        // 暗槓
        internal void AnGang(int wei)
        {
            List<int> pais = new();
            foreach (int w in anGangPaiWei[wei])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPai.Add((pais, 0, YaoDingYi.AnGang));

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
        internal void JiaGang(int wei)
        {
            for (int i = 0; i < fuLuPai.Count; i++)
            {
                (List<int> pais, int jia, YaoDingYi yao) = fuLuPai[i];
                if (yao == YaoDingYi.Bing)
                {
                    if ((pais[0] & QIAO_PAI) == (shouPai[jiaGangPaiWei[wei][0]] & QIAO_PAI))
                    {
                        fuLuPai[i] = (new() { pais[0], pais[1], pais[2], shouPai[jiaGangPaiWei[wei][0]] }, jia, YaoDingYi.JiaGang);
                        Chang.ShePai = shouPai[jiaGangPaiWei[wei][0]];
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
            if (baoZeFan >= 0)
            {
                return;
            }

            // 大四喜、大三元、四槓子
            int fengShu = 0;
            int sanYuanShu = 0;
            int gangShu = 0;
            foreach ((List<int> pais, _, YaoDingYi yao) in fuLuPai)
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
            if (fengShu == 3 || sanYuanShu == 2 || gangShu == 3)
            {
                (List<int> pais, int jia, YaoDingYi yao) = fuLuPai[^1];
                if (yao == YaoDingYi.DaMingGang || yao == YaoDingYi.Bing)
                {
                    int p = pais[0] & QIAO_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        baoZeFan = jia;
                        return;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        baoZeFan = jia;
                        return;
                    }
                    if (yao == YaoDingYi.AnGang || yao == YaoDingYi.JiaGang || yao == YaoDingYi.DaMingGang)
                    {
                        baoZeFan = jia;
                        return;
                    }
                }
            }
        }

        // 大明槓
        internal void DaMingGang()
        {
            List<int> pais = new();
            pais.Add(Chang.ShePai);
            foreach (int w in daMingGangPaiWei[Chang.TaJiaXuanZe])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPai.Add((pais, FuLuJiaJiSuan(), YaoDingYi.DaMingGang));

            List<int> paiWei = new(daMingGangPaiWei[Chang.TaJiaXuanZe]);
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
        internal void Bing()
        {
            List<int> pais = new();
            pais.Add(Chang.ShePai);
            foreach (int w in bingPaiWei[Chang.TaJiaXuanZe])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPai.Add((pais, FuLuJiaJiSuan(), YaoDingYi.Bing));

            List<int> paiWei = new(bingPaiWei[Chang.TaJiaXuanZe]);
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
        internal void Chi()
        {
            List<int> pais = new();
            pais.Add(Chang.ShePai);
            foreach (int w in chiPaiWei[Chang.TaJiaXuanZe])
            {
                pais.Add(shouPai[w]);
            }
            fuLuPai.Add((pais, FuLuJiaJiSuan(), YaoDingYi.Chi));

            List<int> paiWei = new(chiPaiWei[Chang.TaJiaXuanZe]);
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
        internal void ZhenTingPaiChuLi()
        {
            tongShunPai.Add(Chang.ShePai);
            if (liZhi)
            {
                liZhiHouPai.Add(Chang.ShePai);
            }
        }

        // 捨牌処理
        internal void ShePaiChuLi(YaoDingYi yao)
        {
            (int pai, YaoDingYi yao, bool ziMoQue) sp = shePai[^1];
            sp.yao = yao;
            shePai[^1] = sp;
        }

        // 立直処理
        internal void LiZiChuLi()
        {
            DianBangJiSuan(-1000, false);
        }

        // 流局
        internal bool LiuJu()
        {
            yiMan = false;

            yiFan = new();
            fanShuJi = 0;

            heLeDian = 0;

            // 流し満貫
            LiuManGuan();

            // 点計算
            if (yiFan.Count > 0)
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
                return true;
            }
            return false;
        }

        // 消
        internal void Xiao()
        {
            yiFa = false;
            yiXunMu = false;
        }

        // 形聴判定
        internal void XingTingPanDing()
        {
            xingTing = false;

            foreach (int p in Pai.QiaoPai)
            {
                shouPai.Add(p);
                if (HeLePanDing() >= Ting.XingTing)
                {
                    xingTing = true;
                    shouPai.RemoveAt(shouPai.Count - 1);
                    break;
                }
                shouPai.RemoveAt(shouPai.Count - 1);
            }
        }

        // 点棒計算
        internal void DianBangJiSuan(int dian)
        {
            DianBangJiSuan(dian, true);
        }
        internal void DianBangJiSuan(int dian, bool isShouQu)
        {
            dianBang += dian;
            if (isShouQu)
            {
                shouQu += dian;
            }
        }

        internal void ShouQuGongTuoJiSuan(int dian)
        {
            shouQuGongTuo = dian;
        }

        internal void JiJiDianJiSuan(int dian)
        {
            jiJiDian = dian;
        }

        // 思考前初期化
        private void SiKaoQianChuQiHua()
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

            Init(shouPaiDian, 0);
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
                if ((Pai.XuanShangPaiDingYi[x & QIAO_PAI]) == p)
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
                if (yiFan.Count > 0)
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
                if (yiFan.Count > 0)
                {
                    return Ting.TingPai;
                }
                // 役判定
                YiPanDing();
                if (yiFan.Count > 0)
                {
                    fu = 25;
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
            foreach (int sp in shouPai)
            {
                // 手牌数計算
                ShouPaiShuJiSuan();
                // 頭
                int t = sp & QIAO_PAI;
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
                        if (yiFan.Count > 0)
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
                            maxYiFan = new(yiFan);
                            maxYiShu = yiFan.Count;
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
                yiFan = new(maxYiFan);
            }
            return ret;
        }

        // 有効牌数計算
        internal void YouXiaoPaiShuJiSuan()
        {
            youXiaoPaiShu = new();
            int minXiangTingShu = 99;
            List<int> xiangTingShu = new();
            for (int i = 0; i < shouPai.Count; i++)
            {
                XiangTingShuJiSuan(i);
                xiangTingShu.Add(XiangTingShu);
                if (minXiangTingShu > XiangTingShu)
                {
                    minXiangTingShu = XiangTingShu;
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
                foreach (int p in Pai.QiaoPai)
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
                        if (minXiangTingShu > XiangTingShu)
                        {
                            youXiaoPai += Pai.CanShu(GongKaiPaiShu[p]);
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
        internal void XiangTingShuJiSuan()
        {
            int minXiangTing = 99;
            for (int i = 0; i < ShouPai.Count; i++)
            {
                XiangTingShuJiSuan(i, FuLuPai.Count);
                if (minXiangTing > xiangTingShu)
                {
                    minXiangTing = xiangTingShu;
                }
            }
            xiangTingShu = minXiangTing;

            // 手牌数計算(向聴数計算で手牌の一番右を切った状態で手牌数を計算して終わるので、再度 手牌数を計算しておく)
            ShouPaiShuJiSuan();

        }
        internal void XiangTingShuJiSuan(int wei)
        {
            XiangTingShuJiSuan(wei, 0);
        }
        internal void XiangTingShuJiSuan(int wei, int plusMianZiShu)
        {
            xiangTingShu = 99;
            int xiang;

            List<int> shouPaiC = new(shouPai);
            if (wei >= 0)
            {
                shouPai[wei] = 0xff;
            }
            shouPai.Sort();

            if (fuLuPai.Count == 0)
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
                    if (shouPaiShu[p] >= 2)
                    {
                        chongShu++;
                    }
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                xiangTingShu = 6 - duiZi.Count + (duiZi.Count - chongShu);

                // 国士無双向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();
                int shu = 0;
                bool chuHui = true;
                foreach (int p in Pai.YaoJiuPaiDingYi)
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
            foreach ((int yi, _) in yiFan)
            {
                if (yi == (int)YiDingYi.QiDuiZi)
                {
                    fu = 25;
                    return;
                }
                if (yi == (int)YiDingYi.PingHe)
                {
                    if (Chang.guiZe.ziMoPingHe && jiJia)
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
            foreach (List<int> pais in duiZi)
            {
                int p = pais[0] & QIAO_PAI;
                if (p == Chang.ChangFeng)
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
                        fu += (yaoJiu ? 8 : 4);
                        break;
                    case YaoDingYi.Bing:
                        // 明刻
                        fu += (yaoJiu ? 4 : 2);
                        break;
                    case YaoDingYi.AnGang:
                        // 暗槓
                        fu += (yaoJiu ? 32 : 16);
                        break;
                    case YaoDingYi.JiaGang:
                    case YaoDingYi.DaMingGang:
                        // 加槓・大明槓
                        fu += (yaoJiu ? 16 : 8);
                        break;
                }
            }
            // 待
            if (jiJia)
            {
                fu += 2;
            }
            int heLePai = shouPai[^1] & QIAO_PAI;
            bool daiDian = false;
            foreach (List<int> pais in duiZi)
            {
                if (pais[0] == heLePai)
                {
                    fu += 2;
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
                        fu += 2;
                        break;
                    }
                }
            }
            // 切上
            fu = Chang.Ceil(fu, 10);
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
            if (!yiMan)
            {
                if (fanShuJi < 5)
                {
                    heLeDian = (feng == 0x31 ? 6 : 4) * 2 * 2 * fu;
                    for (int i = 0; i < fanShuJi; i++)
                    {
                        heLeDian *= 2;
                    }
                    heLeDian = Chang.Ceil(heLeDian, 100);
                    if (feng == 0x31)
                    {
                        if (heLeDian > 12000)
                        {
                            heLeDian = 12000;
                        }
                    }
                    else
                    {
                        if (heLeDian > 8000)
                        {
                            heLeDian = 8000;
                        }
                    }
                }
                else if (fanShuJi == 5)
                {
                    // 満貫
                    heLeDian = (feng == 0x31 ? 12000 : 8000);
                }
                else if (fanShuJi == 6 || fanShuJi == 7)
                {
                    // 跳満
                    heLeDian = (feng == 0x31 ? 18000 : 12000);
                }
                else if (fanShuJi == 8 || fanShuJi == 9 || fanShuJi == 10)
                {
                    // 倍満
                    heLeDian = (feng == 0x31 ? 24000 : 16000);
                }
                else if (fanShuJi == 11 || fanShuJi == 12)
                {
                    // 三倍満
                    heLeDian = (feng == 0x31 ? 36000 : 24000);
                }
                else if (fanShuJi >= 13)
                {
                    // 役満
                    heLeDian = (feng == 0x31 ? 48000 : 32000);
                }

            }
            else
            {
                heLeDian = (feng == 0x31 ? 48000 : 32000) * fanShuJi;
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
            foreach (int p in Pai.YaoJiuPaiDingYi)
            {
                for (int j = 0; j < shouPai.Count; j++)
                {
                    int sp = shouPai[j] & QIAO_PAI;
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
        private bool ZhenTingPanDing()
        {
            foreach (int dp in daiPai)
            {
                // 振聴
                foreach ((int pai, _, _) in shePai)
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
            List<int> shouPaiC = new(shouPai);
            for (int i = 0; i < shouPai.Count; i++)
            {
                shouPai[i] = 0xff;
                shouPai.Sort();

                int wei = 0;
                List<int> hp = new();
                int[] dian = new int[0x40];
                Init(dian, 0);
                foreach (int p in Pai.QiaoPai)
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

                    if (Pai.CanShanPaiShu() >= 4 && taJiaFuLuShu == 0 && (dianBang >= 1000 || Chang.guiZe.jieJinLiZhi))
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

            foreach (int p in Pai.QiaoPai)
            {
                if (shouPaiShu[p] >= 4)
                {
                    List<int> anGang = new();
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (p == (shouPai[j] & QIAO_PAI))
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
                List<(List<int>, int, YaoDingYi)> fuLuPaiC = new(fuLuPai);

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
                fuLuPai = new(fuLuPaiC);
            }

            if (!heLe)
            {
                anGangPaiWei = new();
            }
        }

        // 加槓判定
        private void JiaGangPanDing()
        {
            foreach ((List<int> pais, _, YaoDingYi yao) in fuLuPai)
            {
                if (yao != YaoDingYi.Bing)
                {
                    continue;
                }
                for (int j = 0; j < shouPai.Count; j++)
                {
                    if ((pais[0] & QIAO_PAI) == (shouPai[j] & QIAO_PAI))
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

            int shePai = Chang.ShePai & QIAO_PAI;
            if (shouPaiShu[shePai] < 3)
            {
                return;
            }
            List<int> daMingGang = new();
            for (int i = 0; i < shouPai.Count; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
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

            int shePai = Chang.ShePai & QIAO_PAI;
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
                int p1 = shouPai[i] & QIAO_PAI;
                if (p1 == shePai)
                {
                    for (int j = i + 1; j < shouPai.Count; j++)
                    {
                        int p2 = shouPai[j] & QIAO_PAI;
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
            if (Chang.QiaoShis.Count <= 2)
            {
                return;
            }
            if (ZiPaiPanDing(Chang.ShePai))
            {
                return;
            }
            if (shouPai.Count < 3)
            {
                return;
            }
            int se = Chang.ShePai & SE_PAI;
            for (int i = 0; i < shouPai.Count - 1; i++)
            {
                int p1 = shouPai[i] & QIAO_PAI;
                int s1 = p1 & SHU_PAI;
                if (ZiPaiPanDing(p1) || ((p1 & SE_PAI) != se))
                {
                    continue;
                }
                for (int j = i + 1; j < shouPai.Count; j++)
                {
                    int p2 = shouPai[j] & QIAO_PAI;
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
                List<int> shouPaiC = new(shouPai);
                for (int j = 0; j < paiWei[i].Count; j++)
                {
                    ShouPai[paiWei[i][j]] = 0xff;
                }
                LiPai();
                int minXiangTing = 99;
                for (int j = 0; j < shouPai.Count - paiWei[i].Count; j++)
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
                shouPai = new(shouPaiC);
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

            (List<int> pais, _, _) = fuLuPai[^1];
            int mingPai = pais[0] & QIAO_PAI;
            shiTiPai.Add(mingPai);
            if ((mingPai & ZI_PAI) != ZI_PAI)
            {
                int p1 = pais[1] & QIAO_PAI;
                int p2 = pais[2] & QIAO_PAI;
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
            foreach (int p in Pai.YaoJiuPaiDingYi)
            {
                if (shouPaiShu[p] == 0)
                {
                    return false;
                }
                if (shouPaiShu[p] == 2)
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
            if (taJiaFuLuShu > 0)
            {
                return false;
            }
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 面子初期化
            MianZiChuQiHua();

            foreach (int p in Pai.QiaoPai)
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
            yiMan = false;

            yiFan = new();
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
            GongKongQiao();
            // 百万石・純正百万石
            BaiWanShi();
            if (Chang.guiZe.baLianZhuang && lianZhuangShu == 7)
            {
                // 八連荘
                YiZhuiJia(YiManDingYi.BaLianZhuang, 1);
            }

            fanShuJi = 0;
            foreach ((_, int fanShu) in yiFan)
            {
                fanShuJi += fanShu;
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
            if (duiZi.Count == 1 && keZi.Count == 0 && shunZi.Count == 0)
            {
                if ((shouPai[^1] & QIAO_PAI) == duiZi[0][0])
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
                if ((shouPai[^1] & QIAO_PAI) == duiZi[0][0])
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
            foreach (int sp in shouPai)
            {
                if (!ZiPaiPanDing(sp))
                {
                    return;
                }
            }
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            foreach (int sp in shouPai)
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
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            if (taJiaFuLuShu > 0)
            {
                return;
            }
            int[] jiuLian = new int[10];
            Init(jiuLian, 0);
            int se = -1;
            foreach (int sp in shouPai)
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
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            if (!Chang.guiZe.localYiMan)
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
            if (!Chang.guiZe.localYiMan)
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
        private void GongKongQiao()
        {
            if (!Chang.guiZe.localYiMan)
            {
                return;
            }
            foreach (int sp in shouPai)
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
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            foreach (int sp in shouPai)
            {
                int se = sp & SE_PAI;
                if (se != 0x00)
                {
                    return;
                }
                wanShi += sp & SHU_PAI;
            }

            foreach ((List<int> pais, int jia, YaoDingYi yao) in fuLuPai)
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

            yiMan = false;
            yiFan = new();
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
            foreach (int p in Pai.QiaoPai)
            {
                if (shouPaiShu[p] == 2)
                {
                    DuiZiJiSuan(p);
                    shouPaiShu[p] += 2;
                    break;
                }
            }
            foreach (int p in Pai.QiaoPai)
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
            yiFan = new();
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

            if (yiFan.Count > 0)
            {
                // 懸賞牌
                XuanShangPai();
                if (!jiJia)
                {
                    if (Chang.guiZe.yanFan && Chang.QiaoShis[Chang.ZiMoFan].ziJiaYao == YaoDingYi.LiZhi)
                    {
                        // 燕返し
                        YiZhuiJia(YiDingYi.YanFan, 1);
                    }
                }

                if (lianZhuangShu == 7)
                {
                    // 八連荘
                    yiFan = new();
                    YiZhuiJia(YiManDingYi.BaLianZhuang, 1);
                    yiMan = true;
                }
            }

            fanShuJi = 0;
            foreach ((_, int fanShu) in yiFan)
            {
                fanShuJi += fanShu;
            }
        }

        // 立直・Ｗ立直・一発
        private void LiZhiWLiZhiYiFa()
        {
            if (wLiZhi)
            {
                YiZhuiJia(YiDingYi.WLiZi, 2);
            }
            else if (liZhi)
            {
                YiZhuiJia(YiDingYi.LiZi, 1);
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
            if (shunZi.Count < 4)
            {
                return;
            }
            if (!Chang.guiZe.ziMoPingHe && jiJia)
            {
                // 自摸平和無し
                return;
            }
            int tou = duiZi[0][0] & QIAO_PAI;
            if (tou > 0x34 || tou == Chang.ChangFeng || tou == feng)
            {
                return;
            }
            int heLePai = shouPai[^1] & QIAO_PAI;
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
            foreach (int sp in shouPai)
            {
                int p = sp & QIAO_PAI;
                if (ZiPaiPanDing(p) || ((p & SHU_PAI) == 1) || ((p & SHU_PAI) == 9))
                {
                    return;
                }
            }
            if (!Chang.guiZe.shiDuan && fuLuPai.Count > 0)
            {
                // 喰断無し
                return;
            }
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            if (taJiaFuLuShu > 0)
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
            foreach (int sp in shouPai)
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
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            if (duiZi.Count == 7)
            {
                YiZhuiJia(YiDingYi.QiDuiZi, 2);
            }
        }

        // 流し満貫
        private void LiuManGuan()
        {
            foreach ((int pai, YaoDingYi yao, _) in shePai)
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
                xuan += shouPaiShu[Pai.XuanShangPaiDingYi[xp & QIAO_PAI]];
            }
            if (liZhi)
            {
                foreach (int lxp in Pai.LiXuanShangPai)
                {
                    xuan += shouPaiShu[Pai.XuanShangPaiDingYi[lxp & QIAO_PAI]];
                }
            }
            foreach (int sp in shouPai)
            {
                if ((sp & CHI_PAI) == CHI_PAI)
                {
                    xuan += 1;
                }
            }
            foreach ((List<int> pais, _, _) in fuLuPai)
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
            yiFan.Add(((int)ming, fan));
        }
        private void YiZhuiJia(YiDingYi ming, int fan)
        {
            yiFan.Add(((int)ming, fan));
        }

        // 対子計算
        protected void DuiZiJiSuan(int p, bool quanXiao = true)
        {
            while (shouPaiShu[p] >= 2)
            {
                shouPaiShu[p] -= 2;

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
            if (shouPaiShu[p] >= 3)
            {
                shouPaiShu[p] -= 3;

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
                while (shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1 && shouPaiShu[p + 2] >= 1)
                {
                    shouPaiShu[p]--;
                    shouPaiShu[p + 1]--;
                    shouPaiShu[p + 2]--;

                    shunZi.Add(new List<int>() { p, p + 1, p + 2 });
                }
            }
            if (s >= 2 && s <= 8)
            {
                while (shouPaiShu[p - 1] >= 1 && shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1)
                {
                    shouPaiShu[p - 1]--;
                    shouPaiShu[p]--;
                    shouPaiShu[p + 1]--;

                    shunZi.Add(new List<int>() { p - 1, p, p + 1 });
                }
            }
            if (s >= 3)
            {
                while (shouPaiShu[p - 2] >= 1 && shouPaiShu[p - 1] >= 1 && shouPaiShu[p] >= 1)
                {
                    shouPaiShu[p - 2]--;
                    shouPaiShu[p - 1]--;
                    shouPaiShu[p]--;

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

            while (shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1)
            {
                shouPaiShu[p]--;
                shouPaiShu[p + 1]--;

                taZi.Add(new List<int>() { p, p + 1 });
            }
            while (shouPaiShu[p] >= 1 && shouPaiShu[p + 2] >= 1)
            {
                shouPaiShu[p]--;
                shouPaiShu[p + 2]--;

                taZi.Add(new List<int>() { p, p + 2 });
            }
        }

        // 副露計算
        private void FuLuJiSuan()
        {
            foreach ((List<int> pais, _, YaoDingYi yao) in fuLuPai)
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

            int heLePai = shouPai[^1] & QIAO_PAI;
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
                if (keZi[i].yao == YaoDingYi.Wu && keZi[i].pais[0] == heLePai && !jiJia)
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
            Init(shouPaiShu, 0);
            foreach (int sp in shouPai)
            {
                shouPaiShu[sp & QIAO_PAI]++;
            }

            if (fuLu)
            {
                // 副露牌加算
                foreach ((List<int> pais, _, _) in fuLuPai)
                {
                    foreach (int fp in pais)
                    {
                        shouPaiShu[fp & QIAO_PAI]++;
                    }
                }
            }
        }

        // 公開牌数計算
        internal void GongKaiPaiShuJiSuan()
        {
            Init(gongKaiPaiShu, 0);
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                // 捨牌
                foreach ((int pai, _, _) in shi.ShePai)
                {
                    int p = pai & QIAO_PAI;
                    gongKaiPaiShu[p]++;
                }
                // 副露牌
                for (int i = 0; i < shi.fuLuPai.Count; i++)
                {
                    (List<int> pais, int jia, YaoDingYi yao) = shi.fuLuPai[i];
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
                            gongKaiPaiShu[p]++;
                        }
                        wei++;
                    }
                }
            }
            // 懸賞牌
            foreach (int pai in Pai.XuanShangPai)
            {
                gongKaiPaiShu[pai & QIAO_PAI]++;
            }
            // 手牌
            foreach (int pai in shouPai)
            {
                gongKaiPaiShu[pai & QIAO_PAI]++;
            }
        }

        // 副露牌数計算
        protected void FuLuPaiShuSuan()
        {
            Init(fuLuPaiShu, 0);
            foreach ((List<int> pais, _, _) in fuLuPai)
            {
                foreach (int fp in pais)
                {
                    fuLuPaiShu[fp & QIAO_PAI]++;
                }
            }
        }

        // 錯和自家判定
        internal bool CuHeZiJiaPanDing()
        {
            if (ziJiaXuanZe < 0)
            {
                cuHeSheng = "打牌選択違い";
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
                        if (!jiuZhongJiuPai)
                        {
                            cuHeSheng = "誤九種九牌";
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
                if (stp == (shouPai[ziJiaXuanZe] & QIAO_PAI))
                {
                    cuHeSheng = "食い替え不可";
                    DaPai(shouPai[ziJiaXuanZe]);
                    return true;
                }
            }

            return false;
        }

        // 錯和他家判定
        internal bool CuHeTaJiaPanDing()
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
}

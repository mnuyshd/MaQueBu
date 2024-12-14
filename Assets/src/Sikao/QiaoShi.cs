using System;
using UnityEngine.UI;
using TMPro;

using Gongtong;

namespace Sikao
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

        // 腰名
        private static readonly string[] YaoMingDingYi = new string[] {
            "", "チー", "ポン", "カン", "カン", "カン", "リーチ", "ツモ", "ロン", "九種九牌",
            "テンパイ", "ノーテン", "和了", "四開槓", "四家立直", "流し満貫", "四風子連打", "チョンボ", "", "", ""
        };
        internal static string YaoMing(Chang.YaoDingYi yao)
        {
            return YaoMingDingYi[(int)yao];
        }
        // ボタン腰名
        private static readonly string[] YaoMingButtonDingYi = new string[] {
            "パス", "チー", "ポン", "カン", "加槓", "暗槓", "立直", "ツモ", "ロン", "九種",
            "", "", "", "", "", "", "", "", "", "", "取消"
        };
        internal string YaoMingButton(Chang.YaoDingYi yao)
        {
            if ((yao == Chang.YaoDingYi.JiaGang || yao == Chang.YaoDingYi.AnGang) && (anGangKeNengShu == 0 || jiaGangKeNengShu == 0))
            {
                // 加槓、暗槓で片方のみ可能な場合、ボタン名は「カン」
                yao = Chang.YaoDingYi.DaMingGang;
            }
            return YaoMingButtonDingYi[(int)yao];
        }

        // 役満名
        internal static readonly string[] YiManMing = new string[] {
            "天和", "人和", "地和", "国士無双", "国士無双十三面", "四暗刻", "四暗刻単騎", "四槓子", "四連刻", "大三元",
            "小四喜", "大四喜", "字一色", "清老頭", "九連宝燈", "純正九連宝燈", "緑一色", "大車輪", "十三不塔"
        };
        // 役名
        internal static readonly string[] YiMing = new string[] {
            "立直", "Ｗ立直", "一発", "海底撈月", "河底撈魚", "嶺上開花", "槍槓", "面前清自摸和", "平和", "断幺九",
            "一盃口", "二盃口", "一気通貫", "三色同順", "全帯幺", "純全帯", "混老頭", "三暗刻", "三槓子", "三連刻",
            "小三元", "混一色", "清一色", "対々和", "役牌", "七対子", "ドラ", "流し満貫"
        };
        // 得点役
        internal static readonly string[] DeDianYi = new string[] {
            "", "", "", "", "", "満貫", "跳満", "跳満", "倍満", "倍満", "倍満", "三倍満", "三倍満", "役満"
        };

        // 役満
        private enum YiManDingYi
        {
            // 天和
            TianHe = 0,
            // 人和
            RenHe = 1,
            // 地和
            DeHe = 2,
            // 国士無双
            GuoShiWuShuang = 3,
            // 国士無双十三面
            GuoShiShiSanMian = 4,
            // 四暗刻
            SiAnKe = 5,
            // 四暗刻単騎
            SiAnKeDanQi = 6,
            // 四槓子
            SiGangZi = 7,
            // 四連刻
            SiLianKe = 8,
            // 大三元
            DaSanYuan = 9,
            // 小四喜
            XiaoSiXi = 10,
            // 大四喜
            DaSiXi = 11,
            // 字一色
            ZiYiSe = 12,
            // 清老頭
            QingLaoTou = 13,
            // 九連宝燈
            JiuLianBaoDegn = 14,
            // 純正九連宝燈
            ChunZhengJiuLian = 15,
            // 緑一色
            LuYiSe = 16,
            // 大車輪
            DaCheLun = 17,
            // 十三不塔
            ShiSanBuTa = 18,
        }

        // 役
        private enum YiDingYi
        {
            // 立直
            LiZi = 0,
            // Ｗ立直
            WLiZi = 1,
            // 一発
            YiFa = 2,
            // 海底撈月
            HaiDiLaoYue = 3,
            // 河底撈魚
            HeDiLaoYu = 4,
            // 嶺上開花
            LingShangKaiHua = 5,
            // 槍槓
            QiangGang = 6,
            // 面前清自摸和
            MianQianQingZiMoHe = 7,
            // 平和
            PingHe = 8,
            // 断幺九
            DuanYaoJiu = 9,
            // 一盃口
            YiBeiKou = 10,
            // 二盃口
            ErBeiKou = 11,
            // 一気通貫
            YiQiTongGuan = 12,
            // 三色同順
            SanSeTongShun = 13,
            // 全帯幺
            QuanDaiYao = 14,
            // 純全帯
            ChunQuanDai = 15,
            // 混老頭
            HunLaoTou = 16,
            // 三暗刻
            SanAnKe = 17,
            // 三槓子
            SanGangZi = 18,
            // 三連刻
            SanLianKe = 19,
            // 小三元
            XiaoSanYuan = 20,
            // 混一色
            HunYiSe = 21,
            // 清一色
            QingYiSe = 22,
            // 対々和
            DuiDuiHe = 23,
            // 役牌
            YiPai = 24,
            // 七対子
            QiDuiZi = 25,
            // 懸賞
            XuanShang = 26,
            // 流し満貫
            LiuManGuan = 27,
        }

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
        private Chang.YaoDingYi ziJiaYao;
        internal Chang.YaoDingYi ZiJiaYao
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
        private Chang.YaoDingYi taJiaYao;
        internal Chang.YaoDingYi TaJiaYao
        {
            get { return taJiaYao; }
            set { taJiaYao = value; }
        }
        // 他家選択
        private int taJiaXuanZe;
        internal int TaJiaXuanZe
        {
            get { return taJiaXuanZe; }
            set { taJiaXuanZe= value; }
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
        // 役
        private readonly int[] yi;
        internal int[] Yi
        {
            get { return yi; }
        }
        // 役数
        private int yiShu;
        internal int YiShu
        {
            get { return yiShu; }
        }
        // 飜数
        private readonly int[] fanShu;
        internal int[] FanShu
        {
            get { return fanShu; }
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
        private readonly int[] shouPai;
        internal int[] ShouPai
        {
            get { return shouPai; }
        }
        internal Button[] goShouPai;
        // 手牌位
        private int shouPaiWei;
        internal int ShouPaiWei
        {
            get { return shouPaiWei; }
        }
        // 副露牌
        private readonly int[][] fuLuPai;
        internal int[][] FuLuPai
        {
            get { return fuLuPai; }
        }
        internal Button[][] goFuLuPai;
        // 副露家
        private readonly int[] fuLuJia;
        internal int[] FuLuJia
        {
            get { return fuLuJia; }
        }
        // 副露種
        private readonly Chang.YaoDingYi[] fuLuZhong;
        internal Chang.YaoDingYi[] FuLuZhong
        {
            get { return fuLuZhong; }
        }
        // 副露牌位
        private int fuLuPaiWei;
        internal int FuLuPaiWei
        {
            get { return fuLuPaiWei; }
        }
        // 包則番
        private int baoZeFan;
        internal int BaoZeFan
        {
            get { return baoZeFan; }
        }
        // 他家副露数
        private int taJiaFuLuShu;
        internal int TaJiaFuLuShu
        {
            get { return taJiaFuLuShu; }
        }
        // 捨牌
        private readonly int[] shePai;
        internal int[] ShePai
        {
            get { return shePai; }
        }
        internal Button[] goShePai;
        // 捨牌位
        private int shePaiWei;
        internal int ShePaiWei
        {
            get { return shePaiWei; }
        }
        // 捨牌腰
        private readonly Chang.YaoDingYi[] shePaiYao;
        internal Chang.YaoDingYi[] ShePaiYao
        {
            get { return shePaiYao; }
        }
        // 捨牌自摸切
        private readonly bool[] shePaiZiMoQie;
        internal bool[] ShePaiZiMoQie
        {
            get { return shePaiZiMoQie; }
        }
        // 立直位
        private int liZhiWei;
        internal int LiZhiWei
        {
            get { return liZhiWei; }
        }
        // 同順牌
        private readonly int[] tongShunPai;
        internal int[] TongShunPai
        {
            get { return tongShunPai; }
        }
        // 同順牌位
        private int tongShunPaiWei;
        internal int TongShunPaiWei
        {
            get { return tongShunPaiWei; }
        }
        // 立直後牌
        private readonly int[] liZhiHouPai;
        internal int[] LiZhiHouPai
        {
            get { return liZhiHouPai; }
        }
        // 立直後牌位
        private int liZhiHouPaiWei;
        internal int LiZhiHouPaiWei
        {
            get { return liZhiHouPaiWei; }
        }
        // 待牌
        private readonly int[] daiPai;
        internal int[] DaiPai
        {
            get { return daiPai; }
        }
        internal Button[] goDaiPai;
        // 残牌数
        internal TextMeshProUGUI[] goCanPaiShu;
        // 待牌数
        private int daiPaiShu;
        internal int DaiPaiShu
        {
            get { return daiPaiShu; }
        }
        // 有効牌数
        private readonly int[] youXiaoPaiShu;
        internal int[] YouXiaoPaiShu
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
        private readonly int[][] duiZi;
        internal int[][] DuiZi
        {
            get { return duiZi; }
        }
        // 対子数
        private int duiZiShu;
        internal int DuiZiShu
        {
            get { return duiZiShu; }
        }
        // 刻子
        private readonly int[][] keZi;
        internal int[][] KeZi
        {
            get { return keZi; }
        }
        // 刻子数
        private int keZiShu;
        internal int KeZiShu
        {
            get { return keZiShu; }
        }
        // 刻子種
        internal readonly Chang.YaoDingYi[] keZiZhong;
        internal Chang.YaoDingYi[] KeZiZhong
        {
            get { return keZiZhong; }
        }
        // 順子
        private readonly int[][] shunZi;
        internal int[][] ShunZi
        {
            get { return shunZi; }
        }
        // 順子数
        private int shunZiShu;
        internal int ShunZiShu
        {
            get { return shunZiShu; }
        }
        // 塔子
        private readonly int[][] taZi;
        internal int[][] TaZi
        {
            get { return taZi; }
        }
        // 塔子数
        private int taZiShu;
        internal int TaZiShu
        {
            get { return taZiShu; }
        }
        // 手牌数
        private readonly int[] shouPaiShu;
        internal int[] ShouPaiShu
        {
            get { return shouPaiShu; }
        }
        // 副露牌数
        private readonly int[] fuLuPaiShu;
        internal int[] FuLuPaiShu
        {
            get { return fuLuPaiShu; }
        }
        // 捨牌数
        private readonly int[] shePaiShu;
        internal int[] ShePaiShu
        {
            get { return shePaiShu; }
        }
        // 公開牌数
        private readonly int[] gongKaiPaiShu;
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
        internal bool WLiZhi
        {
            get { return wLiZhi; }
        }
        // 一発
        private bool yiFa;
        internal bool YiFa
        {
            get { return yiFa; }
        }
        // 一巡目
        private bool yiXunMu;
        internal bool YiXunMu
        {
            get { return yiXunMu; }
        }
        // 副露順
        private bool fuLuShun;
        internal bool FuLuShun
        {
            get { return fuLuShun; }
        }
        // 振聴
        private bool zhenTing;
        internal bool ZhenTing
        {
            get { return zhenTing; }
        }
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
        // 和了牌
        private readonly int[][] heLePai;
        internal int[][] HeLePai
        {
            get { return heLePai; }
        }
        // 和了牌位
        private readonly int[] heLePaiWei;
        internal int[] HeLePaiWei
        {
            get { return heLePaiWei; }
        }
        // 和了可能数
        private int heLeKeNengShu;
        internal int HeLeKeNengShu
        {
            get { return heLeKeNengShu; }
        }
        // 立直牌位
        private readonly int[] liZhiPaiWei;
        internal int[] LiZhiPaiWei
        {
            get { return liZhiPaiWei; }
        }
        // 立直可能数
        private int liZhiKeNengShu;
        internal int LiZhiKeNengShu
        {
            get { return liZhiKeNengShu; }
        }
        // 暗槓牌位
        private readonly int[][] anGangPaiWei;
        internal int[][] AnGangPaiWei
        {
            get { return anGangPaiWei; }
        }
        // 暗槓可能数
        private int anGangKeNengShu;
        internal int AnGangKeNengShu
        {
            get { return anGangKeNengShu; }
        }
        // 加槓牌位
        private readonly int[][] jiaGangPaiWei;
        internal int[][] JiaGangPaiWei
        {
            get { return jiaGangPaiWei; }
        }
        // 加槓可能数
        private int jiaGangKeNengShu;
        internal int JiaGangKeNengShu
        {
            get { return jiaGangKeNengShu; }
        }
        // 大明槓牌位
        private readonly int[][] daMingGangPaiWei;
        internal int[][] DaMingGangPaiWei
        {
            get { return daMingGangPaiWei; }
        }
        // 大明槓可能数
        private int daMingGangKeNengShu;
        internal int DaMingGangKeNengShu
        {
            get { return daMingGangKeNengShu; }
        }
        // 石並牌位
        private readonly int[][] bingPaiWei;
        internal int[][] BingPaiWei
        {
            get { return bingPaiWei; }
        }
        // 石並可能数
        private int bingKeNengShu;
        internal int BingKeNengShu
        {
            get { return bingKeNengShu; }
        }
        // 吃牌位
        private readonly int[][] chiPaiWei;
        internal int[][] ChiPaiWei
        {
            get { return chiPaiWei; }
        }
        // 吃可能数
        private int chiKeNengShu;
        internal int ChiKeNengShu
        {
            get { return chiKeNengShu; }
        }
        // 九種九牌
        private bool jiuZhongJiuPai;
        internal bool JiuZhongJiuPai
        {
            get { return jiuZhongJiuPai; }
        }
        // 自摸牌
        private readonly int[] ziMoPai;
        internal int[] ZiMoPai
        {
            get { return ziMoPai; }
        }
        // 腰
        private readonly Chang.YaoDingYi[] yao;
        internal Chang.YaoDingYi[] Yao
        {
            get { return yao; }
        }
        // 自摸牌位
        private int ziMoPaiWei;
        internal int ZiMoPaiWei
        {
            get { return ziMoPaiWei; }
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
        private readonly int[] shiTiPai;
        internal int[] ShiTiPai
        {
            get { return shiTiPai; }
        }
        // 食替牌数
        private int shiTiPaiShu;
        internal int ShiTiPaiShu
        {
            get { return shiTiPaiShu; }
        }

        // コンストラクタ
        internal QiaoShi()
        {
            yi = new int[0x10];
            fanShu = new int[yi.Length];
            shouPai = new int[14];
            goShouPai = new Button[shouPai.Length];
            fuLuPai = new int[4][];
            goFuLuPai = new Button[fuLuPai.Length][];
            for (int i = 0; i < fuLuPai.Length; i++)
            {
                fuLuPai[i] = new int[4];
                goFuLuPai[i] = new Button[fuLuPai[i].Length];
            }
            fuLuJia = new int[fuLuPai.Length];
            fuLuZhong = new Chang.YaoDingYi[fuLuPai.Length];
            shePai = new int[0x50];
            goShePai = new Button[shePai.Length];
            shePaiYao = new Chang.YaoDingYi[shePai.Length];
            shePaiZiMoQie = new bool[shePai.Length];
            liZhiHouPai = new int[shePai.Length * 4];
            tongShunPai = new int[0x20];
            daiPai = new int[13];
            goDaiPai = new Button[daiPai.Length];
            goCanPaiShu = new TextMeshProUGUI[daiPai.Length];
            youXiaoPaiShu = new int[shouPai.Length];
            duiZi = new int[7][];
            for (int i = 0; i < duiZi.Length; i++)
            {
                duiZi[i] = new int[2];
            }
            keZi = new int[4][];
            for (int i = 0; i < keZi.Length; i++)
            {
                keZi[i] = new int[4];
            }
            shunZi = new int[4][];
            for (int i = 0; i < shunZi.Length; i++)
            {
                shunZi[i] = new int[3];
            }
            taZi = new int[7][];
            for (int i = 0; i < taZi.Length; i++)
            {
                taZi[i] = new int[2];
            }
            keZiZhong = new Chang.YaoDingYi[4];
            shouPaiShu = new int[0x40];
            fuLuPaiShu = new int[0x40];
            shePaiShu = new int[0x40];
            gongKaiPaiShu = new int[0x40];
            heLePai = new int[shouPai.Length][];
            for (int i = 0; i < heLePai.Length; i++)
            {
                heLePai[i] = new int[Pai.QiaoPai.Length];
            }
            heLePaiWei = new int[heLePai.Length];
            liZhiPaiWei = new int[shouPai.Length];
            anGangPaiWei = new int[3][];
            for (int i = 0; i < anGangPaiWei.Length; i++)
            {
                anGangPaiWei[i] = new int[4];
            }
            jiaGangPaiWei = new int[3][];
            for (int i = 0; i < jiaGangPaiWei.Length; i++)
            {
                jiaGangPaiWei[i] = new int[1];
            }
            daMingGangPaiWei = new int[1][];
            for (int i = 0; i < daMingGangPaiWei.Length; i++)
            {
                daMingGangPaiWei[i] = new int[3];
            }
            bingPaiWei = new int[3][];
            for (int i = 0; i < bingPaiWei.Length; i++)
            {
                bingPaiWei[i] = new int[2];
            }
            chiPaiWei = new int[16][];
            for (int i = 0; i < chiPaiWei.Length; i++)
            {
                chiPaiWei[i] = new int[2];
            }
            ziMoPai = new int[0x50];
            yao = new Chang.YaoDingYi[0x50];
            shiTiPai = new int[2];
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

        // 荘初期化
        internal void ZhuangChuQiHua()
        {
            dianBang = GuiZe.kaiShiDian;
            jiJiDian = 0;
        }

        // 局初期化
        internal void JuChuQiHua(int feng)
        {
            this.feng = feng;
            Chang.Init(shouPai, 0xff);
            shouPaiWei = 0;
            Chang.Init(fuLuPai, 0xff);
            Chang.Init(fuLuJia, 0);
            Chang.Init(fuLuZhong, Chang.YaoDingYi.Wu);
            baoZeFan = -1;
            fuLuPaiWei = 0;
            taJiaFuLuShu = 0;
            Chang.Init(shePai, 0xff);
            shePaiWei = 0;
            Chang.Init(shePaiYao, Chang.YaoDingYi.Wu);
            Chang.Init(shePaiZiMoQie, false);
            Chang.Init(shePaiShu, 0);
            Chang.Init(liZhiHouPai, 0xff);
            liZhiHouPaiWei = 0;
            liZhiWei = -1;
            Chang.Init(daiPai, 0xff);
            daiPaiShu = 0;
            Chang.Init(youXiaoPaiShu, 0);
            xiangTingShu = 0;

            liZhi = false;
            wLiZhi = false;
            yiFa = false;
            yiXunMu = true;
            fuLuShun = false;

            Chang.Init(ziMoPai, 0xff);
            Chang.Init(yao, Chang.YaoDingYi.Wu);
            ziMoPaiWei = 0;
            shouQu = 0;
            shouQuGongTuo = 0;
            cuHeSheng = "";
        }

        // 思考自家判定
        internal void SiKaoZiJiaPanDing()
        {
            jiJia = true;

            ziJiaYao = Chang.YaoDingYi.Wu;
            ziJiaXuanZe = shouPaiWei - 1;

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

            if (Pai.XuanShangPaiShu() <= 4 && Pai.CanShanPaiShu() >= Chang.MianZi)
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

            taJiaYao = Chang.YaoDingYi.Wu;
            taJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            shouPai[shouPaiWei++] = Chang.ShePai;
            // 和了判定
            if (HeLePanDing() == Ting.TingPai)
            {
                // 振聴判定
                ZhenTingPanDing();
                if (!zhenTing)
                {
                    heLe = true;
                }
            }
            shouPai[--shouPaiWei] = 0xff;

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

        // ソート
        internal static void Sort(int[] list)
        {
            Sort(list, true);
        }
        internal static void Sort(int[] list, bool isLiPai)
        {
            for (int i = 0; i < list.Length - 1; i++)
            {
                for (int j = i + 1; j < list.Length; j++)
                {
                    if ((isLiPai && (list[i] & QIAO_PAI) > (list[j] & QIAO_PAI)) || (!isLiPai && list[i] == 0xff))
                    {
                        int t = list[i];
                        list[i] = list[j];
                        list[j] = t;
                    }
                }
            }
        }

        // 理牌
        internal void LiPai()
        {
            Sort(shouPai, liPaiDongZuo);
        }

        // 自摸
        internal void ZiMo(int p)
        {
            shouPai[shouPaiWei++] = p;

            ziMoPai[ziMoPaiWei++] = p;
        }

        // 打牌
        internal void DaPai()
        {
            Chang.ShePai = shouPai[Chang.ZiJiaXuanZe];
            shePaiShu[Chang.ShePai & QIAO_PAI]++;
            shouPai[Chang.ZiJiaXuanZe] = 0xff;
            shePai[shePaiWei] = Chang.ShePai;
            if (ziJiaXuanZe == shouPaiWei - 1)
            {
                Chang.YaoDingYi y = yao[ziMoPaiWei - 1];
                if (y == Chang.YaoDingYi.Wu || y == Chang.YaoDingYi.LiZhi)
                {
                    shePaiZiMoQie[shePaiWei] = true;
                }
            }
            shouPaiWei--;
            shePaiWei++;
            Chang.Init(tongShunPai, 0xff);
            tongShunPaiWei = 0;

            fuLuShun = false;
        }

        // 待牌計算
        internal void DaiPaiJiSuan(int ziJiaXuanZe)
        {
            Chang.Init(daiPai, 0xff);
            daiPaiShu = 0;
            for (int i = 0; i < heLeKeNengShu; i++)
            {
                if (heLePaiWei[i] == ziJiaXuanZe)
                {
                    for (int j = 0; j < heLePai[i].Length; j++)
                    {
                        if (heLePai[i][j] == 0xff)
                        {
                            return;
                        }
                        daiPai[j] = heLePai[i][j];
                        daiPaiShu++;
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

            if (Chang.ZiJiaYao == Chang.YaoDingYi.ZiMo)
            {
                yao[ziMoPaiWei - 1] = Chang.YaoDingYi.ZiMo;
            }
            else if (Chang.TaJiaYao == Chang.YaoDingYi.RongHe)
            {
                yao[ziMoPaiWei - 1] = Chang.YaoDingYi.RongHe;
            }
        }

        // 立直
        internal void LiZi()
        {
            if (shePaiWei == 0)
            {
                wLiZhi = true;
            }
            liZhiWei = shePaiWei;
            liZhi = true;
            yiFa = true;
        }

        // 暗槓
        internal void AnGang(int wei)
        {
            fuLuPai[fuLuPaiWei][0] = shouPai[anGangPaiWei[wei][0]];
            fuLuPai[fuLuPaiWei][1] = shouPai[anGangPaiWei[wei][1]];
            fuLuPai[fuLuPaiWei][2] = shouPai[anGangPaiWei[wei][2]];
            fuLuPai[fuLuPaiWei][3] = shouPai[anGangPaiWei[wei][3]];
            fuLuJia[fuLuPaiWei] = 0;
            fuLuZhong[fuLuPaiWei] = Chang.YaoDingYi.AnGang;

            shouPai[anGangPaiWei[wei][0]] = 0xff;
            shouPai[anGangPaiWei[wei][1]] = 0xff;
            shouPai[anGangPaiWei[wei][2]] = 0xff;
            shouPai[anGangPaiWei[wei][3]] = 0xff;

            fuLuPaiWei++;
            shouPaiWei -= 4;

            // 理牌
            LiPai();

            yao[ziMoPaiWei - 1] = Chang.YaoDingYi.AnGang;
        }

        // 加槓
        internal void JiaGang(int wei)
        {
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                if (fuLuZhong[i] == Chang.YaoDingYi.Bing)
                {
                    if ((fuLuPai[i][0] & QIAO_PAI) == (shouPai[jiaGangPaiWei[wei][0]] & QIAO_PAI))
                    {
                        fuLuPai[i][3] = shouPai[jiaGangPaiWei[wei][0]];
                        fuLuZhong[i] = Chang.YaoDingYi.JiaGang;
                        Chang.ShePai = shouPai[jiaGangPaiWei[wei][0]];
                        shouPai[jiaGangPaiWei[wei][0]] = 0xff;
                        shouPaiWei--;
                        // 理牌
                        LiPai();

                        yao[ziMoPaiWei - 1] = Chang.YaoDingYi.JiaGang;
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
                mingFan += Chang.MianZi;
            }
            return mingFan - ziMoFan;
        }

        // 包則判定
        private void BaoZePanDing()
        {
            if (!GuiZe.baoZe)
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
            for (int i = 0; i < fuLuPaiWei - 1; i++)
            {
                if (fuLuZhong[i] == Chang.YaoDingYi.DaMingGang || fuLuZhong[i] == Chang.YaoDingYi.Bing)
                {
                    int p = fuLuPai[i][0] & QIAO_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        fengShu++;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        sanYuanShu++;
                    }
                }
                if (fuLuZhong[i] == Chang.YaoDingYi.AnGang || fuLuZhong[i] == Chang.YaoDingYi.JiaGang || fuLuZhong[i] == Chang.YaoDingYi.DaMingGang)
                {
                    gangShu++;
                }
            }
            if (fengShu == 3 || sanYuanShu == 2 || gangShu == 3)
            {
                int wei = fuLuPaiWei - 1;
                if (fuLuZhong[wei] == Chang.YaoDingYi.DaMingGang || fuLuZhong[wei] == Chang.YaoDingYi.Bing)
                {
                    int p = fuLuPai[wei][0] & QIAO_PAI;
                    if (p >= 0x31 && p <= 0x34)
                    {
                        baoZeFan = fuLuJia[wei];
                        return;
                    }
                    if (p >= 0x35 && p <= 0x37)
                    {
                        baoZeFan = fuLuJia[wei];
                        return;
                    }
                    if (fuLuZhong[wei] == Chang.YaoDingYi.AnGang || fuLuZhong[wei] == Chang.YaoDingYi.JiaGang || fuLuZhong[wei] == Chang.YaoDingYi.DaMingGang)
                    {
                        baoZeFan = fuLuJia[wei];
                        return;
                    }
                }
            }
        }

        // 大明槓
        internal void DaMingGang()
        {
            fuLuPai[fuLuPaiWei][0] = Chang.ShePai;
            fuLuPai[fuLuPaiWei][1] = shouPai[daMingGangPaiWei[Chang.TaJiaXuanZe][0]];
            fuLuPai[fuLuPaiWei][2] = shouPai[daMingGangPaiWei[Chang.TaJiaXuanZe][1]];
            fuLuPai[fuLuPaiWei][3] = shouPai[daMingGangPaiWei[Chang.TaJiaXuanZe][2]];
            fuLuJia[fuLuPaiWei] = FuLuJiaJiSuan();
            fuLuZhong[fuLuPaiWei] = Chang.YaoDingYi.DaMingGang;

            shouPai[daMingGangPaiWei[Chang.TaJiaXuanZe][0]] = 0xff;
            shouPai[daMingGangPaiWei[Chang.TaJiaXuanZe][1]] = 0xff;
            shouPai[daMingGangPaiWei[Chang.TaJiaXuanZe][2]] = 0xff;

            fuLuPaiWei++;
            taJiaFuLuShu++;
            shouPaiWei -= 3;
            // 理牌
            LiPai();

            ziMoPai[ziMoPaiWei] = Chang.ShePai;
            yao[ziMoPaiWei] = Chang.YaoDingYi.DaMingGang;
            ziMoPaiWei++;

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 石並
        internal void Bing()
        {
            fuLuPai[fuLuPaiWei][0] = Chang.ShePai;
            fuLuPai[fuLuPaiWei][1] = shouPai[bingPaiWei[Chang.TaJiaXuanZe][0]];
            fuLuPai[fuLuPaiWei][2] = shouPai[bingPaiWei[Chang.TaJiaXuanZe][1]];
            fuLuJia[fuLuPaiWei] = FuLuJiaJiSuan();
            fuLuZhong[fuLuPaiWei] = Chang.YaoDingYi.Bing;

            shouPai[bingPaiWei[Chang.TaJiaXuanZe][0]] = 0xff;
            shouPai[bingPaiWei[Chang.TaJiaXuanZe][1]] = 0xff;

            fuLuPaiWei++;
            taJiaFuLuShu++;
            shouPaiWei -= 2;
            // 理牌
            LiPai();

            ziMoPai[ziMoPaiWei] = Chang.ShePai;
            yao[ziMoPaiWei] = Chang.YaoDingYi.Bing;
            ziMoPaiWei++;

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 吃
        internal void Chi()
        {
            fuLuPai[fuLuPaiWei][0] = Chang.ShePai;
            fuLuPai[fuLuPaiWei][1] = shouPai[chiPaiWei[Chang.TaJiaXuanZe][0]];
            fuLuPai[fuLuPaiWei][2] = shouPai[chiPaiWei[Chang.TaJiaXuanZe][1]];
            fuLuJia[fuLuPaiWei] = FuLuJiaJiSuan();
            fuLuZhong[fuLuPaiWei] = Chang.YaoDingYi.Chi;

            shouPai[chiPaiWei[Chang.TaJiaXuanZe][0]] = 0xff;
            shouPai[chiPaiWei[Chang.TaJiaXuanZe][1]] = 0xff;

            fuLuPaiWei++;
            taJiaFuLuShu++;
            shouPaiWei -= 2;
            // 理牌
            LiPai();

            ziMoPai[ziMoPaiWei] = Chang.ShePai;
            yao[ziMoPaiWei] = Chang.YaoDingYi.Chi;
            ziMoPaiWei++;

            fuLuShun = true;
        }

        // 振聴牌処理
        internal void ZhenTingPaiChuLi()
        {
            tongShunPai[tongShunPaiWei++] = Chang.ShePai;

            if (liZhi)
            {
                liZhiHouPai[liZhiHouPaiWei++] = Chang.ShePai;
            }
        }

        // 捨牌処理
        internal void ShePaiChuLi(Chang.YaoDingYi yao)
        {
            shePaiYao[shePaiWei - 1] = yao;
        }

        // 立直処理
        internal void LiZiChuLi()
        {
            DianBangJiSuan(-1000, false);
            yao[ziMoPaiWei - 1] = Chang.YaoDingYi.LiZhi;
        }

        // 流局
        internal bool LiuJu()
        {
            yiMan = false;

            Chang.Init(yi, 0);
            Chang.Init(fanShu, 0);
            yiShu = 0;
            fanShuJi = 0;

            heLeDian = 0;

            // 流し満貫
            LiuManGuan();

            // 点計算
            if (yiShu > 0)
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

            shouPaiWei++;
            for (int i = 0; i < Pai.QiaoPai.Length; i++)
            {
                shouPai[shouPaiWei - 1] = Pai.QiaoPai[i];
                if (HeLePanDing() >= Ting.XingTing)
                {
                    xingTing = true;
                    break;
                }
            }
            shouPai[shouPaiWei - 1] = 0xff;
            shouPaiWei--;
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
            Chang.Init(heLePai, 0xff);
            heLeKeNengShu = 0;
            liZhiKeNengShu = 0;
            if (jiJia)
            {
                Chang.Init(anGangPaiWei, 0xff);
                anGangKeNengShu = 0;
                Chang.Init(jiaGangPaiWei, 0xff);
                jiaGangKeNengShu = 0;
                Chang.Init(shiTiPai, 0xff);
                shiTiPaiShu = 0;
            }
            else
            {
                Chang.Init(daMingGangPaiWei, 0xff);
                daMingGangKeNengShu = 0;
                Chang.Init(bingPaiWei, 0xff);
                bingKeNengShu = 0;
                Chang.Init(chiPaiWei, 0xff);
                chiKeNengShu = 0;
            }
            jiuZhongJiuPai = false;
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
            for (int i = 0; i < Pai.XuanShangPaiWei; i++)
            {
                if ((Pai.XuanShangPaiDingYi[Pai.XuanShangPai[i] & QIAO_PAI]) == p)
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

        // 河牌判定
        protected int HePaiPanDing(int pai)
        {
            int hePai = 0;
            for (int i = 0; i < Chang.MianZi; i++)
            {
                QiaoShi shi = Chang.QiaoShi[i];
                for (int j = 0; j < shi.shePaiWei; j++)
                {
                    if ((shi.ShePai[j] & QIAO_PAI) == (pai & QIAO_PAI))
                    {
                        hePai++;
                    }
                }
            }
            return hePai;
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
                if (yiShu > 0)
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
                if (yiShu > 0)
                {
                    return Ting.TingPai;
                }
                // 役判定
                YiPanDing();
                if (yiShu > 0)
                {
                    fu = 25;
                    // 聴牌
                    return Ting.TingPai;
                }
            }

            int[] maxYi = new int[yi.Length];
            int[] maxFanShu = new int[fanShu.Length];
            int maxYiShu = 0;
            int maxFanShuJi = 0;
            int maxFu = 0;
            // 不聴
            Ting ret = Ting.BuTing;
            for (int i = 0; i < shouPaiWei; i++)
            {
                // 手牌数計算
                ShouPaiShuJiSuan();
                // 頭
                int t = shouPai[i] & QIAO_PAI;
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
                        for (int j = 0; j < Pai.QiaoPai.Length; j++)
                        {
                            int p = Pai.QiaoPai[j];
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
                    for (int j = 0; j < shouPaiShu.Length; j++)
                    {
                        shu += shouPaiShu[j];
                        if (shu > 0)
                        {
                            break;
                        }
                    }
                    if (shu == 0)
                    {
                        // 役満判定
                        YiManPanDing();
                        if (yiShu > 0)
                        {
                            // 聴牌
                            return Ting.TingPai;
                        }
                        // 役判定
                        YiPanDing();
                        FuJiSuan();
                        if (fanShuJi > maxFanShuJi)
                        {
                            maxYiShu = yiShu;
                            maxFanShuJi = fanShuJi;
                            maxFu = fu;
                            Chang.Copy(yi, maxYi);
                            Chang.Copy(fanShu, maxFanShu);
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
                yiShu = maxYiShu;
                fanShuJi = maxFanShuJi;
                fu = maxFu;
                Chang.Copy(maxYi, yi);
                Chang.Copy(maxFanShu, fanShu);
            }
            return ret;
        }

        // 有効牌数計算
        internal void YouXiaoPaiShuJiSuan()
        {
            Chang.Init(youXiaoPaiShu, 0);
            int minXiangTingShu = 99;
            int[] xiangTingShu = new int[ShouPaiWei];
            for (int i = 0; i < ShouPaiWei; i++)
            {
                XiangTingShuJiSuan(i);
                xiangTingShu[i] = XiangTingShu;
                if (minXiangTingShu > XiangTingShu)
                {
                    minXiangTingShu = XiangTingShu;
                }
            }
            GongKaiPaiShuJiSuan();
            for (int i = 0; i < ShouPaiWei; i++)
            {
                if (xiangTingShu[i] != minXiangTingShu)
                {
                    continue;
                }
                int youXiaoPai = 0;
                int[] shouPaiC = new int[ShouPai.Length];
                Chang.Copy(ShouPai, shouPaiC);
                for (int j = 0; j < Pai.QiaoPai.Length; j++)
                {
                    ShouPai[i] = Pai.QiaoPai[j];
                    gongKaiPaiShu[ShouPai[i]]--;
                    gongKaiPaiShu[Pai.QiaoPai[j]]++;
                    for (int k = 0; k < ShouPaiWei; k++)
                    {
                        if (i == k)
                        {
                            continue;
                        }
                        XiangTingShuJiSuan(k);
                        if (minXiangTingShu > XiangTingShu)
                        {
                            youXiaoPai += 4 - GongKaiPaiShu[Pai.QiaoPai[j]];
                            break;
                        }
                    }
                    gongKaiPaiShu[ShouPai[i]]++;
                    gongKaiPaiShu[Pai.QiaoPai[j]]--;
                }
                youXiaoPaiShu[i] = youXiaoPai;
                Chang.Copy(shouPaiC, ShouPai);
            }
        }

        // 向聴数計算
        internal void XiangTingShuJiSuan(int wei)
        {
            xiangTingShu = 99;
            int xiang;

            int[] shouPaiC = new int[shouPai.Length];
            Chang.Copy(shouPai, shouPaiC);
            if (wei >= 0)
            {
                shouPai[wei] = 0xff;
            }
            Sort(shouPai);

            if (fuLuPaiWei == 0)
            {
                // 七対子向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();

                // 対子種数
                int chongShu = 0;
                for (int j = 0; j < Pai.QiaoPai.Length; j++)
                {
                    int p = Pai.QiaoPai[j];
                    if (shouPaiShu[p] >= 2)
                    {
                        chongShu++;
                    }
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                xiangTingShu = 6 - duiZiShu + (duiZiShu - chongShu);

                // 国士無双向聴数計算
                // 面子初期化
                MianZiChuQiHua();
                // 手牌数計算
                ShouPaiShuJiSuan();
                int shu = 0;
                bool chuHui = true;
                for (int i = 0; i < Pai.YaoJiuPaiDingYi.Length; i++)
                {
                    int p = Pai.YaoJiuPaiDingYi[i];
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
                    for (int j = 0; j < Pai.QiaoPai.Length; j++)
                    {
                        int p = Pai.QiaoPai[j];
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

                for (int j = 0; j < Pai.QiaoPai.Length; j++)
                {
                    int p = Pai.QiaoPai[j];
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                for (int j = 0; j < Pai.QiaoPai.Length; j++)
                {
                    int p = Pai.QiaoPai[j];
                    // 塔子計算
                    TaZiJiSuan(p);
                }
                // 計
                int mianZiShu = keZiShu + shunZiShu;
                int xingShu = mianZiShu;
                while (xingShu < 4 && taZiShu > 0)
                {
                    taZiShu--;
                    xingShu++;
                }
                while (xingShu < 4 && duiZiShu > 0)
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

            Chang.Copy(shouPaiC, shouPai);
        }

        // 符計算
        private void FuJiSuan()
        {
            for (int i = 0; i < yiShu; i++)
            {
                if (yi[i] == (int)YiDingYi.QiDuiZi)
                {
                    fu = 25;
                    return;
                }
                if (yi[i] == (int)YiDingYi.PingHe)
                {
                    if (GuiZe.ziMoPingHe && jiJia)
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
            for (int i = 0; i < duiZiShu; i++)
            {
                int p = duiZi[i][0] & QIAO_PAI;
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
            for (int i = 0; i < keZiShu; i++)
            {
                bool yaoJiu = false;
                if (YaoJiuPaiPanDing(keZi[i][0]))
                {
                    yaoJiu = true;
                }
                switch (keZiZhong[i])
                {
                    case Chang.YaoDingYi.Wu:
                        // 暗刻
                        fu += (yaoJiu ? 8 : 4);
                        break;
                    case Chang.YaoDingYi.Bing:
                        // 明刻
                        fu += (yaoJiu ? 4 : 2);
                        break;
                    case Chang.YaoDingYi.AnGang:
                        // 暗槓
                        fu += (yaoJiu ? 32 : 16);
                        break;
                    case Chang.YaoDingYi.JiaGang:
                    case Chang.YaoDingYi.DaMingGang:
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
            int heLePai = shouPai[shouPaiWei - 1] & QIAO_PAI;
            bool daiDian = false;
            for (int i = 0; i < duiZiShu; i++)
            {
                if (duiZi[i][0] == heLePai)
                {
                    fu += 2;
                    daiDian = true;
                    break;
                }
            }
            if (!daiDian)
            {
                for (int i = 0; i < shunZiShu; i++)
                {
                    if (shunZi[i][1] == heLePai || (heLePai & SHU_PAI) == 3 || (heLePai & SHU_PAI) == 7)
                    {
                        fu += 2;
                        break;
                    }
                }
            }
            // 切上
            fu = Chang.Ceil(fu, 10);
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
            for (int i = 0; i < Pai.YaoJiuPaiDingYi.Length; i++)
            {
                int p = Pai.YaoJiuPaiDingYi[i];
                for (int j = 0; j < shouPaiWei; j++)
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
        private void ZhenTingPanDing()
        {
            zhenTing = false;
            for (int i = 0; i < daiPaiShu; i++)
            {
                // 振聴
                for (int j = 0; j < shePaiWei; j++)
                {
                    if (daiPai[i] == (shePai[j] & QIAO_PAI))
                    {
                        zhenTing = true;
                        return;
                    }
                }
                // 同順内振聴
                for (int j = 0; j < tongShunPaiWei; j++)
                {
                    if (daiPai[i] == (tongShunPai[j] & QIAO_PAI))
                    {
                        zhenTing = true;
                        return;
                    }
                }
                // 立直後振聴
                for (int j = 0; j < liZhiHouPaiWei; j++)
                {
                    if (daiPai[i] == (liZhiHouPai[j] & QIAO_PAI))
                    {
                        zhenTing = true;
                        return;
                    }
                }
            }
        }

        // 聴牌判定
        private void TingPaiPanDing()
        {
            int[] shouPaiC = new int[shouPai.Length];
            Chang.Copy(shouPai, shouPaiC);

            for (int i = 0; i < shouPaiWei; i++)
            {
                shouPai[i] = 0xff;
                Sort(shouPai);

                int wei = 0;
                for (int j = 0; j < Pai.QiaoPai.Length; j++)
                {
                    shouPai[shouPaiWei - 1] = Pai.QiaoPai[j];

                    if (HeLePanDing() == Ting.TingPai)
                    {
                        heLePai[heLeKeNengShu][wei++] = Pai.QiaoPai[j];
                    }
                }
                if (wei > 0)
                {
                    heLePaiWei[heLeKeNengShu] = i;
                    heLeKeNengShu++;

                    if (Pai.CanShanPaiShu() >= 4 && taJiaFuLuShu == 0 && (dianBang >= 1000 || GuiZe.jieJinLiZhi))
                    {
                        liZhiPaiWei[liZhiKeNengShu] = i;
                        liZhiKeNengShu++;
                    }
                }

                Chang.Copy(shouPaiC, shouPai);
            }
        }

        // 暗槓判定
        private void AnGangPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            for (int i = 0; i < Pai.QiaoPai.Length; i++)
            {
                if (shouPaiShu[Pai.QiaoPai[i]] >= 4)
                {
                    int wei = 0;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (Pai.QiaoPai[i] == (shouPai[j] & QIAO_PAI))
                        {
                            anGangPaiWei[anGangKeNengShu][wei++] = j;
                        }
                    }
                    anGangKeNengShu++;
                }
            }
            // 立直後槓判定
            LiZhiHouGangPanDing();
        }

        // 立直後槓判定
        private void LiZhiHouGangPanDing()
        {
            if (!liZhi || anGangKeNengShu == 0)
            {
                return;
            }

            // 待牌計算
            DaiPaiJiSuan(shouPaiWei - 1);

            bool heLe = true;
            for (int i = 0; i < anGangKeNengShu; i++)
            {
                int[] shouPaiC = new int[shouPai.Length];
                Chang.Copy(shouPai, shouPaiC);
                int shouPaiWeiC = shouPaiWei;
                int[][] fuLuPaiC = new int[fuLuPai.Length][];
                for (int j = 0; j < fuLuPaiC.Length; j++)
                {
                    fuLuPaiC[j] = new int[fuLuPai[0].Length];
                }
                Chang.Copy(fuLuPai, fuLuPaiC);
                Chang.YaoDingYi[] fuLuZhongC = new Chang.YaoDingYi[fuLuZhong.Length];
                Chang.Copy(fuLuZhong, fuLuZhongC);
                int fuLuPaiWeiC = fuLuPaiWei;

                AnGang(i);
                Sort(shouPai);
                for (int j = 0; j < daiPaiShu; j++)
                {
                    shouPai[shouPaiWei++] = daiPai[j];
                    if (HeLePanDing() != Ting.TingPai)
                    {
                        heLe = false;
                        break;
                    }
                    shouPaiWei--;
                }

                Chang.Copy(shouPaiC, shouPai);
                shouPaiWei = shouPaiWeiC;
                Chang.Copy(fuLuPaiC, fuLuPai);
                Chang.Copy(fuLuZhongC, fuLuZhong);
                fuLuPaiWei = fuLuPaiWeiC;
            }

            if (!heLe)
            {
                Chang.Init(anGangPaiWei, 0xff);
                anGangKeNengShu = 0;
            }
        }

        // 加槓判定
        private void JiaGangPanDing()
        {
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                if (fuLuZhong[i] != Chang.YaoDingYi.Bing)
                {
                    continue;
                }
                for (int j = 0; j < shouPaiWei; j++)
                {
                    if ((fuLuPai[i][0] & QIAO_PAI) == (shouPai[j] & QIAO_PAI))
                    {
                        jiaGangPaiWei[jiaGangKeNengShu][0] = j;
                        jiaGangKeNengShu++;
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
            int wei = 0;
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                if (p == shePai)
                {
                    daMingGangPaiWei[daMingGangKeNengShu][wei++] = i;
                }
            }
            daMingGangKeNengShu++;
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
            if (shouPaiWei < 3)
            {
                return;
            }
            for (int i = 0; i < shouPaiWei - 1; i++)
            {
                int p1 = shouPai[i] & QIAO_PAI;
                if (p1 == shePai)
                {
                    for (int j = i + 1; j < shouPaiWei; j++)
                    {
                        int p2 = shouPai[j] & QIAO_PAI;
                        if (p2 == shePai)
                        {
                            bingPaiWei[bingKeNengShu][0] = i;
                            bingPaiWei[bingKeNengShu][1] = j;
                            bingKeNengShu++;
                        }
                    }
                }
            }
        }

        // 吃判定
        private void ChiPanDing()
        {
            if (Chang.MianZi <= 2)
            {
                return;
            }
            if (ZiPaiPanDing(Chang.ShePai))
            {
                return;
            }
            if (shouPaiWei < 3)
            {
                return;
            }
            int se = Chang.ShePai & SE_PAI;
            for (int i = 0; i < shouPaiWei - 1; i++)
            {
                int p1 = shouPai[i] & QIAO_PAI;
                int s1 = p1 & SHU_PAI;
                if (ZiPaiPanDing(p1) || ((p1 & SE_PAI) != se))
                {
                    continue;
                }
                for (int j = i + 1; j < shouPaiWei; j++)
                {
                    int p2 = shouPai[j] & QIAO_PAI;
                    int s2 = p2 & SHU_PAI;
                    if (ZiPaiPanDing(p2) || ((p2 & SE_PAI) != se))
                    {
                        continue;
                    }

                    int shePaiShu = Chang.ShePai & SHU_PAI;
                    if (shePaiShu <= 7)
                    {
                        if (s1 == (shePaiShu + 1) && (s2 == shePaiShu + 2))
                        {
                            chiPaiWei[chiKeNengShu][0] = i;
                            chiPaiWei[chiKeNengShu][1] = j;
                            chiKeNengShu++;
                        }
                    }
                    if (shePaiShu >= 2 && shePaiShu <= 8)
                    {
                        if (s1 == (shePaiShu - 1) && (s2 == shePaiShu + 1))
                        {
                            chiPaiWei[chiKeNengShu][0] = i;
                            chiPaiWei[chiKeNengShu][1] = j;
                            chiKeNengShu++;
                        }
                    }
                    if (shePaiShu >= 3)
                    {
                        if (s1 == (shePaiShu - 2) && (s2 == shePaiShu - 1))
                        {
                            chiPaiWei[chiKeNengShu][0] = i;
                            chiPaiWei[chiKeNengShu][1] = j;
                            chiKeNengShu++;
                        }
                    }
                }
            }
        }

        // 鳴牌判定
        internal bool MingPaiPanDing(Chang.YaoDingYi fuLuZhong, int fuLuJia, int wei)
        {
            if (fuLuZhong == Chang.YaoDingYi.Chi && wei == 0)
            {
                return true;
            }
            else if (fuLuZhong == Chang.YaoDingYi.Bing)
            {
                if ((fuLuJia == 3 && wei == 2) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            else if (fuLuZhong == Chang.YaoDingYi.DaMingGang)
            {
                if ((fuLuJia == 3 && wei == 3) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            else if (fuLuZhong == Chang.YaoDingYi.JiaGang)
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
            if (GuiZe.shiTi || !fuLuShun)
            {
                return;
            }

            int mingPai = fuLuPai[fuLuPaiWei - 1][0] & QIAO_PAI;
            shiTiPai[shiTiPaiShu++] = mingPai;
            if ((mingPai & ZI_PAI) != ZI_PAI)
            {
                int p1 = fuLuPai[fuLuPaiWei - 1][1] & QIAO_PAI;
                int p2 = fuLuPai[fuLuPaiWei - 1][2] & QIAO_PAI;
                if (Math.Abs(p1 - p2) == 1)
                {
                    int mingShu = mingPai & SHU_PAI;
                    if (mingPai < p1 && mingShu < 7)
                    {
                        shiTiPai[shiTiPaiShu++] = mingPai + 3;
                    }
                    if (mingPai > p2 && mingShu > 3)
                    {
                        shiTiPai[shiTiPaiShu++] = mingPai - 3;
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
            for (int i = 0; i < Pai.YaoJiuPaiDingYi.Length; i++)
            {
                if (shouPaiShu[Pai.YaoJiuPaiDingYi[i]] == 0)
                {
                    return false;
                }
                if (shouPaiShu[Pai.YaoJiuPaiDingYi[i]] == 2)
                {
                    duiZi[duiZiShu][0] = Pai.YaoJiuPaiDingYi[i];
                    duiZi[duiZiShu][1] = Pai.YaoJiuPaiDingYi[i];
                    duiZiShu++;
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

            for (int i = 0; i < Pai.QiaoPai.Length; i++)
            {
                int p = Pai.QiaoPai[i];
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
            for (int i = 0; i < duiZiShu - 2; i++)
            {
                int p1 = duiZi[i][0];
                if (ZiPaiPanDing(p1))
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
            Chang.Init(duiZi, 0xff);
            duiZiShu = 0;
            Chang.Init(keZi, 0xff);
            Chang.Init(keZiZhong, Chang.YaoDingYi.Wu);
            keZiShu = 0;
            Chang.Init(shunZi, 0xff);
            shunZiShu = 0;
            Chang.Init(taZi, 0xff);
            taZiShu = 0;
        }

        /**
         * 役満判定
         */
        private void YiManPanDing()
        {
            yiMan = false;

            Chang.Init(yi, 0);
            Chang.Init(fanShu, 0);
            yiShu = 0;
            fanShuJi = 0;

            // 天和
            TianHe();
            // 人和・地和
            RenHeDeHe();
            // 国士無双・国士無双十三面
            GuoShiWuShuang();
            // 四暗刻・四暗刻単騎・四槓子
            SiAnKeSiGangZi();
            // 四連刻
            SiLianKe();
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
            // 大車輪
            DaCheLun();

            fanShuJi = 0;
            for (int i = 0; i < yiShu; i++)
            {
                fanShuJi += fanShu[i];
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

        // 人和・地和
        private void RenHeDeHe()
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
                else
                {
                    YiZhuiJia(YiManDingYi.RenHe, 1);
                }
            }
        }

        // 国士無双・国士無双十三面
        private void GuoShiWuShuang()
        {
            if (duiZiShu == 1 && keZiShu == 0 && shunZiShu == 0)
            {
                if ((shouPai[shouPaiWei - 1] & QIAO_PAI) == duiZi[0][0])
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
            if (keZiShu < 4)
            {
                return;
            }
            int anKe = 0;
            int gangZi = 0;
            for (int i = 0; i < keZiShu; i++)
            {
                if (keZiZhong[i] == Chang.YaoDingYi.Wu || keZiZhong[i] == Chang.YaoDingYi.AnGang)
                {
                    anKe++;
                }
                if (keZiZhong[i] == Chang.YaoDingYi.AnGang || keZiZhong[i] == Chang.YaoDingYi.JiaGang || keZiZhong[i] == Chang.YaoDingYi.DaMingGang)
                {
                    gangZi++;
                }
            }
            if (anKe == 4)
            {
                if ((shouPai[shouPaiWei - 1] & QIAO_PAI) == duiZi[0][0])
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

        // 四連刻
        private void SiLianKe()
        {
            if (keZiShu < 4)
            {
                return;
            }
            int[] p = new int[4];
            p[0] = keZi[0][0];
            p[1] = keZi[1][0];
            p[2] = keZi[2][0];
            p[3] = keZi[3][0];
            Sort(p);
            for (int i = 0; i < p.Length; i++)
            {
                if (ZiPaiPanDing(p[i]))
                {
                    return;
                }
            }
            if ((p[0] + 1 == p[1]) && (p[0] + 2 == p[2]) && (p[0] + 3 == p[3]))
            {
                YiZhuiJia(YiManDingYi.SiLianKe, 1);
            }
        }

        // 大三元
        private void DaSanYuan()
        {
            if (keZiShu < 3)
            {
                return;
            }
            int yuan = 0;
            for (int i = 0; i < keZiShu; i++)
            {
                int p = keZi[i][0];
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
            if (keZiShu < 3)
            {
                return;
            }
            int sixi = 0;
            for (int i = 0; i < keZiShu; i++)
            {
                if (keZi[i][0] >= 0x31 && keZi[i][0] <= 0x34)
                {
                    sixi++;
                }
            }
            int dui = 0;
            for (int i = 0; i < duiZiShu; i++)
            {
                if (duiZi[i][0] >= 0x31 && duiZi[i][0] <= 0x34)
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
            for (int i = 0; i < shouPaiWei; i++)
            {
                if (!ZiPaiPanDing(shouPai[i]))
                {
                    return;
                }
            }
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int p = fuLuPai[i][j] & QIAO_PAI;
                    if (p == QIAO_PAI)
                    {
                        continue;
                    }
                    if (!ZiPaiPanDing(p))
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
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
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
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int p = fuLuPai[i][j] & QIAO_PAI;
                    int s = p & SHU_PAI;
                    if (fuLuPai[i][j] == QIAO_PAI)
                    {
                        continue;
                    }
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
            Chang.Init(jiuLian, 0);
            int se = -1;
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
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
            int heLePai = shouPai[shouPaiWei - 1];
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
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                bool luYi = false;
                for (int j = 0; j < Pai.LuYiSePaiDingYi.Length; j++)
                {
                    if (p == Pai.LuYiSePaiDingYi[j])
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
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int p = fuLuPai[i][j] & QIAO_PAI;
                    if (p == QIAO_PAI)
                    {
                        continue;
                    }
                    bool luYi = false;
                    for (int k = 0; k < Pai.LuYiSePaiDingYi.Length; k++)
                    {
                        if (p == Pai.LuYiSePaiDingYi[k])
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

        // 大車輪
        private void DaCheLun()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();
            for (int i = 0x12; i <= 0x18; i++)
            {
                if (shouPaiShu[i] != 2)
                {
                    return;
                }
            }
            YiZhuiJia(YiManDingYi.DaCheLun, 1);
        }

        // 十三不塔判定
        private bool ShiSanBuTaPanDing()
        {
            yiMan = false;
            Chang.Init(yi, 0);
            Chang.Init(fanShu, 0);
            yiShu = 0;
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
            for (int i = 0; i < Pai.QiaoPai.Length; i++)
            {
                if (shouPaiShu[Pai.QiaoPai[i]] == 2)
                {
                    DuiZiJiSuan(Pai.QiaoPai[i]);
                    shouPaiShu[Pai.QiaoPai[i]] += 2;
                    break;
                }
            }
            for (int i = 0; i < Pai.QiaoPai.Length; i++)
            {
                int p = Pai.QiaoPai[i];
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
            Chang.Init(yi, 0);
            Chang.Init(fanShu, 0);
            yiShu = 0;
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
            // 三連刻
            SanLianKe();
            // 小三元
            XiaoSanYuan();
            // 混一色・清一色
            HunYiSeQingYiSe();
            // 七対子
            QiDuiZi();
            if (yiShu > 0)
            {
                // 懸賞牌
                XuanShangPai();
            }

            fanShuJi = 0;
            for (int i = 0; i < yiShu; i++)
            {
                fanShuJi += fanShu[i];
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
            if (shunZiShu < 4)
            {
                return;
            }
            if (!GuiZe.ziMoPingHe && jiJia)
            {
                // 自摸平和無し
                return;
            }
            int tou = duiZi[0][0] & QIAO_PAI;
            if (tou > 0x34 || tou == Chang.ChangFeng || tou == feng)
            {
                return;
            }
            int heLePai = shouPai[shouPaiWei - 1] & QIAO_PAI;
            for (int i = 0; i < shunZiShu; i++)
            {
                if ((shunZi[i][0] == heLePai && (heLePai & SHU_PAI) != 7)
                    || (shunZi[i][2] == heLePai && (heLePai & SHU_PAI) != 3))
                {
                    YiZhuiJia(YiDingYi.PingHe, 1);
                    return;
                }
            }
        }

        // 断幺九
        private void DuanYaoJiu()
        {
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                if (ZiPaiPanDing(p) || ((p & SHU_PAI) == 1) || ((p & SHU_PAI) == 9))
                {
                    return;
                }
            }
            if (!GuiZe.shiDuan && fuLuPaiWei > 0)
            {
                // 喰断無し
                return;
            }
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int p = fuLuPai[i][j] & QIAO_PAI;
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
            if (shunZiShu < 2)
            {
                return;
            }
            int beiKou = 0;
            for (int i = 0; i < shunZiShu - 1; i++)
            {
                for (int j = i + 1; j < shunZiShu; j++)
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
                for (int i = 0; i < shunZiShu; i++)
                {
                    int shu = 0;
                    for (int j = 0; j < shunZiShu; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        if (shunZi[i][0] == shunZi[j][0])
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
            for (int i = 0; i < keZiShu; i++)
            {
                int p = keZi[i][0];
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
            if (shunZiShu < 3)
            {
                return;
            }
            for (int i = 0; i < shunZiShu; i++)
            {
                bool[] se = new bool[3];
                for (int j = 0; j < shunZiShu; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if ((shunZi[i][0] & SHU_PAI) == (shunZi[j][0] & SHU_PAI)
                        && (shunZi[i][1] & SHU_PAI) == (shunZi[j][1] & SHU_PAI)
                        && (shunZi[i][2] & SHU_PAI) == (shunZi[j][2] & SHU_PAI))
                    {
                        se[(shunZi[i][0] & SE_PAI) >> 4] = true;
                        se[(shunZi[j][0] & SE_PAI) >> 4] = true;
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

            for (int i = 0; i < shunZiShu; i++)
            {
                int p1 = shunZi[i][0] & SHU_PAI;
                int p2 = shunZi[i][1] & SHU_PAI;
                int p3 = shunZi[i][2] & SHU_PAI;
                if (p1 == 1 && p2 == 2 && p3 == 3)
                {
                    xing[(shunZi[i][0] & SE_PAI) >> 4][0] = true;
                }
                else if (p1 == 4 && p2 == 5 && p3 == 6)
                {
                    xing[(shunZi[i][0] & SE_PAI) >> 4][1] = true;
                }
                else if (p1 == 7 && p2 == 8 && p3 == 9)
                {
                    xing[(shunZi[i][0] & SE_PAI) >> 4][2] = true;
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
            for (int i = 0; i < duiZiShu; i++)
            {
                int p = duiZi[i][0];
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
            for (int i = 0; i < keZiShu; i++)
            {
                int p = keZi[i][0];
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
            for (int i = 0; i < shunZiShu; i++)
            {
                int shu = (shunZi[i][0] & SHU_PAI) + (shunZi[i][1] & SHU_PAI) + (shunZi[i][2] & SHU_PAI);
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
                if (shunZiShu == 0)
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
            if (keZiShu == 4)
            {
                YiZhuiJia(YiDingYi.DuiDuiHe, 2);
            }
        }

        // 三暗刻・三槓子
        private void SanAnKeSanGangZi()
        {
            if (keZiShu < 3)
            {
                return;
            }
            int anKe = 0;
            int gang = 0;
            for (int i = 0; i < keZiShu; i++)
            {
                if (keZiZhong[i] == Chang.YaoDingYi.Wu || keZiZhong[i] == Chang.YaoDingYi.AnGang)
                {
                    anKe++;
                }
                if (keZiZhong[i] == Chang.YaoDingYi.AnGang || keZiZhong[i] == Chang.YaoDingYi.JiaGang || keZiZhong[i] == Chang.YaoDingYi.DaMingGang)
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

        // 三連刻
        private void SanLianKe()
        {
            if (keZiShu < 3)
            {
                return;
            }
            for (int i = 0; i < keZiShu - 2; i++)
            {
                if (ZiPaiPanDing(keZi[i][0]))
                {
                    continue;
                }
                int[] p = new int[3];
                p[0] = keZi[i][0];
                p[1] = keZi[i + 1][0];
                p[2] = keZi[i + 2][0];
                Sort(p);
                if ((p[0] + 1 == p[1]) && (p[0] + 2 == p[2]))
                {
                    YiZhuiJia(YiDingYi.SanLianKe, 2);
                }
            }
        }

        // 小三元
        private void XiaoSanYuan()
        {
            if (keZiShu < 2)
            {
                return;
            }
            int yuan = 0;
            for (int i = 0; i < keZiShu; i++)
            {
                int p = keZi[i][0];
                if (p == 0x35 || p == 0x36 || p == 0x37)
                {
                    yuan++;
                }
            }
            int dui = 0;
            for (int i = 0; i < duiZiShu; i++)
            {
                int p = duiZi[i][0];
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
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
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
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int p = fuLuPai[i][j] & QIAO_PAI;
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
            if (duiZiShu == 7)
            {
                YiZhuiJia(YiDingYi.QiDuiZi, 2);
            }
        }

        // 流し満貫
        private void LiuManGuan()
        {
            for (int i = 0; i < shePaiWei; i++)
            {
                Chang.YaoDingYi yao = shePaiYao[i];
                if (yao != Chang.YaoDingYi.Wu && yao != Chang.YaoDingYi.LiZhi)
                {
                    return;
                }
                int p = shePai[i] & QIAO_PAI;
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
            int[] xuanShangPaiQuDe = Pai.XuanShangPaiQuDe();
            for (int i = 0; i < xuanShangPaiQuDe.Length; i++)
            {
                xuan += shouPaiShu[Pai.XuanShangPaiDingYi[xuanShangPaiQuDe[i] & QIAO_PAI]];
            }
            if (liZhi)
            {
                int[] liXuanShangPaiQuDe = Pai.LiXuanShangPaiQuDe();
                for (int i = 0; i < liXuanShangPaiQuDe.Length; i++)
                {
                    xuan += shouPaiShu[Pai.XuanShangPaiDingYi[liXuanShangPaiQuDe[i] & QIAO_PAI]];
                }
            }
            for (int i = 0; i < shouPaiWei; i++)
            {
                if ((shouPai[i] & CHI_PAI) == CHI_PAI)
                {
                    xuan += 1;
                }
            }
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int p = fuLuPai[i][j];
                    if (p == 0xff)
                    {
                        break;
                    }
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

        // 役追加
        private void YiZhuiJia(YiManDingYi ming, int fan)
        {
            YiZhuiJia((int)ming, fan);
        }
        private void YiZhuiJia(YiDingYi ming, int fan)
        {
            YiZhuiJia((int)ming, fan);
        }
        private void YiZhuiJia(int ming, int fan)
        {
            yi[yiShu] = ming;
            fanShu[yiShu] = fan;
            yiShu++;
        }

        // 対子計算
        protected void DuiZiJiSuan(int p, bool quanXiao = true)
        {
            while (shouPaiShu[p] >= 2)
            {
                shouPaiShu[p] -= 2;

                duiZi[duiZiShu][0] = p;
                duiZi[duiZiShu][1] = p;
                duiZiShu++;
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

                keZi[keZiShu][0] = p;
                keZi[keZiShu][1] = p;
                keZi[keZiShu][2] = p;
                keZiZhong[keZiShu] = Chang.YaoDingYi.Wu;
                keZiShu++;
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

                    shunZi[shunZiShu][0] = p;
                    shunZi[shunZiShu][1] = p + 1;
                    shunZi[shunZiShu][2] = p + 2;
                    shunZiShu++;
                }
            }
            if (s >= 2 && s <= 8)
            {
                while (shouPaiShu[p - 1] >= 1 && shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1)
                {
                    shouPaiShu[p - 1]--;
                    shouPaiShu[p]--;
                    shouPaiShu[p + 1]--;

                    shunZi[shunZiShu][0] = p - 1;
                    shunZi[shunZiShu][1] = p;
                    shunZi[shunZiShu][2] = p + 1;
                    shunZiShu++;
                }
            }
            if (s >= 3)
            {
                while (shouPaiShu[p - 2] >= 1 && shouPaiShu[p - 1] >= 1 && shouPaiShu[p] >= 1)
                {
                    shouPaiShu[p - 2]--;
                    shouPaiShu[p - 1]--;
                    shouPaiShu[p]--;

                    shunZi[shunZiShu][0] = p - 2;
                    shunZi[shunZiShu][1] = p - 1;
                    shunZi[shunZiShu][2] = p;
                    shunZiShu++;
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

            int s = p & SHU_PAI;
            while (shouPaiShu[p] >= 1 && shouPaiShu[p + 1] >= 1)
            {
                shouPaiShu[p]--;
                shouPaiShu[p + 1]--;

                taZi[taZiShu][0] = p;
                taZi[taZiShu][1] = p + 1;
                taZiShu++;
            }
            while (shouPaiShu[p] >= 1 && shouPaiShu[p + 2] >= 1)
            {
                shouPaiShu[p]--;
                shouPaiShu[p + 2]--;

                taZi[taZiShu][0] = p;
                taZi[taZiShu][1] = p + 2;
                taZiShu++;
            }
        }

        // 副露計算
        private void FuLuJiSuan()
        {
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                if (fuLuZhong[i] == Chang.YaoDingYi.Chi)
                {
                    shunZi[shunZiShu][0] = fuLuPai[i][0] & QIAO_PAI;
                    shunZi[shunZiShu][1] = fuLuPai[i][1] & QIAO_PAI;
                    shunZi[shunZiShu][2] = fuLuPai[i][2] & QIAO_PAI;
                    Sort(shunZi[shunZiShu]);
                    shunZiShu++;

                }
                else
                {
                    keZi[keZiShu][0] = fuLuPai[i][0] & QIAO_PAI;
                    keZi[keZiShu][1] = fuLuPai[i][1] & QIAO_PAI;
                    keZi[keZiShu][2] = fuLuPai[i][2] & QIAO_PAI;
                    keZi[keZiShu][3] = fuLuPai[i][3] & QIAO_PAI;
                    keZiZhong[keZiShu] = fuLuZhong[i];
                    keZiShu++;
                }
            }

            int heLePai = shouPai[shouPaiWei - 1] & QIAO_PAI;
            for (int i = 0; i < duiZiShu; i++)
            {
                if (duiZi[i][0] == heLePai)
                {
                    return;
                }
            }
            for (int i = 0; i < shunZiShu; i++)
            {
                for (int j = 0; j < shunZi[i].Length; j++)
                {
                    if (shunZi[i][j] == heLePai)
                    {
                        return;
                    }
                }
            }
            for (int i = 0; i < keZiShu; i++)
            {
                if (keZiZhong[i] == Chang.YaoDingYi.Wu && keZi[i][0] == heLePai && !jiJia)
                {
                    keZiZhong[i] = Chang.YaoDingYi.Bing;
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
            Chang.Init(shouPaiShu, 0);
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                shouPaiShu[p]++;
            }

            if (fuLu)
            {
                // 副露牌加算
                for (int i = 0; i < fuLuPaiWei; i++)
                {
                    for (int j = 0; j < fuLuPai[i].Length; j++)
                    {
                        int p = fuLuPai[i][j] & QIAO_PAI;
                        if (p == 0xff)
                        {
                            break;
                        }
                        shouPaiShu[p]++;
                    }
                }
            }
        }

        // 公開牌数計算
        internal void GongKaiPaiShuJiSuan()
        {
            Chang.Init(gongKaiPaiShu, 0);
            for (int i = 0; i < Chang.MianZi; i++)
            {
                QiaoShi shi = Chang.QiaoShi[i];
                // 捨牌
                for (int j = 0; j < shi.shePaiWei; j++)
                {
                    int p = shi.ShePai[j] & QIAO_PAI;
                    gongKaiPaiShu[p]++;
                }
                // 副露牌
                for (int j = 0; j < shi.fuLuPaiWei; j++)
                {
                    for (int k = 0; k < shi.fuLuPai[j].Length; k++)
                    {
                        int p = shi.fuLuPai[j][k] & QIAO_PAI;
                        if (p == 0xff)
                        {
                            continue;
                        }
                        if (shi.fuLuZhong[j] == Chang.YaoDingYi.JiaGang && j == 3)
                        {
                            continue;
                        }
                        bool isMingPai = shi.MingPaiPanDing(shi.fuLuZhong[j], shi.fuLuJia[j], k);
                        if (!isMingPai)
                        {
                            gongKaiPaiShu[p]++;
                        }
                    }
                }
            }
            // 懸賞牌
            for (int i = 0; i < Pai.XuanShangPaiWei; i++)
            {
                int p = Pai.XuanShangPai[i] & QIAO_PAI;
                gongKaiPaiShu[p]++;
            }
            // 手牌
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                gongKaiPaiShu[p]++;
            }
        }

        // 副露牌数計算
        protected void FuLuPaiShuSuan()
        {
            Chang.Init(fuLuPaiShu, 0);
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    if (fuLuPai[i][j] == 0xff)
                    {
                        break;
                    }
                    int p = fuLuPai[i][j] & QIAO_PAI;
                    fuLuPaiShu[p]++;
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
                    case Chang.YaoDingYi.Wu:
                        // 無
                        if (ziJiaXuanZe != shouPaiWei - 1)
                        {
                            cuHeSheng = "立直後打牌手出し";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.JiaGang:
                        // 加槓
                        if (jiaGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "立直後加槓不可";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.AnGang:
                        // 暗槓
                        if (anGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "立直後暗槓不可";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.ZiMo:
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
                    case Chang.YaoDingYi.Wu:
                        // 無
                        if (ziJiaXuanZe > shouPaiWei - 1)
                        {
                            cuHeSheng = "打牌選択間違い";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.JiaGang:
                        // 加槓
                        if (jiaGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "加槓不可";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.AnGang:
                        // 暗槓
                        if (anGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "暗槓不可";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.LiZhi:
                        // 立直
                        if (taJiaFuLuShu > 0)
                        {
                            cuHeSheng = "立直不可";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.ZiMo:
                        // 自摸
                        if (!heLe)
                        {
                            cuHeSheng = "誤自摸";
                            return true;
                        }
                        break;

                    case Chang.YaoDingYi.JiuZhongJiuPai:
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

            for (int i = 0; i < shiTiPaiShu; i++)
            {
                if (shiTiPai[i] == (shouPai[ziJiaXuanZe] & QIAO_PAI))
                {
                    cuHeSheng = "食い替え不可";
                    DaPai();
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
                case Chang.YaoDingYi.Wu:
                    // 無
                    break;

                case Chang.YaoDingYi.Chi:
                    // 吃
                    if (chiKeNengShu <= taJiaXuanZe)
                    {
                        cuHeSheng = "吃不可";
                        return true;
                    }
                    break;

                case Chang.YaoDingYi.Bing:
                    // 石並
                    if (bingKeNengShu <= taJiaXuanZe)
                    {
                        cuHeSheng = "石並不可";
                        return true;
                    }
                    break;

                case Chang.YaoDingYi.DaMingGang:
                    // 大明槓
                    if (daMingGangKeNengShu <= taJiaXuanZe)
                    {
                        cuHeSheng = "大明槓不可";
                        return true;
                    }
                    break;

                case Chang.YaoDingYi.RongHe:
                    // 栄和
                    if (!heLe)
                    {
                        cuHeSheng = "誤栄和";
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

using System;
using UnityEngine.UI;
using TMPro;

using Gongtong;

namespace Sikao
{
    // 雀士
    internal abstract class QiaoShi
    {
        // 【腰】
        internal enum Yao
        {
            // 無
            WU = 0,
            // 吃
            CHI = 1,
            // 石並
            BING = 2,
            // 大明槓
            DA_MING_GANG = 3,
            // 加槓
            JIA_GANG = 4,
            // 暗槓
            AN_GANG = 5,
            // 立直
            LI_ZHI = 6,
            // 自摸
            ZI_MO = 7,
            // 栄和
            RONG_HE = 8,
            // 九種九牌
            JIU_ZHONG_JIU_PAI = 9,
            // 聴牌
            TING_PAI = 10,
            // 不聴
            BU_TING = 11,
            // 和了
            HE_LE = 12,
            // 四開槓
            SI_KAI_GANG = 13,
            // 四家立直
            SI_JIA_LI_ZHI = 14,
            // 流し満貫
            LIU_MAN_GUAN = 15,
            // 四風子連打
            SI_FENG_ZI_LIAN_DA = 16,
            // 錯和
            CU_HE = 17,
            // 選択
            SELECT = 18,
            // 打牌
            DA_PAI = 19,
            // 取消
            CLEAR = 20
        }

        // 牌
        internal static readonly int QIAO_PAI = 0x3f;
        // 数牌
        internal static readonly int SHU_PAI = 0x0f;
        // 牌色
        internal static readonly int SE_PAI = 0x30;
        // 字牌
        internal static readonly int ZI_PAI = 0x30;

        // 腰名
        private static readonly string[] YAO_MING = new string[] {
            "", "チー", "ポン", "カン", "カン", "カン", "リーチ", "ツモ", "ロン", "九種九牌",
            "テンパイ", "ノーテン", "和了", "四開槓", "四家立直", "流し満貫", "四風子連打", "チョンボ", "", "", ""
        };
        internal static string YaoMing(Yao yao)
        {
            return YAO_MING[(int)yao];
        }
        // ボタン腰名
        private static readonly string[] YAO_MING_BUTTON = new string[] {
            "パス", "チー", "ポン", "カン", "加槓", "暗槓", "立直", "ツモ", "ロン", "九種",
            "", "", "", "", "", "", "", "", "", "", "取消"
        };
        internal string YaoMingButton(Yao yao)
        {
            if ((yao == Yao.JIA_GANG || yao == Yao.AN_GANG) && (anGangKeNengShu == 0 || jiaGangKeNengShu == 0))
            {
                // 加槓、暗槓で片方のみ可能な場合、ボタン名は「カン」
                yao = Yao.DA_MING_GANG;
            }
            return YAO_MING_BUTTON[(int)yao];
        }

        // 役満名
        internal static readonly string[] YI_MAN_MING = new string[] {
            "天和", "人和", "地和", "国士無双", "国士無双十三面", "四暗刻", "四暗刻単騎", "四槓子", "四連刻", "大三元",
            "小四喜", "大四喜", "字一色", "清老頭", "九連宝燈", "純正九連宝燈", "緑一色", "大車輪", "十三不塔"
        };
        // 役名
        internal static readonly string[] YI_MING = new string[] {
            "立直", "Ｗ立直", "一発", "海底撈月", "河底撈魚", "嶺上開花", "槍槓", "面前清自摸和", "平和", "断幺九",
            "一盃口", "二盃口", "一気通貫", "三色同順", "全帯幺", "純全帯", "混老頭", "三暗刻", "三槓子", "三連刻",
            "小三元", "混一色", "清一色", "対々和", "役牌", "七対子", "ドラ", "流し満貫"
        };
        // 得点役
        internal static readonly string[] DE_DIAN_YI = new string[] {
            "", "", "", "", "", "満貫", "跳満", "跳満", "倍満", "倍満", "倍満", "三倍満", "三倍満", "役満"
        };

        // 役満
        private enum YiMan
        {
            // 天和
            TIAN_HE = 0,
            // 人和
            REN_HE = 1,
            // 地和
            DE_HE = 2,
            // 国士無双
            GUO_SHI_WU_SHUANG = 3,
            // 国士無双十三面
            GUO_SHI_SHI_SAN_MIAN = 4,
            // 四暗刻
            SI_AN_KE = 5,
            // 四暗刻単騎
            SI_AN_KE_DAN_QI = 6,
            // 四槓子
            SI_GANG_ZI = 7,
            // 四連刻
            SI_LIAN_KE = 8,
            // 大三元
            DA_SAN_YUAN = 9,
            // 小四喜
            XIAO_SI_XI = 10,
            // 大四喜
            DA_SI_XI = 11,
            // 字一色
            ZI_YI_SE = 12,
            // 清老頭
            QING_LAO_TOU = 13,
            // 九連宝燈
            JIU_LIAN_BAO_DENG = 14,
            // 純正九連宝燈
            CHUN_ZHENG_JIU_LIAN = 15,
            // 緑一色
            LU_YI_SE = 16,
            // 大車輪
            DA_CHE_LUN = 17,
            // 十三不塔
            SHI_SAN_BU_TA = 18,
        }

        // 役
        private enum Yi
        {
            // 立直
            LI_ZI = 0,
            // Ｗ立直
            W_LI_ZI = 1,
            // 一発
            YI_FA = 2,
            // 海底撈月
            HAI_DI_LAO_YUE = 3,
            // 河底撈魚
            HE_DI_LAO_YU = 4,
            // 嶺上開花
            LING_SHANG_KAI_HUA = 5,
            // 槍槓
            QIANG_GANG = 6,
            // 面前清自摸和
            MIAN_QIAN_QING_ZI_MO_HE = 7,
            // 平和
            PING_HE = 8,
            // 断幺九
            DUAN_YAO_JIU = 9,
            // 一盃口
            YI_BEI_KOU = 10,
            // 二盃口
            ER_BEI_KOU = 11,
            // 一気通貫
            YI_QI_TONG_GUAN = 12,
            // 三色同順
            SAN_SE_TONG_SHUN = 13,
            // 全帯幺
            QUAN_DAI_YAO = 14,
            // 純全帯
            CHUN_QUAN_DAI = 15,
            // 混老頭
            HUN_LAO_TOU = 16,
            // 三暗刻
            SAN_AN_KE = 17,
            // 三槓子
            SAN_GANG_ZI = 18,
            // 三連刻
            SAN_LIAN_KE = 19,
            // 小三元
            XIAO_SAN_YUAN = 20,
            // 混一色
            HUN_YI_SE = 21,
            // 清一色
            QING_YI_SE = 22,
            // 対々和
            DUI_DUI_HE = 23,
            // 役牌
            YI_PAI = 24,
            // 七対子
            QI_DUI_ZI = 25,
            // 懸賞
            XUAN_SHANG = 26,
            // 流し満貫
            LIU_MAN_GUAN = 27,
        }

        // 不聴
        protected const int BU_TING = 0;
        // 形聴
        protected const int XING_TING = 1;
        // 聴牌
        protected const int TING_PAI = 2;

        // 名前
        internal string mingQian = "";
        // 自家思考結果
        internal Yao ziJiaYao;
        // 自家選択
        internal int ziJiaXuanZe;
        // 他家思考結果
        internal Yao taJiaYao;
        // 他家選択
        internal int taJiaXuanZe;

        // プレイヤー
        internal bool player = false;
        // プレイヤー順(プレイヤーが必ず0となり、そこから順番にふられる)
        internal int playOrder = -1;
        // 理牌
        protected bool liPai = true;
        // フォロー
        internal bool follow = false;

        // 役満
        internal bool yiMan;
        // 役
        internal int[] yi;
        // 役数
        internal int yiShu;
        // 飜数
        internal int[] fanShu;
        // 飜数計
        internal int fanShuJi;
        // 符
        internal int fu;
        // 和了点
        internal int heLeDian;
        // 点棒
        internal int dianBang;
        // 集計点
        internal int jiJiDian;
        // 風
        internal int feng;
        // 手牌
        internal int[] shouPai;
        internal Button[] goShouPai;
        // 手牌懸賞
        internal bool[] shouPaiXuanShang;
        // 手牌位
        internal int shouPaiWei;
        // 副露牌
        internal int[][] fuLuPai;
        internal Button[][] goFuLuPai;
        // 副露家
        internal int[] fuLuJia;
        // 副露種
        internal Yao[] fuLuZhong;
        // 副露牌位
        internal int fuLuPaiWei;
        // 包則番
        internal int baoZeFan;
        // 他家副露数
        internal int taJiaFuLuShu;
        // 捨牌
        internal int[] shePai;
        internal Button[] goShePai;
        // 捨牌位
        internal int shePaiWei;
        // 捨牌腰
        internal Yao[] shePaiYao;
        // 捨牌自摸切
        internal bool[] shePaiZiMoQie;
        // 立直位
        internal int liZhiWei;
        // 同順牌
        internal int[] tongShunPai;
        // 同順牌位
        internal int tongShunPaiWei;
        // 立直後牌
        internal int[] liZhiHouPai;
        // 立直後牌位
        internal int liZhiHouPaiWei;
        // 待牌
        internal int[] daiPai;
        internal Button[] goDaiPai;
        // 残牌数
        internal TextMeshProUGUI[] goCanPaiShu;
        // 待牌数
        internal int daiPaiShu;
        // 向聴数
        internal int xiangTingShu;
        internal TextMeshProUGUI goXiangTingShu;
        // 対子
        internal int[][] duiZi;
        // 対子数
        internal int duiZiShu;
        // 刻子
        internal int[][] keZi;
        // 刻子数
        internal int keZiShu;
        // 刻子種
        internal Yao[] keZiZhong;
        // 順子
        internal int[][] shunZi;
        // 順子数
        internal int shunZiShu;
        // 塔子
        internal int[][] taZi;
        // 塔子数
        internal int taZiShu;
        // 手牌数
        internal int[] shouPaiShu;
        // 副露牌数
        internal int[] fuLuPaiShu;
        // 公開牌数
        internal int[] gongKaiPaiShu;
        // 立直家捨牌数
        internal int[] liZhiJiaShePaiShu;
        // 和了
        internal bool heLe;
        // 立直
        internal bool liZhi;
        // W立直
        internal bool wLiZhi;
        // 一発
        internal bool yiFa;
        // 一巡目
        internal bool yiXunMu;
        // 副露順
        internal bool fuLuShun;
        // 振聴
        internal bool zhenTing;
        // 形聴
        internal bool xingTing;
        // 自家
        internal bool jiJia;
        // 和了牌
        internal int[][] heLePai;
        // 和了牌位
        internal int[] heLePaiWei;
        // 和了可能数
        internal int heLeKeNengShu;
        // 立直牌位
        internal int[] liZhiPaiWei;
        // 立直可能数
        internal int liZhiKeNengShu;
        // 暗槓牌位
        internal int[][] anGangPaiWei;
        // 暗槓可能数
        internal int anGangKeNengShu;
        // 加槓牌位
        internal int[][] jiaGangPaiWei;
        // 加槓可能数
        internal int jiaGangKeNengShu;
        // 大明槓牌位
        internal int[][] daMingGangPaiWei;
        // 大明槓可能数
        internal int daMingGangKeNengShu;
        // 石並牌位
        internal int[][] bingPaiWei;
        // 石並可能数
        internal int bingKeNengShu;
        // 吃牌位
        internal int[][] chiPaiWei;
        // 吃可能数
        internal int chiKeNengShu;
        // 九種九牌
        internal bool jiuZhongJiuPai;
        // 自摸牌
        internal int[] ziMoPai;
        // 腰
        internal Yao[] yao;
        // 自摸牌位
        internal int ziMoPaiWei;
        // 受取
        internal int shouQu;
        // 受取(供託)
        internal int shouQuGongTuo;
        // 錯和声
        internal string cuHeSheng;
        // 食替牌
        internal int[] shiTiPai;
        // 食替牌数
        internal int shiTiPaiShu;

        // 記録
        internal Maqiao.JiLu jiLu;

        // コンストラクタ
        internal QiaoShi()
        {
            yi = new int[0x10];
            fanShu = new int[yi.Length];
            shouPai = new int[14];
            shouPaiXuanShang = new bool[shouPai.Length];
            goShouPai = new Button[shouPai.Length];
            fuLuPai = new int[4][];
            goFuLuPai = new Button[fuLuPai.Length][];
            for (int i = 0; i < fuLuPai.Length; i++)
            {
                fuLuPai[i] = new int[4];
                goFuLuPai[i] = new Button[fuLuPai[i].Length];
            }
            fuLuJia = new int[fuLuPai.Length];
            fuLuZhong = new Yao[fuLuPai.Length];
            shePai = new int[0x50];
            goShePai = new Button[shePai.Length];
            shePaiYao = new Yao[shePai.Length];
            shePaiZiMoQie = new bool[shePai.Length];
            liZhiHouPai = new int[shePai.Length * 4];
            tongShunPai = new int[0x20];
            daiPai = new int[13];
            goDaiPai = new Button[daiPai.Length];
            goCanPaiShu = new TextMeshProUGUI[daiPai.Length];
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
            keZiZhong = new Yao[4];
            shouPaiShu = new int[0x40];
            fuLuPaiShu = new int[0x40];
            gongKaiPaiShu = new int[0x40];
            liZhiJiaShePaiShu = new int[0x40];
            heLePai = new int[shouPai.Length][];
            for (int i = 0; i < heLePai.Length; i++)
            {
                heLePai[i] = new int[Pai.PAI.Length];
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
            yao = new Yao[0x50];
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

        // 初期化
        internal static void Init(int[] list, int value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = value;
            }
        }

        // 初期化
        internal static void Init(Yao[] list, Yao value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = value;
            }
        }

        // 初期化
        internal static void Init(bool[] list, bool value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = value;
            }
        }

        // 初期化(2次元配列)
        internal static void Init(int[][] list, int value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                {
                    list[i][j] = value;
                }
            }
        }

        // コピー
        internal static void Copy(int[] from, int[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }

        // コピー
        internal static void Copy(Yao[] from, Yao[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }

        // コピー(2次元配列)
        internal static void Copy(int[][] from, int[][] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                for (int j = 0; j < from[i].Length; j++)
                {
                    to[i][j] = from[i][j];
                }
            }
        }

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
            Init(shouPai, 0xff);
            Init(shouPaiXuanShang, false);
            shouPaiWei = 0;
            Init(fuLuPai, 0xff);
            Init(fuLuJia, 0);
            Init(fuLuZhong, Yao.WU);
            baoZeFan = -1;
            fuLuPaiWei = 0;
            taJiaFuLuShu = 0;
            Init(shePai, 0xff);
            shePaiWei = 0;
            Init(shePaiYao, Yao.WU);
            Init(shePaiZiMoQie, false);
            Init(liZhiHouPai, 0xff);
            liZhiHouPaiWei = 0;
            liZhiWei = -1;
            Init(daiPai, 0xff);
            daiPaiShu = 0;
            xiangTingShu = 0;

            liZhi = false;
            wLiZhi = false;
            yiFa = false;
            yiXunMu = true;
            fuLuShun = false;

            Init(ziMoPai, 0xff);
            Init(yao, Yao.WU);
            ziMoPaiWei = 0;
            shouQu = 0;
            shouQuGongTuo = 0;
            cuHeSheng = "";
        }

        // 思考自家判定
        internal void SiKaoZiJiaPanDing()
        {
            jiJia = true;

            ziJiaYao = Yao.WU;
            ziJiaXuanZe = shouPaiWei - 1;

            // 初期化
            SiKaoQianChuQiHua();
            // 手牌懸賞判定
            ShouPaiXuanShangPanDing();

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
            if (HeLePanDing() == TING_PAI)
            {
                heLe = true;
            }
            // 聴牌判定
            TingPaiPanDing();

            if (Pai.HaiDiPanDing())
            {
                return;
            }

            if (Pai.XuanShangPaiShu() <= 4 && Pai.CanShanPaiShu() >= 4)
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

            taJiaYao = Yao.WU;
            taJiaXuanZe = 0;

            // 初期化
            SiKaoQianChuQiHua();

            shouPai[shouPaiWei++] = Chang.shePai;
            // 和了判定
            if (HeLePanDing() == TING_PAI)
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
            Sort(shouPai, liPai);
            // 手牌懸賞判定
            ShouPaiXuanShangPanDing();
        }

        // 自摸
        internal void ZiMo(int p)
        {
            shouPai[shouPaiWei++] = p;
            Init(tongShunPai, 0xff);
            tongShunPaiWei = 0;

            ziMoPai[ziMoPaiWei++] = p;
        }

        // 打牌
        internal void DaPai()
        {
            Chang.shePai = shouPai[Chang.ziJiaXuanZe];
            shouPai[Chang.ziJiaXuanZe] = 0xff;
            shePai[shePaiWei] = Chang.shePai;
            if (ziJiaXuanZe == shouPaiWei - 1)
            {
                shePaiZiMoQie[shePaiWei] = true;
            }
            shouPaiWei--;
            shePaiWei++;

            fuLuShun = false;
        }

        // 待牌計算
        internal void DaiPaiJiSuan(int ziJiaXuanZe)
        {
            Init(daiPai, 0xff);
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

        // 和了
        internal void HeLe()
        {
            // 和了判定
            HeLePanDing();
            // 符計算
            if (!yiMan && fanShuJi < 5)
            {
                FuJiSuan();
            }
            else
            {
                fu = 0;
            }
            // 点計算
            DianJiSuan();

            if (Chang.ziJiaYao == Yao.ZI_MO)
            {
                yao[ziMoPaiWei - 1] = Yao.ZI_MO;
            }
            else if (Chang.taJiaYao == Yao.RONG_HE)
            {
                yao[ziMoPaiWei - 1] = Yao.RONG_HE;
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
            fuLuZhong[fuLuPaiWei] = Yao.AN_GANG;

            shouPai[anGangPaiWei[wei][0]] = 0xff;
            shouPai[anGangPaiWei[wei][1]] = 0xff;
            shouPai[anGangPaiWei[wei][2]] = 0xff;
            shouPai[anGangPaiWei[wei][3]] = 0xff;

            fuLuPaiWei++;
            shouPaiWei -= 4;

            // 理牌
            LiPai();

            yao[ziMoPaiWei - 1] = Yao.AN_GANG;
        }

        // 加槓
        internal void JiaGang(int wei)
        {
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                if (fuLuZhong[i] == Yao.BING)
                {
                    if ((fuLuPai[i][0] & QIAO_PAI) == (shouPai[jiaGangPaiWei[wei][0]] & QIAO_PAI))
                    {
                        fuLuPai[i][3] = shouPai[jiaGangPaiWei[wei][0]];
                        fuLuZhong[i] = Yao.JIA_GANG;
                        Chang.shePai = shouPai[jiaGangPaiWei[wei][0]];
                        shouPai[jiaGangPaiWei[wei][0]] = 0xff;
                        shouPaiWei--;
                        // 理牌
                        LiPai();

                        yao[ziMoPaiWei - 1] = Yao.JIA_GANG;
                        return;
                    }
                }
            }
        }

        // 副露家計算
        private int FuLuJiaJiSuan()
        {
            int mingFan = Chang.mingFan;
            int ziMoFan = Chang.ziMoFan;
            if (mingFan < ziMoFan)
            {
                mingFan += Chang.MIAN_ZI;
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
                if (fuLuZhong[i] == Yao.DA_MING_GANG || fuLuZhong[i] == Yao.BING)
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
                if (fuLuZhong[i] == Yao.AN_GANG || fuLuZhong[i] == Yao.JIA_GANG || fuLuZhong[i] == Yao.DA_MING_GANG)
                {
                    gangShu++;
                }
            }
            if (fengShu == 3 || sanYuanShu == 2 || gangShu == 3)
            {
                int wei = fuLuPaiWei - 1;
                if (fuLuZhong[wei] == Yao.DA_MING_GANG || fuLuZhong[wei] == Yao.BING)
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
                    if (fuLuZhong[wei] == Yao.AN_GANG || fuLuZhong[wei] == Yao.JIA_GANG || fuLuZhong[wei] == Yao.DA_MING_GANG)
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
            fuLuPai[fuLuPaiWei][0] = Chang.shePai;
            fuLuPai[fuLuPaiWei][1] = shouPai[daMingGangPaiWei[Chang.taJiaXuanZe][0]];
            fuLuPai[fuLuPaiWei][2] = shouPai[daMingGangPaiWei[Chang.taJiaXuanZe][1]];
            fuLuPai[fuLuPaiWei][3] = shouPai[daMingGangPaiWei[Chang.taJiaXuanZe][2]];
            fuLuJia[fuLuPaiWei] = FuLuJiaJiSuan();
            fuLuZhong[fuLuPaiWei] = Yao.DA_MING_GANG;

            shouPai[daMingGangPaiWei[Chang.taJiaXuanZe][0]] = 0xff;
            shouPai[daMingGangPaiWei[Chang.taJiaXuanZe][1]] = 0xff;
            shouPai[daMingGangPaiWei[Chang.taJiaXuanZe][2]] = 0xff;

            fuLuPaiWei++;
            taJiaFuLuShu++;
            shouPaiWei -= 3;
            // 理牌
            LiPai();

            ziMoPai[ziMoPaiWei] = Chang.shePai;
            yao[ziMoPaiWei] = Yao.DA_MING_GANG;
            ziMoPaiWei++;

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 石並
        internal void Bing()
        {
            fuLuPai[fuLuPaiWei][0] = Chang.shePai;
            fuLuPai[fuLuPaiWei][1] = shouPai[bingPaiWei[Chang.taJiaXuanZe][0]];
            fuLuPai[fuLuPaiWei][2] = shouPai[bingPaiWei[Chang.taJiaXuanZe][1]];
            fuLuJia[fuLuPaiWei] = FuLuJiaJiSuan();
            fuLuZhong[fuLuPaiWei] = Yao.BING;

            shouPai[bingPaiWei[Chang.taJiaXuanZe][0]] = 0xff;
            shouPai[bingPaiWei[Chang.taJiaXuanZe][1]] = 0xff;

            fuLuPaiWei++;
            taJiaFuLuShu++;
            shouPaiWei -= 2;
            // 理牌
            LiPai();

            ziMoPai[ziMoPaiWei] = Chang.shePai;
            yao[ziMoPaiWei] = Yao.BING;
            ziMoPaiWei++;

            fuLuShun = true;
            // 包則判定
            BaoZePanDing();
        }

        // 吃
        internal void Chi()
        {
            fuLuPai[fuLuPaiWei][0] = Chang.shePai;
            fuLuPai[fuLuPaiWei][1] = shouPai[chiPaiWei[Chang.taJiaXuanZe][0]];
            fuLuPai[fuLuPaiWei][2] = shouPai[chiPaiWei[Chang.taJiaXuanZe][1]];
            fuLuJia[fuLuPaiWei] = FuLuJiaJiSuan();
            fuLuZhong[fuLuPaiWei] = Yao.CHI;

            shouPai[chiPaiWei[Chang.taJiaXuanZe][0]] = 0xff;
            shouPai[chiPaiWei[Chang.taJiaXuanZe][1]] = 0xff;

            fuLuPaiWei++;
            taJiaFuLuShu++;
            shouPaiWei -= 2;
            // 理牌
            LiPai();

            ziMoPai[ziMoPaiWei] = Chang.shePai;
            yao[ziMoPaiWei] = Yao.CHI;
            ziMoPaiWei++;

            fuLuShun = true;
        }

        // 振聴牌処理
        internal void ZhenTingPaiChuLi()
        {
            tongShunPai[tongShunPaiWei++] = Chang.shePai;

            if (liZhi)
            {
                liZhiHouPai[liZhiHouPaiWei++] = Chang.shePai;
            }
        }

        // 捨牌処理
        internal void ShePaiChuLi(Yao yao)
        {
            shePaiYao[shePaiWei - 1] = yao;
        }

        // 立直処理
        internal void LiZiChuLi()
        {
            DianBangJiSuan(-1000, false);
            yao[ziMoPaiWei - 1] = Yao.LI_ZHI;
        }

        // 流局
        internal bool LiuJu()
        {
            yiMan = false;

            Init(yi, 0);
            Init(fanShu, 0);
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
            for (int i = 0; i < Pai.PAI.Length; i++)
            {
                shouPai[shouPaiWei - 1] = Pai.PAI[i];
                if (HeLePanDing() >= XING_TING)
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

        // 思考前初期化
        private void SiKaoQianChuQiHua()
        {
            heLe = false;
            Init(heLePai, 0xff);
            heLeKeNengShu = 0;
            liZhiKeNengShu = 0;
            if (jiJia)
            {
                Init(anGangPaiWei, 0xff);
                anGangKeNengShu = 0;
                Init(jiaGangPaiWei, 0xff);
                jiaGangKeNengShu = 0;
                Init(shiTiPai, 0xff);
                shiTiPaiShu = 0;
            }
            else
            {
                Init(daMingGangPaiWei, 0xff);
                daMingGangKeNengShu = 0;
                Init(bingPaiWei, 0xff);
                bingKeNengShu = 0;
                Init(chiPaiWei, 0xff);
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
        protected int XuanShangPaiPanDing(int pai)
        {
            int p = pai & QIAO_PAI;
            int xuan = 0;
            for (int i = 0; i < Pai.xuanShangPaiWei; i++)
            {
                if ((Pai.XUAN_SHANG_PAI[Pai.xuanShangPai[i] & QIAO_PAI]) == p)
                {
                    xuan++;
                }
                if (pai >= 0x40)
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
            if (p == Chang.changFeng)
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
            for (int i = 0; i < Chang.qiaoShi.Length; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                for (int j = 0; j < shi.shePaiWei; j++)
                {
                    if ((shi.shePai[j] & QIAO_PAI) == (pai & QIAO_PAI))
                    {
                        hePai++;
                    }
                }
            }
            return hePai;
        }

        // 和了判定
        protected int HeLePanDing()
        {
            // 国士無双判定
            if (GuoShiWuShuangPanDing())
            {
                // 役満判定
                YiManPanDing();
                if (yiShu > 0)
                {
                    // 聴牌
                    return TING_PAI;
                }
            }
            // 七対子判定
            if (QiDuiZiPanDing())
            {
                // 役満判定
                YiManPanDing();
                if (yiShu > 0)
                {
                    return TING_PAI;
                }
                // 役判定
                YiPanDing();
                if (yiShu > 0)
                {
                    // 聴牌
                    return TING_PAI;
                }
            }

            int[] maxYi = new int[yi.Length];
            int[] maxFanShu = new int[fanShu.Length];
            int maxYiShu = 0;
            int maxFanShuJi = 0;
            // 不聴
            int ret = BU_TING;
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
                        for (int j = 0; j < Pai.PAI.Length; j++)
                        {
                            int p = Pai.PAI[j];
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
                            return TING_PAI;
                        }
                        // 役判定
                        YiPanDing();
                        if (fanShuJi > maxFanShuJi)
                        {
                            maxYiShu = yiShu;
                            maxFanShuJi = fanShuJi;
                            Copy(yi, maxYi);
                            Copy(fanShu, maxFanShu);
                        }
                        // 形聴
                        ret = XING_TING;
                    }
                }
            }
            if (maxYiShu > 0)
            {
                // 聴牌
                ret = TING_PAI;
                yiShu = maxYiShu;
                fanShuJi = maxFanShuJi;
                Copy(maxYi, yi);
                Copy(maxFanShu, fanShu);
            }
            return ret;
        }

        // 向聴数計算
        internal void XiangTingShuJiSuan(int wei)
        {
            xiangTingShu = 999;
            int xiang;

            int[] shouPaiC = new int[shouPai.Length];
            Copy(shouPai, shouPaiC);
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
                for (int j = 0; j < Pai.PAI.Length; j++)
                {
                    int p = Pai.PAI[j];
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
                for (int i = 0; i < Pai.YAO_JIU_PAI.Length; i++)
                {
                    int p = Pai.YAO_JIU_PAI[i];
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
                    for (int j = 0; j < Pai.PAI.Length; j++)
                    {
                        int p = Pai.PAI[j];
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

                for (int j = 0; j < Pai.PAI.Length; j++)
                {
                    int p = Pai.PAI[j];
                    // 対子計算
                    DuiZiJiSuan(p);
                }
                for (int j = 0; j < Pai.PAI.Length; j++)
                {
                    int p = Pai.PAI[j];
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

            Copy(shouPaiC, shouPai);
        }

        // 符計算
        private void FuJiSuan()
        {
            fu = 0;
            for (int i = 0; i < yiShu; i++)
            {
                if (yi[i] == (int)Yi.QI_DUI_ZI)
                {
                    fu = 25;
                    return;
                }
                if (yi[i] == (int)Yi.PING_HE)
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
                if (p == Chang.changFeng)
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
                    case Yao.WU:
                        // 暗刻
                        fu += (yaoJiu ? 8 : 4);
                        break;
                    case Yao.BING:
                        // 明刻
                        fu += (yaoJiu ? 4 : 2);
                        break;
                    case Yao.AN_GANG:
                        // 暗槓
                        fu += (yaoJiu ? 32 : 16);
                        break;
                    case Yao.JIA_GANG:
                    case Yao.DA_MING_GANG:
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

        // 手牌懸賞判定
        internal void ShouPaiXuanShangPanDing()
        {
            Init(shouPaiXuanShang, false);
            for (int i = 0; i < Pai.xuanShangPaiWei; i++)
            {
                int xp = Pai.xuanShangPai[i] & QIAO_PAI;
                for (int j = 0; j < shouPaiWei; j++)
                {
                    int p = shouPai[j] & QIAO_PAI;
                    if (p == Pai.XUAN_SHANG_PAI[xp] || shouPai[j] > 0x40)
                    {
                        shouPaiXuanShang[j] = true;
                    }
                }
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
            for (int i = 0; i < Pai.YAO_JIU_PAI.Length; i++)
            {
                int p = Pai.YAO_JIU_PAI[i];
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
            Copy(shouPai, shouPaiC);

            for (int i = 0; i < shouPaiWei; i++)
            {
                shouPai[i] = 0xff;
                Sort(shouPai);

                int wei = 0;
                for (int j = 0; j < Pai.PAI.Length; j++)
                {
                    shouPai[shouPaiWei - 1] = Pai.PAI[j];

                    if (HeLePanDing() == TING_PAI)
                    {
                        heLePai[heLeKeNengShu][wei++] = Pai.PAI[j];
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

                Copy(shouPaiC, shouPai);
            }
        }

        // 暗槓判定
        private void AnGangPanDing()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();

            for (int i = 0; i < Pai.PAI.Length; i++)
            {
                if (shouPaiShu[Pai.PAI[i]] >= 4)
                {
                    int wei = 0;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (Pai.PAI[i] == (shouPai[j] & QIAO_PAI))
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
                Copy(shouPai, shouPaiC);
                int shouPaiWeiC = shouPaiWei;
                int[][] fuLuPaiC = new int[fuLuPai.Length][];
                for (int j = 0; j < fuLuPaiC.Length; j++)
                {
                    fuLuPaiC[j] = new int[fuLuPai[0].Length];
                }
                Copy(fuLuPai, fuLuPaiC);
                Yao[] fuLuZhongC = new Yao[fuLuZhong.Length];
                Copy(fuLuZhong, fuLuZhongC);
                int fuLuPaiWeiC = fuLuPaiWei;

                AnGang(i);
                Sort(shouPai);
                for (int j = 0; j < daiPaiShu; j++)
                {
                    shouPai[shouPaiWei++] = daiPai[j];
                    if (HeLePanDing() != TING_PAI)
                    {
                        heLe = false;
                        break;
                    }
                    shouPaiWei--;
                }

                Copy(shouPaiC, shouPai);
                shouPaiWei = shouPaiWeiC;
                Copy(fuLuPaiC, fuLuPai);
                Copy(fuLuZhongC, fuLuZhong);
                fuLuPaiWei = fuLuPaiWeiC;
            }

            if (!heLe)
            {
                Init(anGangPaiWei, 0xff);
                anGangKeNengShu = 0;
            }
        }

        // 加槓判定
        private void JiaGangPanDing()
        {
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                if (fuLuZhong[i] != Yao.BING)
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

            int shePai = Chang.shePai & QIAO_PAI;
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

            int shePai = Chang.shePai & QIAO_PAI;
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
            if (Chang.MIAN_ZI <= 2)
            {
                return;
            }
            if (ZiPaiPanDing(Chang.shePai))
            {
                return;
            }
            if (shouPaiWei < 3)
            {
                return;
            }
            int se = Chang.shePai & SE_PAI;
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

                    int shePaiShu = Chang.shePai & SHU_PAI;
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
        internal bool MingPaiPanDing(Yao fuLuZhong, int fuLuJia, int wei)
        {
            if (fuLuZhong == Yao.CHI && wei == 0)
            {
                return true;
            }
            else if (fuLuZhong == Yao.BING)
            {
                if ((fuLuJia == 3 && wei == 2) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            else if (fuLuZhong == Yao.DA_MING_GANG)
            {
                if ((fuLuJia == 3 && wei == 3) || (fuLuJia == 2 && wei == 1) || (fuLuJia == 1 && wei == 0))
                {
                    return true;
                }
            }
            else if (fuLuZhong == Yao.JIA_GANG)
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
            for (int i = 0; i < Pai.YAO_JIU_PAI.Length; i++)
            {
                if (shouPaiShu[Pai.YAO_JIU_PAI[i]] == 0)
                {
                    return false;
                }
                if (shouPaiShu[Pai.YAO_JIU_PAI[i]] == 2)
                {
                    duiZi[duiZiShu][0] = Pai.YAO_JIU_PAI[i];
                    duiZi[duiZiShu][1] = Pai.YAO_JIU_PAI[i];
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

            for (int i = 0; i < Pai.PAI.Length; i++)
            {
                int p = Pai.PAI[i];
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
            for (int i = 0; i < duiZiShu - 3; i++)
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
            Init(duiZi, 0xff);
            duiZiShu = 0;
            Init(keZi, 0xff);
            Init(keZiZhong, Yao.WU);
            keZiShu = 0;
            Init(shunZi, 0xff);
            shunZiShu = 0;
            Init(taZi, 0xff);
            taZiShu = 0;
        }

        /**
         * 役満判定
         */
        private void YiManPanDing()
        {
            yiMan = false;

            Init(yi, 0);
            Init(fanShu, 0);
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
                YiZhuiJia(YiMan.TIAN_HE, 1);
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
                    YiZhuiJia(YiMan.DE_HE, 1);
                }
                else
                {
                    YiZhuiJia(YiMan.REN_HE, 1);
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
                    YiZhuiJia(YiMan.GUO_SHI_SHI_SAN_MIAN, 2);
                }
                else
                {
                    YiZhuiJia(YiMan.GUO_SHI_WU_SHUANG, 1);
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
                if (keZiZhong[i] == Yao.WU || keZiZhong[i] == Yao.AN_GANG)
                {
                    anKe++;
                }
                if (keZiZhong[i] == Yao.AN_GANG || keZiZhong[i] == Yao.JIA_GANG || keZiZhong[i] == Yao.DA_MING_GANG)
                {
                    gangZi++;
                }
            }
            if (anKe == 4)
            {
                if ((shouPai[shouPaiWei - 1] & QIAO_PAI) == duiZi[0][0])
                {
                    YiZhuiJia(YiMan.SI_AN_KE_DAN_QI, 2);
                }
                else
                {
                    YiZhuiJia(YiMan.SI_AN_KE, 1);
                }
            }
            if (gangZi == 4)
            {
                YiZhuiJia(YiMan.SI_GANG_ZI, 1);
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
                YiZhuiJia(YiMan.SI_LIAN_KE, 1);
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
                YiZhuiJia(YiMan.DA_SAN_YUAN, 1);
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
                YiZhuiJia(YiMan.DA_SI_XI, 2);
            }
            if (sixi == 3 && dui == 1)
            {
                YiZhuiJia(YiMan.XIAO_SI_XI, 1);
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
            YiZhuiJia(YiMan.ZI_YI_SE, 1);
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
            YiZhuiJia(YiMan.QING_LAO_TOU, 1);
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
                YiZhuiJia(YiMan.CHUN_ZHENG_JIU_LIAN, 2);
            }
            else
            {
                YiZhuiJia(YiMan.JIU_LIAN_BAO_DENG, 1);
            }
        }

        // 緑一色
        private void LuYiSe()
        {
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                bool luYi = false;
                for (int j = 0; j < Pai.LU_YI_SE_PAI.Length; j++)
                {
                    if (p == Pai.LU_YI_SE_PAI[j])
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
                    for (int k = 0; k < Pai.LU_YI_SE_PAI.Length; k++)
                    {
                        if (p == Pai.LU_YI_SE_PAI[k])
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
            YiZhuiJia(YiMan.LU_YI_SE, 1);
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
            YiZhuiJia(YiMan.DA_CHE_LUN, 1);
        }

        // 十三不塔判定
        private bool ShiSanBuTaPanDing()
        {
            yiMan = false;
            Init(yi, 0);
            Init(fanShu, 0);
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
            for (int i = 0; i < Pai.PAI.Length; i++)
            {
                if (shouPaiShu[Pai.PAI[i]] == 2)
                {
                    DuiZiJiSuan(Pai.PAI[i]);
                    shouPaiShu[Pai.PAI[i]] += 2;
                    break;
                }
            }
            for (int i = 0; i < Pai.PAI.Length; i++)
            {
                int p = Pai.PAI[i];
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
            YiZhuiJia(YiMan.SHI_SAN_BU_TA, 1);
            fanShuJi = 1;
            return true;
        }

        // 役判定
        private void YiPanDing()
        {
            Init(yi, 0);
            Init(fanShu, 0);
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
                YiZhuiJia(Yi.W_LI_ZI, 2);
            }
            else if (liZhi)
            {
                YiZhuiJia(Yi.LI_ZI, 1);
            }
            if (yiFa)
            {
                YiZhuiJia(Yi.YI_FA, 1);
            }
        }

        // 面前清自摸和
        private void MianQianQingZiMohe()
        {
            if (jiJia && taJiaFuLuShu == 0)
            {
                YiZhuiJia(Yi.MIAN_QIAN_QING_ZI_MO_HE, 1);
            }
        }

        // 嶺上開花
        private void LingShangKaiHua()
        {
            if (Pai.LingShanKaiHuaPanDing())
            {
                YiZhuiJia(Yi.LING_SHANG_KAI_HUA, 1);
            }
        }

        // 槍槓
        private void QiangGang()
        {
            if (Pai.QiangGangPanDing())
            {
                YiZhuiJia(Yi.QIANG_GANG, 1);
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
                    YiZhuiJia(Yi.HAI_DI_LAO_YUE, 1);
                }
                else
                {
                    YiZhuiJia(Yi.HE_DI_LAO_YU, 1);
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
            if (tou > 0x34 || tou == Chang.changFeng || tou == feng)
            {
                return;
            }
            int heLePai = shouPai[shouPaiWei - 1] & QIAO_PAI;
            for (int i = 0; i < shunZiShu; i++)
            {
                if ((shunZi[i][0] == heLePai && (heLePai & SHU_PAI) != 7)
                    || (shunZi[i][2] == heLePai && (heLePai & SHU_PAI) != 3))
                {
                    YiZhuiJia(Yi.PING_HE, 1);
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
            YiZhuiJia(Yi.DUAN_YAO_JIU, 1);
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
                    YiZhuiJia(Yi.YI_BEI_KOU, 1);
                }
                else
                {
                    YiZhuiJia(Yi.ER_BEI_KOU, 2);
                }
            }
            if (beiKou == 1)
            {
                YiZhuiJia(Yi.YI_BEI_KOU, 1);
            }
        }

        // 役牌
        private void YiPai()
        {
            int fan = 0;
            for (int i = 0; i < keZiShu; i++)
            {
                int p = keZi[i][0];
                if (p == Chang.changFeng)
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
                YiZhuiJia(Yi.YI_PAI, fan);
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
                        YiZhuiJia(Yi.SAN_SE_TONG_SHUN, 1);
                    }
                    else
                    {
                        YiZhuiJia(Yi.SAN_SE_TONG_SHUN, 2);
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
                        YiZhuiJia(Yi.YI_QI_TONG_GUAN, 1);
                    }
                    else
                    {
                        YiZhuiJia(Yi.YI_QI_TONG_GUAN, 2);
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
                    YiZhuiJia(Yi.HUN_LAO_TOU, 2);
                }
                else if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(Yi.QUAN_DAI_YAO, 1);
                }
                else
                {
                    YiZhuiJia(Yi.QUAN_DAI_YAO, 2);
                }
            }
            else
            {
                if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(Yi.CHUN_QUAN_DAI, 2);
                }
                else
                {
                    YiZhuiJia(Yi.CHUN_QUAN_DAI, 3);
                }
            }
        }

        // 対々和
        private void DuiDuiHe()
        {
            if (keZiShu == 4)
            {
                YiZhuiJia(Yi.DUI_DUI_HE, 2);
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
                if (keZiZhong[i] == Yao.WU || keZiZhong[i] == Yao.AN_GANG)
                {
                    anKe++;
                }
                if (keZiZhong[i] == Yao.AN_GANG || keZiZhong[i] == Yao.JIA_GANG || keZiZhong[i] == Yao.DA_MING_GANG)
                {
                    gang++;
                }
            }
            if (anKe == 3)
            {
                YiZhuiJia(Yi.SAN_AN_KE, 2);
            }
            if (gang == 3)
            {
                YiZhuiJia(Yi.SAN_GANG_ZI, 2);
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
                    YiZhuiJia(Yi.SAN_LIAN_KE, 2);
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
                YiZhuiJia(Yi.XIAO_SAN_YUAN, 2);
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
                    YiZhuiJia(Yi.HUN_YI_SE, 2);
                }
                else
                {
                    YiZhuiJia(Yi.HUN_YI_SE, 3);
                }
            }
            else
            {
                if (taJiaFuLuShu > 0)
                {
                    YiZhuiJia(Yi.QING_YI_SE, 5);
                }
                else
                {
                    YiZhuiJia(Yi.QING_YI_SE, 6);
                }
            }
        }

        // 七対子
        private void QiDuiZi()
        {
            if (duiZiShu == 7)
            {
                YiZhuiJia(Yi.QI_DUI_ZI, 2);
            }
        }

        // 流し満貫
        private void LiuManGuan()
        {
            for (int i = 0; i < shePaiWei; i++)
            {
                Yao yao = shePaiYao[i];
                if (yao != Yao.WU && yao != Yao.LI_ZHI)
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

            YiZhuiJia(Yi.LIU_MAN_GUAN, 5);
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
                xuan += shouPaiShu[Pai.XUAN_SHANG_PAI[xuanShangPaiQuDe[i] & QIAO_PAI]];
            }
            if (liZhi)
            {
                int[] liXuanShangPaiQuDe = Pai.LiXuanShangPaiQuDe();
                for (int i = 0; i < liXuanShangPaiQuDe.Length; i++)
                {
                    xuan += shouPaiShu[Pai.XUAN_SHANG_PAI[liXuanShangPaiQuDe[i] & QIAO_PAI]];
                }
            }
            for (int i = 0; i < shouPaiWei; i++)
            {
                if ((shouPai[i] & 0x40) == 0x40)
                {
                    xuan += 1;
                }
            }
            for (int i = 0; i < fuLuPaiWei; i++)
            {
                for (int j = 0; j < fuLuPai[i].Length; j++)
                {
                    int pai = fuLuPai[i][j];
                    if (pai == 0xff)
                    {
                        break;
                    }
                    for (int k = 0; k < xuanShangPaiQuDe.Length; k++)
                    {
                        int xuanShangPai = Pai.XUAN_SHANG_PAI[xuanShangPaiQuDe[k] & QIAO_PAI];
                        if ((pai & QIAO_PAI) == xuanShangPai)
                        {
                            xuan += 1;
                        }
                    }
                    if ((pai & 0x40) == 0x40)
                    {
                        xuan += 1;
                    }
                }
            }
            if (xuan > 0)
            {
                YiZhuiJia(Yi.XUAN_SHANG, xuan);
            }
        }

        // 役追加
        private void YiZhuiJia(YiMan ming, int fan)
        {
            YiZhuiJia((int)ming, fan);
        }
        private void YiZhuiJia(Yi ming, int fan)
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
                keZiZhong[keZiShu] = Yao.WU;
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
                if (fuLuZhong[i] == Yao.CHI)
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
                if (keZiZhong[i] == Yao.WU && keZi[i][0] == heLePai && !jiJia)
                {
                    keZiZhong[i] = Yao.BING;
                    return;
                }
            }
        }

        // 手牌数計算
        protected void ShouPaiShuJiSuan()
        {
            ShouPaiShuJiSuan(false);
        }
        protected void ShouPaiShuJiSuan(bool anGang)
        {
            Init(shouPaiShu, 0);
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                shouPaiShu[p]++;
            }

            if (anGang)
            {
                // 暗槓牌加算
                for (int i = 0; i < fuLuPaiWei; i++)
                {
                    if (fuLuZhong[i] == Yao.AN_GANG)
                    {
                        int p = fuLuPai[i][0] & QIAO_PAI;
                        shouPaiShu[p] += 4;
                    }
                }
            }
        }

        // 公開牌数計算
        internal void GongKaiPaiShuJiSuan()
        {
            Init(gongKaiPaiShu, 0);
            Init(liZhiJiaShePaiShu, 0);
            for (int i = 0; i < Chang.qiaoShi.Length; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                // 捨牌
                for (int j = 0; j < shi.shePaiWei; j++)
                {
                    int p = shi.shePai[j] & QIAO_PAI;
                    gongKaiPaiShu[p]++;
                    if (shi.liZhi && i != Chang.ziMoFan)
                    {
                        liZhiJiaShePaiShu[p]++;
                    }
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
                        if (shi.fuLuZhong[j] == Yao.JIA_GANG && j == 3)
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
            for (int i = 0; i < Pai.xuanShangPaiWei; i++)
            {
                int p = Pai.xuanShangPai[i] & QIAO_PAI;
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
            Init(fuLuPaiShu, 0);
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
                    case Yao.WU:
                        // 無
                        if (ziJiaXuanZe != shouPaiWei - 1)
                        {
                            cuHeSheng = "立直後打牌手出し";
                            return true;
                        }
                        break;

                    case Yao.JIA_GANG:
                        // 加槓
                        if (jiaGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "立直後加槓不可";
                            return true;
                        }
                        break;

                    case Yao.AN_GANG:
                        // 暗槓
                        if (anGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "立直後暗槓不可";
                            return true;
                        }
                        break;

                    case Yao.ZI_MO:
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
                    case Yao.WU:
                        // 無
                        if (ziJiaXuanZe > shouPaiWei - 1)
                        {
                            cuHeSheng = "打牌選択間違い";
                            return true;
                        }
                        break;

                    case Yao.JIA_GANG:
                        // 加槓
                        if (jiaGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "加槓不可";
                            return true;
                        }
                        break;

                    case Yao.AN_GANG:
                        // 暗槓
                        if (anGangKeNengShu <= ziJiaXuanZe)
                        {
                            cuHeSheng = "暗槓不可";
                            return true;
                        }
                        break;

                    case Yao.LI_ZHI:
                        // 立直
                        if (taJiaFuLuShu > 0)
                        {
                            cuHeSheng = "立直不可";
                            return true;
                        }
                        break;

                    case Yao.ZI_MO:
                        // 自摸
                        if (!heLe)
                        {
                            cuHeSheng = "誤自摸";
                            return true;
                        }
                        break;

                    case Yao.JIU_ZHONG_JIU_PAI:
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
                case Yao.WU:
                    // 無
                    break;

                case Yao.CHI:
                    // 吃
                    if (chiKeNengShu <= taJiaXuanZe)
                    {
                        cuHeSheng = "吃不可";
                        return true;
                    }
                    break;

                case Yao.BING:
                    // 石並
                    if (bingKeNengShu <= taJiaXuanZe)
                    {
                        cuHeSheng = "石並不可";
                        return true;
                    }
                    break;

                case Yao.DA_MING_GANG:
                    // 大明槓
                    if (daMingGangKeNengShu <= taJiaXuanZe)
                    {
                        cuHeSheng = "大明槓不可";
                        return true;
                    }
                    break;

                case Yao.RONG_HE:
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

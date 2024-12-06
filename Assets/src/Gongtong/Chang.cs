using System;
using Sikao;

namespace Gongtong
{
    // 場
    internal class Chang
    {
        // 【腰】
        internal enum YaoDingYi
        {
            // 無
            Wu = 0,
            // 吃
            Chi = 1,
            // 石並
            Bing = 2,
            // 大明槓
            DaMingGang = 3,
            // 加槓
            JiaGang = 4,
            // 暗槓
            AnGang = 5,
            // 立直
            LiZhi = 6,
            // 自摸
            ZiMo = 7,
            // 栄和
            RongHe = 8,
            // 九種九牌
            JiuZhongJiuPai = 9,
            // 聴牌
            TingPai = 10,
            // 不聴
            BuTing = 11,
            // 和了
            HeLe = 12,
            // 四開槓
            SiKaiGang = 13,
            // 四家立直
            SiJiaLiZhi = 14,
            // 流し満貫
            LiuManGuan = 15,
            // 四風子連打
            SiFengZiLianDa = 16,
            // 錯和
            CuHe = 17,
            // 選択
            Select = 18,
            // 打牌
            DaPai = 19,
            // 取消
            Clear = 20
        }

        // 面子
        private static int mianZi = 4;
        internal static int MianZi
        {
            get { return mianZi; }
            set { mianZi = value; }
        }
        // 雀士
        private static QiaoShi[] qiaoShi = new QiaoShi[mianZi];
        internal static QiaoShi[] QiaoShi
        {
            get { return qiaoShi; }
            set { qiaoShi = value; }
        }
        // 場風
        private static int changFeng;
        internal static int ChangFeng
        {
            get { return changFeng; }
            set { changFeng = value; }
        }
        // 局
        private static int ju;
        internal static int Ju
        {
            get { return ju; }
        }
        // 本場
        private static int benChang;
        internal static int BenChang
        {
            get { return benChang; }
        }
        // 親
        private static int qin;
        internal static int Qin
        {
            get { return qin; }
            set { qin = value; }
        }
        // 起家
        private static int qiaJia;
        internal static int QiaJia
        {
            get { return qiaJia; }
            set { qiaJia = value; }
        }
        // 捨牌
        private static int shePai;
        internal static int ShePai
        {
            get { return shePai; }
            set { shePai = value; }
        }
        // 自摸番
        private static int ziMoFan;
        internal static int ZiMoFan
        {
            get { return ziMoFan; }
            set { ziMoFan = value; }
        }
        // 鳴番
        private static int mingFan;
        internal static int MingFan
        {
            get { return mingFan; }
            set { mingFan = value; }
        }
        // 栄和数
        private static int rongHeShu;
        internal static int RongHeShu
        {
            get { return rongHeShu; }
            set { rongHeShu = value; }
        }
        // 栄和番
        private static int[] rongHeFan;
        internal static int[] RongHeFan
        {
            get { return rongHeFan; }
        }
        // 和了番
        private static int heleFan;
        internal static int HeleFan
        {
            get { return heleFan; }
            set { heleFan = value; }
        }
        // 錯和番
        private static int cuHeFan;
        internal static int CuHeFan
        {
            get { return cuHeFan; }
        }
        // 供託
        private static int gongTuo;
        internal static int GongTuo
        {
            get { return gongTuo; }
        }
        // 自家思考結果
        private static YaoDingYi ziJiaYao;
        internal static YaoDingYi ZiJiaYao
        {
            get { return ziJiaYao; }
            set { ziJiaYao = value; }
        }
        // 自家選択
        private static int ziJiaXuanZe;
        internal static int ZiJiaXuanZe
        {
            get { return ziJiaXuanZe; }
            set { ziJiaXuanZe = value; }
        }
        // 他家思考結果
        private static YaoDingYi taJiaYao;
        internal static YaoDingYi TaJiaYao
        {
            get { return taJiaYao; }
            set { taJiaYao = value; }
        }
        // 他家選択
        private static int taJiaXuanZe;
        internal static int TaJiaXuanZe
        {
            get { return taJiaXuanZe; }
            set { taJiaXuanZe = value; }
        }

        // 風
        private static int feng;
        // 立直数
        private static int liZhiShu;
        // 四風子連打牌
        private static int[] siFengZiLianDaPai;
        // 四風子連打牌位
        private static int siFengZiLianDaPaiWei;
        // 四風子連打
        private static bool siFengZiLianDa;
        // 九種九牌
        private static bool jiuZhongJiuPai;

        // 切り上げ
        internal static int Ceil(int value, double p)
        {
            return (int)(Math.Ceiling(value / p) * p);
        }

        // シャッフル
        internal static void Shuffle(int[] list, int num)
        {
            Random r = new();
            for (int i = 0; i < num; i++)
            {
                int n1 = r.Next(0, list.Length);
                int n2 = r.Next(list.Length);
                int tmp = list[n1];
                list[n1] = list[n2];
                list[n2] = tmp;
            }
        }

        // 初期化
        internal static void Init(int[] list, int value)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = value;
            }
        }

        // 初期化
        internal static void Init(YaoDingYi[] list, YaoDingYi value)
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
                Init(list[i], value);
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
        internal static void Copy(YaoDingYi[] from, YaoDingYi[] to)
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
                Copy(from[i], to[i]);
            }
        }

        // 荘初期化
        internal static void ZhuangChuQiHua()
        {
            rongHeFan = new int[mianZi];
            siFengZiLianDaPai = new int[mianZi];
            benChang = 0;
            ju = 0;
            feng = 0;
            changFeng = Pai.FengPaiDingYi[feng];
            gongTuo = 0;
        }

        // 局初期化
        internal static void JuChuQiHua()
        {
            ziMoFan = qin;
            mingFan = 0;
            heleFan = -1;
            cuHeFan = -1;
            shePai = 0xff;
            liZhiShu = 0;
            siFengZiLianDa = false;
            jiuZhongJiuPai = false;
            Init(siFengZiLianDaPai, 0xff);
            siFengZiLianDaPaiWei = 0;
            ziJiaYao = YaoDingYi.Wu;
            taJiaYao = YaoDingYi.Wu;
            rongHeShu = 0;
            Init(rongHeFan, 0xff);
        }

        // 立直処理
        internal static void LiZhiChuLi()
        {
            liZhiShu++;
            gongTuo += 1000;
        }

        // 四家立直判定
        internal static bool SiJiaLiZhiPanDing()
        {
            if (mianZi != 4)
            {
                return false;
            }
            if (liZhiShu >= mianZi)
            {
                return true;
            }
            return false;
        }

        // 四風子連打牌処理
        internal static void SiFengZiLianDaChuLi(int shePai)
        {
            if (mianZi != 4)
            {
                return;
            }
            if (siFengZiLianDaPaiWei >= 4)
            {
                return;
            }
            siFengZiLianDaPai[siFengZiLianDaPaiWei++] = shePai;
            if (siFengZiLianDaPaiWei == 4)
            {
                for (int i = 0; i < siFengZiLianDaPaiWei - 1; i++)
                {
                    int p = siFengZiLianDaPai[i];
                    if (p < 0x31 || p > 0x34)
                    {
                        return;
                    }
                    if (p != siFengZiLianDaPai[i + 1])
                    {
                        return;
                    }
                }
                siFengZiLianDa = true;
            }
        }

        // 四風子連打判定
        internal static bool SiFengZiLianDaPanDing()
        {
            return siFengZiLianDa;
        }

        // 九種九牌処理
        internal static void jiuZhongJiuPaiChuLi()
        {
            jiuZhongJiuPai = true;
        }

        // 連荘
        internal static void LianZhuang()
        {
            benChang++;
        }

        // 輪荘
        internal static void LunZhuang()
        {
            ju++;
            if (ju >= (4 - (4 - mianZi)))
            {
                // 場変更
                ChangBianGeng();
                ju = 0;
            }
            if (heleFan == -1)
            {
                benChang++;
            }
            else
            {
                benChang = 0;
            }
            qin++;
            qin %= mianZi;
        }

        // 点計算
        internal static void DianJiSuan(QiaoShi[] qiaoShi)
        {
            // 和了点
            int dian;
            // 支払
            int zhiFu;
            // 受取
            int shouQu = 0;
            int cuHe = 1;
            for (int i = ziMoFan + 1; i < ziMoFan + mianZi; i++)
            {
                QiaoShi shi = qiaoShi[i % mianZi];
                if (shi.CuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ziJiaYao == YaoDingYi.ZiMo || cuHe == -1)
            {
                // 自摸
                dian = qiaoShi[ziMoFan].HeLeDian;
                // 記録 和了点
                qiaoShi[ziMoFan].JiLu.heLeDian += qiaoShi[ziMoFan].HeLeDian;

                if (qiaoShi[ziMoFan].BaoZeFan >= 0)
                {
                    // 包則
                    int baoZeFan = (ziMoFan + mianZi - qiaoShi[ziMoFan].BaoZeFan) % mianZi;
                    shouQu = dian + benChang * 100 * cuHe;
                    qiaoShi[ziMoFan].DianBangJiSuan(shouQu);
                    qiaoShi[baoZeFan].DianBangJiSuan(-shouQu);
                    // 記録 親和了数
                    qiaoShi[ziMoFan].JiLu.qinHeLeShu++;
                }
                else if (ziMoFan == qin)
                {
                    // 親
                    zhiFu = Ceil(dian / (mianZi - 1), 100);
                    // 本場
                    zhiFu += benChang * 100 * cuHe;
                    for (int i = ziMoFan + 1; i < ziMoFan + mianZi; i++)
                    {
                        qiaoShi[i % mianZi].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    qiaoShi[ziMoFan].DianBangJiSuan(shouQu);
                    // 記録 親和了数
                    qiaoShi[ziMoFan].JiLu.qinHeLeShu++;
                }
                else
                {
                    // 子
                    for (int i = ziMoFan + 1; i < ziMoFan + mianZi; i++)
                    {
                        if ((i % mianZi) == qin)
                        {
                            zhiFu = Ceil(dian / 2, 100);
                        }
                        else
                        {
                            zhiFu = Ceil(dian / 4, 100);
                        }
                        // 本場
                        zhiFu += benChang * 100 * cuHe;
                        if (mianZi == 3)
                        {
                            // 3打ちの場合、北家分を折半
                            zhiFu += Ceil(Ceil(dian / 4, 100) / 2, 100);
                            zhiFu += benChang * 100 / 2 * cuHe;
                        }
                        else if (mianZi == 2)
                        {
                            // 2打ちの場合、全て支払
                            zhiFu = dian;
                            zhiFu += benChang * 300 * cuHe;
                        }
                        qiaoShi[i % mianZi].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    qiaoShi[ziMoFan].DianBangJiSuan(shouQu);
                }
                // 供託
                qiaoShi[ziMoFan].DianBangJiSuan(gongTuo);
                qiaoShi[ziMoFan].ShouQuGongTuoJiSuan(gongTuo);
                gongTuo = 0;
                // 記録 和了数
                qiaoShi[ziMoFan].JiLu.heLeShu++;
                return;
            }
            else if (taJiaYao == YaoDingYi.RongHe)
            {
                // 栄和
                for (int i = 0; i < rongHeShu; i++)
                {
                    dian = qiaoShi[rongHeFan[i]].HeLeDian;
                    // 本場
                    dian += benChang * 300;

                    if (qiaoShi[rongHeFan[i]].BaoZeFan >= 0)
                    {
                        // 包則
                        int baoZeFan = (rongHeFan[i] + mianZi - qiaoShi[rongHeFan[i]].BaoZeFan) % mianZi;
                        shouQu = Ceil(dian / 2, 100);
                        qiaoShi[ziMoFan].DianBangJiSuan(-(dian - shouQu));
                        qiaoShi[baoZeFan].DianBangJiSuan(-shouQu);
                    }
                    else
                    {
                        qiaoShi[ziMoFan].DianBangJiSuan(-dian);
                    }
                    qiaoShi[rongHeFan[i]].DianBangJiSuan(dian);
                    // 供託
                    if (i == 0)
                    {
                        qiaoShi[rongHeFan[i]].DianBangJiSuan(gongTuo);
                        qiaoShi[rongHeFan[i]].ShouQuGongTuoJiSuan(gongTuo);

                    }
                    // 記録 和了数
                    qiaoShi[rongHeFan[i]].JiLu.heLeShu++;
                    // 記録 放銃数
                    qiaoShi[ziMoFan].JiLu.fangChongShu++;
                    // 記録 和了点
                    qiaoShi[rongHeFan[i]].JiLu.heLeDian += qiaoShi[rongHeFan[i]].HeLeDian;
                    // 記録 放銃点
                    qiaoShi[ziMoFan].JiLu.fangChongDian += qiaoShi[rongHeFan[i]].HeLeDian;
                }
                gongTuo = 0;
                return;
            }

            // 流局
            if (liZhiShu >= 4 || siFengZiLianDa || jiuZhongJiuPai)
            {
                return;
            }
            if (ziJiaYao == YaoDingYi.AnGang || ziJiaYao == YaoDingYi.JiaGang || taJiaYao == YaoDingYi.DaMingGang)
            {
                return;
            }
            // 記録 流局数(四家立直や四開槓で流局の場合はカウントしない)
            for (int i = ziMoFan + 1; i < ziMoFan + mianZi; i++)
            {
                QiaoShi shi = qiaoShi[i % mianZi];
                shi.JiLu.liuJuShu++;
            }

            // 形式聴牌計算
            int xingTingShu = 0;
            for (int i = 0; i < mianZi; i++)
            {
                if (qiaoShi[i].XingTing)
                {
                    xingTingShu++;
                    // 記録 聴牌数
                    qiaoShi[i].JiLu.tingPaiShu++;
                }
                else
                {
                    // 記録 不聴数
                    qiaoShi[i].JiLu.buTingShu++;
                }
            }
            if (xingTingShu == mianZi || xingTingShu == 0)
            {
                return;
            }
            for (int i = 0; i < mianZi; i++)
            {
                if (qiaoShi[i].XingTing)
                {
                    qiaoShi[i].DianBangJiSuan(3000 / xingTingShu);
                }
                else
                {
                    qiaoShi[i].DianBangJiSuan(-3000 / (mianZi - xingTingShu));
                }
            }
        }

        // 錯和
        internal static void CuHe(QiaoShi[] qiaoShi, int jia)
        {
            if (jia == qin)
            {
                // 親
                qiaoShi[jia].DianBangJiSuan(-12000);
                for (int i = jia + 1; i < jia + mianZi; i++)
                {
                    int taJia = i % mianZi;
                    qiaoShi[taJia].DianBangJiSuan(12000 / mianZi);
                }
            }
            else
            {
                // 子
                qiaoShi[jia].DianBangJiSuan(-8000);
                for (int i = jia + 1; i < jia + mianZi; i++)
                {
                    int taJia = i % mianZi;
                    int dian;
                    if (taJia == qin)
                    {
                        dian = 4000;
                    }
                    else
                    {
                        dian = 2000;
                    }
                    if (mianZi == 3)
                    {
                        dian += 1000;
                    }
                    qiaoShi[taJia].DianBangJiSuan(dian);
                }
            }
            cuHeFan = jia;
        }

        // 箱判定
        internal static bool XiangPanDing(QiaoShi[] qiaoShi)
        {
            for (int i = 0; i < mianZi; i++)
            {
                if (qiaoShi[i].DianBang <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        // 場変更
        private static void ChangBianGeng()
        {
            feng++;
            if (feng >= Pai.FengPaiDingYi.Length)
            {
                feng = 0;
            }
            changFeng = Pai.FengPaiDingYi[feng];
        }
    }
}

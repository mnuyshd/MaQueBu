using System;
using System.Collections.Generic;
using Assets.Source.Sikao;

namespace Assets.Source.Gongtong
{
    // 場
    internal class Chang
    {
        // ルール
        internal static GuiZe guiZe;
        // 雀士
        private static List<QiaoShi> qiaoShis = new();
        internal static List<QiaoShi> QiaoShis
        {
            get { return qiaoShis; }
            set { qiaoShis = value; }
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
        // 栄和番
        private static List<(int fan, int index)> rongHeFan;
        internal static List<(int fan, int index)> RongHeFan
        {
            get { return rongHeFan; }
            set { rongHeFan = value; }
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
        private static QiaoShi.YaoDingYi ziJiaYao;
        internal static QiaoShi.YaoDingYi ZiJiaYao
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
        private static QiaoShi.YaoDingYi taJiaYao;
        internal static QiaoShi.YaoDingYi TaJiaYao
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
        private static List<int> siFengZiLianDaPai;
        // 四風子連打
        private static bool siFengZiLianDa;
        internal static bool SiFengZiLianDa
        {
            get { return siFengZiLianDa; }
        }
        // 九種九牌
        private static bool jiuZhongJiuPai;

        // 切り上げ
        internal static int Ceil(int value, double p)
        {
            return (int)(Math.Ceiling(value / p) * p);
        }

        // シャッフル
        internal static void Shuffle(List<int> list, int num)
        {
            Random r = new();
            for (int i = 0; i < num; i++)
            {
                int n1 = r.Next(0, list.Count);
                int n2 = r.Next(list.Count);
                int tmp = list[n1];
                list[n1] = list[n2];
                list[n2] = tmp;
            }
        }
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

        // 荘初期化
        internal static void ZhuangChuQiHua()
        {
            benChang = 0;
            ju = 0;
            feng = 0;
            changFeng = Pai.FengPaiDingYi[feng];
            gongTuo = 0;
            foreach (QiaoShi shi in QiaoShis)
            {
                shi.LianZhuangShu = 0;
            }
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
            siFengZiLianDaPai = new List<int>();
            ziJiaYao = QiaoShi.YaoDingYi.Wu;
            taJiaYao = QiaoShi.YaoDingYi.Wu;
            rongHeFan = new List<(int, int)>();
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
            if (guiZe.siJiaLiZhiLianZhuang == 0)
            {
                return false;
            }
            if (qiaoShis.Count != 4)
            {
                return false;
            }
            if (liZhiShu >= qiaoShis.Count)
            {
                return true;
            }
            return false;
        }

        // 四風子連打牌処理
        internal static void SiFengZiLianDaChuLi(int shePai)
        {
            if (qiaoShis.Count != 4)
            {
                return;
            }
            if (siFengZiLianDaPai.Count >= 4)
            {
                return;
            }
            siFengZiLianDaPai.Add(shePai);
            if (siFengZiLianDaPai.Count == 4)
            {
                for (int i = 0; i < siFengZiLianDaPai.Count - 1; i++)
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

        // 九種九牌処理
        internal static void JiuZhongJiuPaiChuLi()
        {
            jiuZhongJiuPai = true;
        }

        // 連荘
        internal static void LianZhuang()
        {
            LianZhuangShuJiSuan();
            benChang++;
        }

        // 輪荘
        internal static void LunZhuang()
        {
            LianZhuangShuJiSuan();

            ju++;
            if (ju >= (4 - (4 - qiaoShis.Count)))
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
            qin %= qiaoShis.Count;
        }

        // 連荘数計算
        private static void LianZhuangShuJiSuan()
        {
            for (int i = 0; i < QiaoShis.Count; i++)
            {
                QiaoShi shi = QiaoShis[i];
                bool isHeLe = false;
                foreach ((int fan, int _) in RongHeFan)
                {
                    if (i == fan)
                    {
                        isHeLe = true;
                    }
                }
                if (isHeLe || HeleFan == i)
                {
                    shi.LianZhuangShu++;
                    if (shi.LianZhuangShu > 7)
                    {
                        shi.LianZhuangShu = 0;
                    }
                }
                else
                {
                    shi.LianZhuangShu = 0;
                }
            }
        }

        // 点計算
        internal static void DianJiSuan()
        {
            // 和了点
            int dian;
            // 支払
            int zhiFu;
            // 受取
            int shouQu = 0;
            int cuHe = 1;
            for (int i = ziMoFan + 1; i < ziMoFan + QiaoShis.Count; i++)
            {
                QiaoShi shi = QiaoShis[i % QiaoShis.Count];
                if (shi.CuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ziJiaYao == QiaoShi.YaoDingYi.ZiMo || cuHe == -1)
            {
                // 自摸
                dian = qiaoShis[ziMoFan].HeLeDian;
                // 記録 和了点
                qiaoShis[ziMoFan].JiLu.heLeDian += qiaoShis[ziMoFan].HeLeDian;

                if (qiaoShis[ziMoFan].BaoZeFan >= 0)
                {
                    // 包則
                    int baoZeFan = (ziMoFan + qiaoShis.Count - qiaoShis[ziMoFan].BaoZeFan) % qiaoShis.Count;
                    shouQu = dian + benChang * 100 * cuHe;
                    qiaoShis[ziMoFan].DianBangJiSuan(shouQu);
                    qiaoShis[baoZeFan].DianBangJiSuan(-shouQu);
                    // 記録 親和了数
                    qiaoShis[ziMoFan].JiLu.qinHeLeShu++;
                }
                else if (ziMoFan == qin)
                {
                    // 親
                    zhiFu = Ceil(dian / (qiaoShis.Count - 1), 100);
                    // 本場
                    zhiFu += benChang * 100 * cuHe;
                    for (int i = ziMoFan + 1; i < ziMoFan + qiaoShis.Count; i++)
                    {
                        qiaoShis[i % qiaoShis.Count].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    qiaoShis[ziMoFan].DianBangJiSuan(shouQu);
                    // 記録 親和了数
                    qiaoShis[ziMoFan].JiLu.qinHeLeShu++;
                }
                else
                {
                    // 子
                    for (int i = ziMoFan + 1; i < ziMoFan + qiaoShis.Count; i++)
                    {
                        if ((i % qiaoShis.Count) == qin)
                        {
                            zhiFu = Ceil(dian / 2, 100);
                        }
                        else
                        {
                            zhiFu = Ceil(dian / 4, 100);
                        }
                        // 本場
                        zhiFu += benChang * 100 * cuHe;
                        if (qiaoShis.Count == 3)
                        {
                            // 3打ちの場合、北家分を折半
                            zhiFu += Ceil(Ceil(dian / 4, 100) / 2, 100);
                            zhiFu += benChang * 100 / 2 * cuHe;
                        }
                        else if (qiaoShis.Count == 2)
                        {
                            // 2打ちの場合、全て支払
                            zhiFu = dian;
                            zhiFu += benChang * 300 * cuHe;
                        }
                        qiaoShis[i % qiaoShis.Count].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    qiaoShis[ziMoFan].DianBangJiSuan(shouQu);
                }
                // 供託
                qiaoShis[ziMoFan].DianBangJiSuan(gongTuo);
                qiaoShis[ziMoFan].ShouQuGongTuoJiSuan(gongTuo);
                gongTuo = 0;
                // 記録 和了数
                qiaoShis[ziMoFan].JiLu.heLeShu++;
                return;
            }
            else if (taJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                // 栄和
                for (int i = 0; i < rongHeFan.Count; i++)
                {
                    dian = qiaoShis[rongHeFan[i].fan].HeLeDian;
                    // 本場
                    dian += benChang * 300;

                    if (qiaoShis[rongHeFan[i].fan].BaoZeFan >= 0)
                    {
                        // 包則
                        int baoZeFan = (rongHeFan[i].fan + qiaoShis.Count - qiaoShis[rongHeFan[i].fan].BaoZeFan) % qiaoShis.Count;
                        shouQu = Ceil(dian / 2, 100);
                        qiaoShis[ziMoFan].DianBangJiSuan(-(dian - shouQu));
                        qiaoShis[baoZeFan].DianBangJiSuan(-shouQu);
                    }
                    else
                    {
                        qiaoShis[ziMoFan].DianBangJiSuan(-dian);
                    }
                    qiaoShis[rongHeFan[i].fan].DianBangJiSuan(dian);
                    // 供託
                    if (i == 0)
                    {
                        qiaoShis[rongHeFan[i].fan].DianBangJiSuan(gongTuo);
                        qiaoShis[rongHeFan[i].fan].ShouQuGongTuoJiSuan(gongTuo);

                    }
                    // 記録 和了数
                    qiaoShis[rongHeFan[i].fan].JiLu.heLeShu++;
                    // 記録 放銃数
                    qiaoShis[ziMoFan].JiLu.fangChongShu++;
                    // 記録 和了点
                    qiaoShis[rongHeFan[i].fan].JiLu.heLeDian += qiaoShis[rongHeFan[i].fan].HeLeDian;
                    // 記録 放銃点
                    qiaoShis[ziMoFan].JiLu.fangChongDian += qiaoShis[rongHeFan[i].fan].HeLeDian;
                }
                gongTuo = 0;
                return;
            }

            // 流局
            if (liZhiShu >= 4 || siFengZiLianDa || jiuZhongJiuPai)
            {
                return;
            }
            if (ziJiaYao == QiaoShi.YaoDingYi.AnGang || ziJiaYao == QiaoShi.YaoDingYi.JiaGang || taJiaYao == QiaoShi.YaoDingYi.DaMingGang)
            {
                return;
            }
            // 記録 流局数(四家立直や四開槓で流局の場合はカウントしない)
            for (int i = ziMoFan + 1; i < ziMoFan + qiaoShis.Count; i++)
            {
                QiaoShi shi = QiaoShis[i % QiaoShis.Count];
                shi.JiLu.liuJuShu++;
            }

            // 形式聴牌計算
            int xingTingShu = 0;
            foreach (QiaoShi shi in qiaoShis)
            {
                if (shi.XingTing)
                {
                    xingTingShu++;
                    // 記録 聴牌数
                    shi.JiLu.tingPaiShu++;
                }
                else
                {
                    // 記録 不聴数
                    shi.JiLu.buTingShu++;
                }
            }
            if (xingTingShu == qiaoShis.Count || xingTingShu == 0)
            {
                return;
            }
            foreach (QiaoShi shi in qiaoShis)
            {
                if (shi.XingTing)
                {
                    shi.DianBangJiSuan(3000 / xingTingShu);
                }
                else
                {
                    shi.DianBangJiSuan(-3000 / (qiaoShis.Count - xingTingShu));
                }
            }
        }

        // 錯和
        internal static void CuHe(int jia)
        {
            if (jia == qin)
            {
                // 親
                qiaoShis[jia].DianBangJiSuan(-12000);
                for (int i = jia + 1; i < jia + qiaoShis.Count; i++)
                {
                    int taJia = i % qiaoShis.Count;
                    qiaoShis[taJia].DianBangJiSuan(12000 / qiaoShis.Count);
                }
            }
            else
            {
                // 子
                qiaoShis[jia].DianBangJiSuan(-8000);
                for (int i = jia + 1; i < jia + qiaoShis.Count; i++)
                {
                    int taJia = i % qiaoShis.Count;
                    int dian;
                    if (taJia == qin)
                    {
                        dian = 4000;
                    }
                    else
                    {
                        dian = 2000;
                    }
                    if (qiaoShis.Count == 3)
                    {
                        dian += 1000;
                    }
                    qiaoShis[taJia].DianBangJiSuan(dian);
                }
            }
            cuHeFan = jia;
        }

        // 箱判定
        internal static bool XiangPanDing()
        {
            foreach (QiaoShi shi in qiaoShis)
            {
                if (shi.DianBang <= 0)
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

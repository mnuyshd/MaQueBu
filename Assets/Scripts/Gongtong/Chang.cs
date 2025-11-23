using System;
using System.Collections.Generic;
using Assets.Scripts.Sikao;

namespace Assets.Scripts.Gongtong
{
    // 場
    internal class Chang
    {
        // ルール
        internal static GuiZe guiZe;
        // 雀士
        internal static List<QiaoShi> QiaoShis { get; set; } = new();
        // 場風
        internal static int ChangFeng { get; set; }
        // 局
        internal static int Ju { get; set; }
        // 本場
        internal static int BenChang { get; set; }
        // 親
        internal static int Qin { get; set; }
        // 起家
        internal static int QiaJia { get; set; }
        // 捨牌
        internal static int ShePai { get; set; }
        // 自摸番
        internal static int ZiMoFan { get; set; }
        // 鳴番
        internal static int MingFan { get; set; }
        // 栄和番
        internal static List<(int fan, int index)> RongHeFan { get; set; } = new();
        // 和了番
        internal static int HeleFan { get; set; }
        // 錯和番
        internal static int CuHeFan { get; set; }
        // 供託
        internal static int GongTuo { get; set; }
        // 自家思考結果
        internal static QiaoShi.YaoDingYi ZiJiaYao { get; set; }
        // 自家選択
        internal static int ZiJiaXuanZe { get; set; }
        // 他家思考結果
        internal static QiaoShi.YaoDingYi TaJiaYao { get; set; }
        // 他家選択
        internal static int TaJiaXuanZe { get; set; }

        // 風
        private static int feng;
        // 立直数
        private static int liZhiShu;
        // 四風子連打牌
        private static List<int> siFengZiLianDaPai;
        // 四風子連打
        internal static bool SiFengZiLianDa { get; set; }
        // 九種九牌
        private static bool jiuZhongJiuPai;

        // ランダム
        private static readonly Random r = new();

        // 状態
        internal static void ZhuangTai(State state, bool isZiJia)
        {
            state.changFeng = ChangFeng;
            state.ju = Ju;
            state.changShePai = isZiJia ? 0 : ShePai;
        }

        // 切り上げ
        internal static int Ceil(int value, double p)
        {
            return (int)(Math.Ceiling(value / p) * p);
        }

        // シャッフル
        internal static void Shuffle(List<int> list, int num)
        {
            for (int i = 0; i < num; i++)
            {
                int n1 = r.Next(list.Count);
                int n2 = r.Next(list.Count);
                (list[n2], list[n1]) = (list[n1], list[n2]);
            }
        }

        // 荘初期化
        internal static void ZhuangChuQiHua()
        {
            BenChang = 0;
            Ju = 0;
            feng = 0;
            ChangFeng = Pai.FengPaiDingYi[feng];
            GongTuo = 0;
            foreach (QiaoShi shi in QiaoShis)
            {
                shi.LianZhuangShu = 0;
            }
        }

        // 局初期化
        internal static void JuChuQiHua()
        {
            ZiMoFan = Qin;
            MingFan = 0;
            HeleFan = -1;
            CuHeFan = -1;
            ShePai = 0xff;
            liZhiShu = 0;
            SiFengZiLianDa = false;
            jiuZhongJiuPai = false;
            siFengZiLianDaPai = new List<int>();
            ZiJiaYao = QiaoShi.YaoDingYi.Wu;
            TaJiaYao = QiaoShi.YaoDingYi.Wu;
            RongHeFan = new List<(int, int)>();
        }

        // 立直処理
        internal static void LiZhiChuLi()
        {
            liZhiShu++;
            GongTuo += 1000;
        }

        // 四家立直判定
        internal static bool SiJiaLiZhiPanDing()
        {
            if (guiZe.siJiaLiZhiLianZhuang == 0)
            {
                return false;
            }
            if (QiaoShis.Count != 4)
            {
                return false;
            }
            if (liZhiShu >= QiaoShis.Count)
            {
                return true;
            }
            return false;
        }

        // 四風子連打牌処理
        internal static void SiFengZiLianDaChuLi(int shePai)
        {
            if (QiaoShis.Count != 4)
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
                SiFengZiLianDa = true;
            }
        }

        // 四風子連打判定
        internal static bool SiFengZiLianDaPanDing()
        {
            return SiFengZiLianDa && guiZe.siFengZiLianDaLianZhuang > 0;
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
            BenChang++;
        }

        // 輪荘
        internal static void LunZhuang()
        {
            LianZhuangShuJiSuan();

            Ju++;
            if (Ju >= (4 - (4 - QiaoShis.Count)))
            {
                // 場変更
                ChangBianGeng();
                Ju = 0;
            }
            if (HeleFan == -1)
            {
                BenChang++;
            }
            else
            {
                BenChang = 0;
            }
            Qin++;
            Qin %= QiaoShis.Count;
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
            for (int i = ZiMoFan + 1; i < ZiMoFan + QiaoShis.Count; i++)
            {
                QiaoShi shi = QiaoShis[i % QiaoShis.Count];
                if (shi.CuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ZiJiaYao == QiaoShi.YaoDingYi.ZiMo || cuHe == -1)
            {
                // 自摸
                dian = QiaoShis[ZiMoFan].HeLeDian;
                // 記録 和了点
                QiaoShis[ZiMoFan].JiLu.heLeDian += QiaoShis[ZiMoFan].HeLeDian;

                if (QiaoShis[ZiMoFan].BaoZeFan >= 0)
                {
                    // 包則
                    int baoZeFan = (ZiMoFan + QiaoShis.Count - QiaoShis[ZiMoFan].BaoZeFan) % QiaoShis.Count;
                    shouQu = dian + BenChang * 100 * cuHe;
                    QiaoShis[ZiMoFan].DianBangJiSuan(shouQu);
                    QiaoShis[baoZeFan].DianBangJiSuan(-shouQu);
                    // 記録 親和了数
                    QiaoShis[ZiMoFan].JiLu.qinHeLeShu++;
                }
                else if (ZiMoFan == Qin)
                {
                    // 親
                    zhiFu = Ceil(dian / (QiaoShis.Count - 1), 100);
                    // 本場
                    zhiFu += BenChang * 100 * cuHe;
                    for (int i = ZiMoFan + 1; i < ZiMoFan + QiaoShis.Count; i++)
                    {
                        QiaoShis[i % QiaoShis.Count].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    QiaoShis[ZiMoFan].DianBangJiSuan(shouQu);
                    // 記録 親和了数
                    QiaoShis[ZiMoFan].JiLu.qinHeLeShu++;
                }
                else
                {
                    // 子
                    for (int i = ZiMoFan + 1; i < ZiMoFan + QiaoShis.Count; i++)
                    {
                        if ((i % QiaoShis.Count) == Qin)
                        {
                            zhiFu = Ceil(dian / 2, 100);
                        }
                        else
                        {
                            zhiFu = Ceil(dian / 4, 100);
                        }
                        // 本場
                        zhiFu += BenChang * 100 * cuHe;
                        if (QiaoShis.Count == 3)
                        {
                            // 3打ちの場合、北家分を折半
                            zhiFu += Ceil(Ceil(dian / 4, 100) / 2, 100);
                            zhiFu += BenChang * 100 / 2 * cuHe;
                        }
                        else if (QiaoShis.Count == 2)
                        {
                            // 2打ちの場合、全て支払
                            zhiFu = dian;
                            zhiFu += BenChang * 300 * cuHe;
                        }
                        QiaoShis[i % QiaoShis.Count].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    QiaoShis[ZiMoFan].DianBangJiSuan(shouQu);
                }
                // 供託
                QiaoShis[ZiMoFan].DianBangJiSuan(GongTuo);
                QiaoShis[ZiMoFan].ShouQuGongTuoJiSuan(GongTuo);
                // 記録 和了数
                QiaoShis[ZiMoFan].JiLu.heLeShu++;
                return;
            }
            else if (TaJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                // 栄和
                for (int i = 0; i < RongHeFan.Count; i++)
                {
                    dian = QiaoShis[RongHeFan[i].fan].HeLeDian;
                    // 本場
                    dian += BenChang * 300;

                    if (QiaoShis[RongHeFan[i].fan].BaoZeFan >= 0)
                    {
                        // 包則
                        int baoZeFan = (RongHeFan[i].fan + QiaoShis.Count - QiaoShis[RongHeFan[i].fan].BaoZeFan) % QiaoShis.Count;
                        shouQu = Ceil(dian / 2, 100);
                        QiaoShis[ZiMoFan].DianBangJiSuan(-(dian - shouQu));
                        QiaoShis[baoZeFan].DianBangJiSuan(-shouQu);
                    }
                    else
                    {
                        QiaoShis[ZiMoFan].DianBangJiSuan(-dian);
                    }
                    QiaoShis[RongHeFan[i].fan].DianBangJiSuan(dian);
                    // 供託
                    if (i == 0)
                    {
                        QiaoShis[RongHeFan[i].fan].DianBangJiSuan(GongTuo);
                        QiaoShis[RongHeFan[i].fan].ShouQuGongTuoJiSuan(GongTuo);

                    }
                    // 記録 和了数
                    QiaoShis[RongHeFan[i].fan].JiLu.heLeShu++;
                    // 記録 放銃数
                    QiaoShis[ZiMoFan].JiLu.fangChongShu++;
                    // 記録 和了点
                    QiaoShis[RongHeFan[i].fan].JiLu.heLeDian += QiaoShis[RongHeFan[i].fan].HeLeDian;
                    // 記録 放銃点
                    QiaoShis[ZiMoFan].JiLu.fangChongDian += QiaoShis[RongHeFan[i].fan].HeLeDian;
                }
                return;
            }

            // 流局
            if (liZhiShu >= 4 || SiFengZiLianDa || jiuZhongJiuPai)
            {
                return;
            }
            if (ZiJiaYao == QiaoShi.YaoDingYi.AnGang || ZiJiaYao == QiaoShi.YaoDingYi.JiaGang || TaJiaYao == QiaoShi.YaoDingYi.DaMingGang)
            {
                return;
            }
            // 記録 流局数(四家立直や四開槓で流局の場合はカウントしない)
            for (int i = ZiMoFan + 1; i < ZiMoFan + QiaoShis.Count; i++)
            {
                QiaoShi shi = QiaoShis[i % QiaoShis.Count];
                shi.JiLu.liuJuShu++;
            }

            // 形式聴牌計算
            int xingTingShu = 0;
            foreach (QiaoShi shi in QiaoShis)
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
            if (xingTingShu == QiaoShis.Count || xingTingShu == 0)
            {
                return;
            }
            foreach (QiaoShi shi in QiaoShis)
            {
                if (shi.XingTing)
                {
                    shi.DianBangJiSuan(3000 / xingTingShu);
                }
                else
                {
                    shi.DianBangJiSuan(-3000 / (QiaoShis.Count - xingTingShu));
                }
            }
        }

        // 供託計算
        internal static void DianGiSuanGongTuo()
        {
            int cuHe = 1;
            for (int i = ZiMoFan + 1; i < ZiMoFan + QiaoShis.Count; i++)
            {
                QiaoShi shi = QiaoShis[i % QiaoShis.Count];
                if (shi.CuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ZiJiaYao == QiaoShi.YaoDingYi.ZiMo || cuHe == -1 || TaJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                GongTuo = 0;
            }
        }

        // 錯和
        internal static void CuHe(int jia)
        {
            if (jia == Qin)
            {
                // 親
                QiaoShis[jia].DianBangJiSuan(-12000);
                for (int i = jia + 1; i < jia + QiaoShis.Count; i++)
                {
                    int taJia = i % QiaoShis.Count;
                    QiaoShis[taJia].DianBangJiSuan(12000 / (QiaoShis.Count - 1));
                }
            }
            else
            {
                // 子
                QiaoShis[jia].DianBangJiSuan(-8000);
                for (int i = jia + 1; i < jia + QiaoShis.Count; i++)
                {
                    int taJia = i % QiaoShis.Count;
                    int dian;
                    if (taJia == Qin)
                    {
                        dian = 4000;
                    }
                    else
                    {
                        dian = 2000;
                    }
                    if (QiaoShis.Count == 3)
                    {
                        dian += 1000;
                    }
                    QiaoShis[taJia].DianBangJiSuan(dian);
                }
            }
            CuHeFan = jia;
        }

        // 箱判定
        internal static bool XiangPanDing()
        {
            foreach (QiaoShi shi in QiaoShis)
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
            ChangFeng = Pai.FengPaiDingYi[feng];
        }
    }
}

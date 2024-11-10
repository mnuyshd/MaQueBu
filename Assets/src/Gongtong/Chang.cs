using System;
using Sikao;

namespace Gongtong
{
    // 場
    internal class Chang
    {
        // 面子
        internal static int mianZi = 4;

        // 雀士
        internal static QiaoShi[] qiaoShi = new QiaoShi[mianZi];
        // 場風
        internal static int changFeng;
        // 局
        internal static int ju;
        // 本場
        internal static int benChang;
        // 親
        internal static int qin;
        // 起家
        internal static int qiaJia;
        // 捨牌
        internal static int shePai;
        // 自摸番
        internal static int ziMoFan;
        // 鳴番
        internal static int mingFan;
        // 栄和数
        internal static int rongHeShu;
        // 栄和番
        internal static int[] rongHeFan;
        // 和了番
        internal static int heleFan;
        // 錯和番
        internal static int cuHeFan;
        // 風
        internal static int feng;
        // 立直数
        internal static int liZhiShu;
        // 四風子連打牌
        internal static int[] siFengZiLianDaPai;
        // 四風子連打牌位
        internal static int siFengZiLianDaPaiWei;
        // 四風子連打
        internal static bool siFengZiLianDa;
        // 九種九牌
        internal static bool jiuZhongJiuPai;
        // 供託
        internal static int gongTuo;
        // 供託(局初め)
        internal static int gongTuoJuChu;
        // サイコロ１
        internal static int sai1;
        // サイコロ２
        internal static int sai2;

        // 自家思考結果
        internal static QiaoShi.Yao ziJiaYao;
        // 自家選択
        internal static int ziJiaXuanZe;
        // 他家思考結果
        internal static QiaoShi.Yao taJiaYao;
        // 他家選択
        internal static int taJiaXuanZe;

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

        // 荘初期化
        internal static void ZhuangChuQiHua()
        {
            rongHeFan = new int[mianZi];
            siFengZiLianDaPai = new int[mianZi];
            benChang = 0;
            ju = 0;
            feng = 0;
            changFeng = Pai.FENG_PAI[feng];
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
            QiaoShi.Init(siFengZiLianDaPai, 0xff);
            siFengZiLianDaPaiWei = 0;
            ziJiaYao = QiaoShi.Yao.WU;
            taJiaYao = QiaoShi.Yao.WU;
            gongTuoJuChu = gongTuo;
            rongHeShu = 0;
            QiaoShi.Init(rongHeFan, 0xff);
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
                if (shi.cuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ziJiaYao == QiaoShi.Yao.ZI_MO || cuHe == -1)
            {
                // 自摸
                dian = qiaoShi[ziMoFan].heLeDian;
                // 記録 和了点
                qiaoShi[ziMoFan].jiLu.heLeDian += qiaoShi[ziMoFan].heLeDian;

                if (qiaoShi[ziMoFan].baoZeFan >= 0)
                {
                    // 包則
                    int baoZeFan = (ziMoFan + mianZi - qiaoShi[ziMoFan].baoZeFan) % mianZi;
                    shouQu = dian + benChang * 100 * cuHe;
                    qiaoShi[ziMoFan].DianBangJiSuan(shouQu);
                    qiaoShi[baoZeFan].DianBangJiSuan(-shouQu);
                    // 記録 親和了数
                    qiaoShi[ziMoFan].jiLu.qinHeLeShu++;
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
                    qiaoShi[ziMoFan].jiLu.qinHeLeShu++;
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
                qiaoShi[ziMoFan].shouQuGongTuo = gongTuo;
                gongTuo = 0;
                // 記録 和了数
                qiaoShi[ziMoFan].jiLu.heLeShu++;
                return;
            }
            else if (taJiaYao == QiaoShi.Yao.RONG_HE)
            {
                // 栄和
                for (int i = 0; i < rongHeShu; i++)
                {
                    dian = qiaoShi[rongHeFan[i]].heLeDian;
                    // 本場
                    dian += benChang * 300;

                    if (qiaoShi[rongHeFan[i]].baoZeFan >= 0)
                    {
                        // 包則
                        int baoZeFan = (rongHeFan[i] + mianZi - qiaoShi[rongHeFan[i]].baoZeFan) % mianZi;
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
                        qiaoShi[rongHeFan[i]].shouQuGongTuo = gongTuo;
                    }
                    // 記録 和了数
                    qiaoShi[rongHeFan[i]].jiLu.heLeShu++;
                    // 記録 放銃数
                    qiaoShi[ziMoFan].jiLu.fangChongShu++;
                    // 記録 和了点
                    qiaoShi[rongHeFan[i]].jiLu.heLeDian += qiaoShi[rongHeFan[i]].heLeDian;
                    // 記録 放銃点
                    qiaoShi[ziMoFan].jiLu.fangChongDian += qiaoShi[rongHeFan[i]].heLeDian;
                }
                gongTuo = 0;
                return;
            }

            // 流局
            if (liZhiShu >= 4 || siFengZiLianDa || jiuZhongJiuPai)
            {
                return;
            }
            if (ziJiaYao == QiaoShi.Yao.AN_GANG || ziJiaYao == QiaoShi.Yao.JIA_GANG || taJiaYao == QiaoShi.Yao.DA_MING_GANG)
            {
                return;
            }
            // 記録 流局数(四家立直や四開槓で流局の場合はカウントしない)
            for (int i = ziMoFan + 1; i < ziMoFan + mianZi; i++)
            {
                QiaoShi shi = qiaoShi[i % mianZi];
                shi.jiLu.liuJuShu++;
            }

            // 形式聴牌計算
            int xingTingShu = 0;
            for (int i = 0; i < mianZi; i++)
            {
                if (qiaoShi[i].xingTing)
                {
                    xingTingShu++;
                    // 記録 聴牌数
                    qiaoShi[i].jiLu.tingPaiShu++;
                }
                else
                {
                    // 記録 不聴数
                    qiaoShi[i].jiLu.buTingShu++;
                }
            }
            if (xingTingShu == mianZi || xingTingShu == 0)
            {
                return;
            }
            for (int i = 0; i < mianZi; i++)
            {
                if (qiaoShi[i].xingTing)
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
                if (qiaoShi[i].dianBang <= 0)
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
            if (feng >= Pai.FENG_PAI.Length)
            {
                feng = 0;
            }
            changFeng = Pai.FENG_PAI[feng];
        }
    }
}

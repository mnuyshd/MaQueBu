using System;

using Gongtong;

namespace Sikao
{
    // 効率雀士
    internal class QiaoXiaoLu : QiaoShi
    {
        // 手牌点数
        private readonly int[] shouPaiDian;

        // コンストラクタ
        internal QiaoXiaoLu(string mingQian) : base(mingQian)
        {
            shouPaiDian = new int[14];
        }

        // 手牌点数計算
        private void ShouPaiDianShuJiSuan()
        {
            Chang.Init(shouPaiDian, 0);

            for (int i = 0; i < ShouPaiWei; i++)
            {
                int dian = 0;
                dian += XuanShangPaiPanDing(ShouPai[i]) * 4;
                int p = ShouPai[i] & QIAO_PAI;
                if (p > ZI_PAI)
                {
                    dian += YiPaiPanDing(p);
                }
                else
                {
                    int s = p & SHU_PAI;
                    dian += 5 - Math.Abs(s - 5);
                }
                shouPaiDian[i] = dian;
            }
        }

        // 思考自家
        internal override void SiKaoZiJia()
        {
            // 和了判定
            if (HeLe)
            {
                // 自摸
                ZiJiaYao = Chang.YaoDingYi.ZiMo;
                ZiJiaXuanZe = 0;
                return;
            }

            if (LiZhi)
            {
                // 立直後自摸切
                ZiJiaYao = Chang.YaoDingYi.Wu;
                ZiJiaXuanZe = (ShouPaiWei - 1);
                return;
            }

            // 立直判定
            if (LiZhiKeNengShu > 0)
            {
                int wei = 0;
                int maxDaiPaiShu = 0;
                for (int i = 0; i < LiZhiKeNengShu; i++)
                {
                    DaiPaiJiSuan(LiZhiPaiWei[i]);
                    GongKaiPaiShuJiSuan();
                    int geHeDaiPaiShu = 0;
                    for (int j = 0; j < DaiPaiShu; j++)
                    {
                        int p = DaiPai[j] & QIAO_PAI;
                        if (p == 0xff)
                        {
                            break;
                        }
                        else
                        {
                            geHeDaiPaiShu += 4 - GongKaiPaiShu[p];
                        }
                    }
                    if (maxDaiPaiShu < geHeDaiPaiShu)
                    {
                        maxDaiPaiShu = geHeDaiPaiShu;
                        wei = LiZhiPaiWei[i];
                    }
                }
                ZiJiaYao = Chang.YaoDingYi.LiZhi;
                ZiJiaXuanZe = wei;
                return;
            }

            ShouPaiDianShuJiSuan();

            int minXiangTingShu = 99;
            int xuanZe = ShouPaiWei - 1;
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

            int maxYouXiaoPai = 0;
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
                    for (int k = 0; k < ShouPaiWei; k++)
                    {
                        XiangTingShuJiSuan(k);
                        if (minXiangTingShu > XiangTingShu)
                        {
                            GongKaiPaiShuJiSuan();
                            youXiaoPai += 4 - GongKaiPaiShu[Pai.QiaoPai[j]];
                            break;
                        }
                    }
                }
                UnityEngine.Debug.Log("i=" + i + " youXiaoPai=" + youXiaoPai);
                if (maxYouXiaoPai == youXiaoPai)
                {
                    UnityEngine.Debug.Log("i=" + i + " shouPaiDian[xuanZe]=" + shouPaiDian[xuanZe] + " shouPaiDian[i]=" + shouPaiDian[i]);
                }
                if (maxYouXiaoPai < youXiaoPai || (maxYouXiaoPai == youXiaoPai && shouPaiDian[xuanZe] >= shouPaiDian[i]))
                {
                    maxYouXiaoPai = youXiaoPai;
                    xuanZe = i;
                }
                Chang.Copy(shouPaiC, ShouPai);
            }

            ZiJiaYao = Chang.YaoDingYi.Wu;
            ZiJiaXuanZe = xuanZe;
        }

        // 思考他家
        internal override void SiKaoTaJia(int jia)
        {
            // 和了判定
            if (HeLe)
            {
                // 栄和
                TaJiaYao = Chang.YaoDingYi.RongHe;
                TaJiaXuanZe = 0;
                return;
            }

            TaJiaYao = Chang.YaoDingYi.Wu;
            TaJiaXuanZe = 0;
        }
    }
}

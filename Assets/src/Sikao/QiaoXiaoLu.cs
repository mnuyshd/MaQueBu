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
                ZiJiaXuanZe = ShouPaiWei - 1;
                return;
            }

            if (LiZhi)
            {
                // 立直後自摸切
                ZiJiaYao = Chang.YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPaiWei - 1;
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

            // 手牌点計算
            ShouPaiDianShuJiSuan();
            // 有効牌数計算
            YouXiaoPaiShuJiSuan();
            int xuanZe = ShouPaiWei - 1;
            int maxYouXiaoPai = 0;
            int minDian = 99;
            for (int i = 0; i < ShouPaiWei; i++)
            {
                if (maxYouXiaoPai < YouXiaoPaiShu[i] || (maxYouXiaoPai == YouXiaoPaiShu[i] && minDian >= shouPaiDian[i]))
                {
                    maxYouXiaoPai = YouXiaoPaiShu[i];
                    minDian = shouPaiDian[i];
                    xuanZe = i;
                }
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

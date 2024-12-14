using System;

using Gongtong;

namespace Sikao
{
    // 効率雀士
    internal class QiaoXiaoLu : QiaoShi
    {
        // 手牌有効度
        private readonly int[] shouPaiYouXiao;
        // 手牌安全度
        private readonly int[] shouPaiAnQuan;

        // コンストラクタ
        internal QiaoXiaoLu(string mingQian) : base(mingQian)
        {
            shouPaiYouXiao = new int[14];
            shouPaiAnQuan = new int[ShouPai.Length];
        }

        // 手牌有効度計算
        private void ShouPaiYouXiaoJiSuan()
        {
            Chang.Init(shouPaiYouXiao, 0);
            // 公開牌数計算
            GongKaiPaiShuJiSuan();
            // 有効度計算
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
                    if (s == 1)
                    {
                        dian += GongKaiPaiShu[p] + GongKaiPaiShu[p + 1] + GongKaiPaiShu[p + 2];
                    }
                    else if (s == 2)
                    {
                        dian += GongKaiPaiShu[p - 1] + GongKaiPaiShu[p] + GongKaiPaiShu[p + 1] + GongKaiPaiShu[p + 2];
                    }
                    else if (s <= 3 || s <= 7)
                    {
                        dian += GongKaiPaiShu[p - 2] + GongKaiPaiShu[p - 1] + GongKaiPaiShu[p] + GongKaiPaiShu[p + 1] + GongKaiPaiShu[p + 2];
                    }
                    else if (s == 8)
                    {
                        dian += GongKaiPaiShu[p - 2] + GongKaiPaiShu[p - 1] + GongKaiPaiShu[p] + GongKaiPaiShu[p + 1];
                    }
                    else if (s == 9)
                    {
                        dian += GongKaiPaiShu[p - 2] + GongKaiPaiShu[p - 1] + GongKaiPaiShu[p];
                    }
                }
                shouPaiYouXiao[i] = dian;
            }
        }
        private int LiZhiZheShu()
        {
            int liZhiShu = 0;
            for (int i = 0; i < Chang.MianZi; i++)
            {
                QiaoShi shi = Chang.QiaoShi[i];
                if (shi.Player)
                {
                    continue;
                }
                if (shi.LiZhi)
                {
                    liZhiShu++;
                }
            }
            return liZhiShu;
        }

        // 手牌安全度計算
        private void ShouPaiAnQuanJiSuan()
        {
            Chang.Init(shouPaiAnQuan, -10 * LiZhiZheShu());
            // 公開牌数計算
            GongKaiPaiShuJiSuan();
            for (int i = 0; i < Chang.MianZi; i++)
            {
                QiaoShi shi = Chang.QiaoShi[i];
                if (shi.Player)
                {
                    continue;
                }
                if (shi.LiZhi)
                {
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        int p = ShouPai[j] & QIAO_PAI;
                        int s = p & SHU_PAI;
                        if (shi.ShePaiShu[p] + shi.LiZhiShePaiShu[p] > 0)
                        {
                            // 現物
                            shouPaiAnQuan[j] += 10;
                        }
                        else
                        {
                            if ((p & ZI_PAI) == ZI_PAI)
                            {
                                // 字牌
                                shouPaiAnQuan[j] += GongKaiPaiShu[p] * 3;
                            }
                            else
                            {
                                // 数牌
                                if ((s == 1 && (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)) || (s == 9 && (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p -3] > 0)))
                                {
                                    shouPaiAnQuan[j] += 9;
                                }
                                else if ((s == 2 && (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)) || (s == 8 && (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0)))
                                {
                                    shouPaiAnQuan[j] += 8;
                                }
                                else if ((s == 3 && (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)) || (s == 7 && (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0)))
                                {
                                    shouPaiAnQuan[j] += 7;
                                }
                                else if (s >= 4 && s <= 6)
                                {
                                    if (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0)
                                    {
                                        shouPaiAnQuan[j] += 4;
                                    }
                                    if (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)
                                    {
                                        shouPaiAnQuan[j] += 4;
                                    }
                                }
                            }
                        }
                    }
                }
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
                ZiJiaXuanZe = PaiXuanZe(wei);
                return;
            }

            int xuanZe = ShouPaiWei - 1;
            // 向聴数計算
            XiangTingShuJiSuan(ShouPaiWei - 1);
            // 有効牌数計算
            YouXiaoPaiShuJiSuan();
            // 手牌有効度計算
            ShouPaiYouXiaoJiSuan();
            // 手牌安全度計算
            ShouPaiAnQuanJiSuan();
            if (LiZhiZheShu() >= 1)
            {
                if (XiangTingShu >= 2)
                {
                    // ベタオリ
                }
                else
                {
                    // 回し
                    for (int i = 0; i < ShouPaiWei; i++)
                    {
                        shouPaiAnQuan[i] += YouXiaoPaiShu[i];
                    }
                }
                int maxAnQuan = -99;
                int minDian = 99;
                for (int i = 0; i < ShouPaiWei; i++)
                {
                    if (maxAnQuan < shouPaiAnQuan[i] || (maxAnQuan == shouPaiAnQuan[i] && minDian >= shouPaiYouXiao[i]))
                    {
                        maxAnQuan = shouPaiAnQuan[i];
                        xuanZe = i;
                    }
                }
            }
            else
            {
                if (XiangTingShu == 0)
                {
                    xuanZe = PaiXuanZe(ShouPaiWei - 1);
                }
                else
                {
                    int maxYouXiaoPai = -99;
                    int minDian = 99;
                    for (int i = 0; i < ShouPaiWei; i++)
                    {
                        if (maxYouXiaoPai < YouXiaoPaiShu[i] || (maxYouXiaoPai == YouXiaoPaiShu[i] && minDian >= shouPaiYouXiao[i]))
                        {
                            maxYouXiaoPai = YouXiaoPaiShu[i];
                            minDian = shouPaiYouXiao[i];
                            xuanZe = i;
                        }
                    }
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
            // 聴牌判定
            XingTingPanDing();
            if (XingTing)
            {
                TaJiaYao = Chang.YaoDingYi.Wu;
                TaJiaXuanZe = 0;
                return;
            }

            // 大明槓
            int daMingGangXuanZe = MingXuanZe(DaMingGangPaiWei, DaMingGangKeNengShu);
            if (daMingGangXuanZe >= 0)
            {
                TaJiaYao = Chang.YaoDingYi.DaMingGang;
                TaJiaXuanZe = daMingGangXuanZe;
                return;
            }
            // 石並
            int bingXuanZe = MingXuanZe(BingPaiWei, BingKeNengShu);
            if (bingXuanZe >= 0)
            {
                TaJiaYao = Chang.YaoDingYi.Bing;
                TaJiaXuanZe = bingXuanZe;
                return;
            }
            // 吃
            int chiXuanZe = MingXuanZe(ChiPaiWei, ChiKeNengShu);
            if (chiXuanZe >= 0)
            {
                TaJiaYao = Chang.YaoDingYi.Chi;
                TaJiaXuanZe = chiXuanZe;
                return;
            }

            TaJiaYao = Chang.YaoDingYi.Wu;
            TaJiaXuanZe = 0;
        }

        // 牌選択(赤牌以外を優先)
        private int PaiXuanZe(int wei)
        {
            if (ShouPai[wei] < CHI_PAI)
            {
                return wei;
            }
            int p = ShouPai[wei] & QIAO_PAI;
            for (int i = 0; i < ShouPaiWei; i++)
            {
                if (p == ShouPai[i])
                {
                    return i;
                }
            }
            return wei;
        }
    }
}

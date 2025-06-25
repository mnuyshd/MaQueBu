using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Source.Gongtong;

namespace Assets.Source.Sikao
{
    // 機械雀士
    internal class QiaoJiXie : QiaoShi
    {
        internal enum XingGe
        {
            // 懸賞
            XUAN_SHANG,
            // 役牌
            YI_PAI,
            // 順子
            SHUN_ZI,
            // 刻子
            KE_ZI,
            // 立直
            LI_ZHI,
            // 鳴き
            MING,
            // 染め
            RAN,
            // 逃げ
            TAO,
        }

        // 脳
        protected Dictionary<XingGe, int> nao = new()
        {
            { XingGe.XUAN_SHANG, 50 },
            { XingGe.YI_PAI, 50 },
            { XingGe.SHUN_ZI, 50 },
            { XingGe.KE_ZI, 50 },
            { XingGe.LI_ZHI, 50 },
            { XingGe.MING, 50 },
            { XingGe.RAN, 50 },
            { XingGe.TAO, 50 },
        };
        internal Dictionary<XingGe, int> Nao
        {
            get { return nao; }
        }

        private readonly int[] shouPaiDianShu;

        // コンストラクタ
        internal QiaoJiXie(string mingQian) : base(mingQian)
        {
            shouPaiDianShu = new int[14];
        }

        // 点差
        private int DianCha()
        {
            // 最高得点
            int zuiGaoDingBang = 0;
            for (int i = 0; i < Chang.QiaoShis.Count; i++)
            {
                QiaoShi shi = Chang.QiaoShis[i];
                if (shi.DianBang > zuiGaoDingBang)
                {
                    zuiGaoDingBang = shi.DianBang;
                }
            }
            // 点差
            return zuiGaoDingBang - DianBang;
        }

        // 思考自家
        internal override void SiKaoZiJia()
        {
            // 和了判定
            if (HeLe)
            {
                // 自摸
                ZiJiaYao = YaoDingYi.ZiMo;
                ZiJiaXuanZe = ShouPai.Count - 1;
                return;
            }

            if (Chang.guiZe.jiuZhongJiuPaiLianZhuang > 0 && JiuZhongJiuPai)
            {
                int dian = DianCha() / 1000;
                if (nao[XingGe.YI_PAI] + dian < 50)
                {
                    // 九種九牌
                    ZiJiaYao = YaoDingYi.JiuZhongJiuPai;
                    ZiJiaXuanZe = ShouPai.Count - 1;
                    return;
                }
            }

            int wei;
            if (LiZhi)
            {
                // 暗槓判定
                wei = AnGang();
                if (wei >= 0)
                {
                    ZiJiaYao = YaoDingYi.AnGang;
                    ZiJiaXuanZe = wei;
                    return;
                }
                // 加槓判定
                wei = JiaGang();
                if (wei >= 0)
                {
                    ZiJiaYao = YaoDingYi.JiaGang;
                    ZiJiaXuanZe = wei;
                    return;
                }

                // 立直後自摸切
                ZiJiaYao = YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPai.Count - 1;
                return;
            }

            // 和了牌点(予想点 x 残牌数)
            GongKaiPaiShuJiSuan();
            foreach ((List<int> _, int w, int[] yuXiangDian) in HeLePai)
            {
                int dian = 0;
                for (int i = 0; i < yuXiangDian.Length; i++)
                {
                    if (yuXiangDian[i] > 0)
                    {
                        foreach (int stp in ShiTiPai)
                        {
                            if (i == stp)
                            {
                                continue;
                            }
                        }
                        dian += yuXiangDian[i] * Pai.CanShu(GongKaiPaiShu[i]);
                    }
                }
                ShouPaiDian[w] -= dian / (nao[XingGe.TAO] == 0 ? 1 : nao[XingGe.TAO]);
            }

            // 手牌点数計算
            ShouPaiDianShuJiSuan();

            if (AnGangPaiWei.Count > 0)
            {
                wei = AnGang();
                if (wei >= 0)
                {
                    // 暗槓
                    ZiJiaYao = YaoDingYi.AnGang;
                    ZiJiaXuanZe = 0;
                    return;
                }
            }
            if (JiaGangPaiWei.Count > 0)
            {
                wei = JiaGang();
                if (wei >= 0)
                {
                    // 加槓
                    ZiJiaYao = YaoDingYi.JiaGang;
                    ZiJiaXuanZe = wei;
                    return;
                }
            }

            wei = ShouPai.Count - 1;
            int minDian = 999;
            int maxShu = 0;
            for (int i = 0; i < ShouPai.Count; i++)
            {
                int sp = ShouPai[i];
                if (minDian > ShouPaiDian[i])
                {
                    minDian = ShouPaiDian[i];
                    maxShu = Math.Abs(5 - (sp & SHU_PAI));
                    wei = i;
                }
                else if (minDian == ShouPaiDian[i])
                {
                    int p = sp & QIAO_PAI;
                    int s = Math.Abs(5 - (sp & SHU_PAI));
                    if (p >= 0x30 || maxShu < s)
                    {
                        maxShu = s;
                        wei = i;
                    }
                }
            }

            // 待牌数
            int heLePaiShu = 0;
            foreach ((List<int> pais, int w, int[] _) in HeLePai)
            {
                if (wei == w)
                {
                    foreach (int pai in pais)
                    {
                        heLePaiShu += Pai.CanShu(GongKaiPaiShu[pai]);
                    }
                }
            }

            if (!IsKaiLiZhi())
            {
                foreach (int w in LiZhiPaiWei)
                {
                    if (wei == w)
                    {
                        // 点差
                        int dian = DianCha() / 1000;
                        dian += Pai.CanShanPaiShu();
                        if (nao[XingGe.LI_ZHI] + dian >= 100)
                        {
                            ZiJiaYao = YaoDingYi.LiZhi;
                        }
                        if (nao[XingGe.LI_ZHI] / 10 * heLePaiShu >= 50 - Pai.CanShanPaiShu())
                        {
                            ZiJiaYao = YaoDingYi.LiZhi;
                        }
                        if (Chang.guiZe.kaiLiZhi)
                        {
                            if (heLePaiShu * (nao[XingGe.LI_ZHI] / 5) + dian >= 100)
                            {
                                ZiJiaYao = YaoDingYi.KaiLiZhi;
                            }
                        }
                        ZiJiaXuanZe = PaiXuanZe(wei);
                        return;
                    }
                }
            }

            ZiJiaYao = YaoDingYi.Wu;
            ZiJiaXuanZe = PaiXuanZe(wei);
        }

        // 暗槓判定
        private int AnGang()
        {
            int minDian = 999;
            int wei = -1;
            for (int i = 0; i < AnGangPaiWei.Count; i++)
            {
                List<int> weis = AnGangPaiWei[i];
                int dian = 999;
                foreach (int w in weis)
                {
                    if (dian > ShouPaiDian[w])
                    {
                        dian = ShouPaiDian[w];
                        wei = i;
                    }
                }
                if (minDian > dian)
                {
                    minDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (minDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    if (minDian < nao[XingGe.XUAN_SHANG] - (nao[XingGe.SHUN_ZI] - 50))
                    {
                        for (int i = 0; i < ShouPai.Count; i++)
                        {
                            if (minDian > ShouPaiDian[i])
                            {
                                return -1;
                            }
                        }
                    }
                    // 暗槓
                    return wei;
                }
            }
            return -1;
        }

        // 加槓判定
        private int JiaGang()
        {
            int minDian = 999;
            int wei = -1;
            for (int i = 0; i < JiaGangPaiWei.Count; i++)
            {
                List<int> weis = JiaGangPaiWei[i];
                int dian = 0;
                foreach (int w in weis)
                {
                    dian += ShouPaiDian[w];
                }
                if (minDian > dian)
                {
                    minDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (minDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    if (minDian < nao[XingGe.XUAN_SHANG] - (nao[XingGe.SHUN_ZI] - 50))
                    {
                        for (int i = 0; i < ShouPai.Count; i++)
                        {
                            if (minDian > ShouPaiDian[i])
                            {
                                return -1;
                            }
                        }
                    }
                    // 加槓
                    return wei;
                }
            }
            return -1;
        }

        // 開立直確認
        private bool IsKaiLiZhi()
        {
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi.Feng == Feng)
                {
                    continue;
                }
                if (shi.KaiLiZhi)
                {
                    return true;
                }
            }
            return false;
        }

        // 思考他家
        internal override void SiKaoTaJia()
        {
            // 和了判定
            if (HeLe)
            {
                // 栄和
                TaJiaYao = YaoDingYi.RongHe;
                TaJiaXuanZe = 0;
                return;
            }

            // 聴牌判定
            XingTingPanDing();
            if (XingTing)
            {
                TaJiaYao = YaoDingYi.Wu;
                TaJiaXuanZe = 0;
                return;
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            // 副露牌数計算
            FuLuPaiShuJiSuan();
            // 色
            int se = SeSuan().se;
            // 点差
            int dianCha = DianCha();
            // 手牌点数計算
            ShouPaiDianShuJiSuan();

            // 危険度
            int weiXian = 0;
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi.Player)
                {
                    continue;
                }
                if (shi.LiZhi)
                {
                    weiXian += nao[XingGe.TAO] * (XiangTingShu == 0 ? 1 : XiangTingShu);
                }
            }

            if (IsKaiLiZhi())
            {
                if (FuLuPai.Count >= 2)
                {
                    TaJiaYao = YaoDingYi.Wu;
                    TaJiaXuanZe = 0;
                    return;
                }
            }

            // 大明槓判定
            int maxDian = 0;
            int wei = -1;
            for (int i = 0; i < DaMingGangPaiWei.Count; i++)
            {
                int dian = weiXian;
                foreach (int w in DaMingGangPaiWei[i])
                {
                    dian += ShouPaiDian[w];
                }
                if (maxDian < dian)
                {
                    maxDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (TaJiaFuLuShu == 0)
                {
                    maxDian += 100 - nao[XingGe.MING];
                }
                else
                {
                    maxDian -= nao[XingGe.MING];
                }
                maxDian -= dianCha / 100;
                if (maxDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 大明槓
                    TaJiaYao = YaoDingYi.DaMingGang;
                    TaJiaXuanZe = 0;
                    return;
                }
            }

            // 石並判定
            maxDian = 0;
            wei = -1;
            for (int i = 0; i < BingPaiWei.Count; i++)
            {
                int dian = weiXian;
                foreach (int w in BingPaiWei[i])
                {
                    dian += ShouPaiDian[w];
                }
                if (maxDian < dian)
                {
                    maxDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                int p = ShouPai[BingPaiWei[0][0]] & QIAO_PAI;
                int yiPai = YiPaiPanDing(p);
                if (yiPai > 0)
                {
                    maxDian -= YiPaiPanDing(p) * 20;
                }
                else
                {
                    if (TaJiaFuLuShu == 0)
                    {
                        maxDian += 100 - nao[XingGe.MING];
                    }
                    else
                    {
                        maxDian -= nao[XingGe.MING];
                    }
                }
                if ((p & SE_PAI) != se && (p & ZI_PAI) != ZI_PAI)
                {
                    maxDian += nao[XingGe.RAN];
                }
                if (!ZiPaiPanDing(p))
                {
                    int s = p & SHU_PAI;
                    if (s >= 2 && ShouPaiShu[p - 1] > 0)
                    {
                        maxDian += ShouPaiShu[p - 1] * (100 - nao[XingGe.MING]);
                    }
                    if (s <= 8 && ShouPaiShu[p + 1] > 0)
                    {
                        maxDian += ShouPaiShu[p + 1] * (100 - nao[XingGe.MING]);
                    }
                }
                if (maxDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 石並
                    TaJiaYao = YaoDingYi.Bing;
                    TaJiaXuanZe = wei;
                    return;
                }
            }

            // 吃判定
            int minDian = 999;
            wei = -1;
            for (int i = 0; i < ChiPaiWei.Count; i++)
            {
                int dian = weiXian;
                foreach (int w in ChiPaiWei[i])
                {
                    dian += ShouPaiDian[w];
                }
                if (minDian > dian)
                {
                    minDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (TaJiaFuLuShu == 0)
                {
                    minDian += 100 - nao[XingGe.MING];
                }
                int p = ShouPai[ChiPaiWei[0][0]] & QIAO_PAI;
                if (se != (p & SE_PAI))
                {
                    minDian += nao[XingGe.RAN];
                }
                if (minDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 吃
                    TaJiaYao = YaoDingYi.Chi;
                    TaJiaXuanZe = wei;
                    return;
                }
            }

            TaJiaYao = YaoDingYi.Wu;
            TaJiaXuanZe = 0;
        }

        // 色計算
        private (int se, int maxShu) SeSuan()
        {
            int[] shu = new int[4];
            Init(shu, 0);
            for (int i = 0; i < ShouPaiShu.Length; i++)
            {
                if (ShouPaiShu[i] > 0)
                {
                    shu[(i & 0xF0) >> 4] += ShouPaiShu[i];
                }
            }
            for (int i = 0; i < FuLuPaiShu.Length; i++)
            {
                if (FuLuPaiShu[i] > 0)
                {
                    shu[(i & 0xF0) >> 4] += FuLuPaiShu[i];
                }
            }
            int se = 0x00;
            int max = shu[se];
            if (max < shu[1])
            {
                max = shu[1];
                se = 0x10;
            }
            if (max < shu[2])
            {
                max = shu[2];
                se = 0x20;
            }
            return (se, max + shu[3]);
        }

        // 牌選択(赤牌以外を優先)
        protected int PaiXuanZe(int wei)
        {
            if (ShouPai[wei] < CHI_PAI)
            {
                return wei;
            }
            int p = ShouPai[wei] & QIAO_PAI;
            for (int i = 0; i < ShouPai.Count; i++)
            {
                if (p == ShouPai[i])
                {
                    return i;
                }
            }
            return wei;
        }

        // 手牌点数計算
        private void ShouPaiDianShuJiSuan()
        {
            // 手牌数計算
            ShouPaiShuJiSuan();
            // 副露牌数計算
            FuLuPaiShuJiSuan();
            // 公開牌数計算
            GongKaiPaiShuJiSuan();
            // 色
            (int se, int maxShu) = SeSuan();

            // 安全度
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi.Feng == Feng)
                {
                    continue;
                }
                if (shi.KaiLiZhi)
                {
                    if (shi.ZhenTingPanDing())
                    {
                        continue;
                    }
                    foreach (int dp in shi.DaiPai)
                    {
                        for (int k = 0; k < ShouPai.Count; k++)
                        {
                            int sp = ShouPai[k] & QIAO_PAI;
                            if (dp == sp)
                            {
                                ShouPaiDian[k] += 2000;
                            }
                        }
                    }
                }
                else
                {
                    if (shi.LiZhi || shi.FuLuPai.Count >= 3 || (Pai.CanShanPaiShu() <= XiangTingShu * 4))
                    {
                        // 得点掛率
                        float taoDian = nao[XingGe.TAO] / 50;
                        // 一発警戒
                        taoDian *= shi.YiFa ? nao[XingGe.TAO] / 25 : 1;
                        // 親警戒
                        taoDian *= shi.Feng == Chang.Qin ? nao[XingGe.TAO] / 25 : 1;
                        // シャンテン数分 降り気味
                        taoDian *= (XiangTingShu > 0 ? XiangTingShu : 1) * nao[XingGe.TAO] / 50;
                        for (int j = 0; j < ShouPai.Count; j++)
                        {
                            int p = ShouPai[j] & QIAO_PAI;
                            int s = p & SHU_PAI;
                            // 安全点
                            int anQuanDian = 0;
                            if (shi.ShePaiShu[p] + shi.LiZhiShePaiShu[p] > 0)
                            {
                                // 現物
                                anQuanDian += (int)(10 * taoDian);
                            }
                            else
                            {
                                if ((p & ZI_PAI) == ZI_PAI)
                                {
                                    // 字牌
                                    anQuanDian += (int)(GongKaiPaiShu[p] * 3 * taoDian);
                                }
                                else
                                {
                                    // 数牌
                                    if ((s == 1 && (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)) || (s == 9 && (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0)))
                                    {
                                        anQuanDian += (int)(9 * taoDian);
                                    }
                                    else if ((s == 2 && (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)) || (s == 8 && (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0)))
                                    {
                                        anQuanDian += (int)(8 * taoDian);
                                    }
                                    else if ((s == 3 && (shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)) || (s == 7 && (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0)))
                                    {
                                        anQuanDian += (int)(7 * taoDian);
                                    }
                                    else if (s >= 4 && s <= 6)
                                    {
                                        if (shi.ShePaiShu[p - 3] + shi.LiZhiShePaiShu[p - 3] > 0 && shi.ShePaiShu[p + 3] + shi.LiZhiShePaiShu[p + 3] > 0)
                                        {
                                            anQuanDian += (int)(7 * taoDian);
                                        }
                                    }
                                    // 壁
                                    if (s <= 3 && (GongKaiPaiShu[p + 1] >= 2 || GongKaiPaiShu[p + 2] >= 2))
                                    {
                                        anQuanDian += (int)(GongKaiPaiShu[p + 1] * GongKaiPaiShu[p + 2] * taoDian);
                                    }
                                    else if (s >= 7 && GongKaiPaiShu[p - 1] >= 2 && GongKaiPaiShu[p - 2] >= 2)
                                    {
                                        anQuanDian += (int)(GongKaiPaiShu[p - 1] * GongKaiPaiShu[p - 2] * taoDian);
                                    }
                                }
                            }
                            ShouPaiDian[j] -= anQuanDian;
                            if (anQuanDian == 0)
                            {
                                ShouPaiDian[j] += nao[XingGe.TAO];
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < ShouPai.Count; i++)
            {
                int sp = ShouPai[i];
                int p = sp & QIAO_PAI;
                foreach (int stp in ShiTiPai)
                {
                    if (p == stp)
                    {
                        // 食替牌
                        ShouPaiDian[i] += 1000;
                    }
                }

                // 懸賞牌
                ShouPaiDian[i] += nao[XingGe.XUAN_SHANG] / 4 * ShouPaiShu[p] * XuanShangPaiPanDing(sp);
                int s = p & SHU_PAI;
                if (p < ZI_PAI)
                {
                    if (s >= 2)
                    {
                        ShouPaiDian[i] += nao[XingGe.XUAN_SHANG] / 5 * XuanShangPaiPanDing(p - 1);
                    }
                    if (s <= 8)
                    {
                        ShouPaiDian[i] += nao[XingGe.XUAN_SHANG] / 5 * XuanShangPaiPanDing(p + 1);
                    }
                    if (s >= 3)
                    {
                        ShouPaiDian[i] += nao[XingGe.XUAN_SHANG] / 10 * XuanShangPaiPanDing(p - 2);
                    }
                    if (s <= 7)
                    {
                        ShouPaiDian[i] += nao[XingGe.XUAN_SHANG] / 10 * XuanShangPaiPanDing(p + 2);
                    }
                }
                // 役牌・風牌
                int yiPai = YiPaiPanDing(p);
                if (yiPai > 0)
                {
                    ShouPaiDian[i] += nao[XingGe.YI_PAI] / 15 * YiPaiPanDing(p);
                    if (p >= 0x35 && p <= 0x37)
                    {
                        ShouPaiDian[i] += 1;
                    }
                }
                // 染め
                if ((16 - maxShu <= nao[XingGe.RAN] / 10) && ((p & SE_PAI) == se || (p & ZI_PAI) == ZI_PAI))
                {
                    ShouPaiDian[i] += nao[XingGe.RAN] / 5;
                }
                // 字牌
                if ((p & ZI_PAI) == ZI_PAI)
                {
                    ShouPaiDian[i] -= nao[XingGe.YI_PAI] / 10 * (GongKaiPaiShu[p] - ShouPaiShu[p]);
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Init(shouPaiDianShu, 1);
            // 刻子
            for (int i = 0x01; i <= 0x37; i++)
            {
                if (ShouPaiShu[i] >= 3)
                {
                    int k = 3;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i)
                        {
                            if (k > 0)
                            {
                                ShouPaiDian[j] += nao[XingGe.KE_ZI] * 2;
                                ShouPaiShu[i]--;
                                k--;
                            }
                            else
                            {
                                if (p >= ZI_PAI)
                                {
                                    continue;
                                }
                                int s = p & SHU_PAI;
                                if ((s >= 3 && ShouPaiShu[i - 2] > 0 && ShouPaiShu[i - 1] > 0) || (s <= 7 && ShouPaiShu[i + 1] > 0 && ShouPaiShu[i + 2] > 0) || (s >= 2 && s <= 8 && ShouPaiShu[i - 1] > 0 && ShouPaiShu[i + 1] > 0))
                                {
                                    ShouPaiDian[j] += nao[XingGe.SHUN_ZI] * 2;
                                }
                                if ((s >= 3 && ShouPaiShu[i - 1] > 0) || (s <= 7 && ShouPaiShu[i + 1] > 0))
                                {
                                    ShouPaiDian[j] += nao[XingGe.SHUN_ZI] * 2 / 3;
                                }
                                if ((s >= 3 && ShouPaiShu[i - 2] > 0) || (s <= 7 && ShouPaiShu[i + 2] > 0))
                                {
                                    ShouPaiDian[j] += nao[XingGe.SHUN_ZI] * 2 / 4;
                                }
                                if ((s == 1 && ShouPaiShu[i + 1] > 0) || (s == 2 && ShouPaiShu[i - 1] > 0) || (s == 8 && ShouPaiShu[i + 1] > 0) || (s == 9 && ShouPaiShu[i - 1] > 0))
                                {
                                    ShouPaiDian[j] += nao[XingGe.SHUN_ZI] * 2 / 5;
                                }
                            }
                        }
                    }
                }
            }
            // 対子
            for (int i = 0x01; i <= 0x37; i++)
            {
                if (ShouPaiShu[i] >= 2)
                {
                    int k = 2;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && k > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.KE_ZI] / 3;
                            ShouPaiShu[i]--;
                            k--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Init(shouPaiDianShu, 1);
            // 順子(1-9)
            for (int i = 0x01; i <= 0x27; i++)
            {
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i + 1] >= 1 && ShouPaiShu[i + 2] >= 1)
                {
                    int m = Math.Min(Math.Min(ShouPaiShu[i], ShouPaiShu[i + 1]), ShouPaiShu[i + 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s2--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }
            // 両面(1-9)
            for (int i = 0x01; i <= 0x27; i++)
            {
                int s = i & SHU_PAI;
                if (s == 1 || s >= 8)
                {
                    continue;
                }
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i + 1] >= 1)
                {
                    int m = Math.Min(ShouPaiShu[i], ShouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }
            // 嵌張(1-9)
            for (int i = 0x01; i <= 0x27; i++)
            {
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i + 2] >= 1)
                {
                    int m = Math.Min(ShouPaiShu[i], ShouPaiShu[i + 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s2--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }
            // 辺張(1-9)
            for (int i = 0x01; i <= 0x28; i++)
            {
                int s = i & SHU_PAI;
                if ((s == 1 || s == 7) && ShouPaiShu[i] >= 1 && ShouPaiShu[i + 1] >= 1)
                {
                    int m = Math.Min(ShouPaiShu[i], ShouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Init(shouPaiDianShu, 1);
            // 順子(9-1)
            for (int i = 0x29; i >= 0x03; i--)
            {
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i - 1] >= 1 && ShouPaiShu[i - 2] >= 1)
                {
                    int m = Math.Min(Math.Min(ShouPaiShu[i], ShouPaiShu[i - 1]), ShouPaiShu[i - 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s2--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }
            // 両面(9-1)
            for (int i = 0x29; i >= 0x03; i--)
            {
                int s = i & SHU_PAI;
                if (s <= 2 || s == 9)
                {
                    continue;
                }
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i - 1] >= 1)
                {
                    int m = Math.Min(ShouPaiShu[i], ShouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }
            // 嵌張(9-1)
            for (int i = 0x29; i >= 0x03; i--)
            {
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i - 2] >= 1)
                {
                    int m = Math.Min(ShouPaiShu[i], ShouPaiShu[i - 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s2--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }
            // 辺張(9-1)
            for (int i = 0x29; i >= 0x02; i--)
            {
                int s = i & SHU_PAI;
                if ((s == 2 || s == 9) && ShouPaiShu[i] >= 1 && ShouPaiShu[i - 1] >= 1)
                {
                    int m = Math.Min(ShouPaiShu[i], ShouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            ShouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Init(shouPaiDianShu, 1);
            // 孤立牌への加減点
            for (int i = 0; i < ShouPai.Count; i++)
            {
                int p = ShouPai[i] & QIAO_PAI;
                if (p >= ZI_PAI || ShouPaiDian[i] > 0)
                {
                    continue;
                }
                int s = p & SHU_PAI;
                int dian = nao[XingGe.SHUN_ZI] / 25 * (5 - Math.Abs(s - 5));
                if (s <= 2 && ShouPaiShu[p] > 0 && ShouPaiShu[p + 1] == 0 && ShouPaiShu[p + 2] == 0 && ShouPaiShu[p + 3] > 0)
                {
                    ShouPaiDian[i] -= dian;
                }
                if (s >= 8 && ShouPaiShu[p] > 0 && ShouPaiShu[p - 1] == 0 && ShouPaiShu[p - 2] == 0 && ShouPaiShu[p - 3] > 0)
                {
                    ShouPaiDian[i] -= dian;
                }
                // 数牌
                if (s >= 2 && s <= 8)
                {
                    ShouPaiDian[i] += dian;
                }
            }
        }

        internal override IEnumerator SiKaoZiJiaCoroutine()
        {
            throw new NotImplementedException();
        }

        internal override IEnumerator SiKaoTaJiaCoroutine()
        {
            throw new NotImplementedException();
        }
    }
}

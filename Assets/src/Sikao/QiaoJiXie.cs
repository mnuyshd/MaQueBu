using System;
using System.Collections.Generic;

using Gongtong;

namespace Sikao
{
    // 雀士機械
    internal class QiaoJiXie : QiaoShi
    {
        protected enum XingGe
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
            // 国士無双
            GUO_SHI_WU_SHUANG,
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
            { XingGe.GUO_SHI_WU_SHUANG, 50 },
            { XingGe.TAO, 50 },
        };

        // 手牌点数
        private readonly int[] shouPaiDian;
        private readonly int[] shouPaiDianShu;

        // コンストラクタ
        internal QiaoJiXie(string mingQian) : base(mingQian)
        {
            shouPaiDian = new int[14];
            shouPaiDianShu = new int[14];
        }

        // 点差
        private int DianCha()
        {
            // 最高得点
            int zuiGaoDingBang = 0;
            for (int i = 0; i < Chang.MianZi; i++)
            {
                QiaoShi shi = Chang.QiaoShi[i];
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
                ZiJiaYao = Chang.YaoDingYi.ZiMo;
                ZiJiaXuanZe = ShouPaiWei - 1;
                return;
            }

            if (JiuZhongJiuPai)
            {
                int dian = DianCha() / 1000;
                if (nao[XingGe.GUO_SHI_WU_SHUANG] + dian < 50)
                {
                    // 九種九牌
                    ZiJiaYao = Chang.YaoDingYi.JiuZhongJiuPai;
                    ZiJiaXuanZe = ShouPaiWei - 1;
                    return;
                }
            }

            int wei;
            if (LiZhi)
            {
                // 暗槓判定
                int liDian = 999;
                wei = -1;
                for (int i = 0; i < AnGangKeNengShu; i++)
                {
                    int dian = 999;
                    for (int j = 0; j < AnGangPaiWei[i].Length; j++)
                    {
                        if (dian > shouPaiDian[AnGangPaiWei[i][j]])
                        {
                            dian = shouPaiDian[AnGangPaiWei[i][j]];
                            wei = i;
                        }
                    }
                    if (liDian > dian)
                    {
                        liDian = dian;
                        wei = i;
                    }
                }
                if (wei >= 0)
                {
                    if (liDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                    {
                        // 暗槓
                        ZiJiaYao = Chang.YaoDingYi.AnGang;
                        ZiJiaXuanZe = wei;
                        return;
                    }
                }
                // 加槓判定
                liDian = 999;
                wei = -1;
                for (int i = 0; i < JiaGangKeNengShu; i++)
                {
                    int dian = 0;
                    for (int j = 0; j < JiaGangPaiWei[i].Length; j++)
                    {
                        dian += shouPaiDian[JiaGangPaiWei[i][j]];
                    }
                    if (liDian > dian)
                    {
                        liDian = dian;
                        wei = i;
                    }
                }
                if (wei >= 0)
                {
                    if (liDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                    {
                        // 加槓
                        ZiJiaYao = Chang.YaoDingYi.JiaGang;
                        ZiJiaXuanZe = wei;
                        return;
                    }
                }
                // 立直後自摸切
                ZiJiaYao = Chang.YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPaiWei - 1;
                return;
            }

            // 手牌点数計算
            ShouPaiDianShuJiSuan();

            if (LiZhiKeNengShu > 0)
            {
                // 当牌の多い待ちを選択
                wei = 0;
                int maxDaiPaiShu = 0;
                int minShouPaiDian = 999;
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
                    if (maxDaiPaiShu < geHeDaiPaiShu || (maxDaiPaiShu == geHeDaiPaiShu && minShouPaiDian > shouPaiDian[LiZhiPaiWei[i]]))
                    {
                        maxDaiPaiShu = geHeDaiPaiShu;
                        minShouPaiDian = shouPaiDian[LiZhiPaiWei[i]];
                        wei = LiZhiPaiWei[i];
                    }
                }
                int dian = maxDaiPaiShu * 10;
                // 点差
                dian += DianCha() / 1000;
                dian += Pai.CanShanPaiShu() / 5;
                if (dian > 100 - nao[XingGe.LI_ZHI])
                {
                    ZiJiaYao = Chang.YaoDingYi.LiZhi;
                }
                else
                {
                    ZiJiaYao = Chang.YaoDingYi.Wu;
                }
                ZiJiaXuanZe = PaiXuanZe(wei);
                return;
            }
            else
            {
                // 当牌の多い待ちを選択
                wei = -1;
                int gao = 0;
                for (int i = 0; i < ShouPaiWei; i++)
                {
                    DaiPaiJiSuan(i);
                    bool shiTi = false;
                    for (int j = 0; j < ShiTiPaiShu; j++)
                    {
                        if (ShouPai[i] == ShiTiPai[j])
                        {
                            shiTi = true;
                            break;
                        }
                    }
                    if (DaiPaiShu > gao && !shiTi)
                    {
                        gao = DaiPaiShu;
                        wei = i;
                    }
                }
                if (wei >= 0)
                {
                    ZiJiaYao = Chang.YaoDingYi.Wu;
                    ZiJiaXuanZe = PaiXuanZe(wei);
                    return;
                }
            }

            if (AnGangKeNengShu > 0 && shouPaiDian[ShouPaiWei - 1] < nao[XingGe.SHUN_ZI])
            {
                // 暗槓
                ZiJiaYao = Chang.YaoDingYi.AnGang;
                ZiJiaXuanZe = 0;
                return;
            }
            if (JiaGangKeNengShu > 0 && shouPaiDian[ShouPaiWei - 1] < nao[XingGe.SHUN_ZI])
            {
                // 加槓
                ZiJiaYao = Chang.YaoDingYi.JiaGang;
                ZiJiaXuanZe = 0;
                return;
            }

            wei = ShouPaiWei - 1;
            int diDian = 999;
            int gaoShu = 0;
            for (int i = 0; i < ShouPaiWei; i++)
            {
                if (diDian > shouPaiDian[i])
                {
                    diDian = shouPaiDian[i];
                    gaoShu = Math.Abs(5 - (ShouPai[i] & SHU_PAI));
                    wei = i;
                }
                else if (diDian == shouPaiDian[i])
                {
                    int p = ShouPai[i] & QIAO_PAI;
                    int s = Math.Abs(5 - (ShouPai[i] & SHU_PAI));
                    if (p >= 0x30 || gaoShu < s)
                    {
                        gaoShu = s;
                        wei = i;
                    }
                }
            }

            ZiJiaYao = Chang.YaoDingYi.Wu;
            ZiJiaXuanZe = PaiXuanZe(wei);
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

            // 手牌数計算
            ShouPaiShuJiSuan();
            // 副露牌数計算
            FuLuPaiShuSuan();
            // 色
            int se = SeSuan().se;
            // 点差
            int dianCha = DianCha();
            // 手牌点数計算
            ShouPaiDianShuJiSuan();

            // 大明槓判定
            int gaoDian = 0;
            int wei = -1;
            for (int i = 0; i < DaMingGangKeNengShu; i++)
            {
                int dian = 0;
                for (int j = 0; j < DaMingGangPaiWei[i].Length; j++)
                {
                    dian += shouPaiDian[DaMingGangPaiWei[i][j]];
                }
                if (gaoDian < dian)
                {
                    gaoDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (TaJiaFuLuShu == 0)
                {
                    gaoDian += 100 - nao[XingGe.MING];
                }
                else
                {
                    gaoDian -= nao[XingGe.MING];
                }
                gaoDian -= dianCha / 100;
                if (gaoDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 大明槓
                    TaJiaYao = Chang.YaoDingYi.DaMingGang;
                    TaJiaXuanZe = 0;
                    return;
                }
            }

            // 石並判定
            gaoDian = 0;
            wei = -1;
            for (int i = 0; i < BingKeNengShu; i++)
            {
                int dian = 0;
                for (int j = 0; j < BingPaiWei[i].Length; j++)
                {
                    dian += shouPaiDian[BingPaiWei[i][j]];
                }
                if (gaoDian < dian)
                {
                    gaoDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                int p = ShouPai[BingPaiWei[0][0]] & QIAO_PAI;
                int yiPai = YiPaiPanDing(p);
                if (yiPai > 0)
                {
                    gaoDian -= YiPaiPanDing(p) * 20;
                }
                else
                {
                    if (TaJiaFuLuShu == 0)
                    {
                        gaoDian += 100 - nao[XingGe.MING];
                    }
                    else
                    {
                        gaoDian -= nao[XingGe.MING];
                    }
                }
                if ((p & SE_PAI) != se && (p & ZI_PAI) != ZI_PAI)
                {
                    gaoDian += nao[XingGe.RAN];
                }
                if (!ZiPaiPanDing(p))
                {
                    int s = p & SHU_PAI;
                    if (s >= 2 && ShouPaiShu[p - 1] > 0)
                    {
                        gaoDian += ShouPaiShu[p - 1] * (100 - nao[XingGe.MING]);
                    }
                    if (s <= 8 && ShouPaiShu[p + 1] > 0)
                    {
                        gaoDian += ShouPaiShu[p + 1] * (100 - nao[XingGe.MING]);
                    }
                }
                if (gaoDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 石並
                    TaJiaYao = Chang.YaoDingYi.Bing;
                    TaJiaXuanZe = wei;
                    return;
                }
            }

            // 吃判定
            int diDian = 999;
            wei = -1;
            for (int i = 0; i < ChiKeNengShu; i++)
            {
                int dian = 0;
                for (int j = 0; j < ChiPaiWei[i].Length; j++)
                {
                    dian += shouPaiDian[ChiPaiWei[i][j]];
                }
                if (diDian > dian)
                {
                    diDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (TaJiaFuLuShu == 0)
                {
                    diDian += 100 - nao[XingGe.MING];
                }
                int p = ShouPai[ChiPaiWei[0][0]] & QIAO_PAI;
                if (se != (p & SE_PAI))
                {
                    diDian += nao[XingGe.RAN];
                }
                if (diDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 吃
                    TaJiaYao = Chang.YaoDingYi.Chi;
                    TaJiaXuanZe = wei;
                    return;
                }
            }

            TaJiaYao = Chang.YaoDingYi.Wu;
            TaJiaXuanZe = 0;
        }

        // 色計算
        private (int se, int gao) SeSuan()
        {
            int[] shu = new int[4];
            Chang.Init(shu, 0);
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
            int gao = shu[se];
            if (gao < shu[1])
            {
                gao = shu[1];
                se = 0x10;
            }
            if (gao < shu[2])
            {
                gao = shu[2];
                se = 0x20;
            }
            return (se, gao + shu[3]);
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

        // 最小
        private int ZuiXiao(int n1, int n2)
        {
            return Math.Min(n1, n2);
        }
        // 最小
        private int ZuiXiao(int n1, int n2, int n3)
        {
            int m = ZuiXiao(n1, n2);
            return Math.Min(m, n3);
        }

        // 手牌点数計算
        private void ShouPaiDianShuJiSuan()
        {
            Chang.Init(shouPaiDian, 0);

            // 手牌数計算
            ShouPaiShuJiSuan();
            // 副露牌数計算
            FuLuPaiShuSuan();
            // 捨牌数計算
            GongKaiPaiShuJiSuan();
            // 色
            (int se, int gao) = SeSuan();

            int[] liZhiJiaShePaiShu = new int[0x40];
            Chang.Init(liZhiJiaShePaiShu, 0);
            for (int i = 0; i < Chang.MianZi; i++)
            {
                QiaoShi shi = Chang.QiaoShi[i];
                if (shi.Player)
                {
                    continue;
                }
                if (shi.LiZhi)
                {
                    for (int j = 0; j < shi.ShePaiShu.Length; j++)
                    {
                        liZhiJiaShePaiShu[j] += shi.ShePaiShu[j];
                    }
                }
            }

            for (int i = 0; i < ShouPaiWei; i++)
            {
                int p = ShouPai[i] & QIAO_PAI;
                int s = p & SHU_PAI;

                for (int j = 0; j < ShiTiPaiShu; j++)
                {
                    if (ShouPai[i] == ShiTiPai[j])
                    {
                        // 食替牌
                        shouPaiDian[i] += 1000;
                    }
                }

                // 懸賞牌
                shouPaiDian[i] += (nao[XingGe.XUAN_SHANG] / 6 / ShouPaiShu[p] * XuanShangPaiPanDing(ShouPai[i]));
                // 役牌・風牌
                shouPaiDian[i] += (nao[XingGe.YI_PAI] / 8 * YiPaiPanDing(p));
                // 染め
                if ((16 - gao <= nao[XingGe.RAN] / 10) && ((p & SE_PAI) == se || (p & ZI_PAI) == ZI_PAI))
                {
                    shouPaiDian[i] += nao[XingGe.RAN] / 5;
                }
                // 国士無双
                if (YaoJiuPaiJiSuan() >= 9 && nao[XingGe.GUO_SHI_WU_SHUANG] >= 70)
                {
                    shouPaiDian[i] += nao[XingGe.GUO_SHI_WU_SHUANG];
                }
                int taoDian = nao[XingGe.TAO];
                if (liZhiJiaShePaiShu[p] > 0)
                {
                    // 安牌
                    shouPaiDian[i] -= (taoDian / 5) * liZhiJiaShePaiShu[p];
                    // 壁
                    if (p < ZI_PAI)
                    {
                        if (s <= 3 && (GongKaiPaiShu[p + 1] >= 2 || GongKaiPaiShu[p + 2] >= 2))
                        {
                            shouPaiDian[i] -= taoDian / 15 * GongKaiPaiShu[p + 1] * GongKaiPaiShu[p + 2];
                        }
                        else if (s >= 7 && (GongKaiPaiShu[p - 1] >= 2 && GongKaiPaiShu[p - 2] >= 2))
                        {
                            shouPaiDian[i] -= taoDian / 15 * GongKaiPaiShu[p - 1] * GongKaiPaiShu[p - 2];
                        }
                    }
                }
                if (p >= ZI_PAI)
                {
                    // 字牌
                    shouPaiDian[i] -= (taoDian / 15 * GongKaiPaiShu[p]);
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Chang.Init(shouPaiDianShu, 1);
            // 刻子
            for (int i = 0x01; i <= 0x37; i++)
            {
                if (ShouPaiShu[i] >= 3)
                {
                    int k = 3;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && k > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.KE_ZI] * 2;
                            ShouPaiShu[i]--;
                            k--;
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
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && k > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.KE_ZI] / 5;
                            ShouPaiShu[i]--;
                            k--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Chang.Init(shouPaiDianShu, 1);
            // 順子(1-9)
            for (int i = 0x01; i <= 0x27; i++)
            {
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i + 1] >= 1 && ShouPaiShu[i + 2] >= 1)
                {
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i + 1], ShouPaiShu[i + 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
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
                if (s == 1 || s == 9)
                {
                    continue;
                }
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i + 1] >= 1)
                {
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
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
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i + 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
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
                if ((s == 1 || s == 7) && (ShouPaiShu[i] >= 1 && ShouPaiShu[i + 1] >= 1))
                {
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Chang.Init(shouPaiDianShu, 1);
            // 順子(9-1)
            for (int i = 0x29; i >= 0x03; i--)
            {
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i - 1] >= 1 && ShouPaiShu[i - 2] >= 1)
                {
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i - 1], ShouPaiShu[i - 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
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
                if (s == 1 || s == 9)
                {
                    continue;
                }
                if (ShouPaiShu[i] >= 1 && ShouPaiShu[i - 1] >= 1)
                {
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
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
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i - 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
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
                if ((s == 2 || s == 9) && (ShouPaiShu[i] >= 1 && ShouPaiShu[i - 1] >= 1))
                {
                    int m = ZuiXiao(ShouPaiShu[i], ShouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < ShouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = ShouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            ShouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            ShouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Chang.Init(shouPaiDianShu, 1);
            // 嵌張・辺張
            for (int i = 0; i < ShouPaiWei; i++)
            {
                int p = ShouPai[i] & QIAO_PAI;
                int s = p & SHU_PAI;
                if ((p & ZI_PAI) == ZI_PAI)
                {
                    continue;
                }
                if (s <= 7 && ShouPaiShu[p + 2] > 0)
                {
                    shouPaiDian[i] += 2;
                }
                if (s >= 3 && ShouPaiShu[p - 2] > 0)
                {
                    shouPaiDian[i] += 2;
                }
                if ((s == 8 && ShouPaiShu[p + 1] > 0) || (s == 2 && ShouPaiShu[p - 1] > 0))
                {
                    shouPaiDian[i] += 1;
                }
            }
        }
    }
}

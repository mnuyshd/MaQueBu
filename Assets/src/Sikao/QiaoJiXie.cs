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
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                if (shi.dianBang > zuiGaoDingBang)
                {
                    zuiGaoDingBang = shi.dianBang;
                }
            }
            // 点差
            return zuiGaoDingBang - dianBang;
        }


        // 思考自家
        internal override void SiKaoZiJia()
        {
            // 和了判定
            if (heLe)
            {
                // 自摸
                ziJiaYao = Yao.ZI_MO;
                ziJiaXuanZe = 0;
                return;
            }

            if (jiuZhongJiuPai)
            {
                int dian = DianCha() / 1000;
                if (nao[XingGe.GUO_SHI_WU_SHUANG] + dian < 50)
                {
                    // 九種九牌
                    ziJiaYao = Yao.JIU_ZHONG_JIU_PAI;
                    ziJiaXuanZe = 0;
                    return;
                }
            }

            int wei;
            if (liZhi)
            {
                // 暗槓判定
                int liDian = 999;
                wei = -1;
                for (int i = 0; i < anGangKeNengShu; i++)
                {
                    int dian = 999;
                    for (int j = 0; j < anGangPaiWei[i].Length; j++)
                    {
                        if (dian > shouPaiDian[anGangPaiWei[i][j]])
                        {
                            dian = shouPaiDian[anGangPaiWei[i][j]];
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
                        ziJiaYao = Yao.AN_GANG;
                        ziJiaXuanZe = wei;
                        return;
                    }
                }
                // 加槓判定
                liDian = 999;
                wei = -1;
                for (int i = 0; i < jiaGangKeNengShu; i++)
                {
                    int dian = 0;
                    for (int j = 0; j < jiaGangPaiWei[i].Length; j++)
                    {
                        dian += shouPaiDian[jiaGangPaiWei[i][j]];
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
                        ziJiaYao = Yao.JIA_GANG;
                        ziJiaXuanZe = wei;
                        return;
                    }
                }
                // 立直後自摸切
                ziJiaYao = Yao.WU;
                ziJiaXuanZe = (shouPaiWei - 1);
                return;
            }

            // 手牌点数計算
            ShouPaiDianShuJiSuan();

            if (liZhiKeNengShu > 0)
            {
                // 当牌の多い待ちを選択
                wei = 0;
                int maxDaiPaiShu = 0;
                int minShouPaiDian = 999;
                for (int i = 0; i < liZhiKeNengShu; i++)
                {
                    DaiPaiJiSuan(liZhiPaiWei[i]);
                    GongKaiPaiShuJiSuan();
                    int geHeDaiPaiShu = 0;
                    for (int j = 0; j < daiPaiShu; j++)
                    {
                        int p = daiPai[j] & QIAO_PAI;
                        if (p == 0xff)
                        {
                            break;
                        }
                        else
                        {
                            geHeDaiPaiShu += 4 - gongKaiPaiShu[p];
                        }
                    }
                    if (maxDaiPaiShu < geHeDaiPaiShu || (maxDaiPaiShu == geHeDaiPaiShu && minShouPaiDian > shouPaiDian[liZhiPaiWei[i]]))
                    {
                        maxDaiPaiShu = geHeDaiPaiShu;
                        minShouPaiDian = shouPaiDian[liZhiPaiWei[i]];
                        wei = liZhiPaiWei[i];
                    }
                }
                int dian = maxDaiPaiShu * 10;
                // 点差
                dian += DianCha() / 1000;
                dian += Pai.CanShanPaiShu() / 5;
                if (dian > 100 - nao[XingGe.LI_ZHI])
                {
                    ziJiaYao = Yao.LI_ZHI;
                }
                else
                {
                    ziJiaYao = Yao.WU;
                }
                ziJiaXuanZe = PaiXuanZe(wei);
                return;
            }
            else
            {
                // 当牌の多い待ちを選択
                wei = -1;
                int gao = 0;
                for (int i = 0; i < shouPaiWei; i++)
                {
                    DaiPaiJiSuan(i);
                    bool shiTi = false;
                    for (int j = 0; j < shiTiPaiShu; j++)
                    {
                        if (shouPai[i] == shiTiPai[j])
                        {
                            shiTi = true;
                            break;
                        }
                    }
                    if (daiPaiShu > gao && !shiTi)
                    {
                        gao = daiPaiShu;
                        wei = i;
                    }
                }
                if (wei >= 0)
                {
                    ziJiaYao = Yao.WU;
                    ziJiaXuanZe = PaiXuanZe(wei);
                    return;
                }
            }

            if (anGangKeNengShu > 0 && shouPaiDian[shouPaiWei - 1] < nao[XingGe.SHUN_ZI])
            {
                // 暗槓
                ziJiaYao = Yao.AN_GANG;
                ziJiaXuanZe = 0;
                return;
            }
            if (jiaGangKeNengShu > 0 && shouPaiDian[shouPaiWei - 1] < nao[XingGe.SHUN_ZI])
            {
                // 加槓
                ziJiaYao = Yao.JIA_GANG;
                ziJiaXuanZe = 0;
                return;
            }

            wei = shouPaiWei - 1;
            int diDian = 999;
            int gaoShu = 0;
            for (int i = 0; i < shouPaiWei; i++)
            {
                if (diDian > shouPaiDian[i])
                {
                    diDian = shouPaiDian[i];
                    gaoShu = Math.Abs(5 - (shouPai[i] & SHU_PAI));
                    wei = i;
                }
                else if (diDian == shouPaiDian[i])
                {
                    int p = shouPai[i] & QIAO_PAI;
                    int s = Math.Abs(5 - (shouPai[i] & SHU_PAI));
                    if (p >= 0x30 || gaoShu < s)
                    {
                        gaoShu = s;
                        wei = i;
                    }
                }
            }

            ziJiaYao = Yao.WU;
            ziJiaXuanZe = PaiXuanZe(wei);
        }

        // 思考他家
        internal override void SiKaoTaJia(int jia)
        {
            // 和了判定
            if (heLe)
            {
                // 栄和
                taJiaYao = Yao.RONG_HE;
                taJiaXuanZe = 0;
                return;
            }

            // 聴牌判定
            XingTingPanDing();
            if (xingTing)
            {
                taJiaYao = Yao.WU;
                taJiaXuanZe = 0;
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
            for (int i = 0; i < daMingGangKeNengShu; i++)
            {
                int dian = 0;
                for (int j = 0; j < daMingGangPaiWei[i].Length; j++)
                {
                    dian += shouPaiDian[daMingGangPaiWei[i][j]];
                }
                if (gaoDian < dian)
                {
                    gaoDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (fuLuPaiWei == 0)
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
                    taJiaYao = Yao.DA_MING_GANG;
                    taJiaXuanZe = 0;
                    return;
                }
            }

            // 石並判定
            gaoDian = 0;
            wei = -1;
            for (int i = 0; i < bingKeNengShu; i++)
            {
                int dian = 0;
                for (int j = 0; j < bingPaiWei[i].Length; j++)
                {
                    dian += shouPaiDian[bingPaiWei[i][j]];
                }
                if (gaoDian < dian)
                {
                    gaoDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                int p = shouPai[bingPaiWei[0][0]] & QIAO_PAI;
                int yiPai = YiPaiPanDing(p);
                if (yiPai > 0)
                {
                    gaoDian -= YiPaiPanDing(p) * 20;
                }
                else
                {
                    if (fuLuPaiWei == 0)
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
                    if (s >= 2 && shouPaiShu[p - 1] > 0)
                    {
                        gaoDian += shouPaiShu[p - 1] * (100 - nao[XingGe.MING]);
                    }
                    if (s <= 8 && shouPaiShu[p + 1] > 0)
                    {
                        gaoDian += shouPaiShu[p + 1] * (100 - nao[XingGe.MING]);
                    }
                }
                if (gaoDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 石並
                    taJiaYao = Yao.BING;
                    taJiaXuanZe = wei;
                    return;
                }
            }

            // 吃判定
            int diDian = 999;
            wei = -1;
            for (int i = 0; i < chiKeNengShu; i++)
            {
                int dian = 0;
                for (int j = 0; j < chiPaiWei[i].Length; j++)
                {
                    dian += shouPaiDian[chiPaiWei[i][j]];
                }
                if (diDian > dian)
                {
                    diDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (fuLuPaiWei == 0)
                {
                    diDian += 100 - nao[XingGe.MING];
                }
                int p = shouPai[chiPaiWei[0][0]] & QIAO_PAI;
                if (se != (p & SE_PAI))
                {
                    diDian += nao[XingGe.RAN];
                }
                if (diDian < nao[XingGe.MING] - (nao[XingGe.TAO] - 50))
                {
                    // 吃
                    taJiaYao = Yao.CHI;
                    taJiaXuanZe = wei;
                    return;
                }
            }

            taJiaYao = Yao.WU;
            taJiaXuanZe = 0;
        }

        // 色計算
        private (int se, int gao) SeSuan()
        {
            int[] shu = new int[4];
            Init(shu, 0);
            for (int i = 0; i < shouPaiShu.Length; i++)
            {
                if (shouPaiShu[i] > 0)
                {
                    shu[(i & 0xF0) >> 4] += shouPaiShu[i];
                }
            }
            for (int i = 0; i < fuLuPaiShu.Length; i++)
            {
                if (fuLuPaiShu[i] > 0)
                {
                    shu[(i & 0xF0) >> 4] += fuLuPaiShu[i];
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
            if (shouPai[wei] < 0x40)
            {
                return wei;
            }
            int p = shouPai[wei] & QIAO_PAI;
            for (int i = 0; i < shouPaiWei; i++)
            {
                if (p == shouPai[i])
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
            Init(shouPaiDian, 0);

            // 手牌数計算
            ShouPaiShuJiSuan();
            // 副露牌数計算
            FuLuPaiShuSuan();
            // 捨牌数計算
            GongKaiPaiShuJiSuan();
            // 色
            (int se, int gao) = SeSuan();

            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                int s = p & SHU_PAI;

                for (int j = 0; j < shiTiPaiShu; j++)
                {
                    if (shouPai[i] == shiTiPai[j])
                    {
                        // 食替牌
                        shouPaiDian[i] += 1000;
                    }
                }

                // 懸賞牌
                shouPaiDian[i] += (nao[XingGe.XUAN_SHANG] / 6 / shouPaiShu[p] * XuanShangPaiPanDing(shouPai[i]));
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
                        if (s <= 3 && (gongKaiPaiShu[p + 1] >= 2 || gongKaiPaiShu[p + 2] >= 2))
                        {
                            shouPaiDian[i] -= taoDian / 15 * gongKaiPaiShu[p + 1] * gongKaiPaiShu[p + 2];
                        }
                        else if (s >= 7 && (gongKaiPaiShu[p - 1] >= 2 && gongKaiPaiShu[p - 2] >= 2))
                        {
                            shouPaiDian[i] -= taoDian / 15 * gongKaiPaiShu[p - 1] * gongKaiPaiShu[p - 2];
                        }
                    }
                }
                if (p >= ZI_PAI)
                {
                    // 字牌
                    shouPaiDian[i] -= (taoDian / 15 * gongKaiPaiShu[p]);
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Init(shouPaiDianShu, 1);
            // 刻子
            for (int i = 0x01; i <= 0x37; i++)
            {
                if (shouPaiShu[i] >= 3)
                {
                    int k = 3;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && k > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.KE_ZI] * 2;
                            shouPaiShu[i]--;
                            k--;
                        }
                    }
                }
            }
            // 対子
            for (int i = 0x01; i <= 0x37; i++)
            {
                if (shouPaiShu[i] >= 2)
                {
                    int k = 2;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && k > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.KE_ZI] / 5;
                            shouPaiShu[i]--;
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
                if (shouPaiShu[i] >= 1 && shouPaiShu[i + 1] >= 1 && shouPaiShu[i + 2] >= 1)
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i + 1], shouPaiShu[i + 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s2--;
                            shouPaiShu[p]--;
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
                if (shouPaiShu[i] >= 1 && shouPaiShu[i + 1] >= 1)
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }
            // 嵌張(1-9)
            for (int i = 0x01; i <= 0x27; i++)
            {
                if (shouPaiShu[i] >= 1 && shouPaiShu[i + 2] >= 1)
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i + 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s2--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }
            // 辺張(1-9)
            for (int i = 0x01; i <= 0x28; i++)
            {
                int s = i & SHU_PAI;
                if ((s == 1 || s == 7) && (shouPaiShu[i] >= 1 && shouPaiShu[i + 1] >= 1))
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
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
                if (shouPaiShu[i] >= 1 && shouPaiShu[i - 1] >= 1 && shouPaiShu[i - 2] >= 1)
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i - 1], shouPaiShu[i - 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI];
                            shouPaiDianShu[j]--;
                            s2--;
                            shouPaiShu[p]--;
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
                if (shouPaiShu[i] >= 1 && shouPaiShu[i - 1] >= 1)
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 3;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }
            // 嵌張(9-1)
            for (int i = 0x29; i >= 0x03; i--)
            {
                if (shouPaiShu[i] >= 1 && shouPaiShu[i - 2] >= 1)
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i - 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 4;
                            shouPaiDianShu[j]--;
                            s2--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }
            // 辺張(9-1)
            for (int i = 0x29; i >= 0x02; i--)
            {
                int s = i & SHU_PAI;
                if ((s == 2 || s == 9) && (shouPaiShu[i] >= 1 && shouPaiShu[i - 1] >= 1))
                {
                    int m = ZuiXiao(shouPaiShu[i], shouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPaiWei; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QIAO_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += nao[XingGe.SHUN_ZI] / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Init(shouPaiDianShu, 1);
            // 嵌張・辺張
            for (int i = 0; i < shouPaiWei; i++)
            {
                int p = shouPai[i] & QIAO_PAI;
                int s = p & SHU_PAI;
                if ((p & ZI_PAI) == ZI_PAI)
                {
                    continue;
                }
                if (s <= 7 && shouPaiShu[p + 2] > 0)
                {
                    shouPaiDian[i] += 2;
                }
                if (s >= 3 && shouPaiShu[p - 2] > 0)
                {
                    shouPaiDian[i] += 2;
                }
                if ((s == 8 && shouPaiShu[p + 1] > 0) || (s == 2 && shouPaiShu[p - 1] > 0))
                {
                    shouPaiDian[i] += 1;
                }
            }
        }
    }
}

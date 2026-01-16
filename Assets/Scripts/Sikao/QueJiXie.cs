using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Gongtong;

namespace Assets.Scripts.Sikao
{
    // 機械雀士
    public class QueJiXie : QueShi
    {
        // 脳
        private int[] shouPaiDianShu;

        // コンストラクタ
        public QueJiXie(string mingQian) : base(mingQian)
        {
            naos.Add(new Nao { xingGe = XingGe.XUAN_SHANG, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.YI_PAI, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.SHUN_ZI, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.KE_ZI, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.LI_ZHI, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.MING, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.RAN, score = 50 });
            naos.Add(new Nao { xingGe = XingGe.TAO, score = 50 });

            shouPaiDianShu = new int[14];
        }

        public override QueShi GetQueShi(string jsonText)
        {
            return (QueShi)JsonUtility.FromJson(jsonText, GetType());
        }

        // 点差
        private int DianCha()
        {
            // 最高得点
            int zuiGaoDingBang = 0;
            for (int i = 0; i < MaQue.Instance.queShis.Count; i++)
            {
                QueShi shi = MaQue.Instance.queShis[i];
                if (shi.dianBang > zuiGaoDingBang)
                {
                    zuiGaoDingBang = shi.dianBang;
                }
            }
            // 点差
            return zuiGaoDingBang - dianBang;
        }

        // 思考自家
        public override void SiKaoZiJia()
        {
            // 和了判定
            if (heLe)
            {
                // 自摸
                ziJiaYao = YaoDingYi.ZiMo;
                ziJiaXuanZe = shouPai.Count - 1;
                return;
            }

            if (GuiZe.Instance.jiuZhongJiuPaiLianZhuang > 0 && jiuZhongJiuPai)
            {
                int dian = DianCha() / 1000;
                if (naos[(int)XingGe.YI_PAI].score + dian < 50)
                {
                    // 九種九牌
                    ziJiaYao = YaoDingYi.JiuZhongJiuPai;
                    ziJiaXuanZe = shouPai.Count - 1;
                    return;
                }
            }

            int wei;
            if (liZhi)
            {
                // 暗槓判定
                wei = AnGang();
                if (wei >= 0)
                {
                    ziJiaYao = YaoDingYi.AnGang;
                    ziJiaXuanZe = wei;
                    return;
                }
                // 加槓判定
                wei = JiaGang();
                if (wei >= 0)
                {
                    ziJiaYao = YaoDingYi.JiaGang;
                    ziJiaXuanZe = wei;
                    return;
                }

                // 立直後自摸切
                ziJiaYao = YaoDingYi.Wu;
                ziJiaXuanZe = shouPai.Count - 1;
                return;
            }

            // 和了牌点(予想点 x 残牌数)
            GongKaiPaiShuJiSuan();
            foreach ((List<int> _, int w, int[] yuXiangDian) in heLePai)
            {
                int dian = 0;
                for (int i = 0; i < yuXiangDian.Length; i++)
                {
                    if (yuXiangDian[i] > 0)
                    {
                        foreach (int stp in shiTiPai)
                        {
                            if (i == stp)
                            {
                                continue;
                            }
                        }
                        dian += yuXiangDian[i] * Pai.Instance.CanShu(gongKaiPaiShu[i]);
                    }
                }
                shouPaiDian[w] -= dian / (naos[(int)XingGe.TAO].score == 0 ? 1 : naos[(int)XingGe.TAO].score);
            }

            // 手牌点数計算
            ShouPaiDianShuJiSuan();

            if (anGangPaiWei.Count > 0)
            {
                wei = AnGang();
                if (wei >= 0)
                {
                    // 暗槓
                    ziJiaYao = YaoDingYi.AnGang;
                    ziJiaXuanZe = 0;
                    return;
                }
            }
            if (jiaGangPaiWei.Count > 0)
            {
                wei = JiaGang();
                if (wei >= 0)
                {
                    // 加槓
                    ziJiaYao = YaoDingYi.JiaGang;
                    ziJiaXuanZe = wei;
                    return;
                }
            }

            wei = shouPai.Count - 1;
            int minDian = 999;
            int maxShu = 0;
            for (int i = 0; i < shouPai.Count; i++)
            {
                int sp = shouPai[i];
                if (minDian > shouPaiDian[i])
                {
                    minDian = shouPaiDian[i];
                    maxShu = Math.Abs(5 - (sp & SHU_PAI));
                    wei = i;
                }
                else if (minDian == shouPaiDian[i])
                {
                    int p = sp & QUE_PAI;
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
            foreach ((List<int> pais, int w, int[] _) in heLePai)
            {
                if (wei == w)
                {
                    foreach (int pai in pais)
                    {
                        heLePaiShu += Pai.Instance.CanShu(gongKaiPaiShu[pai]);
                    }
                }
            }

            if (!IsKaiLiZhi())
            {
                foreach (int w in liZhiPaiWei)
                {
                    if (wei == w)
                    {
                        // 点差
                        int dian = DianCha() / 1000;
                        dian += Pai.Instance.CanShanPaiShu();
                        if (naos[(int)XingGe.LI_ZHI].score + dian >= 100)
                        {
                            ziJiaYao = YaoDingYi.LiZhi;
                        }
                        if (naos[(int)XingGe.LI_ZHI].score / 10 * heLePaiShu >= 50 - Pai.Instance.CanShanPaiShu())
                        {
                            ziJiaYao = YaoDingYi.LiZhi;
                        }
                        if (GuiZe.Instance.kaiLiZhi)
                        {
                            if (heLePaiShu * (naos[(int)XingGe.LI_ZHI].score / 5) + dian >= 100)
                            {
                                ziJiaYao = YaoDingYi.KaiLiZhi;
                            }
                        }
                        ziJiaXuanZe = PaiXuanZe(wei);
                        return;
                    }
                }
            }

            ziJiaYao = YaoDingYi.Wu;
            ziJiaXuanZe = PaiXuanZe(wei);
        }

        // 暗槓判定
        private int AnGang()
        {
            int minDian = 999;
            int wei = -1;
            for (int i = 0; i < anGangPaiWei.Count; i++)
            {
                List<int> weis = anGangPaiWei[i];
                int dian = 999;
                foreach (int w in weis)
                {
                    if (dian > shouPaiDian[w])
                    {
                        dian = shouPaiDian[w];
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
                if (minDian < naos[(int)XingGe.MING].score - (naos[(int)XingGe.TAO].score - 50))
                {
                    if (minDian < naos[(int)XingGe.XUAN_SHANG].score - (naos[(int)XingGe.SHUN_ZI].score - 50))
                    {
                        for (int i = 0; i < shouPai.Count; i++)
                        {
                            if (minDian > shouPaiDian[i])
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
            for (int i = 0; i < jiaGangPaiWei.Count; i++)
            {
                List<int> weis = jiaGangPaiWei[i];
                int dian = 0;
                foreach (int w in weis)
                {
                    dian += shouPaiDian[w];
                }
                if (minDian > dian)
                {
                    minDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (minDian < naos[(int)XingGe.MING].score - (naos[(int)XingGe.TAO].score - 50))
                {
                    if (minDian < naos[(int)XingGe.XUAN_SHANG].score - (naos[(int)XingGe.SHUN_ZI].score - 50))
                    {
                        for (int i = 0; i < shouPai.Count; i++)
                        {
                            if (minDian > shouPaiDian[i])
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
            foreach (QueShi shi in MaQue.Instance.queShis)
            {
                if (shi.feng == feng)
                {
                    continue;
                }
                if (shi.kaiLiZhi)
                {
                    return true;
                }
            }
            return false;
        }

        // 思考他家
        public override void SiKaoTaJia()
        {
            // 和了判定
            if (heLe)
            {
                // 栄和
                taJiaYao = YaoDingYi.RongHe;
                taJiaXuanZe = 0;
                return;
            }

            // 聴牌判定
            XingTingPanDing();
            if (xingTing)
            {
                taJiaYao = YaoDingYi.Wu;
                taJiaXuanZe = 0;
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
            foreach (QueShi shi in MaQue.Instance.queShis)
            {
                if (shi.player)
                {
                    continue;
                }
                if (shi.liZhi)
                {
                    weiXian += naos[(int)XingGe.TAO].score * (xiangTingShu == 0 ? 1 : xiangTingShu);
                }
            }

            if (IsKaiLiZhi())
            {
                if (fuLuPais.Count >= 2)
                {
                    taJiaYao = YaoDingYi.Wu;
                    taJiaXuanZe = 0;
                    return;
                }
            }

            // 大明槓判定
            int maxDian = 0;
            int wei = -1;
            for (int i = 0; i < daMingGangPaiWei.Count; i++)
            {
                int dian = weiXian;
                foreach (int w in daMingGangPaiWei[i])
                {
                    dian += shouPaiDian[w];
                }
                if (maxDian < dian)
                {
                    maxDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (taJiaFuLuShu == 0)
                {
                    maxDian += 100 - naos[(int)XingGe.MING].score;
                }
                else
                {
                    maxDian -= naos[(int)XingGe.MING].score;
                }
                maxDian -= dianCha / 100;
                if (maxDian < naos[(int)XingGe.MING].score - (naos[(int)XingGe.TAO].score - 50))
                {
                    // 大明槓
                    taJiaYao = YaoDingYi.DaMingGang;
                    taJiaXuanZe = 0;
                    return;
                }
            }

            // 石並判定
            maxDian = 0;
            wei = -1;
            for (int i = 0; i < bingPaiWei.Count; i++)
            {
                int dian = weiXian;
                foreach (int w in bingPaiWei[i])
                {
                    dian += shouPaiDian[w];
                }
                if (maxDian < dian)
                {
                    maxDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                int p = shouPai[bingPaiWei[0][0]] & QUE_PAI;
                int yiPai = YiPaiPanDing(p);
                if (yiPai > 0)
                {
                    maxDian -= YiPaiPanDing(p) * 20;
                }
                else
                {
                    if (taJiaFuLuShu == 0)
                    {
                        maxDian += 100 - naos[(int)XingGe.MING].score;
                    }
                    else
                    {
                        maxDian -= naos[(int)XingGe.MING].score;
                    }
                }
                if ((p & SE_PAI) != se && (p & ZI_PAI) != ZI_PAI)
                {
                    maxDian += naos[(int)XingGe.RAN].score;
                }
                if (!ZiPaiPanDing(p))
                {
                    int s = p & SHU_PAI;
                    if (s >= 2 && shouPaiShu[p - 1] > 0)
                    {
                        maxDian += shouPaiShu[p - 1] * (100 - naos[(int)XingGe.MING].score);
                    }
                    if (s <= 8 && shouPaiShu[p + 1] > 0)
                    {
                        maxDian += shouPaiShu[p + 1] * (100 - naos[(int)XingGe.MING].score);
                    }
                }
                if (maxDian < naos[(int)XingGe.MING].score - (naos[(int)XingGe.TAO].score - 50))
                {
                    // 石並
                    taJiaYao = YaoDingYi.Bing;
                    taJiaXuanZe = wei;
                    return;
                }
            }

            // 吃判定
            int minDian = 999;
            wei = -1;
            for (int i = 0; i < chiPaiWei.Count; i++)
            {
                int dian = weiXian;
                foreach (int w in chiPaiWei[i])
                {
                    dian += shouPaiDian[w];
                }
                if (minDian > dian)
                {
                    minDian = dian;
                    wei = i;
                }
            }
            if (wei >= 0)
            {
                if (taJiaFuLuShu == 0)
                {
                    minDian += 100 - naos[(int)XingGe.MING].score;
                }
                int p = shouPai[chiPaiWei[0][0]] & QUE_PAI;
                if (se != (p & SE_PAI))
                {
                    minDian += naos[(int)XingGe.RAN].score;
                }
                if (minDian < naos[(int)XingGe.MING].score - (naos[(int)XingGe.TAO].score - 50))
                {
                    // 吃
                    taJiaYao = YaoDingYi.Chi;
                    taJiaXuanZe = wei;
                    return;
                }
            }

            taJiaYao = YaoDingYi.Wu;
            taJiaXuanZe = 0;
        }

        // 色計算
        private (int se, int maxShu) SeSuan()
        {
            int[] shu = new int[4];
            Array.Fill(shu, 0);
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
            if (shouPai[wei] < CHI_PAI)
            {
                return wei;
            }
            int p = shouPai[wei] & QUE_PAI;
            for (int i = 0; i < shouPai.Count; i++)
            {
                if (p == shouPai[i])
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
            foreach (QueShi shi in MaQue.Instance.queShis)
            {
                if (shi.feng == feng)
                {
                    continue;
                }
                if (shi.kaiLiZhi)
                {
                    if (shi.ZhenTingPanDing())
                    {
                        continue;
                    }
                    foreach (int dp in shi.daiPai)
                    {
                        for (int k = 0; k < shouPai.Count; k++)
                        {
                            int sp = shouPai[k] & QUE_PAI;
                            if (dp == sp)
                            {
                                shouPaiDian[k] += 2000;
                            }
                        }
                    }
                }
                else
                {
                    if (shi.liZhi || shi.fuLuPais.Count >= 3 || (Pai.Instance.CanShanPaiShu() <= xiangTingShu * 4))
                    {
                        // 得点掛率
                        float taoDian = naos[(int)XingGe.TAO].score / 50;
                        // 一発警戒
                        taoDian *= shi.yiFa ? naos[(int)XingGe.TAO].score / 25 : 1;
                        // 親警戒
                        taoDian *= shi.feng == Chang.Instance.qin ? naos[(int)XingGe.TAO].score / 25 : 1;
                        // シャンテン数分 降り気味
                        taoDian *= (xiangTingShu > 0 ? xiangTingShu : 1) * naos[(int)XingGe.TAO].score / 50;
                        for (int j = 0; j < shouPai.Count; j++)
                        {
                            int p = shouPai[j] & QUE_PAI;
                            int s = p & SHU_PAI;
                            // 安全点
                            int anQuanDian = 0;
                            if (shi.shePaiShu[p] + shi.liZhiShePaiShu[p] > 0)
                            {
                                // 現物
                                anQuanDian += (int)(10 * taoDian);
                            }
                            else
                            {
                                if ((p & ZI_PAI) == ZI_PAI)
                                {
                                    // 字牌
                                    anQuanDian += (int)(gongKaiPaiShu[p] * 3 * taoDian);
                                }
                                else
                                {
                                    // 数牌
                                    if ((s == 1 && (shi.shePaiShu[p + 3] + shi.liZhiShePaiShu[p + 3] > 0)) || (s == 9 && (shi.shePaiShu[p - 3] + shi.liZhiShePaiShu[p - 3] > 0)))
                                    {
                                        anQuanDian += (int)(9 * taoDian);
                                    }
                                    else if ((s == 2 && (shi.shePaiShu[p + 3] + shi.liZhiShePaiShu[p + 3] > 0)) || (s == 8 && (shi.shePaiShu[p - 3] + shi.liZhiShePaiShu[p - 3] > 0)))
                                    {
                                        anQuanDian += (int)(8 * taoDian);
                                    }
                                    else if ((s == 3 && (shi.shePaiShu[p + 3] + shi.liZhiShePaiShu[p + 3] > 0)) || (s == 7 && (shi.shePaiShu[p - 3] + shi.liZhiShePaiShu[p - 3] > 0)))
                                    {
                                        anQuanDian += (int)(7 * taoDian);
                                    }
                                    else if (s >= 4 && s <= 6)
                                    {
                                        if (shi.shePaiShu[p - 3] + shi.liZhiShePaiShu[p - 3] > 0 && shi.shePaiShu[p + 3] + shi.liZhiShePaiShu[p + 3] > 0)
                                        {
                                            anQuanDian += (int)(7 * taoDian);
                                        }
                                    }
                                    // 壁
                                    if (s <= 3 && (gongKaiPaiShu[p + 1] >= 2 || gongKaiPaiShu[p + 2] >= 2))
                                    {
                                        anQuanDian += (int)(gongKaiPaiShu[p + 1] * gongKaiPaiShu[p + 2] * taoDian);
                                    }
                                    else if (s >= 7 && gongKaiPaiShu[p - 1] >= 2 && gongKaiPaiShu[p - 2] >= 2)
                                    {
                                        anQuanDian += (int)(gongKaiPaiShu[p - 1] * gongKaiPaiShu[p - 2] * taoDian);
                                    }
                                }
                            }
                            shouPaiDian[j] -= anQuanDian;
                            if (anQuanDian == 0)
                            {
                                shouPaiDian[j] += naos[(int)XingGe.TAO].score;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < shouPai.Count; i++)
            {
                int sp = shouPai[i];
                int p = sp & QUE_PAI;
                foreach (int stp in shiTiPai)
                {
                    if (p == stp)
                    {
                        // 食替牌
                        shouPaiDian[i] += 1000;
                    }
                }

                // 懸賞牌
                shouPaiDian[i] += naos[(int)XingGe.XUAN_SHANG].score / 4 * shouPaiShu[p] * XuanShangPaiPanDing(sp);
                int s = p & SHU_PAI;
                if (p < ZI_PAI)
                {
                    if (s >= 2)
                    {
                        shouPaiDian[i] += naos[(int)XingGe.XUAN_SHANG].score / 5 * XuanShangPaiPanDing(p - 1);
                    }
                    if (s <= 8)
                    {
                        shouPaiDian[i] += naos[(int)XingGe.XUAN_SHANG].score / 5 * XuanShangPaiPanDing(p + 1);
                    }
                    if (s >= 3)
                    {
                        shouPaiDian[i] += naos[(int)XingGe.XUAN_SHANG].score / 10 * XuanShangPaiPanDing(p - 2);
                    }
                    if (s <= 7)
                    {
                        shouPaiDian[i] += naos[(int)XingGe.XUAN_SHANG].score / 10 * XuanShangPaiPanDing(p + 2);
                    }
                }
                // 役牌・風牌
                int yiPai = YiPaiPanDing(p);
                if (yiPai > 0)
                {
                    shouPaiDian[i] += naos[(int)XingGe.YI_PAI].score / 15 * YiPaiPanDing(p);
                    if (p >= 0x35 && p <= 0x37)
                    {
                        shouPaiDian[i] += 1;
                    }
                }
                // 染め
                if ((16 - maxShu <= naos[(int)XingGe.RAN].score / 10) && ((p & SE_PAI) == se || (p & ZI_PAI) == ZI_PAI))
                {
                    shouPaiDian[i] += naos[(int)XingGe.RAN].score / 5;
                }
                // 字牌
                if ((p & ZI_PAI) == ZI_PAI)
                {
                    shouPaiDian[i] -= naos[(int)XingGe.YI_PAI].score / 10 * (gongKaiPaiShu[p] - shouPaiShu[p]);
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            shouPaiDianShu ??= new int[14];
            Array.Fill(shouPaiDianShu, 1);
            // 刻子
            for (int i = 0x01; i <= 0x37; i++)
            {
                if (shouPaiShu[i] >= 3)
                {
                    int k = 3;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i)
                        {
                            if (k > 0)
                            {
                                shouPaiDian[j] += naos[(int)XingGe.KE_ZI].score * 2;
                                shouPaiShu[i]--;
                                k--;
                            }
                            else
                            {
                                if (p >= ZI_PAI)
                                {
                                    continue;
                                }
                                int s = p & SHU_PAI;
                                if ((s >= 3 && shouPaiShu[i - 2] > 0 && shouPaiShu[i - 1] > 0) || (s <= 7 && shouPaiShu[i + 1] > 0 && shouPaiShu[i + 2] > 0) || (s >= 2 && s <= 8 && shouPaiShu[i - 1] > 0 && shouPaiShu[i + 1] > 0))
                                {
                                    shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score * 2;
                                }
                                if ((s >= 3 && shouPaiShu[i - 1] > 0) || (s <= 7 && shouPaiShu[i + 1] > 0))
                                {
                                    shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score * 2 / 3;
                                }
                                if ((s >= 3 && shouPaiShu[i - 2] > 0) || (s <= 7 && shouPaiShu[i + 2] > 0))
                                {
                                    shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score * 2 / 4;
                                }
                                if ((s == 1 && shouPaiShu[i + 1] > 0) || (s == 2 && shouPaiShu[i - 1] > 0) || (s == 8 && shouPaiShu[i + 1] > 0) || (s == 9 && shouPaiShu[i - 1] > 0))
                                {
                                    shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score * 2 / 5;
                                }
                            }
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
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && k > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.KE_ZI].score / 3;
                            shouPaiShu[i]--;
                            k--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Array.Fill(shouPaiDianShu, 1);
            // 順子(1-9)
            for (int i = 0x01; i <= 0x27; i++)
            {
                if (shouPaiShu[i] >= 1 && shouPaiShu[i + 1] >= 1 && shouPaiShu[i + 2] >= 1)
                {
                    int m = Math.Min(Math.Min(shouPaiShu[i], shouPaiShu[i + 1]), shouPaiShu[i + 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score;
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
                if (s == 1 || s >= 8)
                {
                    continue;
                }
                if (shouPaiShu[i] >= 1 && shouPaiShu[i + 1] >= 1)
                {
                    int m = Math.Min(shouPaiShu[i], shouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 3;
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
                    int m = Math.Min(shouPaiShu[i], shouPaiShu[i + 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 2 && s2 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 4;
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
                if ((s == 1 || s == 7) && shouPaiShu[i] >= 1 && shouPaiShu[i + 1] >= 1)
                {
                    int m = Math.Min(shouPaiShu[i], shouPaiShu[i + 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i + 1 && s1 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Array.Fill(shouPaiDianShu, 1);
            // 順子(9-1)
            for (int i = 0x29; i >= 0x03; i--)
            {
                if (shouPaiShu[i] >= 1 && shouPaiShu[i - 1] >= 1 && shouPaiShu[i - 2] >= 1)
                {
                    int m = Math.Min(Math.Min(shouPaiShu[i], shouPaiShu[i - 1]), shouPaiShu[i - 2]);
                    int s0 = m;
                    int s1 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score;
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
                if (s <= 2 || s == 9)
                {
                    continue;
                }
                if (shouPaiShu[i] >= 1 && shouPaiShu[i - 1] >= 1)
                {
                    int m = Math.Min(shouPaiShu[i], shouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 3;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 3;
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
                    int m = Math.Min(shouPaiShu[i], shouPaiShu[i - 2]);
                    int s0 = m;
                    int s2 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 4;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 2 && s2 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 4;
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
                if ((s == 2 || s == 9) && shouPaiShu[i] >= 1 && shouPaiShu[i - 1] >= 1)
                {
                    int m = Math.Min(shouPaiShu[i], shouPaiShu[i - 1]);
                    int s0 = m;
                    int s1 = m;
                    for (int j = 0; j < shouPai.Count; j++)
                    {
                        if (shouPaiDianShu[j] <= 0)
                        {
                            continue;
                        }
                        int p = shouPai[j] & QUE_PAI;
                        if (p == i && s0 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 5;
                            shouPaiDianShu[j]--;
                            s0--;
                            shouPaiShu[p]--;
                        }
                        if (p == i - 1 && s1 > 0)
                        {
                            shouPaiDian[j] += naos[(int)XingGe.SHUN_ZI].score / 5;
                            shouPaiDianShu[j]--;
                            s1--;
                            shouPaiShu[p]--;
                        }
                    }
                }
            }

            // 手牌数計算
            ShouPaiShuJiSuan();
            Array.Fill(shouPaiDianShu, 1);
            // 孤立牌への加減点
            for (int i = 0; i < shouPai.Count; i++)
            {
                int p = shouPai[i] & QUE_PAI;
                if (p >= ZI_PAI || shouPaiDian[i] > 0)
                {
                    continue;
                }
                int s = p & SHU_PAI;
                int dian = naos[(int)XingGe.SHUN_ZI].score / 25 * (5 - Math.Abs(s - 5));
                if (s <= 2 && shouPaiShu[p] > 0 && shouPaiShu[p + 1] == 0 && shouPaiShu[p + 2] == 0 && shouPaiShu[p + 3] > 0)
                {
                    shouPaiDian[i] -= dian;
                }
                if (s >= 8 && shouPaiShu[p] > 0 && shouPaiShu[p - 1] == 0 && shouPaiShu[p - 2] == 0 && shouPaiShu[p - 3] > 0)
                {
                    shouPaiDian[i] -= dian;
                }
                // 数牌
                if (s >= 2 && s <= 8)
                {
                    shouPaiDian[i] += dian;
                }
            }
        }

        public override IEnumerator SiKaoZiJiaCoroutine()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator SiKaoTaJiaCoroutine()
        {
            throw new NotImplementedException();
        }
    }
}

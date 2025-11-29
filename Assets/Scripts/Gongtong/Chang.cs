using System;
using System.Collections.Generic;
using Assets.Scripts.Maqiao;
using Assets.Scripts.Sikao;

namespace Assets.Scripts.Gongtong
{
    // 場
    public class Chang
    {
        // イベント
        public MaQiao.Event eventStatus = MaQiao.Event.KAI_SHI;
        // プレイヤー有り
        public bool existsPlayer = true;
        // 半荘数
        public int banZhuangShu = 0;
        // 連荘
        public MaQiao.Zhuang tingPaiLianZhuang;
        // キー状態
        public bool keyPress = false;
        // 描画フラグ
        public bool isKaiShiDraw;
        public bool isQiaoShiXuanZeDraw;
        public bool isFollowQiaoShiXuanZeDraw;
        public bool isQinJueDraw;
        public bool isPeiPaiDraw;
        public bool isDuiJuDraw;
        public bool isDuiJuZhongLeDraw;
        public bool isYiBiaoShiDraw;
        public bool isDianBiaoShiDraw;
        public bool isZhuangZhongLeDraw;

        public bool isZiJiaYaoDraw = false;
        public bool isTaJiaYaoDraw = false;
        public bool isDianChaDraw = false;

        public bool isBackDuiJuZhongLe = false;
        public bool isDianCha = false;
        public int yiBiaoShiFan;

        // 場風
        public int changFeng;
        // 局
        public int ju;
        // 本場
        public int benChang;
        // 親
        public int qin;
        // 起家
        public int qiaJia;
        // 捨牌
        public int shePai;
        // 自摸番
        public int ziMoFan;
        // 鳴番
        public int mingFan;
        // 栄和番
        public List<RongHeFan> rongHeFans = new();
        // 和了番
        public int heleFan;
        // 錯和番
        public int cuHeFan;
        // 供託
        public int gongTuo;
        // 自家思考結果
        public QiaoShi.YaoDingYi ziJiaYao;
        // 自家選択
        public int ziJiaXuanZe;
        // 他家思考結果
        public QiaoShi.YaoDingYi taJiaYao;
        // 他家選択
        public int taJiaXuanZe;

        // 風
        public int feng;
        // 立直数
        public int liZhiShu;
        // 四風子連打牌
        public List<int> siFengZiLianDaPai;
        // 四風子連打
        public bool siFengZiLianDa;
        // 九種九牌
        public bool jiuZhongJiuPai;

        // ランダム
        private readonly Random r = new();

        // 状態
        public void ZhuangTai(State state, bool isZiJia)
        {
            state.changFeng = changFeng;
            state.ju = ju;
            state.changShePai = isZiJia ? 0 : shePai;
        }

        // 切り上げ
        public int Ceil(int value, double p)
        {
            return (int)(Math.Ceiling(value / p) * p);
        }

        // シャッフル
        public void Shuffle(List<int> list, int num)
        {
            for (int i = 0; i < num; i++)
            {
                int n1 = r.Next(list.Count);
                int n2 = r.Next(list.Count);
                (list[n2], list[n1]) = (list[n1], list[n2]);
            }
        }

        // 荘初期化
        public void ZhuangChuQiHua()
        {
            benChang = 0;
            ju = 0;
            feng = 0;
            changFeng = Pai.FENG_PAI_DING_YI[feng];
            gongTuo = 0;
            foreach (QiaoShi shi in MaQiao.qiaoShis)
            {
                shi.lianZhuangShu = 0;
            }
        }

        // 局初期化
        public void JuChuQiHua()
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
            rongHeFans = new List<RongHeFan>();
        }

        // 立直処理
        public void LiZhiChuLi()
        {
            liZhiShu++;
            gongTuo += 1000;
        }

        // 四家立直判定
        public bool SiJiaLiZhiPanDing()
        {
            if (MaQiao.guiZe.siJiaLiZhiLianZhuang == 0)
            {
                return false;
            }
            if (MaQiao.qiaoShis.Count != 4)
            {
                return false;
            }
            if (liZhiShu >= MaQiao.qiaoShis.Count)
            {
                return true;
            }
            return false;
        }

        // 四風子連打牌処理
        public void SiFengZiLianDaChuLi(int shePai)
        {
            if (MaQiao.qiaoShis.Count != 4)
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

        // 四風子連打判定
        public bool SiFengZiLianDaPanDing()
        {
            return siFengZiLianDa && MaQiao.guiZe.siFengZiLianDaLianZhuang > 0;
        }

        // 九種九牌処理
        public void JiuZhongJiuPaiChuLi()
        {
            jiuZhongJiuPai = true;
        }

        // 連荘
        public void LianZhuang()
        {
            LianZhuangShuJiSuan();
            benChang++;
        }

        // 輪荘
        public void LunZhuang()
        {
            LianZhuangShuJiSuan();

            ju++;
            if (ju >= (4 - (4 - MaQiao.qiaoShis.Count)))
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
            qin %= MaQiao.qiaoShis.Count;
        }

        // 連荘数計算
        private void LianZhuangShuJiSuan()
        {
            for (int i = 0; i < MaQiao.qiaoShis.Count; i++)
            {
                QiaoShi shi = MaQiao.qiaoShis[i];
                bool isHeLe = false;
                foreach (RongHeFan rongHeFan in rongHeFans)
                {
                    if (i == rongHeFan.fan)
                    {
                        isHeLe = true;
                    }
                }
                if (isHeLe || heleFan == i)
                {
                    shi.lianZhuangShu++;
                    if (shi.lianZhuangShu > 7)
                    {
                        shi.lianZhuangShu = 0;
                    }
                }
                else
                {
                    shi.lianZhuangShu = 0;
                }
            }
        }

        // 点計算
        public void DianJiSuan()
        {
            // 和了点
            int dian;
            // 支払
            int zhiFu;
            // 受取
            int shouQu = 0;
            int cuHe = 1;
            for (int i = ziMoFan + 1; i < ziMoFan + MaQiao.qiaoShis.Count; i++)
            {
                QiaoShi shi = MaQiao.qiaoShis[i % MaQiao.qiaoShis.Count];
                if (shi.cuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ziJiaYao == QiaoShi.YaoDingYi.ZiMo || cuHe == -1)
            {
                // 自摸
                dian = MaQiao.qiaoShis[ziMoFan].heLeDian;
                // 記録 和了点
                MaQiao.qiaoShis[ziMoFan].jiLu.heLeDian += MaQiao.qiaoShis[ziMoFan].heLeDian;

                if (MaQiao.qiaoShis[ziMoFan].baoZeFan >= 0)
                {
                    // 包則
                    int baoZeFan = (ziMoFan + MaQiao.qiaoShis.Count - MaQiao.qiaoShis[ziMoFan].baoZeFan) % MaQiao.qiaoShis.Count;
                    shouQu = dian + benChang * 100 * cuHe;
                    MaQiao.qiaoShis[ziMoFan].DianBangJiSuan(shouQu);
                    MaQiao.qiaoShis[baoZeFan].DianBangJiSuan(-shouQu);
                    // 記録 親和了数
                    MaQiao.qiaoShis[ziMoFan].jiLu.qinHeLeShu++;
                }
                else if (ziMoFan == qin)
                {
                    // 親
                    zhiFu = Ceil(dian / (MaQiao.qiaoShis.Count - 1), 100);
                    // 本場
                    zhiFu += benChang * 100 * cuHe;
                    for (int i = ziMoFan + 1; i < ziMoFan + MaQiao.qiaoShis.Count; i++)
                    {
                        MaQiao.qiaoShis[i % MaQiao.qiaoShis.Count].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    MaQiao.qiaoShis[ziMoFan].DianBangJiSuan(shouQu);
                    // 記録 親和了数
                    MaQiao.qiaoShis[ziMoFan].jiLu.qinHeLeShu++;
                }
                else
                {
                    // 子
                    for (int i = ziMoFan + 1; i < ziMoFan + MaQiao.qiaoShis.Count; i++)
                    {
                        if ((i % MaQiao.qiaoShis.Count) == qin)
                        {
                            zhiFu = Ceil(dian / 2, 100);
                        }
                        else
                        {
                            zhiFu = Ceil(dian / 4, 100);
                        }
                        // 本場
                        zhiFu += benChang * 100 * cuHe;
                        if (MaQiao.qiaoShis.Count == 3)
                        {
                            // 3打ちの場合、北家分を折半
                            zhiFu += Ceil(Ceil(dian / 4, 100) / 2, 100);
                            zhiFu += benChang * 100 / 2 * cuHe;
                        }
                        else if (MaQiao.qiaoShis.Count == 2)
                        {
                            // 2打ちの場合、全て支払
                            zhiFu = dian;
                            zhiFu += benChang * 300 * cuHe;
                        }
                        MaQiao.qiaoShis[i % MaQiao.qiaoShis.Count].DianBangJiSuan(-zhiFu);
                        shouQu += zhiFu;
                    }
                    MaQiao.qiaoShis[ziMoFan].DianBangJiSuan(shouQu);
                }
                // 供託
                MaQiao.qiaoShis[ziMoFan].DianBangJiSuan(gongTuo);
                MaQiao.qiaoShis[ziMoFan].ShouQuGongTuoJiSuan(gongTuo);
                // 記録 和了数
                MaQiao.qiaoShis[ziMoFan].jiLu.heLeShu++;
                return;
            }
            else if (taJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                // 栄和
                for (int i = 0; i < rongHeFans.Count; i++)
                {
                    dian = MaQiao.qiaoShis[rongHeFans[i].fan].heLeDian;
                    // 本場
                    dian += benChang * 300;

                    if (MaQiao.qiaoShis[rongHeFans[i].fan].baoZeFan >= 0)
                    {
                        // 包則
                        int baoZeFan = (rongHeFans[i].fan + MaQiao.qiaoShis.Count - MaQiao.qiaoShis[rongHeFans[i].fan].baoZeFan) % MaQiao.qiaoShis.Count;
                        shouQu = Ceil(dian / 2, 100);
                        MaQiao.qiaoShis[ziMoFan].DianBangJiSuan(-(dian - shouQu));
                        MaQiao.qiaoShis[baoZeFan].DianBangJiSuan(-shouQu);
                    }
                    else
                    {
                        MaQiao.qiaoShis[ziMoFan].DianBangJiSuan(-dian);
                    }
                    MaQiao.qiaoShis[rongHeFans[i].fan].DianBangJiSuan(dian);
                    // 供託
                    if (i == 0)
                    {
                        MaQiao.qiaoShis[rongHeFans[i].fan].DianBangJiSuan(gongTuo);
                        MaQiao.qiaoShis[rongHeFans[i].fan].ShouQuGongTuoJiSuan(gongTuo);

                    }
                    // 記録 和了数
                    MaQiao.qiaoShis[rongHeFans[i].fan].jiLu.heLeShu++;
                    // 記録 放銃数
                    MaQiao.qiaoShis[ziMoFan].jiLu.fangChongShu++;
                    // 記録 和了点
                    MaQiao.qiaoShis[rongHeFans[i].fan].jiLu.heLeDian += MaQiao.qiaoShis[rongHeFans[i].fan].heLeDian;
                    // 記録 放銃点
                    MaQiao.qiaoShis[ziMoFan].jiLu.fangChongDian += MaQiao.qiaoShis[rongHeFans[i].fan].heLeDian;
                }
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
            for (int i = ziMoFan + 1; i < ziMoFan + MaQiao.qiaoShis.Count; i++)
            {
                QiaoShi shi = MaQiao.qiaoShis[i % MaQiao.qiaoShis.Count];
                shi.jiLu.liuJuShu++;
            }

            // 形式聴牌計算
            int xingTingShu = 0;
            foreach (QiaoShi shi in MaQiao.qiaoShis)
            {
                if (shi.xingTing)
                {
                    xingTingShu++;
                    // 記録 聴牌数
                    shi.jiLu.tingPaiShu++;
                }
                else
                {
                    // 記録 不聴数
                    shi.jiLu.buTingShu++;
                }
            }
            if (xingTingShu == MaQiao.qiaoShis.Count || xingTingShu == 0)
            {
                return;
            }
            foreach (QiaoShi shi in MaQiao.qiaoShis)
            {
                if (shi.xingTing)
                {
                    shi.DianBangJiSuan(3000 / xingTingShu);
                }
                else
                {
                    shi.DianBangJiSuan(-3000 / (MaQiao.qiaoShis.Count - xingTingShu));
                }
            }
        }

        // 供託計算
        public void DianGiSuanGongTuo()
        {
            int cuHe = 1;
            for (int i = ziMoFan + 1; i < ziMoFan + MaQiao.qiaoShis.Count; i++)
            {
                QiaoShi shi = MaQiao.qiaoShis[i % MaQiao.qiaoShis.Count];
                if (shi.cuHeSheng != "")
                {
                    cuHe = -1;
                }
            }
            if (ziJiaYao == QiaoShi.YaoDingYi.ZiMo || cuHe == -1 || taJiaYao == QiaoShi.YaoDingYi.RongHe)
            {
                gongTuo = 0;
            }
        }

        // 錯和
        public void CuHe(int jia)
        {
            if (jia == qin)
            {
                // 親
                MaQiao.qiaoShis[jia].DianBangJiSuan(-12000);
                for (int i = jia + 1; i < jia + MaQiao.qiaoShis.Count; i++)
                {
                    int taJia = i % MaQiao.qiaoShis.Count;
                    MaQiao.qiaoShis[taJia].DianBangJiSuan(12000 / (MaQiao.qiaoShis.Count - 1));
                }
            }
            else
            {
                // 子
                MaQiao.qiaoShis[jia].DianBangJiSuan(-8000);
                for (int i = jia + 1; i < jia + MaQiao.qiaoShis.Count; i++)
                {
                    int taJia = i % MaQiao.qiaoShis.Count;
                    int dian;
                    if (taJia == qin)
                    {
                        dian = 4000;
                    }
                    else
                    {
                        dian = 2000;
                    }
                    if (MaQiao.qiaoShis.Count == 3)
                    {
                        dian += 1000;
                    }
                    MaQiao.qiaoShis[taJia].DianBangJiSuan(dian);
                }
            }
            cuHeFan = jia;
        }

        // 箱判定
        public bool XiangPanDing()
        {
            foreach (QiaoShi shi in MaQiao.qiaoShis)
            {
                if (shi.dianBang <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        // 場変更
        private void ChangBianGeng()
        {
            feng++;
            if (feng >= Pai.FENG_PAI_DING_YI.Length)
            {
                feng = 0;
            }
            changFeng = Pai.FENG_PAI_DING_YI[feng];
        }
    }

    [Serializable]
    public class RongHeFan
    {
        public int fan;
        public int index;
    }
}

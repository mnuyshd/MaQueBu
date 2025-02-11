using System.Collections.Generic;
using UnityEngine.UI;

using Assets.Source.Sikao;

namespace Assets.Source.Gongtong
{
    // 牌
    internal class Pai
    {
        // 牌定義
        private static readonly int[] Pai4DingYi = new int[] {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        // 牌定義(3人打)
        private static readonly int[] Pai3DingYi = new int[] {
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        private static int[] qiaoPai = Pai4DingYi;
        internal static int[] QiaoPai
        {
            get { return qiaoPai; }
        }

        // 赤牌定義
        private static readonly int[] chiPaiDingYi = new int[] { 0x05, 0x15, 0x25 };
        internal static int[] ChiPaiDingYi
        {
            get { return chiPaiDingYi; }
        }
        // 幺九牌定義
        private static readonly int[] yaoJiuPaiDingYi = new int[] { 0x01, 0x09, 0x11, 0x19, 0x21, 0x29, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        internal static int[] YaoJiuPaiDingYi
        {
            get { return yaoJiuPaiDingYi; }
        }
        // 緑一色牌定義
        private static readonly int[] luYiSePaiDingYi = new int[] { 0x22, 0x23, 0x24, 0x26, 0x28, 0x36 };
        internal static int[] LuYiSePaiDingYi
        {
            get { return luYiSePaiDingYi; }
        }
        // 風牌定義
        private static readonly int[] fengPaiDingYi = new int[] { 0x31, 0x32, 0x33, 0x34 };
        internal static int[] FengPaiDingYi
        {
            get { return fengPaiDingYi; }
        }
        // 懸賞牌定義
        private static readonly int[] xuanShangPaiDingYi = new int[] {
            -1, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x01, -1, -1, -1, -1, -1, -1,
            -1, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x11, -1, -1, -1, -1, -1, -1,
            -1, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x21, -1, -1, -1, -1, -1, -1,
            -1, 0x32, 0x33, 0x34, 0x31, 0x36, 0x37, 0x35
        };
        internal static int[] XuanShangPaiDingYi
        {
            get { return xuanShangPaiDingYi; }
        }
        // 風牌名定義
        private static readonly string[] fengPaiMing = new string[] { "東", "南", "西", "北" };
        internal static string[] FengPaiMing
        {
            get { return fengPaiMing; }
        }

        // 山牌自摸位
        private static readonly int[] ShanPaiZiMoWei = new int[] { 0, 1, 2, 3, 16, 17, 18, 19, 32, 33, 34, 35, 48, 52 };

        // 懸賞牌
        private static List<int> xuanShangPai;
        internal static List<int> XuanShangPai
        {
            get { return xuanShangPai; }
        }
        internal static Button[] goXuanShangPai;
        // 裏懸賞牌
        private static List<int> liXuanShangPai;
        internal static List<int> LiXuanShangPai
        {
            get { return liXuanShangPai; }
        }
        internal static Button[] goLiXuanShangPai;

        // 山牌
        private static List<int> shanPai;
        // 嶺上牌
        private static List<int> lingShangPai;
        // 槍槓
        private static bool qiangGang;
        // 嶺上
        private static bool lingShangKaiHua;
        // 海底
        private static bool haiDi;

        // コンストラクタ
        static Pai()
        {
            goXuanShangPai = new Button[5];
            goLiXuanShangPai = new Button[5];
        }

        // 洗牌
        internal static void XiPai() {
            qiaoPai = Chang.QiaoShis.Count == 3 ? Pai3DingYi : Pai4DingYi;
            shanPai = new();
            // 山牌
            for (int i = 0; i < 4; i++)
            {
                foreach (int p in qiaoPai)
                {
                    shanPai.Add(p);
                }
            }
            // 赤牌
            for (int i = 0; i < ChiPaiDingYi.Length; i++)
            {
                int cp = ChiPaiDingYi[i];
                for (int j = 0; j < GuiZe.chiPaiShu[i]; j++)
                {
                    for (int k = 0; k < shanPai.Count; k++)
                    {
                        int p = shanPai[k];
                        if (cp == p)
                        {
                            shanPai[k] += QiaoShi.CHI_PAI;
                            break;
                        }
                    }
                }
            }
            Chang.Shuffle(shanPai, 30000);

        }

        // 洗牌嶺上
        internal static void XiPaiLingShang()
        {
            lingShangPai = new();
            xuanShangPai = new();
            liXuanShangPai = new();

            // 嶺上牌
            for (int i = 0; i < 14; i++)
            {
                lingShangPai.Add(shanPai[^1]);
                shanPai.RemoveAt(shanPai.Count - 1);
            }
            lingShangPai.Reverse();

            // 懸賞牌
            xuanShangPai.Add(lingShangPai[4]);
            liXuanShangPai.Add(lingShangPai[5]);

            lingShangKaiHua = false;
            qiangGang = false;
            haiDi = false;
        }

        // 積込
        internal static void JiRu(int jia, List<List<int>> jiRuPai)
        {
            List<int> shanPaiC = new(shanPai);

            for (int i = 0; i < jiRuPai.Count; i++)
            {
                List<int> pais = jiRuPai[i];
                for (int j = 0; j < pais.Count; j++)
                {
                    int pai = pais[j];
                    for (int k = 0; k < shanPaiC.Count; k++)
                    {
                        int sp = shanPaiC[k];
                        if (pai == sp)
                        {
                            int wei;
                            if (j < 12)
                            {
                                wei = ShanPaiZiMoWei[j] + ((jia + i) % Chang.QiaoShis.Count * 4);
                            }
                            else
                            {
                                wei = ShanPaiZiMoWei[j] + ((jia + i) % Chang.QiaoShis.Count);
                            }
                            int tmp = shanPai[wei];
                            shanPai[wei] = shanPai[k];
                            shanPai[k] = tmp;
                            shanPaiC[wei] = 0xff;
                            shanPaiC[k] = tmp;
                            break;
                        }
                    }
                }
            }
        }

        // 山牌自摸
        internal static int ShanPaiZiMo()
        {
            if (shanPai.Count == 1)
            {
                haiDi = true;
            }
            int p = shanPai[0];
            shanPai.RemoveAt(0);
            return p;
        }

        // 嶺上牌自摸
        internal static int LingShangPaiZiMo()
        {
            lingShangKaiHua = true;

            if (lingShangPai.Count == 0)
            {
                return 0;
            }
            lingShangPai.Add(shanPai[^1]);
            shanPai.RemoveAt(shanPai.Count - 1);
            int p = lingShangPai[0];
            lingShangPai.RemoveAt(0);
            return p;
        }

        // 海底判定
        internal static bool HaiDiPanDing()
        {
            return haiDi;
        }

        // 嶺上処理
        internal static void LingShanChuLi()
        {
            lingShangKaiHua = false;
        }

        // 嶺上判定
        internal static bool LingShanKaiHuaPanDing()
        {
            return lingShangKaiHua;
        }

        // 懸賞牌数
        internal static int XuanShangPaiShu()
        {
            return xuanShangPai.Count;
        }

        // 嶺上牌処理
        internal static void LingShangPaiChuLi()
        {
            // 懸賞牌
            xuanShangPai.Add(lingShangPai[4 + xuanShangPai.Count]);
            liXuanShangPai.Add(lingShangPai[5 + liXuanShangPai.Count]);
        }

        // 槍槓
        internal static void QiangGang()
        {
            qiangGang = true;
        }

        // 槍槓処理
        internal static void QiangGangChuLi()
        {
            qiangGang = false;
        }

        // 槍槓判定
        internal static bool QiangGangPanDing()
        {
            return qiangGang;
        }

        // 四開槓判定
        internal static bool SiKaiGangPanDing()
        {
            if (xuanShangPai.Count <= 4)
            {
                return false;
            }
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                int gang = 0;
                foreach ((_, _, QiaoShi.YaoDingYi yao) in shi.FuLuPai)
                {
                    if (yao == QiaoShi.YaoDingYi.AnGang || yao == QiaoShi.YaoDingYi.JiaGang || yao == QiaoShi.YaoDingYi.DaMingGang)
                    {
                        gang++;
                    }
                }
                if (gang > 0 && gang < 4)
                {
                    // 四開槓
                    return true;
                }
            }
            return false;
        }

        // 流局判定
        internal static bool LiuJuPanDing()
        {
            return shanPai.Count == 0;
        }

        // 残山牌数
        internal static int CanShanPaiShu()
        {
            return shanPai.Count;
        }

        // 残牌計算
        internal static int CanShu(int shu)
        {
            return 4 - shu;
        }
    }
}

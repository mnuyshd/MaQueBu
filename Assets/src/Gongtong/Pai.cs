using System.Collections.Generic;
using UnityEngine.UI;

using Sikao;

namespace Gongtong
{
    // 牌
    internal class Pai
    {
        // 牌定義
        private static int[] Pai4DingYi = new int[] {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        // 牌定義(3人打)
        private static readonly int[] Pai3DingYi = new int[] {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
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
        private static readonly int[] xuanShangPai;
        internal static int[] XuanShangPai
        {
            get { return xuanShangPai; }
        }
        internal static Button[] goXuanShangPai;
        // 裏懸賞牌
        private static readonly int[] liXuanShangPai;
        internal static int[] LiXuanShangPai
        {
            get { return liXuanShangPai; }
        }
        internal static Button[] goLiXuanShangPai;
        // 懸賞牌位
        private static int xuanShangPaiWei;
        internal static int XuanShangPaiWei
        {
            get { return xuanShangPaiWei; }
        }

        // 山牌
        private static int[] shanPai;
        // 山牌位
        private static int shanPaiWei;
        // 嶺上牌
        private static int[] lingShangPai;
        // 嶺上牌位
        private static int lingShangPaiWei;
        // 槓家
        private static int[] gangJia;
        // 槍槓
        private static bool qiangGang;
        // 嶺上
        private static bool lingShangKaiHua;
        // 海底
        private static bool haiDi;

        // コンストラクタ
        static Pai()
        {
            xuanShangPai = new int[5];
            goXuanShangPai = new Button[xuanShangPai.Length];
            liXuanShangPai = new int[5];
            goLiXuanShangPai = new Button[liXuanShangPai.Length];
            lingShangPai = new int[18];
            gangJia = new int[5];
        }

        // 洗牌
        internal static void XiPai() {
            if (Chang.MianZi == 3)
            {
                // 3人打ちの場合
                qiaoPai = Pai3DingYi;
            }
            else
            {
                qiaoPai = Pai4DingYi;
            }
            shanPai = new int[4 * qiaoPai.Length];
            // 山牌
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < qiaoPai.Length; j++)
                {
                    shanPai[i * qiaoPai.Length + j] = qiaoPai[j];
                }
            }
            // 赤牌
            for (int i = 0; i < ChiPaiDingYi.Length; i++)
            {
                for (int j = 0; j < GuiZe.chiPaiShu[i]; j++)
                {
                    for (int k = 0; k < shanPai.Length; k++)
                    {
                        if (ChiPaiDingYi[i] == shanPai[k])
                        {
                            shanPai[k] += QiaoShi.CHI_PAI;
                            break;
                        }
                    }
                }
            }
            Chang.Shuffle(shanPai, 30000);
            shanPaiWei = 0;

        }

        // 洗牌嶺上
        internal static void XiPaiLingShang()
        {
            // 嶺上牌
            for (int i = 0; i < lingShangPai.Length; i++)
            {
                if (i < 14)
                {
                    int w = shanPai.Length - 1 - i;
                    lingShangPai[i] = shanPai[w];
                    shanPai[w] = 0xff;
                }
                else
                {
                    lingShangPai[i] = 0xff;
                }
            }
            lingShangPaiWei = 0;

            // 懸賞牌
            int wei = 0;
            for (int i = 4; i <= 12; i += 2)
            {
                xuanShangPai[wei] = lingShangPai[i];
                liXuanShangPai[wei] = lingShangPai[i + 1];
                wei++;
            }
            xuanShangPaiWei = 1;
            Chang.Init(gangJia, 0xff);

            lingShangKaiHua = false;
            qiangGang = false;
            haiDi = false;
        }

        // 積込
        internal static void JiRu(int jia, List<List<int>> jiRuPai)
        {
            int[] shanPaiC = new int[shanPai.Length];
            Chang.Copy(shanPai, shanPaiC);

            for (int i = 0; i < jiRuPai.Count; i++)
            {
                for (int j = 0; j < jiRuPai[i].Count; j++)
                {
                    for (int k = 0; k < shanPaiC.Length; k++)
                    {
                        if (jiRuPai[i][j] == shanPaiC[k])
                        {
                            int wei;
                            if (j < 12)
                            {
                                wei = ShanPaiZiMoWei[j] + ((jia + i) % Chang.MianZi * 4);
                            }
                            else
                            {
                                wei = ShanPaiZiMoWei[j] + ((jia + i) % Chang.MianZi);
                            }
                            int p = shanPai[wei];
                            shanPai[wei] = shanPai[k];
                            shanPai[k] = p;
                            shanPaiC[wei] = 0xff;
                            shanPaiC[k] = p;
                            break;
                        }
                    }
                }
            }
        }

        // 山牌自摸
        internal static int ShanPaiZiMo()
        {
            if (shanPai[shanPaiWei + 1] == 0xff)
            {
                haiDi = true;
            }
            return shanPai[shanPaiWei++];
        }

        // 嶺上牌自摸
        internal static int LingShangPaiZiMo()
        {
            lingShangKaiHua = true;

            for (int i = shanPai.Length - 1; i >= 0; i--)
            {
                if (shanPai[i] != 0xff)
                {
                    for (int j = 0; j < lingShangPai.Length; j++)
                    {
                        if (lingShangPai[j] == 0xff)
                        {
                            lingShangPai[j] = shanPai[i];
                            shanPai[i] = 0xff;
                            return lingShangPai[lingShangPaiWei++];
                        }
                    }
                }
            }

            return 0;
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
            return xuanShangPaiWei;
        }

        // 懸賞牌取得
        internal static int[] XuanShangPaiQuDe()
        {
            int[] xuan = new int[xuanShangPaiWei];
            for (int i = 0; i < xuanShangPaiWei; i++)
            {
                xuan[i] = xuanShangPai[i];
            }
            return xuan;
        }

        // 裏懸賞牌取得
        internal static int[] LiXuanShangPaiQuDe()
        {
            int[] xuan = new int[xuanShangPaiWei];
            for (int i = 0; i < xuanShangPaiWei; i++)
            {
                xuan[i] = liXuanShangPai[i];
            }
            return xuan;
        }

        // 嶺上牌処理
        internal static void LingShangPaiChuLi(int jia)
        {
            gangJia[xuanShangPaiWei - 1] = jia;
            xuanShangPaiWei++;
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
            if (xuanShangPaiWei > 4)
            {
                for (int i = 0; i < gangJia.Length - 1; i++)
                {
                    if (gangJia[i] == 0xff)
                    {
                        break;
                    }
                    for (int j = 0; j < gangJia.Length; j++)
                    {
                        if (gangJia[j] == 0xff)
                        {
                            break;
                        }
                        if (gangJia[i] != gangJia[j])
                        {
                            // 四開槓
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // 流局判定
        internal static bool LiuJuPanDing()
        {
            if (shanPai[shanPaiWei] == 0xff)
            {
                return true;
            }
            return false;
        }

        // 残山牌数
        internal static int CanShanPaiShu()
        {
            if (shanPai == null)
            {
                return 0;
            }
            int shu = 0;
            for (int i = shanPaiWei; i < shanPai.Length; i++)
            {
                if (shanPai[i] == 0xff)
                {
                    break;
                }
                shu++;
            }
            return shu;
        }

        // 残牌計算
        internal static int CanShu(int shu)
        {
            return 4 - shu;
        }
    }
}

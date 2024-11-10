using System.Collections.Generic;
using UnityEngine.UI;

using Sikao;

namespace Gongtong
{
    // 牌
    internal class Pai
    {
        // 牌
        internal static int[] PAI4 = new int[] {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        internal static readonly int[] PAI3 = new int[] {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        internal static int[] PAI = PAI4;
        // 赤牌
        internal static readonly int[] CHI_PAI = new int[] { 0x05, 0x15, 0x25 };
        // 幺九牌
        internal static readonly int[] YAO_JIU_PAI = new int[] { 0x01, 0x09, 0x11, 0x19, 0x21, 0x29, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        // 緑一色牌
        internal static readonly int[] LU_YI_SE_PAI = new int[] { 0x22, 0x23, 0x24, 0x26, 0x28, 0x36 };
        // 風牌
        internal static readonly int[] FENG_PAI = new int[] { 0x31, 0x32, 0x33, 0x34 };

        // 懸賞牌
        internal static readonly int[] XUAN_SHANG_PAI = new int[] {
            -1, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x01, -1, -1, -1, -1, -1, -1,
            -1, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x11, -1, -1, -1, -1, -1, -1,
            -1, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x21, -1, -1, -1, -1, -1, -1,
            -1, 0x32, 0x33, 0x34, 0x31, 0x36, 0x37, 0x35
        };
        // 風牌名
        internal static readonly string[] FENG_PAI_MING = new string[] { "東", "南", "西", "北" };

        // 山牌自摸位
        private static readonly int[] SHAN_PAI_ZI_MO_WEI = new int[] { 0, 1, 2, 3, 16, 17, 18, 19, 32, 33, 34, 35, 48, 52 };
        // 懸賞牌
        internal static int[] xuanShangPai;
        internal static Button[] goXuanShangPai;
        // 裏懸賞牌
        internal static int[] liXuanShangPai;
        internal static Button[] goLiXuanShangPai;
        // 懸賞牌位
        internal static int xuanShangPaiWei;
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
            if (Chang.mianZi == 3)
            {
                // 3人打ちの場合、萬子を抜く
                PAI = PAI3;
            }
            else
            {
                PAI = PAI4;
            }
            shanPai = new int[4 * PAI.Length];
            // 山牌
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < PAI.Length; j++)
                {
                    shanPai[i * PAI.Length + j] = PAI[j];
                }
            }
            // 赤牌
            for (int i = 0; i < CHI_PAI.Length; i++)
            {
                for (int j = 0; j < GuiZe.chiPaiShu[i]; j++)
                {
                    for (int k = 0; k < shanPai.Length; k++)
                    {
                        if (CHI_PAI[i] == shanPai[k])
                        {
                            shanPai[k] += 0x40;
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
            QiaoShi.Init(gangJia, 0xff);

            lingShangKaiHua = false;
            qiangGang = false;
            haiDi = false;
        }

        // 積込
        internal static void JiRu(int jia, List<List<int>> jiRuPai)
        {
            int[] shanPaiC = new int[shanPai.Length];
            QiaoShi.Copy(shanPai, shanPaiC);

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
                                wei = SHAN_PAI_ZI_MO_WEI[j] + ((jia + i) % Chang.mianZi * 4);
                            }
                            else
                            {
                                wei = SHAN_PAI_ZI_MO_WEI[j] + ((jia + i) % Chang.mianZi);
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
            for (int i = 0; i < Chang.mianZi; i++)
            {
                QiaoShi shi = Chang.qiaoShi[i];
                if (shi.player)
                {
                    shi.ShouPaiXuanShangPanDing();
                }
            }
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
    }
}

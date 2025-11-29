using System.Collections.Generic;
using UnityEngine.UI;

using Assets.Scripts.Sikao;
using Assets.Scripts.Maqiao;

namespace Assets.Scripts.Gongtong
{
    // 牌
    public class Pai
    {
        // 牌定義
        private static readonly int[] PAI4_DING_YI = new int[] {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        // 牌定義(3人打)
        private static readonly int[] PAI3_DING_YI = new int[] {
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };
        public int[] qiaoPai = PAI4_DING_YI;

        // 赤牌定義
        public static int[] CHI_PAI_DING_YI = new int[] { 0x05, 0x15, 0x25 };
        // 幺九牌定義
        public static int[] YAO_JIU_PAI_DING_YI = new int[] { 0x01, 0x09, 0x11, 0x19, 0x21, 0x29, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37 };
        // 緑一色牌定義
        public static int[] LU_YI_SE_PAI_DING_YI = new int[] { 0x22, 0x23, 0x24, 0x26, 0x28, 0x36 };
        // 紅孔雀牌定義
        public static int[] GONG_KONG_QIAO_PAI_DING_YI = new int[] { 0x21, 0x25, 0x27, 0x29, 0x37 };
        // 風牌定義
        public static int[] FENG_PAI_DING_YI = new int[] { 0x31, 0x32, 0x33, 0x34 };
        // 懸賞牌定義
        public static int[] XUAN_SHANG_PAI_DING_YI = new int[] {
            -1, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x01, -1, -1, -1, -1, -1, -1,
            -1, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x11, -1, -1, -1, -1, -1, -1,
            -1, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x21, -1, -1, -1, -1, -1, -1,
            -1, 0x32, 0x33, 0x34, 0x31, 0x36, 0x37, 0x35
        };
        // 風牌名定義
        public static string[] FENG_PAI_MING = new string[] { "東", "南", "西", "北" };

        // 山牌自摸位
        private static readonly int[] SHAN_PAI_ZI_MO_WEI = new int[] { 0, 1, 2, 3, 16, 17, 18, 19, 32, 33, 34, 35, 48, 52 };

        // 懸賞牌
        public List<int> xuanShangPai;
        public Button[] goXuanShangPai;
        // 裏懸賞牌
        public List<int> liXuanShangPai;
        public Button[] goLiXuanShangPai;

        // 山牌
        public List<int> shanPai;
        // 嶺上牌
        public List<int> lingShangPai;
        // 槍槓
        public bool qiangGang;
        // 嶺上
        public bool lingShangKaiHua;
        // 海底
        public bool haiDi;

        // コンストラクタ
        public Pai()
        {
            goXuanShangPai = new Button[5];
            goLiXuanShangPai = new Button[5];
        }

        // 状態
        public void ZhuangTai(State state)
        {
            state.xuanShangPai = new();
            for (int i = 0; i <= 4; i++)
            {
                state.xuanShangPai.Add(i < xuanShangPai.Count ? xuanShangPai[i] : 0);
            }
        }

        // 洗牌
        public void XiPai()
        {
            qiaoPai = MaQiao.qiaoShis.Count == 3 ? PAI3_DING_YI : PAI4_DING_YI;
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
            for (int i = 0; i < CHI_PAI_DING_YI.Length; i++)
            {
                int cp = CHI_PAI_DING_YI[i];
                for (int j = 0; j < MaQiao.guiZe.chiPaiShu[i]; j++)
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
            MaQiao.chang.Shuffle(shanPai, 30000);

        }

        // 洗牌嶺上
        public void XiPaiLingShang()
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
        public void JiRu(int jia, List<List<int>> jiRuPai)
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
                                wei = SHAN_PAI_ZI_MO_WEI[j] + ((jia + i) % MaQiao.qiaoShis.Count * 4);
                            }
                            else
                            {
                                wei = SHAN_PAI_ZI_MO_WEI[j] + ((jia + i) % MaQiao.qiaoShis.Count);
                            }
                            (shanPai[k], shanPai[wei]) = (shanPai[wei], shanPai[k]);
                            shanPaiC[wei] = 0xff;
                            shanPaiC[k] = shanPai[k];
                            break;
                        }
                    }
                }
            }
        }

        // 山牌自摸
        public int ShanPaiZiMo()
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
        public int LingShangPaiZiMo()
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
        public bool HaiDiPanDing()
        {
            return haiDi;
        }

        // 嶺上処理
        public void LingShanChuLi()
        {
            lingShangKaiHua = false;
        }

        // 嶺上判定
        public bool LingShanKaiHuaPanDing()
        {
            return lingShangKaiHua;
        }

        // 懸賞牌数
        public int XuanShangPaiShu()
        {
            return xuanShangPai.Count;
        }

        // 嶺上牌処理
        public void LingShangPaiChuLi()
        {
            // 懸賞牌
            xuanShangPai.Add(lingShangPai[4 + xuanShangPai.Count]);
            liXuanShangPai.Add(lingShangPai[5 + liXuanShangPai.Count]);
        }

        // 槍槓
        public void QiangGang()
        {
            qiangGang = true;
        }

        // 槍槓処理
        public void QiangGangChuLi()
        {
            qiangGang = false;
        }

        // 槍槓判定
        public bool QiangGangPanDing()
        {
            return qiangGang;
        }

        // 四開槓判定
        public bool SiKaiGangPanDing()
        {
            if (xuanShangPai.Count <= 4)
            {
                return false;
            }
            foreach (QiaoShi shi in MaQiao.qiaoShis)
            {
                int gang = 0;
                foreach (FuLuPai fuLuPai in shi.fuLuPais)
                {
                    if (fuLuPai.yao == QiaoShi.YaoDingYi.AnGang || fuLuPai.yao == QiaoShi.YaoDingYi.JiaGang || fuLuPai.yao == QiaoShi.YaoDingYi.DaMingGang)
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
        public bool LiuJuPanDing()
        {
            return shanPai.Count == 0;
        }

        // 残山牌数
        public int CanShanPaiShu()
        {
            return shanPai.Count;
        }

        // 残牌計算
        public int CanShu(int shu)
        {
            return 4 - shu;
        }
    }
}

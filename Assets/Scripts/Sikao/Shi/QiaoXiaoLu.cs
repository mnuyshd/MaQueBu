using Assets.Scripts.Gongtong;

namespace Assets.Scripts.Sikao.Shi
{
    // 効率雀士
    internal class QiaoXiaoLu : QiaoJiXie
    {
        internal const string MING_QIAN = "効率雀士";
        internal QiaoXiaoLu() : base(MING_QIAN)
        {
            nao = new()
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
        }

        // 思考自家
        internal override void SiKaoZiJia()
        {
            int tingPaiShu = 0;
            foreach (QiaoShi shi in Chang.QiaoShis)
            {
                if (shi.Player)
                {
                    continue;
                }
                if (shi.LiZhi || shi.FuLuPai.Count >= 3 || (Pai.CanShanPaiShu() <= XiangTingShu * 4))
                {
                    tingPaiShu++;
                }
            }
            if (tingPaiShu == 0)
            {
                // 有効牌数計算
                YouXiaoPaiShuJiSuan();
                for (int i = 0; i < ShouPai.Count; i++)
                {
                    ShouPaiDian[i] -= YouXiaoPaiShu[i] * 10;
                }
            }

            base.SiKaoZiJia();
        }
    }
}

using Assets.Scripts.Gongtong;

namespace Assets.Scripts.Sikao.Shi
{
    // 効率雀士
    public class QueXiaoLu : QueJiXie
    {
        public const string MING_QIAN = "効率雀士";
        public QueXiaoLu() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 50;
            naos[(int)XingGe.YI_PAI].score = 50;
            naos[(int)XingGe.SHUN_ZI].score = 50;
            naos[(int)XingGe.KE_ZI].score = 50;
            naos[(int)XingGe.LI_ZHI].score = 50;
            naos[(int)XingGe.MING].score = 50;
            naos[(int)XingGe.RAN].score = 50;
            naos[(int)XingGe.TAO].score = 50;
        }

        // 思考自家
        public override void SiKaoZiJia()
        {
            int tingPaiShu = 0;
            foreach (QueShi shi in MaQue.Instance.queShis)
            {
                if (shi.player)
                {
                    continue;
                }
                if (shi.liZhi || shi.fuLuPais.Count >= 3 || (Pai.Instance.CanShanPaiShu() <= xiangTingShu * 4))
                {
                    tingPaiShu++;
                }
            }
            if (tingPaiShu == 0)
            {
                // 有効牌数計算
                YouXiaoPaiShuJiSuan();
                for (int i = 0; i < shouPai.Count; i++)
                {
                    shouPaiDian[i] -= youXiaoPaiShu[i] * 10;
                }
            }

            base.SiKaoZiJia();
        }
    }
}

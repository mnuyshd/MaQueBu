namespace Assets.Scripts.Sikao.Shi
{
    public class KouzuNaruto : QiaoJiXie
    {
        public const string MING_QIAN = "河津鳴人";
        public KouzuNaruto() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 50;
            naos[(int)XingGe.YI_PAI].score = 80;
            naos[(int)XingGe.SHUN_ZI].score = 10;
            naos[(int)XingGe.KE_ZI].score = 100;
            naos[(int)XingGe.LI_ZHI].score = 50;
            naos[(int)XingGe.MING].score = 80;
            naos[(int)XingGe.RAN].score = 10;
            naos[(int)XingGe.TAO].score = 40;
        }
    }
}

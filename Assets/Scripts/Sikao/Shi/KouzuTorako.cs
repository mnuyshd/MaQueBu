namespace Assets.Scripts.Sikao.Shi
{
    public class KouzuTorako : QiaoJiXie
    {
        public const string MING_QIAN = "河津虎子";
        public KouzuTorako() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 100;
            naos[(int)XingGe.YI_PAI].score = 80;
            naos[(int)XingGe.SHUN_ZI].score = 10;
            naos[(int)XingGe.KE_ZI].score = 100;
            naos[(int)XingGe.LI_ZHI].score = 70;
            naos[(int)XingGe.MING].score = 0;
            naos[(int)XingGe.RAN].score = 10;
            naos[(int)XingGe.TAO].score = 40;
        }
    }
}

namespace Assets.Scripts.Sikao.Shi
{
    public class HikitaMamoru : QiaoJiXie
    {
        public const string MING_QIAN = "引田守";
        public HikitaMamoru() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 10;
            naos[(int)XingGe.YI_PAI].score = 60;
            naos[(int)XingGe.SHUN_ZI].score = 50;
            naos[(int)XingGe.KE_ZI].score = 50;
            naos[(int)XingGe.LI_ZHI].score = 0;
            naos[(int)XingGe.MING].score = 0;
            naos[(int)XingGe.RAN].score = 0;
            naos[(int)XingGe.TAO].score = 100;
        }
    }
}

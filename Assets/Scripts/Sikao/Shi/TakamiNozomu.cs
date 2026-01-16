namespace Assets.Scripts.Sikao.Shi
{
    public class TakamiNozomu : QueJiXie
    {
        public const string MING_QIAN = "高見望";
        public TakamiNozomu() : base(MING_QIAN)
        {
            naos[(int)XingGe.XUAN_SHANG].score = 70;
            naos[(int)XingGe.YI_PAI].score = 70;
            naos[(int)XingGe.SHUN_ZI].score = 50;
            naos[(int)XingGe.KE_ZI].score = 50;
            naos[(int)XingGe.LI_ZHI].score = 90;
            naos[(int)XingGe.MING].score = 30;
            naos[(int)XingGe.RAN].score = 40;
            naos[(int)XingGe.TAO].score = 50;
        }
    }
}

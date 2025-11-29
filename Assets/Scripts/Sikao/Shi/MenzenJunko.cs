namespace Assets.Scripts.Sikao.Shi
{
    public class MenzenJunko : QiaoJiXie
    {
        public const string MING_QIAN = "面前順子";
        public MenzenJunko() : base(MING_QIAN)
        {
            liPaiDongZuo = false;

            naos[(int)XingGe.XUAN_SHANG].score = 50;
            naos[(int)XingGe.YI_PAI].score = 50;
            naos[(int)XingGe.SHUN_ZI].score = 70;
            naos[(int)XingGe.KE_ZI].score = 30;
            naos[(int)XingGe.LI_ZHI].score = 10;
            naos[(int)XingGe.MING].score = 10;
            naos[(int)XingGe.RAN].score = 20;
            naos[(int)XingGe.TAO].score = 80;
        }
    }
}

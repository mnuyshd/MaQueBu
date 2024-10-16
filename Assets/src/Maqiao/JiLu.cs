namespace Maqiao
{
    // 記録
    public class JiLu
    {
        // 半荘数
        public int banZhuangShu = 0;
        // 対局数
        public int duiJuShu = 0;

        // 集計点
        public int jiJiDian = 0;
        // 順位１
        public int shunWei1 = 0;
        // 順位２
        public int shunWei2 = 0;
        // 順位３
        public int shunWei3 = 0;
        // 順位４
        public int shunWei4 = 0;

        // 和了数
        public int heLeShu = 0;
        // 親和了数
        public int qinHeLeShu = 0;
        // 放銃数
        public int fangChongShu = 0;

        // 流局数
        public int liuJuShu = 0;
        // 聴牌数
        public int tingPaiShu = 0;
        // 不聴数
        public int buTingShu = 0;

        // 和了点
        public int heLeDian = 0;
        // 放銃点
        public int fangChongDian = 0;

        // 役満数
        public int[] yiManShu = new int[Sikao.QiaoShi.YI_MAN_MING.Length];
        // 役数
        public int[] yiShu = new int[Sikao.QiaoShi.YI_MING.Length];
    }
}

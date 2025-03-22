namespace Assets.Source.Gongtong
{
    // 規則
    public class GuiZe
    {
        // 喰断
        public bool shiDuan = true;
        // 自摸平和
        public bool ziMoPingHe = true;
        // 食い替え
        public bool shiTi = false;
        // 箱(ドボン)
        public bool xiang = true;
        // 赤牌数
        public int[] chiPaiShu = new int[] { 1, 1, 1 };
        // W栄和(falseの場合、頭ハネ)
        public bool wRongHe = true;
        // 借金立直
        public bool jieJinLiZhi = false;
        // 南場不聴連荘
        public bool nanChangBuTingLianZhuang = false;
        // 開始点
        public int kaiShiDian = 25000;
        // 返し点
        public int fanDian = 30000;
        // 九種九牌 連荘
        public bool jiuZhongJiuPaiLianZhuang = true;
        // 四家立直 連荘
        public bool siJiaLiZhiLianZhuang = true;
        // 四風子連打 連荘
        public bool siFengZiLianDaLianZhuang = true;
        // 四開槓 連荘
        public bool siKaiGangLianZhuang = false;
        // 包則
        public bool baoZe = true;
        // 流し満貫
        public bool liuManGuan = false;
        // 三連刻
        public bool sanLianKe = false;
        // 燕返し
        public bool yanFan = false;
        // 開立直
        public bool kaiLiZhi = false;
        // 十三不塔
        public bool shiSanBuTa = false;
        // 八連荘
        public bool baLianZhuang = false;
        // ローカル役満
        public bool localYiMan = false;
    }
}

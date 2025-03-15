namespace Assets.Source.Maqiao
{
    // 設定
    public class SheDing
    {
        // 打牌方法
        public enum DaPaiFangFa
        {
            // 選択して打牌
            SELECT = 0,
            // １タップ打牌
            TAP_1 = 1,
            // ２タップ打牌
            TAP_2 = 2,
        }

        // 鳴無し
        public bool mingWu = false;
        // 立直後自動
        public bool liZhiAuto = true;
        // 打牌方法
        public DaPaiFangFa daPaiFangFa = DaPaiFangFa.TAP_2;
        // ドラマーク表示
        public bool xuanShangYin = true;
        // ツモ切表示有り
        public bool ziMoQieBiaoShi = true;
        // 待ち牌表示有り
        public bool daiPaiBiaoShi = true;
        // 向聴数表示有り
        public bool xiangTingShuBiaoShi = true;
        // 鳴パスはボタン
        public bool mingQuXiao = true;
        // デバッグ表示無し
        public bool debugDisplay = false;
    }
}
using System.Collections.Generic;

namespace Assets.Source.Sikao
{
    // 遷移
    public class Transition
    {
        // 状態
        public List<int> state;
        // 行動
        public List<int> action;
        // 次状態
        public List<int> nextState;
        // 報酬
        public double reward;
        // 向聴数
        public int xiangTingShu;
        // 立直
        public bool liZhi;
    }
}

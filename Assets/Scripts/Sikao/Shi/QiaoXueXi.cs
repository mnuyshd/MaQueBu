using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.Sikao.Shi
{
    // 学習雀士
    public class QiaoXueXi : QiaoJiXie
    {
#if DEV
        private const string HOST_URL = "http://127.0.0.1:10000/";
#else
        private const string HOST_URL = "https://maquiaobu-api.onrender.com/";
#endif
        public const string MING_QIAN = "学習雀士";
        public QiaoXueXi() : base(MING_QIAN)
        {
            waiBuSikao = true;
        }

        // 思考自家
        public override IEnumerator SiKaoZiJiaCoroutine()
        {
            yield return RequestSiKao(true, transitionZiJia.state);
        }

        // 思考他家
        public override IEnumerator SiKaoTaJiaCoroutine()
        {
            if (transitionTaJia == null)
            {
                taJiaYao = YaoDingYi.Wu;
                taJiaXuanZe = 0;
                yield break;
            }
            yield return RequestSiKao(false, transitionTaJia.state);
        }

        // 思考リクエスト
        private IEnumerator RequestSiKao(bool isZiJia, State state)
        {
            asyncStop = true;
            string json = JsonConvert.SerializeObject(state);
            byte[] raw = Encoding.UTF8.GetBytes(json);
            UnityWebRequest request = new(HOST_URL + (isZiJia ? "SiKaoZiJia" : "SiKaoTaJia"), "POST")
            {
                uploadHandler = new UploadHandlerRaw(raw),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ActionResponse response = JsonUtility.FromJson<ActionResponse>(request.downloadHandler.text);
                YaoDingYi yao = (YaoDingYi)response.action[0];
                int paiOrIndex = response.action[1];
                Debug.Log("action: yao=" + yao + " paiOrIndex=0x" + paiOrIndex.ToString("x2"));

                if (isZiJia)
                {
                    SiKaoZiJiaResponse(yao, paiOrIndex);
                }
                else
                {
                    SiKaoTaJiaResponse(yao, paiOrIndex);
                }
            }
            else
            {
                Debug.Log("リクエスト失敗:" + request.error);
                if (isZiJia)
                {
                    SiKaoZiJia();
                }
                else
                {
                    SiKaoTaJia();
                }
            }

            asyncStop = false;
        }

        private void SiKaoZiJiaResponse(YaoDingYi yao, int paiOrIndex)
        {
            if ((yao == YaoDingYi.ZiMo && !heLe)
                || (yao == YaoDingYi.LiZhi && liZhiPaiWei.Count == 0)
                || (yao == YaoDingYi.AnGang && anGangPaiWei.Count == 0)
                || (yao == YaoDingYi.JiaGang && jiaGangPaiWei.Count == 0))
            {
                Debug.Log("SikaoZiJia エラー");
                ziJiaYao = YaoDingYi.Wu;
                ziJiaXuanZe = shouPai.Count - 1;
                return;
            }

            // 和了判定
            if (heLe)
            {
                // 自摸
                ziJiaYao = YaoDingYi.ZiMo;
                ziJiaXuanZe = shouPai.Count - 1;
                return;
            }

            if (liZhi)
            {
                if (yao == YaoDingYi.AnGang)
                {
                    // 立直後暗槓
                    ziJiaYao = yao;
                    ziJiaXuanZe = paiOrIndex;
                    return;
                }
                // 立直後自摸切
                ziJiaYao = YaoDingYi.Wu;
                ziJiaXuanZe = shouPai.Count - 1;
                return;
            }

            ziJiaYao = yao;
            if (yao == YaoDingYi.Wu)
            {
                ziJiaXuanZe = shouPai.Count - 1;
                for (int i = 0; i < shouPai.Count; i++)
                {
                    if ((shouPai[i] & QIAO_PAI) == paiOrIndex)
                    {
                        ziJiaXuanZe = PaiXuanZe(i);
                        break;
                    }
                }
            }
            else
            {
                ziJiaXuanZe = paiOrIndex;
            }
        }

        private void SiKaoTaJiaResponse(YaoDingYi yao, int pai)
        {
            if ((yao == YaoDingYi.DaMingGang && daMingGangPaiWei.Count == 0)
                || (yao == YaoDingYi.Bing && bingPaiWei.Count == 0)
                || (yao == YaoDingYi.Chi && chiPaiWei.Count == 0))
            {
                Debug.Log("SikaoTaJia エラー");
                taJiaYao = YaoDingYi.Wu;
                taJiaXuanZe = 0;
                return;
            }

            // 和了判定
            if (heLe)
            {
                // 栄和
                taJiaYao = YaoDingYi.RongHe;
                taJiaXuanZe = 0;
                return;
            }

            taJiaYao = yao;
            taJiaXuanZe = pai;
        }
    }

    [Serializable]
    public class ActionResponse
    {
        public List<int> action;
    }
}
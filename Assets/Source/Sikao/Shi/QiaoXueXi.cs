using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Source.Sikao.Shi
{
    // 学習雀士
    internal class QiaoXueXi : QiaoJiXie
    {
        private const string HOST_URL = "http://localhost:5050/";
        internal const string MING_QIAN = "学習雀士";
        internal QiaoXueXi() : base(MING_QIAN)
        {
            WaiBuSikao = true;
        }

        private IEnumerator RequestSikao(string sikao, List<int> state)
        {
            AsyncStop = true;

            string json = JsonConvert.SerializeObject(state);
            UnityWebRequest request = new(HOST_URL + sikao, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ActionResponse response = JsonUtility.FromJson<ActionResponse>(request.downloadHandler.text);
                Debug.Log("(YaoDingYi)response.yao=" + (YaoDingYi)response.yao + " response.xuanZe=" + response.xuanZe);
                if (sikao == "SiKaoZiJia")
                {
                    ZiJiaYao = (YaoDingYi)response.yao;
                    ZiJiaXuanZe = response.xuanZe;
                }
                else
                {
                    TaJiaYao = (YaoDingYi)response.yao;
                    TaJiaXuanZe = response.xuanZe;
                }
            }
            else
            {
                // Debug.Log(request.error);
                if (sikao == "SiKaoZiJia")
                {
                    SiKaoZiJia();
                }
                else
                {
                    SiKaoTaJia();
                }
            }

            AsyncStop = false;
        }

        // 思考自家
        internal override IEnumerator SiKaoZiJiaCoroutine()
        {
            if (LiZhi)
            {
                // 立直後自摸切
                ZiJiaYao = YaoDingYi.Wu;
                ZiJiaXuanZe = ShouPai.Count - 1;
                yield break;
            }

            yield return RequestSikao("SiKaoZiJia", transitionZiJia.state);
        }

        // 思考他家
        internal override IEnumerator SiKaoTaJiaCoroutine()
        {
            yield return RequestSikao("SiKaoTaJia", null);//transitionTaJia.state);
        }
    }

    [Serializable]
    public class ActionResponse
    {
        public int yao;
        public int xuanZe;
    }
}
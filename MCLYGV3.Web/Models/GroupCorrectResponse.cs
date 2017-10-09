namespace MCLYGV3.Web.Models
{
    public class GroupCorrectResponse
    {
        public GroupCorrectResponse_Head Head { get; set; }

        public GroupCorrectResponse_Body Body { get; set; }
    }

    public class GroupCorrectResponse_Head
    {
        /// <summary>
        /// 返回内容的消息
        /// </summary>
        public string ResponseMessage { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public string requestType { get; set; }
        /// <summary>
        /// 返回类型代码
        /// </summary>
        public string ResponseCode { get; set; }


    }

    public class GroupCorrectResponse_Body
    {
        /// <summary>
        /// 批单号
        /// </summary>
        public string EndorNo { get; set; }
        /// <summary>
        /// 原始订单号
        /// </summary>
        public string FtrNo { get; set; }
        /// <summary>
        /// 保单号
        /// </summary>
        public string policyNo { get; set; }
        /// <summary>
        /// 批改生效日期
        /// </summary>
        public string validDate { get; set; }

        /// <summary>
        /// 返回支付的标记
        /// </summary>
        public string PayBackFlag { get; set; }
        /// <summary>
        /// 返回支付的标记
        /// </summary>
        public string PayMent { get; set; }
        /// <summary>
        /// 批改的订单号
        /// </summary>
        public string RandomNo { get; set; }


    }
}

namespace Listening.Admin.Api
{
    public class BaseResponse
    {
        public long Id { get; set; }
        private BaseResponse() { }
        public static BaseResponse Create(long id)
        {
            BaseResponse baseResponse = new BaseResponse();
            baseResponse.Id = id;
            return baseResponse;
        }
    }

}

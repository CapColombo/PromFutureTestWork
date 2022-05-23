namespace PromFutureTestWork.Models
{
    public interface IRequestHelper
    {
        public Task<TerminalSettings> GetSettings();
        public Task<ResponseModel> SendCommand(TerminalBindingModel model);
    }
}

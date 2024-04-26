using Microsoft.AspNetCore.Mvc;

namespace BusinessLogicLayer.Interfaces
{
    public interface INotificationService
    {
        public Task<IActionResult> EmailNotification(string ParEmp, string ParamTo, string ParamBcc, string ParamSubject, string ParamBody, string Attachments);
    }
}

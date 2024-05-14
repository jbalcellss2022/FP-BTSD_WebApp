using BusinessLogicLayer.Interfaces;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace BusinessLogicLayer.Services
{
    internal class NotificationService(IConfiguration Configuration) : INotificationService
    {
        //private readonly IConfiguration ctxConfiguration;
        //private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        void OnMessageSent(object sender, MessageSentEventArgs e)
        {
            //var responseCode = e.Response.Split(' ')[0];
            /*
            if (responseCode.StartsWith("2"))
                emailLoggerDTO.EmailError = false;
            else emailLoggerDTO.EmailError = true;
            emailLoggerDTO.EmailResult = e.Response;
            emailLoggerDTO.MessageId = e.Message.MessageId;

            _ = EmailsLogger(emailLoggerDTO.ParEmp, emailLoggerDTO.EmailError, emailLoggerDTO.MessageId, emailLoggerDTO.EmailTo,
                emailLoggerDTO.EmailToFinal, emailLoggerDTO.EmailBcc, emailLoggerDTO.EmailBccFinal, emailLoggerDTO.ParamSubject, emailLoggerDTO.ParamBody, emailLoggerDTO.EmailResult);
            */
        }

        public async Task<IActionResult> EmailNotification(string ParEmp, string ParamTo, string ParamBcc, string ParamSubject, string ParamBody, string Attachments)
        {
            var resultProcess = false;

            try
            {
                if (ParamTo != null && ParamTo != "")
                {
                    using var smtpClient = new SmtpClient();
                    smtpClient.Timeout = 20000;
                    smtpClient.Connect(Configuration["EmailSettings:smtpServer"], Convert.ToInt16(Configuration["EmailSettings:smtpPort"]), SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(Configuration["EmailSettings:smtpUser"], Configuration["EmailSettings:smtpPass"]);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("QRFY Support", Configuration["EmailSettings:smtpUser"]));

                    // To: first contact
                    List<string> ToList = [];
                    ToList = [.. ParamTo.Split(",")];
                    message.To.Add(new MailboxAddress(ToList[0].ToString(), ToList[0].ToString()));

                    List<string> BccList = [];
                    // To: others contacts
                    for (int i = 1; i < ToList.Count; i++)
                    {
                        if (ToList[i] != null && ToList[i] != "")
                        {
                            message.To.Add(new MailboxAddress(ToList[i].ToString(), ToList[i].ToString()));
                        }
                    }

                    // Bcc:
                    if (ParamBcc != null)
                    {
                        BccList = [.. ParamBcc.Split(",")];
                        for (int i = 0; i < BccList.Count; i++)
                        {
                            if (BccList[i] != null && BccList[i] != "")
                            {
                                message.Bcc.Add(new MailboxAddress(BccList[i].ToString(), BccList[i].ToString()));
                            }
                        }
                    }

                    /*
                    if (Attachments != null && Attachments != "")
                    {
                        var AttachList = Attachments.Split("|").ToList();
                        if (AttachList.Count > 0)
                        {
                            foreach (var file in AttachList)
                            {
                                // Create  the file attachment for this email message.
                                Attachment data = new(file, MediaTypeNames.Application.Octet);
                                // Add time stamp information for the file.
                                ContentDisposition disposition = data.ContentDisposition;
                                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                                // Add the file attachment to this email message.
                                message.Attachments.Add(data);
                            }
                        }
                    }
                    */

                    message.Subject = ParamSubject;
                    message.Body = new TextPart(TextFormat.Html) { Text = ParamBody };

                    try
                    {
                        smtpClient.MessageSent += OnMessageSent!;
                        await smtpClient.SendAsync(message);
                        resultProcess = true;
                    }
                    catch (Exception)
                    {
                        resultProcess = false;
                    }
                    finally
                    {
                        smtpClient.Disconnect(true);
                    }
                }
            }
            catch (Exception)
            {
                resultProcess = false;
            }

            if (resultProcess == true)
            {
                return new OkObjectResult(null);
            }
            else
            {
                return new BadRequestResult();
            }
        }

    }
}

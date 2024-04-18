using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using NLog;

namespace SecurityHubs.Hubs
{
    [Authorize]
    public class SecurityHub : Hub
    {
        private readonly IConfiguration ctxConfiguration;
        private readonly IWebHostEnvironment ctxEnvironment;
        private readonly HttpContext httpContext;

        private readonly static Logger logger = LogManager.GetLogger("SecurityHub");
        private static int userCount = 0;

        //########################################//
        //############## Security hub ############//
        //########################################//

        // ############# Constructor #############  
        public SecurityHub(IConfiguration Configuration, IWebHostEnvironment Environment, IHttpContextAccessor HttpContextAccessor)
        {
            ctxEnvironment = Environment;
            ctxConfiguration = Configuration;
            httpContext = HttpContextAccessor.HttpContext!;
        }

        // ############# Overridable hub methods #############  
        public override async Task OnConnectedAsync()
        {

            // ... Connection Logic ... //

            userCount++;
            var ConnectionId = Context.ConnectionId;
            var Agent = httpContext!.Request.Headers["user-agent"];
            var ConnectionToken = httpContext.Request.Query["tokenId"].ToString();

            await Clients.Client(ConnectionId).SendAsync("SetSessionTokenId", ConnectionId); // Execute method "SetSessionTokenId" through client method "SetSessionTokenId"

            // Execute the existing logic into the original, non-overwritten method.
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            userCount--;

            // ... Disconnection Logic ... //

            // Execute the existing logic into the original, non-overwritten method.
            await base.OnDisconnectedAsync(exception);
        }

        // #############  Public HUB methods  #############  
        // *Use these methods from other .CS in same project or invoking from clients browsers .JS files 
        public void ClientDisconnectAsync() {  
            Task.Run(async () => { await OnDisconnectedAsync(null!); }); 
        }
        public string GetConnectionId() { 
            return Context.ConnectionId; 
        }
        public int GetConnectedUsers() { 
            return userCount; 
        }

        public async Task SetProgramHistory( string SRConnectionToken, string ProgramaHistory) {
            await Task.Run(() => {
                
            });
        }

        // ############# Public CLIENT methods  ############# 
        // *Use these methods from client .JS files to receive actions or messages from the HUB

        [HubMethodName("SetSessionTokenId")] // Alias to be used in client rather than method name in Hub definition
        public async Task SetSessionTokenId(string connectionId) { await Clients.Caller.SendAsync(connectionId); }

        [HubMethodName("ReceiveMessage")] // Alias to be used in client rather than method name in Hub definition
        public async Task ReceiveMessage(string user, string message) { await Clients.All.SendAsync(user, message); }

        [HubMethodName("GetProgressStatus")] // Alias to be used in client rather than method name in Hub definition
        public async Task GetProgressStatus(string type, string status, string message1, string message2) { await Clients.All.SendAsync(type, status, message1, message2); }
    }
}

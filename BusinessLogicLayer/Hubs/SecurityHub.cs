using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using NLog;

namespace SecurityHubs.Hubs
{
    [Authorize]
    public class SecurityHub(IHttpContextAccessor httpContextAccessor): Hub
    {
        private static int userCount = 0;

        //########################################//
        //############## Security hub ############//
        //########################################//

        // ############# Overridable hub methods #############  
        public override async Task OnConnectedAsync()
        {

            // ... Connection Logic ... //

            userCount++;
            var ConnectionId = Context.ConnectionId;
            var Agent = httpContextAccessor!.HttpContext!.Request.Headers.UserAgent;
            var ConnectionToken = httpContextAccessor.HttpContext!.Request.Query["tokenId"].ToString();

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

        public string GetConnectionId() => Context.ConnectionId;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Needs access to instance-level Context property provided by SignalR Hub.")]
        public int GetConnectedUsers() { 
            return userCount; 
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Needs access to instance-level Context property provided by SignalR Hub.")]
        public async Task SetProgramHistory( string SRConnectionToken, string ProgramaHistory) {
            await Task.Run(() => {
                // dummy
                string srconn = SRConnectionToken;
                string programhst = ProgramaHistory;
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

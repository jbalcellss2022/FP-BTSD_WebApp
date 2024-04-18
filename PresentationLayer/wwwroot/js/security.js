window.addEventListener('load', OnLoadSecurity);

var connection;
var timeInactivity;

// !Important to avoid errores on unload page
window.onbeforeunload = function () {
    if (this.connection !== null && this.connection !== this.undefined) {
        connection.invoke("ClientDisconnectAsync");
        this.connection.stop();
    }
}; 

/***** onload() ******/
function OnLoadSecurity() {
    AuthResetTimer();
    AuthInactivityTime(); // Check for user inactivity
    
    url = "/securityhub"; // Token control to generate hub URL
    token = GetTokenId();
    if (token !== "" && token !== null && token !== undefined) { url = "/securityhub?tokenId=" + token; }

    // Create Hub connection
    let connection;
    let signalR;
    try {
        signalR = require("@microsoft/signalr");
        console.log('QRFY. SignalR is installed. Starting Hub..');
        try {
            connection = new signalR
                .HubConnectionBuilder()
                .withUrl(url)
                .configureLogging(signalR.LogLevel.Error)  // Error, Warning, Information, Trace
                .build();

            connection.serverTimeoutInMilliseconds = 60000; // TimeOut 60 sec.
            connection.setTimeout = 60000;

            // ############################################################################################## //
            // ######## Receive methods from the Hub "connection.on" always before start() !Important  ###### //
            // ############################################################################################## //

            connection.on("ReceiveMessage", (user, message) => { console.log(user, message); });

            connection.on("SetConnectionId", (connectionId) => { SetTokenId(connection, connectionId); });

            connection.on("GetProgressStatus", (type, status, message1, message2) => { SetProgressStatus(type, status, message1, message2); });

            start(connection, url);
            connection.onclose(async (error) => { console.log("QRFY. SignalR OnClose: Trying to reconnect to SecurityHub..."); await start(connection, url); });  // Always try reconnection

            // ############################################################################################## //
            // ######## Example to Invoque medthods in the Hub from the client                         ###### //
            // ############################################################################################## //

            //connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
        }
        catch (error) {
            console.error("QRFY. SignalR is not installed. Cannot start. ", error);   // Handle errors that occur during the instantiation of the connection.
        } 
    }
    catch (error) {
        console.log('QRFY. SignalR is not installed.');
    }
}

// ################################################################ //
// ################## COMMON FUNCTIONS AND START ################## //
// ################################################################ //
async function start(connection, url) {
    try {
        if (connection !== undefined) {
            await connection.start({ transport: ['serverSentEvents', 'foreverFrame', 'longPolling'] });
            connection.invoke('GetConnectionId')
                .then(function (connectionId){
                    SetTokenId(connection, connectionId);
                    console.log("QRFY. SignalR Connected to SecurityHub => Token: ", GetTokenId(), " ConnectionId: ", connectionId);
                });
            connection.invoke('SetProgramHistory', GetTokenId(), window.location.href);
        } else {
            connection = new signalR.HubConnectionBuilder()
                .withUrl(url)
                .configureLogging(signalR.LogLevel.Error)   // Error, Warning, Information, Trace
                .build();
            connection.serverTimeoutInMilliseconds = 60000; // TimeOut 60 sec.
            connection.setTimeout = 60000;
            start(connection, url);
        }
    } catch (err) {
        console.log("QRFY. SignalR Exception -> ", err.toString());
        setTimeout(() => start(connection, url), 20000);
    }
}

function GetTokenId() { return window.sessionStorage.tokenId; }

function SetTokenId(connection, connectionId) {
    var sessionId = window.sessionStorage.tokenId;
    if (!sessionId) { window.sessionStorage.tokenId = connectionId; }
}

function AuthLogout() {
    var iLabSpinner = Rats.UI.LoadAnimation.start();
    location.href = '/SignIn/Login';
    $.ajax({
        url: "/SignIn/MakeLogout",
        async: true,
        type: "POST",
        data: null,
        datatype: "json",
        processData: false,
        contentType: false,
        success: function (data, textStatus, XmlHttpRequest) {
            location.href = '/SignIn/Login';
        },
        always: function (data) { },
        error: function (data) { }
    });
}

function AuthResetTimer() {
    //console.log("QRFY. Resetting inativity Time:", timeInactivity);
    clearTimeout(timeInactivity);
    timeInactivity = setTimeout(AuthLogout, 300000);    // 5 Minutes
    //timeInactivity = setTimeout(AuthLogout, 1800000);   // 30 Minutes
    //timeInactivity = setTimeout(AuthLogout, 15000);     // 15 Secs
}

function AuthInactivityTime(){
    // DOM Events
    document.onmousemove = AuthResetTimer;                      // on mouse move
    document.onkeypress = AuthResetTimer;                       // on keypress
    document.ontouchstart = AuthResetTimer;                     // on touchpad clicks
    document.onclick = AuthResetTimer;                          
    document.onkeypress = AuthResetTimer;                       // on keypress
    document.addEventListener('scroll', AuthResetTimer, true);  // on scroll, improved
}

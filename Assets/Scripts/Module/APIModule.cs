using System.Collections.Generic;

public class APIModule
{
    private string API_URL = "https://hkillbijfk.execute-api.ap-northeast-1.amazonaws.com/prod";
        //"https://hkillbijfk.execute-api.ap-northeast-1.amazonaws.com/prod/";
        //"https://0sos1lsc4c.execute-api.ap-northeast-2.amazonaws.com/prod/";
        //"https://4g7b0oce32.execute-api.us-east-1.amazonaws.com/prod/";

    public class MatchmakingRequest
    {
        // Class for MatchRequest API parameters
        public string playerId;
        public string worldId;

        public MatchmakingRequest(string playerId, string worldId)
        {
            this.playerId = playerId;
            this.worldId = worldId;
        }
    }

    public struct MatchmakingResponse
    {
        public string ticketId;

        public MatchmakingResponse(string ticketId)
        {
            this.ticketId = ticketId;
        }
    }

    public struct MatchstatusRequest
    {
        public string playerId;
        public string ticketId;

        public MatchstatusRequest(string playerId, string ticketId)
        {
            this.playerId = playerId;
            this.ticketId = ticketId;
        }
    }

    public struct MatchstatusResponse
    {
        public string address;
        public int port;
        public string playerSessionId;

        public MatchstatusResponse(string address, int port, string playerSessionId)
        {
            this.address = address;
            this.port = port;
            this.playerSessionId = playerSessionId;
        }
    }

    public APIModule()
    {
        Dictionary<string, string> arguments = GetCommandLineArguments();
        if (arguments.ContainsKey("url"))
        {
            API_URL = arguments["url"];
        }
        LogModule.WriteToLogFile("[APIModule] Given API URL : " + API_URL);
    }

    private Dictionary<string, string> GetCommandLineArguments()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string key = null;
        for (int i = 1; i < args.Length; i++)
        {
            if (key != null)
            {
                dictionary.Add(key, args[i]);
                key = null;
            }
            else
            {
                if (args[i].StartsWith("-"))
                {
                    key = args[i].Substring(1);
                }
            }
        }
        return dictionary;
    }

    public string GetMatchRequestAPI()
    {
        return API_URL.EndsWith("/") ? API_URL + "matchrequest" : API_URL + "/matchrequest";
    }

    public string GetMatchStatusAPI()
    {
        return API_URL.EndsWith("/") ? API_URL + "matchstatus" : API_URL + "/matchstatus";
    }
}

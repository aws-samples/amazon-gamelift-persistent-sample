/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System.Collections.Generic;

public class APIModule
{
    private string API_URL = "https://0sos1lsc4c.execute-api.ap-northeast-2.amazonaws.com/prod/";

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
        return API_URL + "matchrequest";
    }

    public string GetMatchStatusAPI()
    {
        return API_URL + "matchstatus";
    }
}

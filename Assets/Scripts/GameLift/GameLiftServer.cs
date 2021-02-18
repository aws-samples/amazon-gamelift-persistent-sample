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
using Aws.GameLift.Server;

public class GameLiftServer
{

    public bool GameLiftStart(int listenPort)
    {
        //Debug.Log("GameLift Start with Port:" + listenPort);
        LogModule.WriteToLogFile("[GameLift] GameLift Start with Port:" + listenPort);
        var initSDKOutcome = GameLiftServerAPI.InitSDK();
        if (initSDKOutcome.Success)
        {
            ProcessParameters processParameters = new ProcessParameters(
                (gameSession) =>
                {
                    //OnStartGameSession Callback
                    LogModule.WriteToLogFile("[GameLift] OnStartGameSession with Parameter=" + gameSession);
                    GameLiftServerAPI.ActivateGameSession();
                },
                (gameSession) =>
                {
                    //OnUpdateGameSession Callback
                    //You can implement custom Match update logics using Backfill Ticket, UpdateReason, GameSession data.
                    LogModule.WriteToLogFile("[GameLift] OnUpdateGameSession with Backfill Ticket=" + gameSession.BackfillTicketId + ", UpdateReason=" + gameSession.UpdateReason);
                },
                () =>
                {
                    //OnProcessTerminate Callback
                    LogModule.WriteToLogFile("[GameLift] ProcessEnding");
                    GameLiftServerAPI.ProcessEnding();
                },
                () =>
                {
                    //OnHealthCheck Callback
                    return true;
                },
                listenPort,
                new LogParameters(new List<string>()
                {
                    "./local/game/logs/myserver.log"
                    //"C:\\game\\myserver.log"
                }
                ));
            var processReadyOutcome = GameLiftServerAPI.ProcessReady(processParameters);
            if (processReadyOutcome.Success)
            {
                LogModule.WriteToLogFile("[GameLift] ProcessReady Success");
                return true;
            }
            else
            {
                LogModule.WriteToLogFile("[GameLift] ProcessReady Failure : " + processReadyOutcome.Error.ToString());
                return false;
            }
        }
        else
        {
            LogModule.WriteToLogFile("[GameLift] InitSDK failure : " + initSDKOutcome.Error.ToString());
            return false;
        }
    }

    public bool AcceptPlayer(string playerSessionId)
    {
        var acceptPlayerSessionOutcome = GameLiftServerAPI.AcceptPlayerSession(playerSessionId);
        if (acceptPlayerSessionOutcome.Success)
        {
            LogModule.WriteToLogFile("[GameLift] Player Session Validated");
            return true;
        }
        else
        {
            LogModule.WriteToLogFile("[GameLift] Player Session Rejected. AcceptPlayerSession Result : " + acceptPlayerSessionOutcome.Error.ToString());
            return false;
        }
    }

    public void RemovePlayer(string playerSessionId)
    {
        var removePlayerSessionOutcome = GameLiftServerAPI.RemovePlayerSession(playerSessionId);
        if (removePlayerSessionOutcome.Success)
        {
            LogModule.WriteToLogFile("[GameLift] Remove Player Session Success : " + playerSessionId);
        }
        else
        {
            LogModule.WriteToLogFile("[GameLift] Remove Player Session Failed. Result : " + removePlayerSessionOutcome.Error.ToString());
        }
    }

    public void EndProcess()
    {
        var processEndingOutcome = GameLiftServerAPI.ProcessEnding();
        if (processEndingOutcome.Success)
        {
            LogModule.WriteToLogFile("[GameLift] End GameLift Server Process");
        }
        else
        {
            LogModule.WriteToLogFile("[GameLift] Process Ending Failed. Result : " + processEndingOutcome.Error.ToString());
        }
    }
}

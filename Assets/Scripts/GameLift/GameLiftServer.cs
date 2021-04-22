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

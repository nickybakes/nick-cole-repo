using AsyncIO;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

/// <summary>
///     Example of requester who only sends Hello. Very nice guy.
///     You can copy this class and modify Run() to suits your needs.
///     To use this class, you just instantiate, call Start() when you want to start and Stop() when you want to stop.
/// </summary>
public class PythonRequester : RunAbleThread
{
    private PlayerController playerController;

    public PythonRequester(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    /// <summary>
    ///     Request Hello message to server and receive message back. Do it 10 times.
    ///     Stop requesting when Running=false.
    /// </summary>
    protected override void Run()
    {
        ForceDotNet.Force(); // this line is needed to prevent unity freeze after one use, not sure why yet
        using (RequestSocket client = new RequestSocket())
        {
            client.Connect("tcp://localhost:5555");

            while (true)
            {
                byte[] frameBuffer;
                if (playerController.CanGetFrame())
                {
                    frameBuffer = playerController.getFrame();
                    client.SendFrame(frameBuffer);
                    string message = null;
                    bool gotMessage = false;
                    while (true)
                    {
                        gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
                        if (gotMessage) break;
                    }

                    if (gotMessage) Debug.Log("Received " + message);

                }
            }
            NetMQConfig.Cleanup();
        }
    }
}
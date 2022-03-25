using UnityEngine;

public static class InputSensor
{
    // When multiplayer will be implemented, these methods
    // will send the event to the server
    public static void TriggerKey(KeyCode code, int playerId)
    {
        InputMaster.Event e = new InputMaster.Event();
        e.type = InputMaster.EventType.KEY_DOWN;
        e.key = code;
        e.playerId = playerId;

        InputMaster.SendEvent(e);
    }

    public static void TriggerMouseClick(int playerId)
    {
        InputMaster.Event e = new InputMaster.Event();
        e.type = InputMaster.EventType.MOUSE_CLICK;
        e.playerId = playerId;

        InputMaster.SendEvent(e);
    }

    public static void TriggerMouseMove(Vector2 pos, int playerId)
    {
        InputMaster.Event e = new InputMaster.Event();
        e.type = InputMaster.EventType.MOUSE_MOVE;
        e.mouse = pos;
        e.playerId = playerId;

        InputMaster.SendEvent(e);
    }
}
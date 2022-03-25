﻿using UnityEngine;

public static class InputMaster
{
    public enum EventType
    {
        KEY_DOWN,
        MOUSE_CLICK,
        MOUSE_MOVE
    }

    public class Event
    {
        public EventType type;

        public KeyCode key;
        public Vector2 mouse;
        public int playerId;

        public bool KeyDown(KeyCode code) => type == EventType.KEY_DOWN && key == code;
        public bool MouseDown() => type == EventType.MOUSE_CLICK;
        public bool MouseMove() => type == EventType.MOUSE_MOVE;
    }

    public static MinigameMaster minigameMaster;

    public static void SendEvent(Event e)
    {
        // This will send the event to the server, which will redirect it to the minigame master.
        // For now, this will directly send it to the minigame master.
        switch(e.type)
        {
            case EventType.KEY_DOWN:
                minigameMaster.OnPlayerKeyDown(e.playerId, e.key);
                break;
            case EventType.MOUSE_CLICK:
                minigameMaster.OnPlayerMouseClick(e.playerId);
                break;
            case EventType.MOUSE_MOVE:
                minigameMaster.OnPlayerMouseMove(e.playerId, e.mouse);
                break;
        }
    }

    public static bool GetKeyDown(KeyCode key)
    {
        return GameMaster.InputEnabled() && Input.GetKeyDown(key);
    }

    public static bool GetMouseButtonDown(int button)
    {
        return GameMaster.InputEnabled() && Input.GetMouseButtonDown(button);
    }
}
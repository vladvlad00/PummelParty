﻿using System.Collections.Generic;
using UnityEngine;

public static class TaleExtra
{
    public static TaleUtil.Action FadeIn(float duration = 1f) =>
        TaleUtil.Queue.Enqueue(new TaleUtil.TransitionAction("Fade", TaleUtil.TransitionAction.Type.IN, duration));
    public static TaleUtil.Action FadeOut(float duration = 1f) =>
        TaleUtil.Queue.Enqueue(new TaleUtil.TransitionAction("Fade", TaleUtil.TransitionAction.Type.OUT, duration));

    public static TaleUtil.Action RipIn(float duration = 0.75f) =>
        TaleUtil.Queue.Enqueue(new TaleUtil.TransitionAction("Rip", TaleUtil.TransitionAction.Type.IN, duration));
    public static TaleUtil.Action RipOut(float duration = 0.75f) =>
        TaleUtil.Queue.Enqueue(new TaleUtil.TransitionAction("Rip", TaleUtil.TransitionAction.Type.OUT, duration));

    public static TaleUtil.Action ReturnToGame() =>
        TaleUtil.Queue.Enqueue(new TaleUtil.SceneAction("Game"));

    public static TaleUtil.Action DisableInput() =>
        TaleUtil.Queue.Enqueue(new TaleUtil.ExecAction(() =>
        {
            GameMaster.INPUT_ENABLED = false;
        }));
    public static TaleUtil.Action EnableInput() =>
        TaleUtil.Queue.Enqueue(new TaleUtil.ExecAction(() =>
        {
            GameMaster.INPUT_ENABLED = true;
        }));
}
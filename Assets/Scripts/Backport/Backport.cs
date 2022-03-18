/*
 * Bridge between Unity and Risa (C99). Designed for in-game consoles. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// Before the event system
[DefaultExecutionOrder(-10000)]
public class Backport : MonoBehaviour
{
    public enum State
    {
        IDLE,
        VM_RUNNING
    }

    // If the VM enters an infinite loop, don't freeze the game.
    public static uint MAX_INSTRUCTIONS_PER_FRAME = 100;

    // Show/hide the console.
    public const KeyCode CONSOLE_TOGGLE_KEY = KeyCode.BackQuote;
    // Force stop the VM (like CTRL+C in a terminal emulator)
    public const KeyCode INTERRUPT_VM_KEY = KeyCode.Escape;

    public static Backport INSTANCE;

    [SerializeField]
    GameObject canvas;
    [SerializeField]
    TMP_InputField output;
    [SerializeField]
    TMP_InputField input;
    [SerializeField]
    GameObject inputPrefix;

    Risa.VM vm;

    State state;
    int lineCount;

    void Awake()
    {
        if(INSTANCE != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        INSTANCE = this;

        vm = new Risa.VM(true);
        vm.GetIO().RedirectIn(Stdin);
        vm.GetIO().RedirectOut(Stdout);
        vm.GetIO().RedirectErr(Stderr);

        vm.LoadAllLibraries();

        state = State.IDLE;

        // Enter -> execute
        input.onSubmit.AddListener((text) =>
        {
            if(string.IsNullOrWhiteSpace(text))
            {
                input.text = "";
                input.ActivateInputField();
                return;
            }

            if (!text.EndsWith(';'))
            {
                // In case the user forgets the semicolon.
                // Even if Risa expects a semicolon, it's a chore for the user to write it every single time.
                // Expressions like "1 + 1" are much easier to write without a trailing semicolon.
                text = text + ';';
            }

            if (state == State.IDLE)
            {
                OnExecutionStart();
                if (!Execute(text))
                {
                    OnExecutionEnd(false);
                }
            }
        });

        lineCount = 1;
        WriteLine(string.Format("Backport ver1.0 (risa {0})\n(C) 2022 The Deprimus Members\n", Risa.C99.VERSION));

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        HandleInput();

        switch(state)
        {
            case State.IDLE:
                break;
            case State.VM_RUNNING:
                try
                {
                    if (vm.Run(MAX_INSTRUCTIONS_PER_FRAME))
                    {
                        OnExecutionEnd(true);
                    }
                }
                catch(Risa.RuntimeException)
                {
                    OnExecutionEnd(false);
                }
                break;
        }
    }

    void HandleInput()
    {
        if(GetKeyDownNoMod(CONSOLE_TOGGLE_KEY))
        {
            if(!canvas.activeSelf)
            {
                canvas.SetActive(true);
                input.Select();
                input.ActivateInputField();
            }
            else
            {
                canvas.SetActive(false);
            }
        }
        else if(state == State.VM_RUNNING && IsOpen() && GetKeyDownNoMod(INTERRUPT_VM_KEY))
        {
            state = State.IDLE;
            WriteError("-- interrupted --");
            OnExecutionEnd(false);
        }
    }

    void OnExecutionStart()
    {
        input.readOnly = true;
        inputPrefix.SetActive(false);

        WriteLine(string.Format("> {0}", input.text));

        input.text = "";
    }

    void OnExecutionEnd(bool success)
    {
        state = State.IDLE;

        input.readOnly = false;
        input.ActivateInputField();
        inputPrefix.SetActive(true);

        if (success)
        {
            string result = vm.GetLastResult().ToString();

            // Print the result, if any.
            // "null" as a string, and not as a keyword, since it's a Risa null.
            if (result != "null")
            {
                WriteLine(result);
            }
        }
    }

    public bool IsOpen()
    {
        return canvas.activeSelf;
    }

    public bool Execute(string source)
    {
        Debug.Assert(state == State.IDLE, "[BACKPORT] Execute() called when the VM is already running");

        try
        {
            vm.Load(source);
            state = State.VM_RUNNING;
            return true;
        }
        catch(Risa.CompileTimeException)
        {
            return false;
        }
    }

    public static void Write(string text)
    {
        Check();

        // First, ensure that the line limit is satisfied
        bool hasNewLines = false;

        for (int i = 0; i < text.Length; ++i)
        {
            if(text[i] == '\n')
            {
                ++INSTANCE.lineCount;
                hasNewLines = true;
            }
        }

        // Limit exceeded, try to delete old lines
        if(hasNewLines && INSTANCE.lineCount > INSTANCE.output.lineLimit)
        {
            // Find the first index from which to substring, such that the line count doesn't exceed the limit
            for (int i = 0; i < INSTANCE.output.text.Length; ++i)
            {
                if (INSTANCE.output.text[i] == '\n')
                {
                    if (--INSTANCE.lineCount == INSTANCE.output.lineLimit)
                    {
                        INSTANCE.output.text = INSTANCE.output.text.Substring(i + 1);
                        break;
                    }
                }
            }

            // If it's still above the limit, deleting old lines isn't enough
            // Therefore, delete from the text lines
            if (INSTANCE.lineCount > INSTANCE.output.lineLimit)
            {
                for (int i = 0; i < text.Length; ++i)
                {
                    if (text[i] == '\n')
                    {
                        if (--INSTANCE.lineCount == INSTANCE.output.lineLimit)
                        {
                            text = text.Substring(i + 1);
                            break;
                        }
                    }
                }

                INSTANCE.output.text = text;
            }
        }

        bool scrollToBottom = (INSTANCE.output.verticalScrollbar.value == 1f || INSTANCE.output.verticalScrollbar.size == 1f);

        INSTANCE.output.text += text;

        // Scroll if the scrollbar was already scrolled to the max before, or if there was no space to scroll before (and now there is)
        if(INSTANCE.output.verticalScrollbar.size != 1f)
        {
            if (scrollToBottom)
            {
                INSTANCE.output.verticalScrollbar.value = 1f;
            }

            if(!INSTANCE.output.verticalScrollbar.gameObject.activeSelf)
            {
                // There's now space to scroll, show the scrollbar
                INSTANCE.output.verticalScrollbar.gameObject.SetActive(true);
            }
        }
        else
        {
            // There's no space to scroll, hide the scrollbar
            if (INSTANCE.output.verticalScrollbar.gameObject.activeSelf)
            {
                INSTANCE.output.verticalScrollbar.gameObject.SetActive(false);
            }
        }
    }

    public static void WriteLine(string text)
    {
        Write(text + "\n");
    }

    public static void WriteError(string text)
    {
        WriteLine(string.Format("<color=#FF0000>{0}</color>", text));
    }

    static string Stdin(Risa.IO.InputMode mode)
    {
        return "";
    }

    static void Stdout(string msg)
    {
        Write(msg);
    }

    static void Stderr(string msg)
    {
        WriteError(msg.Trim('\n'));
    }

    static void Check()
    {
        Debug.Assert(INSTANCE != null, "[BACKPORT] Backport was not initialized, but a backport method was called");
    }

    static bool GetKeyDownNoMod(KeyCode key)
    {
        return Input.GetKeyDown(key)
            && !Input.GetKeyDown(KeyCode.LeftShift)
            && !Input.GetKeyDown(KeyCode.RightShift)
            && !Input.GetKeyDown(KeyCode.LeftControl)
            && !Input.GetKeyDown(KeyCode.RightControl)
            && !Input.GetKeyDown(KeyCode.LeftAlt)
            && !Input.GetKeyDown(KeyCode.RightAlt)
            && !Input.GetKeyDown(KeyCode.LeftMeta)
            && !Input.GetKeyDown(KeyCode.RightMeta);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class BackportCommands
{
    public static void Init()
    {
        Backport.INSTANCE.vm.LoadGlobalNative("rig_dice", RigDice);
        Backport.INSTANCE.vm.LoadGlobalNative("rig_minigame", RigMinigame);
        Backport.INSTANCE.vm.LoadGlobalNative("rd", RigDice);
        Backport.INSTANCE.vm.LoadGlobalNative("rm", RigMinigame);
        Backport.INSTANCE.vm.LoadGlobalNative("reverse", Reverse);
        Backport.INSTANCE.vm.LoadGlobalNative("change_crown_spot", ChangeCrownSpot);
    }

    static Risa.Value RigDice(Risa.VM vm, Risa.Args args)
    {
        if(args.Count() == 0)
        {
            return vm.CreateInt(GameMaster.INSTANCE.rigDice);
        }

        if (args.Count() > 1 || !args.Get(0).IsInt())
        {
            Backport.WriteError("Invalid arguments for rig_dice (expected an int)");
            return Risa.Value.NULL;
        }

        int value = (int) args.Get(0).AsInt();

        GameMaster.INSTANCE.rigDice = value;
        return vm.CreateInt(GameMaster.INSTANCE.rigDice);
    }

    static Risa.Value RigMinigame(Risa.VM vm, Risa.Args args)
    {
        if (args.Count() == 0)
        {
            if(GameMaster.INSTANCE.rigMinigame == null)
            {
                return Risa.Value.NULL;
            }

            return vm.CreateString(GameMaster.INSTANCE.rigMinigame);
        }

        if (args.Count() > 1 || !args.Get(0).IsString())
        {
            Backport.WriteError("Invalid arguments for rig_minigame (expected a string)");
            return Risa.Value.NULL;
        }

        string value = args.Get(0).AsString();

        GameMaster.INSTANCE.rigMinigame = value;
        return vm.CreateString(GameMaster.INSTANCE.rigMinigame);
    }

    // TODO: test if everything works when a player is currently moving and the direction is reversed
    static Risa.Value Reverse(Risa.VM vm, Risa.Args args)
    {
        if (args.Count() > 0)
        {
            Backport.WriteError("Invalid arguments for reverse (expected none)");
            return Risa.Value.NULL;
        }

        GameMaster.INSTANCE.ReverseMoveDirection();
        return vm.CreateBool(GameMaster.INSTANCE.moveDirectionReversed);
    }

    static Risa.Value ChangeCrownSpot(Risa.VM vm, Risa.Args args)
    {
        if (args.Count() > 0)
        {
            Backport.WriteError("Invalid arguments for change_crown_spot (expected none)");
            return Risa.Value.NULL;
        }

        GameMaster.INSTANCE.RemoveCrownSpot();
        GameMaster.INSTANCE.ChooseNewCrownSpot();
        return Risa.Value.NULL;
    }
}
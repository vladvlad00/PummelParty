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
    }

    static Risa.Value RigDice(Risa.VM vm, Risa.Args args)
    {
        if(args.Count() == 0)
        {
            return vm.CreateInt(GameMaster.INSTANCE.rigDice);
        }

        if (args.Count() > 1 || !args.Get(0).IsInt())
        {
            Backport.WriteError("Invalid arguments for line_limit (expected an int > 0)");
            return Risa.Value.NULL;
        }

        int value = (int) args.Get(0).AsInt();

        GameMaster.INSTANCE.rigDice = value;
        return vm.CreateInt(GameMaster.INSTANCE.rigDice);
    }
}
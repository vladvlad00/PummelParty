/**
 * risa-sharp (https://github.com/exom-dev/risa-sharp)
 * C# wrapper for the Risa C99 implementation.
 *
 * Version 0.0.B 'PREVIEW' - This wrapper is NOT guaranteed to work with any other version of the implementation.
 *
 * This project is licensed under the MIT license.
 * Copyright (c) 2021 The Exom Developers (https://github.com/exom-dev)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// Contains wrapper classes for the Risa C99 implementation.
/// </summary>
namespace Risa
{
    /// <summary>
    /// Contains PInvoke functions, structures, and enums which are part of the Risa C99 API.
    /// </summary>
    public static class C99
    {
        // This is the Risa version. You could replace this with branch-commit (e.g. master-1c2f4d1).
        // The version isn't needed anywhere; it's only used for identifying the risa build.
        public const string VERSION = "master-1c2f4d1";

        // The default path is 'risa.dll'. Change this to fit your needs.
        public const string DLL_PATH = "risa.dll";

        [DllImport(DLL_PATH, EntryPoint = "risa_mem_free", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaMemFree(IntPtr ptr, IntPtr file, uint line);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaClusterCreate();

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_write", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaClusterWrite(IntPtr cluster, byte data, uint index);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_write_constant", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaClusterWriteConstant(IntPtr cluster, RisaValue constant);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_get_size", CallingConvention = CallingConvention.Cdecl)]
        public extern static uint RisaClusterGetSize(IntPtr cluster);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_get_data_at", CallingConvention = CallingConvention.Cdecl)]
        public extern static byte RisaClusterGetDataAt(IntPtr cluster, uint index);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_get_constant_count", CallingConvention = CallingConvention.Cdecl)]
        public extern static uint RisaClusterGetConstantCount(IntPtr cluster);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_get_constant_at", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaClusterGetConstantAt(IntPtr cluster, uint index);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_clone", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaClusterClone(IntPtr dest, IntPtr src);

        [DllImport(DLL_PATH, EntryPoint = "risa_cluster_free", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaClusterFree(IntPtr cluster);


        [DllImport(DLL_PATH, EntryPoint = "risa_io_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaIOCreate();

        [DllImport(DLL_PATH, EntryPoint = "risa_io_free", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaIOFree(IntPtr io);

        [DllImport(DLL_PATH, EntryPoint = "risa_io_clone", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaIOClone(IntPtr dest, IntPtr src);

        [DllImport(DLL_PATH, EntryPoint = "risa_io_set_free_input", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaIOSetFreeInput(IntPtr io, bool value);

        [DllImport(DLL_PATH, EntryPoint = "risa_io_redirect_in", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaIoRedirectIn(IntPtr io, IO.InHandler handler);

        [DllImport(DLL_PATH, EntryPoint = "risa_io_redirect_out", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaIoRedirectOut(IntPtr io, IO.OutHandler handler);

        [DllImport(DLL_PATH, EntryPoint = "risa_io_redirect_err", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaIoRedirectErr(IntPtr io, IO.OutHandler handler);


        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaCompilerCreate();

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_load_strings", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaCompilerLoadStrings(IntPtr compiler, IntPtr strings);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_target", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaCompilerTarget(IntPtr compiler, IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_get_io", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaCompilerGetIO(IntPtr compiler);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_get_function", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaCompilerGetFunction(IntPtr compiler);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_get_strings", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaCompilerGetStrings(IntPtr compiler);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_set_repl_mode", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaCompilerSetReplMode(IntPtr compiler, byte value);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_compile", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaCompilerStatus RisaCompilerCompile(IntPtr compiler, string str);

        [DllImport(DLL_PATH, EntryPoint = "risa_compiler_shallow_free", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaCompilerShallowFree(IntPtr compiler);


        [DllImport(DLL_PATH, EntryPoint = "risa_value_from_null", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaValueFromNull();

        [DllImport(DLL_PATH, EntryPoint = "risa_value_from_bool", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaValueFromBool(byte value);

        [DllImport(DLL_PATH, EntryPoint = "risa_value_from_byte", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaValueFromByte(byte value);

        [DllImport(DLL_PATH, EntryPoint = "risa_value_from_int", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaValueFromInt(long value);

        [DllImport(DLL_PATH, EntryPoint = "risa_value_from_float", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaValueFromFloat(double value);

        [DllImport(DLL_PATH, EntryPoint = "risa_value_from_dense", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaValueFromDense(IntPtr value);

        [DllImport(DLL_PATH, EntryPoint = "risa_value_to_string", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaValueToString(RisaValue value);


        [DllImport(DLL_PATH, EntryPoint = "risa_map_entry_get_key", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaMapEntryGetKey(IntPtr entry);

        [DllImport(DLL_PATH, EntryPoint = "risa_map_entry_get_value", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaMapEntryGetValue(IntPtr entry);


        [DllImport(DLL_PATH, EntryPoint = "risa_dense_get_type", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaDenseValueType RisaDenseGetType(IntPtr dense);


        [DllImport(DLL_PATH, EntryPoint = "risa_dense_array_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseArrayCreate();

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_array_get_count", CallingConvention = CallingConvention.Cdecl)]
        public extern static uint RisaDenseArrayGetCount(IntPtr dense);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_array_get", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaDenseArrayGet(IntPtr dense, uint index);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_array_set", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaDenseArraySet(IntPtr dense, uint index, RisaValue value);


        [DllImport(DLL_PATH, EntryPoint = "risa_dense_object_create_under", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseObjectCreateUnder(IntPtr vm, uint entryCount);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_object_get_count", CallingConvention = CallingConvention.Cdecl)]
        public extern static uint RisaDenseObjectGetCount(IntPtr dense);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_object_get_entry", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseObjectGetEntry(IntPtr dense, uint index);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_object_set", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaDenseObjectSet(IntPtr dense, IntPtr key, RisaValue value);


        [DllImport(DLL_PATH, EntryPoint = "risa_dense_string_as_cstring", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseStringAsCString(IntPtr dense);


        [DllImport(DLL_PATH, EntryPoint = "risa_dense_function_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseFunctionCreate();

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_function_get_cluster", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseFunctionGetCluster(IntPtr function);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_function_free", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaDenseFunctionFree(IntPtr function);


        [DllImport(DLL_PATH, EntryPoint = "risa_dense_native_get_arg", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaDenseNativeGetArg(IntPtr args, byte index);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_native_get_base", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaDenseNativeGetBase(IntPtr args, byte argc);

        [DllImport(DLL_PATH, EntryPoint = "risa_dense_native_value", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaDenseNativeValue(RisaNativeFunction function);


        [DllImport(DLL_PATH, EntryPoint = "risa_gc_check", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaGCCheck(IntPtr vm);


        [DllImport(DLL_PATH, EntryPoint = "risa_vm_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaVMCreate();

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_string_create", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaVMStringCreate(IntPtr vm, string str, uint length);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_load_compiler_data", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMLoadCompilerData(IntPtr vm, IntPtr compiler);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_load_function", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMLoadFunction(IntPtr vm, IntPtr function);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_load_strings", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMLoadStrings(IntPtr vm, IntPtr strings);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_global_set", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMGlobalSet(IntPtr vm, string str, uint length, RisaValue value);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_global_set_native", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMGlobalSetNative(IntPtr vm, string str, uint length, RisaNativeFunction fn);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_execute", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaVMStatus RisaVMExecute(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_run", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaVMStatus RisaVMRun(IntPtr vm, uint maxInstr);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_get_strings", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaVMGetStrings(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_get_io", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr RisaVMGetIO(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_get_acc", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaVMGetACC(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_set_repl_mode", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMSetReplMode(IntPtr vm, byte value);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_register_dense", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMRegisterDense(IntPtr vm, IntPtr dense);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_invoke_args", CallingConvention = CallingConvention.Cdecl)]
        public extern static RisaValue RisaVMInvokeArgs(IntPtr vm, IntPtr @base, RisaValue callee, byte argc, IntPtr args);

        [DllImport(DLL_PATH, EntryPoint = "risa_vm_free", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaVMFree(IntPtr vm);


        [DllImport(DLL_PATH, EntryPoint = "risa_std_register_core", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaSTDRegisterCore(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_std_register_io", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaSTDRegisterIO(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_std_register_string", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaSTDRegisterString(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_std_register_math", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaSTDRegisterMath(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_std_register_reflect", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaSTDRegisterReflect(IntPtr vm);

        [DllImport(DLL_PATH, EntryPoint = "risa_std_register_debug", CallingConvention = CallingConvention.Cdecl)]
        public extern static void RisaSTDRegisterDebug(IntPtr vm);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RisaValue RisaNativeFunction(IntPtr vm, byte argc, IntPtr args);

        public enum RisaValueType
        {
            RISA_VAL_NULL,
            RISA_VAL_BOOL,
            RISA_VAL_BYTE,
            RISA_VAL_INT,
            RISA_VAL_FLOAT,
            RISA_VAL_DENSE
        }

        public enum RisaDenseValueType
        {
            RISA_DVAL_STRING,
            RISA_DVAL_ARRAY,
            RISA_DVAL_OBJECT,
            RISA_DVAL_UPVALUE,
            RISA_DVAL_FUNCTION,
            RISA_DVAL_CLOSURE,
            RISA_DVAL_NATIVE
        }

        public enum RisaCompilerStatus
        {
            RISA_COMPILER_STATUS_OK,
            RISA_COMPILER_STATUS_ERROR
        }

        public enum RisaVMStatus
        {
            RISA_VM_STATUS_OK,
            RISA_VM_STATUS_HALTED,
            RISA_VM_STATUS_ERROR
        }

        public enum RisaAssemblerStatus
        {
            RISA_ASM_STATUS_OK,
            RISA_ASM_STATUS_ERROR
        }

        public enum RisaInputMode
        {
            RISA_INPUT_MODE_CHAR = 0,
            RISA_INPUT_MODE_WORD = 1,
            RISA_INPUT_MODE_LINE = 2
        }

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct RisaValue
        {
            [FieldOffset(0)]
            public RisaValueType type;

            [FieldOffset(8)]
            public byte asBoolean;

            [FieldOffset(8)]
            public byte asByte;

            [FieldOffset(8)]
            public long asInteger;

            [FieldOffset(8)]
            public double asFloating;

            [FieldOffset(8)]
            public IntPtr asDense;
        }
    }

    /// <summary>
    /// Contains members and methods that are necessary in order to define native Risa functions.
    /// </summary>
    public static class Native
    {
        /// <summary>
        /// Represents a Risa native function.
        /// </summary>
        /// 
        /// <param name="vm">The Risa virtual machine that the function was invoked by.</param>
        /// <param name="args">The arguments that the function was invoked with.</param>
        /// 
        /// <returns>A value that will be returned to the virtual machine.</returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Value Function(VM vm, Args args);

        /// <summary>
        /// Creates a native function that can be directly used by the C99 implementation.
        /// </summary>
        /// 
        /// <param name="vm">The virtual machine for which to create the C99 delegate.</param>
        /// <param name="function">The native function.</param>
        public static C99.RisaNativeFunction CreateC99Delegate(VM vm, Function function)
        {
            C99.RisaNativeFunction fn = new C99.RisaNativeFunction((c99Vm, c99Argc, c99Args) =>
            {
                return function(vm, new Args(c99Argc, c99Args)).data;
            });

            vm.c99Delegates.Add(fn);
            vm.natives.Add(function);

            return fn;
        }
    }

    /// <summary>
    /// Represents an exception occurred during compile time.
    /// </summary>
    public class CompileTimeException : Exception
    {
        public CompileTimeException() { }
    }

    /// <summary>
    /// Represents an exception occurred during runtime.
    /// </summary>
    public class RuntimeException : Exception
    {
        public RuntimeException() { }
    }

    /// <summary>
    /// Represents an exception occurred in the C99 implementation.
    /// </summary>
    public class APIException : Exception
    {
        public APIException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Represents an exception occurred when casting a value of a type to another type.
    /// </summary>
    public class ValueCastException : Exception
    {
        public ValueCastException(string expected, string actual) : base("Cannot cast '" + expected + "' to '" + actual + "'") { }
    }

    /// <summary>
    /// Represents an exception occurred when a modification is attempted on an object that is not bound to a virtual machine.
    /// </summary>
    public class ObjectNotBoundException : Exception
    {
        public ObjectNotBoundException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Represents a list of arguments passed to a Risa native function.
    /// </summary>
    public class Args
    {
        public byte argc;
        public IntPtr args;

        /// <summary>
        /// Initializes a new instance of the Args class.
        /// </summary>
        /// 
        /// <param name="c99Argc">The number of arguments.</param>
        /// <param name="c99Args">The C99 pointer to the first argument.</param>
        public Args(byte c99Argc, IntPtr c99Args)
        {
            args = c99Args;
            argc = c99Argc;
        }

        /// <summary>
        /// Returns the number of arguments.
        /// </summary>
        public byte Count()
        {
            return argc;
        }

        /// <summary>
        /// Returns the argument at a specific index.
        /// </summary>
        /// 
        /// <param name="index">The argument index.</param>
        public Value Get(byte index)
        {
            if(index >= argc)
                throw new IndexOutOfRangeException("Invalid index (expected max " + (argc - 1) + ", got " + index + ")");

            Value arg = new Value(C99.RisaDenseNativeGetArg(args, index));
            GC.KeepAlive(this);

            return arg;
        }

        /// <summary>
        /// Returns the virtual machine stack base for invoking other functions.
        /// </summary>
        /// <returns></returns>
        public IntPtr Base()
        {
            IntPtr result = C99.RisaDenseNativeGetBase(args, argc);
            GC.KeepAlive(this);

            return result;
        }
    }

    /// <summary>
    /// Represents a Risa object entry.
    /// </summary>
    public class ObjectEntry
    {
        public IntPtr keyPtr;
        public Value value;

        /// <summary>
        /// Initializes a new instance of the ObjectEntry class.
        /// </summary>
        /// 
        /// <param name="c99KeyPtr">The C99 pointer to the entry key.</param>
        /// <param name="c99Value">The C99 value of the entry.</param>
        public ObjectEntry(IntPtr c99KeyPtr, C99.RisaValue c99Value)
        {
            keyPtr = c99KeyPtr;
            value = new Value(c99Value);
        }

        /// <summary>
        /// Returns the entry key.
        /// </summary>
        public string GetKey()
        {
            string key = Marshal.PtrToStringAnsi(C99.RisaDenseStringAsCString(keyPtr));
            GC.KeepAlive(this);

            return key;
        }

        /// <summary>
        /// Returns the entry value.
        /// </summary>
        public Value GetValue()
        {
            return value;
        }
    }

    /// <summary>
    /// Represents a Risa array.
    /// </summary>
    public class ValueArray
    {
        public IntPtr ptr;

        /// <summary>
        /// Initializes a new instance of the ValueArray class.
        /// </summary>
        /// 
        /// <param name="c99Ptr">The C99 pointer to the array.</param>
        public ValueArray(IntPtr c99Ptr)
        {
            ptr = c99Ptr;
        }

        /// <summary>
        /// Initializes a new instance of the ValueArray class.
        /// </summary>
        /// 
        /// <param name="vm">The virtual machine under which to create the array.</param>
        public ValueArray(VM vm)
        {
            ptr = C99.RisaDenseArrayCreate();
            C99.RisaVMRegisterDense(vm.ptr, ptr);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Returns the length of the array.
        /// </summary>
        public uint Count()
        {
            uint count = C99.RisaDenseArrayGetCount(ptr);
            GC.KeepAlive(this);

            return count;
        }

        /// <summary>
        /// Gets a value at a specific index.
        /// </summary>
        /// 
        /// <param name="index">The index.</param>
        public Value Get(uint index)
        {
            uint count = Count();

            if (index >= count)
                throw new IndexOutOfRangeException("Invalid index (expected max " + (count - 1) + ", got " + index + ")");

            Value val = new Value(C99.RisaDenseArrayGet(ptr, index));
            GC.KeepAlive(this);

            return val;
        }

        /// <summary>
        /// Sets the value at an index to another value.
        /// </summary>
        /// 
        /// <param name="index">The index where to place the new value. If it's equal to the length, a new value is added.</param>
        /// <param name="value">The value.</param>
        public void Set(uint index, Value value)
        {
            uint count = Count();

            if (index > count)
                throw new IndexOutOfRangeException("Invalid index (expected max " + count + ", got " + index + ")");

            C99.RisaDenseArraySet(ptr, index, value.data);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Adds a value to the array.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public void Add(Value value)
        {
            Set(Count(), value);
        }

        /// <summary>
        /// Casts the array to a Risa value.
        /// </summary>
        public Value ToValue()
        {
            return new Value(this);
        }
    }

    /// <summary>
    /// Represents a Risa object.
    /// </summary>
    public class ValueObject
    {
        public IntPtr ptr;
        public VM vm;

        /// <summary>
        /// Initializes a new instance of the ValueObject class.
        /// </summary>
        /// 
        /// <param name="c99Ptr">The C99 pointer to the object.</param>
        public ValueObject(IntPtr c99Ptr)
        {
            ptr = c99Ptr;
            vm = null;
        }

        /// <summary>
        /// Initializes a new instance of the ValueObject class.
        /// </summary>
        /// 
        /// <param name="vm">The virtual machine under which to create the object.</param>
        public ValueObject(VM vm)
        {
            ptr = C99.RisaDenseObjectCreateUnder(vm.ptr, 0);
            this.vm = vm;
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Binds this object to a virtual machine, so that it can be modified.
        /// </summary>
        /// 
        /// <param name="vm">The virtual machine.</param>
        public void Bind(VM vm)
        {
            this.vm = vm;
        }

        /// <summary>
        /// Returns the number of entries in the object.
        /// </summary>
        public uint Count()
        {
            uint count = C99.RisaDenseObjectGetCount(ptr);
            GC.KeepAlive(this);

            return count;
        }

        /// <summary>
        /// Returns an entry at an index.
        /// </summary>
        /// 
        /// <param name="index">The index.</param>
        public ObjectEntry Get(uint index)
        {
            uint count = Count();

            if(index >= count)
                throw new IndexOutOfRangeException("Invalid index (expected max " + (count - 1) + ", got " + index + ")");

            IntPtr entry = C99.RisaDenseObjectGetEntry(ptr, index);

            ObjectEntry result = new ObjectEntry(C99.RisaMapEntryGetKey(entry), C99.RisaMapEntryGetValue(entry));
            GC.KeepAlive(this);

            return result;
        }

        /// <summary>
        /// Adds or replaces an entry. The object must be bound to a virtual machine.
        /// </summary>
        /// 
        /// <param name="key">The entry key.</param>
        /// <param name="value">The entry value.</param>
        public void Set(string key, Value value)
        {
            if(vm == null)
                throw new ObjectNotBoundException("The object was not bound to any VM; try calling .Bind(vm) before Set");

            IntPtr str = C99.RisaVMStringCreate(vm.ptr, key, (uint) key.Length);
            C99.RisaDenseObjectSet(ptr, str, value.data);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Casts the object to a Risa value.
        /// </summary>
        public Value ToValue()
        {
            return new Value(this);
        }
    }

    /// <summary>
    /// Represents a Risa function.
    /// </summary>
    public class ValueFunction
    {
        public IntPtr ptr;

        /// <summary>
        /// Initializes a new instance of the ValueFunction class.
        /// </summary>
        /// 
        /// <param name="c99Ptr">The C99 pointer to the function.</param>
        public ValueFunction(IntPtr c99Ptr)
        {
            ptr = c99Ptr;
        }

        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// 
        /// <param name="vm">The virtual machine to invoke the function under.</param>
        /// <param name="context">The context from which to deduce the stack base.</param>
        /// <param name="args">The arguments to invoke the function with.</param>
        public Value Invoke(VM vm, Args context, params Value[] args)
        {
            C99.RisaValue[] values = new C99.RisaValue[args.Length];

            for(ushort i = 0; i < args.Length; ++i)
                values[i] = args[i].data;

            GCHandle pinnedArgs = GCHandle.Alloc(values, GCHandleType.Pinned);
            IntPtr argsPtr = pinnedArgs.AddrOfPinnedObject();

            C99.RisaValue result = C99.RisaVMInvokeArgs(vm.ptr, context.Base(), ToValue().data, (byte) args.Length, argsPtr);

            pinnedArgs.Free();

            GC.KeepAlive(this);

            return new Value(result);
        }

        /// <summary>
        /// Casts the object to a Risa value.
        /// </summary>
        public Value ToValue()
        {
            return new Value(this);
        }
    }

    /// <summary>
    /// Represents a Risa value.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Represents a Risa value type.
        /// </summary>
        public enum Type
        {
            NULL,
            BOOL,
            BYTE,
            INT,
            FLOAT,
            STRING,
            ARRAY,
            OBJECT,
            FUNCTION
        }

        /// <summary>
        /// The Risa null value.
        /// </summary>
        public static Value NULL = new Value(C99.RisaValueFromNull());

        public C99.RisaValue data;

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="c99Data">The C99 value.</param>
        public Value(C99.RisaValue c99Data)
        {
            data = c99Data;
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="boolValue">The boolean value.</param>
        public Value(bool boolValue)
        {
            data = C99.RisaValueFromBool((byte) (boolValue ? 1 : 0));
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="byteValue">The byte value.</param>
        public Value(byte byteValue)
        {
            data = C99.RisaValueFromByte(byteValue);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="intValue">The int value.</param>
        public Value(long intValue)
        {
            data = C99.RisaValueFromInt(intValue);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="floatValue">The float value.</param>
        public Value(double floatValue)
        {
            data = C99.RisaValueFromFloat(floatValue);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="str">The string value.</param>
        /// <param name="vm">The virtual machine under which to create the value.</param>
        public Value(string str, VM vm)
        {
            data.type = C99.RisaValueType.RISA_VAL_DENSE;
            data.asDense = C99.RisaVMStringCreate(vm.ptr, str, (uint) str.Length);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="arr">The array value.</param>
        public Value(ValueArray arr)
        {
            data.type = C99.RisaValueType.RISA_VAL_DENSE;
            data.asDense = arr.ptr;
        }

        /// <summary>
        /// Initializes a new instance of the Value class.
        /// </summary>
        /// 
        /// <param name="obj">The object value.</param>
        public Value(ValueObject obj)
        {
            data.type = C99.RisaValueType.RISA_VAL_DENSE;
            data.asDense = obj.ptr;
        }

        /// <summary>
        /// Initializes a new instance of the Value object.
        /// </summary>
        /// 
        /// <param name="nativeValue">The native value.</param>
        /// <param name="vm">The virtual machine under which to create the value.</param>
        public Value(Native.Function nativeValue, VM vm)
        {
            data = C99.RisaDenseNativeValue(Native.CreateC99Delegate(vm, nativeValue));
            C99.RisaVMRegisterDense(vm.ptr, data.asDense); // Register it so that the VM destructor can free it.
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Initializes a new instance of the Value object.
        /// </summary>
        /// 
        /// <param name="nativeValue">The function value.</param>
        /// <param name="vm">The virtual machine under which to create the value.</param>
        public Value(ValueFunction functionValue)
        {
            data.type = C99.RisaValueType.RISA_VAL_DENSE;
            data.asDense = functionValue.ptr;
        }

        /// <summary>
        /// Returns the value type.
        /// </summary>
        public new Type GetType()
        {
            switch(data.type)
            {
                case C99.RisaValueType.RISA_VAL_NULL:
                    return Type.NULL;
                case C99.RisaValueType.RISA_VAL_BOOL:
                    return Type.BOOL;
                case C99.RisaValueType.RISA_VAL_BYTE:
                    return Type.BYTE;
                case C99.RisaValueType.RISA_VAL_INT:
                    return Type.INT;
                case C99.RisaValueType.RISA_VAL_FLOAT:
                    return Type.FLOAT;
                case C99.RisaValueType.RISA_VAL_DENSE:
                {
                    switch(C99.RisaDenseGetType(data.asDense))
                    {
                        case C99.RisaDenseValueType.RISA_DVAL_STRING:
                            return Type.STRING;
                        case C99.RisaDenseValueType.RISA_DVAL_ARRAY:
                            return Type.ARRAY;
                        case C99.RisaDenseValueType.RISA_DVAL_OBJECT:
                            return Type.OBJECT;
                        case C99.RisaDenseValueType.RISA_DVAL_FUNCTION:
                        case C99.RisaDenseValueType.RISA_DVAL_CLOSURE:
                        case C99.RisaDenseValueType.RISA_DVAL_NATIVE:
                            return Type.FUNCTION;
                        default:
                            throw new APIException("Invalid value type");
                    }
                }
                default:
                    throw new APIException("Invalid value type");
            }
        }

        /// <summary>
        /// Returns whether or not the value is of type bool.
        /// </summary>
        public bool IsBool() => GetType() == Type.BOOL;

        /// <summary>
        /// Returns whether or not the value is of type byte.
        /// </summary>
        public bool IsByte() => GetType() == Type.BYTE;

        /// <summary>
        /// Returns whether or not the value is of type int.
        /// </summary>
        public bool IsInt() => GetType() == Type.INT;

        /// <summary>
        /// Returns whether or not the value is of type float.
        /// </summary>
        public bool IsFloat() => GetType() == Type.FLOAT;

        /// <summary>
        /// Returns whether or not the value is of type string.
        /// </summary>
        public bool IsString() => GetType() == Type.STRING;

        /// <summary>
        /// Returns whether or not the value is of type array.
        /// </summary>
        public bool IsArray() => GetType() == Type.ARRAY;

        /// <summary>
        /// Returns whether or not the value is of type object.
        /// </summary>
        public bool IsObject() => GetType() == Type.OBJECT;

        /// <summary>
        /// Returns whether or not the value is of type function.
        /// </summary>
        public bool IsFunction() => GetType() == Type.FUNCTION;

        /// <summary>
        /// Returns whether or not the valie is of type function and is a native function.
        /// </summary>
        public bool IsNative() => IsFunction() && C99.RisaDenseGetType(data.asDense) == C99.RisaDenseValueType.RISA_DVAL_NATIVE;

        /// <summary>
        /// Casts the value to bool.
        /// </summary>
        public bool AsBool()
        {
            if(!IsBool())
                throw new ValueCastException(GetType().ToString().ToLowerInvariant(), "bool");

            return data.asBoolean != 0;
        }

        /// <summary>
        /// Casts the value to byte.
        /// </summary>
        public byte AsByte()
        {
            if(!IsByte())
                throw new ValueCastException(GetType().ToString().ToLowerInvariant(), "byte");

            return data.asByte;
        }

        /// <summary>
        /// Casts the value to int.
        /// </summary>
        public long AsInt()
        {
            if(!IsInt())
                throw new ValueCastException(GetType().ToString().ToLowerInvariant(), "int");

            return data.asInteger;
        }

        /// <summary>
        /// Casts the value to float.
        /// </summary>
        public double AsFloat()
        {
            if(!IsFloat())
                throw new ValueCastException(GetType().ToString().ToLowerInvariant(), "float");

            return data.asFloating;
        }

        /// <summary>
        /// Casts the value to string.
        /// </summary>
        public string AsString()
        {
            if(!IsString())
                throw new ValueCastException(GetType().ToString().ToLowerInvariant(), "string");

            return Marshal.PtrToStringAnsi(C99.RisaDenseStringAsCString(data.asDense));
        }

        /// <summary>
        /// Casts the value to object.
        /// </summary>
        public ValueObject AsObject()
        {
            if(!IsObject())
                throw new ValueCastException(GetType().ToString(), "object");

            return new ValueObject(data.asDense);
        }

        /// <summary>
        /// Casts the value to array.
        /// </summary>
        public ValueArray AsArray()
        {
            if (!IsArray())
                throw new ValueCastException(GetType().ToString(), "array");

            return new ValueArray(data.asDense);
        }

        /// <summary>
        /// Casts the value to function.
        /// </summary>
        public ValueFunction AsFunction()
        {
            if (!IsFunction())
                throw new ValueCastException(GetType().ToString(), "function");

            return new ValueFunction(data.asDense);
        }

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        public override string ToString()
        {
            IntPtr c99Str = C99.RisaValueToString(data);

            string str = Marshal.PtrToStringAnsi(c99Str);
            
            C99.RisaMemFree(c99Str, IntPtr.Zero, 0);

            GC.KeepAlive(this);

            return str;
        }
    }

    /// <summary>
    /// Represents a compiled Risa function.
    /// </summary>
    public class CompiledFunction
    {
        public IntPtr ptr;

        /// <summary>
        /// Initializes a new instance of the CompiledFunction class.
        /// </summary>
        /// 
        /// <param name="c99Ptr">The C99 pointer to the function.</param>
        public CompiledFunction(IntPtr c99Ptr)
        {
            ptr = c99Ptr;
        }

        /// <summary>
        /// Returns the function cluster.
        /// </summary
        public Cluster GetCluster()
        {
            Cluster cl = new Cluster(C99.RisaDenseFunctionGetCluster(ptr));
            GC.KeepAlive(this);

            return cl;
        }
    }

    /// <summary>
    /// Represents a Risa cluster.
    /// </summary>
    public class Cluster
    {
        public IntPtr ptr;

        /// <summary>
        /// Initializes a new instance of the Cluster class.
        /// </summary>
        /// 
        /// <param name="c99Ptr">The C99 pointer to the cluster.</param>
        public Cluster(IntPtr c99Ptr)
        {
            ptr = c99Ptr;
        }

        /// <summary>
        /// Writes a byte to the cluster bytecode at an index.
        /// </summary>
        /// 
        /// <param name="data">The byte.</param>
        /// <param name="index">The index.</param>
        public void Write(byte data, uint index)
        {
            C99.RisaClusterWrite(ptr, data, index);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Writes a constant to the cluster.
        /// </summary>
        /// 
        /// <param name="constant">The cluster.</param>
        public void WriteConstant(C99.RisaValue constant)
        {
            C99.RisaClusterWriteConstant(ptr, constant);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Returns the size of the cluster bytecode.
        /// </summary>
        public uint GetSize()
        {
            uint size = C99.RisaClusterGetSize(ptr);
            GC.KeepAlive(this);

            return size;
        }

        /// <summary>
        /// Returns the byte in the cluster bytecode at an index.
        /// </summary>
        /// 
        /// <param name="index"></param>
        public byte GetDataAt(uint index)
        {
            byte data = C99.RisaClusterGetDataAt(ptr, index);
            GC.KeepAlive(this);

            return data;
        }

        /// <summary>
        /// Returns the number of constants in the cluster.
        /// </summary>
        public uint GetConstantCount()
        {
            uint count = C99.RisaClusterGetConstantCount(ptr);
            GC.KeepAlive(this);

            return count;
        }

        /// <summary>
        /// Returns a constant at an index.
        /// </summary>
        /// 
        /// <param name="index">The index.</param>
        public C99.RisaValue GetConstant(uint index)
        {
            C99.RisaValue cnst = C99.RisaClusterGetConstantAt(ptr, index);
            GC.KeepAlive(this);

            return cnst;
        }
    }

    /// <summary>
    /// Represents a Risa IO interface.
    /// </summary>
    public class IO
    {
        /// <summary>
        /// Represents a Risa input mode.
        /// </summary>
        public enum InputMode
        {
            /// <summary>
            /// Read only one character.
            /// </summary>
            CHAR = C99.RisaInputMode.RISA_INPUT_MODE_CHAR,
            /// <summary>
            /// Read until either the space character or a line ending is encountered.
            /// </summary>
            WORD = C99.RisaInputMode.RISA_INPUT_MODE_WORD,
            /// <summary>
            /// Read until the line ending is encountered.
            /// </summary>
            LINE = C99.RisaInputMode.RISA_INPUT_MODE_LINE
        }

        public IntPtr ptr;

        // The GC will invalidate the handlers unless there are managed references still around,
        // because the CLR doesn't know that the handlers are still internally referenced in the C99 implementation.
        private InHandler inHandler;
        private OutHandler outHandler;
        private OutHandler errHandler;

        /// <summary>
        /// Represents a Risa input handler.
        /// </summary>
        /// 
        /// <param name="mode">The input mode.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string InHandler(InputMode mode);

        /// <summary>
        /// Represents a Risa output handler.
        /// </summary>
        /// 
        /// <param name="data">The output data.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void OutHandler(string data);

        /// <summary>
        /// Initializes a new instance of the IO class.
        /// </summary>
        /// 
        /// <param name="c99Ptr">The C99 pointer to the IO interface.</param>
        public IO(IntPtr c99Ptr)
        {
            ptr = c99Ptr;
        }

        /// <summary>
        /// Destroys the IO class instance.
        /// </summary>
        ~IO()
        {
            GC.KeepAlive(inHandler);
            GC.KeepAlive(outHandler);
            GC.KeepAlive(errHandler);
        }
        
        /// <summary>
        /// Redirects the input stream.
        /// </summary>
        /// 
        /// <param name="handler">The input handler.</param>
        public void RedirectIn(InHandler handler)
        {
            inHandler = handler;
            C99.RisaIoRedirectIn(ptr, inHandler);
            C99.RisaIOSetFreeInput(ptr, false); // Don't free the strings that come from the custom input handler; those are freed by the CLR GC.
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Redirects the output stream.
        /// </summary>
        /// 
        /// <param name="handler">The output handler.</param>
        public void RedirectOut(OutHandler handler)
        {
            outHandler = handler;
            C99.RisaIoRedirectOut(ptr, outHandler);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Redirects the error stream.
        /// </summary>
        /// <param name="handler">The error handler.</param>
        public void RedirectErr(OutHandler handler)
        {
            errHandler = handler;
            C99.RisaIoRedirectErr(ptr, errHandler);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Clones the IO interface data to another IO interface.
        /// </summary>
        /// 
        /// <param name="dest">The destination.</param>
        public void CloneTo(IO dest)
        {
            C99.RisaIOClone(dest.ptr, ptr);
            dest.inHandler = inHandler;
            dest.outHandler = outHandler;
            dest.errHandler = errHandler;
            GC.KeepAlive(this);
        }
    }

    /// <summary>
    /// Represents a Risa compiled script.
    /// </summary>
    public class CompiledScript
    {
        public CompiledFunction function;
        public IntPtr strings;

        /// <summary>
        /// Initializes a new instance of the CompiledScript class.
        /// </summary>
        /// 
        /// <param name="function">The compiled function.</param>
        /// <param name="strings">The C99 pointer to the string map.</param>
        public CompiledScript(CompiledFunction function, IntPtr strings)
        {
            this.function = function;
            this.strings = strings;
        }
    }

    /// <summary>
    /// Represents a Risa compiler.
    /// </summary>
    public class Compiler
    {
        public IntPtr ptr;
        private IO io;

        /// <summary>
        /// Initializes a new instance of the Compiler class.
        /// </summary>
        public Compiler() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the Compiler class.
        /// </summary>
        /// 
        /// <param name="replMode">Whether or not to compile in REPL mode.</param>
        public Compiler(bool replMode)
        {
            ptr = C99.RisaCompilerCreate();
            C99.RisaCompilerSetReplMode(ptr, (byte) (replMode ? 1 : 0));
            io = new IO(C99.RisaCompilerGetIO(ptr));

            GC.KeepAlive(this);
        }

        /// <summary>
        /// Makes the compiler target a specific virtual machine.
        /// </summary>
        /// 
        /// <param name="vm">The virtual machine.</param>
        public void Target(VM vm)
        {
            C99.RisaCompilerTarget(ptr, vm.ptr);
            GC.KeepAlive(this);
        }

        /// <summary>
        /// Returns the IO interface.
        /// </summary>
        public IO GetIO()
        {
            return io;
        }

        ~Compiler()
        {
            C99.RisaCompilerShallowFree(ptr);
        }

        /// <summary>
        /// Compiles a string containing Risa source code.
        /// </summary>
        /// 
        /// <param name="source">The source code.</param>
        public CompiledScript Compile(string source)
        {
            if (C99.RisaCompilerCompile(ptr, source) == C99.RisaCompilerStatus.RISA_COMPILER_STATUS_ERROR)
            {
                throw new CompileTimeException();
            }

            CompiledScript compiled = new CompiledScript(new CompiledFunction(C99.RisaCompilerGetFunction(ptr)), C99.RisaCompilerGetStrings(ptr));
            GC.KeepAlive(this);

            return compiled;
        }
    }

    /// <summary>
    /// Represents a Risa virtual machine.
    /// </summary>
    public class VM
    {
        public IntPtr ptr;

        // The GC will invalidate the delegates unless there are managed references still around,
        // because the CLR doesn't know that the delegates are still internally referenced in the C99 implementation.
        public List<C99.RisaNativeFunction> c99Delegates;
        public List<Native.Function> natives;

        private IO io;

        /// <summary>
        /// Represents a standard library.
        /// </summary>
        public enum StandardLibrary
        {
            /// <summary>
            /// Contains core functionality, such as conversion functions.
            /// </summary>
            CORE,
            /// <summary>
            /// Contains IO functionality, such as print functions.
            /// </summary>
            IO,
            /// <summary>
            /// Contains string manipulation functions.
            /// </summary>
            STRING,
            /// <summary>
            /// Contains mathematical functions.
            /// </summary>
            MATH,
            /// <summary>
            /// Contains reflection-capable functions.
            /// </summary>
            REFLECT,
            /// <summary>
            /// Contains debugging functionality.
            /// </summary>
            DEBUG
        }

        /// <summary>
        /// Initializes a new instance of the VM class.
        /// </summary>
        public VM() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the VM class.
        /// </summary>
        /// 
        /// <param name="replMode">Whether or not to compile in REPL mode when loading source code.</param>
        public VM(bool replMode)
        {
            ptr = C99.RisaVMCreate();
            c99Delegates = new List<C99.RisaNativeFunction>();
            natives = new List<Native.Function>();

            C99.RisaVMSetReplMode(ptr, (byte)(replMode ? 1 : 0));

            io = new IO(C99.RisaVMGetIO(ptr));

            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Destroys the VM class instance.
        /// </summary>
        ~VM()
        {
            C99.RisaVMFree(ptr);
            System.GC.KeepAlive(c99Delegates);
            System.GC.KeepAlive(natives);
            System.GC.KeepAlive(io);
        }

        /// <summary>
        /// Loads a compiled script.
        /// </summary>
        /// 
        /// <param name="script">The script.</param>
        public void Load(CompiledScript script)
        {
            C99.RisaVMLoadFunction(ptr, script.function.ptr);
            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Loads a native function.
        /// </summary>
        /// 
        /// <param name="fn">The native function pointer.</param>
        public void Load(ValueFunction fn)
        {
            C99.RisaVMLoadFunction(ptr, fn.ptr);
            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Compiles and loads a string containing Risa source code.
        /// </summary>
        /// 
        /// <param name="source">The source code.</param>
        public void Load(string source)
        {
            Compiler compiler = new Compiler();
            compiler.Target(this);

            GetIO().CloneTo(compiler.GetIO());

            CompiledScript script = compiler.Compile(source);

            C99.RisaVMLoadCompilerData(ptr, compiler.ptr);

            Load(script);
            System.GC.KeepAlive(compiler);
            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Loads a Risa standard library.
        /// </summary>
        /// 
        /// <param name="library">The standard library.</param>
        public void LoadLibrary(StandardLibrary library)
        {
            switch(library)
            {
                case StandardLibrary.CORE:
                    C99.RisaSTDRegisterCore(ptr);
                    break;
                case StandardLibrary.IO:
                    C99.RisaSTDRegisterIO(ptr);
                    break;
                case StandardLibrary.STRING:
                    C99.RisaSTDRegisterString(ptr);
                    break;
                case StandardLibrary.MATH:
                    C99.RisaSTDRegisterMath(ptr);
                    break;
                case StandardLibrary.REFLECT:
                    C99.RisaSTDRegisterReflect(ptr);
                    break;
                case StandardLibrary.DEBUG:
                    C99.RisaSTDRegisterDebug(ptr);
                    break;
            }
        }

        /// <summary>
        /// Loads all available Risa standard libraries.
        /// </summary>
        public void LoadAllLibraries()
        {
            LoadLibrary(StandardLibrary.CORE);
            LoadLibrary(StandardLibrary.IO);
            LoadLibrary(StandardLibrary.STRING);
            LoadLibrary(StandardLibrary.MATH);
            LoadLibrary(StandardLibrary.REFLECT);
            LoadLibrary(StandardLibrary.DEBUG);
        }

        /// <summary>
        /// Loads a Risa value such that it can be accessed globally.
        /// </summary>
        /// 
        /// <param name="name">The name under which the value can be accessed.</param>
        /// <param name="value">The value.</param>
        public void LoadGlobal(string name, Value value)
        {
            C99.RisaVMGlobalSet(ptr, name, (uint) name.Length, value.data);
            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Loads a Risa native function such that it can be accessed globally.
        /// </summary>
        /// 
        /// <param name="name">The name under which the function can be accessed.</param>
        /// <param name="value">The function.</param>
        public void LoadGlobalNative(string name, Native.Function native)
        {
            C99.RisaVMGlobalSetNative(ptr, name, (uint) name.Length, Native.CreateC99Delegate(this, native));
            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Runs the virtual machine.
        /// </summary>
        /// 
        /// <param name="maxInstructions">The maximum number of instructions to run. 0 means run all.</param>
        /// 
        /// <returns>True, if the VM has executed all instructions. False, if there are more instructions to execute.</returns>
        public bool Run(uint maxInstructions = 0)
        {
            C99.RisaVMStatus status = C99.RisaVMRun(ptr, maxInstructions);
            System.GC.KeepAlive(this);

            if (status == C99.RisaVMStatus.RISA_VM_STATUS_ERROR)
            {
                throw new RuntimeException();
            }

            return status == C99.RisaVMStatus.RISA_VM_STATUS_OK;
        }

        /// <summary>
        /// Invokes a function.
        /// </summary>
        /// 
        /// <param name="context">The context from which to deduce the stack base.</param>
        /// <param name="callee">The function to invoke.</param>
        /// <param name="args">The arguments to invoke the function with.</param>
        public Value Invoke(Args context, ValueFunction callee, params Value[] args)
        {
            Value result = callee.Invoke(this, context, args);
            System.GC.KeepAlive(this);

            return result;
        }

        /// <summary>
        /// Runs the garbage collector, if necessary.
        /// </summary>
        public void GC()
        {
            C99.RisaGCCheck(ptr);
            System.GC.KeepAlive(this);
        }

        /// <summary>
        /// Returns the IO interface.
        /// </summary>
        public IO GetIO()
        {
            return io;
        }

        /// <summary>
        /// Gets the last result, stored in the virtual machine ACC register.
        /// </summary>
        public Value GetLastResult()
        {
            Value result = new Value(C99.RisaVMGetACC(ptr));
            System.GC.KeepAlive(this);

            return result;
        }

        /// <summary>
        /// Creates a new value of type bool.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public Value CreateBool(bool value) =>
            new Value(value);

        /// <summary>
        /// Creates a new value of type byte.
        /// </summary>
        /// <param name="value">The value.</param>
        public Value CreateByte(byte value) =>
            new Value(value);

        /// <summary>
        /// Creates a new value of type int.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public Value CreateInt(long value) =>
            new Value(value);

        /// <summary>
        /// Creates a new value of type float.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public Value CreateFloat(double value) =>
            new Value(value);

        /// <summary>
        /// Creates a new value of type string.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public Value CreateString(string value) =>
            new Value(value, this);

        /// <summary>
        /// Creates a new value of type array.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public ValueArray CreateArray() =>
            new ValueArray(this);

        /// <summary>
        /// Creates a new value of type object.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public ValueObject CreateObject() =>
            new ValueObject(this);

        /// <summary>
        /// Creates a new value of type native.
        /// </summary>
        /// 
        /// <param name="value">The value.</param>
        public Value CreateNative(Native.Function function) =>
            new Value(function, this);
    }
}

﻿using Sigil.Impl;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Sigil
{
    public partial class Emit<DelegateType>
    {
        /// <summary>
        /// Push a 1 onto the stack if b is true, and 0 if false.
        /// 
        /// Pushed values are int32s.
        /// </summary>
        public Emit<DelegateType> LoadConstant(bool b)
        {
            return LoadConstant(b ? 1 : 0);
        }

        /// <summary>
        /// Push a constant int32 onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(int i)
        {
            switch (i)
            {
                case -1: UpdateState(OpCodes.Ldc_I4_M1, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 0: UpdateState(OpCodes.Ldc_I4_0, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 1: UpdateState(OpCodes.Ldc_I4_1, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 2: UpdateState(OpCodes.Ldc_I4_2, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 3: UpdateState(OpCodes.Ldc_I4_3, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 4: UpdateState(OpCodes.Ldc_I4_4, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 5: UpdateState(OpCodes.Ldc_I4_5, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 6: UpdateState(OpCodes.Ldc_I4_6, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 7: UpdateState(OpCodes.Ldc_I4_7, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 8: UpdateState(OpCodes.Ldc_I4_8, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
            }

            if (i >= sbyte.MinValue && i <= sbyte.MaxValue)
            {
                byte asByte;
                unchecked
                {
                    asByte = (byte)i;
                }

                UpdateState(OpCodes.Ldc_I4_S, asByte, StackTransition.Push<int>().Wrap("LoadConstant"));
                return this;
            }

            UpdateState(OpCodes.Ldc_I4, i, StackTransition.Push<int>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant int32 onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(uint i)
        {
            switch (i)
            {
                case uint.MaxValue: UpdateState(OpCodes.Ldc_I4_M1, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 0: UpdateState(OpCodes.Ldc_I4_0, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 1: UpdateState(OpCodes.Ldc_I4_1, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 2: UpdateState(OpCodes.Ldc_I4_2, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 3: UpdateState(OpCodes.Ldc_I4_3, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 4: UpdateState(OpCodes.Ldc_I4_4, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 5: UpdateState(OpCodes.Ldc_I4_5, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 6: UpdateState(OpCodes.Ldc_I4_6, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 7: UpdateState(OpCodes.Ldc_I4_7, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
                case 8: UpdateState(OpCodes.Ldc_I4_8, StackTransition.Push<int>().Wrap("LoadConstant")); return this;
            }

            if (i <= sbyte.MaxValue)
            {
                byte asByte;
                unchecked
                {
                    asByte = (byte)i;
                }

                UpdateState(OpCodes.Ldc_I4_S, asByte, StackTransition.Push<int>().Wrap("LoadConstant"));
                return this;
            }

            UpdateState(OpCodes.Ldc_I4, i, StackTransition.Push<int>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant int64 onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(long l)
        {
            UpdateState(OpCodes.Ldc_I8, l, StackTransition.Push<long>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant int64 onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(ulong l)
        {
            UpdateState(OpCodes.Ldc_I8, l, StackTransition.Push<long>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant float onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(float f)
        {
            UpdateState(OpCodes.Ldc_R4, f, StackTransition.Push<float>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant double onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(double d)
        {
            UpdateState(OpCodes.Ldc_R8, d, StackTransition.Push<double>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant string onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(string str)
        {
            UpdateState(OpCodes.Ldstr, str, StackTransition.Push<string>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant RuntimeFieldHandle onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }

            UpdateState(OpCodes.Ldtoken, field, StackTransition.Push<RuntimeFieldHandle>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant RuntimeMethodHandle onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            UpdateState(OpCodes.Ldtoken, method, StackTransition.Push<RuntimeMethodHandle>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Push a constant RuntimeTypeHandle onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant<Type>()
        {
            return LoadConstant(typeof(Type));
        }

        /// <summary>
        /// Push a constant RuntimeTypeHandle onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadConstant(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            UpdateState(OpCodes.Ldtoken, type, StackTransition.Push<RuntimeTypeHandle>().Wrap("LoadConstant"));

            return this;
        }

        /// <summary>
        /// Loads a null reference onto the stack.
        /// </summary>
        public Emit<DelegateType> LoadNull()
        {
            UpdateState(OpCodes.Ldnull, StackTransition.Push<NullType>().Wrap("LoadNull"));

            return this;
        }
    }
}

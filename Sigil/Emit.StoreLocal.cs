﻿using Sigil.Impl;
using System;
using System.Reflection.Emit;

namespace Sigil
{
    public partial class Emit<DelegateType>
    {
        /// <summary>
        /// Pops a value off the stack and stores it into the given local.
        /// 
        /// To create a local, use DeclareLocal().
        /// </summary>
        public Emit<DelegateType> StoreLocal(Local local)
        {
            if (local == null)
            {
                throw new ArgumentNullException("local");
            }

            if (((IOwned)local).Owner != this)
            {
                FailOwnership(local);
            }

            UnusedLocals.Remove(local);

            switch (local.Index)
            {
                case 0: UpdateState(OpCodes.Stloc_0, StackTransition.Pop(local.StackType).Wrap("StoreLocal")); return this;
                case 1: UpdateState(OpCodes.Stloc_1, StackTransition.Pop(local.StackType).Wrap("StoreLocal")); return this;
                case 2: UpdateState(OpCodes.Stloc_2, StackTransition.Pop(local.StackType).Wrap("StoreLocal")); return this;
                case 3: UpdateState(OpCodes.Stloc_3, StackTransition.Pop(local.StackType).Wrap("StoreLocal")); return this;
            }

            if (local.Index >= byte.MinValue && local.Index <= byte.MaxValue)
            {
                byte asByte;
                unchecked
                {
                    asByte = (byte)local.Index;
                }

                UpdateState(OpCodes.Stloc_S, asByte, StackTransition.Pop(local.StackType).Wrap("StoreLocal"));
                return this;
            }

            UpdateState(OpCodes.Stloc, local, StackTransition.Pop(local.StackType).Wrap("StoreLocal"));

            return this;
        }

        /// <summary>
        /// Pops a value off the stack and stores it in the local with the given name.
        /// </summary>
        public Emit<DelegateType> StoreLocal(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            return StoreLocal(Locals[name]);
        } 
    }
}

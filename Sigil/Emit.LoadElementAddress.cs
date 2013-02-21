﻿using Sigil.Impl;
using System;
using System.Reflection.Emit;

namespace Sigil
{
    public partial class Emit<DelegateType>
    {
        private void InjectReadOnly()
        {
            while(ReadonlyPatches.Count > 0)
            {
                var elem = ReadonlyPatches[0];
                ReadonlyPatches.RemoveAt(0);

                var at = elem.Item1;
                var value = elem.Item2;

                var asObjToLdfd = value.CountMarks(OpCodes.Ldfld, 0, false);
                var asObjToLdfda = value.CountMarks(OpCodes.Ldflda, 0, false);
                var asObjToStfd = value.CountMarks(OpCodes.Stfld, 0, false);

                var asObjToCall = value.CountMarks(OpCodes.Call, 0, true);

                var asPtrToLdobj = value.CountMarks(OpCodes.Ldobj, 0, false);
                var asPtrToLdind =
                    value.CountMarks(OpCodes.Ldind_I, 0, false) +
                    value.CountMarks(OpCodes.Ldind_I1, 0, false) +
                    value.CountMarks(OpCodes.Ldind_I2, 0, false) +
                    value.CountMarks(OpCodes.Ldind_I4, 0, false) +
                    value.CountMarks(OpCodes.Ldind_I8, 0, false) +
                    value.CountMarks(OpCodes.Ldind_R4, 0, false) +
                    value.CountMarks(OpCodes.Ldind_R8, 0, false) +
                    value.CountMarks(OpCodes.Ldind_Ref, 0, false) +
                    value.CountMarks(OpCodes.Ldind_U1, 0, false) +
                    value.CountMarks(OpCodes.Ldind_U2, 0, false) +
                    value.CountMarks(OpCodes.Ldind_U4, 0, false);

                var asSourceToCpobj = value.CountMarks(OpCodes.Cpobj, 1, false);

                var totalAllowedUses =
                    asObjToLdfd +
                    asObjToLdfda +
                    asObjToStfd +
                    asPtrToLdobj +
                    asPtrToLdind +
                    asSourceToCpobj +
                    asObjToCall;

                var totalActualUses = value.CountMarks();

                if (totalActualUses == totalAllowedUses)
                {
                    InsertInstruction(at, OpCodes.Readonly);
                }
            }
        }

        /// <summary>
        /// Expects a reference to an array of the given element type and an index on the stack.
        /// 
        /// Pops both, and pushes the address of the element at the given index.
        /// </summary>
        public Emit<DelegateType> LoadElementAddress<ElementType>()
        {
            return LoadElementAddress(typeof(ElementType));
        }

        /// <summary>
        /// Expects a reference to an array of the given element type and an index on the stack.
        /// 
        /// Pops both, and pushes the address of the element at the given index.
        /// </summary>
        public Emit<DelegateType> LoadElementAddress(Type elementType)
        {
            // TODO: Another may-not-be-able to infer case, deal with it

            if (!AllowsUnverifiableCIL)
            {
                FailUnverifiable();
            }

            var arrayType = elementType.MakeArrayType();

            // needs to be markable so we can keep track of what makes use of this value
            var pushToStack = TypeOnStack.Get(elementType.MakeByRefType(), makeMarkable: true);

            // Shove this away, later on we'll figure out if we can insert a readonly here
            ReadonlyPatches.Add(Tuple.Create(IL.Index, pushToStack));

            var transitions =
                new[] 
                {
                    new StackTransition(new [] { typeof(NativeIntType), arrayType }, new [] { elementType.MakeByRefType() }),
                    new StackTransition(new [] { typeof(int), arrayType }, new [] { elementType.MakeByRefType() })
                };

            UpdateState(OpCodes.Ldelema, elementType, transitions.Wrap("LoadElementAddress"));

            return this;
        }
    }
}

//
// C# version of the code presented in the reference below.
// Converion by: Bob Elward Feb 2021
//
/*
 * Copyright (c) 2005 Brian Goetz and Tim Peierls
 * Released under the Creative Commons Attribution License
 *   (http://creativecommons.org/licenses/by/2.5)
 * Official home: http://www.jcip.net
 *
 * Any republication or derived work distributed in source code form
 * must include this copyright and license notice.
 */
//
using System;

namespace JcipAnnotations
{
    //
    // The field, method or property to which this annotation is
    // applied can only be accessed when holding a particular lock.
    //
    // The argument determines which lock guards the annotated field or method:
    //
    //   this :                  The intrinsic lock of the object in whose class the field is defined.
    //   class-name.this :       For inner classes, it may be necessary to disambiguate 'this'
    //                           the class-name.this designation allows you to specify which 'this' reference is intended
    //   itself :                For reference fields only; the object to which the field refers.
    //   field-name :            The lock object is referenced by the (instance or static) field
    //                           specified by field-name.
    //   class-name.field-name : The lock object is reference by the static field specified
    //                           by class-name.field-name.
    //   method-name() :         The lock object is returned by calling the named nil-ary method.
    //   class-name.class :      The Class object for the specified class should be used as the lock object.
    //
    [AttributeUsage(AttributeTargets.Field 
                  | AttributeTargets.Method 
                  | AttributeTargets.Property
                  , AllowMultiple = true)]

    public class GuardedBy : Attribute
    {
        private string lockId;

        public GuardedBy(string lockId)
        {
            this.lockId = lockId;

        }
    }
}
